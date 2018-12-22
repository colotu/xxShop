
<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
  CodeBehind="SendSMS.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Accounts.SendSMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <link href="/Admin/js/select2-3.4.6/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-3.4.6/select2.min.js" type="text/javascript" charset="utf-8"></script>
    <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#ctl00_ContentPlaceHolder1_txtUserId").attr("disabled", "disabled");
            $("input[type='radio']").click(function () {
                var value = $(this).val();
                if (value == "Single") {
                    $("#ctl00_ContentPlaceHolder1_txtUserId").removeAttr("disabled");
                    $("#ctl00_ContentPlaceHolder1_txtUserId").select2({
                        placeholder: "输入用户手机号",
                        minimumInputLength: 3,
                        formatInputTooShort: "请输入至少三个字符",
                        formatNoMatches: "没有匹配项",
                        formatSearching: "正在查询......",
                        ajax: {
                            url: "/UserInfo.aspx",
                            type: "POST",
                            dataType: 'json',
                            quietMillis: 100,
                            data: function (term, page) { // page is the one-based page number tracked by Select2
                                return {
                                    Action: "GetUsersByPhone",
                                    q: term, //search term
                                    page_limit: 10, // page size
                                    page: page // page number
                                };
                            },
                            results: function (data, page) {
                                var more = (page * 10) < data.total; // whether or not there are more results available
                                return { results: data.List, more: more };
                            }
                        },
                        formatResult: Format, // omitted for brevity, see the source of this page
                        escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results
                    });
                } else {
                    $("#ctl00_ContentPlaceHolder1_txtUserId").attr("disabled", "disabled");
                    $("#ctl00_ContentPlaceHolder1_txtUserId").prev().hide();
                    $("#ctl00_ContentPlaceHolder1_txtUserId").removeClass("select2-offscreen").show();
                }
            });
            $(".select2-container").css("vertical-align", "middle");
        });

        function Format(data) {
            return data.text;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="短信群发"/>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="您可以自己定义短信内容，并将短信发送给符合查询条件的所有会员或单个用户"/>
                        <br/><span style="color: red; font-size: 12px;font-weight: bold">请确保您申请的接口已开通群发营销的通道网关，如未开通，请联系亿美短信提供商</span>
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
              
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SysManage,lblSendTo %>" />：
                    <asp:RadioButton ID="Multi" runat="server"  GroupName="A" 
                        Checked="true"/><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:SysManage,lblSendMailToUsers %>"/>：
                    
                    <asp:DropDownList ID="DropUserType" runat="server" class="dropSelect">
                        <asp:ListItem Value="" Selected="True" Text="<%$ Resources:SysManage,ListItemAll%>"></asp:ListItem>
                        <asp:ListItem Value="AA" Text="<%$ Resources:Site,fielDescriptionAA %>"></asp:ListItem>
                        <asp:ListItem Value="UU" Text="<%$ Resources:Site,fielDescriptionUU %>"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;
                    <asp:RadioButton ID="Single" runat="server" GroupName="A"/><asp:Literal ID="Literal5"
                        runat="server" Text="<%$Resources:SysManage,lblSendMailToUser %>"/>：                    
                    <asp:TextBox ID="txtUserId" runat="server" Width="150px" ></asp:TextBox>
                    （请输入用户名手机号）
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                        <tr>
                            <td class="td_class"  style="vertical-align: top;">
                                <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:SysManage,lblMessageContent %>"/>：
                            </td>
                            <td>
                                <asp:TextBox ID="txtContent" runat="server" Width="500px" TextMode="MultiLine" Height="80px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <asp:Button ID="btnNext" runat="server" Text="<%$ Resources:SysManage,btnNextText %>" class="adminsubmit" OnClick="btnNext_Click">
                                </asp:Button>
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

