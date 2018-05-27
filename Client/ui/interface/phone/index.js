$(window).on('load', function() {
    resourceEval('resource.CefEventHandlerSingleton.trigger(\"Phone.getPhoneInfo\")')
})

/*
*** Совершает звонок
*/
function makeCall() {
    showDisplay('.call-display')
    var name = $(this).data('name')
    var number = $(this).data('number')
    $('.callee').text(name)
    $('.callee-number').text(number)
    resourceEval('resource.CefEventHandlerSingleton.trigger(\"Phone.startCall\", ' + JSON.stringify([number]) + ')')
}

/*
*** Открытие формы добавления контакта
*/
$('.new-contact').on('click', function() {
    var $form = $('.new-contact-form')
    if ($form.is(':visible')) {
        $form.hide('slow')
    }
    else {
        $form.show('slow')
    }    
})

/*
*** Добавление нового контакта
*/
$('.add-new-contact').on('click', function() {
    var name = $('.name-input').val()
    var number = $('.number-input').val()
    if (name.length == 0 || number.length == 0) {
        return
    }
    $('.name-input, .number-input').val('')
    $('.new-contact-form, .no-contacts').hide()
    addContactToList(name, number)
    resourceEval('resource.CefEventHandlerSingleton.trigger(\"Phone.addContact\", ' + JSON.stringify([name, number]) + ')')
})

/*
*** Кнопка принятия звонка
*/
$('.accept-call').on('click', function() {
    showDisplay('.call-display')
    var name = $('.caller').text()
    var number = $('.caller-number').text()
    $('.callee').text(name)
    $('.callee-number').text(number)
    resourceEval('resource.CefEventHandlerSingleton.trigger(\"Phone.answerCall\", ' + JSON.stringify([true]) + ')')
})

/*
*** Кнопка отклонения звонка
*/
$('.decline-call').on('click', function() {    
    showMainDisplay()
    resourceEval('resource.CefEventHandlerSingleton.trigger(\"Phone.answerCall\", ' + JSON.stringify([false]) + ')')
})

/*
*** Кнопки отмены действий, возврата на главный экран
*/
$('.back, .end-call').on('click', function() {
    if ($('.contacts-display').is(':visible')) {
        resourceEval('resource.CefEventHandlerSingleton.trigger(\"Phone.hidePhone\")')
    }
    else {
        showMainDisplay()
        resourceEval('resource.CefEventHandlerSingleton.trigger(\"Phone.hangupCall\")')
    }    
})

/*
*** Возвращает на главный дисплей
*/
function showMainDisplay() {
    $('.call-display, .inc-call-display').hide()
    $('.contacts-display, .new-contact').show()
}

/*
*** Скрывает всевозможные активные дисплеи и показывает переданный
*/
function showDisplay(selector) {
    $('.contacts-display, .new-contact, .inc-call-display').hide()
    $(selector).show()
}

/*
*** Добавление контакта в список
*/
function addContactToList(name, number) {
    $('.contacts-display > ul').append(
        '<li>' +
            '<div class="contact-info">' +
                '<p class="name">' + name + '</p>' +
                '<small class="number">' + number + '</small>' +
            '</div>' +
            '<div class="contact-btns">' +
                '<i id="call-' + number + '" class="fas fa-phone call" data-name="' + name + '" data-number="' + number + '"></i>' +
            '</div>' +
        '</li>'
    )
    $('#call-' + number).on('click', makeCall)
}

/*
*** Инициализация контактов и истории сообщений
*/
function setInfo(contacts, caller, callerNumber) { 
    if (contacts.length > 0) {
        $('.no-contacts').hide()
        $('.contacts-display > ul').empty()
        contacts.forEach(contact => {
            addContactToList(contact.Name, contact.Number)
        });
    }
    if (caller) {        
        $('.caller').text(caller)
        $('.caller-number').text(callerNumber)
        showDisplay('.inc-call-display')
        
    }
}