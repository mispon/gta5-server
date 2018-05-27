$('.close').on('click', function() {
    safetySend('close')
})

function showInfo(e) {
    $('#hot-keys').hide();
    $('#social').hide();
    $('#tikets').hide();
    $('#info').show();
    e.preventDefault()
}

function showHotKeys(e) {    
    $('#info').hide();
    $('#social').hide();
    $('#tikets').hide();    
    $('#hot-keys').show();
    e.preventDefault()
}

function showSocial(e) {
    $('#info').hide();
    $('#hot-keys').hide();    
    $('#tikets').hide();
    $('#social').show();
    e.preventDefault()
}

function showDonate(e) {
    $('#info').hide();   
    $('#hot-keys').hide();
    $('#social').hide();
    $('#tikets').show();    
    e.preventDefault()
}

function safetySend(methodName) {
	try {		
		resourceEval('resource.CefEventHandlerSingleton.trigger(\"Help.' + methodName + '\")')
	} catch (error) {
		console.error('resourceEval не поддерживается!')
	}
}