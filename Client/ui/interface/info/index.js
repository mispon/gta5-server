$(window).on('load', function() {
    safetySend('getPlayerInfo')
})

function showInfo(data, events) {  
    $('.name').text(data.Name);
    if (data.TagName) {
        $('.tagname').text(data.TagName);
        $('#clan-rank').text(data.ClanRank);
        $('#clan-rep').text(data.ClanRep);
    } else {
        $('.clan-rank').hide();
        $('.clan-rep').hide();
    }    
    $('#exp').text(data.Experience);
    $('#level').text(data.Level);
    $('#money').text(data.Balance);
    if (data.Driver) {
        $('#driver').text('есть');
    }
    $('#wanted').text(data.Wanted);
    $('#phone').text(data.PhoneNumber);
    $('#phone-balance').text(data.PhoneBalance);
    if (data.Work.length > 0) {
        $('#work').text(data.Work);
        $('#work-level').text(data.WorkLevel);
        $('#work-exp').text(data.WorkExp);
        $('#salary').text(data.Salary);
    } else {
        $('.work-level').hide();
        $('.work-exp').hide();
        $('.salary').hide();
    }
    setEventInfo(events);
    $('#svg').prop('checked', data.Settings.SvgSpeedometer);
}

$('#svg').on('click', function(e) {
    var checked = $('#svg').is(':checked');
    safetySend('changeSettings', checked)
})

$('.close').on('click', function() {
    safetySend('close')
})

function showInfoTab(e) {
    $('#events').hide();
    $('#settings').hide();
    $('#info').show();
    e.preventDefault();
}

function showEventsTab(e) {
    $('#info').hide();
    $('#settings').hide();
    $('#events').show();
    e.preventDefault();
}

function showSettingsTab(e) {    
    $('#info').hide();
    $('#events').hide();
    $('#settings').show();
    e.preventDefault()
}

$('#event-btn').on('click', function(e) {
    var isJoin = $('.event-member').is(':hidden')
    setMemberStatus(isJoin);
    safetySend('eventParticipation', isJoin)
    e.preventDefault();
})

function setEventInfo(events) {
    if (!events) {
       return       
    } 
    $('.event-status').text('Идет регистрация на эвент:')
    $('.event-name').text(events.Name)
    $('.event-type').text(events.Type)
    $('.members').text(events.TotalMembers)
    $('.max-members').text(events.MaxMembers)
    $('.event-name').show()
    $('.event-type').show()
    $('.event-members').show()
    setEventButton(events.IsMember)
    $('#event-btn').css('display', 'block')
}

function setMemberStatus(isJoin) {
    var members = parseInt($('.members').text())
    setEventButton(isJoin)
    $('.members').text(isJoin ? ++members : --members)
}

function setEventButton(isJoin) {
    if (isJoin) {
        $('#event-btn').removeClass('enter-event')
        $('#event-btn').addClass('exit-event')
        $('#event-btn').text('Отменить')
        $('.event-member').show()
    } else {
        $('#event-btn').removeClass('exit-event')
        $('#event-btn').addClass('enter-event')
        $('#event-btn').text('Вступить')
        $('.event-member').hide()
    }
}

function safetySend(methodName, value) {
	try {		
		resourceEval('resource.CefEventHandlerSingleton.trigger(\"PlayerInfo.' + methodName + '\", ' + JSON.stringify([value]) + ')')
	} catch (error) {
		console.error('resourceEval не поддерживается!')
	}
}