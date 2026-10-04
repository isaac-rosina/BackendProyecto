function obtenerCategoria() {
    fetch("http://localhost:5030/api/Producto/idCategorias")
        .then((res) => res.json())
        .then((data) => {
            console.log("Categorias:", data);

            const opciones = `<option value="">[SELECCIONE...]</option>
                            ${data.map((categoria) => `
                                <option value="${categoria.id}">
                                ${categoria.nombre}
                                </option>
                                `,).join("")}            
                            `;

            document.getElementById("CategoriaID").innerHTML = opciones;
        })
        .catch((error) => console.error("Error categorias:", error));
}


function obtenerProducto() {
    fetch("http://localhost:5030/api/Producto")
        .then((respuesta) => respuesta.json())
        .then((data) => {
            console.log(data);
            mostrarProducto(data);
        })
        .catch((error) => console.error(error));
}


function mostrarProducto(data) {
    const tbody = document.getElementById("tablaProducto");
    if (!tbody) return;
    tbody.innerHTML = "";

    data.forEach((element) => {
        console.log("Elemento:", element);
        console.log("ID:", element.productoID)

        let tr = tbody.insertRow();
        tr.insertCell(0).innerHTML = element.nombreProducto;
        tr.insertCell(1).innerHTML = element.costoProducto;
        tr.insertCell(2).innerHTML = element.ventaProducto;
        tr.insertCell(3).innerHTML = element.stockProducto;
        tr.insertCell(4).innerHTML = element.descripcionProducto;

        //Boton de eliminar
        let eliminar = document.createElement("button");
        eliminar.textContent = "Eliminar";
        eliminar.classList.add("btn", "btn-danger", "fw-bold");

        eliminar.setAttribute(
            "onclick", `validacionEliminarProducto(${element.productoID})`,
        );

        let tdEliminar = tr.insertCell(5);
        tdEliminar.appendChild(eliminar);

        //Boton de editar
        let editar = document.createElement("button");
        editar.textContent = "Editar";
        editar.classList.add("btn", "btn-primary", "fw-bold");

        editar.setAttribute(
            "onclick", `buscarValoresProducto(${element.productoID})`,
        );

        let tdEditar = tr.insertCell(6);
        tdEditar.appendChild(editar);
    });
}


function agregarProducto() {
    var nuevoProducto = {
        nombres: document.getElementById("nombreProducto").value,
        costo: parseFloat(document.getElementById("costoProducto").value),
        venta: parseFloat(document.getElementById("ventaProducto").value),
        stock: parseInt(document.getElementById("stockProducto").value),
        descripcion: document.getElementById("descripcionProducto").value,
        categoriaId: parseInt(document.getElementById("CategoriaID").value),
    };
    console.log("Producto enviado:", nuevoProducto);

    fetch("http://localhost:5030/api/Producto", {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify(nuevoProducto)
    })
        .then((res) => res.json())
        .then(() => {
            document.getElementById("nombreProducto").value = "";
            document.getElementById("costoProducto").value = "";
            document.getElementById("ventaProducto").value = "";
            document.getElementById("stockProducto").value = "";
            document.getElementById("descripcionProducto").value = "";
            obtenerProducto();
        });
}


function buscarValoresProducto(id) {
    console.log("ID recibido", id);
    fetch(`http://localhost:5030/api/Producto/${id}`)
        .then((res) => {
            if (!res.ok) {
                throw new Error(`Error HTTP: ${res.status}`)
            }
            return res.json();
        })
        .then((data) => {
            console.log("Producto:", data);

            document.getElementById("idEditarProducto").value = data.productoId;
            document.getElementById("CategoriaIDeditar").value = data.categoriaID;
            document.getElementById("nombreEditarProducto").value = data.nombres;
            document.getElementById("costoEditarProducto").value = data.costo;
            document.getElementById("precioEditarProducto").value = data.venta;
            document.getElementById("stockEditarProducto").value = data.stock;

            let modal = new bootstrap.Modal(
                document.getElementById("editarProducto"),
            );

            modal.show();
        })
        .catch((error) => {
            console.error("No se pudo acceder a la API:", error);
        });
}


function editarProducto() {
    let id = document.getElementById("idEditarProducto").value;
    console.log("ID recibido editar:", id);

    let editarProducto = {
        productoID: id,
        categoriaId: document.getElementById("CategoriaIDeditar").value,
        nombres: document.getElementById("nombreEditarProducto").value,
        costo: document.getElementById("costoEditarProducto").value,
        venta: document.getElementById("precioEditarProducto").value,
        stock: document.getElementById("stockEditarProducto").value,
    }

    fetch(`http://localhost:5030/api/Producto/${id}`, {
        method: "PUT",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify(editarProducto),
    })
        .then(() => {
             console.log("ID recibido editarfinal:", id);
            document.getElementById("idEditarProducto").value = 0;
            document.getElementById("CategoriaIDeditar").value = 0;
            document.getElementById("nombreEditarProducto").value = "";
            document.getElementById("costoEditarProducto").value = "";
            document.getElementById("precioEditarProducto").value = "";
            document.getElementById("stockEditarProducto").value = "";

            let modal = bootstrap.Modal.getInstance(
                document.getElementById("editarProducto"),
            )

            modal.hide();
            obtenerProducto();
        })
        .catch((error) => console.error("No se pudo editar la categoría.", error))
}


function validacionEliminarProducto(id) {
    var siEliminar = confirm("¿Estas seguro de eliminar este producto?")
    if (siEliminar == true) {
        eliminarProducto(id);
    }
}
function eliminarProducto(id) {
    fetch(`http://localhost:5030/api/Producto/${id}`, {
        method: "DELETE",
    })
        .then(() => {
            obtenerProducto();
        })
        .catch((error) => console.error("No se pudo acceder la API.", error));
}

obtenerCategoria();
obtenerProducto();