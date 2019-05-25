    'use strict';
    function PageDetailsController($scope, $element, $attrs) {
        var ctrl = this;
        ctrl.activedPage = null;
        ctrl.relatedPages = [];
        ctrl.data = [];
        ctrl.errors = [];
        ctrl.range = function (max) {
            var input = [];
            for (var i = 1; i <= max; i += 1) input.push(i);
            return input;
        };
        ctrl.loadPage = function (pageId) {
            ctrl.isBusy = true;
            var url = '/' + ctrl.currentLanguage + '/page/details/be/' + pageId;//byPage/' + pageId;
            ctrl.settings.method = "GET";
            ctrl.settings.url = url;// + '/true';
            ctrl.settings.data = ctrl.request;
            $.ajax(ctrl.settings).done(function (response) {
                if (response.isSucceed) {
                    ctrl.activedPage = response.data;
                    ctrl.initEditor();
                }
                ctrl.isBusy = false;
                ctrl.$apply();
            });
        };
        ctrl.loadPages = function (pageIndex) {
            ctrl.isBusy = true;
            if (pageIndex !== undefined) {
                ctrl.request.pageIndex = pageIndex;
            }
            if (ctrl.request.fromDate !== null) {
                ctrl.request.fromDate = ctrl.request.fromDate.toISOString();
            }
            if (ctrl.request.toDate !== null) {
                ctrl.request.toDate = ctrl.request.toDate.toISOString();
            }
            var url = '/' + ctrl.currentLanguage + '/page/list';//byPage/' + pageId;
            ctrl.settings.method = "POST";
            ctrl.settings.url = url;// + '/true';
            ctrl.settings.data = ctrl.request;
            $.ajax(ctrl.settings).done(function (response) {
                (ctrl.data = response.data);

                $.each(ctrl.data.items, function (i, page) {
                    $.each(ctrl.activedPages, function (i, e) {
                        if (e.pageId === page.id) {
                            page.isHidden = true;
                        }
                    })
                })
                ctrl.isBusy = false;
                setTimeout(function () {
                    $('[data-toggle="popover"]').popover({
                        html: true,
                        content: function () {
                            var content = $(this).next('.popover-body');
                            return $(content).html();
                        },
                        title: function () {
                            var title = $(this).attr("data-popover-content");
                            return $(title).children(".popover-heading").html();
                        }
                    });
                }, 200);
                ctrl.$apply();
            });
        };

        ctrl.removePage = function (pageId) {
            if (confirm("Are you sure!")) {
                var url = '/' + ctrl.currentLanguage + '/page/delete/' + pageId;
                $.ajax({
                    method: 'GET',
                    url: url,
                    success: function (data) {
                        ctrl.loadPages();
                        ctrl.$apply();
                    },
                    error: function (a, b, c) {
                        console.log(a + " " + b + " " + c);
                    }
                });
            }
        };
        ctrl.savePage = function (page) {
            var url = '/' + ctrl.currentLanguage + '/page/save';
            $.ajax({
                method: 'POST',
                url: url,
                data: page,
                success: function (data) {
                    //ctrl.loadPages();
                    if (data.isSucceed) {
                        alert('success');
                    }
                    else {
                        alert('failed! ' + data.errors);
                    }
                },
                error: function (a, b, c) {
                    console.log(a + " " + b + " " + c);
                }
            });
        };

        ctrl.changeMedia = function (media) {
            var currentItem = null;
            if (ctrl.activedPage.mediaNavs === undefined) {
                ctrl.activedPage.mediaNavs = [];
            }
            $.each(ctrl.activedPage.mediaNavs, function (i, e) {
                if (e.mediaId === media.id) {
                    e.isActived = media.isActived;
                    currentItem = e;
                    return false;
                }
            });
            if (currentItem === null) {
                currentItem = {
                    description: media.description !== 'undefined' ? media.description : '',
                    image: media.fullPath,
                    mediaId: media.id,
                    page: ctrl.activedPage.id,
                    specificulture: media.specificulture,
                    position: 0,
                    priority: ctrl.activedMedias.length + 1,
                    isActived: true
                };
                media.isHidden = true;
                ctrl.activedPage.mediaNavs.push(currentItem);
            }
        }

        ctrl.changePage = function (page) {
            var currentItem = null;
            $.each(ctrl.activedPage.pageNavs, function (i, e) {
                if (e.relatedPageId === page.id) {
                    e.isActived = page.isActived;
                    currentItem = e;
                    return false;
                }
            });
            if (currentItem === null) {
                currentItem = {
                    relatedPageId: page.id,
                    sourcePageId: $('#page-id').val(),
                    specificulture: page.specificulture,
                    priority: ctrl.activedPage.pageNavs.length + 1,
                    page: page,
                    isActived: true
                };
                page.isHidden = true;
                ctrl.activedPage.pageNavs.push(currentItem);
            }
        }


        ctrl.addProperty = function (type) {
            var i = $(".property").length;
            $.ajax({
                method: 'GET',
                url: '/' + ctrl.currentLanguage + '/Portal/' + type + '/AddEmptyProperty/' + i,
                success: function (data) {
                    $('#tbl-properties > tbody').append(data);
                    $(data).find('.prop-data-type').trigger('change');
                },
                error: function (a, b, c) {
                    console.log(a + " " + b + " " + c);
                }
            });
            ctrl.updateHero = function (hero, prop, value) {
                hero[prop] = value;
            };

            ctrl.deleteHero = function (hero) {
                var idx = ctrl.list.indexOf(hero);
                if (idx >= 0) {
                    ctrl.list.splice(idx, 1);
                }
            };
        }

        angular.module(appName).component('pageDetails', {
            templateUrl: 'pageDetails.html',
            controller: PageDetailsController
        });
    }


