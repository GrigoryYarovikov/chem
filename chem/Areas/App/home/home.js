app.controller('HomeCtrl', ['$scope', '$location', 'substanceSvc', function ($scope, $location, substanceSvc) {

    $scope.search = search;
    $scope.advancedSearch = advancedSearch;
    $scope.query = {};
    $scope.query.q = null;
    
    init();

    function init() {
        var q = substanceSvc.getQuery();
        if (q) {
            $scope.query = q;
        }
    }

    function search() {
        saveQ();
        $location.url('/search');
    }

    function advancedSearch() {
        saveQ();
        $location.url('/advancedSearch');
    }

    function saveQ() {
        substanceSvc.setQuery($scope.query);
    }
}]);