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
                access_token: '',
                page: [],
                data: [],
                articles:[],
                show_login: true,
                errors: []
            };
            $scope.init = function () {
                $scope.socialSettings = {
                    app_id: $rootScope.settings.data.FacebookAppId,
                    page_id: $rootScope.settings.data.Facebook_Page_Id,
                    app_secret: $rootScope.settings.data.FacebookAppSecret,
                    access_token: $rootScope.settings.data.FacebookAccessToken,
                    show_login: true,
                    errors: []
                };
                if ($scope.socialSettings.access_token) {
                    $scope.socialSettings.show_login = false;
                    $scope.loadPages();

                }
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
                    $scope.loadPages();
                    //window.location = '/bo/feed?code=' + response.authResponse.accessToken;
                } else {
                    $scope.socialSettings.show_login = true;
                    $scope.$apply();
                    // The person is not logged into your app or we are unable to tell.                    
                }
            }

            $scope.exchangeToken = function (response) {
                var url = '/oauth/access_token?grant_type=fb_exchange_token&client_id=' + $scope.socialSettings.app_id + '&client_secret=' + $scope.socialSettings.app_secret + '&fb_exchange_token=' + response.authResponse.accessToken;
                FB.api(url, function (response) {
                    if (response.access_token) {
                        $scope.socialSettings.access_token = response.access_token;
                    }
                    else {
                        $scope.show_login = true;
                        $scope.socialSettings.errors = response;
                        $scope.$apply();
                    }
                });
            }
            $scope.loadFeeds = function (url) {
                $scope.socialSettings.errors = '';
                $scope.socialSettings.articles = [];
                url = url || '/' + $scope.socialSettings.page_id + '/posts?access_token=' + $scope.socialSettings.access_token + '&fields=type,name,story,full_picture,created_time,permalink_url,message,description,caption,attachments{subattachments},shares.summary(true).limit(0),likes.summary(true).limit(0),comments.summary(true).limit(0)&limit=10';
                $rootScope.isBusy = true;
                FB.api(url, function (response) {
                    if (response.data) {
                        $scope.socialSettings.data = response.data;
                        angular.forEach(response.data, function(e,i){
                            var article = $scope.parsePost(e);
                            $scope.socialSettings.articles.push(article);
                        });
                        if (response.paging) {
                            $scope.socialSettings.nextUrl = response.paging.next;
                            $scope.socialSettings.prevUrl = response.paging.previous;
                        }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                    else {
                        $scope.socialSettings.show_login = true;
                        $rootScope.isBusy = false;
                        $scope.socialSettings.errors = response;
                        $scope.$apply();
                    }
                    console.log(response);
                });
            }
            $scope.loadPages = function () {
                $scope.socialSettings.errors = '';
                var url = '/me/accounts?access_token=' + $scope.socialSettings.access_token + '&fields=id,name';
                FB.api(url, function (response) {
                    if (response.data) {
                        $scope.socialSettings.pages = response.data;
                        $scope.$apply();
                    }
                    else {
                        $scope.socialSettings.show_login = true;
                        $scope.socialSettings.errors = response;
                    }
                });
            }
            $scope.setAttr = function (e, attrName, attVal) {
                $(e).attr(attrName, attVal);
            };
            $scope.parsePost = function(post){
                var article = {
                    title: post.name || post.id,
                    excerpt: post.message,
                    content: post.description,
                    image: post.full_picture,
                    mediaNavs: []

                };
                if(post.attachments){
                    var medias = $scope.parseMedias(post.attachments.data[0].subattachments.data);
                    angular.forEach(medias, function(e,i){
                        article.mediaNavs.push({
                            media: e,
                            specificulture: $rootScope.settings.lang,
                            image: e.fullPath
                        })
                    })
                }
                return article;
            }
            $scope.parseMedias = function(data){
                var result = [];
                if (data) {
                    angular.forEach(data, function (e, i) {
                        if (e.media) {
                            var index = e.media.image.src.lastIndexOf('/');
                            var eIndex  = e.media.image.src.lastIndexOf('.')|| e.media.image.src.length;
                            var qIndex  = e.media.image.src.lastIndexOf('?')|| e.media.image.src.length;
                            var folder = e.media.image.src.substring(0,index);
                            var filename =e.media.image.src.substring(index,eIndex);
                            var extension = e.media.image.src.substring(eIndex,qIndex);
                            var media = {
                                fileFolder: folder,
                                fileName: filename,
                                extension: extension,
                                fullPath: folder + '/' + filename + extension
                            }
                            result.push(media);
                        }
                    })
                }
                return result;
            }
        }]);