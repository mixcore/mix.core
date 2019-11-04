modules.component('uploadCroppie', {
    templateUrl: '/app/app-shared/components/upload-croppie/view.html?v=1',
    bindings: {
        header: '=',
        description: '=',
        src: '=',
        srcUrl: '=',
        postedFile: '=',
        type: '=',
        folder: '=',
        auto: '=',
        onDelete: '&',
        onUpdate: '&'
    },
    controller: ['$rootScope', '$scope', '$http', 'ngAppSettings', function ($rootScope, $scope, $http, ngAppSettings) {
        var ctrl = this;
        ctrl.options = {
            boundary: { width: 250, height: 377 }
        };
        ctrl.isAdmin = $rootScope.isAdmin;
        var image_placeholder = '/assets/img/image_placeholder.jpg';
        ctrl.isImage = false;
        ctrl.mediaNavs = [];
        ctrl.$onInit = function () {
            ctrl.srcUrl = ctrl.srcUrl || image_placeholder;
            ctrl.isImage = ctrl.srcUrl.toLowerCase().match(/([/|.|\w|\s|-])*\.(?:jpg|jpeg|gif|png|svg)/g);
            ctrl.maxHeight = ctrl.maxHeight || '2000px';
            ctrl.id = Math.floor(Math.random() * 100);
            ctrl.cropped = {
                source: 'https://localhost:5001/assets/img/image_placeholder.jpg'
            };
            var frameUrl = '/content/templates/tsets/uploads/2019-10/730149275529721421464195891692074859757568n0037047f8f6f4adab55211aee3538155.png';//$rootScope.settings.data['frame_url'] 
            frameUrl = frameUrl || 'https://travel.mixcore.org/content/templates/travel-together/uploads/2019-10/7329952728167098616815215831685433204932608nf5ae77416f8b46c99eaf85901a5a4517.png';
            ctrl.frame = ctrl.loadImage(frameUrl);
            if (ctrl.isImage) {
                var ext = ctrl.srcUrl.substring(ctrl.srcUrl.lastIndexOf('.') + 1);
                $http({
                    method: 'GET',
                    url: ctrl.srcUrl,
                    responseType: 'arraybuffer'
                }).then(function (resp) {
                    ctrl.cropped.source = "data:image/" + ext + ";base64," + ctrl._arrayBufferToBase64(resp.data);

                });
            }
            // Assign blob to component when selecting a image           
        };
        ctrl.$doCheck = function () {
            if (ctrl.src !== ctrl.srcUrl && ctrl.srcUrl != image_placeholder) {
                ctrl.src = ctrl.srcUrl;
                ctrl.isImage = ctrl.srcUrl.toLowerCase().match(/([/|.|\w|\s|-])*\.(?:jpg|jpeg|gif|png|svg)/g);
                if (ctrl.isImage) {
                    var ext = ctrl.srcUrl.substring(ctrl.srcUrl.lastIndexOf('.') + 1);
                    $http({
                        method: 'GET',
                        url: ctrl.srcUrl,
                        responseType: 'arraybuffer'
                    }).then(function (resp) {
                        ctrl.cropped.source = "data:image/" + ext + ";base64," + ctrl._arrayBufferToBase64(resp.data);
                        ctrl.combineImage();
                    });
                }

            }
        }.bind(ctrl);
        ctrl.combineImage = function () {
            setTimeout(() => {
                var img = document.getElementById('croppie-src');
                var myCanvas = document.getElementById("canvas");
                var w = 1329;
                var h = 2000;
                var rto = w / h;
                var newW = img.width * 5.316;
                var newH = newW / rto;
                var ctx = myCanvas.getContext("2d");
                ctx.imageSmoothingEnabled = true;
                ctx.drawImage(img, 0, 0, newW, newH);
                ctx.drawImage(ctrl.frame, 0, 0, w, h);
                $scope.$apply(function () {
                    ctrl.postedFile.fileStream = myCanvas.toDataURL();//ctx.getImageData(0, 0, 300, 350);
                    ctrl.imgUrl = ctrl.postedFile.fileStream.replace("image/png", "image/octet-stream")
                });
            }, 200);
        };
        ctrl.saveCanvas = function () {
            var canvas = document.getElementById("canvas");
            var link = document.createElement("a");
            link.download = ctrl.postedFile.fileName + ctrl.postedFile.extension;
            $rootScope.isBusy = true;
            canvas.toBlob(function (blob) {                
                link.href = URL.createObjectURL(blob);
                link.click();
                $rootScope.isBusy = false;
                $scope.$apply();
            }, 'image/png');
            

        };
        ctrl.loadBase64 = function (url) {
            var ext = url.substring(url.lastIndexOf('.') + 1);
            $http({
                method: 'GET',
                url: url,
                responseType: 'arraybuffer'
            }).then(function (resp) {
                return "data:image/" + ext + ";base64," + ctrl._arrayBufferToBase64(resp.data);

            });
        };
        ctrl.loadImage = function (src) {
            // http://www.thefutureoftheweb.com/blog/image-onload-isnt-being-called
            var img = new Image();
            // img.onload = onload;
            img.src = src;
            return img;
        }
        ctrl._arrayBufferToBase64 = function (buffer) {
            var binary = '';
            var bytes = new Uint8Array(buffer);
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            return window.btoa(binary);
        };
        ctrl.mediaFile = {
            file: null,
            fullPath: '',
            folder: ctrl.folder,
            title: ctrl.title,
            description: ctrl.description
        };
        ctrl.media = null;

        ctrl.selectFile = function (file, errFiles) {
            if (file !== undefined && file !== null) {
                ctrl.mediaFile.folder = ctrl.folder ? ctrl.folder : 'Media';
                ctrl.mediaFile.title = ctrl.title ? ctrl.title : '';
                ctrl.mediaFile.description = ctrl.description ? ctrl.description : '';
                ctrl.mediaFile.file = file;
                ctrl.getBase64(file);
                if (file.size < 100000) {                
                    var msg = 'Please choose a better photo (larger than 100kb)!';
                    $rootScope.showConfirm(ctrl, null, [], null, null, msg);
                } else {
                    
                }
            }
        };


        ctrl.getBase64 = function (file) {
            if (file !== null) {
                $rootScope.isBusy = true;
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = function () {
                    var index = reader.result.indexOf(',') + 1;
                    var base64 = reader.result.substring(index);
                    
                    if (ctrl.postedFile) {
                        ctrl.postedFile.fileName = file.name.substring(0, file.name.lastIndexOf('.'));
                        ctrl.postedFile.extension = file.name.substring(file.name.lastIndexOf('.'));
                        // ctrl.postedFile.fileStream = reader.result;
                    }
                    ctrl.cropped.source = reader.result;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                };
                reader.onerror = function (error) {
                    $rootScope.isBusy = false;
                    $rootScope.showErrors([error]);
                };
            }
            else {
                return null;
            }
        };
    }]
});