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
