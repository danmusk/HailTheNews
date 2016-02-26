var app = angular.module('PraiseApp', ['ngMaterial']);

app.controller('praiseCtrl', function ($scope, $http) {
    $http({
        method : "GET",
        url : "api/PraiseCases"
    }).then(function mySucces(response) {
        $scope.praiseCases = response.data;
        console.log(response.data);
    }, function myError(response) {
        console.error(response);
    });
});

