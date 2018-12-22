<%@ Page Title="<%$ Resources:SysManage, ptErrorlog%>" Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="ErrorLog.aspx.cs" Inherits="YSWL.MALL.Web.Admin.SysManage.ErrorLog" %>
<%@ register assembly="YSWL.MALL.Web" namespace="YSWL.MALL.Web.Controls" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/admin/js/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/admin/js/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
       <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            
            //绑定日期控件
            var today = new Date();
            var year = today.getFullYear();
            var month = today.getMonth();
            var day = today.getDate();
            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("[id$='txtDate']").prop("readonly", true).datepicker({ dateFormat: "yy-mm-dd", yearRange: ("1949:" + new Date().getFullYear()) });

//            $("[id$='txtDate']").prop("readonly", true).datepicker({
//                numberOfMonths: 1, //显示月份数量
//                onClose: function () {
//                    $(this).css("color", "#000");
//                }
//            }).focus(function () { $(this).val(''); });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:SysManage, ptErrorlog%>" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:SysManage, lblConsultErrorlog%>" />
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no mar-bt">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Site, lblKeyword%>" />：
                    <asp:TextBox ID="txtKeyword" runat="server" class="admininput_1"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit_short"></asp:Button>
                </td>
            </tr>
        </table>
        <div class="newslist mar-bt">
            <div class="newsicon">
            <asp:Button ID="Button2" runat="server" Text="<%$ Resources:Site, btnDeleteListText%>"
                       OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>" class="adminsubmit" OnClick="btnDelete_Click" />
                <%--<ul>
                   <li class="add-btn"><a href="add.aspx"><asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Site, lblAdd %>"/>新增</a></li>
                    <li  id="liDel" runat="server">
                        <asp:LinkButton ID="lbtnDelete" runat="server" CssClass="add-btn"
                            Text="<%$ Resources:Site, btnDeleteListText %>" onclick="lbtnDelete_Click"></asp:LinkButton>
                        </li>
                
                </ul>--%>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            Width="100%" PageSize="10" DataKeyNames="ID" ShowExportExcel="True" ShowExportWord="False"
            ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px" ShowCheckAll="true">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="<%$Resources:Sysmanage,fieldID%>" SortExpression="ID"
                    ControlStyle-Width="40" />
                <asp:BoundField DataField="OPTime" HeaderText="<%$Resources:Sysmanage,fieldOPTime%>"
                    SortExpression="OPTime" ControlStyle-Width="40" />
 
                        <asp:TemplateField HeaderText="<%$Resources:Sysmanage,fieldLoginfo%>"   SortExpression="Loginfo"  >
                    <ItemTemplate>
                        <div class="tx-l">
                            <%#Eval("Loginfo")%>

                        </div>

                    </ItemTemplate>
                </asp:TemplateField>
              
                             <asp:BoundField DataField="StackTrace" HeaderText="堆栈信息" Visible="False"/>
                    
                <%--<asp:HyperLinkField HeaderText="Loginfo" DataTextField="Loginfo" DataNavigateUrlFields="ID"
                                DataNavigateUrlFormatString="show.aspx?id={0}" Text="Loginfo" ItemStyle-HorizontalAlign="Left" />    --%>
         
                     <asp:TemplateField HeaderText="<%$Resources:Sysmanage,fieldUrl%>"   SortExpression="Url"  >
                    <ItemTemplate>
                        <div class="tx-l">
                            <%#Eval("Url")%>

                        </div>

                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ControlStyle-Width="50" HeaderText="" Visible="false">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                         OnClientClick="return confirm($(this).attr('ConfirmText'))" ConfirmText="<%$ Resources:Site,btnIfRemove%>" Text="Delete"></asp:LinkButton>
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
        <table class="def-wrapper" border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
            <tr>
                <td>
                    <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Site, btnDeleteListText%>"
                       OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>" class="add-btn" OnClick="btnDelete_Click" />
                    <asp:Button ID="Button1" runat="server" Text="<%$ Resources:Sysmanage, lblDeleteBeforeOneDay%>"
                        class="add-btn" OnClick="btnDeleteAll_Click" />
                    <asp:TextBox ID="txtDate" runat="server" Width="100px"  ></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
