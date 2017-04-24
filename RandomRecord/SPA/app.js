var app = angular.module('App', []);
app.controller('AppController', function ($scope, $http) {
    $http.get("/api/user")
    .then(function (response) {
        $scope.record = response.data;
    });
})