'use strict';

var app = angular.module('MixClient', ['ngRoute', 'LocalStorageModule', 'components', 'ngSanitize']);
var serviceBase = '';

var modules = angular.module('components', []);