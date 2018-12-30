modules.component('modalNavArticles', {
    templateUrl: '/app/app-portal/components/modal-nav-articles/modal-nav-articles.html',
    controller: [
        function(){
            var ctrl = this;
            ctrl.loadArticles = function(pageIndex){
                ctrl.load({ pageIndex: pageIndex });
            }
        }
        
    ],
    bindings: {
        srcId: '=',
        data: '=',        
        request: '=',        
        load: '&',
        save: '&'
    }
});