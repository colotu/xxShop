<%@ Page Title="订购单列表" Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master"
    CodeBehind="CartList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Order.SubmitOrder.CartList"
    ViewStateEncryptionMode="never" EnableEventValidation="False" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
           
            //商品总金额
            var totalSellPrice = parseFloat($('[id$="hidCartProdTotalSellPrice"]').val());
            //调整后总金额
            var totalAdjustedPrice = parseFloat($('[id$="hidCartProdTotalAdjustedPrice"]').val());
            var freight = parseFloat($('[id$="hidFreight"]').val());
            var promotionsPrice = totalSellPrice - totalAdjustedPrice;
            //if (totalSellPrice > 0) {
                var $parentDocument = $(window.parent.document);//显示页面内容
                $parentDocument.find('#totalSellPrice').text(totalSellPrice.toFixed(2));  //商品总金额
                $parentDocument.find('#promotionsPriceId').text(promotionsPrice > 0?promotionsPrice.toFixed(2):"0.00");//优惠金额
                $parentDocument.find('#freightPriceId').attr('BaseFreight', freight).text(freight.toFixed(2));//运费
                $parentDocument.find('#txtFreight').attr('BaseFreight', freight).val(freight.toFixed(2));//运费
                $parentDocument.find('#payPriceId').attr('BaseTotalAdjustedPrice', totalAdjustedPrice).text((totalAdjustedPrice + freight).toFixed(2)); //调整后总金额
            // }
         
            $(".btnDelete").die().live("click", function () {
                if (confirm("您确认要移除吗？")) {
                    var itemId = $(this).attr("ItemId");
                    $.ajax({
                        type: "POST",
                        dataType: "JSON",
                        url: "/ShoppingCart.aspx",
                        data: { Action: "RemoveItem", ItemIds: itemId },
                        success: function (resultData) {
                            if (resultData.STATUS == "SUCCESS") {
                                ShowSuccessTip("移除成功！");
                                document.location.reload();
                            } else {
                                ShowFailTip("服务器繁忙，请稍候再试！");
                                //setTimeout(function () { $("#LoadCartList").load("@(ViewBag.BasePath)ShoppingCart/CartList"); }, 3000);
                            }
                        }
                    });
                }
            });

            //商品数量修改
            $('.txtQuantity').OnlyNum();
            $('.txtQuantity').die("blur").live("blur", function () {
                var count = parseInt($(this).val());
                var sku = $(this).attr("sku");
                var self = $(this);
                var o_count = $(this).attr("count");
                var stock = $("#lblStock_" + sku, parent.document).attr("stock");
                if (count < 1) {
                    if (confirm("您确定要删除该商品吗？")) {
                        $(this).parents('tr').find('.btnDelete').click();
                        return;
                    }
                    $(this).val(1);
                    return;
                }
                if (count > stock) {
                    ShowFailTip("库存不足！");
                    $(this).val(o_count);
                    return;
                }
                if (count == o_count) {
                    return;
                }
                var itemId = $(this).attr("itemid");
                var productId = $(this).attr("productId");
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    url: "/ShoppingCart.aspx?s=" + new Date().format('yyyyMMddhhmmssS'),
                    data: { Action: "UpdateItemCount", ItemId: itemId, Count: count,ProductId:productId },
                    success: function (resultData) {
                        if (resultData.STATUS == "SUCCESS") {
                            document.location.reload();
                        }
                        else if (resultData.DATA == "NOSTOCK") {
                            self.val(o_count);
                            ShowFailTip("库存不足！");
                        } else if (resultData.DATA == "NOSKU") {
                            ShowFailTip("SKU不存在！");
                        }else {
                            ShowFailTip("服务器繁忙，请稍候再试！");
                        }
                    }
                });
            });



            //价格修改
            $('.txtPrice').OnlyFloat();
            $('.txtPrice').die("blur").live("blur", function () {
                var price = parseFloat($(this).val());
                if (price < 0) {
                    ShowFailTip("输入的价格不正确！");
                    return;
                }
                var itemId = $(this).attr("itemid");
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    url: "/ShoppingCart.aspx?s=" + new Date().format('yyyyMMddhhmmssS'),
                    data: { Action: "UpdateItemPrice", ItemId: itemId, Price: price },
                    success: function (resultData) {
                        if (resultData.STATUS == "SUCCESS") {
                            document.location.reload();
                        }else {
                            ShowFailTip("服务器繁忙，请稍候再试！");
                        }
                    }
                });
            });
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:GridViewEx ID="gridViewCart" runat="server" AllowPaging="True" AllowSorting="True"
        ShowToolBar="False" AutoGenerateColumns="False" OnBind="BindData" Width="100%"
        PageSize="1000" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
        CellPadding="3" BorderWidth="1px" ShowCheckAll="False" DataKeyNames="ItemId"
        Style="float: left;" ShowGridLine="true" ShowHeaderStyle="true">
        <Columns>
            <asp:TemplateField HeaderText="商品名称" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                   <div class="tx-l"><%#DataBinder.Eval(Container.DataItem,"Name")%></div>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="编码" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem,"SKU")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="库存" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%#GetStock(Eval("SKU"))%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="数量" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50">
                <ItemTemplate>
                    <input   productId="<%#DataBinder.Eval(Container.DataItem, "ProductId") %>" class="txtQuantity" type="text" value="<%#DataBinder.Eval(Container.DataItem, "Quantity")%>"
                        sku="<%#DataBinder.Eval(Container.DataItem,"SKU")%>" itemid="<%#DataBinder.Eval(Container.DataItem,"ItemId")%>"
                        style="width: 50px" count="<%#DataBinder.Eval(Container.DataItem, "Quantity")%>" />
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="原价" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60">
                <ItemTemplate>
                     <div class="tx-r"><%#DataBinder.Eval(Container.DataItem, "SellPrice", "{0:N2}")%></div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="单价" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60">
                <ItemTemplate>
                    <div class="tx-r"><input  productId="<%#DataBinder.Eval(Container.DataItem, "ProductId") %>" class="txtPrice" type="text" value="<%#DataBinder.Eval(Container.DataItem, "AdjustedPrice", "{0:N2}")%>"
                        sku="<%#DataBinder.Eval(Container.DataItem,"SKU")%>" itemid="<%#DataBinder.Eval(Container.DataItem,"ItemId")%>"
                        style="width: 50px" /></div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="小计" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60">
                <ItemTemplate>
                    ￥<%#decimal.Parse(DataBinder.Eval(Container.DataItem, "AdjustedPrice", "{0:N2}")) * int.Parse(DataBinder.Eval(Container.DataItem, "Quantity").ToString())%>
                </ItemTemplate>
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50">
                <ItemTemplate>
                    <a href="javascript:;" class="btnDelete" itemid="<%#DataBinder.Eval(Container.DataItem,"ItemId")%>">
                        移除</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <FooterStyle Height="25px" HorizontalAlign="Right" />
        <HeaderStyle Height="35px" />
        <PagerStyle Height="25px" HorizontalAlign="Right" />
        <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
        <RowStyle Height="25px" CssClass="skulist" />
        <SortDirectionStr>DESC</SortDirectionStr>
    </cc1:GridViewEx>
   <input type="hidden" id="hidCartProdTotalSellPrice"  runat="server"/>
   <input type="hidden" id="hidCartProdTotalAdjustedPrice"  runat="server"/>
    <input type="hidden" id="hidFreight"  runat="server"/>

     
</asp:Content>
