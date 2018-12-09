'use strict';
var app = angular.module('MixPortal', ['ngRoute', 'components', 'ngFileUpload', 'LocalStorageModule',
    'bw.paging', 'dndLists', 'ngTagsInput', 'ngSanitize']);
var modules = angular.module('components', []);


function Filter($filter) {
    return function (utcDateString, format) {
        // return if input date is null or undefined
        if (!utcDateString) {
            return;
        }

        // append 'Z' to the date string to indicate UTC time if the timezone isn't already specified
        if (utcDateString.indexOf('Z') === -1 && utcDateString.indexOf('+') === -1) {
            utcDateString += 'Z';
        }

        // convert and format date using the built in angularjs date filter
        return $filter('date')(utcDateString, format);
    };
}
