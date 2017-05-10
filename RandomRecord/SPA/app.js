var app = angular.module('App', []);

app.controller('AppController', function ($scope, $http) {
    $scope.record = null;

    // get data from api
    $scope.getNewRecord = function () {
        $http.get("/api/record").then(function (response) {
            $scope.record = response.data;
        });
    }

    $scope.getNewRecord();
})