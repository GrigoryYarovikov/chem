app.controller('ElementCtrl', ['$scope', 'substanceSvc', '$location', '$document', '$timeout', '$routeParams',
    function ($scope, substanceSvc, $location, $document, $timeout, $routeParams) {
        //from global 
    $scope.drawScheme = drawScheme;


    init();

    function init() {
        $scope.id = $routeParams.id;//$location.search()['q'].trim();
        loadResources();
    }

    function loadResources() {
        substanceSvc.getSubstanceById($scope.id).then(function successCallback(response) {
            $scope.resources = response.data;
            $scope.isLoaded = true;
            $timeout(drawScheme);
        })
    }
}]);