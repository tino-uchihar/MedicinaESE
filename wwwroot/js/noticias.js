let posicion = 0;
const totalItems = document.querySelectorAll(".carousel-item").length;
const visibleItems = 3;

function moverIzquierda() {
    if (posicion > 0) {
        posicion--;
        actualizarCarrusel();
    }
}

function moverDerecha() {
    if (posicion < totalItems - visibleItems) {
        posicion++;
        actualizarCarrusel();
    }
}

function actualizarCarrusel() {
    const carrusel = document.querySelector(".carousel");
    carrusel.style.transform = `translateX(-${posicion * 210}px)`;
}
