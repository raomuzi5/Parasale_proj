var recognitions;
var synths;
var UserInfo;
var FirstLogin = "FirstLogin";

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
iicon.addEventListener('click', () => {
    dictates();
    doSpeech();
});
function RecognitionCredentials() {
    window.SpeechRecognition = window.SpeechRecognition || webkitSpeechRecognition;
    recognitions = new SpeechRecognition();
    recognitions.interimResults = true;
    recognitions.continuous = false;
    recognitions.start();
    recognitions.onstart = function () {
        listening = true;
    };
    // when called, stops listening to the microphone
    recognitions.onend = function () {
        recognitions.stop();
    };
    recognitions.onspeechend = function () {
        recognitions.stop();
    };
}
function doSpeech() {
    //window.tour.steps[window.tour.current]
    //var currentTour = window.tour;
    //var description=currentTour.steps[currentTour.current].description;
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
    //}
    // var html = '';
    //if (directions === "addToCollections") {

    //    setTimeout(function () {
    //        msgToSpeech = "Are   you   done   with   clicking   any   blue   arrows   you   want ? Yes or   No ?";
    //        directionsYes = directions;
    //        directions = '';
    //        doSpeech();
    //        setTimeout(function () {
    //            doSpeech();
    //            $(".next").click();

    //        }, 3000);
    //        doSpeech();
    //       // appendHtml();
    //        $(".next").click();

    //    }, 25000);
    //}
    //if (directions === "building collections and objections") {
    //    //msgToSpeech = 'Do you see the segment that says Collections List? Yes or No';
    //    var ab = {
    //        element: ".dashboard",
    //        title: "Building Collections and Objections",
    //        description: msgToSpeech,
    //        data: "Custom Data",
    //        position: "left"
    //    };
    //    datas.push(ab);
    //    $(".next").click();
    //    voiceOnboarding(datas);
    //    $(".next").click();
    //    $(".next").click();
    //    $(".next").click();
    //    appendHtml();
    //    setTimeout(function () {
    //        doSpeech();
    //    }, 10000);
    //}

    //if (directions === "installing quick start collections" || directions === "installing Quickstart collections") {

    //    $("ul.list a[href='/Admin']:eq(1)").closest('li').addClass('onboardingStyle');
    //    html = '';
    //    html += '<div class="row">';
    //    html += '<div class="col-md-12">';
    //    html += '<img src="/images/onboardingImages/quickstart.png" />';
    //    html += '</div>';
    //    html += '</div>';
    //    $("#CreateNewModal-label").html("Quick Start");
    //    $("#CreateNewBody").html(html);
    //    $("#CreateNewModal").modal('show');
    //    msgToSpeech = "Have you clicked quick start tab? Yes or No.";
    //    setTimeout(function () {
    //        doSpeech();
    //        msgToSpeech = '';
    //    }, 10000);
    //}
}
function doSpeechFromToursJs(speechtext) {
    setTimeout(function () {
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
        // if (confirm("Click OK to speak")) {
        synths.speak(msg);
        msg.onend = function (event) {
            //recognition.start();
            synths.cancel();
        };
    }, 200);

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

function setCookies(step) {
    var d = new Date();
    d.setDate(d.getDate() + 15);
    var expires = "expires=" + d;
    var cookie = "username=" + username + ";" + "expiry=" + expires + ";path=/" + step;
    document.cookie = cookie;
}

function getCookie(name) {
    var value = document.cookie;

    var parts = value.split(name + "=");
    if (parts.length === 2) return parts/*.pop().split(";").shift()*/;
}
function deleteAllCookies() {
    var c = document.cookie.split("; ");
    for (i in c) {
        if (c[i] !== "") {
            document.cookie = /^[^=]+/.exec(c[i])[0] + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT";
        }
    }
}
function createCookie(step, isSaved, IsDirect) {

    var expires;
    var date = new Date();
    date.setDate(date.getDate() + 20);
    expires = "; expires=" + date.toGMTString();
    //    var av = "username=" + username + ",step=" + step + ";" + "expiry=" + expires + ";path=/";
    var av = "username=" + username + ",step=" + step + "?isSaved=" + isSaved + ";" + "expiry=" + expires + ";path=/";
    document.cookie = av;
}



function SaveLatercreateCookie(step, isSaved) {

    var expires;
    var date = new Date();
    date.setDate(date.getDate() + 20);
    expires = "; expires=" + date.toGMTString();
    var av = "username=" + username + ",step=" + step + ",isSaved=" + isSaved + ";" + "expiry=" + expires + ";path=/";
    document.cookie = av;
}
function readCookie(name) {
    var nameEQ = encodeURIComponent(name) + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ')
            c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0)
            return decodeURIComponent(c.substring(nameEQ.length, c.length));
    }
    return null;
}


function getSessionStorageValue(key) {
    return (sessionStorage.getItem(key) !== null) ? sessionStorage.getItem(key) : null;
}

function setSessionStorageItem(key, value) {
    sessionStorage.setItem(key, value);
}

function removeKeyFromSessionStorage(key) {
    if (sessionStorage.getItem(key) !== null)
        sessionStorage.removeItem(key);
}

function eraseCookie(name) {
    createCookie(name, "", -1);
}
function checkSteps() {

    //var chkStep = readCookie("username");
    //chkStep = chkStep.split(',');
    //currentStep = chkStep[1].split('?');
    //currentStep = currentStep[0];

    //get step from sessionstorage
    currentStep = getSessionStorageValue(SessionStep);
    currentStep = "step=" + currentStep;
    //isSavedforLater = chkStep[1];
    //isSavedforLater = isSavedforLater.match("isSaved=(.*)");

    //get isSaved from sessionstorage
    isSavedforLater = getSessionStorageValue(IsSessionSaved);

    //var chkStp = readCookie("isdirect");

    //get isDirect from sessionstorage
    //currentStep = getSessionStorageValue("IsSessionDirect");


    if (isSavedforLater === "false") {

        if (currentStep === "step=1") {
            msgToSpeech = 'Welcome   to  Parasale. My   name   is   Para   I   am   your   self   onboarding   agent. We   are   so   glad   to   have   you. Please   state   what   you   would   like   to   learn   first   from   the   following   list';

            msgToSpeech += 'Onboarding users and creating Teams';
            msgToSpeech += 'Practicing objections';
            msgToSpeech += 'Reporting';
            msgToSpeech += 'Audit logs';


            //synth.speak(msgToSpeech);
            datas = [{
                step: '',
                element: '.navbar',
                title: 'Welcome',
                description: 'Welcome   to  Parasale. My   name   is   Para   I   am   your   self   onboarding   agent. We   are   so   glad   to   have   you. Please   state   what   you   would   like   to learn   first   from   the   following   list: <ul> <li><a href="#" id="14" class="goToStep">Onboarding users and creating Teams</a></li> <li><a href="#" id="2" class="goToStep" data-ele=".practiceObjections">Practicing objections</a></li> <li><a href="#" id="27" class="goToStep">Reporting</a></li> <li><a href="#" id="42" class="goToStep">Audit logs</a></li></ul>',
                position: 'left',

            },
            {
                step: '2',
                element: '.practiceObjections',
                title: 'Practicing Objections',
                description: 'Great. Let’s talk about practicing objections. Practicing   objections   involves   three   things, which   would   you   like   to   start with   first ?<ul><li><a href="#" id="3" class="goToStep">Installing Quick Start Collections.</a></li><li><a href="#" id="6" class="goToStep">Building collections and objections.</a></li><li><a href="#" id="2" class="goToStep">Practicing objections</a></li></ul>',
                data: 'Custom Data',
                position: "right"
            },
            {
                step: '3',
                element: ".quickstart",
                title: "Quick Start",
                description: "Fantastic. Let’s work learning how to install quick start collections. Please click on the quick start collections tab on the left side menu navigation bar." + " <a id='4' href='#' onclick=MoveToUrl('.quickstart')> Yes</a> or <a href = '#'> No</a> ",
                data: "Custom Data",
                position: "right"
            }
            ];
            doSpeech();
            //voiceOnboarding(datas);

        }
        //if (currentStep === "") {
        //    msgToSpeech = 'Welcome   to  Parasale. My   name   is   Para   I   am   your   self   onboarding   agent. We   are   so   glad   to   have   you. Please   state   what   you   would   like   to   learn   first   from   the   following   list';

        //    msgToSpeech += 'Onboarding users and creating teams';
        //    msgToSpeech += 'Practicing objections';
        //    msgToSpeech += 'Reporting';
        //    msgToSpeech += 'Audit logs';

        //    datas = [{
        //        element: ".card",
        //        title: "Welcome",
        //        description: msgToSpeech,
        //        position: "left"
        //    },
        //    {
        //        element: ".practiceObjections",
        //        title: "Practicing Objections",
        //        description: msgToSpeech,
        //        data: "Custom Data",
        //        position: "right"
        //    },
        //    {
        //        element: ".quickstart",
        //        title: "Quick Start",
        //        description: msgToSpeech,
        //        data: "Custom Data",
        //        position: "right"
        //    }
        //    ];
        //    voiceOnboarding(datas);
        //    //clickevent();
        //}
        if (currentStep === "step=2") {
            msgToSpeech = '';
            msgToSpeech += "Great. Let’s talk about practicing objections. Practicing   objections   involves   three   things, which   would   you   like   to   start with   first?";

            msgToSpeech += "Installing Quick Start Collections";
            msgToSpeech += "Building collections and objections";
            msgToSpeech += "Practicing objections";
            datas = [{

                element: ".practiceObjections",
                title: "Practicing Objections",
                description: 'Great. Let’s talk about practicing objections. Practicing   objections   involves   three   things, which   would   you   like   to   start with   first ?<ul><li><a href="#" id="3" class="goToStep">Installing Quick Start Collections.</a></li><li><a href="#" id="6" class="goToStep">Building collections and objections.</a></li><li><a href="#" id="2" class="goToStep">Practicing objections</a></li></ul>',
                data: "Custom Data",
                position: "right"
            }];
            //datas.push(a);
            //voiceOnboarding(datas);

            doSpeech();
            //$(".next").click();
            //appendHtml();
            //clickevent();
        }
        if (currentStep === "step=3") {
            msgToSpeech = '';
            msgToSpeech = "Fantastic. Let’s work learning how to install quick start collections. Please click on the quick start collections tab on the left side menu navigation bar.";
            datas = [{
                element: ".quickstart",
                title: "Quick Start",
                description: msgToSpeech + "<a id='4' href='#' data-urlclass='.quickstart' onclick=MoveToUrl('.quickstart')>Yes</a>  or <a href = '#'> No</a>",
                data: "Custom Data",
                position: "right"
            }]
            //datas.push(b);
            //$(".next").click();
            //doSpeech();
            //appendHtml();

        }
        if (currentStep === "step=4") {
            msgToSpeech = "That was quick. Now, let   me   explain   what   a   quick   start   collection   is. A   quick   start   collection   is   an   easy   and   automatically   set   up   group   of   collections   you   can   push   to users, so   they   can   begin   practicing   with   Parasale in a   matter   of   moments. The   way   to   install   a         quick   start   collection   is   by   clicking   on   the   blue   arrow   shown   next   to   the   collection   name. Please        click   on   any   blue   arrows   you   would   like   to   add   to   your   collections.";
            directions = "addToCollections";
            setTimeout(function () {

                msgToSpeech = "Are   you   done   with   clicking   any   blue   arrows   you   want ? Yes or   No ?";
                directionsYes = directions;
                directions = '';
                doSpeech();
                //setTimeout(function () {
                //    //                doSpeech();
                //    $(".next").click();
                //}, 3000);
                doSpeech();
                // appendHtml();
                //$(".next").click();

            }, 30000);
            datas = [{
                element: ".body",
                title: "Quick Start Collections",
                description: "That was quick. Now, let   me   explain   what   a   quick   start   collection   is. A   quick   start   collection   is   an   easy   and   automatically   set up   group   of   collections   you   can   push   to users, so   they   can   begin   practicing   with   Parasale in a   matter   of   moments. The   way   to   install   a         quick   start   collection   is   by   clicking   on   the   blue   arrow   shown   next   to   the   collection   name. Please        click   on   any   blue   arrows   you   would   like   to   add   to   your   collections.",
                position: "left"

            },
            {
                step: '4',
                element: ".addCollections",
                title: "Move to Collections",
                description: 'Click on the blue arrow button to move collection in your list of collections',
                data: "Custom Data",
                position: "left"
            },
            {
                step: '4',
                element: ".collectionsToDo",
                title: "Done With Collections",
                description: 'Are   you   done   with   clicking   any   blue   arrows   you   want?' + "<a id='4' href='#' data-urlclass='.quickstart' onclick=AddCollectionDemo('#addCollections')>Yes</a>  or<a href = '#'> No</a>",
                data: "Custom Data",
                position: "left"
            },
            {
                step: '4',
                element: ".body",
                title: "What Next?",
                description: 'Quick start   collections   are   ready   for   practice. What   would   you   like to   learn   next?<ul><li><a href="#" id="6" class="goToStep"> Building   collections   and   objections. </a></li> <li> <a href="#" id="2" class="goToStep">Practicing   objections </a></li></ul> ',
                data: "Custom Data",
                position: "left"
            },
            {
                step: '6',
                element: ".dashboard",
                title: "Building collections and objection?",
                description: "Let’s talk about building collections and objections. Please click the <a id='5' href='#'  onclick=MoveToUrl('.dashboard','5')>dashboard</a> button   on   the   left   hand   side   and   then scroll to the very bottom of the page",
                data: "Custom Data",
                position: "right"
            }

            ];
            //voiceOnboarding(datas);
        }
        if (currentStep === "step=5") {

            msgToSpeech = '';
            msgToSpeech += 'Quick start   collections   are   ready   for   practice.   What   would   you   like to   learn   next?';

            msgToSpeech += 'Building   collections   and   objections.';
            msgToSpeech += 'Practicing   objections.';

            doSpeech();
            datas = [{
                element: ".body",
                title: "What Next?",
                description: 'Quick start   collections   are   ready   for   practice.   What   would   you   like to   learn   next ?<ul><li> Building   collections   and   objections. </li> <li> Practicing   objections </li></ul>',
                data: "Custom Data",
                position: "left"
            }];
            //datas.push(ab);
            //voiceOnboarding(datas);

            // $(".next").click();

            //  appendHtml();


        }
        if (currentStep === "step=6") {
            msgToSpeech = '';
            msgToSpeech = 'Let’s talk about building collections and objections. Please click the dashboard button   on   the   left   hand   side   and   then scroll to the very bottom of the page';
            datas = [{
                element: ".dashboard",
                title: "Building collections and objection?",
                description: "Let’s talk about building collections and objections.Please click the dashboard button   on   the   left   hand   side   and   then scroll to the very bottom of the page",
                data: "Custom Data",
                position: "right"
            }];
            //datas.push(acb);
            //voiceOnboarding(datas);

            //$(".next").click();

            doSpeech();
            //appendHtml();
        }
        if (currentStep === "step=7") {
            msgToSpeech = '';
            directionsYes = "CollectionSegment";

            msgToSpeech = 'Do you see the segment that says Collections List? Yes or No';
            datas = [{
                element: ".collectionThings",
                title: "Add New Collection",
                description: 'Do you see the segment that says Collections List?' + "<a  href='#' onclick='Clicknext()'> Yes </a>  or <a href = '#'> No</a>",
                position: "left"
            },
            {
                step: '7',
                element: ".addnewCollections",
                title: "Building collections and objection?",
                description: "Please click <a  href='#' onclick=ClickAddCollection('#createbtn')> add new </a> in the upper right hand corner of the add collections box",
                data: "Custom Data",
                position: "left"
            },
            {
                step: '7',
                element: ".addnewCollections",
                title: "Building collections and objection?",
                description: "Have   you   clicked   add   new   " + "<a  href='#' onclick='Clicknext()'> Yes </a>  or <a href = '#'> No</a>",
                data: "Custom Data",
                position: "left"
            },
            {
                step: '8',
                element: ".CollectionName",
                title: "Collection Name",
                description: "Please   type   the   name   my   first   collection   into   the   collection   box   and   click   save",
                data: "Custom Data",
                position: "left"
            }
                ,
            {
                step: '8',
                element: ".CollectionName",
                title: "Collection Name",
                description: "Have   you   typed   the   name   my   first   collection   in   and   clicked   saved?" + "<a  href='#' onclick='ClicknextAndHideModal()'> Yes </a>  or <a href = '#'> No</a>",
                data: "Custom Data",
                position: "left"
            },
            {
                step: '9',
                element: ".editbtn",
                title: "Collections",
                description: "Great you   created   your   first   collection. Let   me   walk   you   through   a   few features   of   collections. First, the   blue   pencil   allows   you   to   edit   the   collection   name, the   red garbage   can   lets   you   delete the   collection. Now   please   click   the   green   eye   so   you   can   see   what that does",
                data: "Custom Data",
                position: "right"
            },
            {
                step: '9',
                element: ".viewbtn",
                title: "Collections",
                description: "Have   you   clicked   the   green   eye? " + "<a  href='#' onclick='Clicknext()'> Yes </a>  or <a href = '#'> No</a>",
                data: "Custom Data",
                position: "left"

            },
            {
                step: '9',
                element: ".body",
                title: "Collections",
                description: "please   click   add   new   in the   upper   right   hand   corner",
                data: "Custom Data",
                position: "left"

            },
            {
                step: '9',
                element: ".body",
                title: "Collections",
                data: "Custom Data",
                position: "left",
                description: "Please   type   in   the   words   How   do   I   make   an   objection   in   the   objection   box   and   then   type   done into   the   keyword   box.Then   hit   save"
            },
            {
                step: '9',
                element: ".body",
                title: "Collections",
                description: " Have you done this",
                data: "Custom Data",
                position: "left"

            },
            {
                element: ".practiceObjections",
                title: "Practice Objection",
                description: "Congratulations!   you   have   added   your   first   collection   and   objection.   Now   on   to the   last   section   of   this   portion   of   training   practicing   objections or   whatever   the   name   of   the   last thing   they   decided   to   set   up in this   section   is.  Please   click   on   the   left   side   navigation   bar   where it   says   Practice   Objections",
                data: "Custom Data",
                position: "right"
            }

            ];
            // datas.push(accv);
            //voiceOnboarding(datas);
            doSpeech();
            msgToSpeech = '';
            //appendHtml();

        }
        if (currentStep === "step=71") {

            msgToSpeech = 'Please click add new in the upper right hand corner of the add collections box';

            datas = [{
                element: ".addnewCollections",
                title: "Building collections and objection?",
                description: "Please click add new in the upper right hand corner of the add collections box",
                data: "Custom Data",
                position: "left"
            }];
            //datas.push(acc);
            //voiceOnboarding(datas);
            //$(".next").click();
            doSpeech();
            setTimeout(function () {
                directionsYes = "addNewCollections";
                msgToSpeech = '';
                msgToSpeech = 'Have   you   clicked   add   new   yes,   or   no?';
                //var acc = {
                //    element: ".addnewCollections",
                //    title: "Building collections and objection?",
                //    description: "Have   you   clicked   add   new   yes,   or   no?",
                //    data: "Custom Data",
                //    position: "right"
                //};
                //datas.push(acc);
                //voiceOnboarding(datas);
                //  $(".next").click();
                $(".next").click();
                // appendHtml();
                doSpeech();

            }, 8000);
            // appendHtml();
        }
        if (currentStep === "step=8") {

            msgToSpeech = '';
            msgToSpeech = 'Please   type   the   name   my   first   collection   into   the   collection   box   and   click   save';
            doSpeech();
            datas = [{
                element: "#collectionName",
                title: "Collection Name",
                description: "Please   type   the   name   my   first   collection   into   the   collection   box   and   click   save",
                data: "Custom Data",
                position: "right"
            }];
            //datas.push(at);
            //voiceOnboarding(datas);
            //$(".next").click();
            //$(".next").click();
            // $(".next").click();
            setTimeout(function () {
                msgToSpeech = 'Have   you   typed   the   name   my   first   collection   in   and   clicked   saved?   Yes   or   No?';
                directionsYes = "firstCollection";
                doSpeech();
                //var at = {
                //    element: ".CollectionName",
                //    title: "Collection Name",
                //    description: "Have   you   typed   the   name   my   first   collection   in   and   clicked   saved?   Yes   or   No?",
                //    data: "Custom Data",
                //    position: "right"
                //};
                //datas.push(at);
                //voiceOnboarding(datas);
                //$(".next").click();
                //$(".next").click();
                //$(".next").click();
                //$(".next").click();
                $(".next").click();
            }, 10000);

        }
        if (currentStep === "step=9") {
            msgToSpeech = '';

            msgToSpeech = 'Great. you   created   your   first   collection. Let   me   walk   you   through   a   few features   of   collections. First, the   blue   pencil   allows   you   to   edit   the   collection   name, the   red garbage   can   lets   you   delete the   collection. Now   please   click   the   green   eye   so   you   can   see   what that does';
            doSpeech();
            datas = [{
                element: ".editbtn",
                title: "Collections",
                description: "Great you   created   your   first   collection. Let   me   walk   you   through   a   few features   of   collections. First, the   blue   pencil   allows   you   to   edit   the   collection   name, the   red garbage   can   lets   you   delete the   collection. Now   please   click   the   green   eye   so   you   can   see   what that does",
                data: "Custom Data",
                position: "right"
            }];
            //datas.push(accd);
            //voiceOnboarding(datas);
            //$(".next").click();
            //$(".next").click();
            //$(".next").click();
            // appendHtml();
            setTimeout(function () {
                msgToSpeech = 'Have   you   clicked   the   green   eye?   Yes   or   No';
                directionsYes = "collectionsView";
                doSpeech();
                //var accd = {
                //    element: ".viewbtn",
                //    title: "Collections",
                //    description: "Have   you   clicked   the   green   eye?   Yes   or   No",
                //    data: "Custom Data",
                //    position: "right"

                //};
                //datas.push(accd);
                //voiceOnboarding(datas);
                //$(".next").click();
                //$(".next").click();
                $(".next").click();
                //  appendHtml();
            }, 10000);
        }
        if (currentStep === "step=10") {
            msgToSpeech = '';
            msgToSpeech = 'please   click   add   new   in   the   upper   right   hand   corner';
            doSpeech();
            //$(".next").click();
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Please   type   in   the   words   How   do   I   make   an   objection   in   the   objection   box   and   then   type   done into   the   keyword   box. Then   hit   save';
                doSpeech();
                $(".next").click();
            }, 10000);
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Have you done this?';
                directionsYes = "DoneObjections";
                doSpeech();
                $(".next").click();
                // appendHtml();
            }, 15000);

        }
        if (currentStep === "step=11") {
            msgToSpeech = '';
            msgToSpeech += 'Congratulations!   you   have   added   your   first   collection   and   objection.   Now   on   to ';

            msgToSpeech += 'the   last   section   of   this   portion   of   training   practicing   objections or   whatever   the   name   of   the   last ';
            msgToSpeech += 'thing   they   decided   to   set   up in this   section   is.  Please   click   on   the   left   side   navigation   bar   where';
            msgToSpeech += ' it   says   Practice   Objections';
            doSpeech();
            datas = [{
                element: ".practiceObjectiob",
                title: "Practice Objection",
                description: "Congratulations!   you   have   added   your   first   collection   and   objection.   Now   on   to the   last   section   of   this   portion   of   training   practicing   objections or   whatever   the   name   of   the   last thing   they   decided   to   set   up in this   section   is.  Please   click   on   the   left   side   navigation   bar   where it   says   Practice   Objections",
                data: "Custom Data",
                position: "left"
            }];
            //data.push(fc);
            //voiceOnboarding(datas);
            /// appendHtml();
            //  $(".next").click();
            //$(".next").click();
            //$(".next").click();
            //$(".next").click();
            //$(".next").click();
        }
        if (currentStep === "step=12") {

            msgToSpeech = 'Please   select   the   my   first   collection   from   the   drop   down   menu   that   says   collections';
            doSpeech();
            datas = [{

                element: ".listCollections",
                title: "Select Collection",
                description: msgToSpeech,

                position: "right"
            },
            {
                element: ".microphone",
                title: "Practice Objections",
                description: "Now   click   the   purple   microphone   and   then   say   give   me   any   objection   and   respond   the   practice objection",
                data: "Custom Data",
                position: "left"
            }, {
                element: ".microphone",
                title: "Practice Objections",
                description: "Have you done it? Yes or No?",
                data: "Custom Data",
                position: "left"
            },
            {
                element: '.body',
                title: 'Select one of the followings',
                description: 'Congratulations!   You   have   finished   the   first   part   of   training.Please   pick   between   the following   to   continue   the   onboarding: <ul><li> <a href="#" id="14" class="goToStep">Onboarding users and   creating   teams</a></li><li><a href="#" id="27" class="goToStep">Reporting</a></li><li><a href="#" id="42" class="goToStep">Audit   logs</a></li></ul>',
                data: 'Custom Data',
                position: 'left'
            },
            {
                element: ".inviteUsers",
                title: "Onboarding users and creating teams",
                description: "We   are   now   moving   to   onboarding   users   and   creating teams. Let’s   start   with   onboarding   users. Please   click   the   invite   users   tab   on   the   left   side   of navigation   screen",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".inviteUsers",
                title: "Onboarding users and creating teams",
                description: "Are   you   done   with   this ? Yes  or   No?",
                data: "Custom Data",
                position: "right"

            }
            ];
            voiceOnboarding(datas);
            //appendHtml();
            setTimeout(function () {
                msgToSpeech = 'Now   click   the   purple   microphone   and   then   say   give   me   any   objection   and   respond   the   practice objection';
                datas = [{
                    element: ".microphone",
                    title: "Practice Objections",
                    description: "Now   click   the   purple   microphone   and   then   say   give   me   any   objection   and   respond   the   practice objection",
                    data: "Custom Data",
                    position: "left"
                }];
                doSpeech();
                //data.push(mc);
                //voiceOnboarding(datas);
                // appendHtml();
                $(".next").click();

                setTimeout(function () {
                    msgToSpeech = '';
                    directionsYes = "microphone";
                    msgToSpeech = 'Have you done it? Yes or No?';
                    doSpeech();
                    //var mc = [{
                    //    element: ".microphone",
                    //    title: "Practice Objections",
                    //    description: "Have you done it? Yes or No?",
                    //    data: "Custom Data",
                    //    position: "left"
                    //}];
                    doSpeech();
                    //data.push(mc);
                    //voiceOnboarding(datas);
                    // appendHtml();
                    //  $(".next").click();
                    $(".next").click();

                }, 8000);
            }, 10000);

            //doSpeech();
            //msgToSpeech = '';
            //voiceOnboarding(datas);


        }
        if (currentStep === "step=13") {
            msgToSpeech = '';
            msgToSpeech += 'Congratulations!   You   have   finished   the   first   part   of   training.   Please   pick   between   the ';
            msgToSpeech += ' following   to   continue   the   onboarding';

            msgToSpeech += 'Onboarding   users   and   creating   teams';
            msgToSpeech += 'Reporting';
            msgToSpeech += 'Audit logs';

            doSpeech();
            datas = [{
                element: ".body",
                title: "Select one of the followings",
                description: "Congratulations!   You   have   finished   the   first   part   of   training.Please   pick   between   the following   to   continue   the   onboarding: <ul><li>Onboarding users and   creating   teams</li><li>Reporting</li><li>Audit   logs</li></ul>",
                data: "Custom Data",
                position: "left"
            }];
            //doSpeech();
            //data.push(mcc);
            //voiceOnboarding(datas);
            // appendHtml();
            //$(".next").click();
            //$(".next").click();
            //  $(".next").click();

        }
        if (currentStep === "step=14") {
            msgToSpeech = '';
            msgToSpeech = ' We   are   now   moving   to   onboarding   users   and   creating teams. Let’s   start   with   onboarding   users. Please   click   the   invite   users   tab   on   the   left   side   of navigation   screen';
            datas[{
                element: ".inviteUsers",
                title: "Onboarding users and creating teams",
                description: "We   are   now   moving   to   onboarding   users   and   creating teams. Let’s   start   with   onboarding   users. Please   click   the   invite   users   tab   on   the   left   side   of navigation   screen",
                data: "Custom Data",
                position: "right"
            }];
            //voiceOnboarding(datas);
            doSpeech();
            //appendHtml();
            //$(".next").click();
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Are   you   done   with   this?   Yes  or   No?';
                doSpeech();
                $(".next").click();
            }, 10000);
        }
        if (currentStep === "step=15") {
            msgToSpeech = 'Right   on.   Now   in   order   to   invite   a   user   please   type   an   email   into   the   box   that   has   the word   email   above   it in the   upper   left   corner   and   then   click   invite';
            datas = [{
                element: ".userInvites",
                title: "Onboarding users and creating Team",
                description: msgToSpeech,
                data: "Custom Data",
                position: "left"
            }
                ,
            {
                element: ".userInvites",
                title: "Done",
                description: "Are you done with this? Yes or No?",
                data: "Custom Data",
                position: "left"
            },
            {
                element: '.users',
                title: 'Manage Team',
                description: "You   have   now   invited   your   first user.   You   can   see   if   they   have   signed   up,   or   not   below. Now   it   is   time   for   you   to   add   them   to   a   team. Please   click   the   users   tab   on   the   left   side navigation   bar    of   the   screen",
                data: 'Custom Data',
                position: 'right'
            }];
            doSpeech();
            msgToSpeech = '';
            //voiceOnboarding(datas);
            // appendHtml();
            directionsYes = "userinvites";
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Are you done with this? Yes or No?';
                doSpeech();
                datas = [{
                    element: ".userInvites",
                    title: "Done",
                    description: "Are you done with this? Yes or No?",
                    data: "Custom Data",
                    position: "left"
                }];
                //datas.push(dta);
                //voiceOnboarding(datas);
                //   appendHtml();
                $(".next").click();
            }, 10000);

        }
        if (currentStep === "step=16") {
            msgToSpeech = '';
            msgToSpeech = 'You   have   now   invited   your   first user.   You   can   see   if   they   have   signed   up,   or   not   below. Now   it   is   time   for   you   to   add   them   to   a   team. Please   click   the   users   tab   on   the   left   side navigation   bar    of   the   screen';

            datas = [{
                element: '.users',
                title: 'Manage Team',
                description: "You   have   now   invited   your   first user.   You   can   see   if   they   have   signed   up,   or   not   below. Now   it   is   time   for   you   to   add   them   to   a   team. Please   click   the   users   tab   on   the   left   side navigation   bar    of   the   screen",
                data: 'Custom Data',
                position: 'right'
            }];
            //datas.push(aa);
            //voiceOnboarding(datas);
            //$(".next").click();
            //$(".next").click();
            doSpeech();
            appendHtml();

        }
        if (currentStep === "step=17") {
            msgToSpeech = 'Under the users section you can do three things. What would you like to learn first? Making a user a manager or removing a user from management. Assigning a user to a team. Viewing a team.';

            datas = [{
                element: ".body",
                title: "Users Section",
                description: msgToSpeech,
                data: "Custom Data",
                position: "left"
            }
                ,
            {
                element: ".body",
                title: "Are you done",
                description: "Are you done with this? Yes or No?",
                data: "Custom Data",
                position: "left"
            },
            {
                element: '.manager',
                title: 'Make or Remove user as Manager',
                description: "Great   pick. Let’s learn   how   to   make   a   user   a   manager, or   remove   them   from   management. Please   click   the   blue plus   by   a   users   name",
                data: 'custom Data',
                position: 'left'
            },
            {
                element: '.bootbox-accept',
                title: 'Make user as Manager',
                description: "Click yes to make the user a manager.",
                data: 'custom Data',
                position: 'left'
            },
            {
                element: '.manager',
                title: 'Make user as Manager',
                description: "if you ever want to remove the person from management you can click blue X and it will remove them.",
                data: 'custom Data',
                position: 'left'
            },
            {
                element: '.card',
                title: 'Make user as Manager',
                description: "Do you feel conformtable with this yes or no?",
                data: 'custom Data',
                position: 'left'
            },
            {
                element: '.ausers',
                title: 'Assign User to team',
                description: "Now   let’s   look   at   adding   users   to   the   managers   team.   Please   click   the   blue   button   with two   people   and   a   plus   next   to   them",
                data: 'custom Data',
                position: 'left'
            },
            {
                element: '.ausers',
                title: 'Add to team',
                description: "Have   you   clicked   the   blue   button   with   two   people   and   a   plus?   Yes   or   No",
                data: 'custom Data',
                position: 'left'
            },
            {
                element: '.checkisteammember',
                description: 'Now let’s select the users   we   want   to   add   to   the   team.   Please   check   the   box   under   the  header that says add   for   any   users   you   would   like   to   assign   to   this   manager. Then   click   the black   x   to   exit   once   you   are   done',
                title: 'Add to team',
                position: 'right',
                data: 'custom Data'
            },
            {
                element: '.checkisteammember',
                description: 'Have   you   done   this?   Yes,   or   No?',
                title: 'Done',
                position: 'right',
                data: 'custom Data'
            },
            {
                element: '.teamView',
                description: 'Great!   You   have   added   users   to   a   team. Lastly,   let’s   see   how   to  view   those   teams. Please   click   the   blue   eye   icon   next   to   the   managers   name',
                title: 'Add to team',
                position: 'right',
                data: 'custom Data'
            },
            {
                element: '.teamView',
                description: 'Have   you   clicked   the   blue   eye   icon?   Yes,   or   No?',
                title: 'Done',
                position: 'right',
                data: 'custom Data'
            },
            {
                element: '.teams',
                description: 'Awesome   work.   You   are   done   with   this   section.    Now   navigate   to   the   Teams   section   on the   left   side   navigation   bar',
                title: 'Teams',
                position: 'left',
                data: 'custom Data'
            },
            {
                element: '.teams',
                description: 'Have you navigated to Teams? Yes or No?',
                title: 'Navigated to Teams',
                position: 'left',
                data: 'custom Data'
            }

            ];
            doSpeech();
            msgToSpeech = '';
            //voiceOnboarding(datas);
            //appendHtml();
            directionsYes = "userinvites";
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Are you done with this? Yes or No?';
                doSpeech();
                //var dats = {
                //    element: ".body",
                //    title: "Are you done",
                //    description: "Are you done with this? Yes or No?",
                //    data: "Custom Data",
                //    position: "left"
                //};
                //datas.push(dats);
                //voiceOnboarding(datas);
                //$(".next").click(); 
                $(".next").click();
                //appendHtml();

            }, 10000);

        }
        if (currentStep === "step=18") {
            msgToSpeech = '';
            msgToSpeech = 'Great   pick. Let’s learn   how   to   make   a   user   a   manager or   remove   them   from   management. Please   click   the   blue plus   by   a   users   name';
            directionsYes = 'user manager';
            datas = [{
                element: '.manager',
                title: 'Make or Remove user as Manager',
                description: "Great   pick. Let’s learn   how   to   make   a   user   a   manager, or   remove   them   from   management. Please   click   the   blue plus   by   a   users   name",
                data: 'custom Data',
                position: 'left'
            }];
            //datas.push(auser);
            voiceOnboarding(datas);
            //$(".next").click();
            doSpeech();
            //appendHtml();
            msgToSpeech = '';
            setTimeout(function () {
                msgToSpeech = 'Click yes to make the user a manager.';
                // appendHtml();
                //var auser = {
                //    element: '.bootbox-accept',
                //    title: 'Make user as Manager',
                //    description: "Click yes to make the user a manager.",
                //    data: 'custom Data',
                //    position: 'left'
                //};
                //datas.push(auser);
                //voiceOnboarding(datas);
                //appendHtml();
                //$(".next").click();
                $(".next").click();

                doSpeech();
                msgToSpeech = '';
            }, 5000);
            setTimeout(function () {

                msgToSpeech = '';
                msgToSpeech = 'if you ever want to remove the person from management you can click blue X and it will remove them.';
                doSpeech();
                datas = [{
                    element: '.manager',
                    title: 'Make user as Manager',
                    description: "if you ever want to remove the person from management you can click blue X and it will remove them.",
                    data: 'custom Data',
                    position: 'left'
                }];
                //datas.push(auser);
                //voiceOnboarding(datas);
                // appendHtml();
                //$(".next").click();
                //$(".next").click();
                $(".next").click();
            }, 10000);
            setTimeout(function () {

                msgToSpeech = '';
                msgToSpeech = 'Do you feel conformtable with this yes or no?';
                doSpeech();
                //var auser = {
                //    element: '.card',
                //    title: 'Make user as Manager',
                //    description: "'Do you feel conformtable with this yes or no?",
                //    data: 'custom Data',
                //    position: 'left'
                //};
                //datas.push(auser);
                //voiceOnboarding(datas);
                //appendHtml();
                //$(".next").click();
                //$(".next").click();
                $(".next").click();

            }, 16000);
        }
        if (currentStep === "step=19") {
            msgToSpeech = '';
            msgToSpeech = 'Now   let’s   look   at   adding   users   to   the   managers   team.   Please   click   the   blue   button   with two   people   and   a   plus   next   to   them';
            doSpeech();
            datas = [{
                element: '.ausers',
                title: 'Assign User to team',
                description: "Now   let’s   look   at   adding   users   to   the   managers   team.   Please   click   the   blue   button   with two   people   and   a   plus   next   to   them",
                data: 'custom Data',
                position: 'left'
            }]

            //datas.push(ausers);

            //voiceOnboarding(datas);
            //$(".next").click();
            //$(".next").click();
            //appendHtml();
            msgToSpeech = '';
            setTimeout(function () {
                directionsYes = 'assignToTeam';
                msgToSpeech = 'Have   you   clicked   the   blue   button   with   two   people   and   a   plus?   Yes   or   No';
                doSpeech();
                //var ausers = {
                //    element: '.ausers',
                //    title: 'Add to team',
                //    description: "Have   you   clicked   the   blue   button   with   two   people   and   a   plus?   Yes   or   No",
                //    data: 'custom Data',
                //    position: 'left'
                //}

                //datas.push(ausers);

                //voiceOnboarding(datas);
                //$(".next").click();
                //$(".next").click();
                $(".next").click();
                // appendHtml();
            }, 10000);
        }
        if (currentStep === "step=20") {
            msgToSpeech = '';
            msgToSpeech = 'Now let’s select the users   we   want   to   add   to   the   team.   Please   check   the   box   under   the  header that says add   for   any   users   you   would   like   to   assign   to   this   manager. Then   click   the black   X   to   exit   once   you   are   done';
            doSpeech();
            //$(".next").click();
            datas = [{
                element: '.checkisteammember',
                description: 'Now let’s select the users   we   want   to   add   to   the   team.   Please   check   the   box   under   the  header that says add   for   any   users   you   would   like   to   assign   to   this   manager. Then   click   the black   x   to   exit   once   you   are   done',
                title: 'Add to team',
                position: 'right',
                data: 'custom Data'
            }]
            //datas.push(assUser);
            //voiceOnboarding(datas);
            // appendHtml();
            //$(".next").click();
            //$(".next").click();
            //$(".next").click();
            setTimeout(function () {
                msgToSpeech = '';
                directionsYes = 'doneAssigning';
                msgToSpeech = 'Have   you   done   this?   Yes   or   No?';
                //var assUser = {
                //    element: '.checkisteammember',
                //    description: 'Have   you   done   this?   Yes,   or   No?',
                //    title: 'Done',
                //    position: 'right',
                //    data: 'custom Data'
                //}
                //datas.push(assUser);
                //voiceOnboarding(datas);
                //appendHtml();
                //$(".next").click();
                //$(".next").click();
                //$(".next").click();
                $(".next").click();

            }, 15000);

        }
        if (currentStep === "step=21") {
            msgToSpeech = '';
            msgToSpeech = 'Great!   You   have   added   users   to   a   team. Lastly,   let’s   see   how   to  view   those   teams. Please   click   the   blue   eye   icon   next   to   the   managers   name';
            doSpeech();
            // $(".next").click();
            datas = [{
                element: '.teamView',
                description: 'Great!   You   have   added   users   to   a   team. Lastly,   let’s   see   how   to  view   those   teams. Please   click   the   blue   eye   icon   next   to   the   managers   name',
                title: 'Add to team',
                position: 'right',
                data: 'custom Data'
            }]
            //datas.push(viewusers);
            //voiceOnboarding(datas);

            //$(".next").click();
            //$(".next").click();
            //$(".next").click();
            //appendHtml();
            setTimeout(function () {
                msgToSpeech = '';
                directionsYes = 'teamView';
                msgToSpeech = 'Have   you   clicked   the   blue   eye   icon?   Yes,   or   No?';
                //var viewusers = {
                //    element: '.teamView',
                //    description: 'Have   you   clicked   the   blue   eye   icon?   Yes,   or   No?',
                //    title: 'Done',
                //    position: 'right',
                //    data: 'custom Data'
                //}
                //datas.push(viewusers);
                //voiceOnboarding(datas);

                //$(".next").click();
                //$(".next").click();
                //$(".next").click();
                $(".next").click();
                //appendHtml();

            }, 6000);

        }
        if (currentStep === "step=22") {
            msgToSpeech = '';
            msgToSpeech = 'Awesome   work.   You   are   done   with   this   section.    Now   navigate   to   the   Teams   section   on the   left   side   navigation   bar';
            doSpeech();
            datas = [{
                element: '.teams',
                description: 'Awesome   work.   You   are   done   with   this   section.    Now   navigate   to   the   Teams   section   on the   left   side   navigation   bar',
                title: 'Teams',
                position: 'left',
                data: 'custom Data'
            }]
            //datas.push(teams);
            //voiceOnboarding(datas);
            //$(".next").click();
            //$(".next").click();
            //$(".next").click();
            // appendHtml();
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Have you navigated to Teams? Yes or No?';
                //var teams = {
                //    element: '.teams',
                //    description: 'Have you navigated to Teams? Yes or No?',
                //    title: 'Navigated to Teams',
                //    position: 'left',
                //    data: 'custom Data'
                //}
                //datas.push(teams);
                //voiceOnboarding(datas);
                //$(".next").click();
                //$(".next").click();
                //$(".next").click();
                $(".next").click();
                //appendHtml();
            }, 8000);
        }
        if (currentStep === "step=23") {
            msgToSpeech = 'Please   click   on   the   users   name   to   see   a   drop   down   of   who   is   on   their   team';
            datas = [{
                element: ".teamsss",
                title: "Teams",
                description: msgToSpeech,
                data: "Custom Data",
                position: "left"
            },
            {
                element: ".teamsss",
                title: "Clicked Name",
                description: "Have   you   clicked   the   name?   Yes,   or   No",
                data: "Custom Data",
                position: "left"
            },
            {
                element: '.addnewbtn',
                title: 'Teams',
                description: 'Perfect.   Now   let’s   click   the   plus   on   the   right   side   of   the   users   name',
                data: 'custom Data',
                position: 'right'
            },
            {
                element: '.addnewbtn',
                title: 'Understand Concept',
                description: 'Do you understand this concept?   Yes   or   No',
                data: 'custom Data',
                position: 'right'
            },
            {
                element: '.body',
                title: 'Finish',
                description: 'Congratulations.   You   have   finished   this   portion   of   training.     You   are   halfway   done.   What would   you   like   to   learn   next? <ul><li><a href="#" id="27" class="goToStep">Reporting.</a></li> <span>or</span><li><a href="#" id="42" class="goToStep">Audit Logs</a></li></ul>',
                data: 'Custom Data',
                position: 'right'
            },
            //{
            //    element: ".reports",
            //    title: "Reporting?",
            //    description: "Let’s   learn   reporting.   Please   navigate   to   the   dashboard   by   clicking   on   it   on   the   left side   navigation   bar. On   the   dashboard   we   have   two reports. The   first   is   the   completed objections   report. Which   is   located   at   the   top   of   the   dashboard.  Do   you   see   the   completed objections   report   on   the   dashboard? Yes or   No",
            //    data: "Custom Data",
            //    position: "right"
            //},
            {
                element: ".dates",
                title: "Dates",
                description: "Great.This   report   shows   the   number   of   objections   a   user   has   correctly   completed versus   the   number   they   incorrectly   completed.There   are   three   ways   to   filter   the   report.First  let’s   look   at   sorting   by   the   date.Look   at   the   date in the   upper   left   hand   corner   of   the   report.",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".dates",
                title: "Dates",
                description: "Do   you   see   the   date ? Yes or   No",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".dates",
                title: "Dates",
                description: "please   click   on   either   the   start   date,   or   end   date   to   see   how   to   adjust   the   date   range   for the   reports",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".dates",
                title: "Dates",
                description: "Have   you   clicked   the   date? Yes or   No",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".dates",
                title: "Feeling Comfortable",
                description: "Next,   click   The   name   of   the   month   and   year   in   order   to   see   how   you   can   adjust   the year   and   month.",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".dates",
                title: "Feeling Comfortable",
                description: "Do   you   feel   comfortable   adjusting   the   date?   Yes  or   No",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".togglebtns",
                title: "Toggle Buttons",
                description: "Great.   Let’s   move   on   to   the   second   way   to   filter   the   report.   Please   direct   your   eyes   to  the   upper   right   toggle   buttons",
                data: "Custom Data",
                position: "left"
            },
            {
                element: ".togglebtns",
                title: "Toggle Buttons",
                description: "Do   you   see   the   toggle   buttons?   Yes   or   No",
                data: "Custom Data",
                position: "left"
            },
            {
                element: ".togglebtns",
                title: "Toggle Buttons",
                description: "Fantastic.   Feel   free   to   move   the   buttons   around   to   see   what   the   report   looks   like   when filtered   by   user, by   manager, and   by   collection",
                data: "Custom Data",
                position: "left"
            },
            {
                element: ".togglebtns",
                title: "Toggle Buttons",
                description: "Do   you   feel comfortable   with   the   toggle   buttons? Yes or   No?",
                data: "Custom Data",
                position: "left"
            },
            {
                element: ".highcharts-a11y-proxy-button",
                title: "Filter Reports",
                description: "Awesome.   On   to   the   last   way   to   filter   a   report.   Please   direct   your   eyes   to   the   mid-right  section   of   the   report   where   it   has   a   users   name   with   a   line   color   next   to   it",
                data: "Custom Data",
                position: "left"
            },
            {
                element: ".highcharts-a11y-proxy-button",
                title: "Filter Reports",
                description: "Do   you   see   it?   Yes,   or   No",
                data: "Custom Data",
                position: "left"
            },
            {
                element: ".highcharts-a11y-proxy-button",
                title: "Filter Reports",
                data: "Custom Data",
                position: "left",
                description: "Please   click   on   a   user   to   see   the   name   grey   out,   which   means   that   the   user   is   no   longer visible   on   the   graph. Then   re-click   the   user   to   see   the   name   gain   color   meaning   they   are   visible on   the   graph."
            },
            {
                element: ".highcharts-a11y-proxy-button",
                title: "Filter Reports",
                data: "Custom Data",
                position: "left",
                description: "Do   you   feel   comfortable   with   this?   Yes   or   No'"
            },
            {
                element: ".zmdi-info",
                title: "Filter Reports",
                data: "Custom Data",
                position: "left",
                description: "Perfect.   That   completes   report   one   and   filtering.   On   to   our   second   report.   Please   scroll down   on   the   dashboard   page   until   you   see   a   report  that   says   Confidence   and   Consistency score"
            },
            {
                element: ".zmdi-info",
                title: "Filter Reports",
                data: "Custom Data",
                position: "left",
                description: "Do   you   see   the   confidence   and   consistency   score   report?   Yes           or   No"
            },
            {
                element: ".zmdi-info",
                title: "CCS Report",
                description: "Fantastic.   Please   click   the   round   black   dot   with   the   white   letter   i   in   the   middle.   This   will show   you   the   definition   of   this   report ",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".zmdi-info",
                title: "CCS Report",
                description: "Did   you   click   the   information   icon?   Yes, or   No",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".zmdi-info",
                title: "CCS Report",
                description: "Awesome.This   chart   is   shown   in   a   bar   and   line   graph. Like   the   report   we   went   through before   it   can   be   filtered   by   date, by   toggle   buttons, and   by   adding/ subtracting   items   from   the chart.Feel   free   to   explore   this   report   for   a   few   moments",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".zmdi-info",
                title: "CCS Report",
                description: "Do   you   feel   comfortable   with   the   confidence   and   consistency   report?   Yes,   or   No",
                data: "Custom Data",
                position: "right"
            },
                //{
                //    element: ".reports",
                //    title: "CCS Report",
                //    description: "Fantastic.   Please   navigate   to   the   reports   tab   on   the   left   navigation   bar",
                //    data: "Custom Data",
                //    position: "right"
                //}


                //{
                //    element: '.addnewbtn',
                //    title: 'Clicked Plus Sign',
                //    description: 'Have   you   clicked   the   plus sign?   Yes   or   No',
                //    data: 'custom Data',
                //    position: 'right'
                //}

            ];
            doSpeech();
            msgToSpeech = '';
            voiceOnboarding(datas);
            setTimeout(function () {
                msgToSpeech = '';
                directionsYes = 'teamYes';
                msgToSpeech = 'Have   you   clicked   the   name?   Yes,   or   No';
                //var das = {
                //    element: ".teamsss",
                //    title: "Clicked Name",
                //    description: "Have   you   clicked   the   name?   Yes,   or   No",
                //    data: "Custom Data",
                //    position: "left"
                //};
                doSpeech();
                msgToSpeech = '';
                //voiceOnboarding(das);
                $(".next").click();
                //appendHtml();
            }, 10000);
        }
        if (currentStep === "step=24") {

            msgToSpeech = 'Perfect.   Now   let’s   click   the   plus   on   the   right   side   of   the   users   name';
            datas: [{
                element: '.addnewbtn',
                title: 'Teams',
                description: 'Perfect.   Now   let’s   click   the   plus   on   the   right   side   of   the   users   name',
                data: 'custom Data',
                position: 'right'
            }]
            //doSpeech();
            //voiceOnboarding(datas);
            //$(".next").click();
            // appendHtml();
            setTimeout(function () {
                directionsYes = 'userTeamClicked';
                msgToSpeech = '';
                msgToSpeech = 'Have   you   clicked   the   plus sign?   Yes   or   No';
                //var da = {
                //    element: '.addnewbtn',
                //    title: 'Clicked Plus Sign',
                //    description: 'Have   you   clicked   the   plus sign?   Yes   or   No',
                //    data: 'custom Data',
                //    position: 'right'
                //}
                doSpeech();
                //datas.push(daa);
                //voiceOnboarding(datas);
                //$(".next").click();
                $(".next").click();
                // appendHtml();

            }, 6000);
        }
        if (currentStep === "step=25") {

            msgToSpeech = 'Perfect.   Now   let’s   click   the   plus   on   the   right   side   of   the   users   name';
            datas: [{
                element: '.addnewbtn',
                title: 'View Team',
                description: 'Perfect.   Now   let’s   click   the   plus   on   the   right   side   of   the   users   name',
                data: 'custom Data',
                position: 'right'
            }]
            doSpeech();
            //$(".next").click();
            voiceOnboarding(datas);
            // $(".next").click();
            // appendHtml();
            setTimeout(function () {
                directionsYes = 'concept';
                msgToSpeech = '';
                msgToSpeech = 'Do you understand this concept?   Yes   or   No';
                //var das = {
                //    element: '.addnewbtn',
                //    title: 'Understand Concept',
                //    description: 'Do you understand this concept?   Yes   or   No',
                //    data: 'custom Data',
                //    position: 'right'
                //};
                doSpeech();
                //$(".next").click();
                //$(".next").click();
                // datas.push(das);
                //voiceOnboarding(datas);
                $(".next").click();
                //appendHtml();

            }, 8000);
        }
        if (currentStep === "step=26") {
            msgToSpeech = '';
            msgToSpeech = 'Congratulations.   You   have   finished   this   portion   of   training.     You   are   halfway   done.   What would   you   like   to   learn   next? Reporting or Audit Logs';
            datas = [{
                element: ".body",
                title: "Finish",
                description: "Congratulations.   You   have   finished   this   portion   of   training.     You   are   halfway   done.   What would   you   like   to   learn   next? <ul><li>Reporting.</li> <span>or</span><li>Audit Logs</li></ul>",
                data: "Custom Data",
                position: "right"
            }]
            //datas.push(cog);
            //voiceOnboarding(datas);
            //appendHtml();
            //  $(".next").click();
            //$(".next").click();
            //$(".next").click();
        }
        if (currentStep === "step=27") {
            msgToSpeech = '';
            msgToSpeech += 'Let’s   learn   reporting.   Please   navigate   to   the   dashboard   by   clicking   on   it   on   the   left ';
            msgToSpeech += 'side   navigation   bar. On   the   dashboard   we   have   two   reports. The   first   is   the   completed';
            msgToSpeech += 'objections   report. Which   is   located   at   the   top   of   the   dashboard.  Do   you   see   the   completed';
            msgToSpeech += 'objections   report   on   the   dashboard? Yes or   No';
            datas = [{
                element: ".reports",
                title: "Reporting?",
                description: "Let’s   learn   reporting.   Please   navigate   to   the   dashboard   by   clicking   on   it   on   the   left side   navigation   bar. On   the   dashboard   we   have   two reports. The   first   is   the   completed objections   report. Which   is   located   at   the   top   of   the   dashboard.  Do   you   see   the   completed objections   report   on   the   dashboard? <a id='28' href='#' class='goToStep'>Yes</a>  or   <a href='#'>No</a>",
                data: "Custom Data",
                position: "right"
            }];
            //datas.push(rep);
            //voiceOnboarding(datas);
            doSpeech();
            //$(".next").click();
            //appendHtml();
            directions = "dashboardReports";
        }
        if (currentStep === "step=28") {
            msgToSpeech = '';
            msgToSpeech += 'Great.   This   report   shows   the   number   of   objections   a   user   has   correctly   completed ';

            msgToSpeech += 'versus   the   number   they   incorrectly   completed. There   are   three   ways   to   filter   the   report. First';
            msgToSpeech += ' let’s   look   at   sorting   by   the   date. Look   at   the   date in the   upper   left   hand   corner   of   the   report.';
            doSpeech();
            datas = [{
                element: ".dates",
                title: "Dates",
                description: "Great.This   report   shows   the   number   of   objections   a   user   has   correctly   completed versus   the   number   they   incorrectly   completed.There   are   three   ways   to   filter   the   report.First  let’s   look   at   sorting   by   the   date.Look   at   the   date in the   upper   left   hand   corner   of   the   report.",
                data: "Custom Data",
                position: "right"
            }];
            //datas.push(dts);
            //voiceOnboarding(datas);
            // $(".next").click();
            // appendHtml();
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Do   you   see   the   date ? Yes or   No';
                doSpeech();
                //var dts = {
                //    element: ".dates",
                //    title: "Dates",
                //    description: "Do   you   see   the   date ? Yes or   No",
                //    data: "Custom Data",
                //    position: "right"
                //};
                //datas.push(dts);
                //voiceOnboarding(datas);
                // appendHtml();
                $(".next").click();
                //$(".next").click();
                //$(".next").click();
                directionsYes = "Seedates";
            }, 8000);

        }
        if (currentStep === "step=29") {
            msgToSpeech = '';
            msgToSpeech = 'please   click   on   either   the   start   date,   or   end   date   to   see   how   to   adjust   the   date   range   for the   reports ';
            doSpeech();
            //var tds = {
            //    element: ".dates",
            //    title: "Dates",
            //    description: "please   click   on   either   the   start   date,   or   end   date   to   see   how   to   adjust   the   date   range   for the   reports",
            //    data: "Custom Data",
            //    position: "right"
            //};
            //datas.push(tds);
            //voiceOnboarding(datas);
            //appendHtml();
            //$(".next").click();
            //$(".next").click();
            //$(".next").click();
            //$(".next").click();
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Have   you   clicked   the   date? Yes or   No';
                doSpeech();
                //var dts = {
                //    element: ".dates",
                //    title: "Dates",
                //    description: "Have   you   clicked   the   date? Yes or   No",
                //    data: "Custom Data",
                //    position: "right"
                //};
                //datas.push(dts);
                //voiceOnboarding(datas);
                //appendHtml();
                //$("next").click();
                //$("next").click();
                //$("next").click();
                $("next").click();
                directionsYes = "clickdates";
            }, 8000);
        }
        if (currentStep === "step=30") {
            msgToSpeech = '';
            msgToSpeech = ' Next,   click   The   name   of   the   month   and   year   in   order   to   see   how   you   can   adjust   the year   and   month.';
            doSpeech();
            //$(".next").click();
            //appendHtml();
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Do   you   feel   comfortable   adjusting   the   date?   Yes  or   No';
                doSpeech();
                directionsYes = "datecomfort";
                //var dts = {
                //    element: ".dates",
                //    title: "Feeling Comfortable",
                //    description: "Do   you   feel   comfortable   adjusting   the   date?   Yes  or   No",
                //    data: "Custom Data",
                //    position: "right"
                //};
                //datas.push(dts);
                //voiceOnboarding(datas);
                appendHtml();
                $(".next").click();
                //$(".next").click();
                //$(".next").click();
                //$(".next").click();
                //$(".next").click();
            }, 8000);
        }
        if (currentStep === "step=31") {
            msgToSpeech = '';
            msgToSpeech = 'Great.   Let’s   move   on   to   the   second   way   to   filter   the   report.   Please   direct   your   eyes   to  the   upper   right   toggle   buttons';
            doSpeech();
            //var gts = {
            //    element: ".togglebtns",
            //    title: "Toggle Buttons",
            //    description: "Great.   Let’s   move   on   to   the   second   way   to   filter   the   report.   Please   direct   your   eyes   to  the   upper   right   toggle   buttons",
            //    data: "Custom Data",
            //    position: "right"
            //};
            //datas.push(gts);
            //voiceOnboarding(datas);
            // $(".next").click();
            //appendHtml();
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Do   you   see   the   toggle   buttons?   Yes   or   No';
                doSpeech();
                //var gts = {
                //    element: ".togglebtns",
                //    title: "Toggle Buttons",
                //    description: "Do   you   see   the   toggle   buttons?   Yes   or   No",
                //    data: "Custom Data",
                //    position: "right"
                //};
                //datas.push(gts);
                //voiceOnboarding(datas);
                //$(".next").click();
                $(".next").click();
                appendHtml();

                directionsYes = "toggles";
            }, 8000);
        }
        if (currentStep === "step=32") {
            msgToSpeech = '';
            msgToSpeech = 'Fantastic.   Feel   free   to   move   the   buttons   around   to   see   what   the   report   looks   like   when filtered   by   user, by   manager, and   by   collection';
            doSpeech();
            //var tls = {
            //    element: ".togglebtns",
            //    title: "Toggle Buttons",
            //    description:"Fantastic.   Feel   free   to   move   the   buttons   around   to   see   what   the   report   looks   like   when filtered   by   user, by   manager, and   by   collection",
            //    data: "Custom Data",
            //    position: "right"
            //};
            //datas.push(tls);
            //voiceOnboarding(datas);
            //$(".next").click();
            //$(".next").click();
            //$(".next").click();
            //appendHtml();

            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Do   you   feel comfortable   with   the   toggle   buttons ? Yes or   No ?';
                doSpeech();
                //var gts = {
                //    element: ".togglebtns",
                //    title: "Toggle Buttons",
                //    description: "Do   you   feel comfortable   with   the   toggle   buttons? Yes or   No?",
                //    data: "Custom Data",
                //    position: "right"
                //};
                //datas.push(gts);
                //voiceOnboarding(datas);
                //$(".next").click();
                //$(".next").click();
                //$(".next").click();
                $(".next").click();
                appendHtml();

                directionsYes = "toggleComfort";
            }, 8000);
        }
        if (currentStep === "step=33") {
            msgToSpeech = '';
            msgToSpeech = 'Awesome.   On   to   the   last   way   to   filter   a   report.   Please   direct   your   eyes   to   the   mid-right  section   of   the   report   where   it   has   a   users   name   with   a   line   color   next   to   it';
            doSpeech();
            //var fl = {
            //    element: ".highcharts-a11y-proxy-button",
            //    title: "Filter Reports",
            //    description: "Awesome.   On   to   the   last   way   to   filter   a   report.   Please   direct   your   eyes   to   the   mid-right  section   of   the   report   where   it   has   a   users   name   with   a   line   color   next   to   it",
            //    data: "Custom Data",
            //    position: "left"
            //}
            //datas.push(fl);
            //voiceOnboarding(datas);
            //appendHtml();
            //  $(".next").click();
            //$(".next").click();
            //$(".next").click();
            //$(".next").click();
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Do   you   see   it?   Yes,   or   No';
                doSpeech();
                //var fl = {
                //    element: ".highcharts-a11y-proxy-button",
                //    title: "Filter Reports",
                //    description: "Do   you   see   it?   Yes,   or   No",
                //    data: "Custom Data",
                //    position: "left"
                //}
                //datas.push(fl);
                //voiceOnboarding(datas);
                appendHtml();
                $(".next").click();
                //$(".next").click();
                //$(".next").click();
                //$(".next").click();
                //$(".next").click();
                directionsYes = "filterReports";
            }, 8000);
        }
        if (currentStep === "step=34") {
            msgToSpeech = '';
            msgToSpeech += 'Please   click   on   a   user   to   see   the   name   grey   out,   which   means   that   the   user   is   no   longer';

            msgToSpeech += 'visible   on   the   graph. Then   re-click   the   user   to   see   the   name   gain   color   meaning   they   are   visible';
            msgToSpeech += 'on   the   graph.';
            doSpeech();
            // $(".next").click();
            //appendHtml();
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Do   you   feel   comfortable   with   this?   Yes   or   No';
                doSpeech();
                $(".next").click();
                //  appendHtml();
                directionsYes = "filterUsers";
            }, 8000);
        }
        if (currentStep === "step=35") {
            msgToSpeech = '';
            msgToSpeech += 'Perfect.   That   completes   report   one   and   filtering.   On   to   our   second   report.   Please   scroll';
            msgToSpeech += 'down   on   the   dashboard   page   until   you   see   a   report   that   says   Confidence   and   Consistency';
            msgToSpeech += 'score';
            doSpeech();
            //   $(".next").click();
            //appendHtml();

            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Do   you   see   the   confidence   and   consistency   score   report?   Yes           or   No';
                doSpeech();
                $(".next").click();
                //  appendHtml();
                directionsYes = "CCSRep";
            }, 8000);
        }
        if (currentStep === "step=36") {
            msgToSpeech = '';
            msgToSpeech += 'Fantastic.   Please   click   the   round   black   dot   with   the   white   letter   i   in   the   middle.   This   will ';

            msgToSpeech += 'show   you   the   definition   of   this   report';

            doSpeech();
            //var its = {
            //    element: ".zmdi-info",
            //    title: "CCS Report",
            //    description: "Fantastic.   Please   click   the   round   black   dot   with   the   white   letter   i   in   the   middle.   This   will show   you   the   definition   of   this   report ",
            //    data: "Custom Data",
            //    position: "right"
            //};
            //datas.push(its);
            //voiceOnboarding(datas);
            //appendHtml();
            $(".next").click();
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Did   you   click   the   information   icon?   Yes,   or   No';
                doSpeech();
                directionsYes = "infoicn";
                //var its = {
                //    element: ".zmdi-info",
                //    title: "CCS Report",
                //    description: Did   you   click   the   information   icon?   Yes, or   No,
                //    data: "Custom Data",
                //    position: "right"
                //};
                //datas.push(its);
                //voiceOnboarding(datas);
                appendHtml();
                $(".next").click();
                //  $(".next").click();

            }, 8000);
        }
        if (currentStep === "step=37") {
            msgToSpeech = '';
            msgToSpeech += 'Awesome.   This   chart   is   shown   in   a   bar   and   line   graph.   Like   the   report   we   went   through ';
            msgToSpeech += 'before   it   can   be   filtered   by   date, by   toggle   buttons, and   by   adding/subtracting   items   from   the';
            msgToSpeech += ' chart. Feel   free   to   explore   this   report   for   a   few   moments';
            //  $(".next").click();
            // appendHtml();
            doSpeech();

            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Do   you   feel   comfortable   with   the   confidence   and   consistency   report?   Yes,   or   No';
                doSpeech();
                directionsYes = "ccsComfort";
                $(".next").click();
                //appendHtml();
            }, 8000);
        }
        if (currentStep === "step=38") {
            msgToSpeech = '';
            msgToSpeech += 'Fantastic.   Please   navigate   to   the   reports   tab   on   the   left   navigation   bar';
            doSpeech();
            //var rts = {
            //    element: ".reports",
            //    title: "CCS Report",
            //    description: "Fantastic.   Please   navigate   to   the   reports   tab   on   the   left   navigation   bar",
            //    data: "Custom Data",
            //    position: "right"
            //};
            //datas.push(rts);
            //voiceOnboarding(datas);
            //$(".next").click(); $(".next").click();
            //$(".next").click();
            //$(".next").click();
        }
        if (currentStep === "step=39") {
            msgToSpeech = '';
            msgToSpeech += 'Are you on reports page.? Yes or No';
            doSpeech();
            datas = [{
                element: ".body",
                title: "Report",
                description: msgToSpeech,
                position: "right"
            },
            {
                element: ".body",
                title: "Report",
                position: "right",
                data: "Custom Data",
                description: "Perfect. This   page   is   similar   to   the   dashboard, but   you   only   see   reports   on   it. Feel   free   to take   a   few   moments   and   explore   it"
            },
            {
                element: ".body",
                title: "Report",
                description: "Do   you   feel   comfortable   with   this   tab?   Yes,   or   No",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".auditLogs",
                title: "Audit Logs",
                description: "Congratulations.   You   finished   this   part   of   the   training.   You   are   now   two-thirds   of   the   way  through. On   to   the   last   portion: Audit   logs. Please use   the   left-side   navigation   bar   to   go   to   the   tab   that   says   Audit   Logs",
                data: "Custom Data",
                position: "right"
            }
            ];
            voiceOnboarding(datas);
            //  appendHtml();
            directionsYes = "reportsYes";
        }
        if (currentStep === "step=40") {
            msgToSpeech = '';
            msgToSpeech = 'Perfect.   This   page   is   similar   to   the   dashboard,   but   you   only   see   reports   on   it.   Feel   free   to take   a   few   moments   and   explore   it';
            doSpeech();
            setTimeout(function () {
                msgToSpeech = '';
                msgToSpeech = 'Do   you   feel   comfortable   with   this   tab?   Yes,   or   No';
                directionsYes = "menuReports";
                //doSpeech();
                //var tas = [{
                //    element: ".body",
                //    title: "Report",
                //    description: "Do   you   feel   comfortable   with   this   tab?   Yes,   or   No",
                //    data: "Custom Data",
                //    position: "right"
                //}];
                //datas.push(tas);
                //voiceOnboarding(datas);
                // appendHtml();
                $(".next").click();
            }, 10000);
        }
        if (currentStep === "step=41") {
            msgToSpeech = '';
            msgToSpeech += ' Congratulations.   You   finished   this   part   of   the   training.   You   are   now   two-thirds   of   the   way ';
            msgToSpeech += 'through. On   to   the   last   portion: Audit   logs. Please   use   the   left-side   navigation   bar   to   go   to   the';
            msgToSpeech += ' tab   that   says   Audit   Logs';
            doSpeech();
            //var ats = {
            //    element: ".auditLogs",
            //    title: "Audit Logs",
            //    description: "Congratulations.   You   finished   this   part   of   the   training.   You   are   now   two-thirds   of   the   way  through. On   to   the   last   portion: Audit   logs. Please use   the   left-side   navigation   bar   to   go   to   the   tab   that   says   Audit   Logs",
            //    data: "Custom Data",
            //    position: "right"
            //};
            //datas.push(ats);
            //voiceOnboarding(datas);
            //$(".next").click(); $(".next").click();
            //    $(".next").click();
            //appendHtml();
        }
        if (currentStep === "step=42") {
            msgToSpeech = 'Are you on the audit logs page? <a href="#" id="43" class="goToStep">yes </a><a href="#" id="No" class="">or No?</a>';
            directionsYes = 'auditLogs';
            datas = [{
                element: ".auditLogs",
                title: "Audit Logs",
                description: msgToSpeech,
                data: "Custom Data",
                position: "top"
            },
            {
                step: '43',
                element: ".auditLogs",
                title: "Audit Logs",
                description: "Great.   This   area   shows   you   anything   that   has   been   changed   in   the   system   and   who changed   it. You   can   filter   this   report   by   clicking   on  any   of   the   up, or   down   arrows   next   to   the names in the   header   section. Please   take   a   few   moments   to   make   yourself   comfortable   with   this section.",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".auditLogs",
                title: "Audit Logs",
                description: "Do   you   feel   comfortable   with   audit   logs?   Yes,   or   No?",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".auditLogs",
                title: "Audit Logs",
                description: "Congratulations!   You   finished   learning   audit   logs.   You   have    now   successfully completed   onboarding. Would   you   like   to   take   the   onboarding   quiz ? Yes, or   No ?",
                data: "Custom Data",
                position: "right"
            },
            {
                element: ".body",
                title: "Practice Objections",
                description: "Please   go   to   the   practice   objections   tab   and   select   the   collection   Onboarding   Quiz   and then   say   give   me   any   objection",
                data: "Custom Data",
                position: "right"
            }];
            doSpeech();
            msgToSpeech = '';
            //voiceOnboarding(datas);
            //appendHtml();
        }
        if (currentStep === "step=43") {
            msgToSpeech = '';
            msgToSpeech += '   Great.   This   area   shows   you   anything   that   has   been   changed   in   the   system   and   who ';
            msgToSpeech += 'changed   it. You   can   filter   this   report   by   clicking   on   any   of   the   up, or   down   arrows   next   to   the';
            msgToSpeech += 'names in the   header   section. Please   take   a   few   moments   to   make   yourself   comfortable   with   this ';
            msgToSpeech += 'section.';
            doSpeech();
            datas = [{
                element: ".auditLogs",
                title: "Audit Logs",
                description: "Great.   This   area   shows   you   anything   that   has   been   changed   in   the   system   and   who changed   it. You   can   filter   this   report   by   clicking   on  any   of   the   up, or   down   arrows   next   to   the names in the   header   section. Please   take   a   few   moments   to   make   yourself   comfortable   with   this section.",
                data: "Custom Data",
                position: "top"
            }];
            //doSpeech();
            //msgToSpeech = '';
            //datas.push(atas);
            //voiceOnboarding(datas);
            //appendHtml();
            //  $(".next").click();
            var pause = readCookie("Ispause");
            if (pause === null) {
                setTimeout(function () {
                    msgToSpeech = '';
                    directionsYes = 'auditCompleted';
                    msgToSpeech = 'Do   you   feel   comfortable   with   audit   logs?   Yes,   or   No?';
                    doSpeech();
                    //var atas = [{
                    //    element: ".auditLogs",
                    //    title: "Audit Logs",
                    //    description: "Do   you   feel   comfortable   with   audit   logs?   Yes,   or   No?",
                    //    data: "Custom Data",
                    //    position: "left"
                    //}];
                    //doSpeech();
                    //msgToSpeech = '';
                    //datas.push(atas);
                    //voiceOnboarding(datas);
                    // appendHtml();
                    //$(".next").click();
                    $(".next").click();
                }, 10000);
            }
        }
        if (currentStep === "step=44") {
            msgToSpeech = '';
            directionsYes = 'quizYes';
            msgToSpeech += 'Congratulations!   You   finished   learning   audit   logs.   You   have    now   successfully ';
            msgToSpeech += 'completed   onboarding. Would   you   like   to   take   the   onboarding   quiz ? Yes, or   No ?';
            doSpeech();
            //var tas = [{
            //    element: ".auditLogs",
            //    title: "Audit Logs",
            //    description: "Congratulations!   You   finished   learning   audit   logs.   You   have    now   successfully completed   onboarding. Would   you   like   to   take   the   onboarding   quiz ? Yes, or   No ?",
            //    data: "Custom Data",
            //    position: "left"
            //}];
            //doSpeech();
            msgToSpeech = '';
            //datas.push(tas);
            //voiceOnboarding(datas);
            //appendHtml();
            //$(".next").click();
            //$(".next").click();
            //   $(".next").click();
        }
        if (currentStep === "step=45") {
            msgToSpeech = '';
            msgToSpeech += 'Please   go   to   the   practice   objections   tab   and   select   the   collection   Onboarding   Quiz   and';

            msgToSpeech += 'then   say   give   me   any   objection';
            doSpeech();

            msgToSpeech = '';
        }

    }

}
function CreategrantPermissionCookie(isGranted) {

    var expires;
    var date = new Date();
    date.setDate(date.getDate() + 20);
    expires = "; expires=" + date.toGMTString();
    //    var av = "username=" + username + ",step=" + step + ";" + "expiry=" + expires + ";path=/";
    var av = "isGranted=" + username + ",isGranted=" + isGranted + ";" + "expiry=" + expires + ";path=/";
    document.cookie = av;
}
function readGrantCookie(name) {
    var nameEQ = encodeURIComponent(name) + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ')
            c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0)
            return decodeURIComponent(c.substring(nameEQ.length, c.length));
    }
    return null;
}
//function clickevent() {
$(document.body).on("click", ".goToStep", function () {
    var step = $(this).attr('id');
    step = parseInt(step);
    removeSessionValues();
    createSession(step, false, "direct");
    checkSteps();
    $(".next").addClass('Hidden');
    $(".prev").addClass('Hidden');
    $(".ttour-bullets").addClass('Hidden');
    //voiceOnboarding(datas, true);
    $(".next").click();
});
//}

function timeOut() {
    setTimeout(function () {
        $("#autoplay").click();
    }, 2000);
}
$(document).ready(function () {
    GetUser();
    //navigator.permissions.revoke({ name: 'microphone' }).then(function (result) {
    //    console.log(result.state);
    //});


    //navigator.permissions.query(
    //    { name: 'microphone' }
    //).then(function (permissionStatus) {
    //    permissionStatus.state = "prompt";

    //    console.log("PErmission " + permissionStatus.state);

    //});
    //navigator.mediaDevices.getUserMedia({ audio: true })
    //    .then(function (stream) {
    //        console.log('You let me use your mic!')
    //    })
    //    .catch(function (err) {
    //        console.log('No mic for you!')
    //    });
    //Progress bar
    $(window).on('beforeunload', function () {
        synths.cancel();
    });


    // synth.cancel();
    //debugger
    //isSaved = readCookie("username");
    //debugger
    //chkStep = chkStep.split(',');
    //alert(chkStep[2]);
    //debugger

    //var url = window.location.href;

    //var chkStep = readCookie("username");


    //var chkStep = getSessionStorageValue("step");

    //chkStep = parseInt(chkStep);

    //chkStep = chkStep.split(',');
    //currentStep = chkStep[1].split('?');
    //currentStep = chkStep[1].split('?');
    //isSavedforLater = currentStep[1];
    //currentStep = currentStep[0];
    //isSavedforLater = isSavedforLater.match("isSaved=(.*)");
    //if (isSavedforLater === "false") {
    //if (url.includes("AdminCollections") || url.includes("ManagerCollections")) {
    //    //eraseCookie("username");
    //    //createCookie(4, false);
    //    //step
    //    //$('#grantPermission').click();
    //    //RecognitionCredentials();
    //    //removeSessionValues();
    //    //createSession(4, false, "");
    //    //checkSteps();
    //    //timeOut();
    //    //voiceOnboarding(datas);

    //}
    //else if (url.includes('InviteUsers')) {
    //    //eraseCookie("username");
    //    //createCookie(15, false);
    //    //removeSessionValues();
    //    //createSession(15, false, "");
    //    //checkSteps();
    //    //timeOut();

    //}
    //else if (url.includes('Teams')) {
    //    //eraseCookie("username");
    //    //createCookie(23, false);
    //    //removeSessionValues();
    //    //createSession(23, false, "");
    //    //checkSteps();
    //    //timeOut();
    //}
    //else if (url.includes("Reports")) {
    //    //eraseCookie("username");
    //    //createCookie(39, false);
    //    //removeSessionValues();
    //    //createSession(39, false, "");
    //    //checkSteps();
    //    //timeOut();
    //}
    //else if (url.includes('AuditLogs')) {

    //    //eraseCookie("username");
    //    //createCookie(42, false);
    //    //removeSessionValues();
    //    //createSession(42, false, "");
    //    //checkSteps();
    //    //timeOut();
    //}
    //else if (url.includes("List")) {
    //    //eraseCookie("username");
    //    //createCookie(17, false);
    //    //removeSessionValues();
    //    //createSession(17, false, "");
    //    //checkSteps();
    //    //timeOut();
    //}
    //else if (url.includes("PracticeObjections")) {
    //    //eraseCookie("username");
    //    removeSessionValues();
    //    createSession(12, false, "");
    //    //createCookie(12, false);
    //    checkSteps();
    //    timeOut();
    //}
    //else if (url.includes("tutorial")) {
    //    // eraseCookie("username");
    //    //createCookie(1, false);

    //    //createSession(1, false, "");
    //    //checkSteps();
    //    //timeOut();
    //}
    //else {

    //    //var chkStep = readCookie("username");
    //    if (chkStep === null) {
    //        //createCookie(1, false);
    //        //checkSteps();
    //        createSession(1, false, "");
    //    }
    //    else {
    //        var curStp = chkStep;

    //        //chkStep = chkStep.split(',');
    //        //curStp = chkStep[1].split('?');
    //        //currentStep = curStp[0];


    //        if (curStp === "step=username") {
    //            //createCookie(1, false);
    //            //checkSteps();
    //            createSession(1, false, "");
    //        }
    //        else if (curStp === "undefined") {
    //            //createCookie(1, false);
    //            //checkSteps();
    //            createSession(1, false, "");

    //        }
    //        else if (curStp === "NaN") {
    //            //createCookie(1, false);
    //            //checkSteps();
    //            createSession(1, false, "");
    //        }
    //        else {
    //            //var ns = curStp[0].split('=');//match("step=(.*)");
    //            //ns = parseInt(ns[1]);

    //            createSession(curStp, false, "");


    //        }
    //    }
    //}
    //}
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


//$(document).on("click", "#cancelPermission", function () {
//    $("#CreateNewModal").modal('hide');
//    LoadPopUp();
//    IsStepExist();

//});
// when called, activates the SpeechRecognition object so it starts listening

//function IsStepExist() {
//    setTimeout(function () {
//        if (dbCurrentStep !== "") {
//            var cs;
//            var progress;
//            cs = dbCurrentStep;
//            if (cs === '') {
//                cs = 0;
//            }
//            else {
//                cs = parseInt(cs);
//                $("#getStarted").text('');
//                $("#getStarted").text('Restart');
//                var htmlf = '<button class="btn btn-info btn-sm" id="continueWhereLeft">Countinue where I left</button><div class="text-center"><span>Step where you left onboarding: <strong>' + cs + '</strong></span></div>';
//                $("#htmlAppend").append(htmlf);
//                $(document).on("click", "#continueWhereLeft", function () {
//                    $(".ttour-shadow").css('display', 'block');
//                    checkSteps();
//                    voiceOnboarding(datas);
//                    $("#CreateNewModal").modal('hide');
//                    $.ajax({
//                        url: '/Speech/SaveStartPopUp/',
//                        success: function (res) {
//                            console.log('Saved');
//                        }
//                    });

//                });
//            }
//            progress = (cs / 45) * 100;
//            progress = Math.floor(progress);
//            $(".progress-bar").removeAttr('style');
//            $(".progress-bar").attr('style', 'width:' + progress + '%');
//            $(".progress-bar").html(progress + '%');
//        }
//    }, 200);
//    //var paragraphs = document.createElement('p');
//    containers = document.querySelector('.text-boxs');
//    containers.appendChild(paragraphs);
//    setTimeout(function () {
//        dictates();
//    }, 2000);
//};

// when called, activates the SpeechRecognition object so it starts listening

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
    recognition.onresult = (evenst) => {
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
                doTovoice(2);
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
                    // $(".next").click();
                    voiceOnboarding(datas);
                    $(".next").click();
                    //appendHtml();
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
    //eraseCookie("username");
    //createCookie(step, false);
    removeSessionValues();
    createSession(step, false, "");
    checkSteps();
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
        //if ($(this).text() === "Play") {
        //    $(this).text("Pause");
        //    synth.resume();
        //}
        else {
            $(".mphone").addClass("zmdi-mic");
            $(".mphone").removeClass("zmdi-mic-off");
            synths.resume();
            recognitions.start();
            OnPlay();
        }

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
//$(document).on("click", ".MoveToUrl" ,function () {

//});
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
//ClicknextAndHideModal
function ClickAddCollection(createbtn) {
    $(createbtn).click();
}
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
    if (percentagelevel == null || percentagelevel == undefined) {
        percentagelevel = 0;
        GetPercentage(percentagelevel)
    }
    else {
        GetPercentage(percentagelevel)
    }

}
function micPermission() {
    $("#CreateNewModal-label").html("Mic Permission");
    var htm = '';
    htm = '<div class="row"><div class="col-md-12 text-center"><span class="text-danger">Click on Allow to continue.</span></div><div class="text-right col-md-12"><button class="btn btn-danger btn-sm mr-2" id="cancelPermission">Cancel</button><button class="btn btn-primary btn-sm" id="grantPermission"> Grant Permission</button></div></div>';
    // htm = '<div class="row"><div class="col-md-12 text-center"><span><i class="fa fa-microphone"></i></span><span class="text-danger">Parasale wants to use your microphone.</span></div><div class="text-right><button class="btn btn-danger btn-sm" id="cancelPermission">Cancel</button><button class="btn btn-primary btn-sm" style = "margin-right: 5px;" id = "grantPermission"> Grant Permission</button></div></div>';
    $("#CreateNewBody").html(htm);
    $("#CreateNewModal").modal('show');
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
    //var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Get Started</button>';
    //LoadPopUp(htmlf, 0);
});
$(document).on("click", "#grantPermission", function () {
    $("#CreateNewModal").modal('hide');
    setSessionStorageItem("isGranted", true);
    RecognitionCredentials();
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
            var isGranted = getSessionStorageValue("isGranted");
            if (isGranted == null) {
                micPermission();
            }
            else {
                LoadPopUp(htmlf, ProgressLevel);
            }
        }
        else {
            var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Get Started</button>';
            var isGranted = getSessionStorageValue("isGranted");
            if (isGranted == null) {
                micPermission();
            }
            else {
                LoadPopUp(htmlf, 0);
            }
        }
    });
    var IsFirstLogin = getSessionStorageValue(FirstLogin)
    if (IsFirstLogin == "true") {
        $.ajax({
            url: '/Speech/OnFirstLogin/',
            success: function (res) {
                console.log('Saved');
            }
        });
    }

});
$(document).on('click', '#tutorial', function () {
    LoadPopupStepBasis(UserInfo)
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
        if (UserInfo == null || UserInfo == undefined) {

        }
        else {
            var chkStep = parseInt(UserInfo.currentStep);
            chkStep = parseInt(chkStep);
            if (chkStep === null) {
                createSession(1, false, "");
            }
            else {
                var curStp = chkStep;
                if (curStp === "step=username") {
                    createSession(1, false, "");
                }
                else if (curStp === "undefined") {
                    createSession(1, false, "");
                }
                else if (curStp === "NaN") {
                    createSession(1, false, "");
                }
                else {
                    createSession(curStp, false, "");
                }
            }
            if (UserInfo.isFirstLogin == true || UserInfo.isFirstLogin == null || UserInfo.isFirstLogin == '') {
                LoadPopupStepBasis(UserInfo)
                setSessionStorageItem(FirstLogin, UserInfo.isFirstLogin)
            }
        }
    });
}
function LoadPopupStepBasis(data) {
    if (data == undefined) {
        console.log('undefined')
        $.getJSON("/Speech/GetUser", function (data) {
            UserInfo = data;
            var isGranted;
            var cs;
            var progressLevel;
            if (data.currentStep > 1) {
                cs = parseInt(data.currentStep);
                progressLevel = parseInt(data.stepLevel);
                $("#getStarted").text('');
                $("#getStarted").text('Restart');
                var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Restart</button>';
                htmlf += '<button class="btn btn-info btn-sm" id="continueWhereLeft">Countinue where I left</button><div class="text-center"><span>Step where you left onboarding: <strong>' + data.stepTitle + '</strong></span></div>';
                isGranted = getSessionStorageValue("isGranted");
                if (isGranted == null) {
                    micPermission();
                }
                else {
                    LoadPopUp(htmlf, progressLevel);
                }
            }
            else {
                isGranted = getSessionStorageValue("isGranted");
                var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Get Started</button>';
                isGranted = getSessionStorageValue("isGranted");
                if (isGranted == null) {
                    micPermission();
                }
                else {
                    LoadPopUp(htmlf, 0);
                }
            }
        });
    }
    else {
        var isGranted;
        var cs;
        var progressLevel;
        if (data.currentStep > 1) {
            console.log('step >1')
            cs = parseInt(data.currentStep);
            progressLevel = parseInt(data.stepLevel);
            $("#getStarted").text('');
            $("#getStarted").text('Restart');
            var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Restart</button>';
            htmlf += '<button class="btn btn-info btn-sm" id="continueWhereLeft">Countinue where I left</button><div class="text-center"><span>Step where you left onboarding: <strong>' + data.stepTitle + '</strong></span></div>';
            isGranted = getSessionStorageValue("isGranted");
            if (isGranted == null) {
                console.log('isGrantedVal =' + isGranted)
                micPermission();
            }
            else {
                console.log('isGranted != null(else)')
                LoadPopUp(htmlf, progressLevel);
            }
        }
        else {
            isGranted = getSessionStorageValue("isGranted");
            var htmlf = '<button class="btn btn-primary btn-sm" style="margin-right: 5px;" id="getStarted">Get Started</button>';
            isGranted = getSessionStorageValue("isGranted");
            if (isGranted == null) {
                micPermission();
            }
            else {
                LoadPopUp(htmlf, 0);
            }

        }
    }
}
$(document).on('click', '#getStarted', function () {
    stepLevel = 1;
    createSession(1, false, "");
    setSessionStorageItem(StartPopUp, false);
    setSessionStorageItem(StepName, "Welcome");
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