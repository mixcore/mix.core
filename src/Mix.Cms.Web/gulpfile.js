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
        paths.webapp + "app-portal/app.route.js",
        paths.webapp + "app-portal/demo.js",
        paths.webapp + "app-portal/pages/**/*.js",
        paths.webapp + "app-portal/components/**/*.js"
    ],
    dest: paths.webroot + "js/app-portal.min.js"
};

paths.init = {
    src: [
        paths.webapp + "app-init/pages/**/*.js"
    ],
    dest: paths.webroot + "js/app-init.min.js"
};

paths.client = {
    src: [
        paths.webapp + "app-client/app.js",
        paths.webapp + "app-client/components/**/*.js"
    ],
    dest: paths.webroot + "js/app-client.min.js"
};

paths.shared = {
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

gulp.task("min:init", function (cb) {
    return gulp.src(paths.init.src, { base: "." })
        .pipe(concat(paths.init.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:client", function (cb) {
    return gulp.src(paths.client.src, { base: "." })
        .pipe(concat(paths.client.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));

});

gulp.task("min:shared", function (cb) {
    return gulp.src(paths.shared.src, { base: "." })
        .pipe(concat(paths.shared.dest))
        //.pipe(minify(paths.jsOptions))
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

gulp.task("clean:client", function (cb) {
    rimraf(paths.client.dest, cb);
});

gulp.task("clean:shared", function (cb) {
    rimraf(paths.shared.dest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.css.dest, cb);
});

gulp.task("clean", ["clean:js", "clean:client", "clean:shared", "clean:css"]);
gulp.task("minJs", ["min:portal", "min:init", "min:client", "min:shared"]);

gulp.task("build", ["min:views"]);//["clean", "min:views", "min"]);

gulp.task('watch:html', function () {  
    gulp.watch('./app/**/**/*.html', ['min:views']);
    gulp.watch('./app/**/**/*.js', ['minJS']);
    gulp.watch('./app/**/**/*.css', ['min:css']);
});

// [Watch Portal] View & Portal's js & CSS > gulp watch:html 

gulp.task('portalView-watch', ['min:views'], function (done) {
    browserSync.reload();
    done();
});
gulp.task('portalJS-watch', ['clean','minJs'], function (done) {
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