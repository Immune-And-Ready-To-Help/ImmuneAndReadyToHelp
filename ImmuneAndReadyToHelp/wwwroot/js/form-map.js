(function ($) {
    // USE STRICT
    "use strict";

        $(document).ready(function () {

            var selector_map = $('#google_map');
            var latitude = selector_map.attr('latitude');
            var longitude = selector_map.attr('longitude');
            var scrollwhell = selector_map.attr('data-scrollwhell');
            var draggable = selector_map.attr('data-draggable');

            if (latitude == null || longitude == null) {
                latitude = 30.2672;
                longitude = -97.7431;
            }
            if (scrollwhell == null) {
                scrollwhell = 0;
            }
            if (draggable == null) {
                draggable = 0;
            }

            if (selector_map !== undefined) {
                var map = new google.maps.Map(document.getElementById('google_map'), {
                    zoom: 12,
                    scrollwheel: false,
                    zoomControl: false,  
                    disableDoubleClickZoom: true,
                    navigationControl: true,
                    mapTypeControl: false,
                    scaleControl: false,
                    draggable: draggable,
                    center: new google.maps.LatLng(latitude, longitude),
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                });
            }

            var marker = new google.maps.Marker({
                position: new google.maps.LatLng(latitude, longitude),
                map: map
            });
        });

})(jQuery);