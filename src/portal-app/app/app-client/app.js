'use strict';

var app = angular.module('MixClient', ['ngRoute', 'LocalStorageModule', 'components', 'cart', 'ngSanitize']);
var serviceBase = '';
var modules = angular.module('components', []);
var cart = angular.module('cart', []);

// This is the "Offline page" service worker
// Add this below content to your HTML page, or add the js file to your page at the very top to register service worker
// Check compatibility for the browser we're running this in
if ("serviceWorker" in navigator) {
    if (navigator.serviceWorker.controller) {
        console.log("[PWA Builder] active service worker found, no need to register");
    } else {
        // Register the service worker
        navigator.serviceWorker
            .register("service-worker.js", {
                scope: "./"
            })
            .then(function (reg) {
                console.log("[PWA Builder] Service worker has been registered for scope: " + reg.scope);
            });
    }
}
