// NOTE: значение счетчика должно быть равно количеству вопросов
var counter = 8;
// Вопросы, которые уже были
var alreadyQuestions = [];
// Счетчик правильных ответов
var rightAnswersCount = 0;
// Правильный ответ на текущий вопрос
var rightAnswer;

$(window).on('load', () => {
    nextQuestion();
});

// флаг нужен для того, чтобы обезопаситься от накрутки правильных ответов
var needStop = false;
$('li').on('click', (e) => {
    var userAnswer = e.currentTarget.innerHTML;
    if (!needStop && userAnswer === rightAnswer) {
        rightAnswersCount++;
    }
    needStop = alreadyQuestions.length >= counter
    if (needStop) {
        showResult();
        safetySend("finishExam");
    } else {
        nextQuestion();
    }
});

// показать результаты экзамена
function showResult() {
    $('.answers-count').text(rightAnswersCount + ' / ' + counter)
    var isDone = rightAnswersCount >= (counter - 1)
    $('#result-message').text(isDone ? 'Поздравляем! Вы сдали теоретический экзамен' : 'Вы не сдали экзамен. Попробуйте еще раз немного позже')
    $('#result-message').addClass(isDone ? 'success' : 'fail')
    $('.test-info').css('display', 'none')
    $('.result-info').css('display', 'block')
}

function nextQuestion() {
    var question = getQuestion();
    rightAnswer = question[5];
    $('div.question > p').text(question[0]);
    $('ul.answers > li.answ-1').text(question[1]);
    $('ul.answers > li.answ-2').text(question[2]);
    $('ul.answers > li.answ-3').text(question[3]);
    $('ul.answers > li.answ-4').text(question[4]);
}

// Взять рандомный вопрос
function getQuestion() {
  for (var i = 0; i < counter; i++) {
      var questionNumber;
      var attemptCount = 0
      do {
          questionNumber = getRandom();
          attemptCount++;
      } while($.inArray(questionNumber, alreadyQuestions) !== -1 || attemptCount < counter)
      alreadyQuestions.push(questionNumber);
      return questions[questionNumber];
  }
}

// Возвращает рандомное число от 0 до counter
function getRandom() {
    return Math.floor(Math.random() * counter);
}

function safetySend(methodName) {
  try {
      var isDone = rightAnswersCount >= 5;      
      resourceEval('resource.CefEventHandlerSingleton.trigger(\"TheoryExamBrowser.' + methodName + '\", ' + JSON.stringify(isDone) + ')')
  } catch (error) {
      console.error('resourceEval не поддерживается!')
  }
}

// Вопрос, 4 варианта ответов, правильный ответ
var questions = {
    0 : ['Если на перекрёстке нет светофора и знака "Главная дорога", то кто должен ехать первым?', 
          '1. Тот, у кого нет помехи с правой стороны', 
          '2. Тот, у кого нет помехи с левой стороны', 
          '3. Любой желающий', 
          '4. Тот, кто приехал первый на перекрёсток',
          '1. Тот, у кого нет помехи с правой стороны'],
    1 : ['На какой сигнал светофора разрешается движение?', 
          '1. На красный', 
          '2. На желтый',
          '3. На зелёный', 
          '4. На любой',
          '3. На зелёный'],
    2 : ['Разрешён-ли обгон по встречной полосе?', 
          '1. Нет, всегда запрещено', 
          '2. Да, только если полоса пуста и нет запрещающей разметки', 
          '3. Да, даже если зади меня едет автомобиль с проблесковыми маячками', 
          '4. Разрещен всегда',
          '2. Да, только если полоса пуста и нет запрещающей разметки'],
    3 : ['При повороте направо, какую полосу разрешено занимать?', 
          '1. Любую', 
          '2. Только крайнюю правую', 
          '3. Только крайнюю левую', 
          '4. Нет правильного ответа',
          '2. Только крайнюю правую'],
    4 : ['Вы буксируете неисправный автомобиль. По какой полосе Вам можно продолжить движение?', 
          '1. Только по правой', 
          '2. Только по левой', 
          '3. По любой', 
          '4. Нет правильного ответа',
          '1. Только по правой'],
    5 : ['Какие будут ваши действия, если вы услышите автомобиль с сиреной и проблесковым маячком?', 
          '1. Буду ехать дальше так, как и ехал', 
          '2. Займу крайнюю левую полосу и остановлюсь', 
          '3. Остановлюсь на той полосе, которой ехал', 
          '4. Займу крайнюю правую полосу и сброшу скорость, при необходимости остановлюсь',
          '4. Займу крайнюю правую полосу и сброшу скорость, при необходимости остановлюсь'],
    6 : ['Что означает мигание желтого сигнала светофора?',
         '1. Предупреждает о неисправности светофора',
         '2. Разрешает движение и информирует о наличии нерегулируемого перекрестка или пешеходного перехода',
         '3. Запрещает дальнейшее движение',
         '4. Не знаю',
         '2. Разрешает движение и информирует о наличии нерегулируемого перекрестка или пешеходного перехода'],
    7 : ['Должны ли Вы уступить дорогу грузовому автомобилю, выезжающему с заправки?',
         '1. Должны',
         '2. Должны, если намерены повернуть на право',
         '3. Не должны',
         '4. Не знаю',
         '3. Не должны']
}