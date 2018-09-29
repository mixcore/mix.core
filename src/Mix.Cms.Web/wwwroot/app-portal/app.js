'use strict';
var app = angular.module('SwastikaPortal', ['ngRoute', 'components', 'ngFileUpload', 'LocalStorageModule',
    'bw.paging', 'dndLists', 'ngTagsInput', 'ngSanitize']);
var modules = angular.module('components', []);
app.run(['$rootScope', '$location', 'CommonService', 'AuthService', 'TranslatorService','GlobalSettingsService', 'ngAppSettings',
    function ($rootScope, $location, commonServices, authService, translatorService, configurationService, ngAppSettings) {
        $rootScope.currentContext = $rootScope;
        $rootScope.isBusy = false;
        $rootScope.translator = translatorService;
        $rootScope.configurationService = configurationService;
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
        $rootScope.range = function (max) {
            var input = [];
            for (var i = 1; i <= max; i += 1) input.push(i);
            return input;
        };
        
        $rootScope.generateKeyword = function (src, character) {
            return src.replace(/[^a-zA-Z0-9]+/g, character)
                .replace(/([A-Z]+)([A-Z][a-z])/g, '$1-$2')
                .replace(/([a-z])([A-Z])/g, '$1-$2')
                .replace(/([0-9])([^0-9])/g, '$1-$2')
                .replace(/([^0-9])([0-9])/g, '$1-$2')
                .replace(/-+/g, character)
                .toLowerCase();
        };
        
        $rootScope.generatePhone = function(src){
            return src.replace(/^([0-9]{3})([0-9]{3})([0-9]{4})$/,'($1) $2-$3');
        }

        $rootScope.logOut = function () {
            authService.logOut();
            window.top.location.href = '/portal/login';
        };

        $rootScope.updateSettings = function () {
            commonServices.removeSettings();
            commonServices.fillSettings($rootScope.globalSettings.lang).then(function (response) {
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

        $rootScope.preview = function (type, data, title, size) {
            $rootScope.previewObject = {
                title: title || 'Preview',
                size: size || 'modal-md',
                type: type,
                data: data
            };
            $('#dlg-preview-popup').modal('show');
        };

        $rootScope.initEditor = function () {
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

                $.each($('.editor-content'), function (i, e) {
                    var $textArea = $(e);
                    $textArea.trumbowyg(ngAppSettings.editorConfigurations.plugins);

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

        $rootScope.translate = function (keyword, isWrap, defaultText) {
            if ($rootScope.globalSettings && ($rootScope.translator || $rootScope.isBusy)) {
                return $rootScope.translator.get(keyword, isWrap, defaultText);
            }
            else {
                return keyword || defaultText;
            }
        };
        
        $rootScope.getConfiguration = function (keyword, isWrap, defaultText) {
            if ($rootScope.globalSettings && ($rootScope.configurationService || $rootScope.isBusy)) {
                return $rootScope.configurationService.get(keyword, isWrap, defaultText);
            }
            else {
                return keyword || defaultText;
            }
        };

        $rootScope.$watch('isBusy', function (newValue, oldValue) {
            if (newValue) {
                $rootScope.message.content = '';
                $rootScope.errors = [];
            }
        });

    }]);

function Filter($filter) {
    return function (utcDateString, format) {
        // return if input date is null or undefined
        if (!utcDateString) {
            return;
        }

        // append 'Z' to the date string to indicate UTC time if the timezone isn't already specified
        if (utcDateString.indexOf('Z') === -1 && utcDateString.indexOf('+') === -1) {
            utcDateString += 'Z';
        }

        // convert and format date using the built in angularjs date filter
        return $filter('date')(utcDateString, format);
    };
}
