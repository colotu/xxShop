/*
* File:        maticsoft.map.baidu-1.5.js
* Author:      qihq@maticsoft.com, yaoy@maticsoft.com
* Copyright © 2012 YS56. All Rights Reserved.
*/
;
(function() {
    var CurrentOption;

    function baidumapload(option) {
        CurrentOption = option;

        if (!CurrentOption.SearchCity && !CurrentOption.Longitude) return;

        var map = new BMap.Map(CurrentOption.Container); // 创建Map实例
        if (CurrentOption.SearchCity) {
            //            var local = new BMap.LocalSearch(CurrentOption.SearchCity, {
            //                renderCurrentOptions: {
            //                    map: map,
            //                    autoViewport: true,
            //                    selectFirstResult: false
            //                },
            //                pageCapacity: 8
            //            });
            //            if (CurrentOption.SearchArea != null || typeof (CurrentOption.SearchArea) != 'undefined') {
            //                local.search(CurrentOption.SearchArea);
            //            }
            if (CurrentOption.SearchArea) {
                map.centerAndZoom(CurrentOption.SearchArea);
            } else {
                map.centerAndZoom(CurrentOption.SearchCity);
            }
        }
        if (CurrentOption.Longitude && CurrentOption.Dimension) {

            var point = new BMap.Point(CurrentOption.Longitude, CurrentOption.Dimension); // 创建点坐标

            if (CurrentOption.Level) {
                map.centerAndZoom(point, CurrentOption.Level);
            } else {
                map.centerAndZoom(point, 15);
            }

        }
        if (CurrentOption.Markers) {
            if (CurrentOption.Markers.length > 0) {
                for (var i = 0; i < CurrentOption.Markers.length; i++) {
                    (function(index) {
                        if (!CurrentOption.Markers[index]) return;
                        var infoWindow; //弹出框
                        var marker;
                        if (CurrentOption.MarkerIcon) {
                            var myIcon = new BMap.Icon(CurrentOption.MarkerIcon, new BMap.Size(28, 37),
                                {
                                    
                                //                                  offset: new BMap.Size(10, 25),
                                //                                  imageOffset: new BMap.Size(0 - i * 28, 0)
                                });
                            //                          var marker = new BMap.Marker(new BMap.Point(CurrentOption.Markers[index].Longitude, CurrentOption.Markers[index].Dimension), { icon: myIcon });  // 创建标注
                            marker = new BMap.Marker(new BMap.Point(CurrentOption.Markers[index].Longitude, CurrentOption.Markers[index].Dimension)); // 创建标注
                            marker.setIcon(myIcon);
                        } else {
                            marker = new BMap.Marker(new BMap.Point(CurrentOption.Markers[index].Longitude, CurrentOption.Markers[index].Dimension)); // 创建标注
                        }

                        if (CurrentOption.Markers[index].SetAnimation) {
                            marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
                        }
                        if (CurrentOption.Markers[index].EnableDragging) {
                            marker.enableDragging(); //开启标注拖拽功能
                        }
                        infoWindow = undefined;
                        map.addOverlay(marker); // 将标注添加到地图中
                        if (CurrentOption.Markers[index].Window.Content) {
                            infoWindow = new BMap.InfoWindow(CurrentOption.Markers[index].Window.Content); // 创建信息窗口对象
                        }
                        map.addControl(new BMap.OverviewMapControl()); //添加缩略地图控件
                        //                    默认点击都是加载窗口的
                        marker.addEventListener("click", function() {
                            var tmpInfo = infoWindow;
                            this.openInfoWindow(tmpInfo);
                            //图片加载完毕重绘infowindow
                            tmpInfo.redraw();
                        });

                        if (CurrentOption.Markers[index].Window.LoadEvent
                            || CurrentOption.Markers[index].Window.LoadEvent == "MapLoaded") // 
                        {
                            marker.openInfoWindow(infoWindow);
                            //图片加载完毕重绘infowindow
                            infoWindow.redraw();
                        } else {
                            marker.addEventListener(CurrentOption.Markers[index].Window.LoadEvent, function() {
                                this.openInfoWindow(infoWindow);
                                //图片加载完毕重绘infowindow
                                infoWindow.redraw();
                            });
                        }
                    }(i));
                }
            }
        }

        if (CurrentOption.MenuItem) {
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
                            if (CurrentCurrentOption.EnableOnlyMarker) {
                                map.clearOverlays();
                            }
                            var marker = new BMap.Marker(p), px = map.pointToPixel(p);
                            if (CurrentOption.MenuItem.MenuItemsetPoint.SetAnimation) {
                                marker.setAnimation(BMAP_ANIMATION_DROP); //跳动的动画
                            }
                            if (CurrentOption.MenuItem.MenuItemsetPoint.EnableDragging) {
                                marker.enableDragging(true); //开启标注拖拽功能
                            }
                            map.addOverlay(marker);
                            if (CurrentOption.MenuItem.MenuItemsetPoint.MenuEvent &&
                                CurrentOption.MenuItem.MenuItemsetPoint.CallBack) {
                                marker.addEventListener(CurrentOption.MenuItem.MenuItemsetPoint.MenuEvent, function(e) {
                                    CurrentOption.MenuItem.MenuItemsetPoint.CallBack(e.point.lng, e.point.lat);
                                });
                                if (CurrentOption.MenuItem.MenuItemsetPoint.MenuClickCallBack) {
                                    CurrentOption.MenuItem.MenuItemsetPoint.MenuClickCallBack(p.lng, p.lat);
                                }
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


        map.addEventListener("tilesloaded", function() {
            if (CurrentOption.NavigationControl) {
                map.addControl(new BMap.NavigationControl()); // 启用鱼骨头。
            }
            if (CurrentOption.EnableScrollWheelZoom) {
                map.enableScrollWheelZoom(); // 启用滚轮放大缩小。
            }
            if (CurrentOption.EnableKeyboard) {
                map.enableKeyboard(); // 启用键盘操作。
            }

            if (CurrentOption.ScaleControl) {
                map.addControl(new BMap.ScaleControl()); // 启用比例尺。
            }
            if (CurrentOption.MapTypeControl) {
                map.addControl(new BMap.MapTypeControl()); // 是否启用卫星地图等等。
            }

            //版权说明
            var copy = new BMap.CopyrightControl(BMAP_ANCHOR_BOTTOM_RIGHT, new BMap.Size(15, 15));
            copy.addCopyright(new BMap.Copyright(1, "@ 2012 云商未来 (北京) 科技有限公司", map.getBounds()));
            map.addControl(copy);
        });

        ////////--------
        if (CurrentOption.addEventListener) {
            if (CurrentOption.addEventListener.event && CurrentOption.addEventListener.callback) {
                map.addEventListener(CurrentOption.addEventListener.event, function(e) {
                    CurrentOption.addEventListener.callback(e.point.lng, e.point.lat);
                });
            }
        }

//        map.setMinZoom(4);  //设置地图允许的最小级别
    }

    window['baidumap'] = { };
    window['baidumap']['baidumapload'] = baidumapload;

}());