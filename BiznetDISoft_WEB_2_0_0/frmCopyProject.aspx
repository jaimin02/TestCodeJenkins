<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmCopyProject.aspx.vb" Inherits="frmCopyProject" %>

<asp:Content ID="conMyProject" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        #loadingmessage {
            display: none;
            position: fixed;
            z-index: 1000;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            background: rgba( 255, 255, 255, .5 ) url('images/AjaxLoader.gif') 50% 50% no-repeat;
        }

        .dataTables_wrapper {
            width: 1300px;
          
        }
        .dataTables_scrollBody {
            max-height:390px !important;
          
        }
    </style>
    <table cellpadding="0px" width="100%">
        <tr>
            <td style="text-align: center;">
                <table width="100%">
                    <tr>
                        <td>
                            <table cellpadding="4" cellspacing="2" width="100%">
                                <tr>
                                    <td style="width: 100%; text-align: center">
                                        <asp:UpdatePanel ID="upnlProjectsGridView" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvwProjects" runat="server" CssClass="gvwProjects" OnPageIndexChanging="gvwProjects_PageIndexChanging"
                                                    AutoGenerateColumns="False" OnRowCreated="gvwProjects_RowCreated"
                                                    OnRowDataBound="gvwProjects_RowDataBound" CellPadding="3" OnRowCommand="gvwProjects_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField DataField="vWorkspaceID" HeaderText="WorkspaceID" SortExpression="vWorkspaceID">
                                                            <HeaderStyle Font-Underline="False" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Project">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProjectName" runat="server" Text='<%# Eval("vWorkspaceDesc") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vRequestId" HeaderText="Requeset ID">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vProjectNo" HeaderText="Project No.">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vClientName" HeaderText="Sponsor" SortExpression="vClientName">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vDrugName" HeaderText="Drug" SortExpression="vDrugName">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iNoOfSubjects" HeaderText="# Subjects" SortExpression="iNoOfSubjects">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vProjectManager" HeaderText="Project Manager" SortExpression="vBrandName">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vProjectCoordinator" HeaderText="Co-Ordinator" SortExpression="vProjectCoordinator">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vProjectTypeName" HeaderText="Project Type" SortExpression="vProjectTypeName">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vRegionName" HeaderText="Submission" SortExpression="vRegionName">
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cProjectStatusDesc" HeaderText="Status" SortExpression="cProjectStatusDesc">
                                                            <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vClientCode" HeaderText="Sponsor Code" />
                                                        <asp:BoundField DataField="vDrugCode" HeaderText="Drug Code" />
                                                        <asp:BoundField DataField="vLocationCode" HeaderText="LocationCode" />
                                                        <asp:BoundField DataField="vProjectTypeCode" HeaderText="ProjectTypeCode" />
                                                        <asp:TemplateField HeaderText="Details">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkBtnProDet" runat="server" ForeColor="Navy" Text="Project Details" ToolTip="Project Details" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Copy Project">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgCopy" runat="server" ToolTip="Copy" OnClientClick="ShowHideDivCopy('Y');"
                                                                    ImageUrl="~/images/copy.png" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>

                                                <div id="createtable" align="center" style="width: 100%">
                                                </div>

                                                <asp:Button ID="btnEdit" runat="server" Text="Edit" ToolTip="Edit" Style="display: none" />
                                                <asp:Button ID="btnLink" runat="server" Text="Edit" ToolTip="Edit" Style="display: none" />
                                                <asp:HiddenField ID="hdnEditedId" runat="server" />
                                                <asp:HiddenField ID="hdnlinkedId" runat="server" />

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel ID="upnlDivCopy" runat="server">
                                            <ContentTemplate>
                                                <div style="padding-right: 10px; display: none; padding-left: 10px; left: 324px; padding-bottom: 10px; padding-top: 10px; top: 128px; text-align: left"
                                                    id="divCopy"
                                                    class="DIVSTYLE2" runat="server">
                                                    <table cellpadding="3" width="100%">
                                                        <tbody>
                                                            <tr runat="server" id="trDrug">
                                                                <td class="Label" style="width: 40%; text-align: right;">Drug*:
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:DropDownList ID="SlcDrug" runat="server" CssClass="dropDownList" Width="65%">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="trSponsor" style="text-align: right;">
                                                                <td class="Label">Sponsor*:
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:DropDownList ID="SlcSponsor" runat="server" CssClass="dropDownList" Width="65%">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="trProjectName">
                                                                <td class="Label" style="text-align: right; width: 40%;">Project Name*:
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:TextBox ID="txtProjectName" runat="server" CssClass="textBox" Width="65%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="Label" style="text-align: right;">Select Template *:
                                                                </td>
                                                                <td style="text-align: left">
                                                                    <asp:DropDownList ID="SlcTemplate" runat="server" CssClass="dropDownList" Width="65%">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="Label" style="text-align: right;">With Authorized Documents*:
                                                                </td>
                                                                <td style="text-align: left;">
                                                                    <asp:RadioButtonList ID="RBLDoc" runat="server" CssClass="Label" RepeatDirection="Horizontal">
                                                                        <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="N">No</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: center" width="100%" colspan="2">
                                                                    <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text=" Save " ToolTip="Save" CssClass="btn btnsave" />
                                                                    <asp:Button ID="btnClose" runat="server" Text=" Cancel" ToolTip="Cancel" CssClass="btn btnclose" />

                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <asp:HiddenField ID="HFShow" runat="server" Value="N" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="gvwProjects" EventName="RowCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id='loadingmessage' style='display: none'>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            debugger;
            fnGetDataforCopyProject();
            return false;
        });
        function UIgvwProjects() {
            $('#<%= gvwProjects.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvwProjects.ClientID%>').prepend($('<thead>').append($('#<%= gvwProjects.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }
        function ShowPopup(dvName) {

            var dv = document.getElementById(dvName);
            dv.innerHTML = "TESTING";
            return true;
        }

        function ShowHideDivCopy(Show) {
            var Showdv = document.getElementById('<%= divCopy.clientId %>');

            if (Show == 'Y') {
                //alert('Hello');
                Showdv.style.display = 'block';
                SetCenter(Showdv);
                //document.getElementById('<%= HFShow.clientId %>').value = "Y";                                 
            }
            else {
                Showdv.style.display = 'none';
                //document.getElementById('<%= HFShow.clientId %>').value = "N";          
            }

        }

        function SetCenter(dv) {
            //var dv = document.getElementById(dvName);
            var winScroll = BodyScrollHeight();
            var updateProgressDivBounds = Sys.UI.DomElement.getBounds(dv);
            var winBounds = GetWindowBounds();

            var x = Math.round(winBounds.Width / 2) - Math.round(updateProgressDivBounds.width / 2);
            var y = Math.round(winBounds.Height / 2) - Math.round(updateProgressDivBounds.height / 2);

            x += winScroll.xScr;
            y += winScroll.yScr;

            Sys.UI.DomElement.setLocation(dv, parseInt(x), parseInt(y));


        }

        function OpenWindow(Path) {
            window.open(Path);
            return false;
        }

        var summarydata = '';
        function fnGetDataforCopyProject() {

            debugger;
            $('#loadingmessage').show();

            $.ajax({
                type: "post",

                url: "frmCopyProject.aspx/View_MyProjects",
                //data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","vType":"' + vType + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                // async: false,
                dataType: "json",
                success: function (data) {
                    var data = data.d;
                    var msgs = JSON.parse(data);
                    summarydata = msgs;
                    if (summarydata == "") return false;
                    // Table = Object(keys(summarydata))[0];
                    CreateSummaryTable(summarydata);
                    $('#loadingmessage').hide();
                    return false;
                },
                failure: function (response) {
                    alert("failure");
                    alert(data.d);
                },
                error: function (response) {
                    alert("error");
                }
            });
            return false;

        }
        function CreateSummaryTable(summarydata) {

            var ActivityDataset = [];
            var jsondata = summarydata.VIEW_MYPROJECTS;
            for (var Row = 0; Row < jsondata.length; Row++) {
                var InDataset = [];
                InDataset.push(Row + 1, jsondata[Row]['vRequestId'], jsondata[Row]['vProjectNo'], jsondata[Row]['vClientName'], jsondata[Row]['vDrugName'], jsondata[Row]['iNoOfSubjects'], jsondata[Row]['vProjectManager'], jsondata[Row]['vProjectCoordinator'], jsondata[Row]['vProjectTypeName'], jsondata[Row]['vRegionName'], jsondata[Row]['cProjectStatusDesc'], "", "", jsondata[Row]['vWorkspaceId']);
                ActivityDataset.push(InDataset);

            }

            $ = jQuery;
            var createtable1 = $("<table id='Activityrecord'  border='1' align='center'  class='display'  cellspacing='0'> </table>");
            $("#createtable").empty().append(createtable1);

            $('#Activityrecord').DataTable({
                "bJQueryUI": true,
               // "sScrollY": "285px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "aaData": ActivityDataset,
                "fnCreatedRow": function (nRow, aData, iDataIndex) {
                    //$('td:eq(15)', nRow).append("<['vActivityName'] = '" + aData[0] + "', ['vUserTypeName'] = '" + aData[1] + "', ['vDeptName'] = '" + aData[2] + "', ['vLocationName'] = '" + aData[3] + "', ['vOperationName'] = '" + aData[4] + "'>");
                    $('td:eq(11)', nRow).append("<a href='#'     tempworkspaceid='" + aData[13] + "';  OnClick='ViewDetails(this); return false;'  >Project Details</a>");
                    $('td:eq(12)', nRow).append("<input type='image' id='imgcopy" + iDataIndex + "' name='imgcopy$" + iDataIndex + "' src='images/copy.png';  tempworkspaceid='" + aData[13] + "'; OnClick='ViewProject(this); return false;'   style='border-width:0px;' >");
                    // $('td:eq(16)', nRow).append("<input type='image' id='imgexel" + iDataIndex + "' name='imgexel$" + iDataIndex + "' src='images/Export.gif'; tempval2='" + aData[17] + "';   OnClick='ExportToExcel(this); return false;' style='border-width:0px;' >");
                },
                //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
                //    $("td:first", nRow).html(iDisplayIndex + 1);
                //    return nRow;
                //},
                "aoColumns": [

                                             { "sTitle": "#" },
                                              { "sTitle": "Requeset ID" },
                                              { "sTitle": "Project No." },
                                            { "sTitle": "Sponsor" },
                                              { "sTitle": "Drug" },
                                              { "sTitle": "# Subjects" },
                                              { "sTitle": "Project Manager" },
                                                { "sTitle": "Co-Ordinator" },

                                              { "sTitle": "Project Type" },

                                              { "sTitle": "Submission" },
                                              { "sTitle": "Status" },
                                                 { "sTitle": "Details" },
                                                    { "sTitle": "Copy Project" }





                ],

                "columns": [
                    null, null, null, null, null, null, null, null, null, null, null, null, null
                ],
                "oLanguage": {
                    "sEmptyTable": "No Record Found"
                },
            });

            $('#Activityrecord').show();



        }

        function ViewProject(e) {

            var id = e.attributes.tempworkspaceid.value;

            $('#ctl00_CPHLAMBDA_hdnEditedId').val(id);


            $('#ctl00_CPHLAMBDA_btnEdit').click();

        }

        function ViewDetails(e) {

            var id = e.attributes.tempworkspaceid.value;

            $('#ctl00_CPHLAMBDA_hdnlinkedId').val(id);


            $('#ctl00_CPHLAMBDA_btnLink').click();

        }
    </script>

</asp:Content>
