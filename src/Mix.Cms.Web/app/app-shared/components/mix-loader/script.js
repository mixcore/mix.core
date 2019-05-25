
modules.component('mixLoader', {
    templateUrl: '/app/app-shared/components/mix-loader/view.html',
    controller: ['$rootScope', '$location', function($rootScope, $location) {
        var ctrl = this;
        ctrl.imageDataArray = [];
        ctrl.canvasCount = 10;
        ctrl.duration = 500;
        ctrl.bgDuration = 2500;
        ctrl.canvas = null;
        ctrl.init = function(){
            html2canvas($(".mix-loader-container .content")[0]).then(canvas=>{
                ctrl.canvas = canvas;
            });
        };
        ctrl.swap = function() {
                var temp = document.createElement('canvas');
                var buffer = temp.getContext('2d');
                var scale = 0.5;
                //capture all div data as image
                // ctx = ctrl.canvas.getContext("2d");
                temp.width = ctrl.canvas.width * scale;
                temp.height = ctrl.canvas.height * scale;
                buffer.drawImage(ctrl.canvas, 0, 0, ctrl.canvas.width * scale, ctrl.canvas.height * scale);
                var imageData = buffer.getImageData(0, 0, temp.width, temp.height);
                var pixelArr = imageData.data;
                ctrl.createBlankImageData(imageData);
                //put pixel info to ctrl.imageDataArray (Weighted Distributed)
                for (let i = 0; i < pixelArr.length; i += 4) {
                    //find the highest probability canvas the pixel should be in
                    let p = Math.floor((i / pixelArr.length) * ctrl.canvasCount);
                    let a = ctrl.imageDataArray[ctrl.weightedRandomDistrib(p)];
                    a[i] = pixelArr[i];
                    a[i + 1] = pixelArr[i + 1];
                    a[i + 2] = pixelArr[i + 2];
                    a[i + 3] = pixelArr[i + 3];
                }
                //create canvas for each imageData and append to target element
                for (let i = 0; i < ctrl.canvasCount; i++) {
                    let c = ctrl.newCanvasFromImageData(ctrl.imageDataArray[i], temp.width, temp.height);
                    c.classList.add("dust");
                    $(".mix-loader-container").append(c);
                }
                //clear all children except the canvas
                $(".mix-loader-container .content").children().not(".dust").fadeOut(ctrl.bgDuration);
                //apply animation
                $(".dust").each(function(index) {
                    ctrl.animateBlur($(this), 0.8, ctrl.duration);
                    setTimeout(() => {
                        ctrl.animateTransform($(this), 100, -100, chance.integer({ min: -15, max: 15 }), ctrl.duration + (110 * index));
                    }, 70 * index);
                    //remove the canvas from DOM tree when faded
                    $(this).delay(70 * index).fadeOut((110 * index) + ctrl.duration, "swing", () => { $(this).remove(); });
                });
            
        };
        ctrl.weightedRandomDistrib = function(peak) {
            var prob = [], seq = [];
            for (let i = 0; i < ctrl.canvasCount; i++) {
                prob.push(Math.pow(ctrl.canvasCount - Math.abs(peak - i), 3));
                seq.push(i);
            }
            return chance.weighted(seq, prob);
        };
        ctrl.animateBlur = function(elem, radius, duration) {
            var r = 0;
            $({ rad: 0 }).animate({ rad: radius }, {
                duration: duration,
                easing: "swing",
                step: function(now) {
                    elem.css({
                        filter: 'blur(' + now + 'px)'
                    });
                }
            });
        }
        ctrl.animateTransform = function(elem, sx, sy, angle, duration) {
            var td = tx = ty = 0;
            $({ x: 0, y: 0, deg: 0 }).animate({ x: sx, y: sy, deg: angle }, {
                duration: duration,
                easing: "swing",
                step: function(now, fx) {
                    if (fx.prop == "x")
                        tx = now;
                    else if (fx.prop == "y")
                        ty = now;
                    else if (fx.prop == "deg")
                        td = now;
                    elem.css({
                        transform: 'rotate(' + td + 'deg)' + 'translate(' + tx + 'px,' + ty + 'px)'
                    });
                }
            });
        }
        ctrl.createBlankImageData = function(imageData) {
            for (let i = 0; i < ctrl.canvasCount; i++) {
                let arr = new Uint8ClampedArray(imageData.data);
                for (let j = 0; j < arr.length; j++) {
                    arr[j] = 0;
                }
                ctrl.imageDataArray.push(arr);
            }
        }
        ctrl.newCanvasFromImageData = function(imageDataArray, w, h) {
            var canvas = document.createElement('canvas');
            canvas.width = w;
            canvas.height = h;
            tempCtx = canvas.getContext("2d");
            tempCtx.putImageData(new ImageData(imageDataArray, w, h), 0, 0);

            return canvas;
        }
    }],

    bindings: {
        isShow: '=',
    }
});