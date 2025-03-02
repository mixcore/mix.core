/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    //fontmin = require("gulp-fontmin"),
    htmlmin = require("gulp-htmlmin"),

    uglify = require("gulp-uglify-es").default;

var uglifyjs = require('uglify-es'); // can be a git checkout
// or another module (such as `uglify-es` for ES6 support)
var composer = require('gulp-uglify/composer');
var pump = require('pump');

var minify = composer(uglifyjs, console);
var dest = '.';//For publish folder use "./bin/Release/PublishOutput/";
//C:\\Git\\GitHub\\Queen-Beauty\\QueenBeauty\\
var paths = {
    webroot: "../Mix.Cms.Web/wwwroot/",///wwwroot
    webapp: "./app/",///app
    scriptLib: "./lib/",///app
    styleLib: "./lib/",///app
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
        paths.webapp + "app-portal/services/**/*.js",
        paths.webapp + "app-portal/pages/**/*.js",
        paths.webapp + "app-portal/components/**/*.js"
    ],
    dest: paths.webroot + "js/app-portal.min.js"
};
paths.appPortalRequired = {
    src: [
        paths.webapp + "app-portal/shared/**/*.js"
    ],
    dest: paths.webroot + "js/app-portal-required.min.js"
};
paths.initApp = {
    src: [
        paths.webapp + "app-init/app.js",
        paths.webapp + "app-init/app.route.js",
        paths.webapp + "app-init/pages/**/*.js"
    ],
    dest: paths.webroot + "js/app-init.min.js"
};

paths.securityApp = {
    src: [
        paths.webapp + "app-security/app.js",
        paths.webapp + "app-security/app.route.js",
        paths.webapp + "app-security/pages/**/*.js"
    ],
    dest: paths.webroot + "js/app-security.min.js"
};

paths.clientApp = {
    src: [
        paths.webapp + "app-client/app.js",
        paths.webapp + "app-client/app-client-controller.js",
        paths.webapp + "app-client/components/**/script.js"
    ],
    dest: paths.webroot + "js/app-client.min.js"
};
paths.clientAppRequired={
    src: [
        paths.webapp + "app-client/shared/**/*.js"
    ],
    dest: paths.webroot + "js/app-client-required.min.js"
}

paths.tablerJs = {
    src: [        
        paths.scriptLib + "plugins/tabler-0.0.32/js/vendors/**/*.js",
        paths.scriptLib + "plugins/tabler-0.0.32/js/vendors/**/*.*.js",
        paths.scriptLib + "plugins/tabler-0.0.32/js/core.js",
        paths.scriptLib + "plugins/tabler-0.0.32/js/require.min.js",        
        paths.scriptLib + "plugins/tabler-0.0.32/js/dashboard.js",
    ],
    dest: paths.webroot + "js/tabler.min.js"
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
        paths.scriptLib + "angularjs/angular-sanitize.min.js",
        paths.scriptLib + "jquery/jquery.min.js",
        paths.scriptLib + "jquery/jquery-ui.min.js",
        paths.scriptLib + "bootstrap/js/popper.min.js",
        paths.scriptLib + "bootstrap/js/bootstrap.min.js",
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
paths.appInitCss = {
    src: [        
        paths.webapp + "app-init/**/*.css"
    ],
    dest: paths.webroot + "css/app-init.min.css"
};
paths.portalCss = {
    src: [
        "./lib/portal/**/*.css",
        "./lib/portal/**/*.*.css"       
    ],
    dest: paths.webroot + "css/portal.min.css"
};
paths.sharedCss = {
    src: [        
        "./lib/shared/**/*.css",
        "./lib/shared/**/*.*.css"
    ],
    dest: paths.webroot + "css/shared.min.css"
};
paths.tablerCss = {
    src: [
        "./lib/plugins/tabler-0.0.32/css/*.css",
        "./lib/plugins/tabler-0.0.32/css/*..*css",
    ],
    dest: paths.webroot + "css/tabler.min.css"
};

paths.views = {
    src: [
        "./app/app-shared/**/*.html",
        "./app/app-portal/**/*.html",
        "./app/app-client/**/*.html",
        "./app/app-init/**/*.html",
        "./app/app-security/**/*.html",
    ],
    dest: paths.webroot
};

gulp.task("min:portalApp", function (cb) {
    return gulp.src(paths.appPortal.src, { base: "." })
        .pipe(concat(paths.appPortal.dest))
        //.pipe(uglify())
        // .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});
gulp.task("min:portalAppRequired", function (cb) {
    return gulp.src(paths.appPortalRequired.src, { base: "." })
        .pipe(concat(paths.appPortalRequired.dest))
        .pipe(uglify())
         .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});

gulp.task("min:initApp", function (cb) {
    return gulp.src(paths.initApp.src, { base: "." })
        .pipe(concat(paths.initApp.dest))
        //.pipe(uglify())
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});
gulp.task("min:securityApp", function (cb) {
    return gulp.src(paths.securityApp.src, { base: "." })
        .pipe(concat(paths.securityApp.dest))
        //.pipe(uglify())
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});

gulp.task("min:clientApp", function (cb) {
    return gulp.src(paths.clientApp.src, { base: "." })
        .pipe(concat(paths.clientApp.dest))
        //.pipe(uglify())
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});

gulp.task("min:clientAppRequired", function (cb) {
    return gulp.src(paths.clientAppRequired.src, { base: "." })
        .pipe(concat(paths.clientAppRequired.dest))
        .pipe(uglify())
        .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});

gulp.task("min:sharedApp", function (cb) {
    return gulp.src(paths.sharedApp.src, { base: "." })
        .pipe(concat(paths.sharedApp.dest))
        //.pipe(uglify())
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
        .pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});
gulp.task("min:portal", function (cb) {
    return gulp.src(paths.portal.src, { base: "." })
        .pipe(concat(paths.portal.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});
gulp.task("min:tablerJs", function (cb) {
    return gulp.src(paths.tablerJs.src, { base: "." })
        .pipe(concat(paths.tablerJs.dest))
        //.pipe(minify(paths.jsOptions))
        .pipe(gulp.dest(dest));
});
gulp.task("min:appCss", function (cb) {
    return gulp.src(paths.appCss.src, { base: "." })
        .pipe(concat(paths.appCss.dest))
        .pipe(cssmin(paths.appCssOptions))
        .pipe(gulp.dest(dest));
});
gulp.task("min:tablerCss", function (cb) {
    return gulp.src(paths.tablerCss.src, { base: "." })
        .pipe(concat(paths.tablerCss.dest))
        .pipe(cssmin(paths.tablerCss))
        .pipe(gulp.dest(dest));
});
gulp.task("min:appInitCss", function (cb) {
    return gulp.src(paths.appInitCss.src, { base: "." })
        .pipe(concat(paths.appInitCss.dest))
        .pipe(cssmin(paths.appInitCss))
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
gulp.task("clean:portalAppRequired", function (cb) {
    rimraf(paths.appPortalRequired.dest, cb);
});

gulp.task("clean:clientApp", function (cb) {
    rimraf(paths.clientApp.dest, cb);
});
gulp.task("clean:clientAppRequired", function (cb) {
    rimraf(paths.clientAppRequired.dest, cb);
});

gulp.task("clean:sharedApp", function (cb) {
    rimraf(paths.sharedApp.dest, cb);
});

gulp.task("clean:initApp", function (cb) {
    rimraf(paths.initApp.dest, cb);
});

gulp.task("clean:securityApp", function (cb) {
    rimraf(paths.securityApp.dest, cb);
});

gulp.task("clean:tablerJs", function (cb) {
    rimraf(paths.tablerJs.dest, cb);
});

gulp.task("clean:tablerCss", function (cb) {
    rimraf(paths.tablerCss.dest, cb);
});
gulp.task("clean:appCss", function (cb) {
    rimraf(paths.appCss.dest, cb);
});

gulp.task("clean:appInitCss", function (cb) {
    rimraf(paths.appInitCss.dest, cb);
});

gulp.task("clean:portalCss", function (cb) {
    rimraf(paths.portalCss.dest, cb);
});
gulp.task("clean:sharedCss", function (cb) {
    rimraf(paths.sharedCss.dest, cb);
});


gulp.task("clean:css", ["clean:appCss", "clean:appInitCss", "clean:portalCss", "clean:sharedCss"
]);
gulp.task("clean:js", [
    "clean:framework", "clean:portal", "clean:shared"
    , "clean:portalApp","clean:portalAppRequired", "clean:clientApp", "clean:clientAppRequired", "clean:sharedApp", "clean:initApp", "clean:securityApp"
]);
gulp.task("min:js", ["min:portalApp","min:portalAppRequired", "min:initApp", "min:securityApp", "min:clientApp", "min:clientAppRequired", "min:sharedApp"
    , "min:shared", "min:portal", "min:framework"
]);
gulp.task("min:css", ['min:appCss', "min:appInitCss", 'min:portalCss', 'min:sharedCss']);

gulp.task("build", ['clean:js',
    'min:js', 'min:css', "min:views"]);//["clean", "min:views", "min"]);

gulp.task('watch:html', function () {
    gulp.watch('./app/**/**/*.html', ['min:views']);
    gulp.watch('./app/**/**/*.js', ['min:portalApp','min:portalAppRequired']);
    gulp.watch('./app/**/**/*.css', ['min:appCss','min:appInitCss']);
});

gulp.task('watch', function () {
    gulp.watch('./app/**/**/*.html', ['min:views']);
    gulp.watch('./app/app-portal/**/*.js', ['min:portalApp','min:portalAppRequired']);
    gulp.watch('./app/app-shared/**/*.js', ['min:sharedApp']);
    gulp.watch('./app/app-/**/*.js', ['min:sharedApp']);
    gulp.watch('./app/**/**/*.css', ['min:appCss','min:appInitCss']);
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