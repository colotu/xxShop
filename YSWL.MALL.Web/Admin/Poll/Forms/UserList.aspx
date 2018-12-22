<%@ Page Title="会员信息管理" Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master"
    AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Poll.Forms.UserList" %>

 
<%@ Register TagPrefix="cc1" Namespace="YSWL.MALL.Web.Controls" Assembly="YSWL.MALL.Web" %>
<%@ Register TagPrefix="uc1" TagName="Region" Src="~/Controls/Region.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("span:contains('已冻结')").css("color", "red");
            $("span:contains('已激活')").css("color", "#006400");
            $("span:contains('未认证')").css("color", "red");
            $("span:contains('已认证')").css("color", "#006400");

            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("[id$='txtEndTime']").prop("readonly", true).datepicker({ dateFormat: "yy-mm-dd", yearRange: ("1949:" + new Date().getFullYear()) });
            $("[id$='txtBeginTime']").prop("readonly", true).datepicker({ dateFormat: "yy-mm-dd", yearRange: ("1949:" + new Date().getFullYear()) });

            var checkList = $('.GridViewTyle [type=checkbox]');
            if (checkList.length > 0) {
                checkList.eq(0).hide();
            }
//            checkList.click(function () {
//                checkList.not($(this)).hide();
//                var userId = $(this).parent().next().text();
//                if ($(this).hasClass('cur')) {//取消选中
//                    $(this).removeClass('cur');
//                    checkList.show();
//                    checkList.eq(0).hide();
//                } else {//选中
//                    var success = false;
//                    if (success == true) {
//                        $(this).addClass('cur');
//                    } else {
//                        ShowFailTip("该用户已提交过问卷，请重新选择用户");
//                    }
//                    alert(userId);
//                }

//            });
        })
    
  
    </script>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <!--Title -->
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        选择会员
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以进行选择用户
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody" style="width: 253px;">
                    <asp:Literal ID="Literal2" runat="server" Text="关键字" />：
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                   <asp:Literal ID="Literal3" runat="server" Text="地区" />：
                  
                </td>
                <td style="padding-left: 0px;"><asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <contenttemplate>
                                    <uc1:Region ID="RegionID" runat="server"  VisibleAll ="true"  VisibleAllText="--请选择--" />
                                    </contenttemplate>
                    </asp:UpdatePanel></td>
            </tr>
            <tr><td colspan="2"> <asp:Literal ID="Literal1" runat="server" Text="状态" />：
                    <asp:DropDownList ID="dropType" runat="server">
                        <asp:ListItem Value="-1" Text="--请选择--"></asp:ListItem>
                        <asp:ListItem Value="0" Text="冻结"></asp:ListItem>
                        <asp:ListItem Value="1" Text="激活"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Literal ID="Literal5" runat="server" Text="注册时间："></asp:Literal>
                    <asp:TextBox ID="txtBeginTime" runat="server" CssClass="PostDate"></asp:TextBox>
                    <asp:Literal ID="Literal6" runat="server" Text="--"></asp:Literal>
                    <asp:TextBox ID="txtEndTime" runat="server" CssClass="PostDate" > </asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit"></asp:Button></td></tr>
        </table>
        <!--Search end-->
        <br />
        <div class="newslist">
            <div class="newsicon">
                <ul>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowCommand="gridView_RowCommand" OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting"
            UnExportedColumnNames="Modify" Width="100%" PageSize="3" ShowExportExcel="False"
            ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
            ShowCheckAll="true"  DataKeyNames="UserID">
            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="编号" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="40" SortExpression="UserID" />
                <asp:TemplateField ItemStyle-Width="80">
                    <ItemTemplate>
                        <asp:Image ID="Image1" runat="server" Width="80px" Height="80px" ImageAlign="Middle"
                            ImageUrl='<%#GetGravatar(Eval("UserID")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="用户" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td style="width: 240px">
                                    用 户 名：
                                    <%#Eval("UserName")%>
                                </td>
                                <td style="width: 240px">
                                    性&nbsp;&nbsp;&nbsp;&nbsp;别：
                                    <%#Eval("Sex").ToString().Trim() == "0" ? "女" : "男"%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    昵&nbsp;&nbsp;&nbsp;&nbsp;称： <span style="color: Gray">
                                        <%#Eval("NickName")%></span>
                                </td>
                                <td>
                                    真实姓名：<span style="color: Gray">
                                        <%#Eval("TrueName")%></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    手机号码： <span style="color: Gray">
                                        <%#Eval("Phone")%></span>
                                </td>
                                <td>
                                    邮&nbsp;&nbsp;&nbsp;&nbsp;箱：<span style="color: Gray">
                                        <%#Eval("Email")%></span>
                                </td>
                            </tr>
                          <tr>
                                <td colspan="2">
                                    所 在 地： <span style="color: Gray">
                                        <%# GetRegion(Eval("RegionId"))%></span>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="User_dateCreate" HeaderText="注册时间" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="100" SortExpression="User_dateCreate" />
                <asp:TemplateField HeaderText="用户状态" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnID" runat="server" CausesValidation="False" CommandName="Status"
                            CommandArgument='<%#Eval("UserID")+","+Eval("Activity")%>' Style="color: #0063dc;">
                            <span ><%#(bool)Eval("Activity") ? "已激活" : "已冻结"%></span>
                            </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="35px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
            <SortDirectionStr>DESC</SortDirectionStr>
        </cc1:GridViewEx>
        <table border="0" cellpadding="0" cellspacing="1" style="width: 400px; height: 100%" class="def-wrapper">
            <tr>
                <td>
                    <asp:Button ID="btnSave"  runat="server" Text="确定" class="adminsubmit" OnClick="btnSave_Click" />
                </td>
               
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
   
</asp:Content>
