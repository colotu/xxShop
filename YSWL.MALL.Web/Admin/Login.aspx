<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>商城系统框架-系统登录</title>
    <script language="javascript" type="text/javascript">
        ; if (parent.length) { parent.window.location = "Login.aspx"; }
        function newGuid() {
            var guid = "";
            for (var i = 1; i <= 32; i++) {
                var n = Math.floor(Math.random() * 16.0).toString(16);
                guid += n;
                if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                    guid += "-";
            }
            return guid;
        }
        function ChangeCode() {
            var myImg = document.getElementById("ImageCheck");
            myImg.src = "/ValidateCode.aspx?flag=" + newGuid();
            return false;
        }
    </script>
    <script type="text/javascript" src="js/Common.js" charset="gb2312"></script>
</head>
<body class="standard-bg">
  <link href="<%= YSWL.Components.MvcApplication.StaticHost %>/css/login.min.css" rel="stylesheet"   type="text/css"/>
  <link href="<%= YSWL.Components.MvcApplication.StaticHost %>/lib/msgbox-2.0/css/msgbox.min.css" rel="stylesheet"/>
  <script src="<%= YSWL.Components.MvcApplication.StaticHost %>/lib/jquery-2.2.4.min.js"></script>
  <script src="<%= YSWL.Components.MvcApplication.StaticHost %>/lib/msgbox-2.0/js/msgbox.min.js"></script>
<!-- 外层包裹的类（实现左右上间隙） -->
<div class="standard-page-wrapper">
  <!-- start 登录页 -->
  <div class="s-login-box-wrap">
    <div class="logo">
       <asp:Image ID="Image1" runat="server" ImageUrl="images/logo_login.png" />
    </div>
    <div class="s-login-box">
      <div class="s-login-title">登录</div>
      <div class="s-bd">
       <form  id="form1" runat="server">
          <div class="s-form-item">
              <asp:TextBox ID="txtUsername" runat="server"  CssClass="s-input-text" TabIndex="1" placeholder="请输入用户名"></asp:TextBox>
          </div>
          <div class="s-form-item">
              <asp:TextBox ID="txtPass" TextMode="Password" runat="server"  TabIndex="2" CssClass="s-input-text" placeholder="请输入密码"></asp:TextBox>
          </div>
          <div class="s-form-item qrcode-field">
              <input class="s-input-text" id="CheckCode" tabindex="3" maxlength="4" name="user" runat="server"  autocomplete="off"  aria-label="验证码" placeholder="请输入验证码"    />
            <div class="s-captcha"> <img id="ImageCheck" onclick="ChangeCode()" src="/ValidateCode.aspx" tooltip="验证码" /></div>
          </div>
            <asp:Button ID="btnLogin" runat="server"  CssClass="s-btn-def-submit" OnClick="btnLogin_Click"   tabindex="4"  Text="登 录"  />          
          <div class="s-login-msg s-error" ><asp:Label ID="lblMsg" runat="server"></asp:Label></div>
        </form>
      </div>
    </div>
  </div>
  <!-- end 登录页 -->
</div>
<script type="text/javascript">
  var _scrollHeight = $(document).scrollTop(),//获取当前窗口距离页面顶部高度
  _windowHeight = $(window).height(),//获取当前窗口高度
  _posiTop = (_windowHeight - 530)/2 + _scrollHeight;
  $(".s-login-box-wrap").css({"margin-top": _posiTop + "px"});
</script>
</body>
</html>
