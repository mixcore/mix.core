var TTX = window.TTX || {};

(function ($) {
    TTX.Common = {
        CurrerntUser: null,
        edtModule: '',
        addExternalLoginUrl: "/api/Account/AddExternalLogin",
        changePasswordUrl: "/api/Account/changePassword",
        loginUrl: "/Token",
        logoutUrl: "/api/Account/Logout",
        registerUrl: "/api/Account/Register",
        registerExternalUrl: "/api/Account/RegisterExternal",
        removeLoginUrl: "/api/Account/RemoveLogin",
        setPasswordUrl: "/api/Account/setPassword",
        siteUrl: "/",
        userInfoUrl: "/api/Account/UserInfo",

        getBase64: function (file) {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function () {
                return reader.result;
            };
            reader.onerror = function (error) {
                
            };
            return null;
        },       
        test:function(msg){
            alert(msg);
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
        externalLoginsUrl: function (returnUrl, generateState) {
            return "/api/Account/ExternalLogins?returnUrl=" + (encodeURIComponent(returnUrl)) +
                "&generateState=" + (generateState ? "true" : "false");
        },

        manageInfoUrl: function (returnUrl, generateState) {
            return "/api/Account/ManageInfo?returnUrl=" + (encodeURIComponent(returnUrl)) +
                "&generateState=" + (generateState ? "true" : "false");
        },


        // Other private operations

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
            messages.prepend("<li style='color:green;'>" + TTX.Common.getTimeString() + ' ' + line + "</li>");
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

        // Data access operations
        addExternalLogin: function (data) {
            return $.ajax(addExternalLoginUrl, {
                type: "POST",
                data: data,
                headers: getSecurityHeaders()
            });
        },

        changePassword: function (data) {
            return $.ajax(changePasswordUrl, {
                type: "POST",
                data: data,
                headers: getSecurityHeaders()
            });
        },

        getExternalLogins: function (returnUrl, generateState) {
            return $.ajax(externalLoginsUrl(returnUrl, generateState), {
                cache: false,
                headers: getSecurityHeaders()
            });
        },

        getManageInfo: function (returnUrl, generateState) {
            return $.ajax(manageInfoUrl(returnUrl, generateState), {
                cache: false,
                headers: getSecurityHeaders()
            });
        },

        getUserInfo: function (accessToken) {
            var headers;

            if (typeof (accessToken) !== "undefined") {
                headers = {
                    "Authorization": "Bearer " + accessToken
                };
            } else {
                headers = getSecurityHeaders();
            }

            return $.ajax(userInfoUrl, {
                cache: false,
                headers: headers
            });
        },

        logout: function () {
            return $.ajax(logoutUrl, {
                type: "POST",
                headers: getSecurityHeaders()
            });
        },

        register: function (data) {
            return $.ajax(registerUrl, {
                type: "POST",
                data: data
            });
        },

        registerExternal: function (accessToken, data) {
            return $.ajax(registerExternalUrl, {
                type: "POST",
                data: data,
                headers: {
                    "Authorization": "Bearer " + accessToken
                }
            });
        },

        removeLogin: function (data) {
            return $.ajax(removeLoginUrl, {
                type: "POST",
                data: data,
                headers: getSecurityHeaders()
            });
        },

        setPassword: function (data) {
            return $.ajax(setPasswordUrl, {
                type: "POST",
                data: data,
                headers: getSecurityHeaders()
            });
        },

        htmlEncode: function (value) {
            var encodedValue = $('<div />').text(value).html();
            return encodedValue;
        },

        login: function (username, password, exeFuncName, exeFuncParams) {
            $.ajax({
                type: "POST",
                url: '/token',
                data: { grant_type: 'password', username: username, password: password },
                success: function (data) {
                    TTX.Common.CurrerntUser = data;
                    $.signalR.ajaxDefaults.headers = { Authorization: "Bearer " + data.access_token };
                    TTX.Common.executeFunctionByName(exeFuncName, window, exeFuncParams)
                    TTX.Common.setAccessToken(data.access_token, true);
                    TTX.Common.setCurrentUser(data);
                    return data;
                },
                error: function (e) {
                    alert(e.responseJSON.error_description);
                }
            });

        }
    }
}(jQuery));