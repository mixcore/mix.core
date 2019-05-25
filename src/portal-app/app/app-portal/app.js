'use strict';
var app = angular.module('MixPortal', ['ngRoute', 'components', 'ngFileUpload', 'LocalStorageModule',
    'bw.paging', 'dndLists', 'ngTagsInput', 'ngSanitize']);
var modules = angular.module('components', []);