app.controller('ErrorCtrl', ['$scope', '$location', function ($scope, $location) {

    $scope.target = "";

    init();
    function init() {
        loadResources();
    }

    function loadResources() {
        //$scope.resources = resourceSvc.getTopFiveResources();
        $scope.target = $location.search()['target'];
    }
}]);