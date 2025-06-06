document.addEventListener('DOMContentLoaded', function () {
    // Función para alternar visibilidad de la contraseña
    window.togglePasswordVisibility = function () {
        var passwordInput = document.getElementById("password");
        var toggleIcon = document.getElementById("togglePassword");
        if (passwordInput.type === "password") {
            passwordInput.type = "text";
            toggleIcon.classList.remove("fa-eye");
            toggleIcon.classList.add("fa-eye-slash");
        } else {
            passwordInput.type = "password";
            toggleIcon.classList.remove("fa-eye-slash");
            toggleIcon.classList.add("fa-eye");
        }
    };

    // Obtener datos enviados desde el servidor a través del contenedor oculto
    var dataContainer = document.getElementById("loginScriptData");
    var mensaje = dataContainer.dataset.mensaje;
    var redireccion = dataContainer.dataset.redireccion;

    // Desplegar alertas de SweetAlert2 según el mensaje recibido
    if (mensaje === "success") {
        Swal.fire({
            title: '¡Acceso permitido!',
            text: 'Bienvenido al sistema.',
            icon: 'success',
            confirmButtonText: 'Aceptar'
        }).then(() => {
            window.location.href = redireccion;
        });
    } else if (mensaje === "usuarioNoEncontrado") {
        Swal.fire({
            title: 'Usuario no encontrado',
            text: 'No hay un usuario registrado con ese documento.',
            icon: 'question',
            confirmButtonText: 'Reintentar'
        });
    } else if (mensaje === "contraIncorrecta") {
        Swal.fire({
            title: 'Contraseña incorrecta',
            text: 'La contraseña ingresada no es correcta.',
            icon: 'error',
            confirmButtonText: 'Intentar de nuevo'
        });
    } else if (mensaje === "bloqueado") {
        Swal.fire({
            title: 'Demasiados intentos',
            text: 'Has superado el límite de intentos fallidos. Intenta más tarde.',
            icon: 'warning',
            confirmButtonText: 'Aceptar'
        }).then(() => {
            window.location.href = "/ErrorBD";
        });
    }
});
