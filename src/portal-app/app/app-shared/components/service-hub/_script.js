modules.component('serviceHub', {
    templateUrl: '/app/app-shared/components/service-hub/view.html',
    bindings: {
        hubName: '='
    },
    controller: ['$rootScope', '$scope', function ($rootScope, $scope) {
        var ctrl = this;
        BaseHub.call(this, ctrl);  
        
        ctrl.user = {
            loggedIn: false,
            connection: {}
        };
        ctrl.isHide = true;
        ctrl.hideContact = true;
        ctrl.members = [];
        ctrl.messages = [];
        ctrl.message = { connection: {}, content: '' };
        ctrl.request = {
            uid: '',
            action: '',
            objectType: null,
            data: {},
            room: '',
            isMyself: false
        };
        ctrl.loadMsgButton = function () {
            
        }
        ctrl.init = function () {
            ctrl.user.connection.name = Math.random() * 100;
            ctrl.user.connection.id = 'abc';
            ctrl.user.connection.avatar = '';
            ctrl.startConnection('ServiceHub', ctrl.join);            
        };
        ctrl.logout = function () {
            FB.logout(function(response) {
                // user is now logged out
                ctrl.user.loggedIn = false;
            });
        };
        ctrl.login = function () {
            FB.login(function (response) {
                if (response.authResponse) {
                    FB.api('/me', function (response) {

                        ctrl.user.info.name = response.name;
                        ctrl.user.info.id = response.id;
                        ctrl.user.info.avatar = '//graph.facebook.com/' + response.id + '/picture?width=32&height=32';
                        ctrl.join();
                        $scope.$apply();
                    });
                } else {
                    console.log('User cancelled login or did not fully authorize.');
                }
            });
        };
        ctrl.join = function () {
            ctrl.request.action = "join_group";
            ctrl.request.uid = ctrl.user.connection.id;
            ctrl.request.data = ctrl.user.connection;
            ctrl.message.connection = ctrl.user.connection;
            ctrl.connection.invoke('handleRequest', ctrl.request);

        };
        ctrl.toggle = function(){
            ctrl.isHide = !ctrl.isHide;
        }
        ctrl.toggleContact = function(){
            ctrl.hideContact = !ctrl.hideContact;
        }
        ctrl.sendMessage = function () {
            if (ctrl.user.loggedIn) {
                ctrl.request.data = ctrl.message;
                ctrl.connection.invoke('sendMessage', ctrl.request);
                ctrl.message.content = '';         
            }
        };
        ctrl.receiveMessage = function (msg) {
            switch (msg.responseKey) {
                case 'NewMember':
                    ctrl.newMember(msg.data);
                    $('.widget-conversation').scrollTop = $('.widget-conversation')[0].scrollHeight;
                    break;

                case 'NewMessage':
                    ctrl.newMessage(msg.data);
                    break;
                case 'ConnectSuccess':
                    ctrl.user.loggedIn = true;
                    ctrl.initList(msg.data);
                    $scope.$apply();
                    break;

                case 'MemberOffline':
                    ctrl.removeMember(msg.data);
                    break;

            }

        };
        ctrl.checkLoginStatus = function () {
            FB.getLoginStatus(function (response) {
                if (response.status === 'connected') {
                    // The user is logged in and has authenticated your
                    // app, and response.authResponse supplies
                    // the user's ID, a valid access token, a signed
                    // request, and the time the access token 
                    // and signed request each expire.
                    FB.api('/me', function (response) {
                        ctrl.user.info.name = response.name;
                        ctrl.user.info.id = response.id;
                        ctrl.user.info.avatar = '//graph.facebook.com/' + response.id + '/picture?width=32&height=32';
                        ctrl.join();
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
        ctrl.newMember = function (member) {
            var m = $rootScope.findObjectByKey(ctrl.members, 'id', member.id);
            if(!m){
                ctrl.members.push(member);
            }
            $scope.$apply();
        };
        
        ctrl.initList = function (data) {
            data.forEach(member => {
                var index = ctrl.members.findIndex(x => x.id === member.id);
                if (index < 0) {
                    ctrl.members.splice(0, 0, member);
                }    
            });
            
            $scope.$apply();
        }

        ctrl.removeMember = function (memberId) {

            var index = ctrl.members.findIndex(x => x.id === memberId);
            if (index >= 0) {
                ctrl.members.splice(index, 1);
            }
            $scope.$apply();
        }

        ctrl.newMessage = function (msg) {
            ctrl.messages.push(msg);
            $scope.$apply();
            // var objDiv = document.getElementsByClassName("widget-conversation")[0];
            // objDiv.scrollTop = objDiv.scrollHeight + 20;
        }
        
        
    }]
});