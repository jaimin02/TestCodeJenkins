<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false"
    CodeFile="frmAttributeSearch.aspx.vb" Inherits="frmAttributeSearch"  %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        #ctl00_CPHLAMBDA_gvPDFContentSearch th
        {
            /*background: url(    'images/cell-grey.jpg' );*/ /*color: Black;*/
            padding: 8px;
        }
        #ctl00_CPHLAMBDA_gvPDFContentSearch td
        {
            /*background: url(    'images/cell-blue.jpg' );*/
            padding: 4px;
        }
        #fsSearchCriteria
        {
            border-bottom-color: Black;
            border-width: thick;
        }
    </style>
    <div id="divMain" style="width: 100%">
        <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table style="width: 100%">
                    <%--     <asp:UpdatePanel ID ="update" runat="server" UpdateMode ="Conditional"> 
                    <ContentTemplate>--%>
                    <tr>
                        <td style="top: 5px; vertical-align: top; overflow: auto;" align="center">
                            <fieldset class="detailsbox" id="fldDocTypeSelection" style="width: 90%">
                                <legend class="LabelBold" style="color: Black">&nbsp;&nbsp;Select Document Type&nbsp;&nbsp;
                                </legend>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlDocType" runat="server" CssClass="dropDownList" Style="width: 50%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="top: 5px; vertical-align: top; overflow: auto;" align="center">
                            <fieldset class="detailsbox" id="fldClientSelection" style="width: 90%">
                                <legend class="LabelBold" style="color: Black">&nbsp;&nbsp;Select Client & Molecules&nbsp;&nbsp;
                                </legend>
                                <table style="width: 100%">
                                    <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top; width: 50%;">
                                            <strong>
                                                <asp:CheckBox ID="chkAnyClient" runat="server" Text="Any Client" TextAlign="Right"
                                                    Style="padding-left: 4px;" /></strong>
                                            <div style="border-bottom: gray thin solid; text-align: left; border-left: gray thin solid;
                                                border-right: gray thin solid; border-top: gray thin solid; width: 95%; height: 100px;
                                                overflow: auto; margin-top: 6px;" id="divClient">
                                                <asp:CheckBoxList ID="chkclient" runat="server" Font-Size="Small" Font-Names="verdana"
                                                    CssClass="checkboxlist">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                        <td style="text-align: left; padding-left: 10px; padding-bottom: 10px">
                                            <strong>
                                                <asp:CheckBox ID="chkAnyMolecule" runat="server" Text="Any Molecule" TextAlign="Right"
                                                    Style="padding-left: 4px;" /></strong>
                                            <div style="border-bottom: gray thin solid; text-align: left; border-left: gray thin solid;
                                                border-right: gray thin solid; border-top: gray thin solid; width: 95%; height: 100px;
                                                overflow: auto; margin-top: 6px;" id="divMolecule">
                                                <asp:CheckBoxList ID="chkmolecule" runat="server" Font-Size="Small" Font-Names="verdana">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="top: 5px; vertical-align: top; overflow: auto;" align="center">
                            <fieldset class="detailsbox" id="fldProjectTypeSelection" style="width: 90%">
                                <legend class="LabelBold" style="color: Black">&nbsp;&nbsp;Select Project Type & Location&nbsp;&nbsp;
                                </legend>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: left; vertical-align: top; width: 50%;">
                                            <strong>
                                                <asp:CheckBox ID="chkAnyProjectType" runat="server" Text="Any Project Type" TextAlign="Right"
                                                    Style="padding-left: 4px;" /></strong>
                                            <div style="border-bottom: gray thin solid; text-align: left; border-left: gray thin solid;
                                                border-right: gray thin solid; border-top: gray thin solid; width: 95%; height: 100px;
                                                overflow: auto; margin-top: 6px;" id="divProjectType">
                                                <asp:CheckBoxList ID="chkprojecttype" runat="server" Font-Size="Small" Font-Names="verdana">
                                                    <asp:ListItem Text="BA-BE"></asp:ListItem>
                                                    <asp:ListItem Text="DMS"></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                        <td style="text-align: left; padding-left: 10px; padding-bottom: 10px">
                                            <strong>
                                                <asp:CheckBox ID="chkAnyLocation" runat="server" Text="Any Location" TextAlign="Right"
                                                    Style="padding-left: 4px;" /></strong>
                                            <div style="border-bottom: gray thin solid; text-align: left; border-left: gray thin solid;
                                                border-right: gray thin solid; border-top: gray thin solid; width: 95%; height: 100px;
                                                overflow: auto; margin-top: 6px" id="divLocation">
                                                <asp:CheckBoxList ID="chklocation" runat="server" Font-Size="Small" Font-Names="verdana">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="top: 5px; vertical-align: top; overflow: auto;" align="center">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <fieldset class="detailsbox" id="fldPeriodSelection" style="width: 90%">
                                        <legend class="LabelBold" style="color: Black">&nbsp;&nbsp;Period Selection&nbsp;&nbsp;
                                        </legend>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: right; padding-bottom: 10px; width: 50%; vertical-align: top;">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                Report For :
                                                            </td>
                                                            <td align="left" style="padding-left: 5px">
                                                                <asp:DropDownList ID="ddlReportFor" runat="server" CssClass="dropDownList" Width="260px"
                                                                    AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="text-align: left; padding-bottom: 10px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="text-align: left; padding-left: 5px; width: 15%">
                                                                From :
                                                            </td>
                                                            <td style="padding-left: 5px">
                                                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="textBox" ReadOnly="true" Width="120px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calFromDate" runat="server" TargetControlID="txtFromDate"
                                                                    Format="dd-MMM-yyyy">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding-left: 5px; width: 15%">
                                                                To :
                                                            </td>
                                                            <td align="left" style="padding-left: 5px">
                                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="textBox" ReadOnly="true" Width="120px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calToDate" runat="server" TargetControlID="txtToDate" Format="dd-MMM-yyyy">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlReportFor" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="txtFromDate" EventName="" />
                                    <asp:AsyncPostBackTrigger ControlID="txtToDate" EventName="" />
                                    <%--<asp:AsyncPostBackTrigger ControlID ="btnsearch" EventName ="" />--%>
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnrefresh" runat="server" Text="Refresh Above Selection" 
                                Style="font-weight: bold;" OnClientClick="return RefreshClientAndMolecules();" class="btn btnnew" />
                            <asp:Button ID="btnsearch" runat="server" Text="Search"  Style="font-weight: bold;"
                                OnClientClick="checkClientAndMolecules();" class="btn btnnew" />
                            <asp:Button ID="btnExit" runat="server" Text="Exit"  Style="font-weight: bold;" class="btn btnexit" />
                        </td>
                    </tr>
                    <%--QUOTATION SUMMARY--%>
                    <tr id="trQuatation" runat="server">
                        <td style="top: 5px; vertical-align: top; overflow: auto;" align="center">
                            <div style="width: 90%; text-align: right">
                                <asp:ImageButton ID="ImgExport" AlternateText="Image Not Found" ToolTip="Export To Excel"
                                    runat="server" ImageUrl="~/images/Export.png" Height="18px" Width="18px" OnClientClick ="return EmptyQuatationGrid(this);" />
                            </div>
                            <fieldset class="detailsbox" id="fldgvQuation" style="width: 98%">
                                <legend class="LabelBold" style="color: Black">
                                    <%--<img id="ImgexpColAttr" class="flip" alt="Image Not Found" src="images/collapse.jpg" runat="server" onmouseover="this.style.cursor='pointer';"
                                    onclick="showGvQuatation(this);" />--%>
                                    Quotation Summary&nbsp;&nbsp;</legend>
                                <div class="panel">
                                    <asp:GridView ID="gvQuatation" runat="server" AutoGenerateColumns="True" EmptyDataRowStyle-HorizontalAlign="Center"
                                        EmptyDataText="No Data Found" ShowFooter="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemStyle HorizontalAlign="Center" Width="25"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgEdit" AlternateText="Image Not Found" runat="server" ImageUrl="~/images/Edit2.gif"
                                                        Height="18px" Width="18px" OnClientClick =" return ShowEditPageinQuatation(this);"/>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Details">
                                                <ItemStyle HorizontalAlign="Center" Width="25"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgDetails" AlternateText="Image Not Found" runat="server" ImageUrl="~/images/information.png"
                                                        Height="18px" Width="18px" OnClientClick ="return ShowDetailPageinQuatation(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File Link">
                                                <ItemStyle HorizontalAlign="Center" Width="25"></ItemStyle>
                                                <ItemTemplate>
                                                    <a href='<%# Eval("Document") %>' target="_blank">Open File</a>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <%--PROJECT SYNOPSIS--%>
                    <tr id="trProject" runat="server">
                        <td style="top: 5px; vertical-align: top; overflow: auto;" align="center">
                            <div style="width: 90%; text-align: right">
                                <asp:ImageButton ID="ImgExoprtProject" AlternateText="Image Not Found" ToolTip="Export To Excel"
                                    runat="server" ImageUrl="~/images/Export.png" Height="18px" Width="18px" OnClientClick ="return EmptySynopsisGrid(this);"/>
                            </div>
                            <fieldset class="detailsbox" id="fldProject" style="width: 98%">
                                <legend class="LabelBold" style="color: Black">
                                    <%--<img id="Img1" class="flip"
                                    alt="Image Not Found" src="images/collapse.jpg" runat="server" onmouseover="this.style.cursor='pointer';"
                                    onclick="showGvProject(this);" />--%>Project Synopsis&nbsp;&nbsp;</legend>
                                <div class="panel1">
                                    <asp:GridView ID="GridView3" runat="server" SkinID="grdViewAutoSizeMax">
                                    </asp:GridView>
                                    <asp:GridView ID="GridView4" runat="server" SkinID="grdViewAutoSizeMax">
                                    </asp:GridView>
                                    <asp:GridView ID="gvProject" runat="server" AutoGenerateColumns="True" EmptyDataRowStyle-HorizontalAlign="Center"
                                        EmptyDataText="No Data Found" ShowFooter="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemStyle HorizontalAlign="Center" Width="25"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgEdit" AlternateText="Image Not Found" runat="server" ImageUrl="~/images/Edit2.gif"
                                                        Height="18px" Width="18px" OnClientClick ="return ShowEditPageinSynopsis(this);" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Details">
                                                <ItemStyle HorizontalAlign="Center" Width="25"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgDetails" AlternateText="Image Not Found" runat="server" ImageUrl="~/images/information.png"
                                                        Height="18px" Width="18px" OnClientClick ="return ShowDetailPageinSynopsis(this);"  />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <%--     </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID ="btnsearch" EventName =""/>
                    </Triggers>
                    </asp:UpdatePanel>--%>
                </table>
            </ContentTemplate>
            <Triggers>
                <%-- <asp:AsyncPostBackTrigger ControlID="ddlReportFor" EventName="SelectedIndexChanged" />--%>
                <asp:PostBackTrigger ControlID="ImgExport" />
                <asp:PostBackTrigger ControlID="ImgExoprtProject" />
               <%-- <asp:PostBackTrigger ControlID ="btnsearch" />--%>
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script src="Script/json2.js" type="text/javascript"></script>

    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>

    <script src="Script/FixedHeader.min.js" type="text/javascript"></script>

    <%--  javascript added by:vikas shah
    added on: 15th feb 2012
    description:to validate the form elements  --%>

    <script type="text/javascript">

        function showUserDefindeAttrPopup_Quotation(workspaceId, nodeid) {
            if (workspaceId != '' && nodeid != '') {
                
                localStorage.action = 'S';
                window.open('frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=' + workspaceId + '&ActivityId=2335' + '&NodeId=' + nodeid + '&PeriodId=1&SubjectId=0000&Type=BA-BE&ScreenNo=0000&MySubjectNo=0000&SubNo=', 'My Window', 'top=60,left=100,width=800,height=600,titlebar=no,toolbar=no,location=no, directories=no, status=no, menubar=no,resizable=no, copyhistory=no');
                onUpdated();
            }
        }

//        function OpenEditPage(workspaceId, nodeid) {
//            if (workspaceId != '' && nodeid != '') {
//                
//                //document.location.href = 'frmGlobalDocRepository.aspx?Type=r&Mode=2&workspaceId=' + workspaceId + '&nodeid=' + nodeid;
//                localStorage.action = 'S';
//                window.open('frmGlobalDocRepository.aspx?Type=r&Mode=2&workspaceId=' + workspaceId + '&nodeid=' + nodeid, 'Edit Window', 'top=60,left=100,width=800,height=600,titlebar=no,toolbar=no,location=no, directories=no, status=no, menubar=no,resizable=no, copyhistory=no');
//                onUpdated();
//            }
//        }

//        function showUserDefindeAttrPopup_Synopsis(workspaceId, nodeid) {
//            if (workspaceId != '' && nodeid != '') {
//                localStorage.action = 'S';
//                window.open('frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=' + workspaceId + '&ActivityId=2336' + '&NodeId=' + nodeid + '&PeriodId=1&SubjectId=0000&Type=BA-BE&ScreenNo=0000&MySubjectNo=0000&SubNo=', 'My Window', 'top=60,left=100,width=800,height=600,titlebar=no,toolbar=no,location=no, directories=no, status=no, menubar=no,resizable=no, copyhistory=no');
//                onUpdated();
//            }
//        }

            function EmptyQuatationGrid(ele)
            {            
                var a=$('#ctl00_CPHLAMBDA_gvQuatation').dataTable().fnGetNodes().length;
                if(a==0)
                {
                    msgalert("Data Not Available !");
                    return false;
                }
                return true;
            }
            
            
            
            function EmptySynopsisGrid(ele)
            {
                var a=$('#ctl00_CPHLAMBDA_gvProject').dataTable().fnGetNodes().length;
                if(a==0)
                {
                    msgalert("Data Not Available !");
                    return false;
                }
                return true;
            }
            
            


            function ShowEditPageinQuatation(ele)
        {
            
            var temp=ele.id;
            var nodeid=($('#' + temp).parent().next().next().next().next().next().next().next().next().next().next().next().text())
            window.open('frmGlobalDocRepository.aspx?Type=r&Mode=2&workspaceId=0000002892&nodeid=' + nodeid, 'Edit Window', 'top=60,left=100,width=800,height=600,titlebar=no,toolbar=no,location=no, directories=no, status=no, menubar=no,resizable=no, copyhistory=no');
            return false;
        }
        
        function ShowDetailPageinQuatation(ele)
        {
          
            var temp=ele.id;
            var nodeid=($('#' + temp).parent().next().next().next().next().next().next().next().next().next().next().text())
            window.open('frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=0000002892&ActivityId=2336' + '&NodeId=' + nodeid + '&PeriodId=1&SubjectId=0000&Type=BA-BE&ScreenNo=0000&MySubjectNo=0000&SubNo=', 'My Window', 'top=60,left=100,width=800,height=600,titlebar=no,toolbar=no,location=no, directories=no, status=no, menubar=no,resizable=no, copyhistory=no');
            return false;
        }
        
          function ShowEditPageinSynopsis(ele)
        {
            
            var temp=ele.id;
            var nodeid=($('#' + temp).parent().next().next().next().next().next().next().next().next().next().next().next().text())
            window.open('frmGlobalDocRepository.aspx?Type=r&Mode=2&workspaceId=0000002897&nodeid=' + nodeid, 'Edit Window', 'top=60,left=100,width=800,height=600,titlebar=no,toolbar=no,location=no, directories=no, status=no, menubar=no,resizable=no, copyhistory=no');
            return false;
        }
        
        function ShowDetailPageinSynopsis(ele)
        {
                        var temp=ele.id;
            var nodeid=($('#' + temp).parent().next().next().next().next().next().next().next().next().next().next().text())
            window.open('frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=0000002897&ActivityId=2336' + '&NodeId=' + nodeid + '&PeriodId=1&SubjectId=0000&Type=BA-BE&ScreenNo=0000&MySubjectNo=0000&SubNo=', 'My Window', 'top=60,left=100,width=800,height=600,titlebar=no,toolbar=no,location=no, directories=no, status=no, menubar=no,resizable=no, copyhistory=no');
            return false;
        }
             
//        
//                // Selecting all checkbox of Client.
//             
//                
                   $("#ctl00_CPHLAMBDA_chkAnyClient").live("click",function() {
                    if (this.checked == true) {
                        $('#ctl00_CPHLAMBDA_chkclient input[type="checkbox"]').each(function() {
                            this.checked = true;
                        });
                    }
                    else {
                        $('#ctl00_CPHLAMBDA_chkclient input[type="checkbox"]').each(function() {
                            this.checked = false;
                        });
                    }
                });
//                
//                //selecting all checkboxes of molecule

                $("#ctl00_CPHLAMBDA_chkAnyMolecule").live("click",function() {
                    if (this.checked == true) {
                        $('#ctl00_CPHLAMBDA_chkmolecule input[type="checkbox"]').each(function() {
                            this.checked = true;
                        });
                    }
                    else {
                        $('#ctl00_CPHLAMBDA_chkmolecule input[type="checkbox"]').each(function() {
                            this.checked = false;
                        });
                    }
                });

       

                // Selecting all checkbox of Project Type.
                
                $("#ctl00_CPHLAMBDA_chkAnyProjectType").live("click",function() {
                    if (this.checked == true) {
                        $('#ctl00_CPHLAMBDA_chkprojecttype input[type="checkbox"]').each(function() {
                            this.checked = true;
                        });
                    }
                    else {
                        $('#ctl00_CPHLAMBDA_chkprojecttype input[type="checkbox"]').each(function() {
                            this.checked = false;
                      });
                  }
                });
                

//               // Selecting all checkbox of Location.
               
            $("#ctl00_CPHLAMBDA_chkAnyLocation").live("click",function() {
            if (this.checked == true) {
                $('#ctl00_CPHLAMBDA_chklocation input[type="checkbox"]').each(function() {  
                 this.checked =true});           }
            else {
               $('#ctl00_CPHLAMBDA_chklocation input[type="checkbox"]').each(function() {
                    this.checked = false;
              });
            }
        });
        
        
                
                
                 $('#ctl00_CPHLAMBDA_chkclient input[type="checkbox"]').live("click",function() {
                    if(document.getElementById ('ctl00_CPHLAMBDA_chkAnyClient').checked==true)
                    {
                        document .getElementById ('ctl00_CPHLAMBDA_chkAnyClient').checked = false 
                    }
                    }); 
                
        
        
        

                // For Deselecting checkbox of chkAnyMolecule.
                $('#ctl00_CPHLAMBDA_chkmolecule input[type="checkbox"]').live("click", function() {
                    if (document.getElementById('ctl00_CPHLAMBDA_chkAnyMolecule').checked == true) {
                        document.getElementById('ctl00_CPHLAMBDA_chkAnyMolecule').checked = false;
                    }
                });

                // For Deselecting checkbox of chkAnyProjectType.
                $('#ctl00_CPHLAMBDA_chkprojecttype input[type="checkbox"]').live("click", function() {
                    if (document.getElementById('ctl00_CPHLAMBDA_chkAnyProjectType').checked == true) {
                        document.getElementById('ctl00_CPHLAMBDA_chkAnyProjectType').checked = false;
                    }
                });

//                // For Deselecting checkbox of chkAnyLocation.

                  $('#ctl00_CPHLAMBDA_chklocation input[type="checkbox"]').live("click",function() {
                    if(document.getElementById ('ctl00_CPHLAMBDA_chkAnyLocation').checked==true)
                    {
                        document .getElementById ('ctl00_CPHLAMBDA_chkAnyLocation').checked = false 
                    }
                    }); 
                       

        //Expand/Collapse Quatation Grid.
        function showGvQuatation(ele) {
            if (ele.src.toUpperCase().search('COLLAPSE') != -1) {
                $(".panel").slideToggle(500);
                ele.src = "images/expand.jpg";
            }
            else {
                $(".panel").slideToggle(500);
                ele.src = "images/collapse.jpg";
            }
        }

        function showGvProject(ele) {
            if (ele.src.toUpperCase().search('COLLAPSE') != -1) {
               $(".panel1").slideToggle(500);
                ele.src = "images/expand.jpg";
                //$("#ctl00_CPHLAMBDA_gvProject_info").hide();
            }
            else {
                $(".panel1").slideToggle(500);
                ele.src = "images/collapse.jpg";
               // $("#ctl00_CPHLAMBDA_gvProject_info").show();
            }
        }

        function pageLoad() {
            
          
           var aTable = $('#<%= chkclient.ClientID %>').prepend($('<thead>').append($('#<%= chkclient.ClientID %> tr:first'))).dataTable({ 
                "bStateSave": true,
////                "fnStateSave": function(oSettings, oData) {
////                    //localStorage.setItem('<%= gvQuatation.ClientID %>_' + window.location.pathname, JSON.stringify(oData));
////                },
////                "fnStateLoad": function(oSettings) {
////                  //  var data = localStorage.getItem('<%= gvQuatation.ClientID %>_' + window.location.pathname);
////                    return JSON.parse(data);
////                },
////               "sPaginationType": "scrolling"
                    "bPaginate": false,
                    "bSort": false,
                    "bDestory": true,
                    "bRetrieve":true                  
            });
            
              if (aTable.length) new FixedHeader(aTable);
                var bTable = $('#<%= chkmolecule.ClientID %>').prepend($('<thead>').append($('#<%= chkmolecule.ClientID %> tr:first'))).dataTable({
                "bStateSave": true,
//                "fnStateSave": function(oSettings, oData) {
//                    //localStorage.setItem('<%= gvQuatation.ClientID %>_' + window.location.pathname, JSON.stringify(oData));
//                },
//                "fnStateLoad": function(oSettings) {
//                  //  var data = localStorage.getItem('<%= gvQuatation.ClientID %>_' + window.location.pathname);
//                    return JSON.parse(data);
//                },

                     "bPaginate": false,
                     "bSort": false,
                    "bDestory": true,
                    "bRetrieve":true                  
            });
     
            var cTable = $('#<%= chkprojecttype.ClientID %>').prepend($('<thead>').append($('#<%= chkprojecttype.ClientID %> tr:first'))).dataTable({
                "bStateSave": true,
//                "fnStateSave": function(oSettings, oData) {
//                    //localStorage.setItem('<%= gvQuatation.ClientID %>_' + window.location.pathname, JSON.stringify(oData));
//                },
//                "fnStateLoad": function(oSettings) {
//                  //  var data = localStorage.getItem('<%= gvQuatation.ClientID %>_' + window.location.pathname);
//                    return JSON.parse(data);
//                },
//                "sPaginationType": "full_numbers"
                     "bPaginate": false,
                     "bSort": false,
                     "bDestory": true,
                     "bRetrieve":true                  
           
            }); 

                var a=document .getElementById ('#<%= chklocation.ClientID %>');
                var dTable = $('#<%= chklocation.ClientID %>').prepend($('<thead>').append($('#<%= chklocation.ClientID %> tr:first'))).dataTable({
                "bStateSave": true,
//                "fnStateSave": function(oSettings, oData) {
//                    //localStorage.setItem('<%= gvQuatation.ClientID %>_' + window.location.pathname, JSON.stringify(oData));
//                },
//                "fnStateLoad": function(oSettings) {
//                  //  var data = localStorage.getItem('<%= gvQuatation.ClientID %>_' + window.location.pathname);
//                    return JSON.parse(data);
//                },
//                "sPaginationType": "full_numbers"
                     "bPaginate": false,
                     "bSort": false,
                     "bJQueryUI": true,
                     "bDestory": true,
                    "bRetrieve":true                  
            });
            
            
            $('.fixedHeader').remove();
            var oTable = $('#<%= gvQuatation.ClientID %>').prepend($('<thead>').append($('#<%= gvQuatation.ClientID %> tr:first'))).dataTable({
                "bStateSave": true,
//                "fnStateSave": function(oSettings, oData) {
//                    //localStorage.setItem('<%= gvQuatation.ClientID %>_' + window.location.pathname, JSON.stringify(oData));
//                },
//                "fnStateLoad": function(oSettings) {
//                  //  var data = localStorage.getItem('<%= gvQuatation.ClientID %>_' + window.location.pathname);
//                    return JSON.parse(data);
//                },
                "sPaginationType": "full_numbers",
                    "bDestory": true,
                    "bRetrieve":true
            });
            if (oTable.length) new FixedHeader(oTable);

            //return true;
            var oTable1 = $('#<%= gvProject.ClientID %>').prepend($('<thead>').append($('#<%= gvProject.ClientID %> tr:first'))).dataTable({
                "bStateSave": true,
//                "fnStateSave": function(oSettings, oData) {
//                    //localStorage.setItem('<%= gvProject.ClientID %>_' + window.location.pathname, JSON.stringify(oData));
//                },
//                "fnStateLoad": function(oSettings) {
//                    var data = localStorage.getItem('<%= gvProject.ClientID %>_' + window.location.pathname);
//                    return JSON.parse(data);
//                },
                "sPaginationType": "full_numbers",
                    "bDestory": true,
                    "bRetrieve":true
            });
            if (oTable1.length) new FixedHeader(oTable1);
        }

        //Check any client or any molecules is selected or not if not selected then gives a only prompt.
        function checkClientAndMolecules() {
            if ($('#divClient input[type="checkbox"]:checked').length <= 0 && $('#divMolecule input[type="checkbox"]:checked').length <= 0)
                msgalert('You did not select Any Client OR Any Molecules !');
        }
        
        function RefreshClientAndMolecules(){
                        var currentTime = new Date();
            var year = currentTime.getFullYear();
            var fromdate= "1" + "-" + "Jan" + "-" + year;
            var todate= "31" + "-" + "Dec" + "-" + year;
            
                    
            document.getElementById("<%= ddlReportFor.ClientID %>").selectedIndex=4;
            document.getElementById("<%= txtFromDate.ClientID %>").value= fromdate;
            document.getElementById("<%= txtToDate.ClientID %>").value= todate;    
            
            document.getElementById("<%= chkAnyClient.ClientID %>").checked= false;
            $('#ctl00_CPHLAMBDA_chkclient input[type="checkbox"]').each(function()
            {
                this.checked=false;
            });
             
            document.getElementById("<%= chkAnyMolecule.ClientID %>").checked= false;
            $('#ctl00_CPHLAMBDA_chkmolecule input[type="checkbox"]').each(function()
            {
                this.checked=false;
            });
            
            document.getElementById("<%= chkAnyProjectType.ClientID %>").checked= false;
            $('#ctl00_CPHLAMBDA_chkprojecttype input[type="checkbox"]').each(function()
            {
                this.checked=false;
            });
            
             document.getElementById ('ctl00_CPHLAMBDA_chkAnyLocation').checked = false;
             $('#ctl00_CPHLAMBDA_chklocation input[type="checkbox"]').each(function()
             {
                this.checked=false;
             });
             
             return false;
             
            }
//             window.location.reload(true);
    </script>
</asp:Content>
