app.controller('TestCtrl', ['$scope', function ($scope) {
    init();
    function init() {
        loadResources();
    }
    function loadResources() {
        //$scope.resources = resourceSvc.getTopFiveResources();
    }
}]);