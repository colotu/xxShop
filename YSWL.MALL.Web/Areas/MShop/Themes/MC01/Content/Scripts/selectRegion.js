/*
* File:        selectnode.js
* Author:      yaoyuan@ys56.com
* Copyright © 2004-2013 YSWL. All Rights Reserved.
*/
;
(function() {
    var currenScript = $('[src$="selectRegion.js"]').last(); //锁定脚本引用
    var currentBaseSelectGuid = $.Guid.New(); //生成闭包GUID 唯一标识
    //给隐藏域设置GUID属性 以便闭包内脚本跟踪
    var currhfSelectedNode = $('[id$=hfSelectedRegion]').last().attr('HFID', currentBaseSelectGuid);
    var currentIsNull,currentHandleName;
    if (currenScript.length == 1 && currenScript.attr('isnull') && currenScript.attr('handle')) {
        currentIsNull = currenScript.attr('isnull'); //
        currentHandleName = currenScript.attr('handle'); //获取Handler
    } else if(currhfSelectedNode.length == 1 && currhfSelectedNode.attr('isnull') && currhfSelectedNode.attr('handle')) {
        currentIsNull = currhfSelectedNode.attr('isnull'); //
        currentHandleName = currhfSelectedNode.attr('handle'); //获取Handler
    } else {
        //Default
        currentIsNull = 'true';
        currentHandleName = '/RegionHandle.aspx';
    }
    $(document).ready(function() {
        var hfSelectedVal = $('[HFID=' + currentBaseSelectGuid + ']').val();
        if (hfSelectedVal && hfSelectedVal != "0") {
            //逆向加载Select
            InitLastSelect();
        } else {
            //正向加载Select
            InitFirstSelect();
        }
    });

    //逆向初始化第一个下拉列表

    function InitLastSelect() {
       // var isSelectNodeNull = (currentIsNull == 'true');
        //如果隐藏域有值 回传或需初次加载 并选中 ---重点
        var titleContent = "";
        if (!$('[HFID=' + currentBaseSelectGuid + ']').val()) return; //当下拉无值时终止
        $.ajax({
            url: currentHandleName,
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            async: false,
            data: { Action: "GetParentNode", NodeId: $('[HFID=' + currentBaseSelectGuid + ']').val() },
            success: function(resultData) {
                switch (resultData.STATUS) {
                case "OK":
                    //循环添加第一级select
                    for (var n = 0; n < resultData.DATA.length; n++) {
                        var baseSelect;
                        titleContent = '<li>请选择</li>';
                        baseSelect = $('<ol class="dsn"></ol>');
                        //循环添加子集
                        for (var j = 0; j < resultData.DATA[n].length; j++) {
                            //获取项的文本，值
                            var value, name, index = 0;
                            for (var key in resultData.DATA[n][j]) {
                                //resultData.DATA[n]返回值第一个为 项的值，第二个为显示文本
                                if (index == 1) {
                                    value = resultData.DATA[n][j][key];
                                }
                                if (index == 3) {
                                    name = resultData.DATA[n][j][key];
                                }
                                index++;
                            }
                            var k = n;
                            if (n < (resultData.DATA.length - 1)) {
                                k = n + 1;
                            }
                            //是否选中
                            if (value == resultData.PARENT[k] || value == currhfSelectedNode.val()) {
                                titleContent = '<li>' + name + '</li>';
                                $('<li  regionId=' + value + ' class="selected" >' + name + '</li>').appendTo(baseSelect);
                            } else {
                                $('<li  regionId=' + value + '  >' + name + '</li>').appendTo(baseSelect);
                            }
                        }

                        $('[HFID=' + currentBaseSelectGuid + ']').parent().append(baseSelect);
                        //选中值改变，正向加载子集
                        baseSelect.children().on('click',function () {
                            LoadChildNodeData(this);
                        });
                        //添加标题  已选中的内容 
                        $('#baseTitle').append(titleContent);
                    }

                    //选中最后一个
                    $('#baseTitle li:last').addClass('sel_address');
                    $('#baseTitle').nextAll().last().show();
                    break;
                default:
                    break;
                }
            },
            error: function(xmlHttpRequest, textStatus, errorThrown) {
                alert(xmlHttpRequest.responseText);
            }
        });
    }

    //正向初期化第一个下拉列表

    function InitFirstSelect() {
        //第一个下拉列表
        var baseSelect = $("<ol id='" + currentBaseSelectGuid + "'></ol>");
        $.ajax({
            url: currentHandleName,
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: { Action: "GetDepthNode" },
            async: false,
            success: function(resultData) {
                switch (resultData.STATUS) {
                case "OK":
                    $(resultData.DATA).each(function() {
                        baseSelect.append("<li regionId='" + this.RegionId + "'>" + this.RegionName + "</li>");
                    });
                    break;
                default:
                    break;
                }
            },
            error: function(xmlHttpRequest, textStatus, errorThrown) {
                alert(xmlHttpRequest.responseText);
            }
        });

        //将动态生成的Select加载到隐藏域内
        $('[HFID=' + currentBaseSelectGuid + ']').parent().append(baseSelect);
        //添加标题
        $('#baseTitle').append('<li class="sel_address">请选择</li>');

        //选中值改变，正向加载子集
        baseSelect.children().on('click', function () {
            LoadChildNodeData(this);
        });
    }

    //加载子节点数据 追加到父容器内部 递归方法 已完成

    function LoadChildNodeData(send) {
        var rid = $(send).attr('regionId');
        var prid=$(send).parent('ol').prev('ol').find('li.selected').attr('regionId');
        if (rid || !prid) {
            $('[HFID=' + currentBaseSelectGuid + ']').val(rid);
        } else {
            $('[HFID=' + currentBaseSelectGuid + ']').val(prid);
        }
        //获取当前地区列表ol的索引
        var cur_ol_index= $(send).parent('ol').prevAll('ol').length;
        //设置新标题 且 移除后面的标题(ajax内重新获取添加新数据)
        $('#baseTitle li').eq(cur_ol_index).text($(send).text()).nextAll().remove();
        //选中当前 且 移除后面的列表(ajax内重新获取添加新数据)
        $(send).addClass('selected').siblings('.selected').removeClass('selected').parent('ol').nextAll('ol').remove();
        if (!rid) return; //当下拉无值时终止递归
        $.ajax({
            url: currentHandleName,
            type: 'post',
            dataType: 'json',
            timeout: 10000,
            data: { Action: "GetChildNode", ParentId: rid },
            async: false,
            success: function(resultData) {
                switch (resultData.STATUS) {
                case "OK":
                    var baseSelect;
                    var isSelectNodeNull = (currentIsNull == 'true');
                    baseSelect = $("<ol id='" + $.Guid.New() + "'></ol>");
                    $(resultData.DATA).each(function() {
                        var value, name, index = 0;
                        for (var key in this) {
                            if (index == 0) {
                                value = this[key];
                            }
                            if (index == 1) {
                                name = this[key];
                            }
                            index++;
                        }
                        baseSelect.append("<li regionId='" + value + "'>" + name + "</li>");
                    });

                    $('[HFID=' + currentBaseSelectGuid + ']').parent().append(baseSelect);
                    //添加标题
                    $('#baseTitle').append('<li>请选择</li>');

                    //选中值改变，正向加载子集
                    baseSelect.children().on('click', function () {
                        LoadChildNodeData(this);
                    });

                    //选中最后一个标题
                    $('#baseTitle li').removeClass('sel_address').last().addClass('sel_address');
                    //显示最后一列地区
                    $('#baseTitle').nextAll().hide().last().show();

                    //是否立即加载子节点数据
                    if (!isSelectNodeNull) {
                        LoadChildNodeData(baseSelect);
                    }
                    break;
                    default:
                        //无子节点时会走这里
                        completeSetRegion(rid);
                    break;
                }
            },
            error: function(xmlHttpRequest, textStatus, errorThrown) {
                alert(xmlHttpRequest.responseText);
            }
        });
    }

}());