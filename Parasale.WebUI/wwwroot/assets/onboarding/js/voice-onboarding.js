var recognitions;
var synths;
//var UserInfo;
var FirstLogin = "FirstLogin";
var step1;
var step2; var UserInfo = 'UserInfo';
var VOBData = 'VOBData';
if (IsMainJsExist !== "true") {
    synths = window.speechSynthesis;
}
const iicon = document.querySelector('button#autoplay');
const iicons = document.querySelector('button#getStarted');
var paragraphs = document.createElement('p');
var containers;
//recognition.start();
var listening = true;
var msgToSpeech;
var directions;
var tour;
var datas = [];
var urlArray = [];
var directionsYes;
var currentStep;
var totalSteps = 45;
var isSaved;
var stepTitle;
var stepLevel = 1;
var isSavedforLater;
var SessionStep = "step", IsSessionSaved = "isSaved", IsSessionDirect = "isDirect", IsSessionPaused;
var SessionSubStep = "subStep";
var StartPopUp = "isStartPopUp";
var StepName = "title";
var IsGranted = "isGranted";
iicon.addEventListener('click', () => {
    dictates();
    doSpeech();
});
function RecognitionCredentials() {

    window.SpeechRecognition = window.SpeechRecognition || webkitSpeechRecognition;
    recognitions = new SpeechRecognition();
    recognitions.interimResults = true;
    //recognitions.continuous = false;
    recognitions.start();
    recognitions.onstart = function () {
        listening = true;
    };
}
function doSpeech() {
    synths = window.speechSynthesis;
    synths.cancel();

    var msg = new SpeechSynthesisUtterance();
    msg.text = msgToSpeech;
    msg.lang = 'en-US';
    msg.addEventListener('start', function () {
        if (synths.speaking) {
            console.log("This will be printed !");
        }
        else {
            synths.speak(msg);
        }
    });
    //if (confirm("Click OK to speak")) {
    synths.speak(msg);
    msg.onend = function (event) {
        //recognition.start();
        synths.cancel();
    };
}
function doSpeechFromToursJs(speechtext) {
    setTimeout(function () {
        //synths.cancel()
        synths = window.speechSynthesis;
        var msg = new SpeechSynthesisUtterance();
        msg.text = speechtext;
        msg.lang = 'en-US';
        msg.addEventListener('start', function () {
            if (synths.speaking) {
                console.log("This will be printed !");
            }
            else {
                synths.speak(msg);
            }
        });
        synths.speak(msg);
        msg.onend = function (event) {
            synths.cancel();
        };
    }, 200);

}
function doSpeechFromToursJsFirstTime(speechtext) {
    setTimeout(function () {
        synths = window.speechSynthesis;
        var msg = new SpeechSynthesisUtterance();
        msg.text = speechtext;
        msg.lang = 'en-US';
        //msg.addEventListener('start', function () {
        //    if (synths.speaking) {
        //        console.log("This will be printed !");
        //    }
        //    else {
        //        synths.speak(msg);
        //    }
        //});
        //if (synths.speaking) {            
            //synths.cancel()
            synths.speak(msg);
        //}
        //else {
        //    synths.speak(msg);
        //}
        //try {
        //    //synths.cancel()
        //    if (synths.speaking == true) {
        //        synths.cancel()
        //        synths.speak(msg);
        //    }
        //    else {
        //        synths.speak(msg);
        //    }
           
        //}
        //catch (ex) {
           
        //}
        msg.onend = function (event) {
            synths.cancel();
        };
    }, 2000);

}
function finishTour() {
    recognitions.stop();
    $.ajax({
        url: '/Speech/FinishTour/',
        success: function (res) {
            if (res) {
                $.notify("Tour is completed.", success);
            }
        }
    });
}
function getSessionStorageValue(key) {
    //return (sessionStorage.getItem(key) !== null) ? sessionStorage.getItem(key) : null;
    return (localStorage.getItem(key) !== null) ? localStorage.getItem(key) : null;
}
function setSessionStorageItem(key, value) {
    //sessionStorage.setItem(key, value);
    localStorage.setItem(key, value);
}
function removeKeyFromSessionStorage(key) {
    if (localStorage.getItem(key) !== null)
        localStorage.removeItem(key);
}
function checkSteps() {
    currentStep = getSessionStorageValue(SessionStep);
    currentStep = currentStep;
    isSavedforLater = getSessionStorageValue(IsSessionSaved);
    if (isSavedforLater === "false") {
        var VOBStepsData = getSessionStorageValue(VOBData);
        datas = JSON.parse(VOBStepsData);
    }
}
//function clickevent() {
$(document.body).on("click", ".goToStep", function () {
    synths.cancel();
    $("#CreateNewModal").modal('hide');
    var step = $(this).attr('id');
    step = parseInt(step);
    if (step === 2) {
        showVoBPopUp(datas, 2);
      
    }
    removeSessionValues();
    createSession(step, false, "direct");
    checkSteps();
    //$(".next").addClass('Hidden');
    //$(".prev").addClass('Hidden');
    //$(".ttour-bullets").addClass('Hidden');
    //voiceOnboarding(datas, true);
    //$(".next").click();
});
function timeOut() {
    setTimeout(function () {
        $("#autoplay").click();
    }, 2000);
}
$(document).ready(function () {
    GetUser();
    GetVoiceOnBoardingSteps();
    //Progress bar
    $(window).on('beforeunload', function () {
        synths.cancel();
    });
});
window.addEventListener('load', function () {
    //if (IsStartUpPopUP != "true") {
    //console.log("1");
    //checkSteps();
    //voiceOnboarding(datas);
    //setTimeout(function () {
    //    doSpeech()
    //}, 15000);
    //console.log("2");
    //$("#CreateNewModal").modal('hide');
    //$(".ttour-shadow").css('display', 'block');
    //}
})
$(".ppVoice").on("click", function () {
    if ($(this).text === "Play") {
        $(this).text === "Pause";
        synths.resume();
    }
    else {
        $(this).text === "Play";
        endRecognitions();
        synth.pause();
    }
});
$(document).on('click', '.ppVoice', function () {
    if ($(this).text === "Play") {
        $(this).text === "Pause";
        synths.pause();
    }
    else {
        $(this).text === "Play";
        synth.resume();
    }
});
function OnPause() {
    document.cookie = 'Ispause' + "=" + 'yes';
}
function OnPlay() {
    document.cookie = 'Ispause' + "=" + '';
}
const dictates = () => {
    console.log('dictating');
    if (!listening) {
        // starts listening
        recognitions.start();
    }

    // stores the dictated audio into the 'result' variable
    recognitions.onresult = (evenst) => {
        console.log('on result');
        const speechToText = Array.from(event.results)
            .map(result => result[0])  // TODO: I'm not sure what this is doing since it is only looking at item 0 in the array - usually we would use a variable (like 'i') so the codes looks at every item in the array
            .map(result => result.transcript)
            .join('');
        //console.log("Words Per Minutes: " + WPM);
        directions = speechToText;
        paragraphs.textContent = speechToText;
        // adds a new visual elements to the screen
        if (event.results[0].isFinal) {
            paragraphs = document.createElement('p');
            containers.prepend(paragraphs);
            // user initiates a random objection script
            if (speechToText.includes('practicing objections')) {

                // doTovoice(2);

                showVoBPopUp(datas, 2);
            }
            else if (speechToText.includes('installing quick start collections')) {
                doTovoice(3);
            }
            else if (speechToText.includes('installing Quickstart collections')) {
                doTovoice(4);
            }
            else if (speechToText.includes('reporting')) {
                doTovoice(27);
            }
            else if (speechToText.includes('building collections and objections')) {
                doTovoice(6);
            }
            else if (speechToText.includes('repeat step')) {
                doSpeech();
            }
            else if (speechToText.includes('making a user a manager')) {
                doTovoice(18);
            }
            else if (speechToText.includes('onboarding users and creating teams')) {
                doTovoice(14);
            }
            else if (speechToText.includes('yes')) {
                if (directionsYes === "addToCollections") {
                    doTovoice(5);
                }
                if (directionsYes === "building collections and objections") {
                    doTovoice(7);
                }
                if (directionsYes === "CollectionSegment") {
                    directions = '';
                    doTovoice(71);
                }
                if (directionsYes === "userinvites") {
                    doTovoice(16);
                }
                if (directionsYes === "addNewCollections") {
                    doTovoice(8);
                }
                if (directionsYes === "firstCollection") {
                    doTovoice(9);
                }
                if (directionsYes === "collectionsView") {
                    doTovoice(10);
                };
                if (directionsYes === 'DoneObjections') {
                    doTovoice(11);
                }
                if (directionsYes === 'microphone') {
                    doTovoice(13);
                }
                if (directionsYes === 'user manager') {
                    doTovoice(19);
                }
                if (directionsYes === 'assignToTeam') {
                    doTovoice(20);
                }
                if (directionsYes === 'doneAssigning') {
                    doTovoice(21);
                }
                if (directionsYes === 'teamView') {
                    doTovoice(22);
                }
                if (directionsYes === "teamYes") {
                    doTovoice(24);
                }
                if (directionsYes === "userTeamClicked") {
                    doTovoice(25);
                }
                if (directionsYes === 'concept') {
                    doTovoice(26);
                }
                if (directions === "dashboardReports") {
                    doTovoice(28);
                }
                if (directionsYes === "Seedates") {
                    doTovoice(29);
                }
                if (directionsYes === "clickdates") {
                    doTovoice(30);
                }
                if (directionsYes === "datecomfort") {
                    doTovoice(31);
                }
                if (directionsYes === "toggles") {
                    doTovoice(32);
                }
                if (directionsYes === "toggleComfort") {
                    doTovoice(33);
                }
                if (directionsYes === "filterReports") {
                    doTovoice(34);
                }
                if (directionsYes === "filterUsers") {
                    doTovoice(35);

                }
                if (directionsYes === "CCSRep") {
                    doTovoice(36);

                }
                if (directionsYes === "infoicn") {

                    doTovoice(73);
                }
                if (directionsYes === 'auditLogs') {
                    doTovoice(43);
                }
                if (directionsYes === 'auditCompleted') {
                    doTovoice(44);
                }
                if (directionsYes === 'quizYes') {
                    doTovoice(45);
                }
                if (directionsYes === 'viewTeam') {
                    msgToSpeech = 'Confirmed.   This   is   where   you   can   add,   or   subtract   users   from   a   managers   team.   Simply click   the   checkbox   to   uncheck   a   user   from   a   team';
                    datas: [{
                        element: '.ctmCheckbox',
                        title: 'Manage Team',
                        description: msgToSpeech,
                        data: 'custom Data',
                        position: 'right'
                    }]
                    doSpeech();
                    voiceOnboarding(datas);
                    $(".next").click();
                    setTimeout(function () {
                        msgToSpeech = '';
                        msgToSpeech = 'Have   you   clicked   the   name?   Yes,   or   No';
                        doSpeech();
                    }, 6000);
                }
                if (directionsYes === "reportsYes") {
                    doTovoice(40);
                }
                if (directionsYes === "menuReports") {
                    doTovoice(41);
                }
                if (irectionsYes === "ccsComfort") {

                    doTovoice(38);
                }
            }
            else if (speechToText.includes('no')) {
                if (directionsYes === "CollectionSegment") {
                    doTovoice(1);
                }
                if (directionsYes === 'quizYes') {
                    doTovoice(45);
                }
            }
            else {
                return;
            }
        }
    };
    recognitions.onend = recognitions.start;
};
function doTovoice(step) {

    removeSessionValues();
    createSession(step, false, "");
    checkSteps();
    dictates();
    $(".next").click();
}
function voiceOnboarding(datas, isOverlayExist) {
    tour = window.tour = new Tour({
        padding: 0,
        nextText: 'More',
        doneText: 'Done',
        prevText: 'Less',
        tipClasses: 'tip-class active',
        steps: datas
    })
    tour.override('showStep', function (self, step) {
        self(step);
    })
    tour.override('end', function (self, step) {
        self(step);
    });
    tour.start();
}
function endRecognitions() {
    recognitions.onend = recognitions.stop();
}
function appendHtml() {
    var btnHtml = '';
    btnHtml += '<button id="ppVoice" class="ppVoice btn btn-danger"><i class="zmdi zmdi-mic mphone"></i></button>';
    btnHtml += '<button id="skip" class="skip btn btn-light">Save for Later</button>';
    $('.ttour-footer').prepend(btnHtml);
    $(".ppVoice").on("click", function () {
        if ($(".mphone").hasClass("zmdi-mic")) {
            $(".mphone").removeClass("zmdi-mic");
            $(".mphone").addClass("zmdi-mic-off");
            synths.pause();
            endRecognitions();
            OnPause();
        }
        else {
            $(".mphone").addClass("zmdi-mic");
            $(".mphone").removeClass("zmdi-mic-off");
            synths.resume();
            recognitions.start();
            OnPlay();
        }

    });
}
function removeSessionValues() {
    removeKeyFromSessionStorage(SessionStep);
    removeKeyFromSessionStorage(IsSessionDirect);
    removeKeyFromSessionStorage(isSavedforLater);
}
function createSession(step, isSaved, isDirect) {
    setSessionStorageItem(SessionStep, step);
    setSessionStorageItem(IsSessionSaved, isSaved);
    setSessionStorageItem(IsSessionDirect, isDirect);
}
function MoveToUrl(classNameWith, Step) {
    var href = $(classNameWith + ' a').attr("href");
    if (urlArray.indexOf(href) < 0)
        urlArray.push(href);
    setSessionStorageItem("urlArray", urlArray);
    window.location.href = href;
    removeKeyFromSessionStorage(SessionStep);
    setSessionStorageItem(SessionStep, Step);

}
function AddCollectionDemo(addCollections) {
    $(addCollections).click()
    $('.next').click();
}
function Clicknext() {
    $('.next').click();
}
function ClicknextAndHideModal() {
    $('#CreateNewModal').remove()
    $('.next').click();
}
function ClickAddCollection(createbtn) {
    $(createbtn).click();
}
//Custom Functions
function LoadPopUp(htmlf, percentagelevel) {
    $("#CreateNewModal-label").html("Voice onboarding");
    var html = '';
    html += '<div class="row">';
    html += '<div class="col-md-12 text-center">';
    html += '<span class="text-danger">Please ensure your speakers are on.<br>Test your microphone, please say a few words.</span>';
    html += '</div>';
    html += '<div class="text-boxs" contenteditable="false"></div>';
    html += '<div class="col-md-12 mb-3"><div class="progress" id="progress"><div class="progress-bar" role="progressbar" style="width: 1%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">0%</div></div></div>';
    html += '<div class="col-md-12">';
    html += '<span> Estimated Completion Time: <b>30-45 minutes.</b></span><br>';
    html += '<span> Click the button to get started with demo.</span><br>';
    html += '<div class="text-right" id="htmlAppend">' + htmlf;
    html += '</div>';
    html += '</div>';
    html += '</div>';
    $("#CreateNewBody").html(html);
    $("#CreateNewModal").modal('show');
    if (percentagelevel === null || percentagelevel === undefined) {
        percentagelevel = 0;
        GetPercentage(percentagelevel);
    }
    else {
        GetPercentage(percentagelevel);
    }
    var paragraphs = document.createElement('p');
    containers = document.querySelector('.text-boxs');
    //containers.appendChild(paragraphs);
    //setTimeout(function () {

    //    dictates();
    //}, 2000);

}

function GetVoiceOnBoardingSteps() {
    $.ajax({
        url: '/Speech/GetVoiceOnboardings/',
        success: function (res) {
            datas = res;
            setSessionStorageItem(VOBData, JSON.stringify(datas))
        }
    });
}

function micPermission() {

    var htm = '';
    if (datas.length !== 0) {
        for (var i = 0; i < datas.length; i++) {
            if (datas[i].step === 0) {
                step1 = datas[i].description;
                if (step1 === '' || step1 === null || step1 === undefined) {
                    $("#CreateNewModal-label").html("Tutorial");
                    htm = '<div class="row"><div class="col-md-12 text-center"> <span>There are no steps Defined for tutorial. Please come back later..</span></div><div class="text-right col-md-12"><button class="btn btn-success btn-sm" id="tutorialOK">OK</button></div></div>';

                }
                else {
                    $("#CreateNewModal-label").html("Mic Permission");
                    htm = '<div class="row"><div> <p>' + step1 + '</p></div><div class="text-right col-md-12"><button class="btn btn-danger btn-sm mr-2" id="cancelPermission">Cancel</button><button class="btn btn-primary btn-sm" id="grantPermission"> Grant Permission</button></div></div>';
                }
            }
            if (datas[i].step === 1) {

                step2 = datas[i].description;

            }
        }
        $("#CreateNewBody").html(htm);
        $("#CreateNewModal").modal('show');
        var intromsg = "Welcome   to   Parasale.  My   name   is   Para   I   am   your   self   onboarding   agent.   We   are   so   glad   to   have   you.   Please   click  grant   permission   to   grant   permission   for   our   program  t o   use   your   microphone";
        doSpeechFromToursJsFirstTime(intromsg);
    }
    else {
        $.getJSON("/Speech/GetVoiceOnboardings", function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].step === 1) {
                    step2 = data[i].description;
                    console.log(step2);
                }
                if (data[i].step === 0) {
                    step1 = data[i].description;
                    htm = '<div class="row"><div> <p>' + step1 + '</p></div><div class="text-right col-md-12"><button class="btn btn-danger btn-sm mr-2" id="cancelPermission">Cancel</button><button class="btn btn-primary btn-sm" id="grantPermission"> Grant Permission</button></div></div>';
                }
            }
            $("#CreateNewBody").html(htm);
            $("#CreateNewModal").modal('show');
            var intromsg = "Welcome   to   Parasale.  My   name   is   Para   I   am   your   self   onboarding   agent.   We   are   so   glad   to   have   you.   Please   click  grant   permission   to   grant   permission   for   our   program  t o   use   your   microphone";
            doSpeechFromToursJsFirstTime(intromsg)
        });
    }
};

//$(document).on("click", "#continueWhereLeft", function () {
//    $(".ttour-shadow").css('display', 'block');
//    checkSteps();
//    voiceOnboarding(datas);
//    $("#CreateNewModal").modal('hide');
//    var User = GetUser();
//});
$(document).on("click", "#cancelPermission", function () {
    $("#CreateNewModal").modal('hide');
    synths.cancel();
    //var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Get Started</button>';
    //LoadPopUp(htmlf, 0);
});
$(document).on("click", "#tutorialOK", function () {
    $("#CreateNewModal").modal('hide');
});
$(document).on("click", "#grantPermission", function () {
    synths.cancel();
    $("#CreateNewModal").modal('hide');
    setSessionStorageItem(IsGranted, true);
    RecognitionCredentials();

    ///recognitions.start();
    $.getJSON("/Speech/GetUser", function (data) {
        User = data;
        var cs;
        var ProgressPercentage;
        var ProgressLevel;
        var StepTitle;
        data.currentStep = parseInt(data.currentStep);
        if (data.currentStep > 1) {
            ProgressLevel = parseInt(data.stepLevel);
            cs = parseInt(data.currentStep);
            $("#getStarted").text('');
            $("#getStarted").text('Restart');
            var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Restart</button>';
            htmlf += '<button class="btn btn-info btn-sm" id="continueWhereLeft">Countinue where I left</button><div class="text-center"><span>Step where you left onboarding: <strong>' + cs + '</strong></span></div>';
            LoadPopUp(htmlf, ProgressLevel);

            //}
            //setTimeout(function () {

            var paragraphs = document.createElement('p');
            containers = document.querySelector('.text-boxs');
            containers.appendChild(paragraphs);

            setTimeout(function () {
                dictates();
            }, 2000);
            //}, 300);
        }
        else {
            var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Get Started</button>';
            LoadPopUp(htmlf, 0);
            //setTimeout(function () {

            var paragraphs = document.createElement('p');
            containers = document.querySelector('.text-boxs');
            containers.appendChild(paragraphs);
            dictates();
        }
    });
    var IsFirstLogin = getSessionStorageValue(FirstLogin);
    if (IsFirstLogin === "true") {
        $.ajax({
            url: '/Speech/OnFirstLogin/',
            success: function (res) {
                console.log('Saved');
            }
        });
    }

});
$(document).on('click', '#tutorial', function () {

    LoadPopupStepBasis(UserInfo);
})
function GetPercentage(StepLevels) {
    var StepsGoThrough = StepLevels;
    var progress;
    if (StepsGoThrough === '' || StepsGoThrough === null || StepsGoThrough === undefined) {
        StepsGoThrough = 0;
    }
    else {
        StepsGoThrough = parseInt(StepsGoThrough);
        progress = (StepsGoThrough / 45) * 100;
        progress = Math.floor(progress);
        $(".progress-bar").removeAttr('style');
        $(".progress-bar").attr('style', 'width:' + progress + '%');
        $(".progress-bar").html(progress + '%');
    }
};
function GetUser() {
    $.getJSON("/Speech/GetUser", function (data) {
        UserInfo = data;
        setSessionStorageItem(UserInfo, JSON.stringify(data))
        if (UserInfo === null || UserInfo === undefined) {
            //
        }
        else {
            var chkStep = parseInt(UserInfo.currentStep);
            chkStep = parseInt(chkStep);
            if (chkStep === null) {
                createSession(1, false, "");
            }
            if (chkStep === 'NaN') {
                createSession(1, false, "");
            }
            else {
                createSession(chkStep, false, "");
            }
            if (UserInfo.isFirstLogin == true || UserInfo.isFirstLogin == null) {
                //LoadPopupStepBasis(UserInfo)
                $('#tutorial').click();
                setSessionStorageItem(FirstLogin, UserInfo.isFirstLogin)
            }
        }
    });
}
function LoadPopupStepBasis(data) {
    var isGrantedver;
    if (data == undefined) {
        console.log('undefined')
        $.getJSON("/Speech/GetUser", function (data) {
            UserInfo = data;
            isGrantedver;
            var cs;
            var progressLevel;
            //data=JSON.parse(data)
            if (data.currentStep > 1) {
                cs = parseInt(data.currentStep);
                progressLevel = parseInt(data.stepLevel);
                $("#getStarted").text('');
                $("#getStarted").text('Restart');


                var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Restart</button>';
                htmlf += '<button class="btn btn-info btn-sm" id="continueWhereLeft">Countinue where I left</button><div class="text-center"><span>Step where you left onboarding: <strong>' + data.stepTitle + '</strong></span></div>';
                isGrantedver = getSessionStorageValue(IsGranted);
                if (isGrantedver === null) {
                    micPermission();
                }
                else {
                    LoadPopUp(htmlf, progressLevel);
                }
            }
            else {
                //recognitions.start();
                isGrantedver = getSessionStorageValue(IsGranted);

                var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Get Started</button>';
                isGrantedver = getSessionStorageValue(IsGranted);
                if (isGrantedver === null) {
                    micPermission();
                }
                else {
                    LoadPopUp(htmlf, 0);
                }
            }
        });
    }
    else {

        var cs;
        var progressLevel;
        if (data.currentStep > 1) {
            console.log('step >1');
            cs = parseInt(data.currentStep);
            progressLevel = parseInt(data.stepLevel);
            $("#getStarted").text('');
            $("#getStarted").text('Restart');
            var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Restart</button>';
            htmlf += '<button class="btn btn-info btn-sm" id="continueWhereLeft">Countinue where I left</button><div class="text-center"><span>Step where you left onboarding: <strong>' + data.stepTitle + '</strong></span></div>';
            isGrantedver = getSessionStorageValue(IsGranted);
            if (isGrantedver === null) {
                micPermission();
            }
            else {
                LoadPopUp(htmlf, progressLevel);
            }
        }
        else {
            isGrantedver = getSessionStorageValue(IsGranted);
            var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Get Started</button>';
            isGrantedver = getSessionStorageValue(IsGranted);
            if (isGrantedver === null) {
                micPermission();
            }
            else {
                LoadPopUp(htmlf, 0);
            }
        }
    }
}
//Ckick Events
$(document).on('click', '#getStarted', function () {

    stepLevel = 1;
    createSession(1, false, "");
    setSessionStorageItem(StartPopUp, false);
    setSessionStorageItem(StepName, "Welcome");

    if (datas.length !== 0) {
        var ThisStep2;
        for (var i = 0; i < datas.length; i++) {
            if (datas[i].step === 1) {
                ThisStep2 = datas[i];
                //step2 = datas[i].description;
                //showVoBPopUp2(datas[i].step);
                //$("#CreateNewModal-label").html('');
                //$("#CreateNewModal-label").html("What would you like to learn?");
                //var html = '<div class="row"><div class="col-md-12"><p>' + step2 + '</p></div><div class="col-md-12 text-right"><button class="btn btn-sm btn-success"><i class="fa fa-microphone btpMcPhone"></i></button></div></div>';
                //$("#CreateNewBody").html('');
                //$("#CreateNewBody").html(html);
                //doSpeechFromToursJsFirstTime(datas[i].message)
            }
        }
        showVoBPopUp2(ThisStep2);
        doSpeechFromToursJsFirstTime(ThisStep2.message)
    }
    else {
        $.getJSON("/Speech/GetVoiceOnboardings", function (data) {
            var ThisStep;
            for (var i = 0; i < datas.length; i++) {
                if (data[i].step === 1) {
                    ThisStep = data[i];
                    //$("#CreateNewModal-label").html('');
                    //$("#CreateNewModal-label").html("What would you like to learn?");
                    //var html = '<div class="row"><div class="col-md-12"><p>' + step2 + '</p></div></div>';

                    //$("#CreateNewBody").html('');
                    //$("#CreateNewBody").html(html);
                    //showVoBPopUp2(datas[i]);
                    //doSpeechFromToursJsFirstTime(datas[i].message)
                }
            }
            showVoBPopUp2(ThisStep);
            doSpeechFromToursJsFirstTime(ThisStep.message)
        });
    }
    datas = JSON.parse(getSessionStorageValue(VOBData))
});
$(document).on("click", ".btpMcPhone", function () {

    if ($(".btpMcPhone").hasClass("fa-microphone")) {
        $(".btpMcPhone").addClass("fa-microphone-slash");
        $(".btpMcPhone").removeClass("fa-microphone");
        synths.pause();
        //endRecognitions();
    }
    else {
        $(".btpMcPhone").removeClass("fa-microphone-slash");
        $(".btpMcPhone").addClass("fa-microphone");
        synths.resume();
        //recognitions.start();
    }
});
$(document).on("click", "#continueWhereLeft", function () {
    $(".ttour-shadow").css('display', 'block');
    checkSteps();
    voiceOnboarding(datas);
    $("#CreateNewModal").modal('hide');
    $.ajax({
        url: '/Speech/SaveStartPopUp/',
        success: function (res) {
            console.log('Saved');
        }
    });
});
$(document).on("click", ".close", function () {
    synths.cancel();
});
$(".skip").on("click", function () {
    synths.cancel();
    var stepTitle;
    if (recognitions != undefined) {
        recognitions.stop();
    }
    var Urlpath = window.location.href;
    var currentStepNo = getSessionStorageValue(SessionStep);
    var stepTitle = getSessionStorageValue(StepName)
    currentStepNo = parseInt(currentStepNo);
    removeSessionValues();
    createSession(currentStepNo, true, "");
    $.ajax({
        url: '/Speech/SaveLater/',
        data: { Urlpath: Urlpath, step: currentStepNo, StepLevel: stepLevel, StepTitle: stepTitle },
        success: function (res) {
            $(".ttour-shadow").css('display', 'none');
            $.notify("Successfully saved for later", 'success');
        }
    });
});
$(document).on("click", "#cancelPermission", function () {
    $("#CreateNewModal").modal('hide');
});
$(document).on("click", "#grantPermission", function () {
    $("#CreateNewModal").modal('hide');
    setSessionStorageItem(IsGranted, true);
    RecognitionCredentials();
    ///recognitions.start();
    $.getJSON("/Speech/GetUser", function (data) {
        User = data;
        var cs;
        var ProgressPercentage;
        var ProgressLevel;
        var StepTitle;
        data.currentStep = parseInt(data.currentStep);
        if (data.currentStep > 1) {
            ProgressLevel = parseInt(data.stepLevel);
            cs = parseInt(data.currentStep);
            $("#getStarted").text('');
            $("#getStarted").text('Restart');
            var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Restart</button>';
            htmlf += '<button class="btn btn-info btn-sm" id="continueWhereLeft">Countinue where I left</button><div class="text-center"><span>Step where you left onboarding: <strong>' + cs + '</strong></span></div>';
            LoadPopUp(htmlf, ProgressLevel);

            //}
            //setTimeout(function () {

            var paragraphs = document.createElement('p');
            containers = document.querySelector('.text-boxs');
            containers.appendChild(paragraphs);

            setTimeout(function () {
                dictates();
            }, 2000);
            //}, 300);
        }
        else {
            var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Get Started</button>';
            LoadPopUp(htmlf, 0);
            //setTimeout(function () {

            var paragraphs = document.createElement('p');
            containers = document.querySelector('.text-boxs');
            containers.appendChild(paragraphs);
            dictates();
        }
    });
    var IsFirstLogin = getSessionStorageValue(FirstLogin);
    if (IsFirstLogin === "true") {
        $.ajax({
            url: '/Speech/OnFirstLogin/',
            success: function (res) {
                console.log('Saved');
            }
        });
    }

});
function showVoBPopUp(stepData, VoBstep) {

    var html = '';
    var title = '';
    var msg;
    var StepCurrent;
    for (var i = 0; i < datas.length; i++) {
        if (datas[i].step === VoBstep) {
            StepCurrent = datas[i];
            html += datas[i].description;
            title = datas[i].title;
            msg = datas[i].message;
        }
    }
        //var links = "";
    var GetVOBData = getSessionStorageValue(VOBData);
    if (GetVOBData.length > 0) {
        GetVOBData = JSON.parse(GetVOBData);
        for (var i = 0; i < GetVOBData.length; i++) {
            if (GetVOBData[i].parentVOBId === StepCurrent.id) {
                html += '<li><a href="#" class="goToStep" id=' + GetVOBData[i].step + '>' + GetVOBData[i].title + '</a></li>'
            }
        }
    }
    html += '<div class="col-md-12 text-right"><button class="btn btn-sm btn-success"><i class="fa fa-microphone btpMcPhone"></i></button></div>';
    $("#CreateNewModal-label").html(title);
    $("#CreateNewBody").html('');
    $("#CreateNewBody").html(html);

    $("#CreateNewModal").modal('show');
    // doSpeech();
    doSpeechFromToursJsFirstTime(msg);
}
function showVoBPopUp2(VoBstep) {

    var html = '';
    var title = '';
    var msg;
    var links = "";
    var GetVOBData = getSessionStorageValue(VOBData);
    if (GetVOBData.length > 0) {
        GetVOBData = JSON.parse(GetVOBData);
        for (var i = 0; i < GetVOBData.length; i++) {
            if (GetVOBData[i].parentVOBId === VoBstep.id) {
                links += '<li><a href="#" class="goToStep" id=' + GetVOBData[i].step + '>' + GetVOBData[i].title + '</a></li>'
            }
        }
    }

    links = "<ul>" + links + "</ul>";
    VoBstep.description += links;
    html += VoBstep.description;
    title = VoBstep.title;
    msg = VoBstep.message;

    html += '<div class="col-md-12 text-right"><button class="btn btn-sm btn-success"><i class="fa fa-microphone btpMcPhone"></i></button></div>';
    $("#CreateNewModal-label").html(title);
    $("#CreateNewBody").html('');
    $("#CreateNewBody").html(html);

    $("#CreateNewModal").modal('show');
    // doSpeech();
    doSpeechFromToursJsFirstTime(msg);
}
$(document).on('click', '#tutorial', function () {
    UserInfo = getSessionStorageValue(UserInfo)
    LoadPopupStepBasis(UserInfo);
})
