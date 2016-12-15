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


    $scope.onRegistrationClick = function (newRegistrationStatus) {
        var httpOptions = {
            method: 'POST',
            url: "/Admin/ChangeRegistration",
            data: {
                status: newRegistrationStatus
            }
        };
        $http(httpOptions)
            .success(function () {
                $scope.configurationStatus.data.UserRegistration = newRegistrationStatus;
            });
    };

    $scope.onUsersLimitClicked = function (newLimit) {
        var httpOptions = {
            method: 'POST',
            url: "/Admin/ChangeUsersLimit",
            data: {
                limit: newLimit
            }
        };

        $http(httpOptions)
            .success(function () {
                $scope.configurationStatus.data.UserLimit = newLimit;
            });
    };

    $scope.onSpinUpdateJob = function () {
        var httpOptions = {
            method: 'POST',
            url: "/Admin/SpinUpdateJob",
            data: {
            }
        };

        $http(httpOptions)
            .success(function () {
                window.alert("update started");
            });
    };
});

