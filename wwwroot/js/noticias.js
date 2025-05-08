window.addEventListener("load", function() {
    if (!window.noticiasJson || window.noticiasJson.length === 0) {
        console.error("⚠️ No hay noticias para cargar.");
        return;
    }
    
    console.log("Noticias cargadas en JS:", window.noticiasJson); // Verifica estructura en consola
    ajustarNoticias();
});

window.addEventListener("resize", ajustarNoticias);

function ajustarNoticias() {
    let ancho = window.innerWidth;
    let columnas = 3;

    if (ancho < 1440 && ancho > 772) columnas = 2; // Tablet (772px - 1440px)
    if (ancho <= 772) columnas = 1; // Móvil (≤ 772px)

    let noticias = window.noticiasJson;
    console.log("Noticias antes de procesar:", noticias); // Depuración

    let carouselContainer = document.getElementById("carouselContainer");
    carouselContainer.innerHTML = ""; // Limpiar contenido

    for (let i = 0; i < noticias.length; i += columnas) {
        let item = document.createElement("div");
        item.className = "carousel-item" + (i === 0 ? " active" : ""); // Primer slide activo

        let row = document.createElement("div");
        row.className = "row";

        for (let j = i; j < i + columnas && j < noticias.length; j++) {
            let noticia = noticias[j];

            console.log("Procesando noticia:", noticia); // Depuración

            // Acceder a las propiedades con los nombres correctos
            if (!noticia || typeof noticia !== "object") {
                console.error("⚠️ Error en noticia (no es un objeto válido):", noticia);
                continue;
            }

            // Acceder con nombres en minúsculas
            if (!noticia.titulo || !noticia.descripcion || !noticia.imagenUrl) {
                console.error("⚠️ Error en noticia (faltan datos):", noticia);
                continue;
            }

            let col = document.createElement("div");
            col.className = "col-lg-4 col-md-6 col-sm-12";

            col.innerHTML = `
                <div class="news-card">
                    <a href="${noticia.enlace}">
                        <img src="${noticia.imagenUrl}" alt="${noticia.titulo}" class="img-fluid news-img">
                    </a>
                    <h5>${noticia.titulo}</h5>
                    <p class="news-text">${noticia.descripcion}</p>
                </div>
            `;
            row.appendChild(col);
        }

        item.appendChild(row);
        carouselContainer.appendChild(item);
    }
}
