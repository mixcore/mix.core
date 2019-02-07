/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    //fontmin = require("gulp-fontmin"),
    htmlmin = require("gulp-htmlmin"),

    uglify = require("gulp-uglify");

var uglifyjs = require('uglify-es'); // can be a git checkout
// or another module (such as `uglify-es` for ES6 support)
var composer = require('gulp-uglify/composer');
var pump = require('pump');

var minify = composer(uglifyjs, console);
var dest = '.';//For publish folder use "./bin/Release/PublishOutput/";
//C:\\Git\\GitHub\\Queen-Beauty\\QueenBeauty\\
var paths = {
    webroot: "./wwwroot/",///wwwroot
    webapp: "./app/",///app
    scriptLib: "./lib/js/",///app
    styleLib: "./lib/css/",///app
    jsObtions: {},
    htmlOptions: { collapseWhitespace: false },
    cssOptions: {}, //showLog : (True, false) to trun on or off of the log
    fontOptions: { fontPath: dest + '/wwwroot/fonts' }
};
var browserSync = require('browser-sync').create();
paths.appPortal = {
    src: [
        paths.webapp + "app-portal/app.js",
        paths.webapp + "app-portal/app-portal-controller.js",
        paths.webapp + "app-portal/app.route.js",
        paths.webapp + "app-portal/demo.js",
        paths.webapp + "app-portal/pages/**/*.js",
        paths.webapp + "app-portal/components/**/*.js"
    ],
    dest: paths.webroot + "js/app-portal.min.js"
};

paths.initApp = {
    src: [
        paths.webapp + "app-init/app.js",
        paths.webapp + "app-init/app.route.js",
        paths.webapp + "app-init/pages/**/*.js"
    ],
    dest: paths.webroot + "js/app-init.min.js"
};

paths.clientApp = {
    src: [
        paths.webapp + "app-client/app.js",
        paths.webapp + "app-client/app-client-controller.js",
        paths.webapp + "app-client/components/**/*.js"
    ],
    dest: paths.webroot + "js/app-client.min.js"
};

paths.sharedApp = {
    src: [
        paths.webapp + "app-shared/**/*.js",
        paths.webapp + "app-shared/**/*.*.js"
    ],
    dest: paths.webroot + "js/app-shared.min.js"
};

paths.framework = {
    src: [
        paths.scriptLib + "angularjs/angular.min.js",
        paths.scriptLib + "angularjs/angular-route.min.js",
        paths.scriptLib + "angularjs/angular-animate.min.js",
        paths.scriptLib + "angularjs/angular-sanitize.min.js"
    ],
    dest: paths.webroot + "js/framework.min.js"
};
paths.shared = {
    src: [
        paths.scriptLib + "shared/**/*.js",
        paths.scriptLib + "shared/**/*.*.js"
    ],
    dest: paths.webroot + "js/shared.min.js"
};
paths.portal = {
    src: [
        paths.scriptLib + "portal/**/*.js",
        paths.scriptLib + "portal/**/*.*.js"
    ],
    dest: paths.webroot + "js/portal.min.js"
};

paths.appCss = {
    src: [
        paths.webapp + "app-shared/**/*.css",
        paths.webapp + "app-portal/**/*.css",
        paths.webapp + "app-client/**/*.css",
        paths.webapp + "app-init/**/*.css"
    ],
    dest: paths.webroot + "css/app-vendor.min.css"
};
paths.portalCss = {
    src: [
        "./lib/css/portal/**/*.css",
        "./lib/css/portal/**/*.*.css",
        "./lib/js/portal/**/*.css",
        "./lib/js/portal/**/*.*.css"
    ],
    dest: paths.webroot + "css/portal.min.css"
};
paths.sharedCss = {
    src: [
        "./lib/css/shared/**/*.css",
        "./lib/css/shared/**/*.*.css"
    ],
    dest: paths.webroot + "css/shared.min.css"
};

paths.views = {
    src: [
        "./app/app-shared/**/*.html",
        "./app/app-portal/**/*.html",
        "./app/app-client/**/*.html",
        "./app/app-init/**/*.html"
    ],
    dest: './wwwroot/'
};

gulp.task("min:portalApp", function (cb) {
    return gulp.src(paths.appPortal.src, { base: "." })
        .pipe(concat(paths.appPortal.dest))
        // .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});

gulp.task("min:initApp", function (cb) {
    return gulp.src(paths.initApp.src, { base: "." })
        .pipe(concat(paths.initApp.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});

gulp.task("min:clientApp", function (cb) {
    return gulp.src(paths.clientApp.src, { base: "." })
        .pipe(concat(paths.clientApp.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});

gulp.task("min:sharedApp", function (cb) {
    return gulp.src(paths.sharedApp.src, { base: "." })
        .pipe(concat(paths.sharedApp.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});
gulp.task("min:framework", function (cb) {
    return gulp.src(paths.framework.src, { base: "." })
        .pipe(concat(paths.framework.dest))
        .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});
gulp.task("min:shared", function (cb) {
    return gulp.src(paths.shared.src, { base: "." })
        .pipe(concat(paths.shared.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});
gulp.task("min:portal", function (cb) {
    return gulp.src(paths.portal.src, { base: "." })
        .pipe(concat(paths.portal.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});
gulp.task("min:appCss", function (cb) {
    return gulp.src(paths.appCss.src, { base: "." })
        .pipe(concat(paths.appCss.dest))
        .pipe(cssmin(paths.appCssOptions))
        .pipe(gulp.dest(dest));
});
gulp.task("min:portalCss", function (cb) {
    return gulp.src(paths.portalCss.src, { base: "." })
        .pipe(concat(paths.portalCss.dest))
        .pipe(cssmin(paths.appCssOptions))
        .pipe(gulp.dest(dest));
});
gulp.task("min:sharedCss", function (cb) {
    return gulp.src(paths.sharedCss.src, { base: "." })
        .pipe(concat(paths.sharedCss.dest))
        .pipe(cssmin(paths.appCssOptions))
        .pipe(gulp.dest(dest));
});

gulp.task("min:views", function (cb) {
    return gulp.src(paths.views.src, { base: "." })
        .pipe(htmlmin(paths.htmlOptions))
        .pipe(gulp.dest(paths.views.dest));
});

gulp.task("clean:shared", function (cb) {
    rimraf(paths.shared.dest, cb);
});

gulp.task("clean:framework", function (cb) {
    rimraf(paths.framework.dest, cb);
});
gulp.task("clean:portal", function (cb) {
    rimraf(paths.portal.dest, cb);
});
gulp.task("clean:portalApp", function (cb) {
    rimraf(paths.appPortal.dest, cb);
});

gulp.task("clean:clientApp", function (cb) {
    rimraf(paths.clientApp.dest, cb);
});

gulp.task("clean:sharedApp", function (cb) {
    rimraf(paths.sharedApp.dest, cb);
});

gulp.task("clean:initApp", function (cb) {
    rimraf(paths.initApp.dest, cb);
});

gulp.task("clean:appCss", function (cb) {
    rimraf(paths.appCss.dest, cb);
});

gulp.task("clean:portalCss", function (cb) {
    rimraf(paths.portalCss.dest, cb);
});
gulp.task("clean:sharedCss", function (cb) {
    rimraf(paths.sharedCss.dest, cb);
});

gulp.task("clean:js", [

    "clean:framework", "clean:portal", "clean:shared"
    , "clean:portalApp", "clean:clientApp", "clean:sharedApp", "clean:initApp"
]);
gulp.task("clean:css", [
    , "clean:appCss", "clean:portalCss", "clean:sharedCss"
]);
gulp.task("min:js", ["min:portalApp", "min:initApp", "min:clientApp", "min:sharedApp"
    , "min:shared", "min:portal", "min:framework"
]);
gulp.task("min:css", ['min:appCss', 'min:portalCss', 'min:sharedCss']);

gulp.task("build", ['clean:js',
    'min:js', 'min:css', "min:views"]);//["clean", "min:views", "min"]);

gulp.task('watch:html', function () {
    gulp.watch('./app/**/**/*.html', ['min:views']);
    gulp.watch('./app/**/**/*.js', ['min:portalApp']);
    gulp.watch('./app/**/**/*.css', ['min:appCss']);
});

// [Watch Portal] View & Portal's js & CSS > gulp watch:html

gulp.task('portalView-watch', ['min:views'], function (done) {
    browserSync.reload();
    done();
});
gulp.task('portalJS-watch', ['clean:js', 'min:js'], function (done) {
    browserSync.reload();
    done();
});
gulp.task('portalCSS-watch', ['min:css'], function (done) {
    browserSync.reload();
    done();
});

gulp.task('serve', function () {
    browserSync.init({
        proxy: "https://localhost:5001/"
    });
    gulp.watch('./app/**/**/*.html', ['portalView-watch']);
    gulp.watch('./app/**/**/*.js', ['portalJS-watch']);
    gulp.watch('./app/**/**/*.css', ['portalCSS-watch']);
});