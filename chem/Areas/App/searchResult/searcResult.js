app.controller('SearchCtrl', ['$scope', 'substanceSvc', '$location', '$document', '$timeout', function ($scope, substanceSvc, $location, $document, $timeout) {

    $scope.drawScheme = drawScheme;
    $scope.currentPage = 1;
    $scope.pageSize = 6;
    $scope.total = 0;
    $scope.currentPageList = null;
    $scope.pageLink = "";
    $scope.query = {};

    $scope.doCtrlPagingAct = doCtrlPagingAct;

    init();

    function init() {
        $scope.query = substanceSvc.getQuery();
        loadResources();

        if (!$scope.query.q) {
            $location.url('/search');
        }
    }

    function getPageNumber() {
        var numAsString = $location.search()['p'];
        var pageNum = parseInt(numAsString);
        var maxPageNum = $scope.total / $scope.pageSize;
        return (!isNaN(pageNum) && p > 0 && p <= maxPageNum) ? pageNum : 1;
    }

    function loadResources() {
        substanceSvc.getSubstanceByQuerry().then(function successCallback(response) {
            $scope.resources = response.data;
            $scope.total = response.data.length;
            $scope.currentPageList = $scope.resources.slice(0, $scope.pageSize);
            doCtrlPagingAct("", getPageNumber(), $scope.pageSize, $scope.pageSize);
            $scope.isLoaded = true;
        },
        function errorCallback(reason) {
            $scope.error = true;
        })
    }

    function doCtrlPagingAct(text, page, pageSize, total) {
        $location.url('/search?p=' + page)
        $scope.currentPage = page;
        $scope.currentPageList = $scope.resources.slice(pageSize * (page - 1), pageSize * page);
        $timeout(drawScheme);
    };

    //header
    $scope.search = search;
    $scope.advancedSearch = advancedSearch;

    function search() {
        saveQ();
        $scope.isLoaded = false;
        loadResources();
    }

    function advancedSearch() {
        saveQ();
        $location.url('/advancedSearch');
    }

    function saveQ() {
        substanceSvc.setQuery($scope.query);
    }
}]);