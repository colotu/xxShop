<%@ Page Title="模版管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits=" YSWL.MALL.Web.Admin.Ms.Themes.List" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Admin/css/tab.css" rel="stylesheet" type="text/css" charset="utf-8" />
    <link href="/Admin/../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/tab.js" type="text/javascript"> </script>
    <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"> </script>
    <script src="/Scripts/jquery/maticsoft.jquery.min.js" type="text/javascript"> </script>
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"> </script>
    <script type="text/javascript">
        $(function () {
            $("[id$='txtPhotoName']").hide();
            var multiColorThemes = ['pc01', 'pc03', 'pc04'];
            $(".themeList").each(function () {
                var iscurrend = $(this).find("[id$='hidIsCurrent']").val();
                $(this).find(".lblCurrent").hide();
                if (iscurrend == "True") {
                    $(this).find("[id$='linkstart']").hide();
                    $(this).find(".lblCurrent").show();
                    $(this).css({ 'border': '3px solid #06aaea' });

                    //if ($.inArray($(this).find(".colorList").attr('themeName').toLocaleLowerCase(), multiColorThemes) > -1) { //当前模板存在与多颜色模板中，则显示颜色设置
                    //   var color = $('[id$="hidCurrentColor"]').val();
                    //   $(this).find("[item='" + color + "']").addClass('cur');
                    //   $(this).find(".colorList").css('display', 'inline-block');
                    //}
                }
            });

            $(".colorList span").click(function () {
                var _self = $(this);
                var color = _self.attr("item");
                if (color == "") {
                    return;
                }
                $.ajax({
                    url: ("List.aspx?timestamp={0}").format(new Date().getTime()),
                    type: 'POST', dataType: 'json', timeout: 10000,
                    data: { Action: "UpdateColor", Callback: "true", Color: color },
                    success: function (resultData) {
                        if (resultData.STATUS == "SUCCESS") {
                            ShowSuccessTip('设置成功');
                            _self.addClass('cur').siblings().removeClass('cur');
                        } else {
                            ShowFailTip('服务器繁忙，请稍候再试！');
                        }
                    }
                });
            });
        });
    </script>
    <style type="text/css">
        .search
        {
            background-color: #ffffff;
            float: left;
            height: 35px;
        }
        
        .borderkuang td
        {
            bgcolor: "#FFFFFF";
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="txtPhoto" runat="server" Text="网站模板管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal8" runat="server" Text="您可以根据自身的喜好设置网站当前模板" />
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        <!--Add end -->
        <!--Search -->
        <br />
        <asp:HiddenField runat="server" ID="hidCurrentColor" Value="" />
        <asp:HiddenField runat="server" ID="hidHasColorThemes" Value="PC01,PC03,PC04" />
        <table style="text-align: center; width: 100%;" cellpadding="5" cellspacing="5" class="border">
            <tr>
                <td style="text-align: left">
                    <asp:DataList ID="DataListPhoto" RepeatColumns="4" RepeatDirection="Horizontal" 
                        runat="server" OnItemCommand="DataListPhoto_ItemCommand">
                        <ItemTemplate>
                             <div class="shop_themeList themeList" >
                                <div  class="ki">
                                             <img src='<%#Eval("PreviewPhotoSrc") %>'  title="<%#Eval("Description") %>" />
                                </div>
                                        <asp:CheckBox ID="ckPhoto" runat="server" Visible="False" />
                                        <asp:HiddenField runat="server" ID="hfPhotoId" Value='<%#Eval("ID") %>' />
                                 <span>名称：<%#Eval("Name") %><%#Eval("Remark") %></span>
                                 <asp:LinkButton ID="linkstart" runat="server" Style="color: #0063dc;" CommandName="start"
                                            CommandArgument='<%#String.Format("{0},{1}",Eval("Name"),Eval("Color"))%>'>
                                            <asp:Literal ID="btnStart" runat="server" Text="设为当前主题" /></asp:LinkButton>
                                             <span class="lblCurrent"  style='font-weight: bold'>当前主题</span>
                                      <asp:HiddenField runat="server" ID="hidIsCurrent" Value='<%#Eval("IsCurrent") %>' />
                                        <div class="colorList" style="display:none;"  themeName="<%#Eval("Name") %>" ><span item="green">绿色</span><span  item="blue">蓝色</span><span  item="orange">橙色</span><span  item="red">红色</span></div>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td>
                    <webdiyer:AspNetPager runat="server" ID="AspNetPager1" CssClass="anpager" CurrentPageButtonClass="cpb"
                        OnPageChanged="AspNetPager1_PageChanged" PageSize="15" FirstPageText="<%$Resources:Site,FirstPage %>"
                        LastPageText="<%$Resources:Site,EndPage %>" NextPageText="<%$Resources:Site,GVTextNext %>"
                        PrevPageText="<%$Resources:Site,GVTextPrevious %>">
                    </webdiyer:AspNetPager>
                </td>
            </tr>
        </table>
        
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
