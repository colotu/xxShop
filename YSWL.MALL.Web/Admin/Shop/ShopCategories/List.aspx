<%@ Page Title="ShopCategories" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.ShopCategories.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/admin/css/gridviewstyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            //            $("span:contains('启用')").css("color", "#006400");
            //            $("span:contains('不启用')").css("color", "red");

            $(".iframe").colorbox({ iframe: true, width: "450", height: "325", overlayClose: false });
            $("#ctl00_ContentPlaceHolder1_gridView tr").each(function (index, domEle) {
                if (index != 0) {
                    var optionTag = $(this).html();
                    if (optionTag.indexOf("parentid=\"0\"") < 0) {
                        $(domEle).hide();
                        $(".productcag1 span img").attr("src", "/admin/images/jia.gif");
                    }
                }
            });
            $(".btnDelete").click(function () {
                var categoryId = $(this).attr("cateId");
                if (confirm("删除分类会删除该分类下所有子分类\n，确定要删除选择的分类吗？")) {
                    $.ajax({
                        url: ("List.aspx?timestamp={0}").format(new Date().getTime()),
                        type: 'POST', dataType: 'json', timeout: 10000,
                        data: { Action: "Delete", Callback: "true", CategoryId: categoryId },
                        success: function (resultData) {
                            if (resultData.STATUS == "SUCCESS") {
                                ShowSuccessTip('删除成功');
                                location.reload();
                                return;
                            }
                            if (resultData.STATUS == "NO") {
                                ShowFailTip('该分类或者子类已被其它商品数据使用，不能删除！');
                                return;
                            }
                            if (resultData.STATUS == "FAILED") {
                                ShowFailTip('服务器繁忙，请稍候再试！');
                            }
                        }
                    });
                }
            })
            $(".btnUpdate").click(function () {
                var _self = $(this);
                var categoryId = _self.attr("CategoryId");
                var value = _self.prev().val();
                if (isNaN(parseInt(value)) || parseInt(value) <= 0) {
                    ShowFailTip('请填写正确的顺序值');
                    return;
                }
                $.ajax({
                    url: ("List.aspx?timestamp={0}").format(new Date().getTime()),
                    type: 'POST', dataType: 'json', timeout: 10000,
                    data: { Action: "UpdateSeqNum", Callback: "true", CategoryId: categoryId, UpdateValue: value },
                    success: function (resultData) {
                        if (resultData.STATUS == "SUCCESS") {
                            ShowSuccessTip('保存成功');
                            self.prev().val(value);
                        }
                        else {
                            ShowFailTip('服务器繁忙，请稍候再试！');
                        }
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Title -->
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="商品分类管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="您可以新增、编辑、删除商品分类信息" />
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Site, lblSearch%>" />：
                    <asp:DropDownList ID="ddCateList" runat="server">
                    </asp:DropDownList>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit_short"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <div class="newslist mar-bt">
            <div class="newsicon">
                <ul>
                    <li class="add-btn" id="liAdd"
                        runat="server"><a href="add.aspx" title="新增新的店铺分类">新增</a></li>
                    <li class="add-btn">
                        <span id="openAll"><a style="cursor: pointer; text-decoration: none; line-height: normal;">
                            全部展开</a></span></li>
                    <li class="add-btn">
                        <span id="closeAll"><a style="cursor: pointer; text-decoration: none; line-height: normal;">
                            全部收缩</a></span></li>
                </ul>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <asp:GridView ID="gridView" runat="server" AutoGenerateColumns="false" ShowHeader="true"
                    DataKeyNames="CategoryId" CssClass="GridViewStyle" RowStyle-CssClass="grdrow"
                    HeaderStyle-CssClass="GridViewHeaderStyle" ShowFooter="false" SelectedRowStyle-BackColor="#FBFBF4"
                    OnRowCommand="gridView_RowCommand" OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting"
                    CellPadding="3" BorderWidth="1px" BackColor="White" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:CMSVideo, CategoryName %>" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <span id="spShowImage" runat="server" parentid='<%# Eval("ParentCategoryId") %>'>
                                    <img src="/admin/images/jian.gif" width="24" height="24" alt="" />
                                </span>
                                <asp:Label ID="lblName" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="分类编号" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#Eval("CategoryId") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="显示顺序" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Left"
                           >
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%#Eval("DisplaySequence")%>' Width="50px"></asp:TextBox>
                                <a href="javascript:;" categoryid='<%#Eval("CategoryId") %>' class="btnUpdate">保存</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="URL重写名称" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180">
                            <ItemTemplate>
                                <%#Eval("RewriteName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="排序" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center"
                            Visible="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDesc" runat="server" ImageUrl="/admin/images/desc.png" CommandName="Fall"
                                    Width="16" Height="16" />
                                <asp:ImageButton ID="imgAsc" runat="server" ImageUrl="/admin/images/asc.png" CommandName="Rise"
                                    Width="16" Height="16" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="状态" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="180">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnStatus" runat="server" CausesValidation="False" CommandName="Status"
                                    CommandArgument='<%#Eval("CategoryId")+","+Eval("Status")%>' Style="color: #0063dc;">
                            <%#YSWL.Common.Globals.SafeBool(Eval("Status"), false) ? "<span  style='color:#006400'>启用</span>" : "<span  style='color:red'>不启用</span>"%>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:CMSVideo, Operation %>" ItemStyle-Width="240"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemStyle />
                            <ItemTemplate>
                                <asp:HyperLink CssClass="Add" ID="HyperLink1" runat="server" Text="<%$ Resources:Site, ltlAdd %>"
                                    NavigateUrl='<%#Eval("CategoryId", "Add.aspx?id={0}")%>' ForeColor="Black" Visible="false"></asp:HyperLink>
                                <asp:HyperLink CssClass="Show" ID="HyperLink3" runat="server" Text="<%$ Resources:Site, btnDetailText %>"
                                    NavigateUrl='<%#Eval("CategoryId", "Show.aspx?id={0}")%>' ForeColor="Black" Visible="false"></asp:HyperLink>
                                <a class="iframe" href="swap.aspx?id=<%#Eval("CategoryId") %>" style="display: none;">转移商品</a> &nbsp;&nbsp; <span id="lbtnModify" runat="server"><a href="Modify.aspx?id=<%#Eval("CategoryId") %>"
                                >编辑</a> &nbsp;&nbsp;</span> <a class="btnDelete" cateid='<%#Eval("CategoryId") %>'>
                                            删除</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle Height="25px" HorizontalAlign="Right" />
                    <HeaderStyle Height="35px" />
                    <PagerStyle Height="25px" HorizontalAlign="Right" />
                    <RowStyle Height="25px" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;" class="def-wrapper">
            <tr>
                <td align="left">
                    <asp:Button ID="btnUpdateSeq" runat="server" Text="全部确定" class="adminsubmit" OnClick="btnUpdateSeq_Click" />
                    <%--       <asp:Button ID="btnInverseApprove" runat="server" Text="批量下架" class="adminsubmit"
                        OnClick="btnInverseApprove_Click" />--%>
                </td>
            </tr>
        </table>
        <div class="newslist_title">
            <div class="shou" style="background-color: #FFFFFF">
            </div>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            //全部隐藏
            $("#closeAll").bind("click", function () {
                $("#ctl00_ContentPlaceHolder1_gridView tr").each(function (index, domEle) {
                    if (index != 0) {
                        var optionTag = $(this).html();
                        if (optionTag.indexOf("parentid=\"0\"") < 0) {
                            $(domEle).hide();
                            $(".productcag1 span img").attr("src", "/admin/images/jia.gif");
                        }
                    }
                })
            });
            //全部展开
            $("#openAll").bind("click", function () {
                $("#ctl00_ContentPlaceHolder1_gridView tr").each(function (index, domEle) {
                    if (index != 0) {
                        $(domEle).show();
                        $(".productcag1 span img").attr("src", "/admin/images/jian.gif");
                    }
                });
            });
            $(".productcag1 span img").each(function (index, imgObj) {
                $(imgObj).click(function () {
                    if ($(imgObj).attr("src") == "/admin/images/jian.gif") {
                        var currentTrNode = $(imgObj).parents("tr");
                        currentTrNode = currentTrNode.next();
                        var optionHTML;
                        while (true) {
                            optionHTML = currentTrNode.html();
                            if (typeof (optionHTML) != "string") { break; }
                            if (optionHTML.indexOf("parentid=\"0\"") < 0) {
                                currentTrNode.hide();
                                currentTrNode = currentTrNode.next();
                            }
                            else { break; }
                        }
                        //把img src设加可开打状态
                        $(imgObj).attr("src", "/admin/images/jia.gif");
                    }
                    else {
                        var currentTrNode = $(imgObj).parents("tr");
                        currentTrNode = currentTrNode.next();
                        var optionHTML;
                        while (true) {
                            optionHTML = currentTrNode.html();
                            if (typeof (optionHTML) != "string") { break; }
                            if (optionHTML.indexOf("parentid=\"0\"") < 0) {
                                currentTrNode.show();
                                currentTrNode = currentTrNode.next();
                            }
                            else { break; }
                        }
                        $(imgObj).attr("src", "/admin/images/jian.gif");
                    }
                })
            })
        })
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
