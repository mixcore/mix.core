'use strict';
var app = angular.module('MessengerApp', ['LocalStorageModule']);
var serviceBase = "http://chat.demo.smileway.co/";//'https://microsoft-apiapp0b3793bb8b0045e28d315e567a5c882c.azurewebsites.net/';//"http://localhost:52615/";//'http://txt.ogilvy.com.vn/';//
app.constant('ngAppSettings', {
    apiServiceBaseUri: '/',
    clientId: 'ngAuthApp',
    facebookAppId: '464285300363325'
});
app.filter('utcToLocal', Filter);
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
app.animation('.view', function () {
    return {
        enter: function (element, done) {
            TweenLite.from(element[0], 1, {
                opacity: 0,
                onComplete: done
            });
        },
        leave: function (element, done) {
            TweenLite.set(element[0], {
                position: 'absolute',
                top: 0
            });
            TweenLite.to(element[0], 0.2, {
                opacity: 0,
                onComplete: done
            });
        }
    };
});

$(document).ready(function () {
    if (!$('.page').hasClass('page-active')) {
        $('.navbar, .jumbotron').show();
    }
});
