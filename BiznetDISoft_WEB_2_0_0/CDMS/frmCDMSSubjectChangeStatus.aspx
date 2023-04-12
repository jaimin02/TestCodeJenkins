<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false"
    EnableEventValidation="false" CodeFile="frmCDMSSubjectChangeStatus.aspx.vb" Inherits="CDMS_frmCDMSSubjectChangeStatus"
    EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" href="../App_Themes/CDMS.css" />

    <script src="../Script/jquery-1.9.1.js" type="text/javascript"></script>

    <script src="../Script/jquery-ui.js" type="text/javascript"></script>

    <script src="../Script/General.js" type="text/javascript"></script>

    <script src="../Script/Validation.js" type="text/javascript"></script>

    <script src="../Script/AutoComplete.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="upSubject" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div style="width: 100%;">
                <fieldset class="FieldSetBox" style="width: 80%; margin: auto;">
                    <legend class="LegendText" style="color: Black">Status Report</legend>
                    <table width="100%" cellspacing="5px">
                        <tr>
                            <td style="text-align: right; width: 30%;" class="LabelText">
                                <asp:Label ID="lblproject" runat="server" Text="Select Project* :" />
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtprojectForsubject" runat="server" CssClass="TextBox" placeholder="Please Enter Project No..."
                                    Width="60%" onchange="return ChangedTextBox();" />
                                <asp:Button Style="display: none" ID="btnsetsubjectForProject" runat="server" />
                                <asp:HiddenField ID="hdnProjectForsubject" runat="server" />
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" UseContextKey="True"
                                    TargetControlID="txtprojectForsubject" ServicePath="~/AutoComplete.asmx" OnClientShowing="ClientPopulatedprojectForsubject"
                                    OnClientItemSelected="OnSelectedprojectForsubject" MinimumPrefixLength="1" ServiceMethod="GetMyProjectCompletionList"
                                    CompletionListElementID="pnlSubjectForProject" CompletionListItemCssClass="autocomplete_listitem"
                                    CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                                    BehaviorID="AutoCompleteExtender2">
                                </cc1:AutoCompleteExtender>
                                <asp:Panel ID="pnlSubjectForProject" runat="server" Style="max-height: 150px; overflow: auto;
                                    overflow-x: hidden;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;" class="LabelText">
                                <asp:Label ID="lblSubject" runat="server" Text="Select Status* :" />
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlstatus" runat="server" Width="40%">
                                    <asp:ListItem Text="Select Status" Value="0" />
                                    <%--<asp:ListItem Text="Active" Value="AC" />
                                    <asp:ListItem Text="In Active" Value="IA" />--%>
                                    <asp:ListItem Text="Booked" Value="BO" />
                                    <asp:ListItem Text="Hold" Value="HO" />
                                    <asp:ListItem Text="Screened" Value="SC" />
                                    <asp:ListItem Text="On Study" Value="OS" />
                                    <asp:ListItem Text="Forever Ineligible" Value="FI" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;" colspan="2" class="LabelText">
                                <asp:Button ID="btnGo" runat="server" Text="" ToolTip="GO" CssClass="btn btngo" OnClientClick ="return fnValidateGo();" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel" />
                                <asp:Button ID="btnExport" runat="server" Text="" CssClass="btn btnexcel"
                                    ToolTip="Export Grid Data To Excel" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <div id="divgrid" runat="server" style="width: 100%;">
                    <fieldset class="FieldSetBox" style="width: 80%; margin: auto; margin-top: 2%; padding-top: 1%;">
                        <legend class="LegendText" style="color: Black">All Subjects</legend>
                        <div id="tblChangeStatus" runat="server" style="display: none;">
                            <table width="100%" cellspacing="5px">
                                <tr>
                                    <td style="text-align: right; width: 30%;" class="LabelText">
                                        <asp:Label ID="lblstatus" runat="server" Text=" Change Status :" />
                                    </td>
                                    <td style="text-align: left; width: 15%" class="LabelText">
                                        <asp:DropDownList ID="ddlchangeStatus" runat="server" Width="100%">
                                            <asp:ListItem Text="Change Status" Value="0" />
                                            <asp:ListItem Text="Active" Value="AC" />
                                            <asp:ListItem Text="In Active" Value="IA" />
                                            <asp:ListItem Text="Hold" Value="HO" />
                                            <asp:ListItem Text="Screened" Value="SC" />
                                            <asp:ListItem Text="On Study" Value="OS" />
                                            <asp:ListItem Text="Forever Ineligible" Value="FI" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: left;" class="LabelText">
                                        <asp:Button ID="btnChangeStatus" runat="server" Text="Change Status" CssClass="btn btnadd"
                                            ToolTip="Change Status Of The Objects" OnClientClick="return VAlidateStatus();" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:Panel ID="pnlgvwSubjectStatus" runat="server" Style="max-height: 310px; overflow: auto;
                            width: 100%;">
                            <asp:GridView ID="gvwSubjectstatus" runat="server" AutoGenerateColumns="false" SkinID="grdViewSmlAutoSize"
                                Width="100%" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField DataFormatString="number" HeaderText="Sr No">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="Label1" runat="server" Text="Select All"></asp:Label>
                                            <input id="chkSelectAll" onclick="SelectAll(this,'gvwSubjectstatus')" type="checkbox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkMove" runat="server" onclick="Eachcheckbox('gvwSubjectstatus')" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" Wrap="true" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="7%" Wrap="true" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject code">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Subject ID" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lblSubjectCode" runat="server" Target="_blank" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="13%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="13%" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="vRsvpId" HeaderText="RSVP ID">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="true" Width="6%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" Width="6%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vSubjectName" HeaderText="Subject Name">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="true" Width="25%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vInitials" HeaderText="Initials">
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="5%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cSex" HeaderText="Gender">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="dBirthdate" HeaderText="D.O.B." DataFormatString="{0:dd-MMM-yyyy}"
                                        HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cStatus" HeaderText="Current Status">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="dStartDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}"
                                        HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="dEndDate" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}"
                                        HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </fieldset>
                </div>
            </div>
            <button id="BtnChange" runat="server" style="display: none;" />
            <cc1:ModalPopupExtender ID="ModalChangeStatus" runat="server" PopupControlID="divChangeStatus"
                BackgroundCssClass="modalBackground" TargetControlID="BtnChange" BehaviorID="ModalChangeStatus"
                CancelControlID="CancelChangeStaus">
            </cc1:ModalPopupExtender>
            <div id="divChangeStatus" runat="server" class="centerModalPopup" style="display: none;
                left: 30%; width: 40%; position: absolute; top: 525px; max-height: 404px;">
                <table style="width: 100%">
                    <tr>
                        <td class="LabelText" style="text-align: center !important; font-size: 12px !important;
                            width: 97%;">
                            Change Status
                        </td>
                        <td style="text-align: center; height: 22px;" valign="top">
                            <img id="CancelChangeStaus" alt="Close" src="images/Close.png" style="position: relative;
                                float: right; right: 5px; cursor: pointer;" title="Close" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel3" runat="server" Visible="true" Width="100%">
                    <table width="100%">
                        <tr>
                            <td>
                                <table width="100%" cellspacing="2%">
                                    <tr>
                                        <td class="LabelText">
                                            Start Date :
                                        </td>
                                        <td class="LabelText">
                                            <asp:TextBox ID="txtStatusStartDate" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="calStatusStartDate" runat="server" TargetControlID="txtStatusStartDate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td class="LabelText">
                                            End Date :
                                        </td>
                                        <td class="LabelText">
                                            <asp:TextBox ID="txtStatusEndDate" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalStatusEndDate" runat="server" TargetControlID="txtStatusEndDate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td class="LabelText" style="text-align: center;">
                                <asp:Button ID="btnOk" Text="OK" ToolTip="Change Status To Hold" runat="server" OnClientClick ="return fnValidateOK();" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <asp:Button ID="btnWarning" runat="server" Style="display: none;" />
            <cc1:ModalPopupExtender ID="mdlWarning" runat="server" PopupControlID="divWarning"
                BackgroundCssClass="modalBackground" BehaviorID="mdlWarning" TargetControlID="btnWarning">
            </cc1:ModalPopupExtender>
            <div id="divWarning" runat="server" class="centerModalPopup" style="display: none;
                overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td id="WarningHeader" class="LabelText" style="text-align: left !important; font-size: 12px !important;
                            width: 97%;">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td id="WarningMessage" class="LabelText" style="text-align: center !important;">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <input type="button" id="btnWarningOk" value="OK" style="font-size: 12px !important;
                                width: 57px;" onclick="return ClickWarning();" />
                            <%--<asp:Button ID="btnWarningOk" runat="server" Text="Ok" CssClass="ButtonText" Width="57px"
                                Style="font-size: 12px !important;" />--%>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">
        
        function ClientPopulatedprojectForsubject(sender, e) 
           {
               ProjectClientShowing('AutoCompleteExtender2', $get('<%= txtprojectForsubject.ClientId %>'));
           }

        function OnSelectedprojectForsubject(sender, e) 
           {
               ProjectOnItemSelected(e.get_value(), $get('<%= txtprojectForsubject.clientid %>'),
               $get('<%= hdnProjectForsubject.clientid %>'),document.getElementById('<%= btnsetsubjectForProject.ClientId %>'));
               debugger;
           }
           
         function SelectAll(CheckBoxControl, Grid) {
            var str = "";
            var Gvd = document.getElementById('<%=gvwSubjectstatus.ClientId %>');
            if (CheckBoxControl.checked == true) {
                var i;
                var Cell = 0;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        if (document.forms[0].elements[i].disabled == false) {
                            document.forms[0].elements[i].checked = true;
                            Cell += 1;

                            if (str.toString() == "") {
                                str = str + Gvd.lastChild.childNodes[Cell].children[2].innerText;
                            }
                            else {
                                str = str + "," + Gvd.lastChild.childNodes[Cell].children[2].innerText;
                            }

                        }
                    }
                }

                document.getElementById('ctl00_CPHLAMBDA_tblChangeStatus').style.display = '';
                // document.getElementById('autocode').style.display = '';                   
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
                document.getElementById('ctl00_CPHLAMBDA_tblChangeStatus').style.display = 'none';
                // document.getElementById('autocode').style.display = 'none';
            }
        }
    
       
        function Eachcheckbox(Grid) {
            var Checkall = document.getElementById('chkSelectAll');
            var Gvd = document.getElementById('<%=gvwSubjectstatus.ClientId %>');
            var flag = false;
            var sys = false;
            j = 0;

            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                    j = j + 1;
                    if (document.forms[0].elements[i].checked == false) {
                        Checkall.checked = false;
                        var sys = true;
                    }
                    if (document.forms[0].elements[i].checked == true) {
                        var flag = true;
                        if (j == Gvd.rows.length - 2) {
                            Checkall.checked = true;
                        }
                    }
                }
            }
            if (flag == true) {
                document.getElementById('ctl00_CPHLAMBDA_tblChangeStatus').style.display = '';
                //document.getElementById('autocode').style.display = '';    
            }
            else {
                document.getElementById('ctl00_CPHLAMBDA_tblChangeStatus').style.display = 'none';
                // document.getElementById('autocode').style.display = 'none'; 
            }
            if (sys == true) {
                Checkall.checked = false;
            }
        }     
        
    function VAlidateStatus()
    {
       if (document.getElementById('<%=ddlchangeStatus.clientid %>').selectedIndex == 0) {
                msgalert('Please Select Status');
                return false;
            }
            return true 
    }  
    
//    function ChangedDropDown()
//    {
//    debugger ;
//      if($("#ctl00_CPHLAMBDA_ddlstatus").val() == "0")
//         {
//           $("#ctl00_CPHLAMBDA_btnExport").hide();
//           return false ;
//         }
//      else if ($("#ctl00_CPHLAMBDA_txtprojectForsubject").val().trim() == "" )
//      {
//          $("#ctl00_CPHLAMBDA_btnExport").hide();
//          return false ;
//      }   
//       return true ;
//    }  
    
    function ChangedTextBox()
    {
//    debugger ;
      
      if ($("#ctl00_CPHLAMBDA_txtprojectForsubject").val().trim() == "" )
      {
          $("#ctl00_CPHLAMBDA_btnExport").hide();
          $("#ctl00_CPHLAMBDA_ddlstatus").val("0");
          $("#ctl00_CPHLAMBDA_gvwSubjectstatus").hide();
          return false ;
      }   
       return true ;
    }
     var inyear;
 function DateConvertForScreening(ParamDate,txtdate)
        {
        
         if (ParamDate.length == 0)
           {
               return true;
           }
           
         if (ParamDate.trim() != '') {
        
              var dt = ParamDate.trim().toUpperCase();
              var tempdt;
              if (dt.indexOf('UK') >= 0 || dt.indexOf('UNK') >= 0 || dt.indexOf('UKUK') >= 0) {

               if (dt.length < 8) {
                    //$find('mdlWarning').show();
                    //$('#WarningHeader').text('Warning');
                    //$('#WarningMessage').text('Please enter date in DDMMYYYY or dd-Mon-YYYY format only.');
                    msgalert("Please enter date in DDMMYYYY or dd-Mon-YYYY format only.");
                    txtdate.value = "";
                    txtdate.focus();
                    return false;
                }
                var day;
                var month;
                var year;
                if (dt.indexOf('-') >= 0) {
                var arrDate = dt.split('-');
                day = arrDate[0];
                month = arrDate[1];
                year = arrDate[2];
                }
                else {
                day = dt.substr(0, 2);
                month = dt.substr(2, 2);
                year = dt.substr(4, 4);
                if (dt.indexOf('UNK') >= 0) {
                    month = dt.substr(2, 3);
                    year = dt.substr(5, 4);
                }
                if (dt.indexOf('UNK') == -1) {
                    month = dt.substr(2, 2);
                    year = dt.substr(4, 5);
                }
            }
               inyear = parseInt(year, 10);
               
            if (day.length > 2 && day.length != 0) {
                msgalert("Please enter date in DDMMYYYY or dd-Mon-YYYY format only.");
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (month.length > 3 && month.length != 3) {
                msgalert("Please enter date in DDMMYYYY or dd-Mon-YYYY format only.");
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (year.length > 4 && month.length != 4) {
                msgalert("Please enter date in DDMMYYYY or dd-Mon-YYYY format only.");
                txtdate.value = "";
                txtdate.focus();
                return false;
            }
            if (day == 'UK') {
                tempdt = '01';
            }
            else {
                tempdt = day;
            }
            if (dt.indexOf('-') >= 0) {
                tempdt += '-';
            }
            if (month == 'UNK') {
                tempdt += '01';
            }
            else {
                tempdt += month;
            }
            if (dt.indexOf('-') >= 0) {
                tempdt += '-';
            }
            if (year == 'UKUK') {
                tempdt += '1800';
            }
            else {
                tempdt += year;
            }
            var chk = false;
            chk = DateConvert(tempdt, txtdate);
            if (chk == true) {
                if (isNaN(month)) {
                    txtdate.value = day + '-' + month + '-' + year;
                }
                else {
                    txtdate.value = day + '-' + cMONTHNAMES[month - 1] + '-' + year;
                }
                var currentDate=new Date();
                var date= currentDate .getDate() + "-" + cMONTHNAMES[currentDate .getMonth()] + "-" +currentDate .getFullYear();
                var difference = GetDateDifference(txtdate.value, date);
               if (difference.Days > 0)
                {
                    msgalert("You can not add date which is less than current date !");
                    txtdate.value = "";
                    txtdate.focus();
                    return false ;
                }
                    return true;
            }
            txtdate.value = "";
            txtdate.focus();
            return false;
        }
     }
             DateConvert(txtdate.value, txtdate);
             dt = txtdate.value;
             var currentDate=new Date();
             var date= currentDate .getDate() + "-" + cMONTHNAMES[currentDate .getMonth()] + "-" +currentDate .getFullYear();
             //var Year = dt.substring(dt.lastIndexOf('-') + 1);
             var difference = GetDateDifference(dt, date);
             if (difference.Days>0)
              {
                    msgalert("You can not add date which is less than current date !");
                    txtdate.value = "";
                    txtdate.focus();
                    return false ;
              }
     return true ;  
 }  
 function ClickWarning()
 {
   $find('mdlWarning').hide();  
   return false;
  }
  function fnValidateGo() 
   {
       if ($('#<%= txtprojectForsubject.ClientID %>').val() == "" && $('#<%= ddlstatus.ClientID %>').val() == 0)
         {
              msgalert("Please Enter Project Or Select any Status.");
              return false ;
         }     
   }
     
 function fnValidateOK()
   {
       if($('#<%= txtStatusStartDate.ClientId %>').val() == "")
       {
           msgalert("Please Select Start Date");
           $('#<%= txtStatusStartDate.ClientId %>').focus();
           return false ;
       }
     else if($('#<%= txtStatusEndDate.ClientId %>').val() == "")
       {
          msgalert("Please Select End Date");
          $('#<%= txtStatusEndDate.ClientId %>').focus();
          return false ;
       }
      return true ;   
   }       
    </script>

</asp:Content>
