﻿<%@ Page Title="产品疑问管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="ProductQAList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.ProductQA.ProductQAList" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function GetDeleteM() {
            $("[id$='btnDelete']").click();
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Title -->
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="疑问管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以对产品疑问信息进行管理
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
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Site, lblSearch%>" />：
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Value="-1">请选择</asp:ListItem>
                        <asp:ListItem Value="0">未审核</asp:ListItem>
                        <asp:ListItem Value="1">已审核</asp:ListItem>
                        <asp:ListItem Value="2">审核失败</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit_short"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <br />
        <div class="newslist">
            <div class="newsicon">
                <ul>
                    <li style="background: url(/admin/images/delete.gif) no-repeat; width: 60px;" id="liDel"
                        runat="server"><a href="javascript:;" onclick="GetDeleteM()">
                            <asp:Literal ID="Literal6" runat="server" Text="<%$Resources:Site,btnDeleteListText %>" /></a><b>|</b></li>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" 
            Width="100%" PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowCheckAll="true" DataKeyNames="QAId">
            <Columns>
                <asp:BoundField DataField="QAId" HeaderText="编号" SortExpression="ReviewId"
                    ItemStyle-HorizontalAlign="Center" />

                <asp:TemplateField  HeaderText="商品名" ItemStyle-HorizontalAlign="Left"
                    SortExpression="ProductId">
                    <ItemTemplate>
                       <%#GetProudctName(Eval("ProductId"))%>
                    </ItemTemplate>
                </asp:TemplateField>               
                <asp:BoundField DataField="UserName" HeaderText="提问者" SortExpression="UserName"
                    ItemStyle-HorizontalAlign="Center"/>
                <asp:TemplateField  HeaderText="咨询内容" ItemStyle-HorizontalAlign="Left"
                    SortExpression="QAId">
                    <ItemTemplate>
                        <%#YSWL.Common.Globals.HtmlDecode(SubString(Eval("Question"), "...", 20))%>
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:BoundField DataField="CreatedDate" HeaderText="提问时间" SortExpression="CreatedDate"
                    ItemStyle-HorizontalAlign="Center" />

                        <asp:TemplateField  HeaderText="回复内容" ItemStyle-HorizontalAlign="Left"
                    SortExpression="QAId">
                    <ItemTemplate>
                        <%#YSWL.Common.Globals.HtmlDecode(SubString(Eval("ReplyContent"), "...", 20))%>
                    </ItemTemplate>
                </asp:TemplateField>
              
                <asp:BoundField DataField="ReplyUserName" HeaderText="回复者" SortExpression="ReplyUserName"
                    ItemStyle-HorizontalAlign="Center" />
              <asp:BoundField DataField="ReplyDate" HeaderText="回复时间" SortExpression="ReplyDate"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField  HeaderText="提问状态" ItemStyle-HorizontalAlign="Left"
                    SortExpression="State">
                    <ItemTemplate>
                        <%#GetStatus(Eval("State"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="操作" ItemStyle-HorizontalAlign="Center"  SortExpression="ReviewId">
                    <ItemTemplate>
                     <span id="btnModify" runat="server">  <a href="Reply.aspx?qaid=<%#Eval("QAId") %>">回复</a>
                           &nbsp;&nbsp;</span> 
                        <a href="ShowDetail.aspx?qaid=<%#Eval("QAId") %>">详细信息</a>
                        &nbsp;&nbsp;
                        <span>
                         <a href="ReplyList.aspx?qaid=<%#Eval("QAId") %>">查看回复</a>
                         </span>
                        &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="<%$ Resources:Site, btnDeleteText %>"
                    Visible="false" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                           OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>" Text="<%$ Resources:Site, btnDeleteText %>"></asp:LinkButton>
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
        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;" class="def-wrapper">
            <tr>
                <td>
                    <asp:DropDownList ID="ddlAction" runat="server">
                        <asp:ListItem Value="-1">请选择</asp:ListItem>
                        <asp:ListItem Value="1">已审核</asp:ListItem>
                        <asp:ListItem Value="2">审核失败</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btnAction" runat="server" Text="批量操作" class="adminsubmit" 
                        onclick="btnAction_Click"  />
                    <asp:Button ID="btnDelete" OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>" class="adminsubmit" OnClick="btnDelete_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
