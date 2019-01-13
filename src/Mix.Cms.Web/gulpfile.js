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
var dest = '.';//For publish folder use "./bin/Release/PublishOutput/"; 
//C:\\Git\\GitHub\\Queen-Beauty\\QueenBeauty\\
var paths = {
    webroot: "./wwwroot/",///wwwroot
    webapp: "./app/",///app
    jsObtions: {},
    htmlOptions: { collapseWhitespace: false },
    cssOptions: {}, //showLog : (True, false) to trun on or off of the log
    fontOptions: { fontPath: dest + '/wwwroot/fonts' }
};
var browserSync = require('browser-sync').create();
paths.portal = {
    src: [
        paths.webapp + "app-portal/app.js",
        paths.webapp + "app-portal/app-portal-controller.js",
        paths.webapp + "app-portal/app.directive.js",
        paths.webapp + "app-portal/app.filter.js",
        paths.webapp + "app-portal/app.route.js",
        paths.webapp + "app-portal/demo.js",
        paths.webapp + "app-portal/pages/**/*.js",
        paths.webapp + "app-portal/components/**/*.js"
    ],
    dest: paths.webroot + "js/app-portal.min.js"
};

paths.portalApp = {
    src: [
        paths.webapp + "app/app-portal/app.js",
        paths.webapp + "app/app-portal/app-portal-controller.js",
        paths.webapp + "app/app-portal/app.directive.js",
        paths.webapp + "app/app-portal/app.filter.js",
        paths.webapp + "app/app-portal/app.route.js",
        paths.webapp + "app/app-portal/demo.js",
        paths.webapp + "app/app-portal/pages/**/*.js",
        paths.webapp + "app/app-portal/components/**/*.js"
    ]
};

paths.init = {
    src: [
        paths.webapp + "app-init/pages/**/*.js"
    ],
    dest: paths.webroot + "js/app-init.min.js"
};

paths.initApp = {
    src: [
        paths.webapp + "app-init/app.js",
        paths.webapp + "app-init/app.route.js"
    ]
};


paths.client = {
    src: [
        paths.webapp + "app-client/components/**/*.js"
    ],
    dest: paths.webroot + "js/app-client.min.js"
};

paths.clientApp = {
    src: [
        paths.webapp + "app-client/shared/**/*.js",
        paths.webapp + "app-client/app.js",
        paths.webapp + "app-client/app.constant.js",
        paths.webapp + "app-portal/app.route.js"
    ]
};

paths.sharedJs = {
    src: [
        paths.webapp + "app-shared/**/*.js",
        paths.webapp + "app-shared/**/*.*.js"
    ],
    dest: paths.webroot + "js/app-shared.min.js"
};

paths.css = {
    src: [
        paths.webapp + "app-shared/**/*.css",
        paths.webapp + "app-portal/**/*.css",
        paths.webapp + "app-client/**/*.css",
        paths.webapp + "app-init/**/*.css"
    ],
    dest: paths.webroot + "css/vendor.min.css"
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

gulp.task("min:portal", function (cb) {    
    return gulp.src(paths.portal.src, { base: "." })
        .pipe(concat(paths.portal.dest))
        // .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:portalApp", function (cb) {
    return gulp.src(paths.portalApp.src, { base: "." })
        .pipe(concat(paths.portal.dest))
        // .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:init", function (cb) {
    return gulp.src(paths.init.src, { base: "." })
        .pipe(concat(paths.init.dest))
        .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:initApp", function (cb) {
    return gulp.src(paths.initApp.src, { base: "." })
        .pipe(concat(paths.portal.dest))
        .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:clientApp", function (cb) {
    return gulp.src(paths.clientApp.src, { base: "." })
        .pipe(concat(paths.portal.dest))
        .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});
gulp.task("min:clientJs", function (cb) {
    return gulp.src(paths.client.src, { base: "." })
        .pipe(concat(paths.client.dest))
        .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:sharedJs", function (cb) {
    return gulp.src(paths.sharedJs.src, { base: "." })
        .pipe(concat(paths.sharedJs.dest))
        .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});
gulp.task("min:css", function (cb) {
    return gulp.src(paths.css.src, { base: "." })
        .pipe(concat(paths.css.dest))
        .pipe(cssmin(paths.cssOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:views", function (cb) {
    return gulp.src(paths.views.src, { base: "." })
        .pipe(htmlmin(paths.htmlOptions))
        .pipe(gulp.dest(paths.views.dest));
});


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
gulp.task("min", ["min:portal", "min:portalApp", "min:init", "min:initApp"
    , "min:clientApp", "min:clientJs", "min:sharedJs", "min:css"]);

gulp.task("build", ["min:views"]);//["clean", "min:views", "min"]);

gulp.task('watch:html', function () {  
    gulp.watch('./app/**/**/*.html', ['min:views']);
    gulp.watch('./app/**/**/*.js', ['min:portal']);
    gulp.watch('./app/**/**/*.css', ['min:css']);
});

// [Watch Portal] View & Portal's js & CSS > gulp watch:html 

gulp.task('portalView-watch', ['min:views'], function (done) {
    browserSync.reload();
    done();
});
gulp.task('portalJS-watch', ['min:portal'], function (done) {
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