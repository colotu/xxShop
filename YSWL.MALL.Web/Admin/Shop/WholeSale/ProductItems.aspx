<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master" AutoEventWireup="true"
    CodeBehind="ProductItems.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.WholeSale.ProductItems" %>
<%@ Import Namespace="YSWL.MALL.Web" %>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/admin/css/gridviewstyle.css" rel="stylesheet" type="text/css" />
    <script src="/admin/js/jquery/maticsoft.img.min.js" type="text/javascript"></script>
    <script src="/admin/js/jquery/jquery.scrollTo-min.js" type="text/javascript"></script>
    <script src="/admin/js/jquery/RuleProduct.helper.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            resizeImg('.borderImage', 80, 80);
        });
    </script>
    <style type="text/css">
        .borderImage
        {
            width: 81px;
            height: 81px;
            border: 1px solid #CCC;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="background: white; width: 100%" id="relatedProc">
        <div class="advanceSearchArea clearfix">
            <!--预留显示高级查询项区域-->
        </div>
        <div class="toptitle">
            <h3 class="title_height" style="margin-bottom: 5px">
            </h3>
        </div>
        <div class="Goodsgifts">
            <div class="left">
                <h1>
                    <asp:Literal ID="litDesc" runat="server" Text="设置规则应用商品"></asp:Literal></h1>
                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                    <ul>
                        <li>
                            <asp:Literal runat="server" ID="LitProductCategories" Text="分类" />：
                            <abbr class="formselect">
                                <asp:DropDownList ID="drpProductCategory" runat="server">
                                </asp:DropDownList>
                            </abbr>
                        </li>
                        <li>
                            <asp:Literal runat="server" ID="LitProductName" Text="商品名称" />：
                            <asp:TextBox ID="txtProductName" runat="server" Width="120px" />
                        </li>
                        <li style="display: none;">
                            <asp:Literal runat="server" ID="litProductNum" Text="商品编号" />：
                            <asp:TextBox ID="txtProductNum" runat="server" />
                        </li>
                        <li>
                            <asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" Text="查询" CssClass="add-btn" />
                        </li>
                    </ul>
                    <ul>
                        <li>
                            状态:<asp:CheckBox ID="chkStatus" runat="server" Checked="True" />上架
                            <asp:Button ID="btnAddAll" OnClick="btnAddAll_Click" runat="server" Text="一键加入" CssClass="add-btn mar-l10" />
                        </li>
                    </ul>
                </asp:Panel>
                <div class="content">
                    <div class="youhuiproductlist searchproductslist">
                        <asp:HiddenField ID="hfCurrentAllData" runat="server" />
                        <asp:DataList runat="server" ID="dlstSearchProducts" Width="96%" DataKeyField="ProductId"
                            RepeatLayout="Table">
                            <ItemTemplate>
                                <table width="100%" border="0" cellspacing="0" class="conlisttd" productid="<%# Eval("ProductId") %>"
                                    productname="<%# Eval("ProductName") %>">
                                    <tr>
                                        <td width="14%" rowspan="3" class="img">
                                            <div class="borderImage">
                                                <%--<a href="<%= YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.Shop) %>product/Detail/<%# Eval("ProductId") %>" target="_blank">
                                                    <img id="ThumbnailUrl40" src="<%# @YSWL.MALL.Web.Components.FileHelper.GeThumbImage(YSWL.Common.Globals.SafeString(Eval("ThumbnailUrl1"),""), "T128X130_") %>"
                                                        width="80px" height="80px" /></a></div>--%>
                                                <img id="ThumbnailUrl40" src="<%# @YSWL.MALL.Web.Components.FileHelper.GeThumbImage(YSWL.Common.Globals.SafeString(Eval("ThumbnailUrl1"),""), "T128X130_") %>"
                                                        width="80px" height="80px" />
                                        </td>
                                        <td height="27" colspan="5" class="br_none">
                                            <span class="Name"><a href='/Product/Detail/<%# Eval("ProductId") %>' target="_blank">
                                                <%# Eval("ProductName") %></a> </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="28" valign="top">
                                            <span class="colorC">最低价：<%# Eval("LowestSalePrice", "{0:n2}")%></span>
                                        </td>
                                        <td width="11%" align="right" valign="top">
                                            &nbsp;
                                        </td>
                                        <td width="14%" align="left" valign="top" class="a_none">
                                            &nbsp;
                                        </td>
                                        <td width="15%" valign="top">
                                            <a href="javascript:void(0);"><span class="submit_add">新增</span></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            编号：<%# Eval("ProductCode") %>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                    <div class="r">
                        <div style="display: none;">
                            <asp:Button runat="server" ID="btnAddSearch" CssClass="adminsubmit" Text="全部新增" />
                        </div>
                        <div class="pagination">
                            <webdiyer:AspNetPager runat="server" ID="anpSearchProducts" CssClass="anpager" CurrentPageButtonClass="cpb"
                                OnPageChanged="AspNetPager_PageChanged" PageSize="15" FirstPageText="<%$Resources:Site,FirstPage %>"
                                LastPageText="<%$Resources:Site,EndPage %>" NextPageText="<%$Resources:Site,GVTextNext %>"
                                PrevPageText="<%$Resources:Site,GVTextPrevious %>" ShowPageIndexBox="Never" NumericButtonCount="5">
                            </webdiyer:AspNetPager>
                        </div>
                    </div>
                </div>
            </div>
            <div class="right">
                <h1>
                    已新增的商品</h1>
                <ul>
                    <li>
                        <asp:Button runat="server" ID="btnClear" CssClass="adminsubmit" Text="清空列表" OnClick="btnClear_Click" />
                    </li>
                </ul>
                <div class="content">
                    <div class="youhuiproductlist addedproductslist">
                        <asp:HiddenField ID="hfSelectedData" runat="server" />
                        <asp:DataList runat="server" ID="dlstAddedProducts" Width="96%" DataKeyField="ProductId"
                            RepeatLayout="Table">
                            <ItemTemplate>
                                <table width="100%" border="0" cellspacing="0" class="conlisttd">
                                    <tr>
                                        <td width="14%" rowspan="3" class="img">
                                            <div class="borderImage">
                                                <a href="<%= YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.Shop) %>product/Detail/<%# Eval("ProductId") %>" target="_blank">
                                                    <img id="Img1" src="<%# @YSWL.MALL.Web.Components.FileHelper.GeThumbImage(YSWL.Common.Globals.SafeString(Eval("ThumbnailUrl1"),""), "T128X130_") %>"
                                                        width="80px" height="80px" /></a></div>
                                        </td>
                                        <td height="27" colspan="5" class="br_none">
                                            <span class="Name"><a href='/Product/Detail/<%# Eval("ProductId") %>' target="_blank">
                                                <%# Eval("ProductName") %></a> </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="28" valign="top">
                                            <span class="colorC">最低价：<%# Eval("LowestSalePrice", "{0:n2}")%></span>
                                        </td>
                                        <td width="11%" align="right" valign="top">
                                            &nbsp;
                                        </td>
                                        <td width="14%" align="left" valign="top" class="a_none">
                                            &nbsp;
                                        </td>
                                        <td width="15%" valign="top">
                                            <a href="javascript:void(0);"><span class="submit_del" productid="<%# Eval("ProductId") %>">
                                                删除</span></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            编号：<%# Eval("ProductCode") %>
                                        </td>
                                    </tr>
                                    <%--<tr>
                          <td colspan="5">
                              <asp:Repeater ID="rptSKUItems" runat="server">
                                  <ItemTemplate>
                                      <div id="Div1" class="specdiv"><%# Eval("ValueStr")%></div>
                                  </ItemTemplate>
                              </asp:Repeater>
                          </td>
                      </tr>--%>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                    <div class="r">
                        <div>
                            &nbsp;</div>
                        <div class="pagination">
                            <webdiyer:AspNetPager runat="server" ID="anpAddedProducts" CssClass="anpager" CurrentPageButtonClass="cpb"
                                OnPageChanged="AspNetPager_PageChanged" PageSize="15" FirstPageText="<%$Resources:Site,FirstPage %>"
                                LastPageText="<%$Resources:Site,EndPage %>" NextPageText="<%$Resources:Site,GVTextNext %>"
                                PrevPageText="<%$Resources:Site,GVTextPrevious %>" ShowPageIndexBox="Never" NumericButtonCount="5">
                            </webdiyer:AspNetPager>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="bntto">
            <%--<input type="button" id="btnOK" value="确定" class="adminsubmit_short" />--%>
        </div>
    </div>
</asp:Content>
