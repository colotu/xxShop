<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="ProductsInStock.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Products.ProductsInStock" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/admin/js/jquery/maticsoft.img.min.js" type="text/javascript"></script>
       <link href="/Admin/js/select2-3.4.1/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-3.4.1/select2.min.js" type="text/javascript" charset="utf-8"></script>
    
    <script type="text/javascript">
        function changProductName(id) {
            $("#img_" + id).hide();
            $("#txtProductName_" + id).show().focus();
            $("#editsave_" + id).show();
            $("#p_" + id).hide();
        }

        function UpdateStock(id) {
            $("#imgStockNum_" + id).hide();
            $("#TextStockNum_" + id).show().focus();
            $("#aStockNum_" + id).show();
            $("#StockNum_" + id).hide();
        }

        function SaveStockNum(id) {
            var StockNum = $("#TextStockNum_" + id).val();
            if (StockNum == "") {
                alert('请输入库存数量！');
                return;
            }
            $.ajax({
                url: ("ProductsInStock.aspx?timestamp={0}").format(new Date().getTime()),
                type: 'POST', dataType: 'json', timeout: 10000,
                data: { Action: "UpdateStockNum", Callback: "true", ProductId: id, UpdateValue: StockNum },
                success: function (resultData) {
                    if (resultData.STATUS == "SUCCESS") {
                        $("#StockNum_" + id).text(StockNum);
                        $("#imgStockNum_" + id).show();
                        $("#TextStockNum_" + id).hide();
                        $("#aStockNum_" + id).hide();
                        $("#StockNum_" + id).show();
                    }
                    else {
                        alert("系统忙请稍后再试！");
                    }
                }
            });
        }

        function UpdateLowestSalePrice(id) {
            $("#imgLowestSalePrice_" + id).hide();
            $("#TextLowestSalePrice_" + id).show().focus();
            $("#aLowestSalePrice_" + id).show();
            $("#LowestSalePrice_" + id).hide();
        }

        function SaveLowestSalePrice(id) {
            var LowestSalePrice = $("#TextLowestSalePrice_" + id).val();
            if (LowestSalePrice == "") {
                alert('请输入商品销售价！');
                return;
            }
            $.ajax({
                url: ("ProductsInStock.aspx?timestamp={0}").format(new Date().getTime()),
                type: 'POST', dataType: 'json', timeout: 10000,
                data: { Action: "UpdateLowestSalePrice", Callback: "true", ProductId: id, UpdateValue: LowestSalePrice },
                success: function (resultData) {
                    if (resultData.STATUS == "SUCCESS") {
                        $("#LowestSalePrice_" + id).text("￥" + LowestSalePrice);
                        $("#imgLowestSalePrice_" + id).show();
                        $("#TextLowestSalePrice_" + id).hide();
                        $("#aLowestSalePrice_" + id).hide();
                        $("#LowestSalePrice_" + id).show();

                    }
                    else {
                        alert("系统忙请稍后再试！");
                    }
                }
            });
        }

        function UpdateMarketPrice(id) {
            $("#imgMarketPrice_" + id).hide();
            $("#TextMarketPrice_" + id).show().focus();
            $("#aMarketPrice_" + id).show();
            $("#MarketPrice_" + id).hide();
        }

        function SaveMarketPrice(id) {
            var MarketPrice = $("#TextMarketPrice_" + id).val();
            if (MarketPrice == "") {
                alert('请输入市场价！');
                return;
            }
            $.ajax({
                url: ("ProductsInStock.aspx?timestamp={0}").format(new Date().getTime()),
                type: 'POST', dataType: 'json', timeout: 10000,
                data: { Action: "UpdateMarketPrice", Callback: "true", ProductId: id, UpdateValue: MarketPrice },
                success: function (resultData) {
                    if (resultData.STATUS == "SUCCESS") {

                        $("#MarketPrice_" + id).text("￥" + MarketPrice);
                        $("#imgMarketPrice_" + id).show();
                        $("#TextMarketPrice_" + id).hide();
                        $("#aMarketPrice_" + id).hide();
                        $("#MarketPrice_" + id).show();

                    }
                    else {
                        alert("系统忙请稍后再试！");
                    }
                }
            });
        }

        function saveChange(id) {
            var productName = $("#txtProductName_" + id).val();
            if (!productName) {
                alert('请输入商品名称！');
                return;
            }
            $.ajax({
                url: ("ProductsInStock.aspx?timestamp={0}").format(new Date().getTime()),
                type: 'POST', dataType: 'json', timeout: 10000,
                data: { Action: "UpdateProductName", Callback: "true", ProductId: id, UpdateValue: productName },
                async: false,
                success: function (resultData) {
                    if (resultData.STATUS == "SUCCESS") {
                        $("#p_" + id).text(productName);
                        $("#img_" + id).show();
                        $("#editsave_" + id).hide();
                        $("#txtProductName_" + id).hide();
                        $("#p_" + id).show();
                    }
                    else {
                        alert("系统忙请稍后再试！");
                    }
                }
            });
        }

        $(document).ready(function () {

            $("[id$='ddlSupplier']").select2({ placeholder: "请选择" });
            $(".select2-container").css("vertical-align", "middle");
            resizeImg('.borderImage', 80, 80);
            $(".StockNum_Input").blur(function () {
                StockBulr(this);
            });

            $(".MarketPrice_Input").blur(function () {
                $(this).hide();
                var id = $(this).attr('i');
                $("#imgMarketPrice_" + id).show();
                $("#TextMarketPrice_" + id).hide();
                $("#aMarketPrice_" + id).hide();
                $("#MarketPrice_" + id).show();
            });

            $(".LowestSalePrice_Input").blur(function () {
                $(this).hide();
                var id = $(this).attr('i');
                $("#imgLowestSalePrice_" + id).show();
                $("#TextLowestSalePrice_" + id).hide();
                $("#aLowestSalePrice_" + id).hide();
                $("#LowestSalePrice_" + id).show();
            });

            $(".item-title-area").hover(function () {
                $(this).addClass('high-light');
            }, function () {
                $(this).removeClass("high-light");
            });
            $(".txtpname").blur(function () {
                textBulr(this);
            });
            $(".editsave").mouseenter(function () {
                var id = $(this).attr('i');
                $("#txtProductName_" + id).unbind('blur');
                $("#TextStockNum_" + id).unbind('blur');
                $("#TextLowestSalePrice_" + id).unbind('blur');
                $("#TextMarketPrice_" + id).unbind('blur');

            }).mouseleave(function () {
                var id = $(this).attr('i');
                $("#txtProductName_" + id).bind('blur', function () {
                    $("#txtProductName_" + id).hide();
                    $("#img_" + id).show();
                    $("#editsave_" + id).hide();
                    $("#p_" + id).show();
                });

            });


            $(".LowestSalePrice_save").mouseenter(function () {
                var id = $(this).attr('i');
                $("#TextLowestSalePrice_" + id).unbind('blur');

            }).mouseleave(function () {
                var id = $(this).attr('i');
                $("#TextLowestSalePrice_" + id).bind('blur', function () {
                    $("#imgLowestSalePrice_" + id).show();
                    $("#TextLowestSalePrice_" + id).hide();
                    $("#aLowestSalePrice_" + id).hide();
                    $("#LowestSalePrice_" + id).show();

                });
            });

            $(".MarketPrice_save").mouseenter(function () {
                var id = $(this).attr('i');
                $("#TextMarketPrice_" + id).unbind('blur');

            }).mouseleave(function () {
                var id = $(this).attr('i');
                $("#TextMarketPrice_" + id).bind('blur', function () {
                    $("#imgMarketPrice_" + id).show();
                    $("#TextMarketPrice_" + id).hide();
                    $("#aMarketPrice_" + id).hide();
                    $("#MarketPrice_" + id).show();
                });
            });

            // $(".iframe").colorbox({ iframe: true, width: "880", height: "720", overlayClose: false });


        });

        function textBulr(thisenent) {
            $(thisenent).hide();
            var id = $(thisenent).attr('i');
            $("#img_" + id).show();
            $("#editsave_" + id).hide();
            $("#p_" + id).show();
        }
        function StockBulr(thisenent) {
            $(thisenent).hide();
            var id = $(thisenent).attr('i');
            $("#imgStockNum_" + id).show();
            $("#TextStockNum_" + id).hide();
            $("#aStockNum_" + id).hide();
            $("#StockNum_" + id).show();
        }
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
       <div class="">  
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no">
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
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="mar-bt padd-no">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal1" runat="server" Text="商品分类" />：
                    <asp:DropDownList ID="drpProductCategory" runat="server">
                    </asp:DropDownList>
                     <asp:Literal ID="litSupplier" runat="server" Text="商家："  Visible="false" />
                    <asp:DropDownList ID="ddlSupplier" runat="server" width="150" Visible="false" >
                    </asp:DropDownList>
                       <asp:Literal ID="litSalesType" runat="server" Text="销售类型：" Visible="false" />
                <asp:DropDownList ID="dropSalesType" runat="server" Visible="false" >
                <asp:ListItem Value="0">全部</asp:ListItem>
                <asp:ListItem Value="1">正常</asp:ListItem>
                <%--<asp:ListItem  Value="2">预定</asp:ListItem> --%>            
                <asp:ListItem Value="3">赠品</asp:ListItem>
                    </asp:DropDownList>
  
                    <asp:Literal ID="Literal2" runat="server" Text="商品名称" />：
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    <asp:Literal ID="Literal3" runat="server" Text="商品编号" />：
                    <asp:TextBox ID="txtProductNum" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit-short"></asp:Button>
                </td>
            </tr>
        </table>
        <div class="mar-bt" >
             <asp:Button ID="btnDelete2" OnClientClick="return confirm('你确定要放入回收站吗？要还原商品请到回收站找回！')"
                            runat="server" Text="批量删除" class="add-btn" OnClick="btnDelete_Click" />
                           <asp:Button ID="btnInverseApprove2" runat="server" Text="批量下架" class="add-btn"
                            OnClick="btnInverseApprove_Click" />
            <asp:Button ID="btnCheck" runat="server" Text="批量审核" class="add-btn mar-t10" OnClick="btnCheck_Click"  Visible="false"/>
        </div>
        <!--Search end-->
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" unexportedcolumnnames="Modify" Width="100%"
            PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="1" BorderWidth="1px" ShowCheckAll="true" DataKeyNames="ProductId"
            ShowToolBar="True"  OnRowCommand="gridView_RowCommand"     >
            <Columns>

                <asp:TemplateField SortExpression="ProductName" ItemStyle-HorizontalAlign="Left"
                    HeaderText="商品名称" ControlStyle-Width="300">
                    <ItemTemplate>
  
                        
                        <div class="tx-l"> <%# Eval("ProductName")%></div>
                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="市场价" ItemStyle-HorizontalAlign="right" SortExpression="MarketPrice" Visible="false">
                    <ItemTemplate>
                        <div id="MarketPrice_<%# Eval("ProductId")%>" class="tx-r">
                            <%#Eval("MarketPrice", "￥{0:N2}")%></div>

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="销售价" ItemStyle-HorizontalAlign="right" SortExpression="LowestSalePrice">
                    <ItemTemplate>

                        <div style="color: #f60;font-weight: 700"  id="LowestSalePrice_<%# Eval("ProductId")%>"  class="tx-r" >
                            <%#Eval("LowestSalePrice", "￥{0:N2}")%></div>

                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Gwjf" ItemStyle-HorizontalAlign="Center" HeaderText="所扣积分">
                    <ItemTemplate>
                         <%# Eval("Gwjf")%>
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

                 <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="限购数" Visible="false">
                    <ItemTemplate> 
                            <%#Eval("RestrictionCount")%> 
                        </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField  ItemStyle-HorizontalAlign="Center"
                    HeaderText="商家" Visible="false">
                    <ItemTemplate>
                        <%#GetSupplier(Eval("SupplierId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="AddedDate" ItemStyle-HorizontalAlign="Center"
                    HeaderText="发布时间" ControlStyle-Width="120">
                    <ItemTemplate>
                        <%#Convert.ToDateTime(Eval("AddedDate")).ToString("yyyy-MM-dd")%>
                    </ItemTemplate>
                </asp:TemplateField>
<%--                       <asp:TemplateField HeaderText="二维码" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <div class="code_hover mar-l25" style="background: url(/admin/images/qr.png)  no-repeat;width: 37px;height: 48px;cursor: pointer;">
                            <div id="code_img" style="display: none;width: 120px;
height: 122px;
background-color: white;
position: relative;
border: 1px solid silver;
padding-top: 5px;
top: -50px;
left: -42px;"><img style="margin: 0 auto;display: block;" src="/Upload/Shop/QR/Product/<%# Eval("ProductId")%>.png" width="100px" height="100px"/>
<span>扫描或右键另存</span></div>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>--%>
         
                <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center"  Visible="false" >
                    <ItemTemplate>
                         <span id="productModify" runat="server" ><a   style="white-space: nowrap;" href="ProductModify.aspx?pid=<%#Eval("ProductId") %>">[编辑]</a><br /></span>
                        <span id="productParts" runat="server" visible="false"><a   class="iframe" style="white-space: nowrap;" href="/admin/shop/ProductAccessories/List.aspx?pid=<%#Eval("ProductId") %>&acctype=1">[组合配件]</a><br/></span>
                        <span id="hlinkProductAcce" runat="server" visible="false"><a   class="iframe" style="white-space: nowrap;" href="/admin/shop/ProductAccessories/List.aspx?pid=<%#Eval("ProductId") %>&acctype=2">[组合优惠]</a><br/></span>
                     <asp:LinkButton Visible="false" id="productSync" runat="server"  CommandArgument='<%#Eval("productId")%>'
                            CommandName="Sync" Text="[同步]">
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
        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;display:none">
            <tr>
                <td style="width: 5px;">
                </td>
                <td align="left" class="pad-t10">
                    <asp:Button ID="btnDelete" OnClientClick="return confirm('你确定要放入回收站吗？要还原商品请到回收站找回！')"
                        runat="server" Text="批量删除" class="add-btn mar-t10" OnClick="btnDelete_Click" style="display: none;"/>
                    <asp:Button ID="btnInverseApprove" runat="server" Text="批量下架" class="add-btn mar-t10"
                        OnClick="btnInverseApprove_Click" />
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
