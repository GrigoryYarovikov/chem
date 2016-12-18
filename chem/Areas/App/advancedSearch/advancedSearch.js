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

    $scope.submitAdvSearchForm = function () {
        if ($scope.advSearchForm.$valid) {
            search();
        }
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

function addDirective(name, validator) {
    app.directive(name, function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function ($scope, element, attr, ctrl) {
                var selector = attr[name];
                var otherElement = angular.element("[data-ng-model=\"" + selector + "\"]")[0];
                otherElement.addEventListener('input', onOtherFieldChange);

                function onOtherFieldChange() {
                    //    ctrl.$validate();
                }

                function getOtherValue() {
                    return parseInt(otherElement.value);
                }

                ctrl.$validators.gte = function (currentValue) {
                    var otherValue = getOtherValue()
                    console.log(otherValue);
                    var isEmpty = isNaN(otherValue) || isNaN(currentValue) || currentValue == null;
                    return isEmpty || validator(currentValue, otherValue);
                };
            }
        };
    });
}

addDirective('lte', function (currentValue, otherValue) { // less then or equals 
    return currentValue <= otherValue;
});

addDirective('gte', function (currentValue, otherValue) { // greater then or equals 
    console.log(currentValue, otherValue);
    return currentValue >= otherValue;
});
