app.factory('substanceSvc', ['$resource', '$q', '$http', function ($resource, $q, $http) {

    var _query = {};

    var getQuery = function () {
        return _query;
    }

    var setQuery = function (query) {
        _query = query;
    }

    var getSubstanceByQuerry = function () {
        return $http({
            method: 'GET',
            url: '/api/substances/getByQuery',
            params: {query: _query}
        })
    };

    var getSubstanceById = function (id) {
        return $http({
            method: 'GET',
            url: '/api/substances/get',
            params: {id: id}
        })
    };

    var getSubstanceFormulas = function (id) {
        return $http({
            method: 'GET',
            url: '/api/substances/getReactionList',
            params: { id: id }
        })
    }

    return {
        getSubstanceByQuerry: getSubstanceByQuerry,
        getSubstanceById: getSubstanceById,
        getQuery: getQuery,
        setQuery: setQuery,
        getSubstanceFormulas: getSubstanceFormulas
    };

}]);

//$http({
//    method: 'GET',
//    url: '/someUrl'
//}).then(function successCallback(response) {
//    // this callback will be called asynchronously
//    // when the response is available
//}, function errorCallback(response) {
//    // called asynchronously if an error occurs
//    // or server returns response with an error status.
//});