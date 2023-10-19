window.AudioContext = window.AudioContext || window.webkitAudioContext;

var stream = null;
var audioChunks = [];
var recordAudio = () =>
    new Promise(async resolve => {

        stream = await navigator.mediaDevices.getUserMedia({ audio: true });
        const mediaRecorder = new MediaRecorder(stream);

        mediaRecorder.addEventListener("dataavailable", event => {
            audioChunks.push(event.data);
            //alert(mediaRecorder);
        });

        const start = () => mediaRecorder.start();

        const stop = () =>
            new Promise(resolve => {
                mediaRecorder.addEventListener("stop", () => {

                    const audioBlob = new Blob(audioChunks);
                    const audioUrl = URL.createObjectURL(audioBlob);
                    //alert(audioUrl);
                    const audio = new Audio(audioUrl);
                    const play = () => audio.play();
                    resolve({ audioBlob, audioUrl, play });
                });

                mediaRecorder.stop();
            });

        resolve({ start, stop });
    });

const sleep = time => new Promise(resolve => setTimeout(resolve, time));


$("button.microphone").on("click", function () {
    handleAction();
});

const handleAction = async () => {
    const recorder = await recordAudio();
    const actionButton = document.getElementById('microphones');
    actionButton.disabled = true;
    recorder.start();
    await sleep(3000);
    const audio = await recorder.stop();
    var audioctx = new AudioContext();
    var source = audioctx.createMediaStreamSource(stream);
    //alert(stream.getVoices());
    audio.play();
    await sleep(3000);
    actionButton.disabled = false;
};
