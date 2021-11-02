function alert_success(title, msg, reload) {
    Swal.fire({
        icon: 'success',
        title: title,
        text: msg,
        timer: 3000,
        confirmButtonColor: '#28a745',
    }).then(() => {
        if (reload){
            location.reload();
        }
    })
}

function alert_fail(title, msg) {
    Swal.fire({
        icon: 'error',
        title: title,
        text: msg,
        timer: 3000,
        confirmButtonColor: '#dc3545',
    })
}