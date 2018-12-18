'use strict';
app.controller('MessengerController', ['$scope', function ($scope) {
    BaseHub.call(this, $scope);
    $scope.user = {
        loggedIn: false,
        info: {}
    };
    $scope.isHide = true;
    $scope.hideContact = true;
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
    $scope.loadMsgButton = function () {
        
}
    $scope.init = function () {
        $scope.startConnection('MixChatHub', $scope.checkLoginStatus);    
                
          $("button").on("click", function() {
            var text = $("#message").val();
            var hnow = new Date().getHours();
            var mnow = new Date().getMinutes();
            mnow = mnow < 10 ? "0" + mnow : mnow;
            var d = hnow + ":" + mnow;
        
            if (text.length > 0) {
              $("#message").css("border", "1px solid #f4f5f9");
              $("#conversation").append(
                "<li class='message-right'><div class='message-avatar'><div class='avatar ion-ios-person'></div><div class='name'>You</div></div><div class='message-text'>" +
                  text +
                  "</div><div class='message-hour'>" +
                  d +
                  " <span class='ion-android-done-all'></span></div></li>"
              );
              $("#message").val("");
              $(".widget-conversation").scrollTop(
                $("ul li")
                  .last()
                  .position().top +
                  $("ul li")
                    .last()
                    .height()
              );
            } else {
              $("#message").css("border", "1px solid #eb9f9f");
              $("#message").animate({ opacity: "0.1" }, "slow");
              $("#message").animate({ opacity: "1" }, "slow");
              $("#message").animate({ opacity: "0.1" }, "slow");
              $("#message").animate({ opacity: "1" }, "slow");
            }
          });
  
        
        
        // var _createClass = function () {function defineProperties(target, props) {for (var i = 0; i < props.length; i++) {var descriptor = props[i];descriptor.enumerable = descriptor.enumerable || false;descriptor.configurable = true;if ("value" in descriptor) descriptor.writable = true;Object.defineProperty(target, descriptor.key, descriptor);}}return function (Constructor, protoProps, staticProps) {if (protoProps) defineProperties(Constructor.prototype, protoProps);if (staticProps) defineProperties(Constructor, staticProps);return Constructor;};}();function _classCallCheck(instance, Constructor) {if (!(instance instanceof Constructor)) {throw new TypeError("Cannot call a class as a function");}}var HoverButton = function () {
        //   function HoverButton(el) {_classCallCheck(this, HoverButton);
        //     this.el = el;
        //     this.hover = false;
        //     this.calculatePosition();
        //     this.attachEventsListener();
        //   }_createClass(HoverButton, [{ key: 'attachEventsListener', value: function attachEventsListener()
        
        //     {var _this = this;
        //       window.addEventListener('mousemove', function (e) {return _this.onMouseMove(e);});
        //       window.addEventListener('resize', function (e) {return _this.calculatePosition(e);});
        //     } }, { key: 'calculatePosition', value: function calculatePosition()
        
        //     {
        
        
        //       var box = this.el.getBoundingClientRect();
        //       this.x = box.left + box.width * 0.5;
        //       this.y = box.top + box.height * 0.5;
        //       this.width = box.width;
        //       this.height = box.height;
        //     } }, { key: 'onMouseMove', value: function onMouseMove(
        
        //     e) {
        //       var hover = false;
        //       var hoverArea = this.hover ? 0.7 : 0.5;
        //       var x = e.clientX - this.x;
        //       var y = e.clientY - this.y;
        //       var distance = Math.sqrt(x * x + y * y);
        //       if (distance < this.width * hoverArea) {
        //         hover = true;
        //         if (!this.hover) {
        //           this.hover = true;
        //         }
        //         this.onHover(e.clientX, e.clientY);
        //       }
        
        //       if (!hover && this.hover) {
        //         this.onLeave();
        //         this.hover = false;
        //       }
        //     } }, { key: 'onHover', value: function onHover(
        
        //     x, y) {
        //       TweenMax.to(this.el, 0.4, {
        //         x: (x - this.x) * 0.4,
        //         y: (y - this.y) * 0.4,
        //         scale: 1.15,
        //         ease: Power2.easeOut });
        
        //       this.el.style.zIndex = 10;
        //     } }, { key: 'onLeave', value: function onLeave()
        //     {
        //       TweenMax.to(this.el, 0.7, {
        //         x: 0,
        //         y: 0,
        //         scale: 1,
        //         ease: Elastic.easeOut.config(1.2, 0.4) });
        
        //       this.el.style.zIndex = 1;
        //     } }]);return HoverButton;}();
        
        
        // var btn1 = document.querySelector('#quest');
        // new HoverButton(btn1);
    };
    $scope.logout = function () {
        FB.logout(function(response) {
            // user is now logged out
            $scope.user.loggedIn = false;
          });
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
    }
    $scope.toggleContact = function(){
        $scope.hideContact = !$scope.hideContact;
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
                $('.widget-conversation').scrollTop = $('.widget-conversation')[0].scrollHeight;
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
        var objDiv = document.getElementsByClassName("widget-conversation")[0];
        objDiv.scrollTop = objDiv.scrollHeight + 20;
    }
    
    
}]);

