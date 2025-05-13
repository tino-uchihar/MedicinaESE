window.addEventListener("load", function () {
    let noticias = window.noticiasJson;
    if (!noticias || noticias.length === 0) {
        console.error("No hay noticias para mostrar.");
        return;
    }

    let visibles = noticias.slice(0, 3); // 🔹 Noticias que estarán en pantalla
    let ocultas = noticias.slice(3); // 🔹 Noticias en la lista de reserva, NO se renderizan

    let carrusel = document.querySelector(".carousel");

    function renderCarousel() {
        carrusel.innerHTML = ""; // 🔹 Limpiar el carrusel antes de actualizarlo
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
            let primerVisible = visibles.shift(); // 🔹 Elimina el primero de la lista visible
            visibles.push(ocultas.shift()); // 🔹 Mueve el primero de la lista oculta a la visible
            ocultas.push(primerVisible); // 🔹 Lo que salió pasa al final de los ocultos
            renderCarousel(); // 🔹 Solo se actualizan los elementos mostrados en pantalla
        }
    });

    document.querySelector(".carousel-btn-prev").addEventListener("click", function () {
        if (ocultas.length > 0) {
            let ultimoVisible = visibles.pop(); // 🔹 Elimina el último de la lista visible
            visibles.unshift(ocultas.pop()); // 🔹 Mueve el último oculto al inicio de los visibles
            ocultas.unshift(ultimoVisible); // 🔹 Lo que salió pasa al inicio de los ocultos
            renderCarousel(); // 🔹 Solo se actualizan los elementos en pantalla
        }
    });

    console.log("Noticias iniciales en pantalla:", visibles);
    console.log("Noticias ocultas en memoria:", ocultas);
});
