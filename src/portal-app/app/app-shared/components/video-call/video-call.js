modules.component('videoCall', {
    templateUrl: '/app/app-shared/components/video-call/index.html',
    controller: [
        function () {
            var ctrl = this;
            ctrl.constraints = {
                audio: {
                    
                },
                video: {
                    
                }
            };
            ctrl.isPlayed = false;
            ctrl.videoElement = document.querySelector('video');
            ctrl.audioSelect = document.querySelector('select#audioSource');
            ctrl.videoSelect = document.querySelector('select#videoSource');
            ctrl.init = function () {                
                navigator.mediaDevices.enumerateDevices()
                    .then(ctrl.gotDevices).then(ctrl.getStream).catch(ctrl.handleError);
            };
            ctrl.gotDevices = function (deviceInfos) {
                for (let i = 0; i !== deviceInfos.length; ++i) {
                    const deviceInfo = deviceInfos[i];
                    const option = document.createElement('option');
                    option.value = deviceInfo.deviceId;
                    if (deviceInfo.kind === 'audioinput') {
                        option.text = deviceInfo.label ||
                            'microphone ' + (ctrl.audioSelect.length + 1);
                        ctrl.audioSelect.appendChild(option);
                    } else if (deviceInfo.kind === 'videoinput') {
                        option.text = deviceInfo.label || 'camera ' +
                            (ctrl.videoSelect.length + 1);
                            ctrl.videoSelect.appendChild(option);
                    } else {
                        console.log('Found another kind of device: ', deviceInfo);
                    }
                }
            };
            ctrl.getStream = function () {
                if (window.stream) {
                    window.stream.getTracks().forEach(function (track) {
                        track.stop();
                    });
                }
                ctrl.constraints.audio.deviceId = { exact: ctrl.audioSelect.value };
                ctrl.constraints.video.deviceId = { exact: ctrl.videoSelect.value };
                navigator.mediaDevices.getUserMedia(ctrl.constraints).
                    then(ctrl.gotStream).catch(ctrl.handleError);
            };
            ctrl.gotStream = function (stream) {
                window.stream = stream; // make stream available to console
                ctrl.videoElement.srcObject = stream;
            };

            ctrl.handleError = function (error) {
                console.error('Error: ', error);
            };
            ctrl.play = function () {
                ctrl.video.play();
                ctrl.isPlayed = true;
            };
            ctrl.stop = function () {
                ctrl.video.stop();
                ctrl.isPlayed = false;
            };
        }],
    bindings: {
        message: '='
    }
});