app.controller('AdvancedSearchCtrl', ['$scope', '$location', 'substanceSvc',
    function ($scope, $location, substanceSvc) {

    $scope.search = search;
    $scope.query = {
        q : null,
        mp1: null,
        mp2: null,
        fp1: null,
        fp2: null,
        bp1: null,
        bp2: null,
        d1: null,
        d2: null,
        vp1: null,
        vp2: null,
        ri1: null,
        ri2: null,
        ws: null
    };

    init();
    function init() {
        loadResources();
    }

    function loadResources() {
        $scope.query = substanceSvc.getQuery();
    }

    function search() {
        saveQ();
        $location.url('/search');
    }

    function saveQ() {
        substanceSvc.setQuery($scope.query);
    }
}]);