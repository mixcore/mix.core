'use strict';
app.controller('MessengerController', ['$scope', function ($scope) {
    BaseHub.call(this, $scope);
    $scope.user = {
        loggedIn: false,
        info: {}
    };
    $scope.isHide = false;
    $scope.members = [];
    $scope.messages = [];
    $scope.message = { connection: {}, content: '' };
    $scope.request = {
        uid: '',
        objectType: null,
        action: '',
        data: {},
        room: '',
        isMyself: false
    };
    $scope.init = function () {
        $scope.startConnection('MixChatHub', $scope.checkLoginStatus);
    };

    $scope.login = function () {
        FB.login(function (response) {
            if (response.authResponse) {
                FB.api('/me', function (response) {

                    $scope.user.info.name = response.name;
                    $scope.user.info.id = response.id;
                    $scope.user.info.avatar = '//graph.facebook.com/' + response.id + '/picture?width=32&height=32';
                    $scope.join();
                    $scope.$apply();
                });
            } else {
                console.log('User cancelled login or did not fully authorize.');
            }
        });
    };
    $scope.join = function () {
        $scope.request.uid = $scope.user.info.id;
        $scope.request.data = $scope.user.info;
        $scope.message.connection = $scope.user.info;
        $scope.connection.invoke('join', $scope.request);

    };
    $scope.toggle = function(){
        $scope.isHide = !$scope.isHide;
        $scope.$apply();
    }
    $scope.sendMessage = function () {
        if ($scope.user.loggedIn) {
            $scope.request.data = $scope.message;
            $scope.connection.invoke('sendMessage', $scope.request);
            $scope.message.content = '';
            //$scope.$apply();
        }
    };
    $scope.receiveMessage = function (msg) {
        //$scope.responses.splice(0, 0, msg);
        switch (msg.responseKey) {
            case 'NewMember':
                $scope.newMember(msg.data);
                break;

            case 'NewMessage':
                $scope.newMessage(msg.data);
                break;
            case 'ConnectSuccess':
                $scope.user.loggedIn = true;
                $scope.initList(msg.data);
                $scope.$apply();
                break;

            case 'MemberOffline':
                $scope.removeMember(msg.data);
                break;

        }

    };
    $scope.checkLoginStatus = function () {
        FB.getLoginStatus(function (response) {
            if (response.status === 'connected') {
                // The user is logged in and has authenticated your
                // app, and response.authResponse supplies
                // the user's ID, a valid access token, a signed
                // request, and the time the access token 
                // and signed request each expire.
                FB.api('/me', function (response) {
                    $scope.user.info.name = response.name;
                    $scope.user.info.id = response.id;
                    $scope.user.info.avatar = '//graph.facebook.com/' + response.id + '/picture?width=32&height=32';
                    $scope.join();
                    $scope.$apply();
                });
            } else if (response.status === 'authorization_expired') {
                // The user has signed into your application with
                // Facebook Login but must go through the login flow
                // again to renew data authorization. You might remind
                // the user they've used Facebook, or hide other options
                // to avoid duplicate account creation, but you should
                // collect a user gesture (e.g. click/touch) to launch the
                // login dialog so popup blocking is not triggered.
            } else if (response.status === 'not_authorized') {
                // The user hasn't authorized your application.  They
                // must click the Login button, or you must call FB.login
                // in response to a user gesture, to launch a login dialog.
            } else {
                // The user isn't logged in to Facebook. You can launch a
                // login dialog with a user gesture, but the user may have
                // to log in to Facebook before authorizing your application.
            }
        });
    };
    $scope.newMember = function (member) {

        var index = $scope.members.findIndex(x => x.id === member.id);
        if (index < 0) {
            $scope.members.splice(0, 0, member);
        }
        $scope.$apply();
    }
    
    $scope.initList = function (data) {
        data.forEach(member => {
            var index = $scope.members.findIndex(x => x.id === member.id);
            if (index < 0) {
                $scope.members.splice(0, 0, member);
            }    
        });
        
        $scope.$apply();
    }

    $scope.removeMember = function (memberId) {

        var index = $scope.members.findIndex(x => x.id === memberId);
        if (index >= 0) {
            $scope.members.splice(index, 1);
        }
        $scope.$apply();
    }

    $scope.newMessage = function (msg) {
        $scope.messages.push(msg);
        $scope.$apply();
        var objDiv = document.getElementById("mix-discussion");
        objDiv.scrollTop = objDiv.scrollHeight + 20;
    }
}]);

