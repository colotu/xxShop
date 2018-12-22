<%@ Page Title="系统设置" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="SysConfig.aspx.cs" Inherits="YSWL.MALL.Web.Admin.SysManage.SysConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        window.UEDITOR_HOME_URL = "/ueditor/";
    </script>
    <script src="/ueditor/ueditor.config.js" type="text/javascript"></script>
    <script src="/ueditor/ueditor.all.min.js" type="text/javascript"></script>
    <link href="/ueditor/themes/default/ueditor.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery/maticsoft.img.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("[id$='txtRechargeRatio']").OnlyFloat();
            $("[id$='txtRankScoreRatio']").OnlyFloat();
            var isOpen = $("#ctl00_ContentPlaceHolder1_chkOpenCoupon").attr("checked");
            if (isOpen) {
                $("#ddlOpenCoupon").show();
            }
            $("#ctl00_ContentPlaceHolder1_chkOpenCoupon").click(function() {
                var open = $(this).attr("checked");
                if (open) {
                    $("#ddlOpenCoupon").show();
                } else {
                    $("#ddlOpenCoupon").hide();
                }
            })

        });
      
       

        function confirmRegMode() {
            var oldRegVal = $('[id$="hidradiobutRegStr"]').val();
            if ($.trim(oldRegVal).length > 0) {
                if ($('[name$="RadioButtonRegister"]:checked').val() != oldRegVal) {
                    var str = '';
                    if (oldRegVal == "Email") {
                        str = "邮箱";
                    }
                    if (oldRegVal == "Phone") {
                        str = "手机号码";
                    }
                    if (!confirm("修改注册方式会导致" + str + "注册的用户不能登陆，您确认要修改么？")) {
                        return false;
                    } else {
                        return true;
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="hidradiobutRegStr" runat="server" />
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="系统设置" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以对系统进行相关参数设置" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="HiddenField_ID" runat="server" />
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                        <tr  >
                            <td class="td_class">
                                <asp:Literal ID="Literal15" runat="server" Text="登录注册开关" />：
                            </td>
                            <td height="25">
                                <asp:CheckBox ID="chk_OpenLogin" runat="server" Text="关闭用户登录" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="chk_OpenRegister" runat="server" Text="关闭用户注册" />
                                &nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr  >
                            <td class="td_class">
                                <asp:Literal ID="Literal16" runat="server" Text="注册邮件验证开关" />：
                            </td>
                            <td height="25">
                                <asp:CheckBox ID="chk_OpenRegisterSendEmail" runat="server" Text="关闭邮件验证" />
                            </td>
                        </tr>
                        <tr  >
                            <td class="td_class">
                                <asp:Literal ID="Literal5" runat="server" Text="订单邮件通知" />：
                            </td>
                            <td height="25">
                                <asp:CheckBox ID="chk_OpenOrderEmail" runat="server" Text="开启" />
                            </td>
                        </tr>
                        <tr  >
                            <td class="td_class">
                                <asp:Literal ID="Literal18" runat="server" Text="注册方式" />：
                            </td>
                            <td height="25">
                                <asp:RadioButtonList ID="RadioButtonRegister" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Email">邮箱</asp:ListItem>
                                    <asp:ListItem Value="Phone">手机</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                                                <tr  >
                            <td class="td_class">
                                <asp:Literal ID="Literal9" runat="server" Text="注册送优惠券" />：
                            </td>
                            <td height="25">
                                   <asp:CheckBox ID="chkOpenCoupon" runat="server" Text="开启" />
                                               <span id="ddlOpenCoupon" style="display: none">  优惠券规则：   <asp:DropDownList ID="ddlCoupon" runat="server" Width="120px">
                                                        <asp:ListItem  Value="0" Text="请选择"></asp:ListItem>
                                                    </asp:DropDownList> 没有赠送优惠券？<a href="/Admin/Shop/Coupon/AddRule.aspx" style="color:red">点击创建</a>
                                                    </span>
                            </td>
                        </tr>
                        <tr >
                            <td class="td_class">
                                <asp:Literal ID="Literal4" runat="server" Text="购买方式" />：
                            </td>
                            <td height="25">
                                <asp:RadioButtonList ID="rdoBuyMode" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="AddCart">加入购物车</asp:ListItem>
                                    <asp:ListItem Value="BuyNow">立刻购买</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            
                        </tr>
                         <tr >
                            <td class="td_class">
                                <asp:Literal ID="Literal7" runat="server" Text="团购/限时抢购使用优惠券"/>：
                            </td>
                            <td>
                            <asp:CheckBox ID="chbPromotionsIsUseCoupon" runat="server" Text="开启" />
                            </td>
                        </tr>
                        <tr >
                            <td class="td_class">
                                <asp:Literal ID="Literal8" runat="server" Text="商家是否参与促销活动"/>：
                            </td>
                            <td>
                            <asp:CheckBox ID="chbStoreIsInActivity" runat="server" Text="开启" />
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td class="td_class">
                                <asp:Literal ID="Literal14" runat="server" Text="百度分享用户ID" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtBaiduShareUserId" runat="server" Width="400"></asp:TextBox>
                                申请<a href="http://share.baidu.com/" target="_blank" style="color: blue">百度分享</a>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal20" runat="server" Text="充值金额比例" />：
                            </td>
                            <td>
                                <asp:TextBox ID="txtRechargeRatio" runat="server" Width="50" Height="30" value="1"></asp:TextBox>(示例：1.2)
                            </td>
                        </tr>
                        <tr runat="server" id="tr_RankScoreRatio" Visible="False">
                            <td class="td_class">
                                <asp:Literal ID="Literal6" runat="server" Text="购物成长值比例" />：
                            </td>
                            <td>
                                <asp:TextBox ID="txtRankScoreRatio" runat="server" Width="50" Height="30" value="1"></asp:TextBox><span style="padding-left: 5px;">订单完成后获得成长值(购物成长值=结算金额*购物成长值比例，结算金额指以现金或银行卡方式支付的金额)<span style="color:red;">注意：值大于0才会起作用</span></span>
                            </td>
                        </tr>
                       
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal12" runat="server" Text="网站统计脚本" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="txtPageFootJs" runat="server" Width="400" Height="80" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal3" runat="server" Text="注册信息声明" />：
                            </td>
                            <td>
                                <asp:TextBox ID="txtContent" runat="server" Width="80%" TextMode="MultiLine" Height="300"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                  <asp:Button ID="btnReset" runat="server" CausesValidation="false" Text="<%$ Resources:Site, btnCancelText%>"
                                    class="adminsubmit_short" OnClick="btnReset_Click"></asp:Button>
                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Site, btnSaveText %>"
                                    class="adminsubmit_short" OnClientClick="return confirmRegMode();" OnClick="btnSave_Click">
                                </asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </div>
    <script type="text/javascript">
        var editor = new baidu.editor.ui.Editor({//实例化编辑器
            iframeCssUrl: 'ueditor/themes/default/iframe.css', toolbars: [

            ['fullscreen', '|',
                'bold', 'italic', 'underline', 'strikethrough', '|', 'forecolor', 'backcolor', '|',
                  'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', 'insertorderedlist', 'insertunorderedlist', '|', 'indent', '|',
                  'imagenone', 'imageleft', 'imageright',
                'imagecenter', '|', 'insertimage', '|', 'link', 'unlink']
                 ],
            initialContent: '',
            autoHeightEnabled: false,
            initialFrameHeight: 220,
            pasteplain: false
         , wordCount: false
          , elementPathEnabled: false
 , autoClearinitialContent: false, imagePath: "/Upload/RTF/", imageManagerPath: "/"
        });
        editor.render('<%=txtContent.ClientID %>'); //将编译器渲染到容器 //将编译器渲染到容器
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
