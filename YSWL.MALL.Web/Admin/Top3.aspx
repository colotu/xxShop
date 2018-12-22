<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Top3.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Top3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
       <link href="/admin/css/admin.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/jquery-1.7.2.min.js" type="text/javascript"></script>
        <link rel="stylesheet" href="/admin/css/reset.css">
    <link rel="stylesheet" href="/admin/css/main.css">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
   <script type="text/javascript">
        $(function () {
            $('.navbar-header a').click(function () {
                var $this = $(this);
                var href = $this.attr('src');
                var title = $this.text();
                if (href) {
                    title = $.trim(title);
                    if (!title) {
                        title = "系统消息";
                    }
                    window.parent.mainFrame.addTab(title, href);
                }

            });
            $('.navbar-brand.tabCreat').click(function () {
                window.top.location.href = $(this).attr('target-url');
            });
        });
    </script>
</head>
<body>
    <header>
<%--        <div style="float:left;padding:15px">
            <a class="navbar-brand tabCreat" href="javascript:;" target-url="http://saas.ys56.com" target-title="首页">
                <img id="home" alt="Brand" src="/Images/home.png"/>
            </a>
        </div>--%>
        <nav class="navbar-header">
            <ul>
                <li>
                  <%--  <a href="#"><img src="img/message2.png" alt="message">信息</a>--%>
                    <a src="Members/SiteMessages/List.aspx" href="javascript:;" target="mainFrame"   style="display: none">
                      <img src="img/message2.png" alt="message">信息
                    </a>
                </li>
                <li>
                    <a  src="Accounts/userinfo.aspx" href="javascript:;" target="mainFrame" >
                        <img src="img/personal.png" alt="personal"><%=username%>
                    </a>
                   &nbsp;&nbsp;
                      <a  src="Logout.aspx" href="javascript:;" target="mainFrame" >
                        退出
                    </a>
                </li>
            </ul>
        </nav>
    </header>
</body>
</html>
