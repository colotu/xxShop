<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Left3_dlb.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Left3_dlb" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="/Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="/admin/css/layout.css?v=1.0" rel="stylesheet">
    <link href="/admin/css/style-left-dlb.css?v=1.0" rel="stylesheet"> 
</head>
 <body   style="background-color: #333645;">
  <div class=" page-container "  >
  <div class="page-sidebar-wrapper">
<div class=" page-sidebar navbar-collapse collapse content_wrap" >
 <div class="logo"><img src="images/logo.png" id="lblLogo"  ></div>
 <div class="nav-left">
 
    <ul class="page-sidebar-menu " data-keep-expanded="false" data-auto-scroll="true" data-slide-speed="200">
        <li class="sidebar-toggler-wrapper">
                <!-- BEGIN SIDEBAR TOGGLER BUTTON -->
                <div class="sidebar-toggler"></div>
                <!-- END SIDEBAR TOGGLER BUTTON -->
            </li>    
        <%=strMenuTree%>
				<%--<li class="start active open">
					<a href="javascript:;">
					<img class="menu1" src="img/navbar_r.png" alt="right">
					<span class="title">管理中心</span>
					</a>
					<ul class="sub-menu">
						<li class="active">
							<a href="index.html">
							企业信息</a>
						</li>
						<li>
							<a href="index_2.html">
							企业认证</a>
						</li>
						
					</ul>
				</li> --%>
 
			</ul>
    </div>
</div>
  
</div>
 
</div>
     <script src="/Scripts/bootstrap/js/bootstrap.min.js"></script>
    <script src="js/left_metronic.js" type="text/javascript"></script>
     <script src="js/left_layout.js" type="text/javascript"></script>
         <script type="text/javascript">
        $(function () {
            jQuery(document).ready(function () {
                Metronic.init(); // init metronic core componets
                Layout.init(); // init layout
            });
            $('.dropdown_ul a').click(function () {
                var $this = $(this);
                var href = $this.attr('src');
                var title = $this.text();
                if (!href || href == "") {
                    return;
                }
             window.parent.mainFrame.addTab(title, href);
            });
        });
    </script>
  </body>
</html>
 
