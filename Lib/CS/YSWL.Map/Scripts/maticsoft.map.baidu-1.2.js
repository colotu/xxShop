/*
* File:        maticsoft.map.baidu-1.2.js
* Author:      qihq@maticsoft.com, yaoy@maticsoft.com
* Copyright © 2012 YS56. All Rights Reserved.
*/
;
(function () {
    var CurrentOption;
    function baidumapload(option) {
        if (!option.SearchCity && !option.Longitude) return;

        CurrentOption = option;
        var map = new BMap.Map(option.Container); // 创建Map实例
        if (option.SearchCity) {
            //            var local = new BMap.LocalSearch(option.SearchCity, {
            //                renderOptions: {
            //                    map: map,
            //                    autoViewport: true,
            //                    selectFirstResult: false
            //                },
            //                pageCapacity: 8
            //            });
            //            if (option.SearchArea != null || typeof (option.SearchArea) != 'undefined') {
            //                local.search(option.SearchArea);
            //            }
            if (option.SearchArea) {
                map.centerAndZoom(option.SearchArea);
            } else {
                map.centerAndZoom(option.SearchCity);
            }
        }
        if (option.Longitude && option.Dimension) {

            var point = new BMap.Point(option.Longitude, option.Dimension);    // 创建点坐标

            if (option.Level) {
                map.centerAndZoom(point, option.Level);
            }
            else {
                map.centerAndZoom(point, 15);
            }

        }
        if (option.Markers) {
            if (option.Markers.length > 0) {
                for (var i = 0; i < option.Markers.length; i++) {
                    (function (index) {
                        if (!option.Markers[index]) return;
                        var infoWindow;  //弹出框
                        var marker;
                        if (option.MarkerIcon) {
                            var myIcon = new BMap.Icon(option.MarkerIcon, new BMap.Size(28, 37),
                              {
                                  //                                  offset: new BMap.Size(10, 25),
                                  //                                  imageOffset: new BMap.Size(0 - i * 28, 0)
                              });
                            //                          var marker = new BMap.Marker(new BMap.Point(option.Markers[index].Longitude, option.Markers[index].Dimension), { icon: myIcon });  // 创建标注
                            marker = new BMap.Marker(new BMap.Point(option.Markers[index].Longitude, option.Markers[index].Dimension));  // 创建标注
                            marker.setIcon(myIcon);
                        } else {
                            marker = new BMap.Marker(new BMap.Point(option.Markers[index].Longitude, option.Markers[index].Dimension));  // 创建标注
                        }

                        if (option.Markers[index].SetAnimation) {
                            marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
                        }
                        if (option.Markers[index].EnableDragging) {
                            marker.enableDragging(); //开启标注拖拽功能
                        }
                        infoWindow = undefined;
                        map.addOverlay(marker); // 将标注添加到地图中
                        if (option.Markers[index].Window.Content) {
                            infoWindow = new BMap.InfoWindow(option.Markers[index].Window.Content);   // 创建信息窗口对象
                        }
                        map.addControl(new BMap.OverviewMapControl());              //添加缩略地图控件
                        //                    默认点击都是加载窗口的
                        marker.addEventListener("click", function () {
                            var tmpInfo = infoWindow;
                            this.openInfoWindow(tmpInfo);
                            //图片加载完毕重绘infowindow
                            tmpInfo.redraw();
                        });

                        if (option.Markers[index].Window.LoadEvent
                     || option.Markers[index].Window.LoadEvent == "MapLoaded") // 
                        {
                            marker.openInfoWindow(infoWindow);
                            //图片加载完毕重绘infowindow
                            infoWindow.redraw();
                        }
                        else {
                            marker.addEventListener(option.Markers[index].Window.LoadEvent, function () {
                                this.openInfoWindow(infoWindow);
                                //图片加载完毕重绘infowindow
                                infoWindow.redraw();
                            });
                        }
                    }(i));
                }
            }
        }

        if (option.MenuItem) {
            (function() {
                var contextMenu = new BMap.ContextMenu();
                var txtMenuItem = [
                    {
                        text: '放大',
                        callback: function() { map.zoomIn(); }
                    },
                    {
                        text: '缩小',
                        callback: function() { map.zoomOut(); }
                    },
//                    {
//                        text: '放置到最大级',
//                        callback: function() { map.setZoom(18); }
//                    },
//                    {
//                        text: '查看全国',
//                        callback: function() { map.setZoom(5); }
//                    },
                    {
                        text: '在此添加标注',
                        callback: function(p) {
                            if (CurrentOption.EnableOnlyMarker) {
                                map.clearOverlays();
                            }
                            var marker = new BMap.Marker(p), px = map.pointToPixel(p);
                            if (option.MenuItem.MenuItemsetPoint.SetAnimation) {
                                marker.setAnimation(BMAP_ANIMATION_DROP); //跳动的动画
                            }
                            if (option.MenuItem.MenuItemsetPoint.EnableDragging) {
                                marker.enableDragging(true); //开启标注拖拽功能
                            }
                            map.addOverlay(marker);
                            if (option.MenuItem.MenuItemsetPoint.MenuEvent &&
                                option.MenuItem.MenuItemsetPoint.CallBack) {
                                marker.addEventListener('dragend', function(e) {
                                    option.MenuItem.MenuItemsetPoint.CallBack(e.point.lng, e.point.lat);
                                });
                                option.MenuItem.MenuItemsetPoint.CallBack(p.lng, p.lat);
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
            }());
        }


        map.addEventListener("tilesloaded", function () {
            if (option.NavigationControl ) {
                map.addControl(new BMap.NavigationControl());  // 启用鱼骨头。
            }
            if (option.EnableScrollWheelZoom ) {
                map.enableScrollWheelZoom();                  // 启用滚轮放大缩小。
            }
            if (option.EnableKeyboard) {
                map.enableKeyboard();                         // 启用键盘操作。
            }

            if (option.ScaleControl ) {
                map.addControl(new BMap.ScaleControl());           // 启用比例尺。
            }
            if (option.MapTypeControl ) {
                map.addControl(new BMap.MapTypeControl());                 // 是否启用卫星地图等等。
            }
             
            //版权说明
            var copy = new BMap.CopyrightControl(BMAP_ANCHOR_BOTTOM_RIGHT,new BMap.Size(15,15));
            copy.addCopyright(new BMap.Copyright(1, "@ 2012 云商未来 (北京) 科技有限公司", map.getBounds()));
            map.addControl(copy);
        });

        ////////--------
        if (option.addEventListener) {
            if (option.addEventListener.event && option.addEventListener.callback) {
                map.addEventListener(option.addEventListener.event, function (e) {
                    option.addEventListener.callback(e.point.lng, e.point.lat);
                });
            }
        }

//        map.setMinZoom(4);  //设置地图允许的最小级别
    }

    window['baidumap'] = {};
    window['baidumap']['baidumapload'] = baidumapload;

}());