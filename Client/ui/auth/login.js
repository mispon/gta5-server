$("#login").on('click', function (e) {
    e.preventDefault()
    const email = $("#l-email").val();
    // if (isEmpty(email)) {
    //     wrongLogin("Логин не может быть пустым!");
    //     showError('#l-email');
    //     return false;
    // }
    const password = $("#l-password").val();
    // if (isEmpty(password)) {
    //     wrongLogin("Пароль не может быть пустым!");
    //     showError('#l-password');
    //     return false;
    // }
    // if (!validateEmail(email)) {
    //     wrongLogin("Вы ввели некорректную почту!");
    //     showError('#l-email');
    //     return false;
    // }
    $('#login').html("<i class='fa fa-spinner fa-spin fa-fw'></i>");
    safetySend("login", email, password);
    return true;
});

$('.to-register > a').on('click', function() {
    clearErrors()
    $('form.login').hide()
    $('form.registration').show()
})

function badLogin() {
    $('#login').html('Войти');
    wrongLogin('Вы ввели неверный пароль')
}

function wrongLogin(message) {
    $(".error-message > p").empty();
    $(".error-message > p").append(message);
    $(".error-message").css('display', 'block');
}

$("#l-email, #l-password").on('focus', function () {
    clearErrors()
});

function safetySend(methodName, email, password) {
    try {
        resourceEval('resource.CefEventHandlerSingleton.trigger(\"Auth.' + methodName + '\", ' + JSON.stringify([email, password]) + ')')
    } catch (error) {
        console.error('resourceEval не поддерживается!')
    }
}
