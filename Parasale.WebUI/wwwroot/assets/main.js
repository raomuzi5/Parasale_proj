

// if (!('webkitSpeechRecognition' in window)) {
//   alert('get yourself a proper browser');
// }
//var isChrome = !!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime);
//if (!isChrome) {
//    window.location.href = "/Home/MozilaFF";
//}
$(document).ready(function () {
    $(".play").attr("disabled", true);
    
    //console.log(lastVoice);
    //alert(lastVoice);
});
var voices;
$(document).on("click", "#viewObjections", function () {
  
    var colId = $("#collectionId").val();
    if (colId !== "") {

        if ($(".mcicon").hasClass("fa-microphone")) {
            btnstate = "fa-microphone";
            $(".mcicon").addClass("fa-microphone-slash");
            $(".mcicon").removeClass("fa-microphone");
            endRecognition();
        }
        else {
            btnstate = "fa-microphone-slash";

        }
        $.ajax({
            url: "/Speech/GetObjectionViews",
            data: { colId: colId },
            type: "GET",
            success: function (res) {

                $("#CreateNewModal-label").html("Objections in List");
                $("button.close").attr("id", "btnClose");
                $("#CreateNewBody").html(res);
                $("#CreateNewModal").modal();
            }
        });
    }
});
// creates a SpeechRecognition object that we can operate on
window.SpeechRecognition = window.SpeechRecognition || webkitSpeechRecognition;
const recognition = new SpeechRecognition();
//const recognition = new (window.SpeechRecognition || window.webkitSpeechRecognition || window.mozSpeechRecognition || window.msSpeechRecognition)();
//recognition.maxAlternatives = 5;
//recognition.lang = "en-US";
//recognition.interimResults = false;
// recognition.continuous = true;
// TODO: update this comment - what does this code do?
recognition.interimResults = true;
const synth = window.speechSynthesis;
var watch = new Stopwatch();
var WPR;
var WPM;
var RPA;
var wordSpoken;
var wordsMatch;
var finalArray = [];
var obj = {};
var arrayTobeSaved = {};
var TotalScore;	//TotalScore is Sum of: (WPMScore+WPRScore+RPAScore)/3
var WPMTargert = 140;	//
var WPMMeasure;	// wordcount in our case.
var WPMScore;	// =(1-ABS(1-WPMMeasure/WPMTargert))*100
var WPRTarget;	//  WordMatch in our case.
var WPRMeasure;	// wordSpoken in our case.
var WPRScore; 	// =(1-ABS(1-WPRMeasure/WPRTarget))*100
var RPATarget;	//  WordMatch in our case.
var wRPA;		// finalArray in our case.
var RPAScore; 	//RPA in our case
var asked;
var remainig = 0;
var completed = 0;
var completedIs = false;
var currentDT;
var minutes;
var seconds;
var endDT;
var checkCmdType;
var selectedObjections;
var currentSession;
var sessionObj = {};
var btnstate;
// creates an array to store all objections into. this will make it easier to reference the different objections later
var objections = new Array();
$("#collectionId").on("change", function () {

    objections = [];
    var colId = $("#collectionId").val();
    $.ajax({
        url: "/Speech/GetObjections",
        data: { colId: colId },
        type: "GET"
    }).done(function (res) {
        
        $("#microphones").click();
        if (res.objections.length !== 0) {
            for (var i = 0; i < res.objections.length; i++) {
                objections[i] = res.objections[i];
            }
        }
        setTimeout(function () {
            $("#microphones").click();
        }, 200);

    });

});
// keep track of WHICH objection the user is working with
// we can create objection2, etc, above to set-up multiple objection scripts (later, these will be in a DB)
var currentObjection = -1;
// temp note: changing variables from let to var to keep variable scope clean
// sets up the ability to hear the user through their microphone
const icon = document.querySelector('button.microphone');
const stop = document.querySelector('i.fa.fa-stop');
var paragraph = document.createElement('p');
var container = document.querySelector('.text-box');
const sound = document.querySelector('.sound');
container.appendChild(paragraph);
var listening = false;
var question = false;
// when called, activates the SpeechRecognition object so it starts listening
recognition.onstart = function () {
    listening = true;

    console.log('Speech recognition service has started');
};
// when called, stops listening to the microphone
recognition.onend = function () {
    recognition.stop();
    console.log('Speech recognition service disconnected');

};
recognition.onspeechend = function () {
    recognition.stop();
    //watch.stop();
    //console.log("Watch end on Speechend");
    console.log('Speech recognition has stopped.');
};
function objectionsList() {
    currentObjection = Math.floor(Math.random() * objections.length);

    // speak the objection to the user
    speak(getObjection(currentObjection));

    recognition.onend = recognition.stop();
    // capture the user's response
    dictate();

}
// creats a variable that hears the user
const dictate = () => {
    console.log('dictating');
    if (!listening) {
        // starts listening
        recognition.start();
    }
    // stores the dictated audio into the 'result' variable
    recognition.onresult = (event) => {
        console.log('on result');
        const speechToText = Array.from(event.results)
            .map(result => result[0])  // TODO: I'm not sure what this is doing since it is only looking at item 0 in the array - usually we would use a variable (like 'i') so the codes looks at every item in the array
            .map(result => result.transcript)
            .join('');
        WPR = wordCount(speechToText);
        //console.log("Word Per Response (WPR): " + WPR);
        // combines all of the things said into one block of text
        paragraph.textContent = speechToText;
        watch.stop();
        var minutes = watch.getMinutes();
        if (minutes === 0) {
            minutes = 1;
        }
        WPM = WPR / minutes;
        //console.log("Words Per Minutes: " + WPM);
        // adds a new visual elements to the screen
        if (event.results[0].isFinal) {
            container.scrollTo(0, container.scrollHeight);
            paragraph = document.createElement('p');
            container.prepend(paragraph);
            ////TODO: Add Ajax Call to Send speechToText in db..
            //==================================================
            //$.ajax({
            //    url: "/Speech/RecordSpeechToText",
            //    type: "POST",
            //    data: { speech: speechToText }
            //}).done(function (res) {
            //    if (res) {
            //        console.log("Speech Saved Successfully");
            //    }
            //});
            //===================================================
            // user initiates a random objection script
            if (speechToText.includes('give me objections for')) {
                checkCmdType = "";
                var textTobeSplitteds = speechToText.split(' ');
                var askedNumbers = textTobeSplitteds[4];
                if (parseInt(askedNumbers)) {
                    asked = parseInt(askedNumbers);
                    saveUserSession();
                    objectionsList();
                    showCounter();
                }
                else {
                    asked = text2num(askedNumbers);
                    saveUserSession();
                    objectionsList();
                    showCounter();
                }
            }
            else if (speechToText.includes('give me any objection')) {
                // get a random objection based on the number of objections in the 'objections' object
                objectionsList();
            }
            // user initiates 'objection1' objection script
            else if (speechToText.includes('give me objection one')) {
                // establish that we are working with 'objection1'
                currentObjection = 0;
                // speak the objection to the user
                speak(getObjection(currentObjection));
                // capture the user's response
                recognition.onend = recognition.stop();
                dictate();
            }
            else if (speechToText.includes('give me')) {
                checkCmdType = "give me";
                // var substr = speechToText.match('me')
                var subStr = speechToText.match("me(.*)objection");
                var textTobeSplitted;
                //alert(subStr[1]);
                //if (subStr[1] !== null) {
                textTobeSplitted = subStr[1];//speechToText.split(' ');
                // }
                textTobeSplitted = textTobeSplitted.trim();
                var askedNumber = textTobeSplitted;
                if (parseInt(askedNumber)) {
                    asked = parseInt(askedNumber);
                    remainig = asked;
                    saveUserSession();
                    objectionsList();
                    updatehtml();
                }
                else {
                    asked = text2num(askedNumber);
                    remainig = asked;
                    saveUserSession();
                    objectionsList();
                    updatehtml();
                    //selectedObjections = getAskedObjections(objections, asked);
                    //checkRandomObjections();
                }
            }
            else if (speechToText.includes('practicing')) {
                synth.cancel();
                endRecognition();
                asked = 0;
                var sessionEnd = new Date();
                sessionEnd = new Date(sessionEnd).toUTCString();
                sessionEnd = sessionEnd.split(' ').slice(0, 5).join(' ');
                $.ajax({
                    type: 'POST',
                    url: '/Speech/UpdateSession',
                    data: { sessionID: sessionObj.sessionID, sessionEnd: sessionEnd },
                    success: function (res) {

                        var speechToTexts = "Thank You";
                        speak(speechToTexts.toLocaleLowerCase());
                        $(".mcicon").removeClass("fa-microphone");
                        $(".mcicon").addClass("fa-microphone-slash");

                    },
                });
                if (checkCmdType === "give me") {
                    updatehtml();
                }
                else {
                    $("#showcounter").addClass('d-none');
                }
            }
            // conversation round 2
            //if (speechToText.includes('response')) {
            //speak(scoreResponse);
            else {
                //if (currentObjection > -1) {
                setTimeout(function () {
                    speak(scoreResponse(speechToText.toLocaleLowerCase()));
                    if ($("#randomVoices").prop("checked")) {
                        randomVoices();
                    }

                    if (completedIs) {
                        if (checkCmdType === "give me") {
                            remainig = remainig - 1;
                            completed = completed + 1;
                            xObjections();
                            updatehtml();
                        }
                        else {

                            if (minutes > 0 || seconds > 0) {

                                xObjections();
                            }
                            else {
                                synth.cancel();
                            }
                        }
                    }

                }, 2000);
                checkbtnPP();
                //endRecognition();
                //}
                return;
            }
        }
    };
    recognition.onend = recognition.start;
    // recognition.onend = recognition.stop;
    // recognition.start();
};
function showCounter() {

    currentDT = new Date();
    endDT = new Date(currentDT.getTime() + asked * 60000);
    //  var countDownDate = new Date("Dec 11, 2019 15:37:25").getTime();
    // Find the distance between now and the count down date
    $("#showcounter").removeClass('d-none');

    setInterval(function () {
        var now = new Date().getTime();
        var distance = endDT - now;
        // Get today's date and time


        // Time calculations for days, hours, minutes and seconds
        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        seconds = Math.floor((distance % (1000 * 60)) / 1000);

        // Output the result in an element with id="demo"
        document.getElementById("demo").innerHTML = "Time Left: " + minutes + "m " + seconds + "s ";

        if (minutes < 0 || seconds < 0) {
            synth.cancel();
            endRecognition();
            clearInterval();
            document.getElementById("demo").innerHTML = "EXPIRED";
            $("#showcounter").addClass('d-none');
        }
    }, 1000);
};
function updatehtml() {
    if (checkCmdType === "give me") {

        if (asked >= completed) {
            $("#asked").html('<strong>Asked Objections </strong>' + asked);
            $("#remainig").html('<strong>Remaining Objections </strong>' + remainig);
            $("#completed").html('<strong>Completed Objections </strong>' + completed);

            $("#askedobj").removeClass('d-none');
        }
        else {
            $("#asked").html('');
            $("#remainig").html('');
            $("#completed").html('');
            $("#askedobj").addClass('d-none');
        }
    }
}
function xObjections() {
    setTimeout(function () {
        if (asked >= completed) {
            objectionsList();
        }
        else if (minutes > 0 || seconds > 0) {

            objectionsList();
        }
        else {
            document.getElementById("demo").innerHTML = "EXPIRED";
            // $("#showcounter").addClass('d-none');
            synth.cancel();
        }
    }, 3000);

}
function checkRandomObjections() {
    if (selectedObjections.length > 0) {
        setTimeout(function () {
            var ind = Math.floor(Math.random() * selectedObjections.length);
            currentObjection = selectedObjections[ind].sourceIndex;

            selectedObjections.splice(ind, 1);
            // speak the objection to the user
            speak(getObjection(currentObjection));
            recognition.onend = recognition.stop();
            // capture the user's response
            dictate();

        }, 3000);
    }
};
// receives the index of an objection and returns the phrase to present the objection to the user
function getObjection(objectionIndex) {
    return objections[objectionIndex].initialObjection;
};
function getAskedObjections(sourceArray, neededElements) {
    var result = [];
    for (var i = 0; i < neededElements; i++) {
        var randomNumber = Math.floor(Math.random(), sourceArray.length);
        sourceArray[randomNumber].sourceIndex = randomNumber;
        result.push(sourceArray[randomNumber]);
    }
    return result;
};
function serializeData() {
    WPMMeasure = WPM;
    WPRMeasure = wordSpoken.length; WPM
    WPRTarget = wordsMatch.length;
    WPMScore = (1 - Math.abs(1 - (WPMMeasure / WPMTargert))) * 100;
    WPRScore = (1 - (Math.abs(1 - WPRMeasure / WPRTarget))) * 100;
    TotalScore = (WPMScore + WPRScore + RPA) / 3;

    obj = {
        //id: arrayTobeSaved.length + 1,
        objectionId: objections[currentObjection].id,
        //WordPerMinutes: WPM,
        //WordPerResponse: WPR,
        //RPATarget: wordsMatch.length,
        //RPAs: finalArray.length,
        //RPAScore: RPA,
        //DateTime: new Date()
        WPMTargert: 140,
        TotalScore: TotalScore.toFixed(0),
        WPMMeasure: WPM,
        WPMScore: WPMScore.toFixed(0),
        WPRTarget: wordsMatch.length,
        WPRMeasure: wordSpoken.length,
        WPRScore: WPRScore.toFixed(0),// (1 - (Math.abs(1 - WPRMeasure / WPRTarget))) * 100,
        RPATarget: wordsMatch.length,
        RPA: finalArray.length,
        RPAScore: RPA.toFixed(0)
    };
    arrayTobeSaved = obj;
    console.log("object: " + arrayTobeSaved);
}
function compare(wordsMatch, wordSpoken) {

    wordsMatch.forEach((e1) => wordSpoken.forEach((e2) => {
        if (e1.split('.')[0] === e2) {
            finalArray.push(e1);
        }
    }
    ));
};
// round 2- Option A
const scoreResponse = (speechToText) => {
    wordSpoken = speechToText.split(' ');
    var word = objections[currentObjection].pitchKeyword.trim().split(',');
    wordsMatch = word.join(' ').toLocaleLowerCase();
    wordsMatch = wordsMatch.split(' ');
    compare(wordsMatch, wordSpoken);
    RPA = ((finalArray.length / wordsMatch.length) * 100);
    // console.log("Response Phrase Accuracy (RPA): " + RPA.toFixed(2) + "%");
    //Check if the keywords are multiple
    if (word.length > 1) {
        for (var i = 0; i < word.length; i++) {
            if (speechToText.includes(word[i])) {
                serializeData();
                endRecognition();
                LogObjections(objections[currentObjection].id, true);
                return objections[currentObjection].validPitchResponse;
            }
        }
        //if no keyword matches with speechToText
        serializeData();
        //endRecognition();
        LogObjections(objections[currentObjection].id, false);
        return objections[currentObjection].badPitchResponse;
    }
    //if keyword is single.
    else {
        if (speechToText.includes(objections[currentObjection].pitchKeyword.trim().toLocaleLowerCase())) {
            serializeData();
            //endRecognition();
            LogObjections(objections[currentObjection].id, true);
            return objections[currentObjection].validPitchResponse;
        }
        else {
            serializeData();
            //endRecognition();
            LogObjections(objections[currentObjection].id, false);
            return objections[currentObjection].badPitchResponse;
        }
    }
};
function saveUserSession() {

    var sessionStart = new Date();
    sessionStart = new Date(sessionStart).toUTCString();
    sessionStart = sessionStart.split(' ').slice(0, 5).join(' ');

    $.ajax({
        type: "POST",
        url: "/Speech/SaveSession",
        data: { sessionStart: sessionStart },
        success: function (res) {
            if (res.success) {
                sessionObj = res.model;
            }
        }

    });
}
//Function to Log Objections in DB, whether it is completed or not. 
function LogObjections(objectionId, isCompleted) {
   // endRecognition();
    var sessionId = sessionObj.sessionID;
    var formdata = new FormData();
    formdata.append("objectionId", objectionId);
    formdata.append("isCompleted", isCompleted);
    formdata.append("arrayTobeSaved", arrayTobeSaved);
    //var lastVoice = $("#voiceSelect").val();
    completedIs = isCompleted;
    //
    var colId = $('#collectionId').val();
    var Dialect = $('#voiceSelect').text();
    var DialectDataName = $('#voiceSelect option:selected').attr("data-name");
    var DialectDataId = $('#voiceSelect option:selected').attr("data-id");
    $.ajax({
        url: "/Speech/LogObjections",
        type: "POST",

        data: {
            objectionId: objectionId, isCompleted: isCompleted, arrayTobeSaved: arrayTobeSaved, sessionId: sessionId,
            CollectionId: colId, Dialect: Dialect, DialectDataName: DialectDataName, DialectDataId: DialectDataId
        }
    }).done(function (res) {
        if (res) {
            $.ajax({
                url: "/Speech/GetUpdatedObjections",
                type: "GET",
                data: { IsCompleted: true, IsFromAdmin: false },
                success: function (response) {
                    $("#CompletedObjections").html(response);
                },
                complete: function (response) {
                    $('.momentTime').each(function () {
                        var d = moment($(this).html(), "YYYYMMDDHHmmss").fromNow();
                        $(this).html(d);
                    });
                }
            });

            $.ajax({
                url: "/Speech/GetUpdatedObjections",
                type: "GET",
                data: { IsCompleted: false },
                success: function (response) {
                    $("#MissedObjections").html(response);
                },
                complete: function (response) {
                    $('.momentTime').each(function () {
                        var d = moment($(this).html(), "YYYYMMDDHHmmss").fromNow();
                        $(this).html(d);
                    });
                }
            });

            console.log("Objection Saved Successfully");
        }
        else {
            console.log("Error Occurred  While Saving Objection");
        }
    });
}
const speak = (action) => {
    watch.stop();
    //console.log("Minutes: " + watch.getMinutes());
    //console.log("Seconds: " + watch.getSeconds());
    utterThis = new SpeechSynthesisUtterance(action);
    setVoice(utterThis);
    synth.speak(utterThis);
    utterThis.onstart = function (event) {

        if ($(".btnply").hasClass("fa-play")) {
            $(".btnply").removeClass("fa-play").addClass("fa-pause");
            $(".play").attr("disabled", false);
            $("#viewObjections").prop("disabled", true);
        }


        $(".microphone").attr("disabled", true);
        // $(".mcicon").addClass("fa-microphone-slash");
        //$(".mcicon").removeClass("fa-microphone");
        //  recognition.onend = recognition.stop();
    };
    utterThis.onend = function (event) {
        //if (synth.speaking == true) {
        synth.cancel();
     

        //}
        $(".btnply").removeClass("fa-pause").addClass("fa-play");
        $(".play").attr("disabled", true);
        $(".microphone").attr("disabled", false);
        $("#viewObjections").prop("disabled", false);
    };
};
$(".play").on("click", function () {
    if ($(".btnply").hasClass("fa-play")) {
        $(".btnply").removeClass("fa-play").addClass("fa-pause");
        synth.resume();

        if (synth.speaking === false) {
            $(".btnply").removeClass("fa-pause").addClass("fa-play");
            $(".play").attr("disabled", true);
            $(".microphone").attr("disabled", false);
            synth.cancel();

        }

        // get a random objection based on the number of objections in the 'objections' object


        // currentObjection = Math.floor(Math.random() * objections.length);

        // speak the objection to the user
        //  speak(getObjection(currentObjection));
        //recognition.start();
        // capture the user's response
        //   dictate();

        console.log("Play is working");
    }
    else {
        $(".btnply").removeClass("fa-pause").addClass("fa-play");
        synth.pause();
    }

});
$("#randomVoices").on("change", function () {
    if ($(this).prop("checked")) {
        $("#voiceSelect").prop("disabled", true);
        randomVoices();
    }
    else {
        $("#voiceSelect").prop("disabled", false);
    }
});
function randomVoices() {
    var $options = $('#voiceSelect').find('option');
    var random = ~~(Math.random() * $options.length);

    $("#voiceSelect > option").attr('selected', false).eq(random).attr('selected', true);
};
$(".btnpp").on("click", function () {
    $(".fa-stop").removeClass("d-none");
    if ($(this).hasClass("fa-play")) {
        $(this).removeClass("fa-play");
        $(this).addClass("fa-pause");

        // get a random objection based on the number of objections in the 'objections' object
        currentObjection = Math.floor(Math.random() * objections.length);

        // speak the objection to the user
        speak(getObjection(currentObjection));
        recognition.start();
        // capture the user's response
        dictate();

        console.log("Play is working");

    }
    else {

        $(this).removeClass("fa-pause");
        $(this).addClass("fa-play");
        synth.cancel();
        endRecognition();
        //console.log("Pause is working");

    }
});
function endRecognition() {
    recognition.onend = recognition.stop();
}
function checkbtnPP() {
    $(".btnplay").removeClass("fa-pause").addClass("fa-play");
}
// TODO: remove this old sample code if it isn't useful anymore
const stripUrl = (str) => {
    return str.match(/[a-z]+[:.].*?(?=\s)/);
};
icon.addEventListener('click', () => {

    if ($(".mcicon").hasClass("fa-microphone-slash")) {
        $(".mcicon").removeClass("fa-microphone-slash");
        $(".mcicon").addClass("fa-microphone");
        recognition.start();

        console.log("Mic click Watch start");
        if (!listening) {
            // recognition.start();
            return;
        }
        //sound.play();
        dictate();
    }
    else {
        $(".mcicon").removeClass("fa-microphone");
        $(".mcicon").addClass("fa-microphone-slash");
        recognition.onend = recognition.stop();
        watch.stop();
        console.log("Mic click Watch stop");
    }
});
function populateVoiceList() {
    voices = speechSynthesis.getVoices();
    console.log(voices);

    if (typeof speechSynthesis === 'undefined') {

        return;
    }
    
    for (i = 0; i < voices.length; i++) {
        var option = document.createElement('option');
        option.textContent = voices[i].name + ' (' + voices[i].lang + ')';
        if (voices[i].default) {
            option.textContent += ' -- DEFAULT';
        }
        option.setAttribute('data-lang', voices[i].lang);
        option.setAttribute('data-name', voices[i].name);
        option.setAttribute('data-id', i);
        document.getElementById("voiceSelect").appendChild(option);
    }
    //var CollectionId = "option[value=@Model.LastUserHistory.CollectionId]";
    
    GetObjectionByCollectionId();
}
populateVoiceList();
//// Chrome loads voices asynchronously.
//window.speechSynthesis.onvoiceschanged = function (e) {

//    populateVoiceList();
//    alert(populateVoiceList());
//};
if (typeof speechSynthesis !== 'undefined' && speechSynthesis.onvoiceschanged !== undefined) {
    speechSynthesis.onvoiceschanged = populateVoiceList;
}
if (voiceSelect.value) {
    var selectedVoice = speechSynthesis.getVoices().filter(function (voice) { return voice.voiceURI === voiceSelect.value; })[0];
    voice.voiceURI = selectedVoice.voiceURI;
    voice.lang = selectedVoice.lang;
}
//else {
//    if (voiceSelect.value) {
//        var selectedVoice = speechSynthesis.getVoices().filter(function (voice) { return voice.voiceURI === voiceSelect.value; })[0];
//        voice.voiceURI = selectedVoice.voiceURI;
//        voice.lang = selectedVoice.lang;
//    }
//}
const setVoice = (utterThis) => {

    const selectedOption = voiceSelect.selectedOptions[0].getAttribute('data-name');
    for (i = 0; i < voices.length; i++) {
        if (voices[i].name === selectedOption) {
            utterThis.voice = voices[i];

        }
    }
    if (selectedOption.value) {
        var selectedVoice = speechSynthesis.getVoices().filter(function (voice) { return voice.voiceURI === selectedOption.value; })[0];
        voice.voiceURI = selectedOption.voiceURI;
        voice.lang = selectedVoice.lang;

        //utterThis.voice = voices[voice];
    }
};
function Stopwatch() {

    var startTime, endTime, instance = this;

    this.start = function () {

        startTime = new Date();
    };

    this.stop = function () {

        endTime = new Date();
    };

    this.clear = function () {
        startTime = null;
        endTime = null;
    };

    this.getSeconds = function () {

        // return non-numerical value to indicate timer was not started
        if (!startTime) {
            return null;
        }
        if (!endTime) {
            return 0;
        }

        return Math.round((endTime.getTime() - startTime.getTime()) / 1000);
    };

    this.getMinutes = function () {


        return instance.getSeconds() / 60;
    };
    this.getHours = function () {
        return instance.getSeconds() / 60 / 60;
    };
    this.getDays = function () {
        return instance.getHours() / 24;
    };
}
function wordCount(s) {
    //testWords = (jQuery(text).text().length) / 5;
    //return testWords;
    s = s.replace(/(^\s*)|(\s*$)/gi, "");//exclude  start and end white-space
    s = s.replace(/[ ]{2,}/gi, " ");//2 or more space to 1
    s = s.replace(/\n /, "\n"); // exclude newline with a start spacing
    return s.split(' ').filter(function (str) { return str !== ""; }).length;
    //return s.split(' ').filter(String).length;
}
var Small = {
    'zero': 0,
    'one': 1,
    'two': 2,
    'three': 3,
    'four': 4,
    'five': 5,
    'six': 6,
    'seven': 7,
    'eight': 8,
    'nine': 9,
    'ten': 10,
    'eleven': 11,
    'twelve': 12,
    'thirteen': 13,
    'fourteen': 14,
    'fifteen': 15,
    'sixteen': 16,
    'seventeen': 17,
    'eighteen': 18,
    'nineteen': 19,
    'twenty': 20,
    'thirty': 30,
    'forty': 40,
    'fifty': 50,
    'sixty': 60,
    'seventy': 70,
    'eighty': 80,
    'ninety': 90
};
var Magnitude = {
    'thousand': 1000,
    'million': 1000000,
    'billion': 1000000000,
    'trillion': 1000000000000,
    'quadrillion': 1000000000000000,
    'quintillion': 1000000000000000000,
    'sextillion': 1000000000000000000000,
    'septillion': 1000000000000000000000000,
    'octillion': 1000000000000000000000000000,
    'nonillion': 1000000000000000000000000000000,
    'decillion': 1000000000000000000000000000000000,
};
var a, n, g;
function text2num(s) {
    a = s.toString().split(/[\s-]+/);
    n = 0;
    g = 0;
    a.forEach(feach);
    return n + g;
}
function feach(w) {
    var x = Small[w];
    if (x !== null) {
        g = g + x;
    }
    else if (w === "hundred") {
        g = g * 100;
    }
    else {
        x = Magnitude[w];
        if (x !== null) {
            n = n + g * x;
            g = 0;
        }
        else {
            alert("Unknown number: " + w);
        }
    }
}
$(document).on("click", "#btnClose", function () {
    if (btnstate === "fa-microphone") {
        $(".mcicon").addClass("fa-microphone");
        $(".mcicon").removeClass("fa-microphone-slash");
        recognition.start();
    }
    else {
        $(".mcicon").removeClass("fa-microphone");
        $(".mcicon").addClass("fa-microphone-slash");
        endRecognition();
    }
});
//On User Collection Change
$(document).on("change", "#collectionId", function () {
    var colId = $(this).val();
    var Dialect = $('#voiceSelect').text();
    var DialectDataName = $('#voiceSelect option:selected').attr("data-name");
    var DialectDataId = $('#voiceSelect option:selected').attr("data-id");
    $.ajax({
        url: "/Admin/Speech/LogObjections",
        data: { CollectionId: colId, Dialect: Dialect, DialectDataName: DialectDataName, DialectDataId: DialectDataId },
        type: "Post",
        success: function (res) {

        }
    });
});
$(document).on("change", "#voiceSelect", function () {
    var colId = $('#collectionId').val();
    var Dialect = $('#voiceSelect').text();
    var DialectDataName = $('#voiceSelect option:selected').attr("data-name");
    var DialectDataId = $('#voiceSelect option:selected').attr("data-id");
    $.ajax({
        url: "/Admin/Speech/LogObjections",
        data: { CollectionId: colId, Dialect: Dialect, DialectDataName: DialectDataName, DialectDataId: DialectDataId },
        type: "Post",
        success: function (res) {
        }
    });
})
function GetObjectionByCollectionId() {
    objections = [];
    var colId;
    if (ColId !== undefined) {
        colId = ColId;
    }
    else {
         colId = '';
    }
    
    
    if (colId !=='') {
        $.ajax({
            url: "/Speech/GetObjections",
            data: { colId: colId },
            type: "GET"
        }).done(function (res) {

            $("#microphones").click();
            if (res.objections.length !== 0) {
                for (var i = 0; i < res.objections.length; i++) {
                    objections[i] = res.objections[i];
                }
            }
            setTimeout(function () {
                $("#microphones").click();
            }, 200);
        });
    }
}