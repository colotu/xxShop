<%@ Page Title="电子样本管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="SampleImage.aspx.cs" Inherits=" YSWL.Web.Admin.Shop.Sample.SampleImage" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <link href="/Admin/css/tab.css" rel="stylesheet" type="text/css" charset="utf-8" />
    <link href="/Admin/../Content/themes/base/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-ui-1.8.11.min.js" type="text/javascript"></script>
    <script src="/Admin/js/tab.js" type="text/javascript"></script>
    <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery/maticsoft.jquery.min.js" type="text/javascript"></script>
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("[id$='from']").prop("readonly", true).datepicker({ dateFormat: "yy-mm-dd", yearRange: ("1949:"+new Date().getFullYear()) });
            $("[id$='to']").prop("readonly", true).datepicker({ dateFormat: "yy-mm-dd", yearRange: ("1949:"+new Date().getFullYear()) });

        })
    </script>
    <script type="text/javascript">
        $(function () {
            $("[id$='txtPhotoName']").hide();
            $(".group2").colorbox({ rel: 'group2', transition: "fade" });
            $(".iframe").colorbox({ iframe: true, width: "780", height: "630", overlayclose: false });
        });
    </script>
    <style type="text/css">
        .search
        {
            float:left;
            background-color:#ffffff;
            height:35px;
            }
        .borderkuang td{bgcolor:"#FFFFFF"}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="txtPhoto" runat="server" Text="样本图片管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal8" runat="server" Text="您可以对图片进行删除，批量删除等操作" />
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
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:DropDownList ID="ddrSampleList" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddrSampleList_SelectedIndexChanged">
                    </asp:DropDownList>
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
                     <li>
                        <input id="Checkbox1" type="checkbox" onclick='$(":checkbox").attr("checked", $(this).attr("checked")=="checked");' />
                        <asp:Literal ID="Literal7" runat="server" Text="<%$Resources:Site,CheckAll %>" /></li>
                    <li class="add-btn" runat="server"  ID="AddLi" >
                        <asp:HyperLink ID="hlkadd" runat="server" NavigateUrl="ImageAdd4Sample.aspx" >新增</asp:HyperLink>
                    </li>
                    <li class="add-btn" id="liDel" runat="server"><a href="javascript:;" onclick="GetDeleteM()"> <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:Site,btnDeleteListText %>" /></a>

                    </li>
                  
                </ul>
            </div>
        </div>
        
        
        
        <table style="width: 100%; margin-left: 10; text-align: center" cellpadding="5" cellspacing="5"
            class="border">
            <tr>
                <td style="text-align: center">
                    <div id="gallery">
                        <asp:DataList ID="DataListPhoto" RepeatColumns="5" RepeatDirection="Horizontal" HorizontalAlign="Center"
                            runat="server" OnItemCommand="DataListPhoto_ItemCommand">
                            <ItemTemplate>
                                <table cellpadding="2" cellspacing="8">
                                    <tr>
                                        <td style="border: 1px solid #ecf4d3; text-align: center">
                                            <a class="group2" href='<%#Eval("NormalImageUrl") %>' title='<%#Eval("Title") %>'>
                                                <img src='<%#Eval("ThumbImageUrl") %>' style="width: 180px;" />
                                            </a>
                                            <br />
                                            <asp:CheckBox ID="ckPhoto" runat="server" />
                                            <asp:HiddenField runat="server" ID="hfPhotoId" Value='<%#Eval("ID") %>' />
                                              <asp:Label ID="Label1" runat="server" Text='<%#Eval("Title") %>'></asp:Label><br /> <br /> 
                                             <asp:Label ID="Label2" runat="server" Text='<%#Eval("CreatedDate") %>'></asp:Label>[<asp:LinkButton ID="lbtnDel" runat="server"
                                                    Style="color: #0063dc;" CommandName="delete" CommandArgument='<%#Eval("ID") %>'
                                                    OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>">
                                                    <asp:Literal ID="Literal11" runat="server" Text="<%$Resources:Site,btnDeleteText %>" /></asp:LinkButton>] </td></tr></table></ItemTemplate></asp:DataList></div></td></tr><tr>
                <td>
                    <webdiyer:AspNetPager runat="server" ID="AspNetPager1" CssClass="anpager" CurrentPageButtonClass="cpb" 
                        OnPageChanged="AspNetPager1_PageChanged" PageSize="15" FirstPageText="<%$Resources:Site,FirstPage %>"
                        LastPageText="<%$Resources:Site,EndPage %>" NextPageText="<%$Resources:Site,GVTextNext %>"
                        PrevPageText="<%$Resources:Site,GVTextPrevious %>">
                    </webdiyer:AspNetPager>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
            <tr>
       
            
                <td style="float: left">
                    <asp:Button ID="btnDelete" OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>"
                        class="adminsubmit" OnClick="btnDelete_Click" />
                                           
                                     <asp:Button ID="goback" runat="server" Text="返回"
                                    class="adminsubmit_short" onclick="goback_Click"></asp:Button> </td></tr></table></div></asp:Content><asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
