/*
* File:        maticsoft.waterfall.js
* Author:      yaoyuan@ys56.com
* Copyright © 2004-2013 YSWL. All Rights Reserved.
*/
/**
* //TODO: 瀑布流待解决的缺陷问题 BEN ADD 20130503
* 1. 图片加载后调用个算法, 重排已加载队列的div, 界面可能会有闪现的情况, 可用淡出特效解决.
* 2. 集中加载, 给用户等待提示, 未完全加载的内容不予显示, 等完全加载后(已知高度时)显示出来.
*/
;
$(function () {
    waterFall.ScrollTop = document.documentElement.scrollTop || document.body.scrollTop;
});
var waterFall = {
    AjaxOptions: {
        Type: "POST",
        Params: {},
        DataURL: null,
        Async: true,       //必须同步请求, 保证线程安全, *)目前异步模式测试中, 非线程安全 在特殊情况下会出现BUG BEN ADD 2012-11-02
        DataType: "html",
        Success: null,
        Error: null
    },
    CurrentAjaxStartIndexHF: null,
    StartIndex: 0,
    EndIndex: 0,

    PagedContainer: null,
    ColumnFirstId: null,
    ColumnNumber: 3,
    RowNumber: 0,
    LoadDataCount: 0,

    ScrollTop: 0,

    LoadFinish: false,

    ScrollTrigger: 100, //为提高性能，建议滚动前后距离大于100像素再处理

    // 滚动加载
    scroll: function () {
        $(window).scroll(function () {
            // 为提高性能，建议滚动前后距离大于100像素再处理
            var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
            if (!waterFall.LoadFinish && Math.abs(scrollTop - waterFall.ScrollTop) > waterFall.ScrollTrigger) {
                waterFall.ScrollTop = scrollTop;
                waterFall.appendDetect();
            }
        });
        return this;
    },

    // 是否滚动载入的检测
    appendDetect: function () {
        var eleColumn;
        if (waterFall.ColumnNumber) {
            var start = 0;
            for (start; start < waterFall.ColumnNumber; start++) {
                eleColumn = document.getElementById(waterFall.ColumnFirstId + start);
                if (eleColumn && !waterFall.LoadFinish) {
                    if (eleColumn.offsetTop + eleColumn.clientHeight < waterFall.ScrollTop + (window.innerHeight || document.documentElement.clientHeight)) {
                        waterFall.append(eleColumn);
                    }
                }
            }
        } else {
            //整行加载模式 连续追加指定数量数据
            eleColumn = document.getElementById(waterFall.ColumnFirstId);
            if (eleColumn && !waterFall.LoadFinish) {
                if (eleColumn.offsetTop + eleColumn.clientHeight < waterFall.ScrollTop + (window.innerHeight || document.documentElement.clientHeight)) {
                    waterFall.append(eleColumn);
                    if (waterFall.RowNumber && waterFall.RowNumber > 1) {
                        for (var rowIndex = 2; rowIndex < waterFall.RowNumber; rowIndex++) {
                            waterFall.append(eleColumn);
                        }
                    }
                }
            }
        }
        return this;
    },

    // 滚动载入
    append: function (column) {
        if (waterFall.StartIndex >= waterFall.EndIndex) {
            waterFall.LoadFinish = true;
            waterFall.PagedContainer.show();
            return this;
        }
        var baseData = { StartIndex: waterFall.StartIndex };

        //异步线程安全临时解决方案
        waterFall.LoadDataCount += 1;

        if ($('#hfCurrentPageAjaxSize').length > 0 ) {
            waterFall.StartIndex = waterFall.StartIndex + parseInt($('#hfCurrentPageAjaxSize').val());
            waterFall.CurrentAjaxStartIndexHF.val(waterFall.StartIndex);
        } else {//兼容之前的
            waterFall.CurrentAjaxStartIndexHF.val(++waterFall.StartIndex);
        }
        $.ajax({
            type: waterFall.AjaxOptions.Type,
            url: waterFall.AjaxOptions.DataURL,
            async: waterFall.AjaxOptions.Async,
            dataType: waterFall.AjaxOptions.DataType,
            data: $.extend(waterFall.AjaxOptions.Params, baseData),
            success: function (data) {
                if (!data) {
                    //减少无效请求, 一次无数据后停止瀑布流
                    waterFall.LoadFinish = true;
                    waterFall.PagedContainer.show();
                    return;
                }
                var jqData = $($.parseHTML(data));
                //                waterFall.LoadDataCount += 1;
                //                waterFall.CurrentAjaxStartIndexHF.val(++waterFall.StartIndex);
                //                console.debug("Waterfall load data Index:" + waterFall.StartIndex);
                $(column).append(jqData);

                if (waterFall.AjaxOptions.Success) {
                    waterFall.AjaxOptions.Success.call(jqData);
                }
            },
            error: function (event, XMLHttpRequest, ajaxOptions, thrownError) {
                // thrownError 只有当异常发生时才会被传递 this;
                if (waterFall.AjaxOptions.Error) {
                    waterFall.AjaxOptions.Error.call(event, XMLHttpRequest, ajaxOptions, thrownError);
                }
            }
        });
        if (waterFall.StartIndex >= waterFall.EndIndex) {
            waterFall.LoadFinish = true;
            waterFall.PagedContainer.show();
            return this;
        }
        return this;
    },

    refresh: function () {
        // 检测
        waterFall.appendDetect();
        return this;
    },

    // 浏览器窗口大小变换
    resize: function () {
        window.onresize = function () {
            waterFall.refresh();
        };
        return this;
    },

    init: function (options) {
        //默认配置
        var defaultOptions = {
            //Ajax请求参数
            AjaxOptions: {
                //Ajax请求参数
                Params: { AlbumId: $.getUrlParam('AlbumID') },
                //Ajax请求URL
                DataURL: "WaterfallPhotoListData"
            },
            //瀑布流起始索引记录器 - 用于累计
            CurrentAjaxStartIndexHF: $('#hfCurrentPageAjaxStartIndex'),
            //瀑布流起始索引
            StartIndex: $('#hfCurrentPageAjaxStartIndex').val() ? parseInt($('#hfCurrentPageAjaxStartIndex').val()) : 0,
            //瀑布流结束索引
            EndIndex: $('#hfCurrentPageAjaxEndIndex').val() ? parseInt($('#hfCurrentPageAjaxEndIndex').val()) : 0,

            //分页容器
            PagedContainer: $('.in_pages'),
            //列容器ID前缀
            ColumnFirstId: "col_",
            //列数 : 0 特殊模式 整行加载 功能完善中
            ColumnNumber: 3,
            //列数:0 时, 启用此参数, 整行加载功能, 每行加载数量
            RowNumber: 0,
            //加载灵敏度  建议滚动前后距离大于100像素再处理
            ScrollTrigger: 100
        };

        if (options)
            defaultOptions = $.extend(defaultOptions, options);

        //默认隐藏分页控件
        defaultOptions.PagedContainer.hide();

        //Safe
        if (defaultOptions.CurrentAjaxStartIndexHF.length == 0) return;
        if (defaultOptions.StartIndex == 0 || defaultOptions.EndIndex == 0) return;
        //列数 : 0 特殊模式 整行加载 功能完善中
        //        if (!document.getElementById(defaultOptions.ColumnFirstId + '0')) return;

        //初始化
        this.CurrentAjaxStartIndexHF = defaultOptions.CurrentAjaxStartIndexHF;
        this.ColumnFirstId = defaultOptions.ColumnFirstId;
        this.ColumnNumber = defaultOptions.ColumnNumber;
        this.RowNumber = defaultOptions.RowNumber;
        this.PagedContainer = defaultOptions.PagedContainer;
        this.StartIndex = defaultOptions.StartIndex;
        this.EndIndex = defaultOptions.EndIndex;
        this.AjaxOptions = $.extend(this.AjaxOptions, defaultOptions.AjaxOptions);

        if (this.StartIndex && this.ColumnFirstId) {
            $(document).scrollTop(0);
            this.LoadFinish = false;
            this.scroll().refresh().resize();
        }
    }
};
