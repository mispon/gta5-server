var email, password, rePassword, youtuberCode, friendCode;

$("#continue").on('click', function (e) {
    e.preventDefault();  
    email = $("#r-email").val();
    password = $("#r-password").val();
    rePassword = $("#re-password").val();
    // if (isEmpty(email)) {
    //     wrongRegisterType("Почта не может быть пустой!");
    //     showError('#r-email');
    //     return false;
    // }
    // password = $("#r-password").val();
    // if (isEmpty(password)) {
    //     wrongRegisterType("Пароль не может быть пустой!");
    //     showError('#r-password');
    //     return false;
    // }
    // rePassword = $("#re-password").val();
    // if (isEmpty(rePassword)) {
    //     wrongRegisterType("Повтор пароля не может быть пустой!");
    //     showError('#re-password');
    //     return false;
    // }
    // if (!validateEmail(email)) {
    //     wrongRegisterType("Вы ввели некорректную почту!");
    //     showError('#r-email');
    //     return false;
    // }
    // if (password.length < 6) {
    //     showError('#r-password');
    //     wrongRegisterType("Пароль не может быть короче 6 символов!");
    //     return false;
    // }
    // if (password !== rePassword) {
    //     showError('#r-password, #re-password');
    //     wrongRegisterType("Введенные пароли не совпадают!");
    //     return false;
    // }
    $('.main-info').hide();
    $('.referal-info').show();    
});

$("#register").on('click', function (e) {
    e.preventDefault();    
    //youtuberCode = $("#youtuber-referal").val();
    friendCode = $("#friend-referal").val();
    $('#register').html("<i class='fa fa-spinner fa-spin fa-fw'></i>");
    safetySend("register", email, password, friendCode);
    return true;
});

$('.to-login > a').on('click', function() {
    clearErrors()
    $('form.registration').hide();
    $('form.login').show();
})

function badRegister() {
    $('#register').html("Зарегистрироваться");
    $('.referal-info').hide('slow');
    $('.main-info').show('slow');    
    wrongRegisterType("Аккаунт с такой почтой уже существует!");
    showError('#r-email, #r-password, #re-password');
}

function wrongRegisterType(message) {
    $(".error-message > p").empty();
    $(".error-message > p").append(message);
    $(".error-message").css('display', 'block');
}

function showError(selector) {
    $(selector).val('');
    $(selector).css('border', '1px solid #ff0000');
}

$("#r-email, #r-password, #re-password").on('focus', function () {
    clearErrors()
});

function clearErrors() {
    $("#l-email, #l-password, #r-email, #r-password, #re-password").css('border', '1px solid #b5b8bf');
    $(".error-message > p").empty();
    $(".error-message").css('display', 'none');
}

function safetySend(methodName, email, password, friendReferal) {
    try {
        resourceEval('resource.CefEventHandlerSingleton.trigger(\"Auth.' + methodName + '\", ' + JSON.stringify([email, password, friendReferal]) + ')')
    } catch (error) {
        console.error('resourceEval не поддерживается!')
    }
}