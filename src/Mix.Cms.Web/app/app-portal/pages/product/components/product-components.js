
    'use strict';
    function ProductDetailsController($scope, $element, $attrs) {
        var ctrl = this;
        ctrl.activedData = null;
        ctrl.relatedProducts = [];
        ctrl.data = [];
        ctrl.errors = [];
        ctrl.range = function (max) {
            var input = [];
            for (var i = 1; i <= max; i += 1) input.push(i);
            return input;
        };
        ctrl.loadProduct = function (productId) {
            ctrl.isBusy = true;
            var url = '/' + ctrl.currentLanguage + '/product/details/be/' + productId;//byProduct/' + productId;
            ctrl.settings.method = "GET";
            ctrl.settings.url = url;// + '/true';
            ctrl.settings.data = ctrl.request;
            $.ajax(ctrl.settings).done(function (response) {
                if (response.isSucceed) {
                    ctrl.activedData = response.data;
                    ctrl.initEditor();
                }
                ctrl.isBusy = false;
                ctrl.$apply();
            });
        };
        ctrl.loadProducts = function (pageIndex) {
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
            var url = '/' + ctrl.currentLanguage + '/product/list';//byProduct/' + productId;
            ctrl.settings.method = "POST";
            ctrl.settings.url = url;// + '/true';
            ctrl.settings.data = ctrl.request;
            $.ajax(ctrl.settings).done(function (response) {
                (ctrl.data = response.data);

                $.each(ctrl.data.items, function (i, product) {
                    $.each(ctrl.activedDatas, function (i, e) {
                        if (e.productId === product.id) {
                            product.isHidden = true;
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

        ctrl.removeProduct = function (productId) {
            if (confirm("Are you sure!")) {
                var url = '/' + ctrl.currentLanguage + '/product/delete/' + productId;
                $.ajax({
                    method: 'GET',
                    url: url,
                    success: function (data) {
                        ctrl.loadProducts();
                        ctrl.$apply();
                    },
                    error: function (a, b, c) {
                        console.log(a + " " + b + " " + c);
                    }
                });
            }
        };
        ctrl.saveProduct = function (product) {
            var url = '/' + ctrl.currentLanguage + '/product/save';
            $.ajax({
                method: 'POST',
                url: url,
                data: product,
                success: function (data) {
                    //ctrl.loadProducts();
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
            if (ctrl.activedData.mediaNavs === undefined) {
                ctrl.activedData.mediaNavs = [];
            }
            $.each(ctrl.activedData.mediaNavs, function (i, e) {
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
                    product: ctrl.activedData.id,
                    specificulture: media.specificulture,
                    position: 0,
                    priority: ctrl.activedMedias.length + 1,
                    isActived: true
                };
                media.isHidden = true;
                ctrl.activedData.mediaNavs.push(currentItem);
            }
        }

        ctrl.changeProduct = function (product) {
            var currentItem = null;
            $.each(ctrl.activedData.productNavs, function (i, e) {
                if (e.relatedProductId === product.id) {
                    e.isActived = product.isActived;
                    currentItem = e;
                    return false;
                }
            });
            if (currentItem === null) {
                currentItem = {
                    relatedProductId: product.id,
                    sourceProductId: $('#product-id').val(),
                    specificulture: product.specificulture,
                    priority: ctrl.activedData.productNavs.length + 1,
                    product: product,
                    isActived: true
                };
                product.isHidden = true;
                ctrl.activedData.productNavs.push(currentItem);
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

        angular.module(appName).component('productDetails', {
            templateUrl: 'productDetails.html',
            controller: ProductDetailsController
        });
    }

