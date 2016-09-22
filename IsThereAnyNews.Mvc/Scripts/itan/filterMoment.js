angular.module("itan")
    .filter("moment", function () {
        return function (input, format) {
            return moment(input).format(format);
        }
    });