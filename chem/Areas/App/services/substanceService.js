app.factory('substanceSvc', ['$resource', '$q', '$http', function ($resource, $q, $http) {

    var getSubstanceByQuerry = function (query) {
        return $http({
            method: 'GET',
            url: '/api/substances/getByQuery',
            params: {query: query}
        })
    };

    var getSubstanceById = function (id) {
        return $http({
            method: 'GET',
            url: '/api/substances/get',
            params: {id: id}
        })
    };


    return {
        getSubstanceByQuerry: getSubstanceByQuerry,
        getSubstanceById: getSubstanceById
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