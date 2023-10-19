//import { setTimeout } from "core-js";

window.SpeechRecognition = window.SpeechRecognition || webkitSpeechRecognition;
const recognition = new SpeechRecognition();
recognition.interimResults = true;
const synth = window.speechSynthesis;
const icon = document.querySelector('button#autoplay');
var listening = true;
var msgToSpeech;
var directions;
icon.addEventListener('click', () => {
    dictate();
    recognition.start();
    doSpeech();
});
function doSpeech() {
    var synth = window.speechSynthesis;
    var msg = new SpeechSynthesisUtterance();
    msg.text = msgToSpeech;
    msg.lang = 'en-US';
    msg.addEventListener('start', function () {
        if (synth.speaking) {
            console.log("This will be printed !");
        }
    });

    // if (confirm("Click OK to speak")) {
    synth.speak(msg);
    //}
    var html = '';
    if (directions === "practicing objections") {
        html = '';
        html += '<div class="row">';
        html += '<div class="col-md-12">';
        html += '<span id="1">1. Installing Quick Start Collections</span><br>';
        html += '<span id="2">2. Building collections and objections</span><br>';
        html += '<span id="3">3. Practicing objections</span><br>';
        html += '</div>';
        html += '</div>';
        setTimeout(function () {
            $("#CreateNewModal-label").html("Select One");
            $("#CreateNewBody").html(html);
            $("#CreateNewModal").modal('show');
        }, 10000);

    }

    if (directions === "installing quick start collections" || directions === "installing Quickstart collections") {

        $("ul.list a[href='/Admin']:eq(1)").closest('li').addClass('onboardingStyle');
        html = '';
        html += '<div class="row">';
        html += '<div class="col-md-12">';
        html += '<img src="/images/onboardingImages/quickstart.png" />';
        html += '</div>';
        html += '</div>';
        $("#CreateNewModal-label").html("Quick Start");
        $("#CreateNewBody").html(html);
        $("#CreateNewModal").modal('show');
        msgToSpeech = "Have you clicked quick start tab? Yes or No.";
        setTimeout(function () {
            doSpeech();
            msgToSpeech = '';
        }, 10000);
    }
}

$(document).ready(function () {
    var html = '';
    var alreadyLoaded;
    var url = window.location.href;
    if (url.includes("AdminCollections")) {
        setTimeout(function () {
            alreadyLoaded = true;
        }, 60000);
        if (!alreadyLoaded) {
            msgToSpeech = "That was quick. Now, let   me   explain   what   a   quick   start   collection   is. A   quick   start   collection   is   an   easy   and   automatically   set   up   group   of   collections   you   can   push   to users, so   they   can   begin   practicing   with   Parasale in a   matter   of   moments.The   way   to   install   a         quick   start   collection   is   by   clicking   on   the   blue   arrow   shown   next   to   the   collection   name. Please        click   on   any   blue   arrows   you   would   like   to   add   to   your   collections.";
            html = '';
            html += '<div class="row">';
            html += '<div class="col-md-12">';
            html += '<div>';
            html += 'That was quick. Now, let   me   explain   what   a   quick   start   collection   is. A   quick   start   collection   is   an   easy   and   automatically   set   up   group   of   collections   you   can   push   to users, so   they   can   begin   practicing   with   Parasale in a   matter   of   moments.The   way   to   install   a         quick   start   collection   is   by   clicking   on   the   blue   arrow   shown   next   to   the   collection   name. Please        click   on   any   blue   arrows   you   would   like   to   add   to   your   collections.';
            html += '</div>';
            html += '<img src="/images/onboardingImages/addToCollections.png" />';
            html += '</div>';
            html += '</div>';
            $("#CreateNewModal-label").html("Add To Collections");
            $("#CreateNewBody").html(html);
            $("#CreateNewModal").modal('show');

            setTimeout(function () {
                msgToSpeech = "Are   you   done   with   clicking   any   blue   arrows   you   want ? Yes, or   No ?";
                setTimeout(function () {
                    doSpeech();
                }, 5000);
            }, 5000);
        }
    }
    else {
        msgToSpeech = "Welcome   to  Parasale. My   name   is   Para   I   am   your   self   onboarding   agent. We   are   so   glad   to   have   you. Please   state   what   you   would   like   to   learn   first   from   the   following   list:Onboarding   users   and   creating   Teams. Practicing   objections. Reporting. Audit   logs";
        html = '';
        html += '<div class="row">';
        html += '<div class="col-md-12">';
        html += '<div>';
        html += 'Welcome   to  Parasale.My   name   is   Para   I   am   your   self   onboarding   agent.We   are   so   glad   to   have   you.Please   state   what   you   would   like   to   learn   first   from   the   following   list: Onboarding   users   and   creating   Teams.Practicing   objections.Reporting.Audit   logs';
        html += '</div>';
        html += '<span id="1">1. Onboarding users and creating Teams</span><br>';
        html += '<span id="2">2. Practicing Objections</span><br>';
        html += '<span id="3">3. Reporting</span><br>';
        html += '<span id="4">4. Audit Logs</span>';
        html += '</div>';
        html += '</div>';
        $("#CreateNewModal-label").html("Select One");
        $("#CreateNewBody").html(html);
        $("#CreateNewModal").modal('show');
    }
    setTimeout(function () {
        $("#autoplay").click();
    }, 2000);
});

// when called, activates the SpeechRecognition object so it starts listening
recognition.onstart = function () {
    listening = true;
};

// when called, stops listening to the microphone
recognition.onend = function () {
    recognition.stop();

};
recognition.onspeechend = function () {
    recognition.stop();
};


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
        //console.log("Words Per Minutes: " + WPM);
        directions = speechToText;
        // adds a new visual elements to the screen
        if (event.results[0].isFinal) {

            // user initiates a random objection script
            if (speechToText.includes('practicing objections')) {
                $("#CreateNewModal").modal('hide');
                msgToSpeech = '';
                msgToSpeech = "Great. Let’s talk about practicing   objections. Practicing   objections   involves   three   things, which   would   you   like   to   start with   first ? ";
                doSpeech();
            }
            if (speechToText.includes('installing quick start collections') || speechToText.toLocaleLowerCase().includes('installing Quickstart collections')) {

                $("#CreateNewModal").modal('hide');
                msgToSpeech = '';
                msgToSpeech = "Fantastic. Let’s work learning how to install quick start collections. Please click on the quick start collections tab on the left side menu navigation bar.";
                doSpeech();
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
                alert("No")
            }

            else {
                //setTimeout(function () {
                //    speak(scoreResponse(speechToText.toLocaleLowerCase()));
                //    if ($("#randomVoices").prop("checked")) {
                //        randomVoices();
                //    }

                //    if (completedIs) {
                //        if (checkCmdType === "give me") {
                //            remainig = remainig - 1;
                //            completed = completed + 1;
                //            xObjections();
                //            updatehtml();
                //        }
                //        else {

                //            if (minutes > 0 || seconds > 0) {

                //                xObjections();
                //            }
                //            else {
                //                synth.cancel();
                //            }
                //        }
                //    }

                //}, 2000);
                return;
            }
        }
    };
    recognition.onend = recognition.start;
};
