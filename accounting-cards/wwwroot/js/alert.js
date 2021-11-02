function alert_success(title, msg) {
    Swal.fire({
        icon: 'success',
        title: title,
        text: msg,
        timer: 1500,
        confirmButtonColor: '#28a745',
    })
}