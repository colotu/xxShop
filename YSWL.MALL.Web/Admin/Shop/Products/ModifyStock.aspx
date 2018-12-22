<%@ Page Title="编辑商品" Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master" AutoEventWireup="true" CodeBehind="ModifyStock.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Products.ModifyStock" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="/admin/css/gridviewstyle.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <link href="/admin/css/productstyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            var skuTr = '<tr  class="skuInfo"><td    class="td td_class"  >{0}</td><td class="td">{1} </td><td class="td" >' +
                '<input id="sku_{0}"  style="width:60px" type="text" value="{2}" > </td></tr>';

            var isOpenSku = $("#ctl00_ContentPlaceHolder1_hfIsOpenSku").val().toLowerCase();

            if (isOpenSku == "true") {
                $("#SKUsTR").show();
            }
            if ($("[id$='hidSkuJson']").val() != null && $("[id$='hidSkuJson']").val().length > 0) {
                var hidSkuJson = eval($("[id$='hidSkuJson']").val());
                var haveSku = $("[id$='hidHasSku']").val();
                if (isOpenSku == "true" && haveSku) {//开启了sku循环sku列表
                    //todo 
                    $.each(hidSkuJson, function (index, value) {
                        $("#SKUsTR").after($(skuTr.format(value.sku, value.skuValues, value.Stock)));
                    });
                    $('#SKUsTR').show();
                } else {
                    //未开启sku   显示单个sku
                    $('[id$="txtStock"]').text(hidSkuJson[0].Stock);
                    $('#haveSku').show();
                }
            }


        });

        function SubForm() {
            var isOpenSku = $("#ctl00_ContentPlaceHolder1_hfIsOpenSku").val().toLowerCase();
            var hidSkuJson = eval($("[id$='hidSkuJson']").val());
            var haveSku = $("[id$='hidHasSku']").val();
            var SKUInfos = [];
            if (isOpenSku == "true" && haveSku) { //开启了sku循环sku列表
                var errorNum = 0;
                $.each(hidSkuJson, function(index, value) {
                    var stock = parseInt($("#sku_" + value.sku).val());
                    if (isNaN(stock) || stock < 0) {
                        errorNum = errorNum + 1;
                    }
                    SKUInfos.push({
                        "SKU": value.sku,
                        "Stock": stock
                    });
                });
                if (errorNum > 0) {
                    alert("请输入正常的库存数");
                    return false;
                }
            } else {
                var stock = $("#txtStock").val();
                if (!stock || stock < 0) {
                    alert("请输入正常的库存数");
                    return false;
                }
                SKUInfos.push({
                    "SKU": hidSkuJson[0].sku,
                    "Stock": stock
                });
            }
            $('[id$="Hidden_TempSKUInfo"]').val(JSON.stringify(SKUInfos));
            alert("123");
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HiddenField_RelatedProductInfo" runat="server" />
    <input type="hidden" id="hidden_IsFirstLoad" value="1" />
    <asp:HiddenField ID="Hidden_TempSKUInfo" runat="server" />
    <asp:HiddenField ID="hfHasSku" runat="server" />
    <input type="hidden" value="" id="hfCurrentProductType" />
    <div class="newslistabout">
        <asp:HiddenField ID="hfIsOpenSku" runat="server" Value="True" />
        <asp:HiddenField ID="hidHasSku" runat="server" Value="True" />
        <input type="hidden" id="hidSkuJson" runat="server" />
        <table style="width: 100%; border-bottom: none;  float: left;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0" class="formTR">
                        <tr>
                            <td class="td_class" clospan="2">
                                <em>*</em>商品名称 ：
                            </td>
                            <td height="25">
                                <asp:Literal ID="litProductName" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr class="haveSku" style="display: none;">
                            <td class="td_class">
                                <em>*</em>商品库存 ：
                            </td>
                            <td height="25">
                                <input ID="txtStock" Class="OnlyNum" Width="200px" MaxLength="6" Text="0"/>
                                
                            </td>
                        </tr>
                        <tr id="SKUsTR" style="display: none;" >
                       
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div class="newslistabout">

        <table style="width: 100%; border-top: none; float: left;" cellpadding="2" cellspacing="1">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td style="height: 6px;"></td>
                            <td height="6"></td>
                        </tr>
                        <tr>
                            <td class="td_class"></td>
                            <td height="25">
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_OnClick" Text="<%$ Resources:Site, btnSaveText %>" class="adminsubmit_short" OnClientClick="return SubForm();"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
