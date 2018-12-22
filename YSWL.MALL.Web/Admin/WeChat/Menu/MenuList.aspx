<%@ Page Title="微信菜单管理" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="MenuList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.WeChat.Menu.MenuList" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="Grv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/admin/css/gridviewstyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $(".iframe").colorbox({ iframe: true, width: "680", height: "450", overlayClose: false });

            $(".btnUpdate").click(function () {
                var _self = $(this);
                var menuId = _self.attr("MenuId");
                var value = _self.prev().val();
                if (isNaN(parseInt(value)) || parseInt(value) <= 0) {
                    ShowFailTip('请填写正确的顺序值');
                    return;
                }
                $.ajax({
                    url: ("MenuList.aspx?timestamp={0}").format(new Date().getTime()),
                    type: 'POST', dataType: 'json', timeout: 10000,
                    data: { Action: "UpdateSeqNum", Callback: "true", MenuId: menuId, UpdateValue: value },
                    success: function (resultData) {
                        if (resultData.STATUS == "SUCCESS") {
                            ShowSuccessTip('保存成功');
                            _self.prev().val(value);
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
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang borderkuang-noc">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="微信服务号自定义菜单设置" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="在设置服务号自定义菜单之前，请先在账号设置中设置好您的服务号AppId 和服务号AppSecret" />
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        <!--Search end-->
        <div class="newslist mar-bt">
            <div class="newsicon">
                <ul>
                    <li id="liAdd" class="add-btn" runat="server">
                        <a href="AddMenu.aspx" title="新增新的菜单">新增</a>
                    </li>
                </ul>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <Grv:GridViewEx ID="gridView" runat="server" AutoGenerateColumns="false" ShowHeader="true"
                    DataKeyNames="MenuId" 
                   OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            Width="100%" PageSize="10" ShowExportExcel="False" ShowExportWord="False"
            ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px" ShowCheckAll="false">
                    <Columns>
                        <asp:TemplateField HeaderText="显示顺序" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%#Eval("Sequence")%>' Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="菜单名称" ItemStyle-HorizontalAlign="Left"
                            ItemStyle-Width="120">
                            <ItemTemplate>
                                  <div class="tx-l">
                                <span id="spShowImage" runat="server" parentid='<%# Eval("ParentId") %>' style="padding-left: 24px"></span>
                                <asp:Label ID="lblName" runat="server" />
                                      </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="菜单类型" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%#GetTypeName(Eval("Type"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="菜单Key" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" Visible="False">
                            <ItemTemplate>
                                <%#Eval("MenuKey")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="菜单Url" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div class="tx-l" style="width:460px;word-wrap: break-word;">
                                     <%#Eval("MenuUrl")%>   
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="菜单状态" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="ss">
                            <ItemTemplate>
                                <%#GetStatus(Eval("Status"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="创建时间" ItemStyle-Width="120" ItemStyle-HorizontalAlign="Center" Visible="False">
                            <ItemTemplate>
                                <%#Eval("CreateDate", "{0:yyyy-MM-dd}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:CMSVideo, Operation %>" ItemStyle-Width="15%"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                &nbsp;&nbsp;    <span id="lbtnModify" runat="server"><a class="iframe-modf" href="UpdateMenu.aspx?id=<%#Eval("MenuId") %>">编辑</a>
                                    &nbsp;&nbsp;</span>
                                <asp:LinkButton ID="linkDel" CssClass="iframe-modf" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="<%$ Resources:Site, btnDeleteText %>" ForeColor="" OnClientClick="return confirm('删除菜单会删除该菜单下所有子菜单\n，确定要删除选择的菜单吗？');"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle Height="25px" HorizontalAlign="Right" />
                    <HeaderStyle Height="35px" />
                    <PagerStyle Height="25px" HorizontalAlign="Right" />
                    <RowStyle Height="25px" />
                </Grv:GridViewEx>
                <script type="text/javascript">
                    $(function () {
                        $(".GridViewHeaderStyle").find("th").css("font-weight", "bold").css("color", "#666");
                    })
                </script>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;" class="def-wrapper">
            <tr>
                <td align="left">
                    <asp:Button ID="btnUpdateSeq" runat="server" Text="全部保存" class="add-btn" OnClick="btnUpdateSeq_Click" />
                    <asp:Button ID="btnGenerate" runat="server" Text="生成菜单" class="add-btn" OnClick="btnGenerate_Click"
                        OnClientClick="return confirm('请确认您访问的域名['+window.location.host+']\n与微信公众平台域名设置是否一致，确定要生成菜单吗？');" />
                </td>
            </tr>
        </table>
        <div class="newslist_title">
            <div class="shou" style="background-color: #FFFFFF">
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>


