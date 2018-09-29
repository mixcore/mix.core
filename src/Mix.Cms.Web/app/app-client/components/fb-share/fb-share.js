
modules.component('fbShare', {
    templateUrl: '/app-client/components/fb-share/fb-share.html',
    controller: ['$location', function ($location) {
        var ctrl = this;
        ctrl.href = ctrl.href || window.top.location.href;
        ctrl.share = function () {
            var href = window.top.location.href;
            FB.ui({
                method: 'share',
                href: href,
            }, function (response) { });
        };
    }],
    bindings: {
        href:'='
    }
});