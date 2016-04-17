window.app = angular.module('resourceManagerApp', ['ngRoute', 'ngResource', 'ngAnimate', 'custom-utilities', 'bw.paging']);
app.config(['$routeProvider', '$locationProvider', '$httpProvider', '$provide', function ($routeProvider, $locationProvider, $httpProvider, $provide) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
    $httpProvider.defaults.useXDomain = true;
    $locationProvider.html5Mode(true).hashPrefix('!');
    $routeProvider
        .when('/advancedSearch', { templateUrl: '/Areas/App/advancedSearch/advancedSearch.html', controller: 'AdvancedSearchCtrl', title: 'Расширенный поиск', caseInsensitiveMatch: true})
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

app.run(['$location', '$rootScope', function ($location, $rootScope) {
    $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
        $rootScope.title = current.$$route.title;
    });
}]);

window.utilities = angular.module("custom-utilities", []);



