<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main_index_dlb.aspx.cs" Inherits="YSWL.MALL.Web.Admin.main_index_dlb" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>商城系统框架</title>
    <script src="/admin/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="/admin/css/admin.css" rel="stylesheet" type="text/css" />
    <link href="/admin/js/easyui/easyui.css" rel="stylesheet" type="text/css" />
    <script src="/admin/js/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function addTab(title, url) {
            if ($('#tabs').tabs('exists', title)) {
                $('#tabs').tabs('select', title); //选中并刷新
                var currTab = $('#tabs').tabs('getSelected');
                var url = $(currTab.panel('options').content).attr('src');
                if (url != undefined && currTab.panel('options').title != '我的桌面') {
                    $('#tabs').tabs('update', {
                        tab: currTab,
                        options: {
                            content: createFrame(url)
                        }
                    });
                }
            } else {
                var index = $('#tabs').find(".tabs").find("li").length;
                if (index == 20) {
                    alert("开启的菜单太多，请先关闭部分菜单！");
                    return;
                }
                var content = createFrame(url);
                $('#tabs').tabs('add', {
                    title: title,
                    content: content,
                    closable: true
                });
            }
            tabClose();
        }
        function createFrame(url) {
            var s = '<iframe scrolling="auto" frameborder="0"  border="0px" src="' + url + '" style="width:100%;height:99%;"></iframe>';
            return s;
        }

        function tabClose() {
            /*双击关闭TAB选项卡*/
            $(".tabs-inner").dblclick(function () {
                var subtitle = $(this).children(".tabs-closable").text();
                $('#tabs').tabs('close', subtitle);
            });
            /*为选项卡绑定右键*/
            $(".tabs-inner").bind('contextmenu', function (e) {
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });

                var subtitle = $(this).children(".tabs-closable").text();

                $('#mm').data("currtab", subtitle);
                $('#tabs').tabs('select', subtitle);
                return false;
            });
        }
        //绑定右键菜单事件
        function tabCloseEven() {
            //刷新
            $('#mm-tabupdate').click(function () {
                var currTab = $('#tabs').tabs('getSelected');
                var url = $(currTab.panel('options').content).attr('src');
                if (url != undefined && currTab.panel('options').title != '我的桌面') {
                    $('#tabs').tabs('update', {
                        tab: currTab,
                        options: {
                            content: createFrame(url)
                        }
                    })
                }
            })
            //关闭当前
            $('#mm-tabclose').click(function () {
                var currtab_title = $('#mm').data("currtab");
                $('#tabs').tabs('close', currtab_title);
            })
            //全部关闭
            $('#mm-tabcloseall').click(function () {
                $('.tabs-inner span').each(function (i, n) {
                    var t = $(n).text();
                    if (t != '我的桌面') {
                        $('#tabs').tabs('close', t);
                    }
                });
            });
            //关闭除当前之外的TAB
            $('#mm-tabcloseother').click(function () {
                var prevall = $('.tabs-selected').prevAll();
                var nextall = $('.tabs-selected').nextAll();
                if (prevall.length > 0) {
                    prevall.each(function (i, n) {
                        var t = $('a:eq(0) span', $(n)).text();
                        if (t != '我的桌面') {
                            $('#tabs').tabs('close', t);
                        }
                    });
                }
                if (nextall.length > 0) {
                    nextall.each(function (i, n) {
                        var t = $('a:eq(0) span', $(n)).text();
                        if (t != '我的桌面') {
                            $('#tabs').tabs('close', t);
                        }
                    });
                }
                return false;
            });
            //关闭当前右侧的TAB
            $('#mm-tabcloseright').click(function () {
                var nextall = $('.tabs-selected').nextAll();
                if (nextall.length == 0) {
                    //msgShow('系统提示','后边没有啦~~','error');
                    alert('后边没有啦~~');
                    return false;
                }
                nextall.each(function (i, n) {
                    var t = $('a:eq(0) span', $(n)).text();
                    $('#tabs').tabs('close', t);
                });
                return false;
            });
            //关闭当前左侧的TAB
            $('#mm-tabcloseleft').click(function () {
                var prevall = $('.tabs-selected').prevAll();
                if (prevall.length == 0) {
                    alert('到头了，前边没有啦~~');
                    return false;
                }
                prevall.each(function (i, n) {
                    var t = $('a:eq(0) span', $(n)).text();
                    $('#tabs').tabs('close', t);
                });
                return false;
            });

            //退出
            $("#mm-exit").click(function () {
                $('#mm').menu('hide');
            });
        }

        $(function () {
            tabCloseEven();
            $('#tabIndex').find('a').click(function () {
                var $this = $(this);
                var href = $this.attr('src');
                var title = $this.text();
                addTab(title, href);
            });
        });

    </script>
</head>
<body class="easyui-layout">
    <form id="form1" runat="server">
    <div region="center" id="mainPanle">
        <div id="tabs" class="easyui-tabs" fit="true" border="false">
            <div title="我的桌面">
                <div id="tabIndex" style="overflow: scroll; overflow-x: hidden; overflow-y: hidden">
                    <%--<div class="admincenter">
                        <div class="admintitle">
                            <div class="sj">
                                <img src="images/zao.gif" width="34" height="25" /></div>
                            <span><strong style="font-size: 14px;">
                                <%=CurrentUserName%>
                                <%=GetDateTime%></strong> <a src="Accounts/userinfo.aspx" href="javascript:;" target="mainFrame">
                                    账号设置</a></span></div>
                    </div>--%>
                    <div class="main_main">
                        <div class="sj">
                            <img src="images/icon5.gif" width="22" height="20" /></div>
                        <span class="main_time">您最后一次登录的时间：<asp:Literal ID="LitLastLoginTime" runat="server"></asp:Literal>
                            (不是您登录的？<a href="javascript:;" src="Logout.aspx" target="mainFrame">请点这里</a>)</span></div>
                    <div class="main_line">
                    </div>
                    <div class="main_iconmenu" style="line-height: normal">
                        <span><a src="SysManage/WebSiteConfig.aspx" href="javascript:;" target="mainFrame">
                            <img src="images/mian_webSit.png" width="48" height="48" /><br />
                            网站设置 </a></span><span><a src="CMS/Content/List.aspx?type=0" href="javascript:;" target="mainFrame">
                                <img src="images/main_contentManage.png" width="48" height="48" /><br />
                                内容管理</a></span> <span><a src="Members/MembershipManage/list.aspx" href="javascript:;"
                                    target="mainFrame">
                                    <img src="images/main_userManage.png" width="48" height="48" /><br />
                                    会员管理</a></span> <span><a src="shop/Products/ProductsInStock.aspx?SaleStatus=1" href="javascript:;"
                                        target="mainFrame">
                                        <img src="images/icon_4.gif" width="48" height="48" /><br />
                                        商品管理</a></span> <span><a src="shop/ProductReview/List.aspx" href="javascript:;" target="mainFrame">
                                            <img src="images/main_CommentManage.png" width="48" height="48" /><br />
                                            用户评论</a></span>
                    </div>
                    <div class="main_tj" style="display: none">
                        <a src="sysmanage/treefavorite.aspx" href="javascript:;" target="mainFrame">新增新的快捷功能</a></div>
                    <div class="admintitle adminxia" style="margin-top: 10px">
                        <div class="sj" style="margin-right: 20px;">
                            <img src="images/icon6.gif" width="21" height="28" /></div>
                        <strong>系统管理</strong></div>
                    <div class="main_bottomzi">
                       <%-- <ul>
                            <li>您可以随时查看用户的查询日志记录</li>
                            <li><a src="SNS/SearchWord/SearchLog.aspx" href="javascript:;">查看查询记录</a></li></ul>--%>
                        <ul>
                            <li>您可以快速清除缓存，及时更新缓存数据</li>
                            <li><a src="sysManage/ClearCache.aspx" href="javascript:;">清除缓存数据</a></li>
                        </ul>
                    </div>
                    <div class="main_line_1">
                    </div>
                    <div class="main_3">
                        <div class="sj">
                            <img src="images/icon7.gif" width="15" height="15" /></div>
                        <div class="mainzileft">
                            个人资料</div>
                    </div>
                    <div class="main_xiaocaidan">
                        <ul>
                            <li><a src="Accounts/userinfo.aspx" href="javascript:;">登录信息</a></li>
                            <li><a src="Accounts/userpass.aspx" href="javascript:;">修改密码</a></li>
                            <li><a src="Members/SiteMessages/List.aspx" href="javascript:;">信息中心</a></li>
                           <%-- <li><a src="sysmanage/treefavorite.aspx?TreeType=0" href="javascript:;">定制菜单</a></li>--%>
                        </ul>
                    </div>
                    <div class="main_main" style="display: none;">
                        <span class="main_time">域名：<asp:Literal ID="litServerDomain" runat="server"></asp:Literal></span>
                    </div>
                    <div class="admintitle adminxia">
                        <div class="sj" style="margin-right: 20px;">
                            <img src="images/icon_3.gif" width="21" height="28" /></div>
                        <strong>系统环境</strong></div>
                    <div class="main_bottomzi systeminfo">
                        <ul>
                            <li>程序版本：</li>
                            <li>
                                <asp:Literal ID="litProductLine" runat="server"></asp:Literal></li>
                        </ul>
                        <ul>
                            <li>操作系统：</li>
                            <li>
                                <asp:Literal ID="litOperatingSystem" runat="server"></asp:Literal></li>
                        </ul>
                        <ul>
                            <li>服务器IIS：</li>
                            <li>
                                <asp:Literal ID="litWebServerVersion" runat="server"></asp:Literal></li>
                        </ul>
                        <ul>
                            <li>.NET框架：</li>
                            <li>
                                <asp:Literal ID="litDotNetVersion" runat="server"></asp:Literal></li>
                        </ul>
                    </div>
                    <div class="main_line_1">
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
    <div id="mm" class="easyui-menu" style="width: 120px;">
        <div id="mm-tabupdate">
            刷新</div>
        <div class="menu-sep">
        </div>
        <div id="mm-tabclose">
            关闭</div>
        <div id="mm-tabcloseother">
            关闭其他</div>
        <div id="mm-tabcloseall">
            关闭全部</div>
    </div>
</body>
</html>
