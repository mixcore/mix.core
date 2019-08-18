# Mixcore CMS 

> Fully Open Source UI Tools to create web apps
> CMS and Dashboards built on top of .Net Core, Angular.JS and Bootstrap.

![Mixcore CMS](https://github.com/mixcore/mix.core/blob/master/assets/mixcore.png?raw=true "What is Mixcore CMS?")

|Services  |Result  |Services  |Result  |Services  |Result  |
|---------|---------|---------|---------|---------|---------|
|Travis CI     |[![Build Status](https://travis-ci.org/mixcore/mix.core.svg?branch=master)](https://travis-ci.org/mixcore/mix.core)|AppVeyor CI     |[![Build status](https://ci.appveyor.com/api/projects/status/8o02frivdxa0dgpl/branch/master?svg=true)](https://ci.appveyor.com/project/Smilefounder/mix-core/branch/master)          |Gitter     |[![Join the chat at https://gitter.im/mix-core/Lobby](https://badges.gitter.im/mix-core/Lobby.svg)](https://gitter.im/mix-core/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)|
Licenses status     |[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fmixcore%2Fmix.core.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2Fmixcore%2Fmix.core?ref=badge_shield)         |Codefactor     |[![CodeFactor](https://www.codefactor.io/repository/github/mixcore/mix.core/badge)](https://www.codefactor.io/repository/github/mixcore/mix.core)         |

## References
- Demo: http://stag.mixcore.org
- Docs: https://docs.mixcore.org
- Youtube: https://www.youtube.com/channel/UChqzh6JnC8HBUSQ9AWIcZAw
- Twitter: https://twitter.com/mixcore_cms
- Medium: https://medium.com/mixcore

## GITs clone
```sh
mkdir mixcore
cd mixcore

git clone https://github.com/mixcore/mix.heart.git
git clone https://github.com/mixcore/mix.identity.git
git clone https://github.com/mixcore/mix.core.git
```

## Build & Run
### Website
```sh
cd mix.core/src/Mix.Cms.Web
npm install
dotnet restore
dotnet build
dotnet run
```
### Modify Portal
````sh
cd mix.core/src/portal-app
gulp serve
````

> Note: If you facing any System.Data.SqlClient.SqlException error, please replace all content inside "appsettings.json" file with "{}".

## UI Screenshots 
### Default Theme: [Now UI Pro](https://demos.creative-tim.com/now-ui-kit-pro/presentation.html)

> Now UI Kit PRO is a premium Bootstrap 4 kit provided by Invision and Creative Tim. It is a beautiful cross-platform UI kit featuring over 1000 components, 34 sections and 11 example pages.

![Mixcore CMS default template with Now UI Pro](https://demos.creative-tim.com/now-ui-kit-pro/assets/img/presentation-page/pages/blog-posts.jpg "Mixcore CMS default template with Now UI Pro")

### Admin Portal

> Mixcore CMS Back-office is built on top of the much awaited Bootstrap 4. This makes starting a new project very simple. It also provides benefits if you are already working on a Bootstrap 4 project.

![Mixcore Admin Portal Bootstrap 4.x](https://docs.mixcore.org/img/screencapture-stag-mixcore-org-portal-2019-08-04-16_04_15.jpg "Mixcore CMS Admin Portal Bootstrap 4")

## Thanks to

This project has been developed using:
* [Creative Tim](https://www.creative-tim.com/)
* [Bootstrap](https://getbootstrap.com/)
* [BrowserStack](https://www.browserstack.com/)
* [Micon](http://xtoolkit.github.io/Micon/icons/)
* [.NET](https://www.microsoft.com/net/core)
* [Designed by Freepik](https://www.freepik.com)
* And more...


## License

Mixcore CMS is licensed under the **[GNU General Public License v3.0](https://github.com/mixcore/mix.core/blob/master/LICENSE)**

[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fmixcore%2Fmix.core.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fmixcore%2Fmix.core?ref=badge_large)
=======

## How to contribute

Fork this repo to your GitHub account, clone it locally and try to follow
the following simple guidelines.

* **Never** write any code in the master branch
* When writing code, do it in a specific feature branch
* Send your pull request from that feature branch
* After your pull request has been accepted, sync the changes into master from the upstream remote
* Delete you feature branch
* Again, **NEVER** write any code in the master branch ;)
