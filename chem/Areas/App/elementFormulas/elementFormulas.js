app.controller('ElementFormulasCtrl', ['$scope', 'substanceSvc', '$location', '$document', '$timeout', '$routeParams', '$sce',
    function ($scope, substanceSvc, $location, $document, $timeout, $routeParams, $sce) {
        //from global 
        $scope.drawScheme = drawScheme;
        $scope.massString = "";
        $scope.LoadData = loadData;
        init();

        function init() {
            $scope.id = $routeParams.id;
            loadResources();
        }

        function loadResources() {
            substanceSvc.getSubstanceFormulas($scope.id).then(function successCallback(response) {
                $scope.resources = response.data;
                toTrust();
                $scope.isLoaded = true;
            },
            function errorCallback(reason) {
                $scope.error = true;
            })
        }

        function toTrust()
        {
            $.each($scope.resources, function (id, item) {
                item.TextValue = $sce.trustAsHtml(item.TextValue);
                });
        }

        function loadData()
        {

        }

        //header
        $scope.search = search;
        $scope.advancedSearch = advancedSearch;
        $scope.query = {};
        $scope.query.q = null;

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