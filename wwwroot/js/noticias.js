window.addEventListener("load", function() {
    const noticias = window.noticiasJson;

    if (!noticias || noticias.length === 0) {
        console.error("No hay noticias para mostrar.");
        return;
    }

    const totalItems = noticias.length; // Para tu ejemplo, serán 4
    const visibleCount = 3;              // Se muestran 3 ítems a la vez
    let currentIndex = 0;                // Índice del primer ítem visible

    const carousel = document.querySelector(".carousel");
    carousel.innerHTML = ""; // Limpiar el contenido previamente generado

    // Crear y agregar todos los ítems al contenedor
    noticias.forEach(noticia => {
        const item = document.createElement("div");
        item.className = "carousel-item";
        item.dataset.id = noticia.id;

        item.innerHTML = `
            <img src="${noticia.imagenUrl}" alt="${noticia.titulo}" class="news-img">
            <h5 class="news-title">${noticia.titulo}</h5>
            <p class="news-description" style="display:none;">${noticia.descripcion}</p>
            <span class="news-date" style="display:none;">${new Date(noticia.fechaPublicacion).toLocaleDateString()}</span>
        `;
        carousel.appendChild(item);
    });

    // Función que actualiza la posición del carrusel mediante translateX
    function updateCarousel() {
        const containerWidth = carousel.offsetWidth;
        // Consideramos el gap de 10px definido en el CSS. Como se muestran visibleCount ítems,
        // calculamos el ancho aproximado de cada ítem:
        const gap = 10;
        const itemWidth = (containerWidth - (visibleCount - 1) * gap) / visibleCount;
        // Se desplaza el contenedor en función del índice actual.
        const shift = currentIndex * (itemWidth + gap);
        carousel.style.transform = `translateX(-${shift}px)`;
    }

    updateCarousel();

    // Asignación de eventos a los botones de navegación
    const btnNext = document.querySelector(".carousel-btn-next");
    const btnPrev = document.querySelector(".carousel-btn-prev");

    btnNext.addEventListener("click", function() {
        currentIndex = (currentIndex + 1) % totalItems;
        updateCarousel();
    });

    btnPrev.addEventListener("click", function() {
        currentIndex = (currentIndex - 1 + totalItems) % totalItems;
        updateCarousel();
    });

    // Ajustar al cambiar el tamaño de la ventana
    window.addEventListener("resize", updateCarousel);

    console.log("Noticias generadas en el carrusel:", noticias);
});
