var testControllers = angular.module("testControllers", []);
testControllers.controller("testController", ["$scope", function ($scope) {
    $scope.message = "Hello Asp.net & Angularjs";
}]);