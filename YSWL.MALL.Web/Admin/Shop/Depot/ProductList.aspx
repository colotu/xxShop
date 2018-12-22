<%@ Page Title="仓库商品" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="ProductList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Depot.ProductList" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <link href="/Admin/js/select2-3.4.1/select2.css" rel="stylesheet" type="text/css" />
    <style>
    .padd-no .select2-container .select2-choice{
        margin-right:0;
    }
    .padd-no .select2-container{
        margin-right:20px;
    }
    </style>
    <script src="/Admin/js/select2-3.4.1/select2.min.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("span:contains('已删除')").css("color", "red");
            $("span:contains('下架')").css("color", "red");
            $("span:contains('上架')").css("color", "#006400");
            $("[id$='ddlDepot']").select2({ placeholder: "请选择" });

            $('[id$="ltAdd"]').click(function () {
                var did = $("[id$='ddlDepot']").select2("val");
                if (isNaN(did) || parseInt(did) <= 0) {
                    ShowFailTip("请先选择对应的仓库");
                    return;
                }
                var url_s = "DepotProList.aspx?did=" + did;
                $.colorbox({ href: url_s, iframe: true, width: "1000", height: "700", overlayClose: false, onLoad: function () {
                    $('#cboxClose').click(function () {
                        $(parent.document).find('[id$=btnSearch]').click();
                    });
                }
                });

            });

            $('.stock_Input').OnlyNum();


        });


        function UpdateStock($this) {
            var sku = $this.hide().next().show().attr('sku');
            $("#span_stock_" + sku).hide();
            $("#TextStock_" + sku).show().focus();
        }

        function SaveStock($this) {
            var did = $("[id$='ddlDepot']").select2("val");
            if (isNaN(did) || parseInt(did) <= 0) {
                ShowFailTip("请先选择对应的仓库");
                return;
            }
            var sku = $this.attr('sku');
            var stock = $("#TextStock_" + sku).val()
            if (stock == "") {
                alert('请输入商品库存！');
                return;
            }
            if (isNaN(parseInt(stock)) || parseInt(stock) <= 0) {
                alert('请正确输入商品库存！');
                return;
            }
            $.ajax({
                url: ("ProductList.aspx?timestamp={0}").format(new Date().getTime()),
                type: 'POST', dataType: 'json', timeout: 10000,
                data: { Action: "UpdateStock", Callback: "true", sku: sku, stock: stock, did: did },
                success: function (resultData) {
                    if (resultData.STATUS == "SUCCESS") {
                        $("#span_stock_" + sku).text(stock).show();
                        $("#TextStock_" + sku).hide();
                        $this.hide().prev().show();
                    }
                    else {
                        alert("系统忙请稍后再试！");
                    }
                }
            });
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
                        <asp:Literal ID="Literal1" runat="server" Text="仓库商品信息管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以对仓库商品进行查看，库存设置，以及删除，下架等操作
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no mar-bt">
            <tr>
                <td height="35" class="newstitlebody">
                    <asp:Literal ID="LiteralDepot" runat="server" Text="仓库" />：
                    <asp:DropDownList ID="ddlDepot" runat="server" Width="200px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlDepot_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Literal ID="Literal2" runat="server" Text="商品名称" />：
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="add-btn"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <div class="newslist mar-bt" id="liAdd" runat="server" visible="false">
            <div class="newsicon">
                <ul>
                    <li class="add-btn" id="ltAdd"><a
                        href="javascript:;">
                        <asp:Literal ID="ld" runat="server" Text="新增" />
                    </a></li>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="true" AutoGenerateColumns="false" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowCommand="gridView_RowCommand" UnExportedColumnNames="Modify"
            Width="100%" PageSize="10" ShowExportExcel="false" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowCheckAll="false" DataKeyNames="SKU">
            <Columns>
                <%--<asp:BoundField DataField="ProductName" HeaderText="商品名称" ItemStyle-HorizontalAlign="left" />--%>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="商品名称" ItemStyle-HorizontalAlign="left">
                    <ItemTemplate>
                           <div class="tx-l">
                     <%#Eval("ProductName")%>  &nbsp;&nbsp;  <%#GetSKUStr(Eval("SKU"))%>
                               </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="SKU" HeaderText="商品编码" ItemStyle-HorizontalAlign="left"
                    ItemStyle-Width="150" />
                    <asp:TemplateField ControlStyle-Width="86" HeaderText="销售类型" ItemStyle-HorizontalAlign="left"  ItemStyle-Width="86">
                    <ItemTemplate>
                        <div style="width:86px;">
                             <%#GetSalesType(Eval("SalesType"))%>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Weight" HeaderText="商品重量" ItemStyle-HorizontalAlign="Center"  Visible="false" />

                

                <asp:TemplateField ControlStyle-Width="50" HeaderText="库存" ItemStyle-HorizontalAlign="right"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <span style="color: #f60; font-family: tahoma; font-weight: 700" id="span_stock_<%# Eval("sku")%>">
                            <%#Eval("Stock")%></span>
                        <input id="TextStock_<%# Eval("sku")%>" type="text" maxlength="7" class="stock_Input"
                            value="<%#Eval("Stock")%>" style="width: 70px; display: none;" />
                        &nbsp;
                        <asp:Image ID="imgStock" ToolTip="修改" AlternateText="修改" ImageUrl="/admin/Images/up_xiaobi.png"
                            Visible="False" OnClick="return UpdateStock($(this))" runat="server" Style="width: 24px;" />
                        <a id="aStock_<%# Eval("sku")%>" href="javascript:;" style="display: none;" onclick="SaveStock($(this))"
                            class="Stock_save" sku="<%# Eval("sku")%>">保存</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="AlertStock" HeaderText="警戒库存" ItemStyle-HorizontalAlign="Center"
                    Visible="false" />
                <asp:BoundField DataField="CostPrice" HeaderText="成本价" ItemStyle-HorizontalAlign="Center"
                    Visible="false" />
                <%--<asp:TemplateField ControlStyle-Width="50" HeaderText="销售价" ItemStyle-HorizontalAlign="right"
                    ItemStyle-Width="50">
                    <ItemTemplate>
                        <%#Eval("SalePrice", "{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField ControlStyle-Width="50" HeaderText="状态" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="50" Visible="False">
                    <ItemTemplate >
                        
                            <asp:LinkButton ID="lbtnID" runat="server" CausesValidation="False" CommandName="Status"
                            CommandArgument='<%#Eval("SKU")+","+Eval("SaleStatus")+","+Eval("Upselling")%>' Style="color: #0063dc;">
                            <span >
   <%#GetSaleStatus(Eval("SaleStatus"), Eval("Upselling"))%>
</span>
                            </asp:LinkButton>
                    
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" Visible="false"  ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:LinkButton ID="linkDel1" runat="server" OnClientClick="return confirm('是否确定删除？')"
                            CommandArgument='<%#Eval("SKU")%>' CommandName="Del" ConfirmText="<%$Resources:Site,TooltipDelConfirm %>"
                            Text="<%$ Resources:Site, btnDeleteText %>">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="25px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
            <SortDirectionStr>DESC</SortDirectionStr>
        </cc1:GridViewEx>
        <%--<table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;" class="def-wrapper">
            <tr>
                <td>
                    <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>"
                        class="adminsubmit" OnClick="btnDelete_Click"  Visible="false"/>
                </td>
            </tr>
        </table>--%>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
