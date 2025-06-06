window.onload = function () {
    setTimeout(function () {
        var mensaje = document.getElementById("mensaje-acceso-denegado");
        if (mensaje) {
            mensaje.style.display = "none";
        }
    }, 3000); // Se oculta después de 3 segundos
};
