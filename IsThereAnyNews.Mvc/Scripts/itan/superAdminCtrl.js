angular.module("itan")
.controller("itan.SuperAdminCtrl", function ($scope, $http) {

    $scope.configurationStatus = {
        loaded: false,
        data: {}
    }

    $http.get("/Admin/ConfigurationStatus")
    .success(function (data) {
        $scope.configurationStatus.data = data;
        $scope.configurationStatus.loaded = true;
    });
});

