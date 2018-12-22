<%@ Page Title="用户代下单功能" Language="C#" MasterPageFile="~/Admin/Basic.Master" CodeBehind="ProductSKUList.aspx.cs"
    Inherits="YSWL.MALL.Web.Admin.Shop.Order.SubmitOrder.ProductSKUList" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <link href="/Admin/js/select2-3.4.6/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-3.4.6/select2.min.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        $(function () {
            $(".select2-container").css("vertical-align", "middle");
            $(".txtCount").OnlyNum();
            $(".txtPrice").OnlyFloat();
            $("#txtFreight").OnlyFloat();
        });
        //$(window).height(); //浏览器时下窗口可视区域高度
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="Goodsgifts" style="min-width:600px;">
                 <div class="borderkuang" style="height:125px;" >
              <table    border="0" cellspacing="0" cellpadding="0"   style="float: left;width :360px; ">                            
                         <tr>
                        <td class="td-height">
                           <asp:HiddenField runat="server" ID="selectedUserDepotId"/>
                            已选用户：<asp:Literal ID="ltlSelectUser" runat="server" Text="  " />
                        </td>
                    </tr>
                         <tr>
                        <td  >
                         &#12288;收货人：<asp:Literal ID="ltshipName" runat="server" Text="  " />
                        </td>
                    </tr>
                    <tr>
                        <td class="td-height">
                        手机号码：<asp:Literal ID="ltphone" runat="server" Text="  " />  
                        </td>
                    </tr>
                    <tr>
                        <td class="td-height">
                       收货地址：<asp:Literal ID="ltAddress" runat="server" Text="  " />  
                        </td>
                    </tr>                                   
                  <tr runat="server" id="tr_depot" visible="false">
                        <td class="td-height">
                       仓&#12288;&#12288;库：<asp:Literal ID="ltdepot" runat="server" Text="  " /> 
                        </td>
                    </tr>
                </table>

              <table  border="0" cellspacing="" cellpadding="2"   style="float: right;width:230px;">    
                                <tr>
                        <td  >
                        付款方式：<asp:DropDownList   ID="dropPayType" runat="server">
                            <asp:ListItem Value="2" Selected="True">已支付</asp:ListItem>
                        </asp:DropDownList>                            
                            <%--<select  id="payType"> 
                        <option  value="2"  selected="selected" >已支付</option>
                        <option value="1"   style="display:none;">货到付款</option>
                      </select>--%>
                        </td>
                    </tr>
                    <tr>
                        <td  >
                        配送方式：<asp:DropDownList ID="dropShippingType" runat="server">      
                    </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td  >
                        配送费用：<input type="text" id="txtFreight"   style="width: 115px;" />
                        </td>
                    </tr>     
                </table>
                     </div>
             <br />
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                    <tr>
                        <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                            <div style="margin: 5px auto 5px">
                                <asp:TextBox ID="txtKeyword" runat="server" Width="100%" Height="28px"></asp:TextBox>          
                            </div>
                            <input type="hidden" id="hidUserId"  value=<%=UserId %> />  
                        </td>
                    </tr>
                </table>
                <br />

                <iframe width="100%" id="divCarList" name="divCarList"   frameborder="0"  src="CartList.aspx" onload="this.height=($(window).height() - 450)"></iframe> 
                   <div style="text-align: right;" >    <%-- onload="this.height=(divCarList.document.body.scrollHeight)"--%>  
                      <input   type="button" value="清空订购单" class="adminsubmit clearCar" /></div>       
                <br />
                <div class="borderkuang" >
                   
              <div style="height:120px; ">               
                    <table  border="0" cellspacing="0" cellpadding="2" style="float:right;width:230px;">                                    
                        <tr>
                        <td  >
                        商品总额： <span   id="totalSellPrice">0.00</span>
                        </td>
                    </tr>
                    <tr>
                        <td  >
                        运&#12288;&#12288;费： <span   id="freightPriceId">0.00</span>
                        </td>
                    </tr>
                        <tr>
                        <td   >
                        优惠金额： <span   id="promotionsPriceId">0.00</span>
                        </td>
                    </tr>
                        <tr>
                        <td  >
                        应付金额： <span style="color:red;" id="payPriceId" BaseTotalAdjustedPrice="0">0.00</span>
                        </td>
                    </tr>
                </table>
                 </div> 

                <div style="text-align: center;" id="div_save">
                      <input  type="button" value="提交" class="adminsubmit"  id="btn_save" />
                   
                </div>

                <div style="text-align: center;" id="submit_message">
                    </div>
                </div>

            </div>
 
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
    <script type="text/javascript">
        $(function () {
 

            $(".left").find("input").each(
            function () {
                $(this).keypress(function (e) {
                    var key = window.event ? e.keyCode : e.which;
                    if (key.toString() == "13") {
                        return false;
                    }
                });
            });


            $("[id$='txtKeyword']").select2({
                placeholder: "输入商品名称或编码",
                minimumInputLength: 1,
                formatInputTooShort: "请输入至少一个字符",
                formatNoMatches: "没有匹配项",
                formatSearching: "正在查询......",
                ajax: {
                    url: "/AProductHandler.aspx",
                    type: "POST",
                    dataType: 'json',
                    quietMillis: 100,
                    data: function (term, page) { // page is the one-based page number tracked by Select2
                        return {
                            Action: "GetSKUList",
                            q: term, //search term
                            page_limit: 10, // page size
                            page: page, // page number
                        };
                    },
                    results: function (data, page) {
                        var more = (page * 10) < data.total; // whether or not there are more results available
                        return { results: data.productList, more: more };
                    }
                },
                formatResult: Format, // omitted for brevity, see the source of this page
                escapeMarkup: function (m) { return m; } // we do not want to escape markup since we are displaying html in results
            });

            //直接加购物车
            $("[id$='txtKeyword']").change(function () {
                var sku = $(this).val();
                if (sku=="") {
                    ShowFailTip("请选择商品！");
                    return;
                }
                $(this).val('');
                $('#select2-chosen-1').text('输入商品名称或编码');
                $.ajax({
                    url: "/ShoppingCart.aspx",
                    type: "POST",
                    async: false,
                    dataType: "JSON",
                    data: { Action: "AddCartSku", Sku: sku },
                    success: function (resultData) {
                        if (resultData.STATUS == "SUCCESS") {
                           // ShowSuccessTip("加入购物车成功！");
                            window.frames[0].document.location.reload();
                        } else if (resultData.DATA == "NOSTOCK") {
                            ShowFailTip("库存不足！"); NOSKU
                        } else if (resultData.DATA == "NOSKU") {
                            ShowFailTip("sku不存在！"); 
                        }else {
                            ShowFailTip("服务器繁忙，请稍候再试！");
                        }
                    }
                });
            });
 

            //清空购物车操作
            $(".clearCar").click(function () {
                if (confirm("您确定清空订购单吗？")) {
                    $.ajax({
                        type: "POST",
                        dataType: "JSON",
                        url: "/ShoppingCart.aspx",
                        data: { Action: "ClearShopCart" },
                        success: function (resultData) {
                            if (resultData.STATUS == "SUCCESS") {
                                ShowSuccessTip("清空订购单成功！");
                                window.frames[0].document.location.reload();
                            } else {
                                ShowFailTip("服务器繁忙，请稍候再试！");
                            }
                        }
                    });
                }
            });


            //配送方式
            $('[id$="dropShippingType"]').change(function () {
                $.ajax({
                    url: ("ProductSKUList.aspx?timestamp={0}").format(new Date().getTime()),
                    type: 'POST', dataType: 'json', timeout: 10000,
                    data: { Action: "SetShippingType", Callback: "true", shipTypeId: $(this).val() },
                    success: function (resultData) {
                        if (resultData.STATUS == "SUCCESS") {
                            CalcPrice(parseFloat(resultData.DATA));
                        }
                        else {
                            ShowFailTip('服务器繁忙，请稍候再试！');
                        }
                    }
                });
            });

            //保存
            $('#btn_save').click(function () {  
                submitOrder();
            });
 
            //修改运费
            $("#txtFreight").bind("keyup", function () {//键盘弹起事件
                var freight = parseFloat($(this).val());
                $('#freightPriceId').text(freight.toFixed(2));//运费
                var totalAdjustedPrice = parseFloat($('#payPriceId').attr('BaseTotalAdjustedPrice'));
                $('#payPriceId').text((totalAdjustedPrice + freight).toFixed(2));
            });
 
         
           
        });
 

        //计算价格
        function CalcPrice(freight) {
            $('#txtFreight').attr('basefreight', freight).val(freight.toFixed(2));//运费
            $('#freightPriceId').attr('basefreight', freight).text(freight.toFixed(2));//运费
            var totalAdjustedPrice = parseFloat($('#payPriceId').attr('BaseTotalAdjustedPrice'));
            $('#payPriceId').text((totalAdjustedPrice + freight).toFixed(2));
        }

        //提交订单
        function submitOrder() {
            var shipTypeId = parseInt($('[id$="dropShippingType"]').val());
            if (shipTypeId <= 0) {
                ShowFailTip('请选择配送方式！');
                return false;
            }
            var payType = parseInt($('[id$="dropPayType"]').val());// parseInt($('#dropPayType').val());
            if (payType <= 0) {
                ShowFailTip('请选择付款方式！');
                return false;
            }
            $('#div_save').hide();
            $("#submit_message").text('订单正在提交请稍等......').show();
            // 已付款 货到付款
            var freight = $('#txtFreight').val();
            $.ajax({
                url: '/AOrderHandler.aspx',
                type: 'post',
                dataType: 'json',
                timeout: 0,
                async: true,
                data: {
                    Action: "SubmitOrder",
                    freight: freight,
                    payType: payType,
                    Remark: ""
                },
                success: function (resultData) {
                    $("#submit_message").text('').hide();
                    var isOK = false;
                    switch (resultData.STATUS) {
                        // 提交订单成功   
                        //
                        case "SUCCESS":
                            isOK = true;
                            ShowSuccessTip('提交成功，正在为您跳转....');
                            $("#submit_message").text('提交成功,正在为您跳转...').show();
                            ////延迟两秒后跳转
                            //setTimeout(function () {
                                window.location.replace('selectUser.aspx');
                           // }, 2000)
                            break;
                        case "NOSTOCK":
                            ShowFailTip("很抱歉.您购买的部分商品已经被其TA人抢先下单了,");
                            break;
                        case "NOSHOPPINGCARTINFO":
                            ShowFailTip("您的购物车是空的, 请加入商品后再保存!");
                            break;
                        case "NOLOGIN":
                            ShowFailTip("'您还没有登陆或者登陆已超时，请您登陆后提交订单．");
                            // 用户未登陆
                            break;
                        case "UNAUTHORIZED":
                            // 权限不足
                            $("#submit_message").html("您非管理员用户，您没有权限提交订单哦！");
                            $("#submit_message").show();
                            break;
                        case "PayTypeISNULL":
                            ShowFailTip("支付类型不正确.");
                            break;
                        case "NOPAYTYPE":
                            ShowFailTip("支付类型不正确.");
                            break; 
                        default:
                            ShowFailTip(resultData.STATUS);
                            // 抛出异常消息
                            break;
                    }
                    if (!isOK) {
                        $('#div_save').show();
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    if (textStatus != 'timeout') {
                        alert(xmlHttpRequest.responseText);
                    } else {
                        $("#submit_message").html("噗, 您的网络忒慢了! 访问服务器超时了, 请再试一下!");
                    }
                    $("#submit_message").show();
                }
            });

        }
        function Format(data) {
            return data.text;
        }
    </script>
</asp:Content>
