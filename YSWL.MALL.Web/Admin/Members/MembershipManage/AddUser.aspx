<%@ Page Title="<%$ Resources:Site, ptAddUser %>" Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master"
    AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Members.MembershipManage.AddUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
           $('#btnSave').click(function () {
                $(this).attr({ "disabled": "disabled" });
                var username = $('[id$="txtUserName"]').val();
                if (username == "") {
                    ShowFailTip("请填写手机号！");
                    $(this).removeAttr("disabled");
                    return;
                }
                var regs=/^1\d{10}$/;
                if (!regs.test(username)) {
                    ShowFailTip("请填写有效的手机号！");
                    $(this).removeAttr("disabled");
                    return;
                }
                var trueName = $('[id$="txtTrueName"]').val();
                if (trueName == "") {
                    ShowFailTip("请填写真实姓名！");
                    $(this).removeAttr("disabled");
                    return;
                }
                
                var password = $('[id$="txtPassword"]').val();
                if (password == "") {
                    ShowFailTip("请填写密码！");
                    $(this).removeAttr("disabled");
                    return;
                }
                var password1 = $('[id$="txtPassword1"]').val();
                if (password1 == "") {
                    ShowFailTip("请填写确认密码！");
                    $(this).removeAttr("disabled");
                    return;
                }
                if (password != password1) {
                    ShowFailTip("密码不一致！");
                    $(this).removeAttr("disabled");
                    return;
                }
                var phone = $('[id$="txtPhone"]').val();
                //if (phone == "") {
                //    ShowFailTip("请填写联系方式！");
                //    $(this).removeAttr("disabled");
                //    return;
                //}
                var email = $('[id$="txtEmail"]').val();
                var shipname = $('[id$="txtshipName"]').val();
                if (shipname == "") {
                    ShowFailTip("请填写收货人！");
                    $(this).removeAttr("disabled");
                    return;
                }
                var shipphone = $('[id$="txtshipPhone"]').val();
                if (shipphone == "") {
                    ShowFailTip("请填写收货人手机号码！");
                    $(this).removeAttr("disabled");
                    return;
                }
                var regionId = parseInt($('[id$="hfSelectedNode"]').val());
                if (isNaN(regionId) || regionId <= 0) {
                    ShowFailTip("请选择收货地区！");
                    $(this).removeAttr("disabled");
                    return;
                }
                var address = $('[id$="txtAddress"]').val();
                if (address == "") {
                    ShowFailTip("请填写收货地址！");
                    $(this).removeAttr("disabled");
                    return;
                }
                $.ajax({
                    url: ("AddUser.aspx?timestamp={0}").format(new Date().getTime()),
                    type: 'POST', dataType: 'json', timeout: 10000,
                    data: { Action: "AddInfo", Callback: "true", username: username, trueName: trueName, password: password, phone: phone, email: email, shipname: shipname, shipphone: shipphone, regionId: regionId, address: address },
                    success: function (resultData) {
                        if (resultData.STATUS == "SUCCESS") {
                            ShowSuccessTip("新增成功");
                            window.parent.location.href = "List2.aspx";
                        }else {
                            switch (resultData.DATA) {
                                case "UserNameIsNull":
                                    ShowFailTip('手机号不能为空！');
                                    break;
                                case "PasswordIsNull":
                                    ShowFailTip('密码不能为空！');
                                    break;
                                //case "PhoneIsNull":
                                //    ShowFailTip('联系方式不能为空！');
                                //    break;     
                                case "HasUserName":
                                    ShowFailTip('手机号已存在！');
                                    break;
                                case "ADDFAILED":
                                    ShowFailTip('新增失败！');
                                    break;
                            }
                        }
                        $('#btnsave').removeAttr("disabled");
                    }
                });
                $(this).removeAttr("disabled");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                         <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SysManage, lblAddUser%>" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                         <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:SysManage, lblAddNewUserOperate%>" />
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                        <tr>
                            <td class="td_class">
                                <span class="red">* </span>
                                <asp:Literal ID="Literal2" runat="server" Text="手机号" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtUserName" TabIndex="1" runat="server" Width="200px" MaxLength="20"
                                    ></asp:TextBox><br />
                                
                            </td>
                            <td class="td_class">
                                <span class="red">* </span>
                                <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Site, fieldTrueName%>" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtTrueName" TabIndex="2" runat="server" Width="200px" MaxLength="20" ></asp:TextBox><br />
                              
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                 <span class="red">* </span>
                                <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Site, fieldPassword%>" />：
                            </td>
                            <td height="25">                       
                                <asp:TextBox ID="txtPassword" TabIndex="3" runat="server" Width="200px" MaxLength="20"
                                     TextMode="Password"></asp:TextBox><br />
                                
                            </td>
                             <td class="td_class">
                                 <span class="red">* </span>
                                <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Site, fieldConfirmationPassword%>" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtPassword1" TabIndex="4" runat="server" Width="200px" MaxLength="20"
                                     TextMode="Password"></asp:TextBox><br />
                                 
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                               
                                <asp:Literal ID="Literal7" runat="server" Text="联系方式" />：
                            </td>
                            <td style="height: 3px" height="3">
                                <asp:TextBox ID="txtPhone" runat="server" Width="200px" TabIndex="5" ></asp:TextBox><br />
                                
                            </td>
                              <td class="td_class">
                                <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:Site, fieldEmail%>" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtEmail" runat="server" Width="200px" TabIndex="6" ></asp:TextBox>
                            </td>
                        </tr>    
                        <tr>
                            <td class="td_class">
                               <span class="red">* </span>收货人 ：
                            </td>
                            <td  >
                                  <asp:TextBox   ID="txtshipName" runat="server" TabIndex="7" ></asp:TextBox><br />
                            
                            </td>
                             <td class="td_class">
                               <span class="red">* </span>收货人手机号码  ：
                            </td>
                            <td  colspan="2">
                                <asp:TextBox   ID="txtshipPhone" runat="server" TabIndex="8" ></asp:TextBox><br />
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <span class="red">* </span>收货地区 ：
                            </td>
                            <td  colspan="3"  id="ship_address">
                               <div id="region_h">   
                                    <script src="/Scripts/jquery/jquery.guid.js" type="text/javascript"></script>
                                    <input type="hidden" id="hfSelectedNode" value="" TabIndex="9"  />
                                    <script src="/Scripts/jquery/maticsoft.selectregion.js" handle="/RegionHandle.aspx" isnull="true" type="text/javascript"></script>
                               </div>            
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                               <span class="red">* </span>详细地址 ：
                            </td>
                            <td  colspan="3" >
                                <asp:TextBox   ID="txtAddress" runat="server" style="width: 400px;" TabIndex="10" ></asp:TextBox><br />
                               
                            </td>
                          </tr>          
                        <tr>
                            <td height="25"  colspan="4" style="text-align: center;">
                                <asp:Button ID="btnCancle" runat="server" CausesValidation="false" Text="<%$ Resources:Site, btnCancleText %>"
                                  class="adminsubmit_short" OnClientClick="javascript:parent.$.colorbox.close();"></asp:Button>
                                 <input type="button" id="btnSave" value="保存"  class="adminsubmit_short" />
                         
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
