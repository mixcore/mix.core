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
    webroot: "./app/",///wwwroot
    jsObtions:{},
    htmlOptions:{collapseWhitespace: false},
    cssOptions:{}, //showLog : (True, false) to trun on or off of the log
    fontOptions:{fontPath: dest+ '/wwwroot/fonts'}
};

paths.views = {
    src: [
        paths.webroot + "app-shared/**/*.html",
        paths.webroot + "app-portal/**/*.html",
        paths.webroot + "app-client/**/*.html",
        paths.webroot + "app-init/**/*.html"
    ],
    dest: './wwwroot/'
};

gulp.task("min:views", function (cb) {
    return gulp.src(paths.views.src, { base: "." })        
        .pipe(htmlmin(paths.htmlOptions))
        .pipe(gulp.dest(paths.views.dest));
});
gulp.task("build", ["min:views"]);