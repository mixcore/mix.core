'use strict';
app.controller('SocialFeedController', 
    ['$rootScope', '$scope', '$http', 
    function ($rootScope, $scope, $http) {
        $scope.types = [
          'Facebook',
          'Instagram'
        ];
        $scope.isInit = false;
        $scope.data = [];
        $scope.errors = [];
        $scope.socialSettings = {
            app_id: null,
            page_id: null,
            app_secret: null,
            access_token:'',
            show_login: true,
            errors:[]
        };
        $scope.init = function () {
            $scope.socialSettings = {
                app_id: $rootScope.settings.data.FacebookAppId,
                page_id: $rootScope.settings.data.Facebook_Page_Id,
                app_secret: $rootScope.settings.data.FacebookAppSecret,
                access_token:'',
                show_login: true,
                errors:[]
            };
        };
        
        // This function is called when someone finishes with the Login
        // Button.  See the onlogin handler attached to it in the sample
        // code below.
        $scope.login = function () {
            FB.login(function (response) {
                // handle the response
                $scope.statusChangeCallback(response);

            }, {
                scope: 'email, manage_pages',
                return_scopes: true
            });
        }
        
        $scope.statusChangeCallback = function (response) {
            // The response object is returned with a status field that lets the
            // app know the current login status of the person.
            // Full docs on the response object can be found in the documentation
            // for FB.getLoginStatus().
            if (response.status === 'connected') {
                // Logged into your app and Facebook.
                $scope.exchangeToken(response);
                $scope.loadFeeds();
                //window.location = '/bo/feed?code=' + response.authResponse.accessToken;
            } else {
                alert('Login failed');
                // The person is not logged into your app or we are unable to tell.                    
            }
        }

        $scope.exchangeToken = function (response) {
            var url = '/oauth/access_token?grant_type=fb_exchange_token&client_id='+ $scope.socialSettings.app_id +'&client_secret='+ $scope.socialSettings.app_secret +'&fb_exchange_token=' + response.authResponse.accessToken;
            FB.api(url, function (response) {
                if(response.access_token) {
                    $scope.socialSettings.access_token = response.access_token;
                }
                else{
                    $scope.show_login = true;
                    $scope.socialSettings.errors = response;
                    $scope.$apply();
                }
            });
        }
        $scope.loadFeeds = function (url) {
            $scope.socialSettings.errors = '';
            url = url || '/' + $scope.socialSettings.page_id + '/posts?access_token=' + $scope.socialSettings.access_token + '&fields=name,story,created_time,permalink_url,message,description,caption,attachments,shares.summary(true).limit(0),likes.summary(true).limit(0),comments.summary(true).limit(0)&limit=6';
            FB.api(url, function (response) {
                if (response.data) {
                    $scope.socialSettings.data = response.data;
                    $scope.socialSettings.nextUrl = response.paging.next;
                    $scope.socialSettings.prevUrl = response.paging.previous;
                    $scope.$apply();
                }
                else {
                    $scope.socialSettings.show_login = true;
                    $scope.socialSettings.errors = response;
                }
                console.log(response);
            });
        }
        $scope.setAttr = function (e, attrName, attVal) {
            $(e).attr(attrName, attVal);
        };
        $scope.addPage = function(){
            var page={
                page_name:'',
                page_id:'',
                access_token:'',
                show_login: true,
                is_edit:true
            }
            $scope.pages.push(page);
            $scope.socialSettings.pages.push(page);
        }

        $scope.removePage = function(index){
            $scope.pages.splice(index,1);
            $scope.socialSettings.pages.splice(index,1);
            $scope.strSettings = JSON.stringify($scope.socialSettings);
            $('.data').val(JSON.stringify($scope.socialSettings));
        }
    }]);