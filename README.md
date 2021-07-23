# Mixcore CMS [![Become a Backer](https://opencollective.com/mixcore/tiers/backer.svg?avatarHeight=36)](https://opencollective.com/mixcore#support) 

[![backer](https://opencollective.com/mixcore/tiers/backer/badge.svg?label=backer&color=brightgreen)](https://opencollective.com/mixcore#support) [![Donate](https://img.shields.io/badge/$-donate-ff69b4.svg)](https://www.paypal.me/mixcore) [![Buy us a coffee](https://img.shields.io/badge/$-BuyMeACoffee-orange.svg)](https://www.buymeacoffee.com/mixcore) 

## Fully Open Source UI Tools to create multi-purpose Enterprise Web Apps, Mobile Apps & Application Services


## UI Screenshots 

### Admin Portal

> Mixcore CMS Back-office is built on top of the much awaited Bootstrap 5. This makes starting a new project very simple. It also provides benefits if you are already working on a Bootstrap 5 project.

(New Portal in Angular / Svelte will be coming soon)

![Mixcore Admin Portal Bootstrap 5.x](https://user-images.githubusercontent.com/3785721/125382627-322d7d00-e3c0-11eb-8ff7-f02316770876.png "Mixcore CMS Admin Portal Bootstrap 5")


![Mixcore Admin Portal Bootstrap 5.x](https://user-images.githubusercontent.com/3785721/126033976-28302532-1284-4be8-b8dd-74258a227873.png "Mixcore CMS Admin Portal Bootstrap 5")


## Special features (Out of the box)

- [x] **Reliability** - Member roles and permissions.
- [x] **High Security** - Strong Data Encryption and Security compliance.
- [x] **Multilingual** - Flexible multilingual content migration.
- [x] **High Performance** - Millisecond response time.
- [x] **Cross Platforms** - Powered by .NET Core and run everywhere.
- [x] **Online Coding** - Visual Studio Code's heart inside.
- [x] **Customizable Designs** - Build any kinds of website.
- [x] **SEO Friendly** - No extra plugin required.
- [x] **Media Management** - Multiple file formats for your website / application.
- [x] **Manage On The Go** - Manage and Code everywhere you want.
- [x] **Easy and Accessible** - Non deep tech knowledge required.
- [x] **Analytics** - Inside Google Analytics dashboard & no extra plugin required.
- [x] **Dynamic Modular Architecture** - Powerful module layers & Attribute sets feature.
- [x] **Extensibility** - API-first architecture for Plug & Play.
- [x] **Easy Backup** - Powerful 1 step export.
- [x] **More Coffee time!** - You can relax and explore more ton of features are not listed here...


### CMS and Dashboards built on top of ASP.Net Core / Dotnet Core, SignalR, Angular / Bootstrap / React / Vue / Svelte.
 
|Services  |Result  |Services  |Result  |
|---------|---------|---------|---------|
|Github workflow     |[![Build Status](https://github.com/mixcore/mix.core/actions/workflows/build-check.yml/badge.svg)](https://github.com/mixcore/mix.core/actions/workflows/build-check.yml)|
|Travis CI     |[![Build Status](https://travis-ci.org/mixcore/mix.core.svg?branch=master)](https://travis-ci.org/mixcore/mix.core)|AppVeyor CI     |[![Build status](https://ci.appveyor.com/api/projects/status/8o02frivdxa0dgpl/branch/master?svg=true)](https://ci.appveyor.com/project/Smilefounder/mix-core/branch/master)          |
Gitter     |[![Join the chat at https://gitter.im/mix-core/Lobby](https://badges.gitter.im/mix-core/Lobby.svg)](https://gitter.im/mix-core/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)|Licenses status     |[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fmixcore%2Fmix.core.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2Fmixcore%2Fmix.core?ref=badge_shield)         |
Codefactor     |[![CodeFactor](https://www.codefactor.io/repository/github/mixcore/mix.core/badge)](https://www.codefactor.io/repository/github/mixcore/mix.core)         |Azure|[![Build Status](https://dev.azure.com/mixcore/mix.core/_apis/build/status/mixcore.mix.core?branchName=master)](https://dev.azure.com/mixcore/mix.core/_build/latest?definitionId=1&branchName=master)|


## References


|  |Links  |
|---------|---------|
|STAG / Demo     |https://demo.mixcore.org or https://dev.mixcore.org or https://mochahost.demo.mixcore.org/portal (administrator / P@ssw0rd)  Ref: [How to install Mixcore CMS with Azure Docker Composer](https://community.mixcore.org/topic/4/install-mixcore-cms-with-azure-and-docker-compose)|
|Dev docs     |https://docs.mixcore.org / https://mixcore.dev|
|Community     |https://community.mixcore.org|
|Youtube     |https://www.youtube.com/channel/UChqzh6JnC8HBUSQ9AWIcZAw|
|Twitter     |https://twitter.com/mixcore_cms         |
|Medium     |https://medium.com/mixcore         |

## Run with Docker

###  Latest Docker Image
```sh
docker pull mixcore/mix.core:latest
docker run -it --rm -p 5000:80 --name mixcore_cms mixcore/mix.core:latest
```

### Or with Docker Compose
```sh
docker-compose build
docker-compose up
```

## GITs clone
```sh
mkdir mixcore
cd mixcore

git clone https://github.com/mixcore/mix.core.git
```

Optional:

> Optional steps as those packages are Nuget Library

```bash
git clone https://github.com/mixcore/mix.heart.git
git clone https://github.com/mixcore/mix.identity.git
```



## Build & Run with [Dotnet SDK](https://dotnet.microsoft.com/download)

### Build & Run Mixcore CMS

> REM Make sure you already read and download Dotnet Core SDK here https://dotnet.microsoft.com/download

```sh
cd mix.core/src/Mix.Cms.Web

dotnet restore
dotnet build
dotnet run
```
### Modify & Build Portal Front-End source (Optional)

> This step is optional and only needed in case you would like to modify the portal front-end code

````sh
cd mix.core/src/portal-app

npm install
npm install --global gulp-cli
gulp build
````

> Note: If you facing any System.Data.SqlClient.SqlException error, please replace all content inside "appsettings.json" file with "{}".

## Thanks to

> This project has been developed using:
* [.NET](https://www.microsoft.com/net/core)
* [Bootstrap](https://getbootstrap.com/)
* [BrowserStack](https://www.browserstack.com/)
* [Designed by Freepik](https://www.freepik.com)
* And more...


## License

Mixcore CMS is licensed under the **[MIT](https://github.com/mixcore/mix.core/blob/master/LICENSE)**


|Permissions  |Limitations  |Conditions  |
|---------|---------|---------|
|✔ Commercial use     |✖ Liability         |ℹ License and copyright notice         |
|✔ Modification     |✖ Warranty         |         |
|✔ Distribution     |         |         |
|✔ Private use     |         |         |
|     |         |         |


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
* Ref: https://datasift.github.io/gitflow/IntroducingGitFlow.html
