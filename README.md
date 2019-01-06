# mix.core

![Mixcore CMS](https://github.com/mixcore/mix.core/blob/master/assets/mixcore.png?raw=true "What is Mixcore CMS?")

[![Build status](https://ci.appveyor.com/api/projects/status/8o02frivdxa0dgpl/branch/master?svg=true)](https://ci.appveyor.com/project/Smilefounder/mix-core/branch/master) [![Join the chat at https://gitter.im/mix-core/Lobby](https://badges.gitter.im/mix-core/Lobby.svg)](https://gitter.im/mix-core/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fmixcore%2Fmix.core.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2Fmixcore%2Fmix.core?ref=badge_shield)

# Tutorials
- Demo: http://dev.mixcore.org
- Docs: https://docs.mixcore.org
- Youtube: https://www.youtube.com/channel/UChqzh6JnC8HBUSQ9AWIcZAw
- Twitter: https://twitter.com/mixcore_cms

# GITs clone

```
mkdir mixcore
cd mixcore

git clone https://github.com/mixcore/mix.heart.git
git clone https://github.com/mixcore/mix.identity.git
git clone https://github.com/mixcore/mix.core.git
```

# Build & Run

```
cd mix.core/src/Mix.Cms.Web

npm install
gulp build
dotnet restore
dotnet bundle
dotnet build
dotnet run
```

> Note: If you facing any System.Data.SqlClient.SqlException error, please replace all content inside "appsettings.json" file with "{}".

# UI:  
  - **Default template:**
![Mixcore CMS default template with Now UI Pro](https://github.com/mixcore/mix.core/blob/master/assets/front-end.jpg?raw=true "Mixcore CMS default template with Now UI Pro")
  - **Admin Portal:**
![Mixcore Admin Portal Bootstrap 4.x](https://github.com/mixcore/mix.core/blob/master/assets/admin-portal.jpg?raw=true "Mixcore CMS Admin Portal Bootstrap 4")

# Thanks to

This project has been developed using:
* [Creative Tim](https://www.creative-tim.com/)
* [Bootstrap](https://getbootstrap.com/)
* [BrowserStack](https://www.browserstack.com/)
* [Micon](http://xtoolkit.github.io/Micon/icons/)
* [.NET](https://www.microsoft.com/net/core)
* And more...


# License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fmixcore%2Fmix.core.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fmixcore%2Fmix.core?ref=badge_large)
=======

# How to contribute

Fork this repo to your GitHub account, clone it locally and try to follow
the following simple guidelines.

* **Never** write any code in your master branch
* When writing code, do it in a specific feature branch
* Send your pull request from that feature branch
* After your pull request has been accepted, sync the changes into master from the upstream remote
* Delete you feature branch
* Again, **NEVER** write any code in your master branch ;)
