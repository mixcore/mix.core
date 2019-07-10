'use strict';
app.controller('Step1Controller', ['$scope', '$rootScope', 'ngAppSettings', '$timeout', '$location', '$http',
    'CommonService', 'Step1Services',
    function ($scope, $rootScope, ngAppSettings, $timeout, $location, $http, commonService, step1Services) {
        var rand = Math.floor(Math.random() * 10000) + 1;
        $scope.settings = {
            providers: [
                { text: 'Microsoft SQL Server', value: 'MSSQL', img: '/assets/img/mssql.jpg' },
                { text: 'MySQL Server', value: 'MySQL', img: '/assets/img/mysql.jpg' }
            ],
            cultures: []
        };
        $scope.loadSettings = async function () {
            step1Services.saveDefaultSettings();
            var getCultures = await commonService.loadJArrayData('cultures.json');
            if(getCultures.isSucceed){
                $scope.settings.cultures = getCultures.data;
                $scope.initCmsModel.culture = $scope.settings.cultures[0];
                $scope.dbProvider = $scope.settings.providers[0];
                $scope.initCmsModel.databaseProvider = $scope.dbProvider.value;
                $rootScope.isBusy = false;
                $scope.$apply();
            }else {
                if (getCultures) {
                    $rootScope.showErrors(getCultures.errors);
                }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
           
        };
        $scope.loadProgress = async function (percent) {
            var elem = document.getElementsByClassName("progress-bar")[0]; 
            elem.style.width = percent + '%'; 
        };
        $scope.initCmsModel = {
            isUseLocal: true,
            localDbConnectionString: '',
            sqliteDbConnectionString: '',
            localDbConnectionString: 'Server=(localdb)\\MSSQLLocalDB;Initial Catalog=' + rand + 'mix-cms.db;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True',
            sqliteDbConnectionString: 'Data Source=' + rand + 'mix-cms',
            localDbName: rand + '-mix-cms',
            dataBaseServer: '',
            dataBaseName: '',
            dataBaseUser: '',
            dataBasePassword: '',
            adminPassword: '',
            lang: 'en-us',
            isMysql: false,
            databaseProvider: '',
            culture: $scope.settings.cultures[0]
        };
        
        $scope.updateLocalDbName = function () {
            $scope.initCmsModel.localDbName = $scope.initCmsModel.localDbName+'.db';
            $scope.initCmsModel.localDbConnectionString = 'Server=(localdb)\\mssqllocaldb;Database=' + $scope.initCmsModel.localDbName + ';Trusted_Connection=True;MultipleActiveResultSets=true';
            $scope.initCmsModel.sqliteDbConnectionString = 'Data Source=' + $scope.initCmsModel.localDbName;
        };
        $scope.initCms = async function () {
            $rootScope.isBusy = true;            
            var result = await step1Services.initCms($scope.initCmsModel);
            if (result.isSucceed) {
                $rootScope.isBusy = false;
                window.location.href = '/init/step2';
            }
            else {
                if (result) { $rootScope.showErrors(result.errors); }
                $rootScope.isBusy = false;                
            }            
           
        }
    }]);