
modules.component('goToTop', {
    templateUrl: '/app/app-shared/components/go-to-top/view.html',
    controller: ['$rootScope', '$scope',
        function ($rootScope, $scope) {
            var ctrl = this;
            ctrl.$onInit = function(){
                $(window).scroll(function() {
                    if ($(window).scrollTop() >= 50) {        // If page is scrolled more than 50px
                        $('#return-to-top').fadeIn(200);    // Fade in the arrow
                    } else {
                        $('#return-to-top').fadeOut(200);   // Else fade out the arrow
                    }
                });
            };
           ctrl.goToTop = function(){
               // ===== Scroll to Top ==== 
                $('body,html').animate({
                    scrollTop : 0                       // Scroll to top of body
                }, 500);
           };
        }],
    bindings: {
    }
});