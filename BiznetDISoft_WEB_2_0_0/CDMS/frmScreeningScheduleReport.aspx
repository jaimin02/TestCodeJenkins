<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false"
    CodeFile="frmScreeningScheduleReport.aspx.vb" Inherits="CDMS_frmScreeningScheduleReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" href="../App_Themes/CDMS.css" />

    <script src="../Script/jquery-1.9.1.js" type="text/javascript"></script>

    <script src="../Script/jquery-ui.js" type="text/javascript"></script>

    <script src="../Script/General.js" type="text/javascript"></script>

    <script src="../Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="../Script/AutoComplete.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="upSubject" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div id="tabs" style="text-align: left; width: 100%;">
                <table cellspacing="5px" style="width: 95%; margin: auto;">
                    <tr>
                        <td colspan="2">
                            <fieldset class="FieldSetBox" style="width: 22%; height: 150px; float: left;">
                                <legend id="lblStatus" runat="server" class="LegendText" style="color: Black; font-size: 12px;">
                                    Study</legend>
                                <table cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td class="LabelText" style="text-align: center">
                                            <asp:RadioButtonList ID="rblstatus" runat="server" ToolTip="Project/Date & Location"
                                                RepeatDirection="Vertical" AutoPostBack="true" Width="85%">
                                                <asp:ListItem Value="0" Text="Project & Location" />
                                                <asp:ListItem Value="1" Text="Date & Location &nbsp&nbsp" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <fieldset class="FieldSetBox" style="width: 73%; height: 150px; float: left;">
                                <legend class="LegendText" style="color: Black; font-size: 12px;">Search Subject</legend>
                                <table cellpadding="5px" width="100%">
                                    <tr id="trProject" runat="server" style="display: none;">
                                        <td style="text-align: right; width: 30%;" class="LabelText">
                                            <asp:Label ID="lblproject" runat="server" Text="Select Project* :" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtprojectForsubject" runat="server" CssClass="TextBox" placeholder="Please Enter Project No..."
                                                Width="65%" />
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
                                    <tr id="trDate" runat="server">
                                        <td style="text-align: right; width: 30%;" class="LabelText">
                                            <asp:Label ID="lbldate" runat="server" Text="Select Date :" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtdate" runat="server" Width="40%" placeholder="Pick a date..."
                                                ToolTip="Please enter date in dd-mm-yyyy format (e.g. '01012013'/'01-Jan-2013')"
                                                onChange="DateConvertForScreening(this.value,this);" />
                                            <cc1:CalendarExtender ID="caltxtdate" runat="server" TargetControlID="txtdate" Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="LabelText">
                                            <asp:Label ID="lblSubject" runat="server" Text="Select Location* :" />
                                        </td>
                                        <td style="text-align: left;" class="labeltext">
                                            <asp:DropDownList ID="ddlLoction" runat="server" Width="42%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;" colspan="2" class="LabelText">
                                            <asp:Button ID="btnGo" runat="server" Text="" ToolTip="GO" CssClass="btn btngo"
                                                OnClientClick="return validation();" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel" />
                                            <asp:Button ID="btnExport" runat="server" CssClass="btn btnexcel"
                                                ToolTip="Export Grid Data To Excel" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset class="FieldSetBox" style="width: 97.5%; height: auto; float: left;">
                                <legend class="LegendText" style="color: Black; font-size: 12px;">Subject List</legend>
                                <asp:GridView ID="GrdSubject" runat="server" AutoGenerateColumns="false" AllowPaging="True"
                                    PageSize="10" ShowFooter="True" OnRowDataBound="GrdSubject_RowDataBound" SkinID="grdViewSmlAutoSize"
                                    Style="width: 100%; margin-top: 2%;">
                                    <HeaderStyle BackColor="#3A87AD" />
                                    <Columns>
                                        <asp:BoundField DataFormatString="number" HeaderText="Sr No">
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Wrap="true" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vProjectNo" HeaderText="Project No.">
                                            <ItemStyle HorizontalAlign="Center" Wrap="true" Width="8%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" Width="8%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vSubjectID" HeaderText="Subject ID">
                                            <ItemStyle HorizontalAlign="Center" Width="8%" Wrap="true" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vRsvpId" HeaderText="RSVP ID">
                                            <ItemStyle HorizontalAlign="Center" Width="8%" Wrap="true" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vSubjectName" HeaderText="Subject Name">
                                            <ItemStyle HorizontalAlign="Left" Width="34%" Wrap="true" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="34%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vInitials" HeaderText="Initials">
                                            <ItemStyle HorizontalAlign="Left" Width="5%" Wrap="true" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="dBirthdate" HeaderText="D.O.B." DataFormatString="{0:dd-MMM-yyyy}"
                                            HtmlEncode="false">
                                            <ItemStyle HorizontalAlign="Center" Width="10%" Wrap="true" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" Wrap="true" />
                                        </asp:BoundField>
                                       <%--  <asp:BoundField DataField="AgeYearsIntTrunc" HeaderText="Age">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" Width="8%" Wrap="true" />
                                        </asp:BoundField>--%>
                                        <asp:BoundField DataField="cSex" HeaderText="Gender">
                                            <ItemStyle HorizontalAlign="Center" Width="5%" Wrap="true" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="vContactNo1" HeaderText="Contact No">
                                            <ItemStyle HorizontalAlign="Center" Width="12%" Wrap="true" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="dScheduledate" HeaderText="Appointment Date" DataFormatString="{0:dd-MMM-yyyy}"
                                            HtmlEncode="false">
                                            <ItemStyle HorizontalAlign="Center" Width="10%" Wrap="true" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="dStartTime" HeaderText="Appointment Time">
                                            <ItemStyle HorizontalAlign="Center" Wrap="true" Width="5%" />
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" Wrap="true" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="cStatus" HeaderText="Status">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" Width="8%" Wrap="true" />
                                        </asp:BoundField>
                                      <%--  <asp:BoundField DataField="AgeYearsIntTrunc" HeaderText="Age">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" Wrap="true" />
                                            <ItemStyle HorizontalAlign="Center" Width="8%" Wrap="true" />
                                        </asp:BoundField>--%>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
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
      var inyear;

    $(document).ready(function () {
          });

    function ClientPopulatedprojectForsubject(sender, e) 
         {
             ProjectClientShowing('AutoCompleteExtender2', $get('<%= txtprojectForsubject.ClientId %>'));
         }

    function OnSelectedprojectForsubject(sender, e) 
         {
             ProjectOnItemSelected(e.get_value(), $get('<%= txtprojectForsubject.clientid %>'),
             $get('<%= hdnProjectForsubject.clientid %>'),document.getElementById('<%= btnsetsubjectForProject.ClientId %>'));
         }
    
      
    function DateConvertForScreening(ParamDate,txtdate)
        {
        //debugger ;
             if (ParamDate.length == 0)
              {
                 return true;
              }
           
             if (ParamDate.trim() != '') 
              {
                 var dt = ParamDate.trim().toUpperCase();
                 var tempdt;
                 
                 if (dt.indexOf('UK') >= 0 || dt.indexOf('UNK') >= 0 || dt.indexOf('UKUK') >= 0) 
                  {

                     if (dt.length < 8) 
                      {
                         msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                         txtdate.value = "";
                         txtdate.focus();
                         return false;
                      }
                 var day;
                 var month;
                 var year;
                 if (dt.indexOf('-') >= 0) 
                  {
                     var arrDate = dt.split('-');
                     day = arrDate[0];
                     month = arrDate[1];
                     year = arrDate[2];
                  }
                 else 
                  {
                     day = dt.substr(0, 2);
                     month = dt.substr(2, 2);
                     year = dt.substr(4, 4);
                     if (dt.indexOf('UNK') >= 0) 
                      {
                         month = dt.substr(2, 3);
                         year = dt.substr(5, 4);
                      }
                     if (dt.indexOf('UNK') == -1) 
                      {
                         month = dt.substr(2, 2);
                         year = dt.substr(4, 5);
                      }
                   }
                  inyear = parseInt(year, 10);
               
             if (day.length > 2 && day.length != 0) 
              {
                 msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                 txtdate.value = "";
                 txtdate.focus();
                 return false;
              }
             if (month.length > 3 && month.length != 3) 
              {
                 msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                 txtdate.value = "";
                 txtdate.focus();
                 return false;
              }
             if (year.length > 4 && month.length != 4) 
              {
                 msgalert('Please Enter Date In DDMMYYYY Or dd-Mon-YYYY Format Only.');
                 txtdate.value = "";
                 txtdate.focus();
                 return false;
              }
             if (day == 'UK') 
              {
                 tempdt = '01';
              }
             else 
              {
                 tempdt = day;
              }
             if (dt.indexOf('-') >= 0) 
              {
                 tempdt += '-';
              }
             if (month == 'UNK') 
              {
                 tempdt += '01';
              }
            else 
              {
                 tempdt += month;
              }
             if (dt.indexOf('-') >= 0) 
              {
                 tempdt += '-';
              }
            if (year == 'UKUK') 
              {
                 tempdt += '1800';
              }
            else 
              {
                 tempdt += year;
              }
            var chk = false;
            chk = DateConvert(tempdt, txtdate);
             if (chk == true) 
              {
                 if (isNaN(month)) 
                  {
                     txtdate.value = day + '-' + month + '-' + year;
                  }
                else 
                  {
                     txtdate.value = day + '-' + cMONTHNAMES[month - 1] + '-' + year;
                  }
                 if (inyear < 1900)
                  {
                     msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900"  ');
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
         var Year = dt.substring(dt.lastIndexOf('-') + 1);
         inyear = parseInt(Year, 10);
         if (inyear < 1900)
          {
            msgalert('You Can Not Add Date Which Is Less Than "01-Jan-1900"  ');
            txtdate.value = "";
            txtdate.focus();
            return false ;
          }
         return true ;  
     }
   function validation()
     {
      // debugger ;
         if ($('#<%= rblstatus.ClientID %> input[type=radio]:checked').val() == "0")
          { 
             if (document.getElementById('<%=hdnProjectForsubject.ClientID%>').value == "") 
              {
                 document.getElementById('<%=txtprojectForsubject.ClientID%>').value = '';
                 msgalert('Please Enter Project');
                 document.getElementById('<%=txtprojectForsubject.ClientID%>').focus();
                 return false;
            }  
           
          }
    else if ($('#<%= rblstatus.ClientID %> input[type=radio]:checked').val() == "1")
          {
             if (document.getElementById('<%=txtdate.ClientID%>').value.toString().trim().length <= 0) 
             {
                 document.getElementById('<%=txtdate.ClientID%>').focus();
                 document.getElementById('<%=txtdate.ClientID%>').value = '';
                 msgalert('Please Choose Date');
                 return false;
            }      
            
          }
         
         
         if (document.getElementById('<%=ddlLoction.clientid %>').selectedIndex == 0) 
          {
             msgalert('Please Select Location');
             return false;
          } 
                
         return true;       
     }   
    </script>

</asp:Content>
