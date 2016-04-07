app.controller('ElementCtrl', ['$scope', 'substanceSvc', '$location', '$document', '$timeout', '$routeParams', '$sce',
    function ($scope, substanceSvc, $location, $document, $timeout, $routeParams, $sce) {
        //from global 
    $scope.drawScheme = drawScheme;
    $scope.massString = "";

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
            $scope.massString = $sce.trustAsHtml(buildMassString($scope.resources.Elements));
        })
    }

    function buildMassString(list) {
        var s = "";
        $.each(list, function (id,elem) {
            s += "<p>" +
                "<span class='chem_elem-short'>" + elem.Sign + "</span>" +
                "<span class='chem_elem-long'>" + elem.Rus + "</span>" +
                "<span class='chem_elem-long'>" + elem.Weight.toFixed(3) + "</span>" +
                "<span class='chem_elem-short'>" + elem.Count + "</span>" +
                "<span class='chem_elem-long'>" +
                (100.0 / $scope.resources.MolecularWeight * elem.Count * elem.Weight).toFixed(3) + 
                "%</span>" +
                "</p>";
        });
        return s;
    }
}]);