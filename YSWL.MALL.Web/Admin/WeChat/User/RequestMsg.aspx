﻿<%@ Page Title="<%$ Resources:SysManage,ptFeedbackList%>" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="RequestMsg.aspx.cs" Inherits="YSWL.MALL.Web.Admin.WeChat.User.RequestMsg" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <link href="/admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <link href="/Admin/js/select2-2.1/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-2.1/select2.min.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        $(document).ready(function () { $("[id$='ddUser']").select2(); });
    </script>
    <script type="text/javascript">
        $(function () {
            $(".iframe").colorbox({ iframe: true, width: "780", height: "630", overlayclose: false });
            $(".imageUrl").each(function () {
                var src = $(this).attr("src");
                if (src != "") {
                    $(this).show();
                }
            });
            $("[id$='ddlAction']").hide();
            var event = $("[id$='ddlEvent']").val();
            if (event == "CLICK") {
                $("[id$='ddlAction']").show();
            }

            $("[id$='ddlEvent']").change(function () {
                var value = $(this).val();
                $("[id$='ddlAction']").hide();
                if (value == "CLICK") {
                    $("[id$='ddlAction']").show();
                }
            })
            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("#ctl00_ContentPlaceHolder1_txtFrom").prop("readonly", true).datepicker({
               
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("#ctl00_ContentPlaceHolder1_txtTo").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#ctl00_ContentPlaceHolder1_txtTo").prop("readonly", true).datepicker({
               
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("#ctl00_ContentPlaceHolder1_txtFrom").datepicker("option", "maxDate", selectedDate);
                    $("#ctl00_ContentPlaceHolder1_txtTo").val($(this).val());
                }
            });

            $('#showEventMsg').click(function () {
                $('[id$="chkShowEvent"]').click();
            }); 
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="微信用户消息管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="lblTitle" runat="server" Text="微信用户消息管理" />
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang borderkuang-noc padd-no mar-bt">
            <tr>
                <td>
                    消息时间：
                    <asp:TextBox ID="txtFrom" runat="server" Width="120" CssClass="mar-r0"></asp:TextBox>
                    --
                    <asp:TextBox ID="txtTo" runat="server" Width="120"></asp:TextBox>

                    <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:Site,lblKeyword%>" />：
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    <asp:DropDownList ID="ddlEvent" runat="server">
                        <asp:ListItem Value="">请选择</asp:ListItem>
                        <asp:ListItem Value="subscribe">关注事件</asp:ListItem>
                        <asp:ListItem Value="unsubscribe">取消关注</asp:ListItem>
                        <asp:ListItem Value="LOCATION">地理位置</asp:ListItem>
                        <asp:ListItem Value="CLICK">自定义事件</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlAction" runat="server">
                    </asp:DropDownList>
                    <asp:CheckBox ID="chkShowEvent" runat="server" /><label for="chkShowEvent" id="showEventMsg">显示事件消息</label>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit-short"></asp:Button>
                </td>
            </tr>
        </table>
        <div class="mar-bt">
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="true" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify" Width="100%"
            PageSize="10" ShowExportExcel="false" ShowExportWord="False" CellPadding="3"
            BorderWidth="1px" ShowCheckAll="True" DataKeyNames="UserMsgId">
            <Columns>
                <asp:BoundField DataField="CreateTime" HeaderText="发送时间" ItemStyle-Width="120px"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="消息内容" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%# Eval("Description")%>
                        <a href="<%#Eval("PicUrl")%>" class="iframe">
                            <img src="<%#Eval("PicUrl")%>" alt="" width="80" height="80" class="imageUrl" style="display: none" /></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="用户名" ItemStyle-HorizontalAlign="center" ItemStyle-Width="120px">
                    <ItemTemplate>
                        <%# GetNickName(Eval("UserName"))%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="消息类型" ItemStyle-HorizontalAlign="center" ItemStyle-Width="120px">
                    <ItemTemplate>
                        <%# GetMsgType(Eval("MsgType"))%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="事件类型" ItemStyle-HorizontalAlign="center" Visible="False"
                    ItemStyle-Width="120px">
                    <ItemTemplate>
                        <%# Eval("Event")%></ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="35px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
        </cc1:GridViewEx>
        </div>
        <asp:Button ID="btnDeleteMsg" runat="server" Text="批量删除" OnClick="btnDeleteMsg_Click"
            CssClass="adminsubmit add-btn" OnClientClick="return confirm('你确认要删除么？')" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
