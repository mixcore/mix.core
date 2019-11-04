'use strict';

var app = angular.module('MixClient', ['angularCroppie','ngRoute', 'LocalStorageModule', 'ngFileUpload'
    , 'components', 'cart', 'ngSanitize']);
var serviceBase = '';
var modules = angular.module('components', []);
var cart = angular.module('cart', []);