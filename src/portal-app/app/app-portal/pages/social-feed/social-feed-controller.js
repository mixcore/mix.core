'use strict';
app.controller('SocialFeedController',
    ['$rootScope', '$scope', '$http', 'PostService',
        function ($rootScope, $scope, $http, postService) {
            $scope.types = [
                'Facebook',
                'Instagram'
            ];
            $scope.isInit = false;
            $scope.defaultPost = null;
            $scope.defaultProperty = {
                name: null,
                dataType: 7,
                value: ""
            }
            $scope.data = [];
            $scope.errors = [];
            $scope.socialSettings = {
                app_id: null,
                page_id: null,
                app_secret: null,
                access_token: '',
                page: [],
                data: [],
                posts: [],
                show_login: true,
                errors: []
            };
            $scope.init = async function () {

                window.fbAsyncInit = function () {
                    FB.init({
                        appId: $rootScope.getConfiguration('FacebookAppId'),
                        autoLogAppEvents: true,
                        xfbml: true,
                        version: 'v3.2'
                    });
                };
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
                postService.getSingle(['portal']).then((resp) => {
                    $scope.defaultPost = resp.data;
                });
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
                $scope.socialSettings.posts = [];
                url = url || '/' + $scope.socialSettings.page_id + '/posts?access_token=' + $scope.socialSettings.access_token + '&fields=type,name,story,full_picture,created_time,permalink_url,message,description,caption,attachments{media,type,target,subattachments},shares.summary(true).limit(0),likes.summary(true).limit(0),comments.summary(true).limit(0)&limit=10';
                $rootScope.isBusy = true;
                FB.api(url, function (response) {
                    if (response.data) {
                        $scope.socialSettings.data = response.data;
                        angular.forEach(response.data, function (e, i) {
                            var post = $scope.parsePost(e);
                            $scope.socialSettings.posts.push(post);
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
            $scope.parsePost = function (post) {
                var post = angular.copy($scope.defaultPost);
                var prop = angular.copy($scope.defaultProperty);
                post.title = "Facebook Id";
                post.name = "facebook_id";
                post.value = post.id;
                post.properties.push(prop);

                post.title = post.name || post.id;
                post.excerpt = post.message;
                post.content = post.description;
                post.source = 'Facebook';
                post.image = post.full_picture;
                post.detailsUrl = post.permalink_url
                var attachments = post.attachments.data[0];

                if (attachments.media) {
                    var media = $scope.parseMedia(attachments.media, attachments.type);
                    if (media) {
                        post.mediaNavs.push({
                            media: media,
                            specificulture: $rootScope.settings.lang,
                            image: media.fullPath
                        });
                    }
                }

                if (attachments.subattachments) {
                    var medias = $scope.parseMedias(attachments.subattachments.data);
                    angular.forEach(medias, function (e, i) {
                        post.mediaNavs.push({
                            media: e,
                            specificulture: $rootScope.settings.lang,
                            image: e.fullPath
                        })
                    })
                }
                return post;
            }
            $scope.parseMedias = function (data) {
                var result = [];
                if (data) {
                    angular.forEach(data, function (e, i) {
                        var media = $scope.parseMedia(e.media, e.type);
                        if (media) {
                            result.push(media);
                        }
                    })
                }
                return result;
            }
            $scope.parseMedia = function (media, type) {
                if (media) {
                    var src = '';
                    switch (type) {
                        case 'video_autoplay':
                            src = media.source;
                            break;
                        case 'profile_media':
                        case 'photo':
                        default:
                            src = media.image.src;
                            break;
                    }
                    try {
                        if (src) {
                            var index = src.lastIndexOf('/');
                            var ext = src.match(/.(?:jpg|gif|png|gif|jpeg|mp4)/)[0];
                            // if valid  file
                            if (ext) {
                                var eIndex = src.indexOf(ext);
                                var filename = src.substring(index + 1, eIndex);
                                var media = {
                                    fileName: filename,
                                    fileFolder: 'Facebook',
                                    extension: ext,
                                    targetUrl: src,
                                    fullPath: src,
                                    fileType: type,
                                    source: 'Facebook'
                                }

                                return media;
                            }
                            else {
                                return null;
                            }
                        } else {
                            return null;
                        }
                    }
                    catch (err) {
                        console.log('Cannot parse media', media);
                        return null;
                    }
                }
            }
            $scope.syncPosts = async function () {
                $rootScope.isBusy = true;
                var resp = await postService.saveList($scope.socialSettings.posts);
                if (resp && resp.isSucceed) {
                    $rootScope.showMessage('success', 'success');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            }
        }]);