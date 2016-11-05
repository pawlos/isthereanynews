angular.module("itan")
    .filter("rawHtml",
        function($sce) {
            return function(html) {
                return $sce.trustAsHtml(html);
            };
        });