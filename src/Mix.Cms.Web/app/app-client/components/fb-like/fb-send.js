
modules.component('fbLike', {
    templateUrl: '/app/app-client/components/fb-like/fb-like.html',
    controller: ['$location', function ($location) {
        var ctrl = this;
        ctrl.href = ctrl.href || window.top.location.href;        
        ctrl.layout = ctrl.layout || 'standard';        
        ctrl.size = ctrl.size || 'small';        
        ctrl.showFaces = ctrl.showFaces || true;    
        this.$onInit = function(){
            setTimeout(() => {
                FB.XFBML.parse();
            }, 200);
        }    
    }],
    bindings: {
        href:'=',
        layout:'=',
        size:'=',
        showFaces:'='
    }
});