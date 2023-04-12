<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false"
    CodeFile="frmSubjectCDMSAdvanceSearch.aspx.vb" Inherits="CDMS_frmSubjectCDMSAdvanceSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" href="../App_Themes/CDMS.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/smoothnessjquery-ui.css" />

    <script src="../Script/jquery-1.9.1.js" type="text/javascript"></script>

    <script src="../Script/jquery-ui.js" type="text/javascript"></script>

    <script src="../Script/General.js" type="text/javascript"></script>

    <script src="../Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="../Script/AutoComplete.js" type="text/javascript"></script>

    <style type="text/css">
        .ui-widget
        {
            font-size: 0.9em !important;
            text-align: left !important;
        }
        /* Added by Pratik Soni on 1/11/2013 */.ui-autocomplete
        {
            max-height: 200px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden;
        }
        /*--------------------------------*/.ui-menu-item:hover
        {
            background: rgb(30,87,153); /* Old browsers */
            background: -moz-linear-gradient(top, rgba(30,87,153,1) 0%, rgba(41,137,216,1) 50%, rgba(32,124,202,1) 100%, rgba(125,185,232,1) 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(30,87,153,1)), color-stop(50%,rgba(41,137,216,1)), color-stop(100%,rgba(32,124,202,1)), color-stop(100%,rgba(125,185,232,1))); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* IE10+ */
            background: linear-gradient(to bottom, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=                                                                                '#1e5799' , endColorstr= '#7db9e8' ,GradientType=0 ); /* IE6-9 */
            color: White !important;
            font-weight: bolder;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <fieldset class="FieldSetBox" style="width: 80%; margin: auto; text-align: left;">
                <legend id="lblStatus" runat="server" class="LegendText" style="color: Black; font-size: 12px;">
                    Advance Search Query </legend>
                <table id="tblSearch" width="100%">
                </table>
                <div style="height: 4px;">
                </div>
                <asp:Label ID="lblQuery" runat="server" Text="" Style="color: Black; font-weight: bold;
                    font-size: 10pt;" CssClass="labeltext"></asp:Label>
                <hr />
                <div style="width: 100%; text-align: center;">
                    <asp:Button ID="btnAdvanceSearch" CssClass="btn btnadd" runat="server" Text="Filter"
                        Style="font-weight: bold;" ToolTip="Filter Subjects According To Selected Criteria"
                        OnClientClick="return ValidateAdvanceSearch();" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" ToolTip="Reset Filter Query & Data"
                        CssClass="btn btnadd" />
                    <asp:Button ID="btnAdvanceSearchExport" CssClass="btn btnexcel " runat="server" Text=""
                        Visible="false" Style="font-weight: bold;" ToolTip="Export Filtered Subjects To Excel" />
                    <%--<asp:Button ID="btnAddToGrid" runat="server" CssClass="buttontext" Text="Send" Style="font-weight: bold;"
                        ToolTip="Add To Grid For Booking Of The Subjects" OnClientClick="return ValidateExportOrRender();"
                        Visible="false" />--%>
                    <input id="btnSend" type="button" value="Send" title="Add To Grid For Booking Of The Subjects"
                        class="btn btnadd" style="display: none;" onclick="return ValidateExportOrRender();"
                        runat="server" />
                </div>
            </fieldset>
            <fieldset class="FieldSetBox" id="fldGrid" runat="server" visible="false" style="width: 80%;
                margin: auto; text-align: left; margin-top: 2%;">
                <legend id="Legend1" runat="server" class="LegendText" style="color: Black; font-size: 12px;">
                    Advance Search Result </legend>
                <table style ="width:100%;">
                    <tr>
                        <td align="center" style="padding-bottom: 1px;">
                            <asp:PlaceHolder ID="phTopPager" runat="server"></asp:PlaceHolder>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GrdSubject" runat="server" AutoGenerateColumns="false" 
                                ShowFooter="True" SkinID="grdViewSmlAutoSize" Style="width: 100%; margin-top: 2%;">
                                <HeaderStyle BackColor="#3A87AD" />
                                <Columns>
                                    <asp:BoundField DataField ="RowNo" HeaderText="Sr No">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" Wrap="true" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vSubjectID" HeaderText="Subject ID">
                                        <ItemStyle HorizontalAlign="Center" Wrap="true" Width="8%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" Width="8%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vSubjectName" HeaderText="Subject Name">
                                        <ItemStyle HorizontalAlign="Center" Wrap="true" Width="20%" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="true" Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="dBirthDate" HeaderText="D.O.B." DataFormatString="{0:dd-MMM-yyyy}"
                                        HtmlEncode="false">
                                        <ItemStyle HorizontalAlign="Center" Width="8%" Wrap="true" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cSex" HeaderText="Gender">
                                        <ItemStyle HorizontalAlign="Center" Width="8%" Wrap="true" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="8%" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vContactNo1" HeaderText="Contact No">
                                        <ItemStyle HorizontalAlign="Left" Width="34%" Wrap="true" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="34%" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="nHeight" HeaderText="Height">
                                        <ItemStyle HorizontalAlign="Left" Width="5%" Wrap="true" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="nWeight" HeaderText="Weight">
                                        <ItemStyle HorizontalAlign="Center" Width="5%" Wrap="true" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="nBMI" HeaderText="BMI">
                                        <ItemStyle HorizontalAlign="Center" Width="12%" Wrap="true" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%" Wrap="true" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="padding-top: 1px;">
                            <asp:PlaceHolder ID="phBottomPager" runat="server"></asp:PlaceHolder>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAdvanceSearchExport" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnSearchQuery" runat="server" />
    <asp:HiddenField ID="hdnMedicalValue" runat="server" />

    <script type="text/javascript" language="javascript">
        var content = {};

        function pageLoad() {
            CreateTableForAdvanceSearch(0);
            var Query = $('#<%= hdnSearchQuery.ClientID %>').val();
         var MedQuery = $('#<%= hdnMedicalValue.ClientID %>').val();
          var str = ""
          if (MedQuery != "") {
              str = "AND vDescription = '" + MedQuery + "'"
          }
          if (Query != "") {
              $('#ctl00_CPHLAMBDA_lblQuery').text("Filter Query :" + Query.substring(3, Query.length));
          }

      }

      function CreateTableForAdvanceSearch(objects) {

          var str = "";
          if (objects == "0") {
              str = "<tr class = 'FirstTr'><th></th><th class='LabelText' style='text-align:center;'>Select Criteria</th><th class='LabelText' style='text-align:center;'>Select Operator</th><th class='LabelText' style='text-align:center;'>Value</th><th class='LabelText' style='text-align:center;'>Operator</th></tr>";
              str += "<tr>";
              str += "<td style='width:5%;'><img src='images/add.png' class='addCls' title='ADD' style ='cursor : pointer;'/></td>"


          }
          else {
              str += "<tr>";
              str += "<td style='width:5%;'><img src='images/add.png' class='addCls' title='ADD' style ='cursor : pointer;'/></td>"
          }
          str += "<td style='width:25%; text-align: center;' class='LabelText'><select  style='width:60%;' id ='SelectSubject' class = 'DDLSubject' ><option  value='vFirstName'>First Name</option><option value='vMiddleName'>Middle Name</option>" +
             "<option value='vSurName'>Sur Name</option><option value='vInitials'>Initials</option><option value='dBirthDate'>Birth Date</option><option value='Age'>Age</option><option value='dEnrollmentDate'>Enrollment Date</option>" +
             "<option value='vContactNo'>Contact No</option><option value='vEmailAddress'>Email Address</option><option value='vPlace'>Place</option><option value='cSex'>Sex</option>" +
             "<option value='vRace'>Race</option><option value='nHeight'>Height</option><option value='vTransportation'>Transportation</option>" +
             "<option value='vAvailiability'>Availiability</option><option value='cRegularDiet'>Regular Diet</option><option value='nWeight'>Weight</option><option value='nBMI'>BMI</option>" +
             "<option value='nBloodAvailable'>Blood Available</option><option value='dWashOutDate'>WashOut Date</option><option value='nBloodUsed'>Blood Used</option><option value='cStatus'>Status</option><option value='vDescription'>Medical Description</option><option value='vLastStudy'>Last Study</option>" +
             "</select></td>";
          str += "<td style='width:25%;text-align: center;' class='LabelText' ><select id='selectOperator' style='width:60%;' class='DDLOperator' ><option  value='='>Equal</option><option class='DDLOperator' value='<>'>Not Equal</option><option class='DDLOperator'  value ='>'>Greater Than</option>"
             + "<option   value = '<'>Less Than</option><option  value = '>='>Greater Than Equal</option><option  value = '<='>Less Than Equal</option></select></td>";
          str += "<td style='width:25%;text-align: center;' class='clsvaluefieldTD LabelText'><input type='TEXTBOX' class='clsvaluefield'/></td>";
          str += "<td style='width:20%;text-align: center;' class='LabelText'><select id='SelectClause'  class='DDLClause' style='width:60%;'><option class='DDLClause' value='AND'>And</option><option value='OR'>Or</option></select></td>"
          str += "</tr>"

          $('#tblSearch').append(str);
          initRemoveClick();
          initADDClick();
          initCriteriaChange();
      }

      function initRemoveClick() {

          $('.btnRemoveTr').unbind('click').click(function () {
              $(this).closest('tr').remove();
          });
      }
      function initADDClick() {
          $('.addCls').unbind('click').click(function () {
              $(this).attr('src', 'images/minus.png');
              $(this).attr('class', 'btnRemoveTr');
              $(this).attr('title', 'Remove');
              CreateTableForAdvanceSearch(1);
          });
      }
      function initCriteriaChange() {
          $('.DDLSubject').unbind('change').change(function () {
              if ($(this).val() == "cSex") {
                  $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
                  $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<select class='clsvaluefield' style='width:68%;'><option value = '0'>Select Gender</option><option value = 'M'>Male</option><option value = 'F'>Female</option></select>");
              }
              else if ($(this).val() == "vRace") {
                  $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
                  $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<select class='clsvaluefield' style='width:68%;'>" +
                                   "<option value = '0'>Select Race</option><option value = 'Asian/Oriental'>Asian/Oriental</option><option value = 'Black'>Black</option><option value = 'Caucasian'>Caucasian</option>" +
                                   "<option value = 'Hispanic'>Hispanic</option><option value = 'Mulatto'>Mulatto</option><option value = 'Nativ'>Nativ</option>" +
                                   "<option value = 'Verify at Screening'>Verify at Screening</option>"
                                   + "</select>");
              }
              else if ($(this).val() == "vAvailiability") {
                  $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
                  $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<select class='clsvaluefield' style='width:68%;'>" +
                                   "<option value = '0'>Select Availibility</option><option value = 'Includes all availability'>Includes all availability</option><option value = 'Monday to Friday'>Monday to Friday</option><option value = 'Friday to Monday'>Friday to Monday</option>" +
                                   "</select>");
              }
              else if ($(this).val() == "cStatus") {
                  $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
                  $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<select class='clsvaluefield' style='width:68%;'>" +
                                   "<option value = '0'>Select Status</option><option value = 'AC'>Active</option><option value = 'IA'>In Active</option><option value = 'FI'>Forever Ineligible</option>" +
                                   "</select>");
              }
              else if ($(this).val() == "vTransportation") {
                  $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
                  $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<select class='clsvaluefield' style='width:68%;'>" +
                                   "<option value = '0'>Select Transport</option><option value = 'Public Transportation'>Public Transportation</option><option value = 'Car'>Car</option><option value = 'None'>None</option>" +
                                   "</select>");
              }
              else if ($(this).val() == "cRegularDiet") {
                  $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
                  $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<select class='clsvaluefield' style='width:68%;'>" +
                                   "<option value = '0'>Select Diet</option><option value = 'Y'>Yes</option><option value = 'N'>No</option>" +
                                   "</select>");
              }
              else if ($(this).val() == "vDescription") {
                  $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
                  $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<input type='TEXTBOX' placeholder='Type keyword here' class='clsvaluefield'/>");
                  $(this).closest('tr').find('.clsvaluefield').autocomplete({
                      source: function (request, response) {
                          content.SelectText = request.term;
                          $.ajax({
                              type: "POST",
                              url: "frmSubjectCDMSAdvanceSearch.aspx/GetDescriptionColumn",
                              data: JSON.stringify(content),
                              contentType: "application/json; charset=utf-8",
                              dataType: "json",
                              error: function (errorThrown) {
                              },
                              success: function (result) {
                                  var fndata = $.parseJSON(result.d)
                                  response($.map(fndata, function (item) {
                                      return {
                                          label: item.vDescription,
                                          value: item.vDescription
                                      };
                                  }));
                              }
                          });
                      },
                      minLength: 2
                  });
              }

              else if ($(this).val() == "dBirthDate" || $(this).val() == "dEnrollmentDate" || $(this).val() == "dWashOutDate") {
                  $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
                  $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<input type='TEXTBOX' class='clsvaluefield' />");
                  $('.clsvaluefield').datepicker({});
              }
              else if ($(this).val() == "nHeight" || $(this).val() == "nWeight" || $(this).val() == "nBMI" || $(this).val() == "nBloodAvailable" || $(this).val() == "nBloodUsed") {
                  $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
                  $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<input type='TEXTBOX' class='clsvaluefield' />");
                  $(this).closest('tr').find('.clsvaluefield').ForceNumericOnly();

              }
              else {
                  $(this).closest('tr').children('.clsvaluefieldTD').children().remove();
                  $(this).closest('tr').children('.clsvaluefieldTD').wrapInner("<input type='TEXTBOX' class='clsvaluefield' />");
              }
          });
      }

      //cRegularDiet,vFirstName,vMiddleName,vSurName,cSex,vRace,vAvailiability,vInitials,vTransportation,dBirthDate,dEnrollmentDate,vContactNo,vEmailAddress,vPlace,vLanguage,nHeight,Weight,nBMI,vLastStudy,nBloodUsed,dWashOutDate,nBloodAvailable            
      function ValidateAdvanceSearch() {
          debugger;
          var NoOfTr = $('#tblSearch tr').size() - 1, criteria = "", operator = "", Fieldvalue = "", clause = "", Wstr = "", query = "AND";
          //var query = "SELECT * FROM View_CDMSSubjectDetails_Medicalcondition WHERE "
          $('#<%= hdnMedicalValue.ClientID %>').val("");
            for (var i = 1; i <= NoOfTr; i++) {
                if ($('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').val() != "" && $('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').val() != "0") {
                    $('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').css('border-color', '');
                    //criteria = $('#tblSearch tr:eq(' + i + ')').find('.DDLSubject').val();
                    criteria = $('#tblSearch tr:eq(' + i + ')').find('#SelectSubject').val();
                    if (criteria == "vDescription") {
                        $('#<%= hdnMedicalValue.ClientID %>').val($('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').val());
                        //operator = $('#tblSearch tr:eq(' + i + ')').find('.DDLOperator').val(); 
                        operator = $('#tblSearch tr:eq(' + i + ')').find('#selectOperator').val();
                        Fieldvalue = $('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').val();
                        //clause = $('#tblSearch tr:eq(' + i + ')').find('.DDLClause').val();
                        clause = $('#tblSearch tr:eq(' + i + ')').find('#SelectClause').val();
                        query += "  " + criteria + " " + operator + " '" + Fieldvalue + "' " + clause;
                    }
                    else {
                        //operator = $('#tblSearch tr:eq(' + i + ')').find('.DDLOperator').val();
                        operator = $('#tblSearch tr:eq(' + i + ')').find('#selectOperator').val();
                        Fieldvalue = $('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').val();
                        //clause = $('#tblSearch tr:eq(' + i + ')').find('.DDLClause').val();
                        clause = $('#tblSearch tr:eq(' + i + ')').find('#SelectClause').val();
                        query += "  " + criteria + " " + operator + " '" + Fieldvalue + "' " + clause;
                    }
                }
                else {
                    msgalert('Please Enter Value Or Select From Dropdown !');
                    $('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').css('border-color', 'red');
                    $('#tblSearch tr:eq(' + i + ')').find('.clsvaluefield').focus();
                    return false;
                }
            }
            Wstr = query.substring(0, query.toString().length - 3);

            if ($('#<%= hdnSearchQuery.ClientID %>').val() == "") {
                $('#<%= hdnSearchQuery.ClientID %>').val(Wstr);
            }
            else {
                // Wstr = Wstr.replace("SELECT * FROM View_CDMSSubjectDetails_Medicalcondition WHERE ","");
                var Qstr = $('#<%= hdnSearchQuery.ClientID %>').val() + Wstr;
                $('#<%= hdnSearchQuery.ClientID %>').val(Qstr);
            }


            return true
        }


        //prevent alphabate from entering textbox   
        jQuery.fn.ForceNumericOnly =
       function () {
           return this.each(function () {
               $(this).keydown(function (e) {
                   var key = e.charCode || e.keyCode || 0;
                   // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY
                   // home, end, period, and numpad decimal
                   return (
                        key == 8 ||
                        key == 9 ||
                        key == 46 ||
                        key == 110 ||
                        key == 190 ||
                        (key >= 35 && key <= 40) ||
                        (key >= 48 && key <= 57) ||
                        (key >= 96 && key <= 105));
               });
           });
       };

        function ValidateExportOrRender() {
            var parWin = window.opener;
            if (parWin != null && typeof (parWin) != 'undefined') {
                parWin.SetGrid();
                self.close();
            }
        }

    </script>

</asp:Content>
