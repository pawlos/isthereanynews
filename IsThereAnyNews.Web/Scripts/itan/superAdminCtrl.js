angular.module("itan")
.controller("itan.SuperAdminCtrl", function ($scope, $http) {

    $scope.configurationStatus = {
        loaded: false,
        data: {}
    }

    $http.get("/Home/ConfigurationStatus")
    .success(function (data) {
        $scope.configurationStatus.data = data;
        $scope.configurationStatus.loaded = true;
    });


    $scope.onRegistrationClick = function (newRegistrationStatus) {
        var httpOptions = {
            method: 'POST',
            url: "/Home/ChangeRegistration",
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
            url: "/Home/ChangeUsersLimit",
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
            url: "/Home/SpinUpdateJob",
            data: {
            }
        };

        $http(httpOptions)
            .success(function () {
                window.alert("update started");
            });
    };
});

