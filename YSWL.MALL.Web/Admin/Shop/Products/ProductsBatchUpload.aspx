<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="ProductsBatchUpload.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Products.ProductsBatchUpload" %>

<%@ Register Src="/Admin/../Controls/ProductsBatchUploadDropList.ascx" TagName="ProductsBatchUploadDropList"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .PageTitleArea
        {
            margin: 0px;
            margin-bottom: 4px;
            padding: 8px;
            border: #D5DCE6 1px solid;
            background: #F8F8F8;
            font-size: 120%;
            font-style: normal;
            font-size: 12px;
            font-weight: normal;
            color: #404040;
            line-height: 17px;
        }
        .PageTitle
        {
            font-size: 15px;
            color: #2E6CD2;
            font-weight: bold;
        }
        
        .AdminSearchform
        {
            padding-left: 0;
            font-family: verdana, tahoma, helvetica;
            font-size: 12px;
            margin-bottom: 8px;
            padding-bottom: 8px;
            padding-top: 0px;
            overflow: hidden;
            clear: both;
        }
        
        /* button样式 */
        .inp_L1
        {
            height: 20px;
            padding: 0 3px;
            border: 1px solid #87a3c1;
            color: #174b73;
            background: url(../images/botton_newbg.gif) repeat-x;
            cursor: pointer;
            vertical-align: middle;
        }
        
        legend
        {
            font-weight: bold;
            color: #2E6CD2;
            font-size: 12px;
            margin-left: 10px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="批量上传商品" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text=" 如果你想批量上传商品,可在这里通过Excel文件上传." />
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td width="60px" height="30" >
                    第一步：
                </td>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    下载批量上传Excel文件(选择使用网站Excel文件上传的用户执行)
                </td>
            </tr>
            <tr>
                <td width="60px" height="30" >
                </td>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                 <a href="/Upload/Template/ImportProduct.xls" class="a-link">下载导入商品模板文件  </a>&#12288;&#12288;&#12288;
                 <asp:LinkButton CssClass="a-link" onclick="downExplanationBut_Click"  ID="downExplanationBut" runat="server"  >下载导入商品说明文件</asp:LinkButton>
                </td>
            </tr>
          <%--  <tr>
                <td width="60px" height="30" >
                </td>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                     <span style="color: red;">* csv 文件中不能用Alt+回车键来换行</span>
                </td>
            </tr>--%>
        </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td width="60px" height="30" >
                    第二步：
                </td>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    填写Excel文件
                </td>
            </tr>
            <tr>
                <td width="60px" height="30" >
                </td>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                      1.请把本地电脑上的商品清晰图整理好并通过 FTP 上传到： 网站根目录/Upload/Shop/Images/Product/  <span style="color:#DA8210;">+</span>  ImageUrl
                </td>
            </tr>
            <tr>
                <td width="60px" height="30" >
                </td>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                      2.在Excel文件里面对应处写入上传商品的各项内容。注意商品清晰图(ImageUrl)只填写日期文件夹和图片名称即可,例如：20150809/01.jpg
                </td>
            </tr>
        </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td width="60px" height="30" >
                    第三步：
                </td>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    开始批量操作
                </td>
            </tr>
            <tr>
                <td width="60px" height="30" >
                    文件：
                </td>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                     <asp:FileUpload ID="upload" runat="server" Width="235px" />
                   <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="upload" runat="server" ErrorMessage="请选择正确的格式" ValidationExpression="^.+(xls|xlsx)$">
                       
                   </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td width="60px" height="30" >
                </td>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                       <asp:Button ID="btnUpload" runat="server" Text="上传Excel" CssClass="adminsubmit" 
                                    onclick="btnUpload_Click" />

                    <asp:LinkButton  Font-Bold="true"   ForeColor="Red"  onclick="downTipBut_Click"  ID="downTipBut" runat="server" Visible="false">下载失败数据信息</asp:LinkButton>
                    <asp:HiddenField ID="hidDownUrl" runat="server" />
                </td>
             
            </tr>
        </table>

    
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
