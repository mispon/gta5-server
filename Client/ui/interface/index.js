function updateInfo(satiety, money, exp) {
	updateSatiety(satiety);
	updateMoney(money)
	updateExperience(exp)
}

function updateMoney(money) {
    var moneyString = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    $('.money').text('$ ' + moneyString);
}

function updateSatiety(percent) {
    $("#satiety-bar").width(percent + '%');
    $('.satiety-percent').text(percent + '%');
}

function updateExperience(percent) {
    $("#exp-bar").width(percent + '%');
    $('.exp-percent').text(percent + '%');
}

function showHint(message, px) {
	let heightSize = px ? px : 80;
	$('.hint').css('height', heightSize + 'px')
	$('.hint-message').text(message)
	$('.hint').show()
}

function hideHint() {
	$('.hint').hide()
}

var timerId;
function startTimer(seconds, methodName) {
	var time = seconds
	$('.timer').css('display', 'block')	
	timerId = setInterval(function() {
		if (time > 0) {
			setTimerColor(time)
			$('.timer > p').text(getPrettyTime(time))
			--time
			return
		}		
		safetySend(methodName)
		stopTimer()
	}, 1000)	
}

function stopTimer() {
	$('.timer').css('display', 'none')	
	$('.timer > p').text('')
	clearInterval(timerId)
	timerId = 0
}

function setTimerColor(time) {
	if (time < 6) {
		$('.timer > p').css('color', '#ff0000')
	} else {
		$('.timer > p').css('color', '#F1F1F1')
	}
}

function getPrettyTime(time) {
	var seconds = time % 60 
	var minutes = (time - seconds) / 60
	var isTwoDigit = 9 < seconds
	return getDigitForm(minutes) + ':' + getDigitForm(seconds)
}

function getDigitForm(num) {
	const maxOneDigit = 9;
	return maxOneDigit < num ? num : '0' + num
}

var actionTimer;
function setProgressAction(seconds, methodName) {
	var time = 0
	$('#action-progress').css('display', 'block')	
	actionTimer = setInterval(function() {
		var percent = (time * 100) / seconds  
		$("#action-progress-bar").width(percent + '%');
		if (time >= seconds) {
			safetySend(methodName)
			stopProgressAction()
		}
		time++
	}, 1000)
}

function stopProgressAction() {
	$('#action-progress').css('display', 'none')
	$("#action-progress-bar").width('0%');
	clearInterval(actionTimer)
}

var giftsTimerId = 0;
function setGiftsTimer(seconds, methodName) {
	if (giftsTimerId !== 0) {
		return
	}
	var time = seconds
	giftsTimerId = setInterval(function() {
		if (time > 0) {
			setTimerColor(time)
			$('.gifts > p').text(getPrettyTime(time))
			--time
			return
		}
		safetySend(methodName)
		clearInterval(giftsTimerId)
		giftsTimerId = 0
	}, 1000)
}

function safetySend(methodName) {
	try {		
		resourceEval('resource.CefEventHandlerSingleton.trigger(\"Interface.' + methodName + '\")')
	} catch (error) {
		console.error('resourceEval не поддерживается!')
	}
  }
