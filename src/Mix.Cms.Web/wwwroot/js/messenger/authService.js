'use strict';
app.factory('AuthService', ['$http', '$q', 'localStorageService', 'ngAppSettings', function ($http, $q, localStorageService, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        username: "",
        userId: '',
        token: '',
        useRefreshTokens: false,
        avatarUrl: "",
        referredUrl: '/Home'
    };

    var _externalAuthData = {
        provider: "",
        username: "",
        externalAccessToken: ""
    };

    var _saveRegistration = function (registration) {

        _logOut();
        var deferred = $q.defer();
        return $http.post(serviceBase + '/api/account/register', registration)
            .success(function (response) {
                if (response.data.userData !== undefined) {

                    localStorageService.set('authorizationData',
                        {
                            token: response.data.access_token
                            , username: response.data.userData.userName
                            , firstName: response.data.userData.firstName
                            , lastName: response.data.userData.lastName
                            , roleNames: response.data.userData.roleNames
                            , avatarUrl: response.data.userData.avatarUrl
                            , refresh_token: response.data.refresh_token
                            , userId: response.data.userData.id
                        });
                    _authentication.isAuth = true;
                    _authentication.isAdmin = $.inArray("Admin", response.data.userData.RoleNames) >= 0;
                    _authentication.username = response.data.userData.userName;
                    _authentication.firstName = response.data.userData.firstName;
                    _authentication.lastName = response.data.userData.lastName;
                    _authentication.roleNames = response.data.userData.roleNames;
                    _authentication.userId = response.data.userData.id;
                    _authentication.avatarUrl = response.data.userData.avatarUrl;
                    _authentication.token = response.data.access_token;
                    _authentication.refresh_token = response.data.refresh_token;
                }
                deferred.resolve(response);
            })
            .error(function (err, status) {
                _logOut();
                deferred.reject(err);
            });
    };

    var _login = function (loginData) {

        //var data = "grant_type=password&email=" + loginData.username + "&deviceId=" + loginData.password;
        var data = {
            username: loginData.username,
            password: loginData.password,
            grant_type: 'password'
        };
        //if (loginData.useRefreshTokens) {
        //    data = data + "&client_id=" + ngAuthSettings.clientId;
        //}

        var deferred = $q.defer();

        $http.post(serviceBase + '/api/account/login', data).success(function (response) {
            if (response.data.userData !== null) {

                localStorageService.set('authorizationData',
                    {
                        token: response.data.access_token
                        , username: response.data.userData.userName
                        , firstName: response.data.userData.firstName
                        , lastName: response.data.userData.lastName
                        , roleNames: response.data.userData.roleNames
                        , avatarUrl: response.data.userData.avatarUrl
                        , refresh_token: response.data.refresh_token
                        , userId: response.data.userData.id
                    });
                _authentication.isAuth = true;
                _authentication.isAdmin = $.inArray("Admin", response.data.userData.RoleNames) >= 0;
                _authentication.username = response.data.userData.userName;
                _authentication.firstName = response.data.userData.firstName;
                _authentication.lastName = response.data.userData.lastName;
                _authentication.roleNames = response.data.userData.roleNames;
                _authentication.userId = response.data.userData.id;
                _authentication.avatarUrl = response.data.userData.avatarUrl;
                _authentication.useRefreshTokens = loginData.useRefreshTokens;
                _authentication.token = response.data.access_token;
                _authentication.refresh_token = response.data.refresh_token;
            }
        
            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _logOut = function () {

        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.isAdmin = false;
        _authentication.username = "";
        _authentication.firstName = '';
        _authentication.lastName = '';
        _authentication.useRefreshTokens = false;

    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.isAdmin = $.inArray("Admin", authData.roleNames) >= 0;
            _authentication.username = authData.username;
            _authentication.firstName = authData.firstName;
            _authentication.lastName = authData.lastName;
            _authentication.roleNames = authData.roleNames;
            _authentication.userId = authData.userId;
            _authentication.avatarUrl = authData.avatarUrl;

            _authentication.token = authData.token;
            _authentication.refresh_token = authData.refresh_token;
        }

    };

    var _refreshToken = function () {
        var deferred = $q.defer();

        var authData = localStorageService.get('authorizationData');

        if (authData) {

            if (authData.useRefreshTokens) {

                var data = "grant_type=refresh_token&refresh_token=" + authData.refresh_token + "&client_id=" + ngAuthSettings.clientId;

                localStorageService.remove('authorizationData');

                $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                    localStorageService.set('authorizationData', { token: response.access_token, username: response.username, refresh_token: response.refresh_token, useRefreshTokens: true });

                    deferred.resolve(response);

                }).error(function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });
            }
        }

        return deferred.promise;
    };

    var _obtainAccessToken = function (externalData) {

        var deferred = $q.defer();

        $http.get(serviceBase + '/api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, username: response.username, roleName: response.userData.roleNames, refresh_token: response.refresh_token, useRefreshTokens: true });

            _authentication.isAuth = true;
            _authentication.isAdmin = _authentication.isAdmin = $.inArray("Admin", response.userData.RoleNames) >= 0;
            _authentication.username = response.username;
            _authentication.useRefreshTokens = false;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _registerExternal = function (registerExternalData) {

        var deferred = $q.defer();

        $http.post(serviceBase + '/api/account/registerexternal', registerExternalData).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, username: response.username, refresh_token: response.refresh_token, useRefreshTokens: true });

            _authentication.isAuth = true;
            _authentication.username = response.username;
            _authentication.useRefreshTokens = false;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.refreshToken = _refreshToken;

    authServiceFactory.obtainAccessToken = _obtainAccessToken;
    authServiceFactory.externalAuthData = _externalAuthData;
    authServiceFactory.registerExternal = _registerExternal;

    return authServiceFactory;
}]);