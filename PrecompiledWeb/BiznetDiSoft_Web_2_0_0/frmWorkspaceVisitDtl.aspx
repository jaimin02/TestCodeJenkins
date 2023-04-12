<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmWorkspaceVisitDtl, App_Web_2mzu20n4" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">

<script type="text/javascript" src="Script/popcalendar.js"></script>

<script language="javascript" type="text/javascript">
     
function Validation()
{
    if (document.getElementById('<%= ddlProjectGroup.ClientId %>').selectedIndex ==0)
    {
        msgalert('Please Select Project Group !');
        document.getElementById('<%= ddlProjectGroup.ClientId %>').focus();
        return false;
    }
    return true;
}                        
   
        
</script>

    <table>
        <tr>
            <td class="Label" style="width: 100%">
                <table style="width: 367px">
                    <tr>
                        <td nowrap="nowrap" style="text-align: right">
                From Date :</td>
                        <td nowrap="nowrap" style="text-align: left">
                <asp:TextBox ID="txtFromDate" runat="server" TabIndex="2" Width="100px"></asp:TextBox>
                <img id="ImgFromDate" alt="Select  Date" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtFromDate,'dd-mmm-yyyy');"
                    src="images/Calendar_scheduleHS.png" />&nbsp; To : &nbsp;<asp:TextBox
                        ID="txtToDate" runat="server" TabIndex="3" Width="100px"></asp:TextBox>
                <img id="ImgToDate" alt="Select  Date" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtToDate,'dd-mmm-yyyy');"
                    src="images/Calendar_scheduleHS.png" />
                        </td>
                    </tr>
                    <tr>
                        <td nowrap="nowrap">
                Project Group:</td>
                        <td>
                <asp:DropDownList ID="ddlProjectGroup" runat="server" CssClass="dropDownList" Width="334px">
                </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="text-align: left">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btnnew" Text="Search" 
                             OnClientClick="return Validation();"/>
                <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" onclientclick="return msgconfirmalert('Are you sure you want to Exit?',this);" /></td>
                    </tr>
                </table>
                &nbsp; &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <asp:GridView ID="gvWorkspaceSubject" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    PageSize="25" ShowFooter="True" SkinID="grdViewAutoSizeMax" style="width:65%; margin:auto;">
                    <Columns>
                        <asp:BoundField DataFormatString="number" HeaderText="#"><ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="vWorkSpaceId" HeaderText="WorkSpaceId">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="vWorkspaceDesc" HeaderText="Project">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="iPeriod" HeaderText="Visit">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="NoOfSubject" HeaderText="Patient Details">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        
                        <%--<asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>

</asp:Content>

