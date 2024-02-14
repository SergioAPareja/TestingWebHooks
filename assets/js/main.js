function animacion(pagina, animacion) {
    if (localStorage.animacion != pagina || localStorage.animacion == 'undefined') {
        localStorage.animacion = pagina;
        $('.animated').addClass('visible ' + animacion);
    }
    else {
        $('.animated').removeClass('animated');
    }
}

function salirApp() {
    swal({
        title: '',
        text: '¿Deseas salir de la aplicación?',
        type: '',
        showCancelButton: true,
        confirmButtonText: 'Sí',
        cancelButtonText: 'No',
        allowEscapeKey: false,
        allowOutsideClick: false,
        allowEnterKey: false
    }).then((result) => {
        if (result.value) {
            location.href = 'login.aspx';
        }
    });
};

function salirAppADM() {
    swal({
        title: '',
        text: '¿Deseas salir de la aplicación?',
        type: '',
        showCancelButton: true,
        confirmButtonText: 'Sí',
        cancelButtonText: 'No',
        allowEscapeKey: false,
        allowOutsideClick: false,
        allowEnterKey: false
    }).then((result) => {
        if (result.value) {
            location.href = 'loginADM.aspx';
}
});
};