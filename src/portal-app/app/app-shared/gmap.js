var SW = window.SW || {};

(function ($) {
    //var arrLocations = window.arrLocations ||  [
    //  [{
    //      marker: null,
    //      name:	'Alexandra House',
    //      type:	'1',
    //      address:	'7-15 Des Voeux Road Central, Central, Hong Kong',
    //      LatLng:	{
    //          lb:  22.2810133,
    //          mb:  114.1587265
    //      },
    //      icon:		'/images/maps/maker-1.png',
    //      infoBox:	null,
    //      visible:	false
    //  }],
    //  [{
    //      marker: null,
    //      name:	'York House',
    //      type:	'0',
    //      address:	'12-16 Des Voeux Road Central, Central, Hong Kong',
    //      LatLng:	{
    //          lb:  22.281554,
    //          mb:  114.15761799999996
    //      },
    //      icon:		'/images/maps/maker-offices.png',
    //      infoBox:	null,
    //      visible:	false
    //  }]
    //];

    //styledMap = new google.maps.StyledMapType(styles, { name: "Styled Map" });
    //// Enable the visual refresh
    //google.maps.visualRefresh = true;
    //var styles = [
    //  {
    //      "featureType": "administrative.province",
    //      "stylers": [
    //        { "visibility": "off" }
    //      ]
    //  },{
    //      "featureType": "administrative.locality",
    //      "stylers": [
    //        { "visibility": "off" }
    //      ]
    //  },{
    //      "featureType": "landscape.natural",
    //      "stylers": [
    //        { "visibility": "off" },
    //        { "color": "#8080c2" },
    //        { "weight": 0.1 },
    //        { "saturation": 1 },
    //        { "hue": "#66ff00" }
    //      ]
    //  },{
    //      "featureType": "water",
    //      "stylers": [
    //        { "visibility": "on" },
    //        { "hue": "#00ff77" },
    //        { "saturation": -66 },
    //        { "lightness": 17 }
    //      ]
    //  }
    //];
    SW.GMap = {
        apiKey: window.apiKey,
        map: null,
        markers: [],
        refLocation: {},
        geocoder: null,
        centerPos: null,
        initMap: function (address) {
            if (address) {
                var url = 'https://maps.googleapis.com/maps/api/geocode/json?address=' + address + '&key=' + SW.GMap.apiKey;
                $.ajax({
                    method: 'GET',
                    url: url,
                    success: function (data) {
                        console.log(data);
                        SW.GMap.geocoder = new google.maps.Geocoder();
                        if (data.results.length > 0) {
                            SW.GMap.centerPos = data.results[0].geometry.location;
                        }
                        else {
                            SW.GMap.centerPos = { lat: 10.8230989, lng: 106.6296638 };
                        }
                        if ($('#gmap').length > 0) {
                            SW.GMap.map = new google.maps.Map(document.getElementById('gmap'), {
                                zoom: 14,
                                center: SW.GMap.centerPos
                            });
                            SW.GMap.addMarker(SW.GMap.centerPos);
                        }
                        google.maps.event.addListener(SW.GMap.map, 'click', function (event) {
                            var pos = { lat: event.latLng.lat(), lng: event.latLng.lng() };
                            SW.GMap.addMarker(pos);
                        });

                        // Removes the markers from the map, but keeps them in the array.
                        function clearMarkers() {
                            setMapOnAll(null);
                        }
                    },
                    error: function (a, b, c) {
                        console.log(a + " " + b + " " + c);
                    }
                });
            }
        },
        addMarker: function (location) {
            SW.GMap.deleteMarkers();
            var marker = new google.maps.Marker({
                position: location,
                map: SW.GMap.map
            });
            SW.GMap.markers.push(marker);
            if (SW.GMap.refLocation != undefined) {
                SW.GMap.refLocation.Lat = location.lat;
                SW.GMap.refLocation.Lng = location.lng;
            }
        },
        deleteMarkers: function () {
            SW.GMap.clearMakers();
            SW.GMap.markers = [];
        },
        clearMakers: function () {
            SW.GMap.setMapOnAll(null);
        },
        setMapOnAll: function (map) {
            for (var i = 0; i < SW.GMap.markers.length; i++) {
                SW.GMap.markers[i].setMap(map);
            }
        },
        initialize: function () {
            SW.GMap.geocoder = new google.maps.Geocoder();
            if (SW.GMap.map == undefined) {
                var mapOptions = {
                    zoom: initZoom,
                    center: new google.maps.LatLng(10.8230989, 106.6296638),//16.051571, 108.21489700000006),
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                SW.GMap.map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
            }
            SW.GMap.map.mapTypes.set('map_style', styledMap);
            SW.GMap.map.setMapTypeId('map_style');

            setMarkers(arrLocations);
        },
        createMarker: function (location) {
            var myLatLng = new google.maps.LatLng(location.LatLng.lb, location.LatLng.mb);
            location.marker = new google.maps.Marker({
                map: SW.GMap.map,
                position: myLatLng,
                icon: location.icon
            });
            var contentString = '<div id="infoWindow-1" class="infowindow" style="background:#142b39; padding:10px 10px;">' +
                '<div class="info-main">' +
                '</div>' +
                '</div>';

            var myOptions = {
                //content: boxText
                content: contentString
                , disableAutoPan: false
                , maxWidth: 0
                , alignBottom: true
                , pixelOffset: new google.maps.Size(-199, -54)
                , zIndex: null
                , boxStyle: {
                    opacity: 1
                    , width: "398px"
                }
                , closeBoxMargin: "10px 5px"
                , closeBoxURL: "/images/maps/close.gif"
                , infoBoxClearance: new google.maps.Size(1, 1)
                , isHidden: false
                , pane: "floatPane"
                , enableEventPropagation: false
            };

            google.maps.event.addListener(location.marker, "click", function (e) {
                location.visible = false;
                if (location.visible) {
                    location.infoBox.close();
                    location.visible = false;
                } else {
                    closeAllInfoBox(arrLocations);
                    //location.infoBox.open(map, this);
                    /*Update html for modal*/
                    getPlaceDetail(location.name);

                    location.visible = true;
                }
            });

            location.infoBox = new InfoBox(myOptions);

            //open info when create
            //location.infoBox.open(map, location.marker);
            //location.visible = true;

            google.maps.event.addListener(location.infoBox, "closeclick", function (e) {
                location.visible = false;
            });
        },
        getPlaceDetail: function (id) {
            $.ajax({
                type: "POST",
                url: "/Services/DoveTetServices.aspx/GetPlaceDetail",
                data: "{ placeID: " + id + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (rsp) {
                    var result = rsp.d;
                    $("#marker-modal").html(result);
                    if ($('.marker-status').html == 'Đã diễn ra!') {
                        DoveTet.Tracking.trackEvent("button", "click", 'location-detail_da-dien-ra');
                    }
                    else {
                        DoveTet.Tracking.trackEvent("button", "click", 'location-detail_dang-va-sap-dien-ra');
                    }
                    $("#marker-modal").dialog('open');
                }
            });
        },
        setMarkers: function (locations) {
            for (var i = 0; i < locations.length; i++) {
                var location = locations[i][0];
                getLatLngCodeAddress(location.address)
                createMarker(location);
            }
        },

        closeAllInfoBox: function (locations) {
            for (var i = 0; i < locations.length; i++) {
                var location = locations[i][0];
                location.visible = false;
                location.infoBox.close();
            }
        },

        getLatLngCodeAddress: function (address) {
            SW.GMap.geocoder.geocode({ 'address': address }, function (results, status) {
                if (status === 'OK') {
                    var loc = results[0].geometry.location;
                    $('#txt-lat').val(loc.lat);
                    $('#txt-lng').val(loc.lng);
                } else {
                    //alert('Geocode was not successful for the following reason: ' + status);
                }
            });
        }
    }
    $(document).ready(function () {
        //google.maps.event.addDomListener(window, 'load', function () {
        //    initialize();
        //});
    })
}(jQuery));
