modules.component('starRating', {
    templateUrl: '/app/app-shared/components/star-rating/star-rating.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.translate = function (keyword) {
            return $rootScope.translate(keyword);
        };
        ctrl.readonly = true;
        ctrl.rateFunction = function (rating) {
            console.log('Rating selected: ' + rating);
        };
        ctrl.init = function () {
            ctrl.stars = [];
            ctrl.max = ctrl.max || 5;
            for (var i = 0; i < ctrl.max; i++) {
                ctrl.stars.push({
                    filled: i < ctrl.ratingValue
                });
            }
        };
        ctrl.toggle = function (index) {
            if (ctrl.readonly === undefined || ctrl.readonly === false) {
                ctrl.ratingValue = index + 1;                
            }
        };

    }],
    bindings: {
        ratingValue: '=',
        max: '=?', // optional (default is 5)
        onRatingSelect: '&?',
        isReadonly: '=?'
    }
});