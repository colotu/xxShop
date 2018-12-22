
//var stop = true;
////获取滚动条当前的位置
//function getScrollTop() {
//    var scrollTop = 0;
//    if (document.documentElement && document.documentElement.scrollTop) {
//        scrollTop = document.documentElement.scrollTop;
//    }
//    else if (document.body) {
//        scrollTop = document.body.scrollTop;
//    }
//    return scrollTop;
//}
////获取当前可视范围的高度
//function getClientHeight() {
//    var clientHeight = 0;
//    if (document.body.clientHeight && document.documentElement.clientHeight) {
//        clientHeight = Math.min(document.body.clientHeight, document.documentElement.clientHeight);
//    }
//    else {
//        clientHeight = Math.max(document.body.clientHeight, document.documentElement.clientHeight);
//    }
//    return clientHeight;
//}
////获取文档完整的高度
//function getScrollHeight() {
//    return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
//}

//window.onscroll = function () {
//    if (getScrollTop() + getClientHeight() == getScrollHeight()) {
//        //ajax从这里开始
//        if (stop == true) {
//            stop = false;
//            $('.storeIndex ').append('<div id="loading" style="width: 100%;margin:10px auto 5px;text-align: center;"><img style="width: 20px; float: left;margin-left: 35%;" src="images/loading.gif"/><span style="margin-top: -2px;float: left;margin-left: 10px;color: #ccc">加载中...</span></div>');
//        } else {
//            //stop = true;
//            $("#loading").html(' ');
//            var pl = $("#productList li").eq(1).html();
//            $('.storeIndex ').append(pl);
 
//        }
//        //alert('没有了，别滚了');
//    }
//}




/*
* File:        maticsoft.pullUp.js
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
    pullUp.ScrollTop = document.documentElement.scrollTop || document.body.scrollTop;
    pullUp.scroll();
});
var pullUp = {
    AjaxOptions: {
        Type: "POST",
        Params: {},
        DataURL: null,
        Async: false,       
        DataType: "html",
        Success: null,
        Error: null
    },
    //当前页码
    CurrentPage: 1,
    //页码参数名称
    PageParamsName: "pageIndex",
    //每页数量
    PageSize: 30,
    //每页数量参数名称
    PageSizeParamsName: "pageSize",
    //内容容器
    ContentContainer: null,
    //正在加载提示框
    LoadingTipBox: null,
    //无更多数据提示框
    NoMoreTipBox: null,
    ScrollTop: 0,
    LoadFinish: false,
    //是否加载  当页面弹层及加载其它内容时，上拉需要加载时使用，当层关闭时需要将此值设为true,否则无法继续加载
    Load:true,
    ScrollTrigger: 100, //为提高性能，建议滚动前后距离大于100像素再处理

    // 滚动加载
    scroll: function () {  
        $(window).scroll(function () {
             //为提高性能，建议滚动前后距离大于100像素再处理
            var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
            if (!pullUp.LoadFinish && pullUp.Load && Math.abs(scrollTop - pullUp.ScrollTop) > pullUp.ScrollTrigger) {
                pullUp.ScrollTop = scrollTop;
                pullUp.append();
            }
        });
        //window.onscroll = function () {
        //    if (getScrollTop() + getClientHeight() == getScrollHeight()) {
        //        //ajax从这里开始          
        //            //pullUp.ScrollTop = scrollTop;
        //        pullUp.append();
        //        //alert('没有了，别滚了');
        //    }
        //}
        return this;
    },
 
    //载入
    append: function () {
        if(pullUp.LoadFinish){
            return;
        }
        if(pullUp.CurrentPage<=0){
            pullUp.CurrentPage = 1;
        }
        var pageName=pullUp.PageParamsName;
        var pageSizeName = pullUp.PageSizeParamsName;
        var baseDataStr = '{{0}:{1},{2}:{3}}';
        var baseData = eval("(" + baseDataStr.format(pageName, pullUp.CurrentPage, pageSizeName, pullUp.PageSize) + ")");
        if (pullUp.LoadingTipBox!=null &&  pullUp.LoadingTipBox.length > 0){
            pullUp.LoadingTipBox.show();
        }
        $.ajax({
            type: pullUp.AjaxOptions.Type,
            url: pullUp.AjaxOptions.DataURL,
            async: pullUp.AjaxOptions.Async,
            dataType: pullUp.AjaxOptions.DataType,
            data: $.extend(pullUp.AjaxOptions.Params, baseData),
            success: function (data) {
                if (pullUp.LoadingTipBox != null &&  pullUp.LoadingTipBox.length > 0) {
                    pullUp.LoadingTipBox.hide();
                }
                if (!$.trim(data)) {
                    //减少无效请求, 一次无数据后停止加载
                    pullUp.LoadFinish = true;
                    //没有更多了
                    if (pullUp.NoMoreTipBox != null && pullUp.NoMoreTipBox.length > 0) {
                        pullUp.NoMoreTipBox.show();
                    }
                    return;
                }
                var jqData = $($.parseHTML(data));
                pullUp.CurrentPage += 1;
                if (pullUp.ContentContainer!=null) {
                    pullUp.ContentContainer.append(jqData)
                }
            },
            error: function (event, XMLHttpRequest, ajaxOptions, thrownError) {
                // thrownError 只有当异常发生时才会被传递 this;
                if (pullUp.AjaxOptions.Error) {
                    pullUp.AjaxOptions.Error.call(event, XMLHttpRequest, ajaxOptions, thrownError);
                }
            }
        });
        return this;
    },
    init: function (options) {
        //默认配置
        var defaultOptions = {
            //Ajax请求参数
            AjaxOptions: {
                //Ajax请求参数
                Params: {},//示例 cid: 5
                //Ajax请求URL
                DataURL: "ListData"
            },
            //当前页码
            CurrentPage: 1,
            //页码参数名称
            PageParamsName: "pageIndex",
            //每页数量
            PageSize: 30,
            //每页数量参数名称
            PageSizeParamsName: "pageSize",
            //内容容器
            ContentContainer: $('#p_list'),
            //正在加载提示框
            LoadingTipBox: $('#loading'),
            //无更多数据提示框
            NoMoreTipBox: $('#nomore'),
            //是否加载完成
            LoadFinish: false,
            //是否加载  当页面弹层及加载其它内容时，上拉需要加载时使用，当层关闭时需要将此值设为true,否则无法继续加载
            Load:true,
            //加载灵敏度  建议滚动前后距离大于100像素再处理
            ScrollTrigger: 100
        };

        if (options) {
            defaultOptions = $.extend(defaultOptions, options);
        }
         

        //Safe
        //if (defaultOptions.CurrentAjaxStartIndexHF.length == 0) return;
        //if (defaultOptions.StartIndex == 0 || defaultOptions.EndIndex == 0) return;
      
        //初始化
        this.ContentContainer = defaultOptions.ContentContainer;
        this.LoadingTipBox = defaultOptions.LoadingTipBox;
        this.NoMoreTipBox = defaultOptions.NoMoreTipBox;
        this.AjaxOptions = $.extend(this.AjaxOptions, defaultOptions.AjaxOptions);
        //当前页码
        this.CurrentPage=defaultOptions.CurrentPage,
        //页码参数名称
        this.PageParamsName=defaultOptions.PageParamsName,
        //每页数量
        this.PageSize=defaultOptions.PageSize,
        //每页数量参数名称
        this.PageSizeParamsName = defaultOptions.PageSizeParamsName,
        //this.ScrollTop= defaultOptions.ScrollTop,

        //是否加载完成
        this.LoadFinish = defaultOptions.LoadFinish,

        //是否加载
        this.Load = this.Load,
        this.ScrollTop=0
    }
};








