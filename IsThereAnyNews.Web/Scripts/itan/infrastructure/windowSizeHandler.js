angular
.module('itan')
.factory('windowSizeHandler', function(){
    return {
        maxHeight:function () {
                var h = document.documentElement.clientHeight -
                    $(".navbar-fixed-top").height() -
                    $(".navbar-fixed-bottom").height() -
                    20; // missing px somewhere :)
                return h;
            },

            maxHeight2:function () {
                var h = document.documentElement.clientHeight -
                    $(".navbar-fixed-top").height() -
                    $(".navbar-fixed-bottom").height() -
                    $(".title").height() -
                    20; // missing px somewhere :)
                return h;
            },

            setHeights:function () {
                var h = $(".height");
                h.height(this.maxHeight());
                h[0].style.overflowY = "auto";
                h[0].style.overflowX = "hidden";

                h = $(".height2");
                h.height(this.maxHeight2());
                h[0].style.overflowY = "auto";
                h[0].style.overflowX = "hidden";
            }
    };
});