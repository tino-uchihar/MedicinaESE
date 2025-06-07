function confirmLogout() {
    Swal.fire({
        title: "¿Seguro que quieres cerrar sesión?",
        text: "Se cerrará tu sesión actual y necesitarás iniciar sesión de nuevo.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Sí, cerrar sesión",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = "/Admin/Logout"; // Redirige a la página de cierre de sesión
        }
    });
}




document.addEventListener('DOMContentLoaded', function() {
    // Función para mostrar u ocultar secciones según el rol seleccionado.
    window.toggleCampos = function () {
        var tipo = document.getElementById("TipoUsuario").value;
        var divMedico = document.getElementById("div-medico");
        var divPaciente = document.getElementById("div-paciente");

        if (tipo === "admin" || tipo === "medico") {
            divMedico.style.display = "block";
            divPaciente.style.display = "block";
        } else if (tipo === "paciente") {
            divMedico.style.display = "none";
            divPaciente.style.display = "block";
        } else {
            divMedico.style.display = "none";
            divPaciente.style.display = "none";
        }
    };

    // Función para gestionar la opción "Otro" en especialidad.
    window.toggleOtraEspecialidad = function () {
        var especialidad = document.getElementById("Especialidad").value;
        var divOtra = document.getElementById("div-otra-especialidad");
        if (especialidad === "Otro") {
            divOtra.style.display = "block";
        } else {
            divOtra.style.display = "none";
        }
    };

    // Función para gestionar la opción "Otro" en entidad de salud.
    window.toggleOtraEntidad = function () {
        var entidad = document.getElementById("EntidadSalud").value;
        var divOtra = document.getElementById("div-otra-entidad");
        if (entidad === "Otro") {
            divOtra.style.display = "block";
        } else {
            divOtra.style.display = "none";
        }
    };

    // Confirmación para el botón Cancelar usando SweetAlert2.
    var cancelButton = document.querySelector('.action-back');
    if (cancelButton) {
        cancelButton.addEventListener('click', function(e) {
            e.preventDefault();
            Swal.fire({
                title: '¿Está seguro?',
                text: "Se cancelará la creación del usuario.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Sí, cancelar',
                cancelButtonText: 'No, volver'
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = '../admin-main';
                }
            });
        });
    }

    // Confirmación para el botón Confirmar (envío del formulario) usando SweetAlert2.
    var form = document.querySelector('form');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();
            Swal.fire({
                title: '¿Confirma la creación del usuario?',
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Sí, confirmar',
                cancelButtonText: 'No, volver'
            }).then((result) => {
                if (result.isConfirmed) {
                    form.submit();
                }
            });
        });
    }    
});





window.togglePasswordVisibility = function () {
    var passwordInput = document.getElementById("Contraseña");
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
