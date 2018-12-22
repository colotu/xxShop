<%@ Page Title="电子样本管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="SampleList.aspx.cs" Inherits=" YSWL.Web.Admin.Shop.Sample.SampleList" %>

<%@ Register Assembly="YSWL.Web" Namespace="YSWL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" >
    $(function () {
        $("span:contains('设为可用')").css("color", "black");
        $("span:contains('取消可用')").css("color", "#006400");
        $("span:contains('设为菜单')").css("color", "black");
        $("span:contains('取消菜单')").css("color", "#006400");
        $("span:contains('显示')").css("color", "black");
        $("span:contains('取消显示')").css("color", "#006400");
    });

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
                        <asp:Literal ID="Literal1" runat="server" Text="样本管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text=" 您可以新增、修改、删除样本" />
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
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit_short"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <div class="newslist mar-bt">
            <div class="newsicon">
                <ul>
                    <li class="add-btn" runat="server"  ID="AddLi" >
                        <asp:HyperLink ID="hlkadd" runat="server" NavigateUrl="SampleAdd.aspx" >新增</asp:HyperLink>
                    </li>
                    <li class="add-btn" id="liDel"
                        runat="server"><a href="javascript:;" onclick="GetDeleteM()">
                            <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:Site,btnDeleteListText %>" /></a></li>
                   
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"  OnRowCommand="gridView_RowCommand"  
            Width="100%" PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowCheckAll="true" DataKeyNames="SampleId">
            <Columns>
                <asp:BoundField DataField="SampleId" HeaderText="编号" SortExpression="SampleId"
                    ItemStyle-HorizontalAlign="Center" />
                    
                

                <asp:BoundField DataField="Tiltle" HeaderText="名称"  ItemStyle-HorizontalAlign="Center" />
                
                       <asp:TemplateField HeaderText="状态" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# GetStatus(Eval("Status"))%>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="CreatedDate" HeaderText="新增时间"  SortExpression="CreatedDate" ItemStyle-HorizontalAlign="Center"  />

               <asp:BoundField DataField="Sequence" HeaderText="显示顺序"  SortExpression="Sequence" ItemStyle-HorizontalAlign="Center"  />
               
                  <asp:TemplateField HeaderText="GPG封面" ItemStyle-HorizontalAlign="Center"   Visible="True">
                    <ItemTemplate>
                        <img src="<%# Eval("ThumblElecCoverImageUrl")%>" width="60px" height="60px" />
                    </ItemTemplate>
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="PDF封面" ItemStyle-HorizontalAlign="Center" Visible="True">
                    <ItemTemplate>
                        <img src="<%# Eval("ThumbPdfCoverImageUrl")%>" width="60px" height="60px" />
                    </ItemTemplate>
                </asp:TemplateField>

               <asp:HyperLinkField HeaderText="编辑" ControlStyle-Width="50"
                    DataNavigateUrlFields="SampleId" DataNavigateUrlFormatString="SampleModify.aspx?id={0}" Text="<%$ Resources:Site, btnEditText %>"
                    ItemStyle-HorizontalAlign="Center" />
                    
                    <asp:HyperLinkField HeaderText="查看下属PDF" ControlStyle-Width="50"
                    DataNavigateUrlFields="SampleId" DataNavigateUrlFormatString="SamplePDF.aspx?id={0}" Text="查看"
                    ItemStyle-HorizontalAlign="Center" />
                    
                    
                    <asp:HyperLinkField HeaderText="查看下属图片" ControlStyle-Width="50"
                    DataNavigateUrlFields="SampleId" DataNavigateUrlFormatString="SampleImage.aspx?id={0}" Text="查看"
                    ItemStyle-HorizontalAlign="Center" />
<asp:HyperLinkField HeaderText="新增图片" ControlStyle-Width="50"
                    DataNavigateUrlFields="SampleId" DataNavigateUrlFormatString="ImageAdd4Sample.aspx?id={0}" Text="新增"
                    ItemStyle-HorizontalAlign="Center" />
                    
                    
                    <asp:HyperLinkField HeaderText="新增PDF" ControlStyle-Width="50"
                    DataNavigateUrlFields="SampleId" DataNavigateUrlFormatString="ImageAdd4Sample.aspx?id={0}" Text="新增"
                    ItemStyle-HorizontalAlign="Center" />
                    <asp:HyperLinkField HeaderText="编辑" ControlStyle-Width="50"
                    DataNavigateUrlFields="SampleId" DataNavigateUrlFormatString="SampleModify.aspx?id={0}" Text="<%$ Resources:Site, btnEditText %>"
                    ItemStyle-HorizontalAlign="Center" />

                <asp:TemplateField ControlStyle-Width="50" HeaderText="<%$ Resources:Site, btnDeleteText %>"  ItemStyle-HorizontalAlign="Center">
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
        <table border="0" cellpadding="0" cellspacing="1" style="width: 200px; height: 100%;" class="def-wrapper">
            <tr>
                <td>
                    <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>"
                       OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>" class="adminsubmit" OnClick="btnDelete_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
