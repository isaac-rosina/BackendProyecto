let detallesPedido = []; // donde guardamos los detalles del pedido

// funcion donde obtenemos los productos
function obtenerProductos() {
    fetch("http://localhost:5030/api/Producto")
        .then((respuesta) => respuesta.json())
        .then((data) => {
            let productoSeleccionado = document.getElementById("producto");

            productoSeleccionado.innerHTML =
                '<option value="">[SELECCIONE...]</option>';

            data.productos.forEach((producto) => {
                productoSeleccionado.innerHTML += `
                    <option 
                        value="${producto.productoID}"data-precio="${producto.ventaProducto}">
                        ${producto.nombreProducto}
                    </option>
                `;
            });
        })
        .catch((error) => {
            console.error("Error al obtener productos:", error);
        });
}


function obtenerPedido() {
    fetch("http://localhost:5030/api/Pedido")
        .then((respuesta) => respuesta.json())
        .then((data) => {
            console.log(data);
            mostrarPedido(data);
        })
        .catch((error) => console.error(error));
}


function cargarPrecio() {
    let producto = document.getElementById("producto");

    let precioInput = document.getElementById("precioUnitario");

    let opcionSeleccionada = producto.options[producto.selectedIndex];

    if (producto.value == "") {
        precioInput.value = "";
        return;
    }

    let precio = opcionSeleccionada.dataset.precio;

    precioInput.value = precio;
}


function CrearPedido() {
    let pedido = {
        nombre: document.getElementById("nombre").value,
        fecha: document.getElementById("fecha").value,
        estado: Number(document.getElementById("estado").value),
        total: Number(document.getElementById("total").value),

        detallePedidos: detallesPedido,
    }

    console.log("ENVIANDO PEDIDO:", pedido);

    fetch("http://localhost:5030/api/Pedido", {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify(pedido),
    })
        .then((res) => res.json())
        .then((data) => {
            console.log("Pedido creado:", data);
            alert("Pedido creado correctamente");

            detallesPedido = [];
            document.getElementById("fecha").value = "";
            document.getElementById("total").value = "";
            document.getElementById("nombre").value = "";

            MostrarDetalles();
            obtenerProductos();
        })
        .catch((error) => {
            console.error("Error al crear pedido:", error);
            alert("Error al conectar con el servidor.");
        });
}


function AgregarDetalle() {
    let producto = document.getElementById("producto");
    let cantidad = Number(document.getElementById("cantidad").value);
    let precio = Number(document.getElementById("precioUnitario").value);

    let detalle = {
        productoID: Number(producto.value),
        nombreProducto: producto.options[producto.selectedIndex].text, // lo muestra en pantalla
        cantidad: cantidad,
        precioUnitario: precio,
    };

    detallesPedido.push(detalle);
    MostrarDetalles();
}


function mostrarPedido(data) {
    document.getElementById("totalPedidos").textContent = data.totalPedidos;
    document.getElementById("totalCapital").textContent = data.capitalTotal;

    const tbody = document.getElementById("TablaPedido");
    tbody, innerHTML = "";

    data.pedido.forEach((element) => {
        console.log("Elemento:", element);
        console.log("ID:", element.pedidoID);

        let tr = tbody.insertRow();
        tr.insertCell(0).innerHTML = element.nombreDelCliente;
        tr.insertCell(1).innerHTML = element.fechaFormateada;
        tr.insertCell(2).innerHTML = element.estadoPedido;
        tr.insertCell(3).innerHTML = element.totalDelCliente;

        // Boton de mostrar detalles
        let mostrar = document.createElement("button");
        mostrar.textContent = "Ver Detalles";
        mostrar.classList.add("btn", "btn-warning", "fw-bold", "btn-sm");

        mostrar.setAttribute(
            "onclick", `abrirModal(${element.pedidoID})`,
        );

        let tdMostrar = tr.insertCell(4);
        tdMostrar.appendChild(mostrar);
    });
}


function MostrarDetalles() {
    let tbody = document.getElementById("tablaDetalles");
    if (!tbody) return;

    tbody.innerHTML = "";
    let totalAcumulado = 0;

    detallesPedido.forEach((detalle, index) => {
        let subtotal = detalle.cantidad * detalle.precioUnitario;
        totalAcumulado += subtotal;

        tbody.innerHTML += `
        <tr>
            <td>${detalle.nombreProducto}</td>
            <td>${detalle.cantidad}</td>
            <td>$${detalle.precioUnitario.toFixed(2)}</td>
            <td>$${subtotal.toFixed(2)}</td>
            <td><button type="button" class="btn btn-danger fw-bold" onclick="EliminarDetalle(${index})">Eliminar</button></td>
        </tr>
        `;
    });
    let inputTotal = document.getElementById("total");
    if (inputTotal) {
        inputTotal.innerHTML = totalAcumulado.toFixed(2);
    }
}

function abrirModal(id) {
    const tbody = document.getElementById("tablaDetallesModal");
    if (!tbody) return;
    tbody.innerHTML = "";

    fetch(`http://localhost:5030/api/Pedido/${id}`)
        .then((res) => {
            if (!res.ok) {
                throw new Error(`Error HTTP: ${res.status}`)
            }
            return res.json();
        })
        .then((data) => {
            console.log("Producto:", data);

            data.detallePedidos.forEach((element) => {
                console.log("Elemento:", element);
                console.log("ID:", element.productoID)

                let tr = tbody.insertRow();
                tr.insertCell(0).innerHTML = element.nombreProducto;
                tr.insertCell(1).innerHTML = element.cantidad;
                tr.insertCell(2).innerHTML = element.precioUnitario;
                tr.insertCell(3).innerHTML = element.precioUnitario * element.cantidad;
            });

            let modal = new bootstrap.Modal(
                document.getElementById("mostrarDetallesModal"),
            );

            modal.show();
        })
        .catch((error) => {
            console.error("No se pudo acceder a la API:", error);
        });
}


function EliminarDetalle(index) {
    detallesPedido.splice(index, 1);
    MostrarDetalles();
}

obtenerProductos();
obtenerPedido();