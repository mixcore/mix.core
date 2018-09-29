/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    fontmin = require("gulp-fontmin"),
    htmlmin = require("gulp-htmlmin"),

    uglify = require("gulp-uglify");

var uglifyjs = require('uglify-es'); // can be a git checkout
// or another module (such as `uglify-es` for ES6 support)
var composer = require('gulp-uglify/composer');
var pump = require('pump');

var minify = composer(uglifyjs, console);
var dest = './wwwroot';//For publish folder use "./bin/Release/PublishOutput/"; 
//C:\\Git\\GitHub\\Queen-Beauty\\QueenBeauty\\
var paths = {
    webroot: './app/',
    jsObtions: {},
    htmlOptions: { collapseWhitespace: false },
    cssOptions: {}, //showLog : (True, false) to trun on or off of the log
    fontOptions: { fontPath: dest + '/wwwroot/fonts' }
};

paths.views = {
    src: [
        paths.webroot + "app-shared/**/*.html",
        paths.webroot + "app-portal/**/*.html",
        paths.webroot + "app-client/**/*.html",
        paths.webroot + "app-init/**/*.html"
    ],
    dest: paths.webroot + "html/*.html"
};
paths.fonts = {
    src: [
        paths.webroot + "lib/micon/fonts/*.*"
    ],
    dest: paths.webroot + dest + "/fonts"
};
paths.css = {
    src: [
        paths.webroot + "app-shared/**/*.css",
        paths.webroot + "app-portal/**/*.css",
        paths.webroot + "app-client/**/*.css",
        paths.webroot + "app-init/**/*.css"
    ],
    dest: paths.webroot + "css/vendor.min.css"
};
paths.plugins = {
    src: [
        paths.webroot + "lib/Trumbowyg-2.9.0/**/*.min.js",
        paths.webroot + "lib/ace/src/ace.js",
        paths.webroot + "lib/ace/src/mode-csharp.js",
        paths.webroot + "lib/ace/src/mode-razor.js",
        paths.webroot + "lib/ace/src/mode-json.js",
        paths.webroot + "lib/ace/src/mode-css.js",
        paths.webroot + "lib/ace/src/mode-javascript.js",
        paths.webroot + "lib/ace/src/mode-html.js",
        paths.webroot + "lib/ace/src/theme-chrome.js",
        paths.webroot + "lib/ace/src/theme-clouds_midnight.js",
        paths.webroot + "lib/ace/src/worker-json.js",
    ],
    dest: paths.webroot + "js/vendor.min.js"
};
paths.portal = {
    src: [
        paths.webroot + "app-portal/pages/**/*.js"
    ],
    dest: paths.webroot + "js/app-portal.min.js"
};

paths.portalApp = {
    src: [
        paths.webroot + "app-portal/shared/**/*.js",
        paths.webroot + "app-portal/app.js",
        paths.webroot + "app-portal/app.filter.js",
        paths.webroot + "app-portal/app.directive.js",
        paths.webroot + "app-portal/app.route.js",
        paths.webroot + "app-portal/demo.js"
    ]
};

paths.init = {
    src: [
        paths.webroot + "app-init/pages/**/*.js"
    ],
    dest: paths.webroot + "js/app-init.min.js"
};

paths.initApp = {
    src: [
        paths.webroot + "app-init/app.js",
        paths.webroot + "app-init/app.route.js"
    ]
};


paths.client = {
    src: [
        paths.webroot + "app-client/components/**/*.js"
    ],
    dest: paths.webroot + "js/app-client.min.js"
};

paths.clientApp = {
    src: [
        paths.webroot + "app-client/shared/**/*.js",
        paths.webroot + "app-client/app.js",
        paths.webroot + "app-client/app.constant.js",
        paths.webroot + "app-portal/app.route.js"
    ]
};

paths.sharedJs = {
    src: [
        paths.webroot + "app-shared/**/*.js",
        paths.webroot + "app-shared/**/*.*.js"
    ],
    dest: paths.webroot + "js/app-shared.min.js"
};



gulp.task("clean:js", function (cb) {
    rimraf(paths.portal.dest, cb);
});

gulp.task("clean:clientJs", function (cb) {
    rimraf(paths.client.dest, cb);
});

gulp.task("clean:sharedJs", function (cb) {
    rimraf(paths.sharedJs.dest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.css.dest, cb);
});

gulp.task("clean", ["clean:js", "clean:clientJs", "clean:sharedJs", "clean:css"]);

gulp.task("min:plugins", function (cb) {
    return gulp.src(paths.plugins.src, { base: "." })
        .pipe(concat(paths.plugins.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:portal", function (cb) {
    return gulp.src(paths.portal.src, { base: "." })
        .pipe(concat(paths.portal.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:portalApp", function (cb) {
    return gulp.src(paths.portalApp.src, { base: "." })
        //.pipe(concat(paths.portal.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:init", function (cb) {
    return gulp.src(paths.init.src, { base: "." })
        .pipe(concat(paths.init.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:initApp", function (cb) {
    return gulp.src(paths.initApp.src, { base: "." })
        //.pipe(concat(paths.portal.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:views", function (cb) {
    return gulp.src(paths.views.src, { base: "." })
        .pipe(htmlmin(paths.htmlOptions))
        .pipe(gulp.dest(dest));

});
gulp.task("min:fonts", function (cb) {
    return gulp.src(paths.fonts.src, { base: "." })
        .pipe(fontmin(paths.fontOptions))
        .pipe(gulp.dest(paths.fonts.dest));

});

gulp.task("min:clientApp", function (cb) {
    return gulp.src(paths.clientApp.src, { base: "." })
        //.pipe(concat(paths.portal.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});
gulp.task("min:clientJs", function (cb) {
    return gulp.src(paths.client.src, { base: "." })
        .pipe(concat(paths.client.dest))
        .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest+'/js'));

});

gulp.task("min:sharedJs", function (cb) {
    return gulp.src(paths.sharedJs.src, { base: "." })
        .pipe(concat(paths.sharedJs.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:css", function (cb) {
    return gulp.src(paths.css.src, { base: "." })
        .pipe(concat(paths.css.dest))
        .pipe(cssmin(paths.cssOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min", ["min:plugins", "min:portal", "min:portalApp", "min:init", "min:initApp"
    , "min:clientApp", "min:clientJs", "min:sharedJs", "min:css", "min:fonts"]);
gulp.task("build", ["min", "min:views"]);