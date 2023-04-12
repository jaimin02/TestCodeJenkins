<%@ page language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="frmMedExTemplateMst, App_Web_vq2225em" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <link rel="stylesheet" type="text/css" href="App_Themes/ECTD.css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/smoothnessjquery-ui.css" />

    <script type="text/javascript" src="Script/jquery-1.9.1.js"></script>

    <script type="text/javascript" src="Script/jquery-ui-1.10.2.custom.min.js"></script>

    <!-- <script type="text/javascript" src="Script/jquery-ui.js"></script>  -->
    
    <script type="text/javascript"   src="Script/JQuery-ui-1.11.4.js"></script>

    <script type="text/javascript" src="Script/slimScroll.min.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>

    <style type="text/css">
        .quad li
        {
            width: 25%;
        }
        #ctl00_CPHLAMBDA_TempMedex li
        {
            width: 25%;
        }
        #SeqMedex
        {
            width: 100%;
            margin-bottom: 20px;
            overflow: hidden;
            margin-left: 4%;
        }
        #TempMedex
        {
            width: 100%;
            margin-bottom: 20px;
            overflow: hidden;
        }
        .allmed
        {
            line-height: 1.5em;
            border-bottom: 1px solid #ccc;
            float: left;
            display: inline;
            border: 1px solid #d3d3d3;
            background: -moz-linear-gradient(top,  rgba(247,247,247,0.73) 0%, rgba(206,227,237,1) 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(247,247,247,0.73)), color-stop(100%,rgba(206,227,237,1))); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top,  rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top,  rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top,  rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* IE10+ */
            background: linear-gradient(to bottom,  rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=                                  '#baf7f7f7' , endColorstr= '#cee3ed' ,GradientType=0 ); /* IE6-9 */
            font-weight: normal;
            color: #555555;
        }
        .allmed:hover
        {
            background: rgb(30,87,153); /* Old browsers */
            background: -moz-linear-gradient(top, rgba(30,87,153,1) 0%, rgba(41,137,216,1) 50%, rgba(32,124,202,1) 100%, rgba(125,185,232,1) 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(30,87,153,1)), color-stop(50%,rgba(41,137,216,1)), color-stop(100%,rgba(32,124,202,1)), color-stop(100%,rgba(125,185,232,1))); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* IE10+ */
            background: linear-gradient(to bottom, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=                                                                                   '#1e5799' , endColorstr= '#7db9e8' ,GradientType=0 ); /* IE6-9 */
            color: White !important;
        }
        .Savemed
        {
            line-height: 1.5em;
            border-bottom: 1px solid #ccc;
            float: left;
            display: inline;
            border: 1px solid #d3d3d3;
            background: -moz-linear-gradient(top,  rgba(247,247,247,0.73) 0%, rgba(206,227,237,1) 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(247,247,247,0.73)), color-stop(100%,rgba(206,227,237,1))); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top,  rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top,  rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top,  rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* IE10+ */
            background: linear-gradient(to bottom,  rgba(247,247,247,0.73) 0%,rgba(206,227,237,1) 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=                                  '#baf7f7f7' , endColorstr= '#cee3ed' ,GradientType=0 ); /* IE6-9 */
            font-weight: normal;
            color: #555555;
        }
        .Savemed:hover
        {
            background: rgb(30,87,153); /* Old browsers */
            background: -moz-linear-gradient(top, rgba(30,87,153,1) 0%, rgba(41,137,216,1) 50%, rgba(32,124,202,1) 100%, rgba(125,185,232,1) 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(30,87,153,1)), color-stop(50%,rgba(41,137,216,1)), color-stop(100%,rgba(32,124,202,1)), color-stop(100%,rgba(125,185,232,1))); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* IE10+ */
            background: linear-gradient(to bottom, rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 100%,rgba(125,185,232,1) 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=                                                                                   '#1e5799' , endColorstr= '#7db9e8' ,GradientType=0 ); /* IE6-9 */
            color: White !important;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="HdnNewAddedMedex" runat="server" />
            <asp:HiddenField ID="HdnExistMedex" runat="server" />
            <asp:HiddenField ID="HdnBlankMedex" runat="server" />
            <asp:HiddenField ID="HdnSequenceMedex" runat="server" />
            <asp:HiddenField ID="HdnExistMedexRemoved" runat="server" />
            <asp:HiddenField ID="HdnRemoveIndicator" runat="server" />
            <asp:Panel ID="pheader" runat="server">
                <div id="divExpandable" runat="server" style="font-weight: bold; font-size: 13px;
                    float: none; color: white; background-color: #1560a1; width: 90%; margin: auto;">
                    <div>
                        <asp:Image ID="imgArrows" runat="server" Style="float: right;" />
                    </div>
                    <div style="text-align: left; margin-left: 3%; height:20px;">
                        <asp:Label ID="Label1" runat="server" Text="Add/Edit Template">
                        </asp:Label></div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnltable" runat="server">
                <table style="width: 90%; text-align: center; border: 1px solid black; margin: auto;
                    padding: 1%;" cellpadding="5px">
                    <tbody>
                        <tr>
                            <td style="width: 35%; text-align: right; color: Black;">
                                Project Type* :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlProjectType" runat="server" Width="45%" AutoPostBack="true"
                                    TabIndex="1" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; color: Black;">
                                Template*:
                            </td>
                            <td style="text-align: left;">
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                    CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                    OnClientShowing="ClientPopulated" ServiceMethod="GetTemplateCompletionList" ServicePath="AutoComplete.asmx"
                                    TargetControlID="txtTemplate" UseContextKey="True" CompletionListElementID="pnlTemplate">
                                </cc1:AutoCompleteExtender>
                                <asp:Panel ID="pnlTemplate" runat="server" Style="max-height: 200px; overflow: auto;
                                    overflow-x: hidden;" />
                                <asp:HiddenField ID="HTemplateId" runat="server" />
                                <asp:TextBox ID="txtTemplate" TabIndex="2" runat="server" CssClass="textBox" Width="44%" />
                                <asp:Button Style="display: none" ID="btnedit" runat="server" Text="Edit" CssClass="btn btnedit" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; color: Black;">
                                Attribute Group :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlMedExGroup" runat="server" Width="45%" AutoPostBack="True"
                                    TabIndex="3" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; color: Black;">
                                Attribute Sub Group :
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlMedExSubGroup" runat="server" Width="45%" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlMedExSubGroup_SelectedIndexChanged" TabIndex="4" />
                                <asp:CheckBox ID="chkAllSubGroups" runat="server" Text="All" AutoPostBack="True"
                                    OnCheckedChanged="chkAllSubGroups_CheckedChanged" TabIndex="5" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="2">
                                <asp:Button ID="btnsave" runat="server" Text="Save" ToolTip="Save" CssClass="btn btnsave"
                                    Visible="false" OnClientClick="return Validation();" TabIndex="6" />
                                <asp:Button ID="btnupdate" OnClick="btnupdate_Click" runat="server" Text="Update"
                                    ToolTip="Update" CssClass="btn btnupdate" Visible="False" OnClientClick="return Validation();"
                                    TabIndex="7" />
                                <asp:Button ID="btncancel" OnClick="btncancel_Click" runat="server" Text="Cancel"
                                    ToolTip="Cancel" CssClass="btn btncancel" TabIndex="8" />
                                <asp:Button ID="btnExit" OnClick="btnExit_Click" TabIndex="9" runat="server" Text="Exit"
                                    ToolTip=" Exit" CssClass="btn btnexit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="Collpase1" runat="server" TargetControlID="pnltable"
                ExpandControlID="pheader" CollapseControlID="pheader" ExpandedImage="images/panelcollapse.png"
                CollapsedImage="images/panelexpand.png" ImageControlID="imgArrows" AutoCollapse="false"
                AutoExpand="false" ExpandDirection="Vertical">
            </cc1:CollapsiblePanelExtender>
            <asp:Panel ID="pheader2" runat="server" Style="margin-top: 2%; display: none;">
                <div id="divExpandable2" runat="server" style="font-weight: bold; font-size: 13px;
                    float: none; color: white; background-color: #1560a1; width: 90%; margin: auto;">
                    <div>
                        <asp:Image ID="imgArrows2" runat="server" Style="float: right;" />
                    </div>
                    <div style="text-align: left; margin-left: 3%;">
                        <asp:Label ID="Label2" runat="server" Text="Add/Order Attributes">
                        </asp:Label></div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnltable2" runat="server" Style="display: none;">
                <table width="90%" style="margin: auto; padding: 1%; border: 1px solid; border-color: Black;">
                    <tr>
                        <td style="text-align: left; width: 40%;">
                            <asp:Label ID="lblSeq" runat="server" Style="color: black; font-weight: bold; font-size: 12.5px;"></asp:Label>
                            <%--<asp:Panel ID="pnlorg" runat="server" Style="max-height: 350px; width: 100%; min-height: 100px;
                                overflow: auto; overflow-x: hidden;" BorderWidth="1px">--%>
                            <ul id="SeqMedex" runat="server" style="list-style-type: none !important; padding: 0%;
                                padding-top: 2%; width: 100%; max-height: 350px; min-height: 100px; width: 100%;
                                overflow: auto; overflow-x: hidden; float: right; border: 1px solid black;">
                            </ul>
                            <%--  </asp:Panel>--%>
                        </td>
                        <td style="text-align: center;">
                            <asp:Image ID="imgLeft" ImageUrl="images/MedexLeft.png" runat="server" Style="vertical-align: middle" />
                            <asp:Image ID="imgright" ImageUrl="images/MedexRight%20.png" runat="server" Style="vertical-align: middle" />
                        </td>
                        <td style="text-align: left; width: 40%;">
                            <asp:Label ID="lblTemp" runat="server" Style="color: black; font-weight: bold; font-size: 12.5px;"></asp:Label>
                            <%--<asp:Panel ID="Pnldup" runat="server" Style="" BorderWidth="1px">--%>
                            <ul id="TempMedex" runat="server" style="list-style-type: none !important; padding: 10px;
                                width: 100%; margin-left: 4%; max-height: 350px; min-height: 100px; width: 100%;
                                overflow: auto; overflow-x: hidden; float: right; border: 1px solid black;">
                            </ul>
                            <%--</asp:Panel>--%>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="Collpase2" runat="server" TargetControlID="pnltable2"
                ExpandControlID="pheader2" CollapseControlID="pheader2" ExpandedImage="images/panelcollapse.png"
                CollapsedImage="images/panelexpand.png" ImageControlID="imgArrows2" AutoCollapse="false"
                AutoExpand="false" ExpandDirection="Vertical">
            </cc1:CollapsiblePanelExtender>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlMedExGroup" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
var isInside = true  ;
        function pageLoad() {

            // Anand
            if ($("#li-id li").size() > 1) {
                $("#ol-id ol").sortable({
                    revert: true,
                    axis: 'y',
                    containment: 'parent',
                    cursor: 'move',
                    handle: 'div.link_div',
                    smooth: false,
                    opacity: 0.7,
                    tolerance: 'pointer',
                    start: function () {
                        $("#ol-id").removeClass("bottom_dragged");
                    },
                    update: function () {
                        $("#ol-id ol").sortable({ disabled: true });
                        $("#saving_indicator").html("saving...")
                        $("#saving_indicator").show();
                        //do other stuff...
                    }
                })
            }
            // Anand

            $("#ctl00_CPHLAMBDA_SeqMedex").sortable({
                connectWith: ['#ctl00_CPHLAMBDA_TempMedex']
            });

            $("#ctl00_CPHLAMBDA_TempMedex").sortable({
                connectWith: ['#ctl00_CPHLAMBDA_SeqMedex'],
                receive: function (event, ui) {
                    
                    //alert('aa');
                    //var itemText= ui.item.text();
                    var Lidata = ui.item.attr('id').split('_')
                    if (Lidata.length == 3) {
                        var medexcode = ui.item.attr('id').replace("ctl00_CPHLAMBDA_", "");
                    }
                    else if (Lidata.length == 4) {
                        var medexcode = ui.item.attr('id').replace("ctl00_CPHLAMBDA_mev_", "");
                    }
                    //alert (ui.item.attr('id'));
                    //alert (medexcode);
                    var formulaData = $('#<%= HdnBlankMedex.ClientId %>').val().split(',');
                    var match = jQuery.inArray(medexcode, formulaData);
                    if (match >= 0) {
                        msgalert('You cannot Add blank  Formula !');
                        $('#ctl00_CPHLAMBDA_SeqMedex').append(ui.item);
                    }
                    else {
                        if ($('#<%= HdnExistMedex.ClientId %>').val() == "") {

                            $('#<%= HdnExistMedex.ClientId %>').val(medexcode + ",");
                        }
                        else {
                            var Data = $('#<%= HdnExistMedex.ClientId %>').val().split(',');
                            var found = jQuery.inArray(medexcode, Data);
                            if (found >= 0) {
                                //alert('The Drraged Attribute Already Exists');
                                Lititle = ui.item.attr('title');
                                LiId = ui.item.attr('id')
                                Lihtml = ui.item.html();
                                //alert (Lihtml);
                                ui.item.remove();
                                $('#ctl00_CPHLAMBDA_SeqMedex').append('<li id=" ' + LiId + '" class="allmed" style="text-align:left;margin-bottom:1%;margin-left:2%;height:40px;border-radius:7px 7px 7px 7px;" title="' + Lititle + '">' + Lihtml + '</li>');

                                // Data.splice(found, 1);
                            }
                            else {

                                Data.push(medexcode);
                                // datanew.push(medexcode);
                            }
                            var blkstr = $.map(Data, function (val, index) {
                                return val;
                            }).join(",");

                            $('#<%= HdnExistMedex.ClientId %>').val(blkstr.replace(',,', ','));

                        }

                        if ($('#<%= HdnNewAddedMedex.ClientId %>').val() == "") {
                            ui.item.removeClass();
                            ui.item.addClass('Savemed');
                            $('#<%= HdnNewAddedMedex.ClientId %>').val(medexcode + ",");
                        }
                        else {
                            var datanew = $('#<%= HdnNewAddedMedex.ClientId %>').val().split(',');
                            var getmedex = jQuery.inArray(medexcode, datanew);
                            if (getmedex >= 0) {
                                //alert ('Removing');    
                                datanew.splice(getmedex, 1);
                            }
                            else {
                                ui.item.removeClass();
                                ui.item.addClass('Savemed');
                                datanew.push(medexcode);
                            }
                            var blkstrex = $.map(datanew, function (val, index) {
                                return val;
                            }).join(",");
                            $('#<%= HdnNewAddedMedex.ClientId %>').val(blkstrex.replace(',,', ','));
                        }

                    }


                },
                remove: function (event, ui) {
                    var Lidata = ui.item.attr('id').split('_')
                    if (Lidata.length == 3) {
                        var medexcode = ui.item.attr('id').replace("ctl00_CPHLAMBDA_", "");
                    }
                    else if (Lidata.length == 4) {
                        var medexcode = ui.item.attr('id').replace("ctl00_CPHLAMBDA_mev_", "");
                    }

                    //alert (ui.item.attr('id'));
                    //alert(medexcode);
                    if ($('#<%= HdnNewAddedMedex.ClientId %>').val() != "") {
                        var datanew = $('#<%= HdnNewAddedMedex.ClientId %>').val().split(',');
                        var getmedex = jQuery.inArray(medexcode, datanew);

                        if (getmedex >= 0) {
                            //    alert ('deleted');
                            datanew.splice(getmedex, 1);
                        }
                        var blkstrex = $.map(datanew, function (val, index) {
                            return val;
                        }).join(",");
                        $('#<%= HdnNewAddedMedex.ClientId %>').val(blkstrex.replace(',,', ','));
                    }
                    if ($('#<%= HdnExistMedex.ClientId %>').val() != "") {
                        var Data = $('#<%= HdnExistMedex.ClientId %>').val().split(',');
                        var found = jQuery.inArray(medexcode, Data);

                        if (found >= 0) {

                            //       alert(' Drraged  Exists removed');
                            Data.splice(found, 1);
                        }

                        var blkstr = $.map(Data, function (val, index) {
                            return val;
                        }).join(",");

                        $('#<%= HdnExistMedex.ClientId %>').val(blkstr.replace(',,', ','));
                    }
                    if ($('#<%= HdnExistMedexRemoved.ClientId %>').val() == "") {
                        // alert (medexcode)

                        $('#<%= HdnExistMedexRemoved.ClientId %>').val(medexcode + ",");
                    }
                    else {
                        var Data = $('#<%= HdnExistMedexRemoved.ClientId %>').val().split(',');
                        var found = jQuery.inArray(medexcode, Data);
                        if (found >= 0) {
                            msgalert('The Drraged Attribute Already Exists !');
                            return false ;
                            // Data.splice(found, 1);
                        }
                        else {
                            Data.push(medexcode);
                            // datanew.push(medexcode);
                        }
                        var blkstr = $.map(Data, function (val, index) {
                            return val;
                        }).join(",");

                        $('#<%= HdnExistMedexRemoved.ClientId %>').val(blkstr.replace(',,', ','));

                    }
                    
                    $('#<%= SeqMedex.ClientId %> .Savemed').remove();
                    $('#<%= HdnRemoveIndicator.ClientId %>').val('Active');

                },

                deactivate: function (event, ui) {

                    getsequence();
                },
                
               stop: function( event, ui ) {
                     //alert (isInside);
                 if ( isInside == false )
                   {
                       if ($('#<%= HdnRemoveIndicator.ClientId %>').val() != 'Active')
                          {
                            $(this).sortable( "cancel" );
                          }
                       else 
                          {
                            $('#<%= HdnRemoveIndicator.ClientId %>').val("");
                          }   
                   }
//                  if ($('#<%= HdnRemoveIndicator.ClientId %>').val() == 'Active')
//                   {
//                      $('#<%= HdnRemoveIndicator.ClientId %>').val("");
//                      
//                   }
              }
            }).droppable({
                    over: function() { isInside = true; },
                    out: function() { isInside = false; }
                    
            });
        }




        function ClientPopulated(sender, e) {
            MedexTemplateClientShowing('AutoCompleteExtender1', $get('<%= txtTemplate.ClientId %>'));
        }

        function OnSelected(sender, e) {
            MedexTemplateOnItemSelected(e.get_value(), $get('<%= txtTemplate.clientid %>'),
                    $get('<%= HTemplateId.clientid %>'), $get('<%= btnedit.clientid %>'));

        }

        function Validation() {

            var ddlProjectType = document.getElementById('<%= ddlProjectType.ClientId %>');
            var txtBox = document.getElementById('<%= txtTemplate.ClientId %>');
            var ddlMedExGroup = document.getElementById('<%= ddlMedExGroup.ClientId %>');
            var ddlMedExSubGroup = document.getElementById('<%= ddlMedExSubGroup.ClientId %>');

            if (ddlProjectType.selectedIndex == 0) {
                msgalert('Please Select Project Type !');
                ddlProjectType.focus();
                return false;
            }
            else if (txtBox.value.toString().trim().length <= 0) {
                msgalert('Please Enter Template Name !');
                txtBox.focus();
                txtBox.value = '';
                return false;
            }
            else if (ddlMedExGroup.selectedIndex == 0 && $('#ctl00_CPHLAMBDA_btnupdate').val() != "Update") {
                msgalert('Please Select Attribute Group !');
                ddlMedExGroup.focus();
                return false;
            }
            else if (ddlMedExSubGroup.selectedIndex == 0 && $('#ctl00_CPHLAMBDA_btnupdate').val() != "Update") {
                msgalert('Please Select Attribute Sub Group !');
                ddlMedExSubGroup.focus();
                return false;
            }
            getsequence();
            return true;
        }

        function getsequence() {
           
            var i = 1;
            var sequencedata = [];
            $('.Savemed').each(function () {
               
                var contentmedex = new Object();
                var Lidata = $(this).attr('id').split('_')
                if (Lidata.length == 3) {
                    var medexcode = $(this).attr('id').replace("ctl00_CPHLAMBDA_", "");
                }
                else if (Lidata.length == 4) {
                    var medexcode = $(this).attr('id').replace("ctl00_CPHLAMBDA_mev_", "");
                }
                //var medexcode =  $(this).attr('id').replace("ctl00_CPHLAMBDA_","");
                contentmedex.vMedExCode = medexcode;
                contentmedex.iSeqNo = i;
                sequencedata.push(contentmedex);
                i = parseInt(i) + 1;
            });
            $('#<%= HdnSequenceMedex.ClientId %>').val(JSON.stringify(sequencedata));
            // alert ('got it');
            return true;
        }
       
   
    </script>

</asp:Content>
