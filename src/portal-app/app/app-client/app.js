'use strict';

var app = angular.module('MixClient', ['ngRoute', 'LocalStorageModule', 'components', 'cart', 'ngSanitize']);
var serviceBase = '';
var modules = angular.module('components', []);
var cart = angular.module('cart', []);