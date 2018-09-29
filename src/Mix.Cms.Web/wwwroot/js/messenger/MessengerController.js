'use strict';
app.controller('MessengerController', [ '$scope', '$location', 'AuthService', function ($scope, $location, authService) {

    $scope.pageClass = 'page-chat';
    $scope.authentication = authService.authentication;
    $scope.response = null;
    $scope.rsHubMethod = null;
    $scope.invokeHubMethod = null;
    $scope.request = null;
    $scope.connection = null;
    $scope.proxy = null;
    $scope.discussions = [];
    $scope.users = [];
    $scope.targetCnnId = '';
    $scope.user = {
        userId: $scope.authentication.userId,
        username: $scope.authentication.username,
        connectionId: '',
        mode: 'idle', // UI mode ['idle', 'calling', 'incall']
        loading: false // Loading indicator control
        //userAvatar: $scope.authentication.avatar
    };
    $scope.target = {
        userId: '',
        username: '',
        connectionId: '',
        mode: 'idle', // UI mode ['idle', 'calling', 'incall']
        loading: false // Loading indicator control
        //userAvatar: $scope.authentication.avatar
    };
    $scope.getTeamRequest = {
        userId: $scope.authentication.userId,
        teamId: 0,
        memberStatus: 0
    };
    $scope.joinTeamRequest = {
        userId: $scope.authentication.userId,
        teamId: 0,
        memberStatus: 0
    };
    $scope.createTeamData = {
        name: '',
        avatarUrl: '',
        type: 0,
        hostId: $scope.authentication.userId,
        isOpen: true,
        avatarFileStream: {
            base64: null,
            name: '',
            size: 0,
            type: ''
        }
    };
    $scope.team = {
        id: 0,
        teamInfo: {},
        Messages: {
            models: [],
            pageIndex: 0,
            pageSize: 1
        }
    };
    $scope._mediaStream = null;
    $scope.teams = [];
    $scope.otherTeams = [];
    $scope.message = {
        userId: $scope.authentication.userId,
        username: $scope.authentication.firstName + ' ' + $scope.authentication.lastName,
        avatarUrl: $scope.authentication.avatarUrl,
        teamId: $scope.team.id,
        message: ''
    }
    $scope.startChatHub = function () {
        $.signalR.ajaxDefaults.headers = { "Authorization": $scope.authentication.token };
        $scope.connection = $.hubConnection();
        $scope.connection.url = serviceBase + 'signalr/hubs'
        $scope.proxy = $scope.connection.createHubProxy('messenger');

        //$scope.connectionManager = WebRtcDemo.ConnectionManager;

        // Create a function that the hub can call back to display messages.
        $scope.proxy.on('receiveMessage', function (response) {
            // Add the message to the page.
            $scope.rsHubMethod = 'receiveMessage';
            $scope.response = TTX.Common.prettyJsonObj(response);
            var message = response.data;
            switch (response.responseKey) {
                case 'UpdateOnlineStatus':
                    if (response.status === 1 && $scope.team.id === message.teamId) {
                        $.each($scope.team.members.items, function (i, val) {
                            if (val.memberId === message.userId) {
                                $scope.team.members.items[i].isOnline = message.isOnline;
                                $scope.team.members.items[i].chatInfo.connectionId = message.connectionId;
                                $scope.$apply();
                                return false;
                            }
                        });
                    }
                    break;
                case 'SendMessage':
                    if (response.status === 1) {

                        //$.each($scope.teams, function (index, team) {
                        if ($scope.team.id === message.teamId) {
                            $scope.team.messages.items.push(message);
                            $scope.seenTeamMessages();

                            $("#discussion").animate({
                                scrollTop: $('#discussion')[0].scrollHeight - $('#discussion')[0].clientHeight
                            }, 500);
                        }
                        else {
                            $.each($scope.teams, function (i, val) {
                                if (val.id === message.teamId) {
                                    $scope.teams[i].isNewMessage = true;
                                    return false;
                                }
                            })
                        }
                        //});
                    }
                    break;
                case 'Connect':
                    if (response.status === 1) {
                        $scope.teams = response.data.teams.items;
                        $scope.otherTeams = response.data.otherTeams.items;
                        //$scope._startSession();
                        $scope.startChatHub();
                        if ($scope.teams.length > 0) {
                            //$scope.getTeam($scope.teams[0].id);
                        }
                    }
                    break;
                case 'GetTeam':
                    if (response.status === 1) {
                        $scope.team = response.data;
                        $scope.seenTeamMessages();
                        $.each($scope.teams, function (i, val) {
                            if (val.id === $scope.team.id) {
                                $scope.teams[i].isNewMessage = false;
                                $scope.$apply();
                            }
                        })
                        setTimeout(function () {
                            $("#discussion").animate({
                                scrollTop: $('#discussion')[0].scrollHeight - $('#discussion')[0].clientHeight
                            }, 500);
                        }, 500);
                    }
                    break;
                case 'RemovedTeam':
                    if (response.status === 1) {
                        $scope.proxy.invoke('hubconnect', $scope.user);
                    }
                    break;
            }
            $scope.$apply()
        });
        //$scope._setupHubCallbacks();
        //Tracking
        $scope.connection.logging = true;

        $scope.connection.connectionSlow(function () {
            TTX.Common.writeEvent("connectionSlow");
        });

        $scope.connection.disconnected(function () {
            TTX.Common.writeEvent("disconnected");
        });

        $scope.connection.error(function (error) {
            var innerError = error;
            var message = "";
            while (innerError) {
                message += " Message=" + innerError.message + " Stack=" + innerError.stack;
                innerError = innerError.source
            }
            TTX.Common.writeError("Error: " + message);
        });

        $scope.connection.reconnected(function () {
            TTX.Common.writeEvent("reconnected id:" + $scope.connection.id);
            console.log($scope.connection);
        });

        $scope.connection.reconnecting(function () {
            TTX.Common.writeEvent("reconnecting id:" + $scope.connection.id);
        });

        $scope.connection.starting(function () {
            TTX.Common.writeEvent("starting");
        });

        $scope.connection.received(function (data) {
            //$scope.message = 'done';
            TTX.Common.writeLine("received: \n" + TTX.Common.prettyJsonObj(data));
        });

        $scope.connection.stateChanged(function (change) {
            TTX.Common.writeEvent("stateChanged: " + TTX.Common.printState(change.oldState)
                + " => " + TTX.Common.printState(change.newState)
                + " cnnId: " + $scope.connection.id);
        });

        // Start the connection.
        $scope.connection.start({ transport: ['webSockets', 'longPolling'] }).done(function () {
            TTX.Common.writeLine("transport=" + $scope.connection.transport.name);
            $scope.invokeHubMethod = 'hubconnect';
            $scope.request = TTX.Common.prettyJsonObj($scope.user);
            $scope.proxy.invoke('hubconnect', $scope.user);

            $scope.user.connectionId = $scope.proxy.connection.id;

            //$scope.$apply();
        })
            .fail(function (e) {
                console.log(e);
            });

    }

    $scope.createTeam = function () {
        $scope.$apply($scope.response = '');
        $scope.invokeHubMethod = 'SaveTeam';
        $scope.request = TTX.Common.prettyJsonObj($scope.SaveTeam);
        $scope.response = null;
        if ($scope.createTeamData.name !== '') {
            // Call the Send method on the hub.
            $scope.proxy.invoke('SaveTeam', $scope.createTeamData).done(function () {
                $scope.startChatHub();
            }).fail(function (error) {
                console.log('Invocation failed. Error: ' + error);
            });
            //$scope.$apply();
        }
        else {
            alert('Invalid Team');
        }

    }

    $scope.joinTeam = function () {
        $scope.response = '';
        $scope.message.teamId = $scope.team.id;
        $scope.invokeHubMethod = 'joinTeam';
        $scope.request = TTX.Common.prettyJsonObj($scope.joinTeamRequest);
        $scope.response = null;
        if ($scope.joinTeamRequest.teamId > 0) {
            // Call the Send method on the hub.
            $scope.proxy.invoke('joinTeam', $scope.joinTeamRequest).done(function () {
                $scope.startChatHub();
            }).fail(function (error) {
                console.log('Invocation failed. Error: ' + error);
            });
            //$scope.$apply();
        }
        else {
            alert('Invalid Team');
        }
        $scope.$apply();

    }

    $scope.send = function () {
        $scope.response = '';
        $scope.message.teamId = $scope.team.id;
        $scope.invokeHubMethod = 'sendMessage';
        $scope.request = TTX.Common.prettyJsonObj($scope.message);
        $scope.response = null;
        if ($scope.message.teamId > 0 && $.trim($scope.message.message) !== '') {
            // Call the Send method on the hub.
            $scope.proxy.invoke('sendMessage', $scope.message).done(function () {
                $scope.message.message = '';
                $('#message').val('').focus();
            }).fail(function (error) {
                console.log('Invocation of NewContosoChatMessage failed. Error: ' + error);
            });
            //$scope.$apply();
        }
        else {
            alert('Invalid Team or empty message');
        }
        $scope.$apply();
    }

    $scope.remove = function () {
        $scope.response = '';
        $scope.message.teamId = $scope.team.id;
        $scope.invokeHubMethod = 'removeTeam';
        $scope.request = TTX.Common.prettyJsonObj($scope.getTeamRequest);
        $scope.response = null;
        if ($scope.message.teamId > 0) {
            // Call the Send method on the hub.
            $scope.proxy.invoke('removeTeam', $scope.getTeamRequest).done(function () {
                $scope.message.message = '';
                $('#message').val('').focus();
            }).fail(function (error) {
                console.log('Invocation of NewContosoChatMessage failed. Error: ' + error);
            });
            //$scope.$apply();
        }
        else {
            alert('Invalid Team or empty message');
        }
        $scope.$apply();
    }

    $scope.getInvitations = function () {
        $scope.response = '';
        var teamId = $scope.team.id;
        $scope.$apply($scope.invokeHubMethod = 'getTeamNotifications');
        $scope.$apply($scope.getTeamRequest.teamId = 0);
        $scope.$apply($scope.getTeamRequest.memberStatus = 1);
        $scope.request = TTX.Common.prettyJsonObj($scope.getTeamRequest);
        if (teamId > 0) {
            $scope.getTeamRequest.teamId = teamId;

            // Call the Send method on the hub.
            $scope.proxy.invoke('getTeamNotifications', $scope.getTeamRequest).done(function () {

            }).fail(function (error) {
                console.log('Invocation of NewContosoChatMessage failed. Error: ' + error);
            });;
            $scope.$apply();
        }
        else {
            alert('Invalid Team or empty message');
        }
        $scope.$apply();
    }

    $scope.getRequests = function () {
        var teamId = $scope.team.id;
        $scope.response = '';
        $scope.invokeHubMethod = 'getTeamNotifications';
        $scope.getTeamRequest.teamId = teamId;
        $scope.getTeamRequest.memberStatus = 0;
        $scope.request = TTX.Common.prettyJsonObj($scope.getTeamRequest);
        if (teamId > 0) {


            // Call the Send method on the hub.
            $scope.proxy.invoke('getTeamNotifications', $scope.getTeamRequest).done(function () {

            }).fail(function (error) {
                console.log('Invocation of NewContosoChatMessage failed. Error: ' + error);
            });;
            $scope.$apply();
        }
        else {
            alert('Invalid Team or empty message');
        }
        $scope.$apply();
    }

    $scope.seenTeamMessages = function () {
        var teamId = $scope.team.id;
        $scope.getTeamRequest.teamId = teamId;
        $scope.getTeamRequest.memberStatus = 0;

        //$scope.response = '';
        //$scope.invokeHubMethod = 'seenTeamMessages';
        //$scope.request = TTX.Common.prettyJsonObj($scope.getTeamRequest);
        if (teamId > 0) {


            // Call the Send method on the hub.
            $scope.proxy.invoke('seenTeamMessages', $scope.getTeamRequest).done(function () {

            }).fail(function (error) {
                console.log('Invocation of NewContosoChatMessage failed. Error: ' + error);
            });;
            $scope.$apply();
        }
        else {
            alert('Invalid Team or empty message');
        }
        $scope.$apply();
    }

    $scope.getTeam = function (teamId) {
        $scope.response = '';
        $scope.invokeHubMethod = 'getTeam';
        $scope.getTeamRequest.teamId = teamId;
        $scope.request = TTX.Common.prettyJsonObj($scope.getTeamRequest);
        if (teamId > 0) {


            // Call the Send method on the hub.
            $scope.proxy.invoke('getTeam', $scope.getTeamRequest).done(function () {

            }).fail(function (error) {
                console.log('Invocation of NewContosoChatMessage failed. Error: ' + error);
            });;
        }
        else {
            alert('Invalid Team or empty message');
        }
    }

    //$scope._startSession = function () {
    //    //viewModel.Username(username); // Set the selected username in the UI
    //    $scope.user.loading = true; // Turn on the loading indicator
    //    $scope.proxy.invoke('join', $scope.user.username);
    //    if (webrtcDetectedBrowser === null) {
    //        console.log('Your browser doesnt appear to support WebRTC.');
    //        $('.browser-warning').show();
    //    }
    //    // Ask the user for permissions to access the webcam and mic
    //    getUserMedia(
    //        {
    //            // Permissions to request
    //            video: true,
    //            audio: false
    //        },
    //        function (stream) { // succcess callback gives us a media stream
    //            $('.instructions').hide();

    //            // tell the viewmodel our conn id, so we can be treated like the special person we are.
    //            //viewModel.MyConnectionId(hub.connection.id);

    //            // Initialize our client signal manager, giving it a signaler (the SignalR hub) and some callbacks
    //            console.log('initializing connection manager');
    //            $scope.connectionManager.initialize($scope.proxy, $scope._callbacks.onReadyForStream, $scope._callbacks.onStreamAdded, $scope._callbacks.onStreamRemoved);

    //            // Store off the stream reference so we can share it later
    //            $scope._mediaStream = stream;

    //            // Load the stream into a video element so it starts playing in the UI
    //            console.log('playing my local video feed');
    //            var videoElement = document.querySelector('.video.mine');
    //            attachMediaStream(videoElement, $scope._mediaStream);

    //            // Hook up the UI

    //            $scope.user.loading = false;
    //        },
    //        function (error) { // error callback
    //            alertify.alert('<h4>Failed to get hardware access!</h4> Do you have another browser type open and using your cam/mic?<br/><br/>You were not connected to the server, because I didn\'t code to make browsers without media access work well. <br/><br/>Actual Error: ' + JSON.stringify(error));
    //            $scope.user.loading = false;
    //        }
    //    );
    //}

    //$scope._callbacks = {
    //    onReadyForStream: function (connection) {
    //        // The connection manager needs our stream
    //        // todo: not sure I like this
    //        connection.addStream($scope._mediaStream);
    //    },
    //    onStreamAdded: function (connection, event) {
    //        console.log('binding remote stream to the partner window');

    //        // Bind the remote stream to the partner window
    //        var otherVideo = document.querySelector('.video.partner');
    //        attachMediaStream(otherVideo, event.stream); // from adapter.js
    //    },
    //    onStreamRemoved: function (connection, streamId) {
    //        // todo: proper stream removal.  right now we are only set up for one-on-one which is why this works.
    //        console.log('removing remote stream from partner window');

    //        // Clear out the partner window
    //        var otherVideo = document.querySelector('.video.partner');
    //        otherVideo.src = '';
    //    }
    //};
    //$scope.call = function (callUser) {
    //    // Find the target user's SignalR client id
    //    $scope.target = callUser;
    //    var targetConnectionId = $scope.target.connectionId; //$(this).attr('data-cid');

    //    // Make sure we are in a state where we can make a call
    //    if ($scope.user.mode !== 'idle') {
    //        alertify.error('Sorry, you are already in a call.  Conferencing is not yet implemented.');
    //        return;
    //    }

    //    // Then make sure we aren't calling ourselves.
    //    if (targetConnectionId !== $scope.user.connectionId) {
    //        // Initiate a call
    //        //_hub.server.callUser(targetConnectionId);
    //        $scope.proxy.invoke('callUser', targetConnectionId);
    //        // UI in calling mode
    //        $scope.user.mode = 'calling';
    //    } else {
    //        alertify.error("Ah, nope.  Can't call yourself.");
    //    }
    //};
    //$scope.hangUp = function () {
    //    if ($scope.user.mode !== 'idle') {
    //        //_hub.server.hangUp();
    //        $scope.proxy.invoke('hangUp');

    //        $scope.connectionManager.closeAllConnections();
    //        $scope.user.mode = 'idle';
    //    }
    //}
    //$scope._setupHubCallbacks = function () {
    //    // Hub Callback: Incoming Call
    //    $scope.proxy.on('incomingCall', function (callingUser) {
    //        console.log('incoming call from: ' + JSON.stringify(callingUser));

    //        // Ask if we want to talk
    //        alertify.confirm(callingUser.Username + ' is calling.  Do you want to chat?', function (e) {
    //            if (e) {
    //                // I want to chat
    //                $scope.proxy.invoke('answerCall', true, callingUser.connectionId);

    //                // So lets go into call mode on the UI
    //                $scope.user.mode = 'incall';
    //            } else {
    //                // Go away, I don't want to chat with you
    //                $scope.proxy.invoke('answerCall', false, callingUser.connectionId);
    //            }
    //        });
    //    });

    //    // Hub Callback: Call Accepted
    //    $scope.proxy.on('callAccepted', function (acceptingUser) {
    //        console.log('call accepted from: ' + JSON.stringify(acceptingUser) + '.  Initiating WebRTC call and offering my stream up...');

    //        // Callee accepted our call, let's send them an offer with our video stream

    //        $scope.connectionManager.initiateOffer(acceptingUser.connectionId, $scope._mediaStream);



    //        // Set UI into call mode
    //        $scope.user.mode = 'incall';
    //    });

    //    // Hub Callback: Call Declined
    //    $scope.proxy.on('callDeclined', function (decliningConnectionId, reason) {
    //        console.log('call declined from: ' + decliningConnectionId);

    //        // Let the user know that the callee declined to talk
    //        alertify.error(reason);

    //        // Back to an idle UI
    //        $scope.user.mode = 'idle';
    //    });

    //    // Hub Callback: Call Ended
    //    $scope.proxy.on('callEnded', function (connectionId, reason) {
    //        console.log('call with ' + connectionId + ' has ended: ' + reason);

    //        // Let the user know why the server says the call is over
    //        alertify.error(reason);

    //        // Close the WebRTC connection
    //        $scope.connectionManager.closeConnection(connectionId);

    //        // Set the UI back into idle mode
    //        $scope.user.mode = 'idle';
    //    });

    //    // Hub Callback: Update User List
    //    $scope.proxy.on('updateUserList', function (userList) {
    //        //viewModel.setUsers(userList);
    //    });

    //    // Hub Callback: WebRTC Signal Received
    //    $scope.proxy.on('receiveSignal', function (callingUser, data) {
    //        $scope.connectionManager.newSignal(callingUser.connectionId, data);
    //    });
    //}
    if ($scope.authentication) {

        $scope.startChatHub();
        $scope.username = authService.authentication.username;
    }
    else {
        authService.referredUrl = $location.path();
        $location.path('/login');
    }

    $scope.$on("$destroy", function () {
        $scope.connection.stop();
    });

    $('#sel-teams').on('change', function (i, val) {
        var teamId = $scope.teams[$(this).val()].id;
        $scope.getTeam(teamId);
    })
    $scope.changeTeam = function (teamId) {
        $scope.getTeam(teamId);
    };

    $('#sel-other-teams').on('change', function (i, val) {
        var teamId = $scope.otherTeams[$(this).val()].id;
        $scope.joinTeamRequest.teamId = teamId;
    });

    $('#file-avatar').on('change', function (i, val) {
        var img = document.querySelector('#file-avatar').files[0];
        if (img !== null) {
            var reader = new FileReader();
            reader.readAsDataURL(img);
            reader.onload = function () {
                //$('#file-avatar').next('img').attr('src', reader.result);

                $scope.createTeamData.avatarUrl = reader.result;
                $scope.createTeamData.avatarFileStream.base64 = reader.result
                $scope.createTeamData.avatarFileStream.name = img.name;
                $scope.createTeamData.avatarFileStream.type = img.type;
                $scope.createTeamData.avatarFileStream.size = img.size;
                $scope.$apply();
            }
        }
    })
}]);