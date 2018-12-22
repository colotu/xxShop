<%@ Page  Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="MsgList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.WeChat.CustomerMsg.MsgList" %>

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
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="客服发送消息记录管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="客服发送消息记录管理" />
                    </td>
                </tr>
            </table>
        </div>
       
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang borderkuang-noc padd-no">
            <tr>
                <td>
                   消息时间：
                    <asp:TextBox ID="txtFrom"   runat="server" Width="120" CssClass="mar-r0"></asp:TextBox>
                    --
                     <asp:TextBox ID="txtTo"   runat="server" Width="120" ></asp:TextBox>
                    <span class="gong-zh">公众号：</span>
                    <asp:DropDownList ID="ddOpenId" runat="server">
                    </asp:DropDownList>
                      <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit-short add-btn"></asp:Button>
                </td>
            </tr>
        </table>
        <div class="newslist mar-bt">
            <div class="newsicon">
                <ul>
                    <li id="liAdd" class="add-btn" runat="server">
                        <a href="../User/SendMsg.aspx">
                            <asp:Literal ID="Literal5" runat="server" Text="群发消息" /></a>
                    </li>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="true" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify" Width="100%"
            PageSize="10" ShowExportExcel="false" ShowExportWord="False" CellPadding="3"
            BorderWidth="1px" ShowCheckAll="True" DataKeyNames="MsgId">
            <Columns>
                <asp:BoundField DataField="CreateTime" HeaderText="发送时间" ItemStyle-Width="120px"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="消息内容" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%# Eval("Description")%>
                        <%# Eval("MsgType").ToString() == "voice"?"语音文件":""%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="公众号" ItemStyle-HorizontalAlign="center" ItemStyle-Width="120px">
                    <ItemTemplate>
                        <%# Eval("OpenId")%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="消息类型" ItemStyle-HorizontalAlign="center" ItemStyle-Width="120px">
                    <ItemTemplate>
                        <%# GetMsgType(Eval("MsgType"))%></ItemTemplate>
                </asp:TemplateField>

                   <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="center" ItemStyle-Width="120px">
                    <ItemTemplate>
                       <a href='UserList.aspx?id=<%#Eval("MsgId")%>'  class="iframe">查看用户</a>
                     </ItemTemplate>
                </asp:TemplateField>
            </Columns>
                <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="35px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
        </cc1:GridViewEx>
        <div class="def-wrapper">
<asp:Button ID="btnDeleteMsg" runat="server" Text="批量删除"   OnClick="btnDeleteMsg_Click" CssClass="add-btn"  OnClientClick="return confirm('你确认要删除么？')"/>
        </div>
   
        
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>

