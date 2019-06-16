$(document).ready(function () {

    // const container = document.querySelector('.perfect-scroll');
    // const ps = new PerfectScrollbar(container);
    
    // or just with selector string
    // const ps = new PerfectScrollbar('main');

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

    $('[data-toggle="tooltip"]').tooltip({
        delay: { "show": 500, "hide": 100 }
    });

    $("#sw-docs-nav li a").click(function () {
        $(this).children("span.mi").toggleClass('mi mi-ChevronRightSmall').toggleClass('mi mi-ChevronDownSmall');
    });

    // $(".sortable").sortable({
    //     revert: true,
    //     update: function (event, ui) {
    //         //create the array that hold the positions...
    //         var order = [];
    //         //loop trought each li...
    //         $('.sortable .sortable-item').each(function (i, e) {
    //             //add each li position to the array...
    //             // the +1 is for make it start from 1 instead of 0
    //             //order.push($(this).attr('id') + '=' + ($(this).index() + 1));
    //             $(e).find('.item-priority').val($(e).index() + 1);
    //             //alert($(this).attr('module-priority'));

    //             var model = $(e).attr('sort-model');
    //             var modelId = $(e).attr('sort-model-id');
    //             if (model !== undefined && modelId !== undefined) {
    //                 var data = [{
    //                     "propertyName": "Priority",
    //                     "propertyValue": $(e).index() + 1
    //                 }];
    //                 var settings = {
    //                     "async": true,
    //                     "crossDomain": true,
    //                     "url": "/api/vi-vn/" + model + "/save/" + modelId,
    //                     "method": "POST",
    //                     "headers": {
    //                         "Content-Type": "application/json"
    //                     },
    //                     "processData": false,
    //                     "data": JSON.stringify(data)
    //                 }

    //                 $.ajax(settings).done(function (response) {
    //                     console.log(response);
    //                 });
    //             }
    //         });

    //         // join the array as single variable...
    //         var positions = order.join(';')
    //         //use the variable as you need!
    //         //alert(positions);
    //         // $.cookie( LI_POSITION , positions , { expires: 10 });
    //     }
    // });
    // $(".draggable").draggable({
    //     cursor: "move",
    //     cursorAt: { top: 56, left: 56 },
    //     connectToSortable: ".sortable",
    //     helper: "clone",
    //     revert: "invalid"
    // });
    // $(".draggable-header").draggable({
    //     cursor: "move",
    //     cursorAt: { top: 56, left: 56 },
    //     connectToSortable: ".sortable",
    //     handle: ".card-header",
    //     helper: "clone",
    //     revert: "invalid"
    // });
    // $("ul, li").disableSelection();

    // $(window).on("scroll",function(){
    //     var wn = $(window).scrollTop();
    //   if(wn > 120){
    //       $("header").css("background","rgba(0, 0, 0, 0.88)");
    //   }
    //   else{
    //       $("header").css("background","rgba(0, 0, 0, 0.5)");
    //   }
    // });
});