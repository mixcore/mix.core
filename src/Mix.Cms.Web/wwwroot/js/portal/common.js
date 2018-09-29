var SW = window.SW || {};

(function ($) {
    SW.Common = {
        currentLanguage: $('#curentLanguage').val(),
        settings: {
            "async": true,
            "crossDomain": true,
            "url": "",
            "method": "POST",
            "headers": {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            "data": ""
        },
        templateEditor: null,
        edtModule: '',
        loadFiles: async function (container) {
            $.ajax({
                url: '/api/files',
                type: "GET",
                success: function (result) {
                    //container.dataTable().fnDestroy();
                    container.find('tbody').empty();
                    $.each(result, function (index, val) {
                        var html = '<tr>';
                        html += '<td>' + val.fileFolder + '</td>';
                        html += '<td>' + val.filename + '</td>';
                        if (val.fileFolder === 'Images') {
                            html += '<td><img width="100px" src="' + val.fullPath + '"/></td>';
                        }
                        else {
                            html += '<td>' + val.filename + '</td>';
                        }
                        html += '<td><input onClick="this.select();" class="form-control" value="' + location.origin + val.fullPath + '"/></td>';
                        html += '</tr>';
                        container.find('tbody').append(html);
                    });

                    //container.DataTable({
                    //    "paging": true,
                    //    "pageLength": 5,
                    //    "lengthChange": false,
                    //    "select": true,
                    //    "searching": false,
                    //    "ordering": false,
                    //    "info": false,
                    //    "autoWidth": true//,
                    //    //"rowReorder": true
                    //});
                },
                error: function (err) {
                    return null;
                }
            });
        },
        loadFileStream: async function (folder) {
            var img = document.querySelector('#file').files[0];

            if (img !== null) {
                var name = img.name.split('.')[0];
                var ext = img.name.split('.')[1];

                var reader = new FileReader();
                reader.readAsDataURL(img);
                reader.onload = function () {
                    //var index = reader.result.indexOf(',') + 1;
                    var base64 = reader.result; //.substring(index);
                    var obj = {
                        fileFolder: folder,
                        filename: name,
                        extension: ext,
                        fileStream: base64
                    };
                    $.ajax({
                        url: '/api/file/uploadFile',
                        type: "POST",
                        data: obj,
                        success: function (result) {
                            var container = $("#modal-files").find('table');
                            SW.Common.loadFiles(container);
                        },
                        error: function (err) {
                            return '';
                        }
                    });
                    console.log(obj);
                };
                reader.onerror = function (error) {
                };
            }
        },
        init: async function () {
            var clipboard = new ClipboardJS('.btn-clipboard');

            //$("#modal-files").on('show.bs.modal', function () {
            //    var container = $("#modal-files").find('table');
            //    SW.Common.loadFiles(container);
            //});

            $('[data-toggle="popover"]').popover({
                html: true,
                content: function () {
                    var content = $(this).next('.popover-body');
                    return $(content).html();
                },
                title: function () {
                    var title = $(this).attr("data-popover-content");
                    return $(title).children(".popover-heading").html();
                }
            });

            $(".sortable").sortable({
                revert: true,
                handle: ".drag-header",
                update: function (event, ui) {
                    //create the array that hold the positions...
                    var order = [];
                    //loop trought each li...
                    $('.sortable .sortable-item').each(function (i, e) {
                        //add each li position to the array...
                        // the +1 is for make it start from 1 instead of 0
                        //order.push($(this).attr('id') + '=' + ($(this).index() + 1));
                        $(e).find('.item-priority').val($(e).index() + 1);
                        //alert($(this).attr('module-priority'));

                        var model = $(e).attr('sort-model');
                        var modelId = $(e).attr('sort-model-id');
                        if (model !== undefined && modelId !== undefined) {
                            var data = [{
                                "propertyName": "Priority",
                                "propertyValue": $(e).index() + 1
                            }];
                            var settings = {
                                "async": true,
                                "crossDomain": true,
                                "url": '/api/' + SW.Common.currentLanguage + '/' + model + '/save/' + modelId,
                                "method": "POST",
                                "headers": {
                                    "Content-Type": "application/json"
                                },
                                "processData": false,
                                "data": JSON.stringify(data)
                            }

                            $.ajax(settings).done(function (response) {
                                console.log(response);
                            });
                        }
                    });

                    // join the array as single variable...
                    var positions = order.join(';')
                    //use the variable as you need!
                    //alert(positions);
                    // $.cookie( LI_POSITION , positions , { expires: 10 });
                }
            });

            $(".sortable").disableSelection();

            $(document).on('change', '.custom-file .custom-file-input', function () {
                var file = this.files[0];
                if (file !== undefined && file !== null) {
                    var container = $(this).parent('.custom-file');
                    //SW.Common.getBase64(file, $('.custom-file')).then(result => console.log(result));
                    //await SW.Common.getBase64(file).then(result => console.log(result));
                    if ($(this).hasClass('auto-upload')) {
                        var fileName = SW.Common.uploadImage(file, container);
                    }
                    else {
                        SW.Common.getBase64(file, $(container)).then(result => {
                            container.find('.custom-file-val').val(result);
                            container.find('.custom-file-img').attr('src', result);
                            container.find('.custom-file-cropper').show().attr('data-imgsrc', result);
                        });
                    }
                }
            });

            window.onload = function () {
                'use strict';

                $('.btn-group .btn input[checked="checked"]').parent().addClass('active');

                var container = document.querySelector('.image-crop-modal-lg .img-container');
                if (container) {
                    var Cropper = window.Cropper;
                    var URL = window.URL || window.webkitURL;
                    var image = container.getElementsByTagName('img').item(0);
                    var download = document.getElementById('download');
                    var actionsLeftSidebar = document.getElementById('actions-left-sidebar');
                    var actionsRightSidebar = document.getElementById('actions-right-sidebar');
                    var dataX = document.getElementById('dataX');
                    var dataY = document.getElementById('dataY');
                    var dataHeight = document.getElementById('dataHeight');
                    var dataWidth = document.getElementById('dataWidth');
                    var dataRotate = document.getElementById('dataRotate');
                    var dataScaleX = document.getElementById('dataScaleX');
                    var dataScaleY = document.getElementById('dataScaleY');
                    var options = {};

                    var cropper = new Cropper(image, options);
                    var originalImageURL = image.src;
                    var uploadedImageType = 'image/jpeg';
                    var uploadedImageURL;
                    var fileProps = "";

                    $(".image-crop-modal-lg").on('shown.bs.modal', function (event) {
                        var button = $(event.relatedTarget) // Button that triggered the modal
                        var recipient = button.data('imgsrc') // Extract info from data-* attributes
                        // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
                        // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
                        //var modal = $(this);
                        //modal.find('.modal-body #image-crop-placeholder').attr('src', recipient);
                        $('.img-container img').attr('src', recipient);

                        options = {
                            aspectRatio: 16 / 9,
                            preview: '.img-preview',
                            ready: function (e) {
                                console.log(e.type);
                            },
                            cropstart: function (e) {
                                console.log(e.type, e.detail.action);
                            },
                            cropmove: function (e) {
                                console.log(e.type, e.detail.action);
                            },
                            cropend: function (e) {
                                console.log(e.type, e.detail.action);
                                fileProps = {
                                    "mediaProperties": {
                                        "imageData": this.cropper.getImageData(),
                                        "canvasData": this.cropper.getCanvasData(),
                                        "cropBoxData": this.cropper.getCropBoxData(),
                                        "croppedCanvas": this.cropper.getCroppedCanvas({
                                            maxWidth: 1920,
                                            maxHeight: 1920
                                        }).toDataURL('image/jpeg')
                                    }
                                }
                            },
                            crop: function (e) {
                                var data = e.detail;

                                console.log(e.type);
                                dataX.innerHTML = Math.round(data.x);
                                dataY.innerHTML = Math.round(data.y);
                                dataHeight.innerHTML = Math.round(data.height);
                                dataWidth.innerHTML = Math.round(data.width);
                                dataRotate.innerHTML = typeof data.rotate !== 'undefined' ? data.rotate : '';
                                dataScaleX.innerHTML = typeof data.scaleX !== 'undefined' ? data.scaleX : '';
                                dataScaleY.innerHTML = typeof data.scaleY !== 'undefined' ? data.scaleY : '';
                            },
                            zoom: function (e) {
                                console.log(e.type, e.detail.ratio);
                            }
                        };

                        // Restart
                        cropper.destroy();
                        cropper = new Cropper(image, options);
                    });

                    $('.image-crop-modal-lg #btn-submit-crop-info').on('click', function (event) {
                        // TODO: Submit Ajax
                        alert('TODO: Submit ajax');
                        SW.Common.settings.url = '';
                        SW.Common.settings.data = JSON.stringify(fileProps);

                        $.ajax(SW.Common.settings).done(function (response) {
                        });
                    });

                    // Tooltip
                    $('[data-toggle="tooltip"]').tooltip();

                    // Buttons
                    if (!document.createElement('canvas').getContext) {
                        $('button[data-method="getCroppedCanvas"]').prop('disabled', true);
                    }

                    if (typeof document.createElement('cropper').style.transition === 'undefined') {
                        $('button[data-method="rotate"]').prop('disabled', true);
                        $('button[data-method="scale"]').prop('disabled', true);
                    }

                    //// Download
                    //if (typeof download.download === 'undefined') {
                    //    download.className += ' disabled';
                    //}

                    // Options
                    actionsRightSidebar.querySelector('.docs-toggles').onchange = function (event) {
                        var e = event || window.event;
                        var target = e.target || e.srcElement;
                        var cropBoxData;
                        var canvasData;
                        var isCheckbox;
                        var isRadio;

                        if (!cropper) {
                            return;
                        }

                        if (target.tagName.toLowerCase() === 'label') {
                            target = target.querySelector('input');
                        }

                        isCheckbox = target.type === 'checkbox';
                        isRadio = target.type === 'radio';

                        if (isCheckbox || isRadio) {
                            if (isCheckbox) {
                                options[target.name] = target.checked;
                                cropBoxData = cropper.getCropBoxData();
                                canvasData = cropper.getCanvasData();

                                options.ready = function () {
                                    console.log('ready');
                                    cropper.setCropBoxData(cropBoxData).setCanvasData(canvasData);
                                };
                            } else {
                                options[target.name] = target.value;
                                options.ready = function () {
                                    console.log('ready');
                                };
                            }

                            // Restart
                            cropper.destroy();
                            cropper = new Cropper(image, options);
                        }
                    };

                    // Methods
                    actionsLeftSidebar.querySelector('.docs-buttons').onclick = function (event) {
                        var e = event || window.event;
                        var target = e.target || e.srcElement;
                        var cropped;
                        var result;
                        var input;
                        var data;

                        if (!cropper) {
                            return;
                        }

                        while (target !== this) {
                            if (target.getAttribute('data-method')) {
                                break;
                            }

                            target = target.parentNode;
                        }

                        if (target === this || target.disabled || target.className.indexOf('disabled') > -1) {
                            return;
                        }

                        data = {
                            method: target.getAttribute('data-method'),
                            target: target.getAttribute('data-target'),
                            option: target.getAttribute('data-option') || undefined,
                            secondOption: target.getAttribute('data-second-option') || undefined
                        };

                        cropped = cropper.cropped;

                        if (data.method) {
                            if (typeof data.target !== 'undefined') {
                                input = document.querySelector(data.target);

                                if (!target.hasAttribute('data-option') && data.target && input) {
                                    try {
                                        data.option = JSON.parse(input.value);
                                    } catch (e) {
                                        console.log(e.message);
                                    }
                                }
                            }

                            switch (data.method) {
                                case 'rotate':
                                    if (cropped && options.viewMode > 0) {
                                        cropper.clear();
                                    }

                                    break;

                                case 'getCroppedCanvas':
                                    try {
                                        data.option = JSON.parse(data.option);
                                    } catch (e) {
                                        console.log(e.message);
                                    }

                                    if (uploadedImageType === 'image/jpeg') {
                                        if (!data.option) {
                                            data.option = {};
                                        }

                                        data.option.fillColor = '#fff';
                                    }

                                    break;
                            }

                            result = cropper[data.method](data.option, data.secondOption);

                            switch (data.method) {
                                case 'rotate':
                                    if (cropped && options.viewMode > 0) {
                                        cropper.crop();
                                    }

                                    break;

                                case 'scaleX':
                                case 'scaleY':
                                    target.setAttribute('data-option', -data.option);
                                    break;

                                case 'getCroppedCanvas':
                                    if (result) {
                                        // Bootstrap's Modal
                                        $('#getCroppedCanvasModal').modal().find('.modal-body').html(result);

                                        if (!download.disabled) {
                                            download.href = result.toDataURL(uploadedImageType);
                                        }
                                    }

                                    break;

                                case 'destroy':
                                    cropper = null;

                                    if (uploadedImageURL) {
                                        URL.revokeObjectURL(uploadedImageURL);
                                        uploadedImageURL = '';
                                        image.src = originalImageURL;
                                    }

                                    break;
                            }

                            if (typeof result === 'object' && result !== cropper && input) {
                                try {
                                    input.value = JSON.stringify(result);
                                } catch (e) {
                                    console.log(e.message);
                                }
                            }
                        }
                    };

                    document.body.onkeydown = function (event) {
                        var e = event || window.event;

                        if (!cropper || this.scrollTop > 300) {
                            return;
                        }

                        switch (e.keyCode) {
                            case 37:
                                e.preventDefault();
                                cropper.move(-1, 0);
                                break;

                            case 38:
                                e.preventDefault();
                                cropper.move(0, -1);
                                break;

                            case 39:
                                e.preventDefault();
                                cropper.move(1, 0);
                                break;

                            case 40:
                                e.preventDefault();
                                cropper.move(0, 1);
                                break;
                        }
                    };

                    //// Import image
                    //var inputImage = document.getElementById('inputImage');

                    //if (URL) {
                    //    inputImage.onchange = function () {
                    //        var files = this.files;
                    //        var file;

                    //        if (cropper && files && files.length) {
                    //            file = files[0];

                    //            if (/^image\/\w+/.test(file.type)) {
                    //                uploadedImageType = file.type;

                    //                if (uploadedImageURL) {
                    //                    URL.revokeObjectURL(uploadedImageURL);
                    //                }

                    //                image.src = uploadedImageURL = URL.createObjectURL(file);
                    //                cropper.destroy();
                    //                cropper = new Cropper(image, options);
                    //                inputImage.value = null;
                    //            } else {
                    //                window.alert('Please choose an image file.');
                    //            }
                    //        }
                    //    };
                    //} else {
                    //    inputImage.disabled = true;
                    //    inputImage.parentNode.className += ' disabled';
                    //}
                };
            }

            //var cropBoxData;
            //var canvasData;
            //var cropper;

            //function each(arr, callback) {
            //    var length = arr.length;
            //    var i;

            //    for (i = 0; i < length; i++) {
            //        callback.call(arr, arr[i], i, arr);
            //    }

            //    return arr;
            //}

            //$(".image-crop-modal-lg").on('shown.bs.modal', function (event) {
            //    var button = $(event.relatedTarget) // Button that triggered the modal
            //    var recipient = button.data('imgsrc') // Extract info from data-* attributes
            //    // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
            //    // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
            //    var modal = $(this);
            //    modal.find('.modal-body #image-crop-placeholder').attr('src', recipient);

            //    var image = $(".image-crop-modal-lg #image-crop-placeholder").get(0);
            //    var previews = document.querySelectorAll('.image-crop-modal-lg .img-cropper-preview');

            //    cropper = new Cropper(image, {
            //        ready: function () {
            //            var clone = this.cloneNode();

            //            clone.className = ''
            //            clone.style.cssText = (
            //                'display: block;' +
            //                'width: 100%;' +
            //                'min-width: 0;' +
            //                'min-height: 0;' +
            //                'max-width: none;' +
            //                'max-height: none;'
            //            );

            //            each(previews, function (elem) {
            //                elem.appendChild(clone.cloneNode());
            //            });
            //        },

            //        crop: function (e) {
            //            var data = e.detail;
            //            var cropper = this.cropper;
            //            var imageData = cropper.getImageData();
            //            var previewAspectRatio = data.width / data.height;

            //            each(previews, function (elem) {
            //                var previewImage = elem.getElementsByTagName('img').item(0);
            //                var previewWidth = elem.offsetWidth;
            //                var previewHeight = previewWidth / previewAspectRatio;
            //                var imageScaledRatio = data.width / previewWidth;

            //                elem.style.height = previewHeight + 'px';
            //                previewImage.style.width = imageData.naturalWidth / imageScaledRatio + 'px';
            //                previewImage.style.height = imageData.naturalHeight / imageScaledRatio + 'px';
            //                previewImage.style.marginLeft = -data.x / imageScaledRatio + 'px';
            //                previewImage.style.marginTop = -data.y / imageScaledRatio + 'px';
            //            });
            //            var fileProps = {
            //                "mediaProperties": {
            //                    "imageData": cropper.getImageData(),
            //                    "canvasData": cropper.getCanvasData(),
            //                    "cropBoxData": cropper.getCropBoxData()
            //                }
            //            }

            //            $('#image-crop-info').val(JSON.stringify(fileProps));
            //        }
            //    });
            //}).on('hidden.bs.modal', function () {
            //    cropBoxData = cropper.getCropBoxData();
            //    canvasData = cropper.getCanvasData();
            //    cropper.destroy();
            //});

            $(".image-preview-modal-lg").on('show.bs.modal', function (event) {
                var button = $(event.relatedTarget) // Button that triggered the modal
                var recipient = button.data('imgsrc') // Extract info from data-* attributes
                // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
                // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
                var modal = $(this)
                //modal.find('.modal-title').text('New message to ' + recipient)
                //modal.find('.modal-body input').val(recipient)
                modal.find('.modal-body .img-fluid').attr('src', recipient);
            });

            // not work with BS 4 (using now http://bootstrap-tagsinput.github.io/bootstrap-tagsinput/examples/)
            //$(".tags")
            //    .on('tokenfield:createdtoken', function (e) {
            //        //$('.tags').val($('.tags').tokenfield('getTokensList'));
            //    })

            //    .on('tokenfield:edittoken', function (e) {
            //    })

            //    .on('tokenfield:removedtoken', function (e) {
            //        //$('.tags').val($('.tags').tokenfield('getTokensList'));
            //    }).tokenfield();

            //Enable iCheck plugin for checkboxes
            //iCheck for checkbox and radio inputs
            //$('input[type="checkbox"]').iCheck({
            //    checkboxClass: 'icheckbox_square-blue',
            //    radioClass: 'iradio_square-blue'
            //});
            $("input[type='checkbox']").on('ifChanged', function (e) {
                $(this).val(e.target.checked === true);
            });

            //$(".select2").select2();

            //$('.dataTable').DataTable({
            //    "paging": false,
            //    "lengthChange": false,
            //    //"select": true,
            //    "searching": false,
            //    "ordering": false,
            //    "info": false,
            //    "autoWidth": true//,
            //    //"rowReorder": true
            //});
            //$('.dataTable tr').on('click', function () {
            //    $(this).toggleClass('selected');
            //})

            $('.custom-file .custom-file-val').on('change', function () {
                $(this).parent('.custom-file').find('img').attr('src', $(this).val());
                $(this).parent('.custom-file').find('.custom-file-cropper').show().attr('data-imgsrc', $(this).val());
            });
            //$('.editor-content').trumbowyg({
            //    btns: [
            //        ['base64', 'foreColor', 'backColor']
            //    ]
            //});

            if ($.trumbowyg) {
                var configurations = {
                    core: {},
                    plugins: {
                        btnsDef: {
                            // Customizables dropdowns
                            image: {
                                dropdown: ['insertImage', 'upload', 'base64', 'noembed'],
                                ico: 'insertImage'
                            }
                        },
                        btns: [
                            ['viewHTML'],
                            ['undo', 'redo'],
                            ['formatting'],
                            ['strong', 'em', 'del', 'underline'],
                            ['link'],
                            ['image'],
                            ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
                            ['unorderedList', 'orderedList'],
                            ['foreColor', 'backColor'],
                            ['preformatted'],
                            ['horizontalRule'],
                            ['fullscreen']
                        ],
                        plugins: {
                            // Add imagur parameters to upload plugin
                            upload: {
                                serverPath: 'https://api.imgur.com/3/image',
                                fileFieldName: 'image',
                                headers: {
                                    'Authorization': 'Client-ID 9e57cb1c4791cea'
                                },
                                urlPropertyName: 'data.link'
                            }
                        }
                    }
                };

                // Demo switch
                var $demoTextarea = $('.editor-content');
                $demoTextarea.trumbowyg(configurations.plugins);
                //$('.demo-switcher .button').on('click', function () {
                //    var $current = $('.demo-switcher .current');
                //    $(this).parent().removeClass('current-' + $current.data('config'));
                //    $current.removeClass('current');
                //    $(this).addClass('current');
                //    $(this).parent().addClass('current-' + $(this).data('config'));
                //    $demoTextarea.trumbowyg('destroy');
                //    $demoTextarea.trumbowyg(configurations[$(this).data('config')]);
                //});

                // Lang accordion
                $('#lang-list-view-full').on('click', function () {
                    $('#lang-list-light').slideUp(100);
                    $('#lang-list-full').slideDown(350);
                });
            }

            // Init Code editor
            //$.each($('.code-editor'), function (i, e) {
            //    var container = $(this);
            //    var editor = ace.edit(e);
            //    if (container.hasClass('json')) {
            //        editor.session.setMode("ace/mode/json");
            //    }
            //    else {
            //        editor.session.setMode("ace/mode/razor");
            //    }
            //    editor.setTheme("ace/theme/chrome");
            //    //editor.setReadOnly(true);

            //    editor.session.setUseWrapMode(true);
            //    editor.setOptions({
            //        maxLines: Infinity
            //    });
            //    editor.getSession().on('change', function (e) {
            //        // e.type, etc
            //        $(container).parent().find('.code-content').val(editor.getValue());
            //    });
            //})
            if ($('#code-editor').length > 0) {
                SW.Common.templateEditor = ace.edit("code-editor");
                SW.Common.templateEditor.setTheme("ace/theme/chrome");
                SW.Common.templateEditor.session.setMode("ace/mode/razor");
                SW.Common.templateEditor.session.setUseWrapMode(true);
                SW.Common.templateEditor.setOptions({
                    maxLines: Infinity
                });
                SW.Common.templateEditor.getSession().on('change', function (e) {
                    // e.type, etc
                    $('#code-editor').parent().find('.code-content').val(SW.Common.templateEditor.getValue());
                });
            }
            $('#sel-template').on('change', function () {
                SW.Common.templateEditor.setValue($(this).val());
                var templateName = $(this).find('option:selected').text();
                if (templateName === "[ NEW TEMPLATE ]") {
                    $('.sel-filename').attr('value', ''); // use attr instead of val to fix bug value not change https://stackoverflow.com/questions/11873721/jquery-val-change-doesnt-change-input-value
                } else {
                    $('.sel-filename').attr('value', templateName);
                }
            });
            $('.sel-filename').on('change', function () {
                //$('.sel-filename').attr('value', $(this).val());
                $(this).next('.template-id').val(0);
            });

            var selVal = $('.selectpicker').data('val');
            // TODO: ERROR with bootstrap 4
            //$('.selectpicker').selectpicker('val', selVal);

            // TODO: ERROR with bootstrap 4
            //$('[data-toggle=confirmation]').confirmation({
            //    rootSelector: '[data-toggle=confirmation]',
            //    container: 'body'
            //});
        },
        //folder : 'Modules/Banners'
        uploadImage: function (file, container) {
            // Create FormData object
            var files = new FormData();
            var folder = container.find('.folder-val').val();
            var title = container.find('.title').val();
            var description = container.find('.description').val();
            // Looping over all files and add it to FormData object
            files.append(file.name, file);

            // Adding one more key to FormData object
            files.append('fileFolder', folder);
            files.append('title', title);
            files.append('description', description);
            $.ajax({
                url: '/api/' + SW.Common.currentLanguage + '/media/upload', //'/api/tts/UploadImage',
                type: "POST",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: files,
                success: function (result) {
                    container.find('.custom-file-val').val(result.data.fullPath);
                    container.find('.custom-file-img').attr('src', result.data.fullPath);
                    container.find('.custom-file-cropper').show().attr('data-imgsrc', result.data.fullPath).attr('data-imgid', result.data.id);
                    return result;
                },
                error: function (err) {
                    return '';
                }
            });
        },
        showError: function (error) {
            console.log(error);
            $('#modal-error .modal-body').html('Error messages');
            $('#modal-error').modal('show');
        },
        ajaxSubmitForm: function (form, url) {
            var frm = new FormData();
            frm.append('ModuleData', form.serialize());
            $.ajax({
                url: url,
                type: 'POST',
                processData: true, // Not to process data
                data: form.serialize(),
                success: function (data) {
                    console.log('Submission was successful.');
                    console.log(data);
                },
                error: function (data) {
                    console.log('An error occurred.');
                    console.log(data);
                },
            });
        },
        getApiResult: async function (req) {
            //req.Authorization = authService.authentication.token;
            var headers = {
                'Content-Type': 'application/json'
                //'RefreshToken': authService.authentication.refresh_token
            };
            req.headers = headers;
            return $.ajax(req).done(function (results) {
                if (results.data.responseKey === 'NotAuthorized') {
                    //Try again with new token from previous Request (optional)
                    setTimeout(function () {
                        headers = {
                            'Content-Type': 'application/json',
                            'RefreshToken': authService.authentication.refresh_token
                        };
                        req.headers = headers;
                        return $.ajax(req).done(function (results) {
                            //if (results.data.responseKey === 'NotAuthorized') {
                            //    authService.logOut();
                            //    $location.path('/admin/login');
                            //}
                            //else {
                            //    return results;
                            //}
                        });
                    }, 2000);
                }
                //else if (results.data.authData !== null && results.data.authData !== undefined) {
                //    var authData = results.data.authData;
                //    //localStorageService.set('authorizationData', { token: authData.access_token, userName: authData.userData.NickName, roleNames: authData.userData.RoleNames, avatar: authData.userData.Avatar, refresh_token: authData.refresh_token, userId: authData.userData.Id });
                //    //authService.authentication.isAuth = true;
                //    //authService.authentication.isAdmin = $.inArray("Admin", authData.userData.RoleNames) >= 0;
                //    //authService.authentication.userName = authData.userData.NickName;
                //    //authService.authentication.roleNames = authData.userData.RoleNames;
                //    //authService.authentication.userId = authData.userData.Id;
                //    //authService.authentication.avatar = authData.userData.Avatar;
                //    //authService.authentication.token = authData.access_token;
                //    //authService.authentication.refresh_token = authData.refresh_token;
                //}
                return results;
            },
                function () {
                });
        },
        getPagingTable: function (obj, title, headers) {
            /// Example: var table = SW.Common.getPagingTable(obj, 'Header Title', ['Col 1', null, 'Col 2', 'Col 3', null, 'Col 4']);

            var items = obj['items'];
            title = title !== undefined ? title : '';

            var card = this.createElement('div', 'card');
            var icon = this.createElement('i', 'fab fa-align-justify');
            var cardHeader = this.createElement('div', 'card-header');
            cardHeader.innerHTML = title;
            $(cardHeader).prepend(icon);
            card.appendChild(cardHeader);

            var cardBlock = this.createElement('div', 'card-block');

            var table = document.createElement("Table");
            table.className = 'table table-bordered table-striped';

            // Create table headers
            var thead = document.createElement('thead');
            if (headers === undefined) {
                headers = [];
                var fstObj = items[0];
                for (var name in fstObj) {
                    headers.push(name.toString());
                    console.log(name);
                }
            }
            headers.forEach(function (header) {
                if (header !== null) {
                    var th = document.createElement('th');
                    th.innerHTML = header.display;
                    thead.appendChild(th);
                }
            });
            table.appendChild(thead);

            if (items.length > 0) {
                var paging = document.createElement("paging");
                paging.className = 'pagination';

                items.forEach(function (item) {
                    var trContent = document.createElement('tr');
                    var i = 0;
                    headers.forEach(function (header) {
                        //if (trContent.childNodes.length < headers.length && headers[i] != null) {
                        var tdContent = document.createElement('td');
                        tdContent.innerHTML = '<span>' + item[header.key] + '</span>';
                        trContent.appendChild(tdContent);
                        //}
                        i++;
                    });
                    table.appendChild(trContent);
                });

                //$(paging).pagination({
                //    items: obj.totalItems,
                //    itemsOnPage: obj.pageSize,
                //    currentPage: obj.pageIndex + 1
                //});
                //$(paging).find('li').addClass('page-item')
                cardBlock.appendChild(table);
                cardBlock.appendChild(paging);
                card.appendChild(cardBlock);
            }
            return card;
        },
        createElement: function (eName, eClass) {
            var el = document.createElement(eName);
            el.className = eClass;
            return el;
        },
        getBase64: async function (file, container) {
            if (file !== null) {
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = function () {
                    var index = reader.result.indexOf(',') + 1;
                    var base64 = reader.result.substring(index);
                    if (container) {
                        container.find('.custom-file-val').val(reader.result);
                        container.find('.custom-file-img').attr('src', reader.result);
                    }
                    return base64;
                };
                reader.onerror = function (error) {
                    console.log(error);
                };
            }
            else {
                return null;
            }
        },
        loadMedia: function (callBack, keyword = '', pageSize = 12, pageIndex = 0, orderBy = 'fileName', direction = 0) {
            var settings = {
                "async": true,
                "crossDomain": true,
                "url": '/api/' + SW.Common.currentLanguage + '/media/list',
                "method": "POST",
                "headers": {
                    "Content-Type": "application/x-www-form-urlencoded"
                },
                "data": {
                    "pageSize": pageSize,
                    "pageIndex": pageIndex,
                    "orderBy": orderBy,
                    "direction": direction,
                    "keyword": keyword
                }
            }

            $.ajax(settings).done(function (response) {
                callBack(response);
            });
        },
        executeFunctionByName: function (functionName, args, context) {
            if (functionName !== null) {
                var namespaces = functionName.split(".");
                var func = namespaces.pop();
                for (var i = 0; i < namespaces.length; i++) {
                    context = context[namespaces[i]];
                }
                return context[func].apply(this, args);
            }
        },
        delayExecuteFunction: function (time, callbackFunctionName, params) {
            var timer = setInterval(function () {
                CEPT.Global.executeFunctionByName(callbackFunctionName, window, params);
                clearInterval(timer);
            }, time);
        },
        prettyJsonObj: function (obj) {
            return JSON.stringify(obj, null, '\t');
        },
        // Route operations

        writeEvent: function (line) {
            var messages = $("#Messages");
            messages.prepend("<li style='color:blue;'>" + TTX.Common.getTimeString() + ' ' + line + "</li>");
        },

        writeError: function (line) {
            var messages = $("#Messages");
            messages.prepend("<li style='color:red;'>" + TTX.Common.getTimeString() + ' ' + line + "</li>");
        },

        writeLine: function (line) {
            var messages = $("#Messages");
            messages.prepend("<li style='color:black;'>" + TTX.Common.getTimeString() + ' ' + line + "</li>");
        },

        printState: function (state) {
            var messages = $("#Messages");
            return ["connecting", "connected", "reconnecting", state, "disconnected"][state];
        },

        getTimeString: function () {
            var currentTime = new Date();
            return currentTime.toTimeString();
        },

        getQueryVariable: function (variable) {
            var query = window.location.search.substring(1),
                vars = query.split("&"),
                pair;
            for (var i = 0; i < vars.length; i++) {
                pair = vars[i].split("=");
                if (pair[0] === variable) {
                    return unescape(pair[1]);
                }
            }
        },

        getSecurityHeaders: function () {
            var accessToken = sessionStorage["accessToken"] || localStorage["accessToken"];

            if (accessToken) {
                return { "Authorization": "Bearer " + accessToken };
            }

            return {};
        },

        // Operations
        clearAccessToken: function () {
            localStorage.removeItem("accessToken");
            sessionStorage.removeItem("accessToken");
            sessionStorage.removeItem("currentUser");
        },

        setAccessToken: function (accessToken, persistent) {
            if (persistent) {
                localStorage["accessToken"] = accessToken;
            } else {
                sessionStorage["accessToken"] = accessToken;
            }
        },
        setCurrentUser: function (user) {
            sessionStorage["currentUser"] = JSON.stringify(user);
        },
        getCurrentUser: function () {
            var currentUser = sessionStorage["currentUser"];
            if (currentUser) {
                return $.parseJSON(currentUser);
            }
        },

        toErrorsArray: function (data) {
            var errors = new Array(),
                items;

            if (!data || !data.message) {
                return null;
            }

            if (data.modelState) {
                for (var key in data.modelState) {
                    items = data.modelState[key];

                    if (items.length) {
                        for (var i = 0; i < items.length; i++) {
                            errors.push(items[i]);
                        }
                    }
                }
            }

            if (errors.length === 0) {
                errors.push(data.message);
            }

            return errors;
        },

        // Data
        //self.returnUrl = siteUrl;

        htmlEncode: function (value) {
            var encodedValue = $('<div />').text(value).html();
            return encodedValue;
        }
    };
    $(document).ready(function () {
        SW.Common.init();
    })
}(jQuery));