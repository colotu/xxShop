/*
* File:        maticsoft.map.baidu-1.1.js
* Author:      qihq@maticsoft.com
* Copyright © 2012 YS56. All Rights Reserved.
*/
;
(function () {
    function baidumapload(option) {
        var map = new BMap.Map(option.container); // 创建Map实例
        if (option.SearchCity != null || typeof (option.SearchCity) != 'undefined') {
            //            var local = new BMap.LocalSearch(option.SearchCity, {
            //                renderOptions: {
            //                    map: map,
            //                    autoViewport: true,
            //                    selectFirstResult: false
            //                },
            //                pageCapacity: 8
            //            });
            //            if (option.searchArea != null || typeof (option.searchArea) != 'undefined') {
            //                local.search(option.searchArea);
            //            }
            if (option.searchArea != null || typeof (option.searchArea) != 'undefined') {
                map.centerAndZoom(option.searchArea);
            } else {
                map.centerAndZoom(option.SearchCity);
            }
        }
        if ((option.Longitude != null || typeof (option.Longitude) != 'undefined') && (option.dimension != null || typeof (option.dimension) != 'undefined')) {

            var point = new BMap.Point(option.Longitude, option.dimension);    // 创建点坐标

            if (option.Level != null || typeof (option.Level) != 'undefined') {
                map.centerAndZoom(point, option.Level);
            }
            else {

                map.centerAndZoom(point, 15);
            }

        }
        if (option.Markers != null || typeof (option.Markers) != 'undefined') {
            if (option.Markers.length > 0) {
                for (var i = 0; i < option.Markers.length; i++) {
                    (function (send) {
                        var infoWindow;  //弹出框  
                        var marker;
                        if (option.markerIcon != undefined) {
                            var myIcon = new BMap.Icon(option.markerIcon, new BMap.Size(28, 37),
                              {
                                  //                                  offset: new BMap.Size(10, 25),
                                  //                                  imageOffset: new BMap.Size(0 - i * 28, 0)
                              });
                            //                          var marker = new BMap.Marker(new BMap.Point(option.Markers[i].Longitude, option.Markers[i].dimension), { icon: myIcon });  // 创建标注
                            marker = new BMap.Marker(new BMap.Point(option.Markers[i].Longitude, option.Markers[i].dimension));  // 创建标注
                            marker.setIcon(myIcon);
                        } else {
                            marker = new BMap.Marker(new BMap.Point(option.Markers[i].Longitude, option.Markers[i].dimension));  // 创建标注
                        }


                        if (option.Markers[i].setAnimation == "true") {
                            marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
                        }
                        if (option.Markers[i].enableDragging == "true") {
                            marker.enableDragging(true); //跳动的动画
                        }
                        infoWindow = undefined;
                        map.addOverlay(marker); // 将标注添加到地图中
                        if (option.Markers[i].Window.Content != null || typeof (option.Markers[i].Window.Content) != 'undefined') {
                            infoWindow = new BMap.InfoWindow(option.Markers[i].Window.Content);   // 创建信息窗口对象
                        }
                        map.addControl(new BMap.OverviewMapControl());              //添加缩略地图控件
                        //                    默认点击都是加载窗口的
                        marker.addEventListener("click", function () {
                            var tmpInfo = infoWindow;
                            this.openInfoWindow(tmpInfo);
                            //图片加载完毕重绘infowindow
                            tmpInfo.redraw();


                        });

                        if ((option.Markers[i].Window.LoadEvent == null || typeof (option.Markers[i].Window.LoadEvent) == 'undefined')
                     || option.Markers[i].Window.LoadEvent == "MapLoaded") // 
                        {
                            marker.openInfoWindow(infoWindow);
                            //图片加载完毕重绘infowindow
                            infoWindow.redraw();
                        }
                        else {
                            marker.addEventListener(option.Markers[i].Window.LoadEvent, function () {
                                this.openInfoWindow(infoWindow);
                                //图片加载完毕重绘infowindow
                                infoWindow.redraw();


                            });
                        }


                    })(i);
                }
            }

        }


        if (option.MenuItem != null && typeof (option.MenuItem) != 'undefined') {
            var contextMenu = new BMap.ContextMenu();
            var txtMenuItem = [
                    {
                        text: '放大',
                        callback: function () { map.zoomIn() }
                    },
                    {
                        text: '缩小',
                        callback: function () { map.zoomOut() }
                    },
                    {
                        text: '放置到最大级',
                        callback: function () { map.setZoom(18) }
                    },
                    {
                        text: '查看全国',
                        callback: function () { map.setZoom(4) }
                    },
                    {
                        text: '在此添加标注',
                        callback: function (p) {
                            var marker = new BMap.Marker(p), px = map.pointToPixel(p);
                            if (option.MenuItem.MenuItemsetPoint.setAnimation == "true") {
                                marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
                            }
                            if (option.MenuItem.MenuItemsetPoint.enableDragging == "true") {
                                marker.enableDragging(true); //跳动的动画
                            }
                            map.addOverlay(marker);
                            if (option.MenuItem.MenuItemsetPoint.MenuEvent != null &&
                             typeof (option.MenuItem.MenuItemsetPoint.MenuEvent) != 'undefined' &&
                            option.MenuItem.MenuItemsetPoint.CallBack != null &&
                            typeof (option.MenuItem.MenuItemsetPoint.CallBack) != 'undefined') {
                                marker.addEventListener("dblclick", function (e) {
                                    option.MenuItem.MenuItemsetPoint.CallBack(e.point.lng, e.point.lat)
                                }
                                );
                            }
                        }
                    }
                    ];
            for (var i = 0; i < txtMenuItem.length; i++) {
                contextMenu.addItem(new BMap.MenuItem(txtMenuItem[i].text, txtMenuItem[i].callback, 100));
                if (i == 1 || i == 3) {
                    contextMenu.addSeparator();
                }
            }
            map.addContextMenu(contextMenu);
        }



        map.addEventListener("tilesloaded", function () {

            if (option.NavigationControl == "true") {

                map.addControl(new BMap.NavigationControl());  // 启用鱼骨头。
            }
            if (option.enableScrollWheelZoom == "true") {

                map.enableScrollWheelZoom();                  // 启用滚轮放大缩小。
            }
            map.disableScrollWheelZoom();
            if (option.enableKeyboard != null || typeof (option.enableKeyboard) != 'undefined') {
                map.enableKeyboard();                         // 启用键盘操作。
            }

            if (option.ScaleControl == "true") {

                map.addControl(new BMap.ScaleControl());           // 启用比例尺。
            }
            if (option.MapTypeControl == "true") {

                map.addControl(new BMap.MapTypeControl());                 // 是否启用卫星地图等等。
            }


        });

        ////////--------
        if (option.addEventListener != null || typeof (option.addEventListener) != 'undefined') {
            if ((option.addEventListener.event != null ||
             typeof (option.addEventListener.event) != 'undefined') && (option.addEventListener.callback != null
          || typeof (option.addEventListener.callback) != 'undefined')) {
                map.addEventListener(option.addEventListener.event, function (e) {
                    option.addEventListener.callback(e.point.lng, e.point.lat);

                });
            }
        }
    }

    window['baidumap'] = {};
    window['baidumap']['baidumapload'] = baidumapload;

})();