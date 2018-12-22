<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="ProductsList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Products.ProductsList" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/admin/js/jquery/maticsoft.img.min.js" type="text/javascript"></script>
       <link href="/Admin/js/select2-3.4.1/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-3.4.1/select2.min.js" type="text/javascript" charset="utf-8"></script>
    
    <script type="text/javascript">
        $(function() {
            $(".iframe").colorbox({ iframe: true, width: "680", height: "450", overlayClose: false });
            
        });
    </script>

            <style type="text/css">
        .autobrake
        {
            word-wrap: break-word;
            width: 280px;
        }
        .item-title-area
        {
            width: 300px;
        }
        .high-light
        {
            cursor: pointer;
        }
        .txtpname
        {
            width: 260px;
            display: none;
        }
        </style>
            </asp:Content>
        <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Title -->
   <div class="outer_width">     
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        商品管理
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <%=strTitle %>
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        

        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang borderkuang-noc">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal1" runat="server" Text="商品分类" />：
                    <asp:DropDownList ID="drpProductCategory" runat="server">
                    </asp:DropDownList>
                
               <%--        <asp:Literal ID="Literal6" runat="server" Text="销售类型" />：
                <asp:DropDownList ID="dropSalesType" runat="server" width="60">
                <asp:ListItem Value="0">全部</asp:ListItem>
                <asp:ListItem Value="1">正常</asp:ListItem>
               
                <asp:ListItem Value="3">赠品</asp:ListItem>
                    </asp:DropDownList>     --%>
                    <asp:Literal ID="Literal2" runat="server" Text="商品名称" />：
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    <asp:Literal ID="Literal3" runat="server" Text="商品编号" />：
                    <asp:TextBox ID="txtProductNum" runat="server"></asp:TextBox>
                      
<%--                      <asp:Literal ID="Literal4" runat="server" Text="警戒库存商品" />：
                    <asp:CheckBox ID="chkAlert" runat="server" />--%>
          <%--      <asp:Literal ID="Literal7" runat="server" Text="限购" />：
                    <asp:CheckBox ID="chkRest" runat="server" />--%>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit-short mar-le"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <div class="newslist mar-bt">
            <div class="newsicon">
                <ul>
                    <li style="width: 1px; padding-left: 0px"></li>
                    <li id="liDel" runat="server" style="padding-left: 0px; display: none;">
                        <asp:Button ID="Button1" OnClientClick="return confirm('你确定要放入回收站吗？要还原商品请到回收站找回！')"
                            runat="server" Text="批量删除" class="add-btn" OnClick="btnDelete_Click" />
                    </li>
                    <li style="padding-left: 0px">
                        <asp:Button ID="btnInverseApprove2" runat="server" Text="批量下架" class="add-btn"
                            OnClick="btnInverseApprove_Click" />
                    </li>
                </ul>
            </div>
        </div>
        <!--Search end-->
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" unexportedcolumnnames="Modify" Width="100%"
            PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="1" BorderWidth="1px" ShowCheckAll="true" DataKeyNames="ProductId"
            ShowToolBar="True"       >
            <Columns>
                
                <asp:TemplateField SortExpression="ProductName" ItemStyle-HorizontalAlign="Left"
                    HeaderText="商品名称" ControlStyle-Width="300">
                    <ItemTemplate >
                        <div class="tx-l"> <%# Eval("ProductName")%></div>
                    
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="市场价" ItemStyle-HorizontalAlign="right" SortExpression="MarketPrice" Visible="false">
                    <ItemTemplate>
                        <div id="MarketPrice_<%# Eval("ProductId")%>" class="tx-r">
                            <%#Eval("MarketPrice", "￥{0:N2}")%></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="销售价" ItemStyle-HorizontalAlign="right" SortExpression="LowestSalePrice" >
                    <ItemTemplate>
                        <div style="color: #f60;font-weight: 700"  id="LowestSalePrice_<%# Eval("ProductId")%>"  class="tx-r" >
                            <%#Eval("LowestSalePrice", "￥{0:N2}")%></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="所在分类">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="litProductCate"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="VistiCounts" ItemStyle-HorizontalAlign="Center"
                    HeaderText="浏览" >
                    <ItemTemplate>
                        <%#Eval("VistiCounts") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="库存">
                    <ItemTemplate>
                        <span id="StockNum_<%# Eval("ProductId")%>">
                            <%#StockNum(Eval("ProductId"))%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                 
                <asp:TemplateField SortExpression="AddedDate" ItemStyle-HorizontalAlign="Center"
                    HeaderText="发布时间" ControlStyle-Width="120">
                    <ItemTemplate>
                        <%#Convert.ToDateTime(Eval("AddedDate")).ToString("yyyy-MM-dd")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center"  >
                    <ItemTemplate>
                         <span id="productModify" runat="server" ><a  class="iframe-modf"  style="white-space: nowrap;" href="ModifyStock.aspx?pid=<%#Eval("ProductId") %>">[编辑库存]</a></span>
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
        <div></div>
        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;display:none">
            <tr>
                <td style="width: 5px;">
                </td>
                <td align="left" class="mar-t15">
                    <asp:Button ID="btnDelete" OnClientClick="return confirm('你确定要放入回收站吗？要还原商品请到回收站找回！')"
                        runat="server" Text="批量删除" class="adminsubmit-short mar-t15" OnClick="btnDelete_Click"  style="display: none;"/>
                    <asp:Button ID="btnInverseApprove" runat="server" Text="批量下架" class="adminsubmit-short add-btn mar-t15"
                        OnClick="btnInverseApprove_Click" />
                    <asp:Button ID="btnCheck" runat="server" Text="批量审核" class="adminsubmit-short add-btn mar-l40 mar-t15" OnClick="btnCheck_Click"  Visible="false"/>
                </td>
            </tr>
        </table>
    </div>
   </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.code_hover').hover(
                function() {
                    $(this).find('#code_img').show();
                },
                function() {
                    $(this).find('#code_img').hide();
                });
        });
    
    </script>
</asp:Content>
