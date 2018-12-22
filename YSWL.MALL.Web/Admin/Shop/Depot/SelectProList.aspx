
<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master" AutoEventWireup="true"  CodeBehind="SelectProList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Depot.SelectProList" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>
<%@ Register TagPrefix="YSWL" TagName="CategoriesDropList" Src="~/Controls/CategoriesDropList.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/admin/css/gridviewstyle.css" rel="stylesheet" type="text/css" />
    <script src="/admin/js/jquery/maticsoft.img.min.js" type="text/javascript"></script>
    <script src="/admin/js/jquery/jquery.scrollTo-min.js" type="text/javascript"></script>
    <script src="/admin/js/jquery/DepotProduct.helper.js?t=1234" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            resizeImg('.borderImage', 80, 80);
 
        });
    </script>
    <style type="text/css">
        .borderImage{width:81px;height: 81px;border: 1px solid #CCC;text-align: center;}
        .cate_ff div{display: initial;}
         .cate_ff div select{margin-right: 1px;}
         .margtop{margin-top: 5px;}
    </style>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    
    <div  style="background: white;width: 100%" id="relatedProc">
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
                <asp:Literal ID="litDesc" runat="server"></asp:Literal></h1>
                <asp:panel id="Panel1" runat="server" defaultbutton="btnSearch">
                <div> 
                         <div  class="margtop  cate_ff" >  
                              <asp:Literal runat="server" ID="LitProductCategories" Text="分&#12288;&#12288;类" />：
                              <YSWL:CategoriesDropList ID="drpProductCategory" runat="server" IsNull="true" />  
                          </div>
                        <div class="margtop">
                                <asp:Literal runat="server" ID="LitProductName" Text="商品名称" />：
                            <asp:textbox id="txtProductName" runat="server"/> 
                              <asp:button id="btnSearch" OnClick="btnSearch_Click" runat="server" text="查询" cssclass="adminsubmit_short" />
                              <input type="hidden" runat="server"  id="hiddepotId"/>
                         </div>
                    </div>
                    <ul>
                    </ul>
                </asp:panel>
                <div class="content">
                    <div class="youhuiproductlist searchproductslist">
                   
                        <asp:DataList runat="server" ID="dlstSearchProducts" Width="96%" DataKeyField="ProductId" RepeatLayout="Table" OnItemDataBound="dlstSearchProducts_ItemDataBound">
                            <ItemTemplate>
                    <table width="100%" border="0" cellspacing="0" class="conlisttd" skuid="<%# Eval("ProductId") %>">
                         <tr>
                           <td width="14%" rowspan="3" class="img">
                                 <div class="borderImage">
                                  <a href="<%= YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.Shop) %>product/Detail/<%# Eval("ProductId") %>" target="_blank"><img  width="80px" height=80px id="ThumbnailUrl40" src="<%# @YSWL.MALL.Web.Components.FileHelper.GeThumbImage(YSWL.Common.Globals.SafeString(Eval("ThumbnailUrl1"),""), "T128X130_") %>" /></a></div>
                            </td>
                            <td height="27" colspan="5" class="br_none">
                            <span class="Name">
                                <a href='/Product/Detail/<%# Eval("ProductId") %>' target="_blank"><%# Eval("ProductName") %>  </a>
                            </span>
                            <span style="display: inline-block;margin-top: 5px;">
                                编码：<%# Eval("sku") %> &nbsp;&nbsp; <%#GetSKUStr(Eval("SKU")) %>
                            </span>
                            </td>
                          </tr>
                       <tr>
                        <td width="29%" height="28" valign="top"><span class="colorC">销售价：<%# Eval("SalePrice", "{0:n2}")%></span></td>
                        <td width="29%" valign="top"> 库存：<input id="i_stock"  sku="<%# Eval("sku") %>"  maxlength="7" style="width:60px;" type="text"  value="9999"  /> <span id="span_stock" style="display:none;"></span></td>
                        <td width="8%" align="right" valign="top">&nbsp;</td>
                        <td width="7%" align="left" valign="top" class="a_none">&nbsp;</td>
                        <td width="15%" valign="top"><a href="javascript:;"><span  runat="server" id="lbtnAdd" class="submit_add">新增</span></a></td>
                      </tr>
                   </table>
                </itemtemplate>
                        </asp:DataList>
                    </div>
                    <div class="r">
                        <div style="display:none;">
                            <asp:button runat="server" id="btnAddSearch" cssclass="adminsubmit" text="全部新增" />
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
                  <li >
                  &nbsp;&nbsp;
                    <%--<asp:button runat="server" id="btnClear" cssclass="adminsubmit" text="清空列表" 
                            /> --%>
                    </li>
                </ul>
                <div class="content">
                    <div class="youhuiproductlist addedproductslist">
                   
                        <asp:DataList runat="server" id="dlstAddedProducts"  width="96%" datakeyfield="ProductId" repeatlayout="Table" OnItemDataBound="dlstAddedProducts_ItemDataBound">
                            <itemtemplate>
                     <table width="100%" border="0" cellspacing="0" class="conlisttd" skuid="<%# Eval("ProductId") %>">
                        <tr>
                            <td width="14%" rowspan="3" class="img">
                                  
                                <div class="borderImage"><img id="Img1" width="80px" height="80px" src="<%# @YSWL.MALL.Web.Components.FileHelper.GeThumbImage(YSWL.Common.Globals.SafeString(Eval("ThumbnailUrl1"),""), "T128X130_") %>" /></div>
                            </td>
                            <td height="27" colspan="5" class="br_none"><span class="Name">
                                <a href='/Product/Detail/<%# Eval("ProductId") %>' target="_blank"><%# Eval("ProductName") %></a>
                            </span>
                             <span style="display: inline-block;margin-top: 5px;">
                                编码：<%# Eval("sku") %> &nbsp;&nbsp; <%#GetSKUStr(Eval("SKU")) %>
                            </span>
                            </td>
                          </tr>
                       <tr>
                        <td width="29%" height="28" valign="top"><span class="colorC">销售价：<%# Eval("SalePrice", "{0:n2}")%></span></td>
                        <td width="29%" valign="top"> 库存：<span id="span_stock" ><%# Eval("Stock") %></span><input id="i_stock"  sku="<%# Eval("sku") %>"  maxlength="7" style="width:60px;display:none;" type="text"  value="9999"  /> </td>
                        <td width="8%" align="right" valign="top">&nbsp;</td>
                        <td width="7%" align="left" valign="top" class="a_none">&nbsp;</td>
                        <td width="15%" valign="top"><a href="javascript:;"><span runat="server" id="lbtnDel"   > <span class="submit_del" sku="<%# Eval("sku") %>">删除</span></span></a></td>
                      </tr>
                     
                     </table>
                </itemtemplate>
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
         
        </div>
    </div>
</asp:content>
