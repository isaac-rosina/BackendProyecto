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
            document.getElementById("CategoriaIDeditar").innerHTML = opciones;

        })
        .catch((error) => console.error("Error categorias:", error));
}

obtenerCategoria();