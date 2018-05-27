const male = 1885233650;
const female = -1667301416;

var character = {
    Skin: male,
    FatherFace: 0,
    MotherFace: 0,
    SkinMix: 0.5,
    ShapeMix: 0.5,
    HairStyle: 0,
    HairColor: 0,
    EyesColor: 0
}

let facesFather = [
	0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 42, 43, 44
];
let facesMother = [
	21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 45
];

$('.arrow-left').on('click', function() {
    var $elem = '#' + $(this).data('id')
    var value = parseFloat($($elem).val()) - parseFloat($($elem).attr('step'))
    if (value < 0) {
        return
    }
    $($elem).val(value)
    $($elem).trigger('input')
})

$('.arrow-right').on('click', function() {
    var $elem = '#' + $(this).data('id')
    var value = parseFloat($($elem).val()) + parseFloat($($elem).attr('step'))
    var maxValue = $('#fatherFace').attr('max')
    if (value === maxValue) {
        return
    }
    $($elem).val(value)
    $($elem).trigger('input')
})

$('#male').on('change', function() {
    $('#face').attr({'value': 0, 'min': 0, 'max': 20})
    $('#hairStyle').attr('max', 36)
    clearModel()
    character.Skin = parseInt(male)    
    safetySend('ChangeSkin')
})

$('#female').on('change', function() {    
    $('#face').attr({'value': 21, 'min': 21, 'max': 45})
    $('#hairStyle').attr('max', 38)
    clearModel()
    character.Skin = parseInt(female)
    safetySend('ChangeSkin')
})

$('#fatherFace').on('input', function() {
    var value = $(this).val();
    character.FatherFace = facesFather[parseInt(value)];
    safetySend('ShowView')
});

$('#motherFace').on('input', function() {
    var value = $(this).val();
    character.MotherFace = facesMother[parseInt(value)];
    safetySend('ShowView')
});

$('#skinMix').on('input', function() {
    var value = $(this).val();
    character.SkinMix = parseFloat(value);
    safetySend('ShowView')
});

$('#shapeMix').on('input', function() {
    var value = $(this).val();
    character.ShapeMix = parseFloat(value);
    safetySend('ShowView')
});

$('#hairStyle').on('input', function() {
    var value = $(this).val();
    if ((character.Skin == female && value == '24') || 
        (character.Skin == male && value == '23')) {
        return
    }
    character.HairStyle = parseInt(value);
    safetySend('ShowView')
});

$('#hairColor').on('input', function() {
    var value = $(this).val();
    character.HairColor = parseInt(value);
    safetySend('ShowView')
});

$('#eyesColor').on('input', function() {
    var value = $(this).val();
    character.EyesColor = parseInt(value);
    safetySend('ShowView')
});

function saveChar(e) {
    e.preventDefault();
    var name = $("#name").val();
    var surname = $("#surname").val();
    if (isEmpty(name) || isEmpty(surname)) {
        wrongName("Имя или фамилия не могут быть пустыми!");
        return;
    }
    if (!isOnlyLatinChars(name) || !isOnlyLatinChars(surname)) {
        wrongName("Используйте только латинские буквы");
        return;
    }
    $('#create-btn').html("<i class='fa fa-spinner fa-spin fa-fw'></i>");
    safetySend('SaveChar', name, surname)
}

function nameAlreadyExist() {
    $('#create-btn').html('Создать');
    wrongName('Данное имя уже занято другим игроком')
}

function wrongName(message) {
    $('.text-input').css('border', '1px solid red')
    $(".error").text(message);
    $(".error").show();
}

$(".text-input").on('focus', function () {
    $(".text-input").css('border', '1px solid #b5b8bf');
    $(".error").empty();
    $(".error").hide();
});

function clearModel() {
    character.FatherFace = 0;
    character.MotherFace = 0;    
    character.SkinMix = 0.5;
    character.ShapeMix = 0.5;
    character.HairStyle = 0;
    character.HairColor = 0;
    character.EyesColor = 0;
    $('#fatherFace').val(0);
    $('#motherFace').val(0);
    $('#skinMix').val(0.5);
    $('#shapeMix').val(0.5);
    $('#hairStyle').val(0);
    $('#hairColor').val(0);
    $('#eyesColor').val(0);
}

function safetySend(methodName, name, surname) {
    try {
        var params = [character];
        if (name != undefined > 0 && surname != undefined) {
            params.push(name);
            params.push(surname);
        }
        resourceEval('resource.CefEventHandlerSingleton.trigger(\"CharCreator.' + methodName + '\", ' + JSON.stringify(params) + ')')
    } catch (error) {
        console.error('resourceEval не поддерживается!')
    }
}