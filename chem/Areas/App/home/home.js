app.controller('HomeCtrl', ['$scope', '$location', function ($scope, $location) {

    $scope.search = search;
    $scope.query = "";

    init();
    function init() {
        loadResources();
    }

    function loadResources() {
        //$scope.resources = resourceSvc.getTopFiveResources();
        $scope.malina = 'malina';
    }

    function search() {
        $location.url('/search?q=' + $scope.query);
    }
}]);