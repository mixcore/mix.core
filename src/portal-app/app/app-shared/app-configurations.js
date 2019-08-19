app.constant('AppSettings', {
    serviceBase: '',
    apiVersion: 'v1',
});
app.constant('ngAppSettings', {
    serviceBase: '',
    clientId: 'ngAuthApp',
    facebookAppId: '464285300363325',
    request: {
        pageSize: '20',
        pageIndex: 0,
        status: '2',
        orderBy: 'CreatedDateTime',
        direction: '1',
        fromDate: null,
        toDate: null,
        keyword: '',
        key: '',
        query: ''
    },
    privacies: [
        'VND',
        'USD'
    ],
    pageSizes: [
        '',
        '5',
        '10',
        '15',
        '20'
    ],
    directions: [
        {
            value: '0',
            title: 'Asc'
        },
        {
            value: '1',
            title: 'Desc'
        }
    ],
    orders: [
        {
            value: 'CreatedDateTime',
            title: 'Created Date'
        }
        ,
        {
            value: 'Priority',
            title: 'Priority'
        },

        {
            value: 'Title',
            title: 'Title'
        }
    ],
    contentStatuses: [
        'Deleted',
        'Preview',
        'Published',
        'Draft',
        'Schedule'
    ],
    dataTypes: [
        { title: 'Custom', value: 0 },
        { title: 'DateTime', value: 1 },
        { title: 'Date', value: 2 },
        { title: 'Time', value: 3 },
        { title: 'Duration', value: 4 },
        { title: 'PhoneNumber', value: 5 },
        { title: 'Currency', value: 6 },
        { title: 'Text', value: 7 },
        { title: 'Html', value: 8 },
        { title: 'MultilineText', value: 9 },
        { title: 'EmailAddress', value: 10 },
        { title: 'Password', value: 11 },
        { title: 'Url', value: 12 },
        { title: 'ImageUrl', value: 13 },
        { title: 'CreditCard', value: 14 },
        { title: 'PostalCode', value: 15 },
        { title: 'Upload', value: 16 },

    ]
    , icons: []
});
app.run(['$http', '$rootScope', 'ngAppSettings', '$location', 'BaseODataService', 'CommonService', 'AuthService', 'TranslatorService',
    function ($http, $rootScope, ngAppSettings, $location, baseODataService, commonService, authService, translatorService,
    ) {
        $rootScope.currentContext = $rootScope;
        $rootScope.isBusy = false;
        $rootScope.translator = translatorService;
        $rootScope.errors = [];
        $rootScope.breadCrumbs = [];
        $rootScope.message = {
            title: '',
            content: '',
            errors: [],
            okFuncName: null,
            okArgs: [],
            cancelFuncName: null,
            cancelArgs: [],
            lblOK: 'OK',
            lblCancel: 'Cancel',
            context: $rootScope
        };

        $rootScope.isBusy = false;
        $rootScope.translator = translatorService;
        $rootScope.message = {
            title: 'test',
            content: '',
            errors: [],
            okFuncName: null,
            okArgs: [],
            cancelFuncName: null,
            cancelArgs: [],
            lblOK: 'OK',
            lblCancel: 'Cancel',
            context: $rootScope
        };
        $rootScope.range = function (max) {
            var input = [];
            for (var i = 1; i <= max; i += 1) input.push(i);
            return input;
        };

        $rootScope.generateKeyword = function (src, character) {
            if (src) {
                src = $rootScope.parseUnsignString(src);
                return src.replace(/[^a-zA-Z0-9]+/g, character)
                    .replace(/([A-Z]+)([A-Z][a-z])/g, '$1-$2')
                    .replace(/([a-z])([A-Z])/g, '$1-$2')
                    .replace(/([0-9])([^0-9])/g, '$1-$2')
                    .replace(/([^0-9])([0-9])/g, '$1-$2')
                    .replace(/-+/g, character)
                    .toLowerCase();
            }
        };

        $rootScope.generatePhone = function (src) {
            return src.replace(/^([0-9]{3})([0-9]{3})([0-9]{4})$/, '($1) $2-$3');
        }
        $rootScope.parseUnsignString = function(str) {
            str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
            str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
            str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
            str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
            str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
            str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
            str = str.replace(/đ/g, "d");
            str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
            str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
            str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
            str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
            str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
            str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
            str = str.replace(/Đ/g, "D");
            return str;
        }
        $rootScope.logOut = function () {
            authService.logOut();            
        };

        $rootScope.updateSettings = function () {
            commonService.removeSettings();
            commonService.fillSettings($rootScope.globalSettings.lang).then(function (response) {
                $rootScope.globalSettings = response;

            });
            $rootScope.isBusy = false;
        };
        $rootScope.executeFunctionByName = async function (functionName, args, context) {
            if (functionName !== null) {
                var namespaces = functionName.split(".");
                var func = namespaces.pop();
                for (var i = 0; i < namespaces.length; i++) {
                    context = context[namespaces[i]];
                }
                functionName = null;
                return context[func].apply(this, args);
            }
        };

        $rootScope.showConfirm = function (context, okFuncName, okArgs, cancelFuncName, title, msg
            , cancelArgs, lblOK, lblCancel) {
            $rootScope.confirmMessage = {
                title: title,
                content: msg,
                context: context,
                okFuncName: okFuncName,
                okArgs: okArgs,
                cancelFuncName: cancelFuncName,
                cancelArgs: cancelArgs,
                lblOK: lblOK ? lblOK : 'OK',
                lblCancel: lblCancel ? lblCancel : 'Cancel'
            };
            $('#dlg-confirm-msg').modal('show');
        };

        $rootScope.preview = function (type, data, title, size, objClass) {
            $rootScope.previewObject = {
                title: title || 'Preview',
                size: size || 'modal-md',
                objClass: objClass,
                type: type,
                data: data
            };
            $('#dlg-preview-popup').modal('show');
        };

        $rootScope.initEditors = function () {
            setTimeout(function () {

                $.each($('.code-editor'), function (i, e) {
                    var container = $(this);
                    var editor = ace.edit(e);
                    if (container.hasClass('json')) {
                        editor.session.setMode("ace/mode/json");
                    }
                    else {
                        editor.session.setMode("ace/mode/razor");
                    }
                    editor.setTheme("ace/theme/chrome");
                    //editor.setReadOnly(true);
                    editor.$blockScrolling = Infinity;
                    editor.session.setUseWrapMode(true);
                    editor.setOptions({
                        maxLines: Infinity
                    });
                    editor.getSession().on('change', function (e) {
                        // e.type, etc                        
                        $(container).parent().find('.code-content').val(editor.getValue());
                    });
                });
            }, 200);
        };

        $rootScope.showErrors = function (errors) {
            if (errors.length) {

                $.each(errors, function (i, e) {
                    $rootScope.showMessage(e, 'danger');
                });
            }
            else {
                $rootScope.showMessage('Server Errors', 'danger');
            }
        };
        $rootScope.shortString = function (msg, max) {
            var data = decodeURIComponent(msg);
            if (data) {
                if (max < data.length) {
                    return data.replace(/[+]/g, ' ').substr(0, max) + ' ...';
                }
                else {
                    return data.replace(/[+]/g, ' ');
                }
            }
        };
        $rootScope.showMessage = function (content, type) {
            var from = 'bottom';
            var align = 'right';
            $.notify({
                icon: "now-ui-icons ui-1_bell-53",
                message: $rootScope.translate(content)

            }, {
                    type: type,
                    timer: 2000,
                    placement: {
                        from: from,
                        align: align
                    }
                });
        };
        $rootScope.encrypt = function (message) {
            var keySize = 256;
            var ivSize = 128;
            var iterations = 100;
            var salt = CryptoJS.lib.WordArray.random(128 / 8);
            var pass = 'secret-key';
            var key = CryptoJS.PBKDF2(pass, salt, {
                keySize: keySize / 32,
                iterations: iterations
            });

            var iv = CryptoJS.lib.WordArray.random(ivSize / 8);

            var options = {
                mode: CryptoJS.mode.ECB,
                padding: CryptoJS.pad.Pkcs7,
                iv: iv,
            };
            var encrypted = CryptoJS.AES.encrypt(message, key, options);
            return {
                key: key.toString(CryptoJS.enc.Base64),
                iv: iv.toString(CryptoJS.enc.Base64),
                data: encrypted.ciphertext.toString(CryptoJS.enc.Base64)
            };
        }
        $rootScope.decrypt = function (encryptedData) {
            var ivSize = 128;
            var cipherParams = CryptoJS.lib.CipherParams.create({
                ciphertext: CryptoJS.enc.Base64.parse(encryptedData.data)
            });
            var key = CryptoJS.enc.Base64.parse(encryptedData.key);
            var iv = CryptoJS.lib.WordArray.random(ivSize / 8);
            var options = {
                mode: CryptoJS.mode.ECB,
                padding: CryptoJS.pad.Pkcs7,
                iv: iv,
            };

            var decrypted = CryptoJS.AES.decrypt(
                cipherParams,
                key,
                options);
            return decrypted.toString(CryptoJS.enc.Utf8);
        }

        $rootScope.translate = function (keyword, isWrap, defaultText) {
            if ($rootScope.globalSettings && ($rootScope.translator)) {
                return $rootScope.translator.get(keyword, isWrap, defaultText) || keyword;
            }
            else {
                return keyword || defaultText;
            }
        };

        $rootScope.getConfiguration = function (keyword, isWrap, defaultText) {
            if ($rootScope.globalSettings && ($rootScope.globalSettingsService || $rootScope.isBusy)) {
                return $rootScope.globalSettingsService.get(keyword, isWrap, defaultText);
            }
            else {
                return keyword || defaultText;
            }
        };

        $rootScope.waitForInit = async function (functionName, args, scope) {
            if (!$rootScope.isInit) {
                () => {
                    setTimeout(() => {
                        $rootScope.waitForInit(functionName, args, scope);
                    }, 200);
                }
            }
            else {
                $rootScope.executeFunctionByName(functionName, args, scope);
            }
        }

        $rootScope.$watch('isBusy', function (newValue, oldValue) {
            if (newValue) {
                $rootScope.message.content = '';
                $rootScope.errors = [];
            }
        });
        $rootScope.generateUUID = function() { // Public Domain/MIT
            var d = new Date().getTime();
            if (typeof performance !== 'undefined' && typeof performance.now === 'function'){
                d += performance.now(); //use high-precision timer if available
            }
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = (d + Math.random() * 16) % 16 | 0;
                d = Math.floor(d / 16);
                return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
            });
        };
        $rootScope.filterArray = function (array, key, value) {
            var result = [];
            for (var i = 0; i < array.length; i++) {
                if (array[i][key] === value) {
                    result.push(array[i]);
                }
            }
            return result;
        };
        $rootScope.findObjectByKey = function (array, key, value) {
            for (var i = 0; i < array.length; i++) {
                if (array[i][key] == value) {
                    return array[i];
                }
            }
            return null;
        }
        $rootScope.removeObjectByKey = function (array, key, value) {
            for (var i = 0; i < array.length; i++) {
                if (array[i][key] == value) {
                    array.splice(i,1);
                    break;
                }
            }
        }
        
        $rootScope.changeLang = async function (lang) {
            var url = await translatorService.translateUrl(lang);
            translatorService.translateUrl(lang);
            window.top.location = url;
        };
        // upload on file select or drop
        $rootScope.upload = function (file, url) {
            Upload.upload({
                url: 'upload/url',
                data: { file: file, 'username': $scope.username }
            }).then(function (resp) {
                console.log('Success ' + resp.config.data.file.name + 'uploaded. Response: ' + resp.data);
            }, function (resp) {
                console.log('Error status: ' + resp.status);
            }, function (evt) {
                var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                console.log('progress: ' + progressPercentage + '% ' + evt.config.data.file.name);
            });
        };
        $rootScope.goToSiteUrl = function(url){
            window.top.location = url;
        };
        $rootScope.goToPath = function(url){
            $rootScope.isBusy = true;
            $location.url(url);
        };
        $rootScope.encryptAttributeSet = function(attributes, data){
            angular.forEach(attributes, function(attr){
                if(attr.isEncrypt){
                    angular.forEach(data, function(item){
                        var fieldData = $rootScope.findObjectByKey(item.data, 'attributeName', attr.name);
                        if(fieldData){
                            var encryptedData = $rootScope.encrypt(fieldData.stringValue);
                            fieldData.encryptValue = encryptedData.data;
                            fieldData.encryptKey = encryptedData.key;
                            fieldData.stringValue = '';
                        }
                    });
                }
            });
        };
        $rootScope.decryptAttributeSet = function(attributes, data){
            angular.forEach(attributes, function(attr){
                if(attr.isEncrypt){
                    angular.forEach(data, function(item){
                        var fieldData = $rootScope.findObjectByKey(item.data, 'attributeName', attr.name);
                        if(fieldData){
                            var encryptedData = {
                                key: fieldData.encryptKey,
                                data: fieldData.encryptValue
                            };
                            fieldData.stringValue =  $rootScope.decrypt(encryptedData);;
                        }
                    });
                }
            });
        };

        $rootScope.getODataService = function(modelName, isGlobal)
        {
            var serviceFactory = angular.copy(baseODataService);
            serviceFactory.init(modelName, isGlobal);
            return serviceFactory;            
        };

    }]);
if ($.trumbowyg) {
    $.trumbowyg.svgPath = '/assets/icons.svg';
}