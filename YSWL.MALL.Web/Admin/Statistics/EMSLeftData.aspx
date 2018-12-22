<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master" AutoEventWireup="true" CodeBehind="EMSLeftData.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.EMSLeftData" %>
<%@ Register TagPrefix="cc1" Namespace="YSWL.MALL.Web.Controls" Assembly="YSWL.MALL.Web" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="Div1" style="height: 600px;float: left;margin: 25px 5px;" runat="server">
        <asp:Literal runat="server" ID="nodata" Visible="False">没有符合条件的数据</asp:Literal>
                       <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
             OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting"
            UnExportedColumnNames="Modify" Width="100%" PageSize="10" ShowExportExcel="False"
            ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
            ShowCheckAll="False" DataKeyNames="UserID" >
            <Columns>
                <asp:TemplateField  ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <div class="hdValue" latitude="<%#Eval("Latitude")%>" longitude="<%#Eval("Longitude")%>">
                        <table>
                            <tr>
                                <td >
                                    店铺名称：
                                    <%#Eval("ShopName")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    配送站名称： 
                                        <%#Eval("Name")%>
                                </td>
                            </tr>
                                                        <tr>
                                <td>
                                    电&nbsp;&nbsp;&nbsp;&nbsp;话： 
                                        <%#Eval("CellPhone")%>
                                </td>
                            </tr>
                        </table>
                        </div>
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
                     
                   </div>
                   <script type="text/javascript">
                       $(function () {
                           $(".hdValue").die('mouseenter').live('mouseenter', function () {
                               var mouseEnter = $(window.parent.document).find(".mouseEnter");
                               $(mouseEnter).val((new Date()).valueOf());
                               $(mouseEnter).attr('latitude', $(this).attr('latitude')).attr('longitude', $(this).attr('longitude'));
                               //$(mouseEnter).attr([{ 'latitude': $(this).attr('latitude') }, { 'longitude': $(this).attr('longitude')}]);

                           });
                           $(".hdValue").die('mouseleave').live('mouseleave', function () {
                               var mouseLeave = $(window.parent.document).find(".mouseLeave");
                               $(mouseLeave).val((new Date()).valueOf());
                               $(mouseLeave).attr('latitude', $(this).attr('latitude')).attr('longitude', $(this).attr('longitude'));
                           });
                       });
                   </script>
</asp:Content>
