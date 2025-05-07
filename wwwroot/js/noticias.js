window.addEventListener("load", function() {
    if (!window.noticiasJson || window.noticiasJson.length === 0) {
        console.error("⚠️ No hay noticias para cargar.");
        return;
    }
    
    ajustarNoticias(window.noticiasJson);
});


function ajustarNoticias(noticiasJson) {
    let ancho = window.innerWidth;
    let columnas = 3;

    if (ancho <= 1024) columnas = 2; // Tablet (768px - 1024px)
    if (ancho <= 768) columnas = 1; // Móvil (≤ 768px)

    let noticias = JSON.parse(noticiasJson);
    let carouselContainer = document.getElementById("carouselContainer");
    carouselContainer.innerHTML = ""; // Limpiar contenido

    for (let i = 0; i < noticias.length; i += columnas) {
        let item = document.createElement("div");
        item.className = "carousel-item" + (i === 0 ? " active" : ""); // Primer slide activo

        let row = document.createElement("div");
        row.className = "row";

        for (let j = i; j < i + columnas && j < noticias.length; j++) {
            let col = document.createElement("div");
            col.className = "col-lg-4 col-md-6 col-sm-12";

            col.innerHTML = `
                <div class="news-card">
                    <a href="${noticias[j].Enlace}">
                        <img src="${noticias[j].ImagenUrl}" alt="${noticias[j].Titulo}" class="img-fluid news-img">
                    </a>
                    <h5>${noticias[j].Titulo}</h5>
                    <p class="news-text">${noticias[j].Descripcion}</p>
                </div>
            `;
            row.appendChild(col);
        }

        item.appendChild(row);
        carouselContainer.appendChild(item);
    }
}

window.addEventListener("resize", () => ajustarNoticias(window.noticiasJson));
window.addEventListener("load", () => ajustarNoticias(window.noticiasJson));
