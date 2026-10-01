let detallesPedido = []; // donde guardamos los detalles del pedido

// funcion donde obtenemos los productos
function obtenerProductos() {
    fetch("http://localhost:5030/api/Producto")
        .then((respuesta) => respuesta.json())
        .then((data) => {
            let productoSeleccionado = document.getElementById("producto");

            productoSeleccionado.innerHTML =
                '<option value="">[SELECCIONE...]</option>';

            data.forEach((producto) => {
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
            <td><button type="button" class="btn btn-danger" onclick="EliminarDetalle(${index})">Eliminar</button></td>
        </tr>
        `;
    });
}


function EliminarDetalle(index) {
    detallesPedido.splice(index, 1);
    MostrarDetalles();
}

obtenerProductos();