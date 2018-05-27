var keysToSwap = [
    32,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,
	70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,190
]

var systemKeys = [
    8,9,13,16,17,18,19,20,27,33,34,35,36,37,38,39,40,45,46,144,145,
	96,97,98,99,100,101,102,103,104,105
]

var capsEnabled = false;
var shiftPressed = false;

$(window).on('keydown', function(e) {
	if (e.keyCode == 16) {
        shiftPressed = true;
    }
    if (e.keyCode == 20) {
        capsEnabled = !capsEnabled;
    }
})

$(window).on('keyup', function(e) {
    if (e.keyCode == 16) {
        shiftPressed = false;
    }
})

$('input').on('keydown', function(e) {
    var isSystem = systemKeys.indexOf(e.keyCode) != -1;
    if (!isSystem) {
        e.preventDefault();
        var char = (keysToSwap.indexOf(e.keyCode) != -1) ? getChar(e.keyCode) : '';
        var oldValue = $(this).val();
        $(this).val(oldValue + char);
    }
})

function getChar(keyCode) {
    var result = String.fromCharCode(keyCode).toLowerCase();;
    if ((capsEnabled && !shiftPressed) || (!capsEnabled && shiftPressed)) {
        result = result.toUpperCase();
    }
    if (keyCode == 50 && shiftPressed) {
        result = '@';
    }
    if (keyCode == 190) {
        result = '.';
    }
    if (keyCode == 32) {
        result = ' ';
    }
    return result;
}