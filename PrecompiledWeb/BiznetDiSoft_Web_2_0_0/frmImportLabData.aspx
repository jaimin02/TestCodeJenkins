<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmImportLabData, App_Web_pna05jsx" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:content id="Content1" contentplaceholderid="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <style type="text/css">
        /*#tblProjectWiseAuditTrail {
            width:auto !important;
        }*/
    </style>

    <table cellspacing="0" cellpadding="5px" width="100%" border="0">
        <tbody>
            <tr>
                <td style="text-align: right; width :30%;" class="Label">
                    Project Name/Request Id*:
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="60%"></asp:TextBox>
                    <asp:Button
                        Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click" runat="server"
                        Text=" Project"></asp:Button>
                    <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                        TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                        OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                        CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                        CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                    </cc1:AutoCompleteExtender>
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap; text-align :right ;" class="Label">
                    Activity*:
                </td>
                <td style ="text-align :left ;">
                    <asp:DropDownList ID="ddlActivity" TabIndex="1" runat="server" CssClass="dropDownList" Width="60%"  AutoPostBack="true"  >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap; text-align :right ;" class="Label" >
                    Select File*:
                </td>
                <td style ="text-align :left ;">
                    <asp:FileUpload ID="flupLabData" runat="server"></asp:FileUpload>
                    <asp:Button ID="btnGo" runat="server" Text="" ToolTip="Go" CssClass="btn btngo" OnClientClick="return ValidationForGo();" />
                    <a href="images/FileFormat_ImportLabData.png"  target="_blank"><b><U>View CSV File Format</U></b></a>    
                  
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.CSV|.csv)$"
    ControlToValidate="flupLabData" runat="server" ForeColor="Red" ErrorMessage="Please select a valid CSV file."
    Display="Dynamic" />

                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap; height: 21px; text-align: left;" class="Label" 
                    colspan="2">
                    <asp:UpdatePanel ID="upMain" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width ="70%" style ="margin :auto;" cellpadding ="5px">
                                <tbody>

                                    <tr>
                                        <td>
                                            <div style="width: 20px; height: 20px; float: left; background-color: red; font-weight: bold;"></div>
                                            <div style="margin-left:25px;">Invisible Attributes</div>
                                         </td>
                                    </tr>

                                    <tr>
                                        <td class ="Label" style ="width :30%;">
                                             Source Fields   
                                        </td>
                                        <td class ="Label" style =" width :30%;">
                                            CSV Import Fields
                                        </td>
                                        <td style =" width :9%;" >
                                        </td>
                                        <td class ="Label">
                                            Mapped Fields
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td valign="top">
                                        <%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
                                            <asp:Panel ID="pnlSourceFields" runat="server" Width="100%" Height="230px" ScrollBars="Auto"
                                                BorderColor="Navy" BorderWidth="1px" CssClass="Label">
                                                <asp:CheckBoxList ID="chkSourceColumns" runat="server">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                              <%--      </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnMap" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlActivity" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>--%>
                                        </td>
                                        <td valign="top">
                                            <asp:Panel ID="pnlTargetFields" runat="server" Width="100%" Height="230px" ScrollBars="Auto"
                                                BorderColor="Navy" BorderWidth="1px" CssClass="Label">
                                                <asp:CheckBoxList ID="chkTargetColumns" runat="server">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnMap" runat="server" Text="Map" CssClass="btn btnnew" ></asp:Button>
                                        </td>
                                        <td valign="top">
                                            <asp:UpdatePanel ID="upMappedFields" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlGrid" runat="server" Width="100%" Height="230px" ScrollBars="Auto"
                                                        BorderColor="Navy" BorderWidth="1px" CssClass="Label">
                                                        <asp:GridView ID="GVWMapping" runat="server" SkinID="grdViewSmlAutoSize" AutoGenerateColumns="False">
                                                            <Columns>
                                                                <asp:BoundField HeaderText="#" DataField="Sr.No" />
                                                                <asp:BoundField HeaderText="Source Field" DataField="Source Field" />
                                                                <asp:BoundField HeaderText="Target Field" DataField="Target Field" />
                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <%-- <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="~/images/cancel.gif" />--%>
                                                                        <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="~/Images/i_delete.gif"
                                                                            OnClientClick="return msgconfirmalert('Are You Sure You Want To Delete?',this)" ToolTip="Delete" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnMap" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnImport" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
           
             <tr>
                <td class="Label" style ="text-align :center ;" colspan ="2">
                    <asp:Button ID="btnImport" runat="server" Text="Import" ToolTip="Import" CssClass="btn btnnew" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel" />
                    <asp:Button ID="btnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                        OnClientClick=" return msgconfirmalert('Are you sure you want to Exit?',this); " />
                </td>
            </tr>
        </tbody>
    </table>

     
    <div id="myModal" class="modal" runat="server">
        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header" style="text-align: left">
                <img id="Img1" alt="Close1" src="images/close_pop.png" class="close modalCloseImage" title="Close" onclick="ModalPopupClose()"/>
                <h3 style="text-align: center">
                    <asp:Label runat="server" ID="modalHeading">Remarks</asp:Label></h3>
            </div>
            <div class="modal-body">
                <table width="100%">
                    <tr id="trRandomizationRemarks">
                        <td align="left" style="font-weight: bold; font-size: small;">Remarks *
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="textBox" Height="65px" Width="80%" />
                        </td>
                    </tr>
                    <tr id="trRandomizationGenerate" >
                        <td align="center" style="font-weight: bold; font-size: small;" colspan="2">
                            <asp:Label runat="server" ID="lblAlertStr"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnSaveDataWithRemarks" runat="server" CssClass="btn btnsave" Text="OK" Width="105px" onClientClick="return ValidateForRemarks()" onclick="btnImport_Click" />
                                    <input id="btnCloseRemarks"  type="button" class="btn btnclose" title="Close" onclick="ModalPopupClose()" style="width:105px" value="Close" />
                                </ContentTemplate>
                                <Triggers>
                                    <%--<asp:AsyncPostBackTrigger ControlID="btnSaveRandomizationNoSave" EventName="Click" />--%>
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div id="dvTabluer" style="white-space: nowrap; width:100%; overflow:auto; margin:0 auto;">   
        <table id="tblImportData" >
        </table>
    </div>
    
    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script src="Script/jquery.multiselect.min.js" type="text/javascript"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    
    <script type="text/javascript" language="javascript">

        function ModalPopupClose() {
            document.getElementById('<%=myModal.ClientID()%>').style.display = 'none';
            $("[id$='txtRemarks']").val("");
        }
        function ModalPopupOpen(str) {
            ////alert(str);
            document.getElementById('<%=myModal.ClientID()%>').style.display = 'inline';
            $("[id$='txtRemarks']").val("");
            return false;
        }

        function ValidateForRemarks() {
            if ($("[id$='txtRemarks']").val().trim() == "") {
                alert("Please Enter Remarks !");
                return false;
            }
            return true;
        }
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }
        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function ValidationForGo() {
            if (document.getElementById('<%= HProjectId.ClientId %>').value.trim() == '') {
                msgalert('Please Select Project !');
                document.getElementById('<%= txtProject.ClientId%>').value = '';
                document.getElementById('<%= txtProject.ClientId%>').focus();
                return false;
            }
            else if (document.getElementById('<%= flupLabData.ClientId %>').value.trim() == '') {
                msgalert('Please Select File !');
                document.getElementById('<%= flupLabData.ClientId%>').value = '';
                document.getElementById('<%= flupLabData.ClientId%>').focus();
                return false;
            }
        return true;
    }


    function GetDataAlreadyExist() {
        var ddlActivity = $("[id$='ddlActivity']").val();
        var iNodeId = ddlActivity.split("#")[0];
        var iNodeIndex = ddlActivity.split("#")[1];
        var vActivityId = ddlActivity.split("#")[2];

        $.ajax({
            type: "POST",
            url: "frmImportLabData.aspx/GetDataAlreadyImportCSVFile",
            data: '{"vWorkspaceId":"' + $("[id$='HProjectId']").val() + '" , "iNodeId":"' + iNodeId + '" , "iNodeIndex":"' + iNodeIndex + '" , "vActivityId":"' + vActivityId + '" }',
            contentType: "application/json; charset=utf-8",
            datatype: JSON,
            async: false,
            success: SuccessData,
            failure: function (response) {
                msgalert(response.d);
            },
            error: function (response) {
                msgalert(response.d);
            }
        });



    }

    function SuccessData(jsonData) {

        jsonData = jQuery.parseJSON(jsonData.d);
        var ActivityDataset = [];
        if (jsonData.length > 0) {

            tableHeaders = "";
            var columnHeader;

            columnHeader = jsonData[0];

            $.each(columnHeader, function (key, value) {
                tableHeaders += "{\"sTitle\" : \"" + key + "\"},";
            });
            tableHeaders = "[" + tableHeaders + ",]";
            tableHeaders = tableHeaders.replace(",,", "");

            // For Dynamic Column Data
            for (var i = 0; i < jsonData.length; i++) {
                var ColumnValue = "";
                var ColumnData;
                ColumnData = jsonData[i];
                $.each(ColumnData, function (key, value) {
                    ColumnValue += "'" + value + "',";
                });
                ColumnValue = "[" + ColumnValue + ",]";
                ColumnValue = ColumnValue.replace(",,", "")
                ColumnValue = ColumnValue.replace(/'/g, '"');
                ColumnValue = eval('(' + ColumnValue + ')');
                ActivityDataset.push(ColumnValue);
            }


            var otableProjectWiseAuditTrail = $('#tblImportData').dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "aaData": ActivityDataset,
                "aaSorting": [],
                "bInfo": true,
                "bAutoWidth": false,
                "bDestroy": true,
                "aoColumns": eval('(' + tableHeaders + ')'),
                "oLanguage": {
                    "sEmptyTable": "No Record Found",
                },
                //"sScrollX": "100%",
                //"sScrollXInner": "2200" /* It varies dynamically if number of columns increases */,
                "columnDefs": [
                {
                    "targets": [0, 1, 2, 3, 4],
                    "visible": false,
                    "searchable": false
                },
                ]
            });
        }
        else {
            //alert("No details found for selected criteria.");
            return false;
        }

    }


    function TablulerFormate(DataRepetation, DataAttribute) {
        var WorkspaceID = $("[id$='HProjectId']").val();
        var ddlActivity = $("[id$='ddlActivity']").val();
        var ActivityId = ddlActivity.split("#")[2];
        var NodeId = ddlActivity.split("#")[0];
        var StatusTableBind = "true";

        $("#dvTabluer").css({ "max-width": $(window).width() * 0.95 });


        $.ajax({
            type: "post",
            url: "frmImportLabData.aspx/TablulerRepetationGrid",
            data: '{"WorkspaceID" :"' + WorkspaceID + '","ActivityId":"' + ActivityId + '","NodeId":"' + NodeId + '"}',
            contentType: "application/json; charset=utf-8",
            datatype: JSON,
            async: false,
            dataType: "json",
            success: SuccessData,

            failure: function (response) {
                alert(response.d);
            },
            error: function (response) {
                alert(response.d);
            }
        });

        /* Apply the tooltips */
        //oTable.$('tr').tooltip({
        //    "delay": 0,
        //    "track": true,
        //    "fade": 250
        //});

        $(".dataTables_wrapper").css("width", ($(window).width() * 0.90 | 0) + "px");

        //var abc = $("#tblProjectWiseAuditTrail").height();
        //abc = abc + 28 + "px";
        $(".tblImportData").removeAttr("style");
        //var para = "position: relative; overflow: auto; width: 100%;height:" + abc;
        var para = "display: block; overflow: scroll";

        $("#tblImportData").attr("style", para);

        return false;
    }

    </script>

</asp:content>
