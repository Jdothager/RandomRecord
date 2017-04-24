var app = angular.module('App', []);
app.controller('AppController', function ($scope, $http) {
    $http.get("/api/record")
    .then(function (response) {
        $scope.record = response.data;

        $scope.FirstName = record.FirstName
    });
})