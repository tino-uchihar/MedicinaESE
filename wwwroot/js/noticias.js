document.addEventListener("DOMContentLoaded", function () {
    const carouselWrapper = document.querySelector(".carousel-wrapper");
    const newsItems = document.querySelectorAll(".news-item");
    const prevBtn = document.getElementById("prevBtn");
    const nextBtn = document.getElementById("nextBtn");

    let visibleNews = 3; // Por defecto en PC
    let currentIndex = 0;

    function adjustCarousel() {
        let screenWidth = window.innerWidth;
        
        if (screenWidth < 768) {
            visibleNews = 1; // En móviles, 1 noticia visible
        } else if (screenWidth < 1024) {
            visibleNews = 2; // En tablets, 2 noticias visibles
        } else {
            visibleNews = 3; // En PC, máximo 3 noticias visibles
        }

        showNews(); // Ajustar la vista tras cambiar el tamaño
    }

    function showNews() {
        let offset = -currentIndex * (newsItems[0].offsetWidth);
        carouselWrapper.style.transform = `translateX(${offset}px)`;
    }

    prevBtn.addEventListener("click", () => {
        currentIndex = (currentIndex - 1 + newsItems.length) % newsItems.length;
        showNews();
    });

    nextBtn.addEventListener("click", () => {
        currentIndex = (currentIndex + 1) % newsItems.length;
        showNews();
    });

    window.addEventListener("resize", adjustCarousel);
    adjustCarousel();
});
