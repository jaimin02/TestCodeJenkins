<%@ page language="VB" autoeventwireup="false" inherits="SubjectPatientMapping, App_Web_eoahe1pj" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Subject Patient Mapping</title>

    <script src="Script/jquery-1.7.min.js" type="text/javascript"></script>

    <script type="text/javascript">
    function UncheckOthers(obj)
    {
        
        var targetClass=obj.value;
       
        $('input:radio').each(function(){
         
            if ($(this).attr('value')==targetClass){
                $(this).attr('checked', false);
            }
        
        })
        document.getElementById(obj.id).checked=true;
        //var allRadios= document.getElementsByClassName(targetClass);
        
    }

    </script>

</head>
<body>

    <script src="Script/jquery-1.7.min.js" type="text/javascript"></script>

    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnSetProject"   runat="server" class="btn btnnew"></asp:Button>
    </div>
    <div>
        <asp:GridView AllowPaging="true" PageSize="5" runat="server" SkinID="grdView" ID="gvSubjectPatient"
            CssClass=" " AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField HeaderText="#" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:RadioButton runat="server" ID="rbSubjectID" CssClass="rbSubject" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="SubjectID(BizNET)" DataField="Subject" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:RadioButton ID="rbPatientID" runat="server" CssClass="rbPatient" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="PatientID (Lab)" DataField="Patient" />
                <asp:BoundField DataField="vSubjectID" />
                <asp:BoundField DataField="vPatientID"  />
                
            </Columns>
        </asp:GridView>
    </div>
    <div>
        <asp:Button ID="btnMap" Text="Map IDs" runat="server" CssClass="btn btnnew" />
    </div>
    <div>
        <div>
            <asp:GridView AllowPaging="true"  PageSize="10" runat="server" SkinID="grdView" ID="gvMappedResult"
                AutoGenerateColumns="false" Visible="false">
                <Columns>
                    <asp:BoundField HeaderText="#" />
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:Button Text="Delete" CssClass="btn btncancel" OnClientClick="return confirm('Are You Sure to delete This Mapping!')" ID="btnDelete" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="SubjectID(BizNET)" DataField="Subject" />
                    <asp:BoundField HeaderText="PatientID(Lab)" DataField="Patient" />
                    <asp:BoundField DataField="vSubjectID"  />
                    <asp:BoundField DataField="vPatientID" />
                    <asp:BoundField DataField="nSubjectPatientMapping" />
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <asp:Button ID="btnSaveChanges" Text="Save" runat="server" CssClass="btn btnsave" /></div>
    </div>
    </form>
</body>
</html>
