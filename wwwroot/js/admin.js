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
        var tipoElem = document.getElementById("TipoUsuario");
        if (!tipoElem) return; // Si no existe, salimos de la función
    
        var tipo = tipoElem.value;
        var divMedico = document.getElementById("div-medico");
        var divPaciente = document.getElementById("div-paciente");
    
        if (tipo === "admin" || tipo === "medico") {
            if (divMedico) divMedico.style.display = "block";
            if (divPaciente) divPaciente.style.display = "block";
        } else if (tipo === "paciente") {
            if (divMedico) divMedico.style.display = "none";
            if (divPaciente) divPaciente.style.display = "block";
        } else {
            if (divMedico) divMedico.style.display = "none";
            if (divPaciente) divPaciente.style.display = "none";
        }
    };
    
    // Ejecuta toggleCampos al cargar la página
    toggleCampos();

    // Función para gestionar la opción "Otro" en especialidad.
    window.toggleOtraEspecialidad = function () {
        var especialidadElem = document.getElementById("Especialidad");
        if (!especialidadElem) return;
        var especialidad = especialidadElem.value;
        var divOtra = document.getElementById("div-otra-especialidad");
        if (divOtra) {
            divOtra.style.display = (especialidad === "Otro") ? "block" : "none";
        }
    };

    // Función para gestionar la opción "Otro" en entidad de salud.
    window.toggleOtraEntidad = function () {
        var entidadElem = document.getElementById("EntidadSalud");
        if (!entidadElem) return;
        var entidad = entidadElem.value;
        var divOtra = document.getElementById("div-otra-entidad");
        if (divOtra) {
            divOtra.style.display = (entidad === "Otro") ? "block" : "none";
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

// Función para alternar la visibilidad del campo de contraseña (modo creación o edición).
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




// Función para alternar la visibilidad del campo de contraseña en modo edición.
function toggleEditPasswordVisibility() {
    var passwordInput = document.getElementById("editContraseña");
    if (!passwordInput) return;
    passwordInput.type = (passwordInput.type === "password") ? "text" : "password";
}

// Función para restablecer el valor original del campo usando su atributo data-original.
function resetField(fieldId) {
    var field = document.getElementById(fieldId);
    if (field && field.dataset.original) {
        field.value = field.dataset.original;
    }
}

// Función para invocar la edición del usuario mediante Fetch, cargando el partial view renderizado.
function editUser(documentoId) {
    fetch(`/Admin/editar-usuario?documentoId=${documentoId}`)
        .then(response => {
            if (!response.ok) {
                // Mostrar un mensaje minimalista en caso de error 404.
                Swal.fire({
                    title: "Error",
                    text: "No se encontró el usuario.",
                    icon: "error",
                    confirmButtonText: "Aceptar"
                });
                throw new Error("Error al obtener información del usuario");
            }
            return response.text();
        })
        .then(html => {
            // Inyecta el HTML del partial view en el contenedor con id "editSection"
            var container = document.getElementById("editSection");
            if (container) {
                container.innerHTML = html;
            }
            // Opcionalmente, resalta la fila del usuario editado en la lista.
            resaltarFila(documentoId);
        })
        .catch(error => console.error("Error:", error));
}

// Función para resaltar la fila de usuario editada en la lista.
function resaltarFila(documentoId) {
    const rows = document.querySelectorAll("#userTableBody tr.userRow");
    rows.forEach(row => {
        if (row.cells[1].innerText.trim() === documentoId) {
            row.classList.add("editing");
        } else {
            row.classList.remove("editing");
        }
    });
}

// Función para cancelar la edición (oculta el partial view y quita el resaltado).
function cancelEditing() {
    var container = document.getElementById("editSection");
    if (container) {
        container.innerHTML = "";
    }
    document.querySelectorAll("#userTableBody tr.userRow")
        .forEach(row => row.classList.remove("editing"));
}

// Función para confirmar la edición (placeholder; aquí se acumularían o guardarían cambios).
function confirmEditing() {
    Swal.fire({
        title: "¿Confirmar edición?",
        text: "Los cambios se acumularán hasta que se haga clic en 'Guardar todo'.",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Sí, confirmar",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            cancelEditing();
        }
    });
}



/* ---------- flujo borrar ---------- */
function deleteUser(documentoId) {
    fetch(`/Admin/borrar-usuario?documentoId=${documentoId}`)
        .then(r => {
            if (!r.ok) throw "No encontrado";
            return r.text();
        })
        .then(html => {
            document.getElementById("editSection").innerHTML = html;
            resaltarFila(documentoId);
        })
        .catch(() => Swal.fire("Error", "No se encontró el usuario.", "error"));
}

function confirmDelete(documentoId) {
    Swal.fire({
        title: "¿Eliminar usuario?",
        text: "Esta acción no se puede deshacer.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonText: "Cancelar",
        confirmButtonText: "Sí, eliminar"
    }).then(res => {
        if (!res.isConfirmed) return;

        fetch(`/api/Usuarios/${documentoId}`, { method: "DELETE" })
            .then(r => {
                if (!r.ok) throw "Falló el borrado";
                // 1) quitar fila
                document.querySelectorAll("#userTableBody tr.userRow")
                    .forEach(row => {
                        if (row.cells[1].innerText.trim() === documentoId) row.remove();
                    });
                // 2) limpiar panel
                cancelEditing();
                Swal.fire("Eliminado", "Usuario eliminado.", "success");
            })
            .catch(() => Swal.fire("Error", "No se pudo eliminar.", "error"));
    });
}
