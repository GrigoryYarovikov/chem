window.app = angular.module('resourceManagerApp', ['ngRoute', 'ngResource', 'ngAnimate', 'custom-utilities', 'bw.paging']);
app.config(['$routeProvider', '$locationProvider', '$httpProvider', '$provide', function ($routeProvider, $locationProvider, $httpProvider, $provide) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
    $httpProvider.defaults.useXDomain = true;
    $locationProvider.html5Mode(true).hashPrefix('!');
    $routeProvider
        //.when('/Login', { templateUrl: '/Scripts/app/views/shared/Login.html' })
        //.when('/Register', { templateUrl: '/Scripts/app/views/shared/Register.html' })
        //.when('/Locations', { templateUrl: '/Scripts/app/views/locations/Locations.html', controller: 'LocationsCtrl' })
        //.when('/About', { templateUrl: '/Scripts/app/views/about/About.html' })
        //.when('/Locations/Add', { templateUrl: '/Scripts/app/views/locations/Add.html', controller: 'LocationCtrl' })
        //.when('/Locations/Edit/:locationId', { templateUrl: '/Scripts/app/views/locations/Edit.html', controller: 'LocationCtrl' })
        //.when('/Resources', { templateUrl: '/Scripts/app/views/resources/Resources-ng-table.html', controller: 'ResourcesCtrl' })
        //.when('/Resources/Add', { templateUrl: '/Scripts/app/views/resources/Add.html', controller: 'ResourceCtrl' })
        //.when('/Resources/Edit/:resourceId', { templateUrl: '/Scripts/app/views/resources/Edit.html', controller: 'ResourceEditCtrl' })
        //.when('/Resources/:resourceId', { templateUrl: '/Scripts/app/views/resources/Details.html', controller: 'ResourceCtrl' })
        //.when('/Activities/Add', { templateUrl: '/Scripts/app/views/activities/Add.html', controller: 'ActivityAddCtrl' })
        .when('/', { templateUrl: '/Areas/App/home/Home.html', controller: 'HomeCtrl', title: 'Большой химический справочник', caseInsensitiveMatch: true })
        .when('/Search', { templateUrl: '/Areas/App/searchResult/list.html', controller: 'SearchCtrl', title: 'Поиск химических веществ', caseInsensitiveMatch: true, reloadOnSearch: false })
        .when('/Error', { templateUrl: '/Areas/App/error/Error.html', controller: 'ErrorCtrl', title: 'Ошибка' })
        .when('/substance/:id', { templateUrl: '/Areas/App/element/element.html', controller: 'ElementCtrl', title: 'Вещество', caseInsensitiveMatch: true, reloadOnSearch: false })
        .otherwise({
            redirectTo: '/Error'
        });

    //$httpProvider.interceptors.push('authorizationInterceptor');
    //$httpProvider.interceptors.push('httpInterceptor');
}]);
//    .factory("userProfileSvc", function () {
//    return {};
//});

app.run(['$location', '$rootScope', function ($location, $rootScope) {
    $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
        $rootScope.title = current.$$route.title;
    });
}]);

window.utilities = angular.module("custom-utilities", []);



