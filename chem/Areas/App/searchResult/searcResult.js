app.controller('SearchCtrl', ['$scope', 'substanceSvc', '$location', '$document', '$timeout', function ($scope, substanceSvc, $location, $document, $timeout) {

    $scope.drawScheme = drawScheme;
    $scope.currentPage = 1;
    $scope.pageSize = 6;
    $scope.total = 0;
    $scope.currentPageList = null;
    $scope.pageLink = "";

    $scope.doCtrlPagingAct = doCtrlPagingAct;

    init();

    function init() {
        $scope.q = $location.search()['q'].trim();
        loadResources();

        if (!$scope.q) {
            $location.url('/search');
        }
    }

    function loadResources() {
        substanceSvc.getSubstanceByQuerry($scope.q).then(function successCallback(response) {
            $scope.resources = response.data;
            $scope.total = response.data.length;
            $scope.currentPageList = $scope.resources.slice(0, $scope.pageSize);

            var p = $location.search()['p'];
            if (!isNaN(p) && p > 0 && p < $scope.total / $scope.pageSize) {
                p = parseInt(p);
                $scope.currentPage = p;
                $scope.currentPageList = $scope.resources.slice($scope.pageSize * p, $scope.pageSize * (p + 1));
            }
            $scope.isLoaded = true;
            $timeout(drawScheme);
        })
    }

    function doCtrlPagingAct(text, page, pageSize, total) {
        $location.url('/search?q=' + $scope.q + '&p=' + page)
        $scope.currentPageList = $scope.resources.slice(pageSize * page, pageSize * (page + 1));
        $timeout(drawScheme);
    };
}]);