window.addEventListener("load", function () {
    let noticias = window.noticiasJson;
    if (!noticias || noticias.length === 0) {
        console.error("No hay noticias para mostrar.");
        return;
    }

    let visibles = noticias.slice(0, 3); // 🔹 Inicialmente muestra los primeros 3
    let ocultas = noticias.slice(3); // 🔹 Resto de las noticias que no están en pantalla

    let carrusel = document.querySelector(".carousel");
    carrusel.innerHTML = ""; // Limpiar el contenido

    function renderCarousel() {
        carrusel.innerHTML = ""; // 🔹 Limpiar antes de actualizar
        visibles.forEach(noticia => {
            let item = document.createElement("div");
            item.className = "carousel-item";
            item.dataset.id = noticia.id;
            item.innerHTML = `
                <img src="${noticia.imagenUrl}" alt="${noticia.titulo}" class="news-img">
                <h5 class="news-title">${noticia.titulo}</h5>
                <p class="news-description" style="display: none;">${noticia.descripcion}</p>
                <span class="news-date" style="display: none;">${new Date(noticia.fechaPublicacion).toLocaleDateString()}</span>
            `;
            carrusel.appendChild(item);
        });
    }

    renderCarousel();

    document.querySelector(".carousel-btn-next").addEventListener("click", function () {
        if (ocultas.length > 0) {
            let primerVisible = visibles.shift(); // 🔹 Elimina el primer visible
            visibles.push(ocultas.shift()); // 🔹 Mueve el primero de la lista oculta a la visible
            ocultas.push(primerVisible); // 🔹 El eliminado pasa al final de los ocultos
            renderCarousel();
        }
    });

    document.querySelector(".carousel-btn-prev").addEventListener("click", function () {
        if (ocultas.length > 0) {
            let ultimoVisible = visibles.pop(); // 🔹 Elimina el último visible
            visibles.unshift(ocultas.pop()); // 🔹 Mueve el último oculto al inicio de los visibles
            ocultas.unshift(ultimoVisible); // 🔹 El eliminado pasa al inicio de los ocultos
            renderCarousel();
        }
    });

    console.log("Noticias iniciales en pantalla:", visibles);
    console.log("Noticias ocultas:", ocultas);
});
