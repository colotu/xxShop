<%@ Page Title="AD_Advertisement" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="SingleList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Advertisement.SingleList" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="YSWL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript">
        $(document).ready(function () {
            var Pid = $.getUrlParam("id");
            $(".iframe").attr("href", "add.aspx?id=" + Pid);
            $(".btnUpdate").click(function () {
                var self = $(this);
                var AdvertisementId = self.attr("AdvertisementId");
                var value = self.prev().val();
                if (isNaN(parseInt(value)) || parseInt(value) <= 0) {
                    ShowFailTip('请填写正确的顺序值');
                    return;
                }
                $.ajax({
                    url: ("SingleList.aspx?timestamp={0}").format(new Date().getTime()),
                    type: 'POST', dataType: 'json', timeout: 10000,
                    data: { Action: "UpdateSeqNum", Callback: "true", AdvId: AdvertisementId, UpdateValue: value },
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

        function GetDeleteM() {
            $("[id$='btnDelete']").click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Title -->
    <div class="newslistabout">
        <div class="newstitle" style="color: #999;">
            【<asp:Literal ID="litTitle" runat="server"></asp:Literal>】的所有广告内容
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->

        <div class="newslist mar-bt">
            <div class="newsicon">
                <ul>
                    <li id="liAdd" runat="server" class="add-btn">
                        <a class="iframe">新增</a> </li>
                    <li id="liDel" runat="server" class="add-btn"><a href="javascript:;" onclick="GetDeleteM()">批量删除</a></li>
                </ul>
            </div>
        </div>
        <YSWL:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="False" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            Width="100%" PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowCheckAll="true" DataKeyNames="AdvertisementId">
            <Columns>
               <asp:TemplateField HeaderText="显示顺序" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%#Eval("Sequence")%>' Width="30px"></asp:TextBox>
                                 <a href="javascript:;" AdvertisementId='<%#Eval("AdvertisementId") %>' class="btnUpdate">保存</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                <asp:TemplateField HeaderText="广告内容" ItemStyle-HorizontalAlign="Center" SortExpression="AdvertisementId"
                    ItemStyle-Width="80">
                    <ItemTemplate>
                        <asp:Literal ID="litContent" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="广告名称" ItemStyle-HorizontalAlign="Left" SortExpression="AdvertisementName">
                    <ItemTemplate>
                        <%#Eval("AdvertisementName") %>
                        <asp:HiddenField ID="hfShowType" runat="server" Value='<%#Eval("ContentType") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="显示类型" ItemStyle-HorizontalAlign="Center" SortExpression="ContentType">
                    <ItemTemplate>
                        <%#ConvertContentType(Eval("ContentType"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Impressions" HeaderText="显示频率" SortExpression="Impressions"
                    ItemStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="CreatedDate" HeaderText="新增时间" SortExpression="CreatedDate"
                    ItemStyle-HorizontalAlign="Center" Visible="false" />
                <asp:TemplateField HeaderText="广告状态" ItemStyle-HorizontalAlign="Center" SortExpression="State">
                    <ItemTemplate>
                        <asp:Literal ID="litState" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
              
                <asp:TemplateField HeaderText="广告主" ItemStyle-HorizontalAlign="Center" SortExpression="EnterpriseID">
                    <ItemTemplate>
                        <%#GetEnName(Eval("EnterpriseID"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="详细信息" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <a class="modify" href='Show.aspx?id=<%#Eval("AdvertisementId") %>&Adid=<%=ADPositionId %>'>
                            详细信息</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="编辑" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <a class="modify" href='Modify.aspx?id=<%#Eval("AdvertisementId") %>&Adid=<%=ADPositionId %>'>
                            编辑</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Site, btnDeleteText %>" SortExpression="AdvertisementId"
                    Visible="false" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                            OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>"
                            Text="<%$ Resources:Site, btnDeleteText %>"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="35px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
            <SortDirectionStr>DESC</SortDirectionStr>
        </YSWL:GridViewEx>
   <div class="def-wrapper">
            <asp:Button ID="btnDelete" OnClientClick='return confirm($(this).attr("ConfirmText"))'
                        ConfirmText="<%$Resources:Site,TooltipDelConfirm %>" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>"
                        class="adminsubmit" OnClick="btnDelete_Click" />
   </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
