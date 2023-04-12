<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmArchiveMonitorReport.aspx.vb" Inherits="frmReport"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        #accordion
        {
            /*background-color: #eee;*/
            border: 1px solid #ccc;
            width: 700px;
            padding: 5px 10px 5px 10px;
            margin: 5px auto;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            border-radius: 3px;
            box-shadow: 0 1px 0 #999;
            list-style: none;
        }
        #accordion div
        {
            float: none;
            display: block;
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=                          '#1560a1' , endColorstr= '#2372b2' ,GradientType=1 ); /* IE6-9 */
            font-family: Verdana;
            font-size: "11px";
            margin: 1px;
            cursor: pointer;
            padding: 5px 10px 5px 10px;
            list-style: circle;
            color: White;
            font-weight: bold;
        }
        #accordion ul
        {
            list-style: none;
            display: none;
            padding: 0 5 0 5;
        }
        #accordion ul ul li div
        {
            cursor: pointer;
            caption-side: top;
            display: block; /*#A4E4FF*/
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=          '#A4E4FF' , endColorstr= '#CEE3ED' ,GradientType=1 ); /* IE6-9 fallback on horizontal gradient */
            padding: 5px 10px 5px 10px;
            text-decoration: none;
            color: navy;
            font-weight: bold;
        }
        #accordion div:hover
        {
            font-weight: bolder;
            font-style: italic;
            cursor: pointer;
        }
        #accordion ul li div
        {
            caption-side: top;
            display: block;
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr=                                   '#3e88c4' , endColorstr= '#60a0d1' ,GradientType=1 ); /* IE6-9 fallback on horizontal gradient */
            padding: 5px 10px 5px 10px;
            text-decoration: none;
            color: White;
            cursor: auto;
        }
        #ctl00_CPHLAMBDA_DvChilds
        {
            list-style: none;
            margin: 3px 0;
            padding: 0;
            height: 200px;
            overflow: auto;
            height: 500px;
            scrollbar-arrow-color: white;
            scrollbar-face-color: #7db9e8;
        }
    </style>
    <asp:UpdatePanel runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td width="900px" align="left" valign="top">
                        <asp:UpdatePanel ID="ProjectData" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table align="center" cellpadding="5">
                                    <tr>
                                        <td align="right" class="Label" nowrap="nowrap" style="text-align: right; width: 80px;">
                                            Project
                                        </td>
                                        <td align="left" class="Label" style="text-align: left; width: 740px;">
                                            <asp:TextBox ID="txtProject" runat="server" CssClass="textBox" Width="600px"></asp:TextBox>
                                            <asp:HiddenField ID="HProjectId" runat="server" />
                                            <asp:HiddenField ID="HSchemaId" runat="server" />
                                            <asp:HiddenField ID="HArchivedFlag" runat="server" />
                                            <asp:HiddenField ID="HLabArchiveFlag" runat="server" />
                                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project">
                                            </asp:Button>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                OnClientShowing="ClientPopulated" ServiceMethod="view_ArchiveProjectList" ServicePath="AutoComplete.asmx"
                                                TargetControlID="txtProject" UseContextKey="True">
                                            </cc1:AutoCompleteExtender>
                                            <asp:CheckBox runat="server" ID="ChkBClient" Style="display: none;" Text="All Clients" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <table align="left" cellpadding="8" style="margin-left: 0px; vertical-align: top;
                                    display: none;" id="MonitorInfo" runat="server" width="910px">
                                    <tr>
                                        <td style="padding-left: 82px;" id="tdProjectInfo" runat="server">
                                            <fieldset id="fProjectInfo" runat="server" style="width: 770px; height: auto;">
                                                <legend id="lProjectInfo" runat="server">
                                                    <img id="imgExpand" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectInfo(this);" />
                                                    <asp:Label ID="lblProjectInfo" runat="server" Text="Project Information" CssClass="Label">
                                                    </asp:Label>
                                                </legend>
                                                <div id="ProjectInfo" style="display: none; margin: 5px;">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 19%">
                                                                <asp:Label runat="server" ID="lblmgr" Text="Project Manager : " CssClass="Label" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblManager" class="Label" runat="server" CssClass="labeldisplay" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lblPIMonitor" CssClass="Label" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPI" Style="width: 50%" class="Label" runat="server" CssClass="labeldisplay" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 82px;">
                                            <fieldset id="fReports" runat="server" style="width: 770px; display: block; height: auto;">
                                                <legend id="lReports" runat="server">
                                                    <img id="img2" alt="Report" src="images/expand.jpg" onclick="displayReports(this);" />
                                                    <asp:Label ID="Label1" runat="server" Text="Reports" CssClass="Label">
                                                    </asp:Label>
                                                </legend>
                                                <div id="Reports" style="display: none; margin: 5px;">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="center" style="cursor: pointer;">
                                                                <asp:Label runat="server" ID="lblRptAttandance" Text="Attandance" CssClass="Label"
                                                                    Font-Underline="true" Font-Bold="true" onclick="funReport('frmWorkspaceSubjectMst.aspx?mode=4&Type=ArchiveRPT&WorkSpaceId=','2','')" />
                                                            </td>
                                                            <td align="center" style="cursor: pointer;">
                                                                <asp:Label runat="server" ID="lblRptLabReport" Text="Lab Report " CssClass="Label"
                                                                    Font-Underline="true" Font-Bold="true" onclick="funcheckLabData('frmArchiveLabReportReview.aspx?Type=ArchiveRPT&vWorkSpaceId=','0')" />
                                                            </td>
                                                            <td align="left" style="cursor: pointer;">
                                                                <asp:Label runat="server" ID="lblRptScreening" Text="Screening" CssClass="Label "
                                                                    Font-Underline="true" Font-Bold="true" onclick="funcheckLabData('frmSourceQA.aspx?mode=4&Type=ArchiveRPT&Act=QA on MSR&vActivityId= 1185&WorkSpaceId=','1')" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 82px;" id="tdProjectActivity" runat="server">
                                            <fieldset id="fProjectActivity" runat="server" style="width: 770px;">
                                                <legend id="lProjectActivity" runat="server">
                                                    <img id="img1" alt="SubjectSpecific" src="images/expand.jpg" onclick="displayProjectActivity(this),funParentActivity(this);" />
                                                    <asp:Label ID="lblProjectActivity" runat="server" Text="Project Activity" CssClass="Label">
                                                    </asp:Label>
                                                </legend>
                                                <div id="divsubject" style="display: none; padding-left: 20px">
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="50%" align="left">
                                                                <asp:DropDownList ID="Ddlsubject" Width="300px" CssClass="dropDownList" AutoPostBack="true"
                                                                    runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td width="50%" align="left">
                                                                <img src="images/red.gif" /><label class="Label">
                                                                    -REJECTED SUBJECTS</label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div id="ProjectActivity" style="display: none; overflow: auto; height: 450px; scrollbar-arrow-color: white;
                                                    scrollbar-face-color: #7db9e8;">
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 82px;">
                                            <div runat="server" id="DvChilds" style="display: none;">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:Button ID="btnShow" runat="server" Text="Show Dialog" Style="display: none" />
                                                <cc1:ModalPopupExtender ID="MPEActivitySequence" runat="server" TargetControlID="btnShow"
                                                    PopupControlID="DivMPECTM" BackgroundCssClass="modalBackground" PopupDragHandleControlID="DivMPECTM"
                                                    BehaviorID="MPEId">
                                                </cc1:ModalPopupExtender>
                                                <div id="DivMPECTM" runat="server" class="centerModalPopup" style="display: none;
                                                    width: 800px; max-height: 600px">
                                                    <div style="width: 100%">
                                                        <h1 class="header">
                                                            <label id="lblDocAction" class="labelbold">
                                                                For Which Site Do You Want To See Report?
                                                            </label>
                                                        </h1>
                                                    </div>
                                                    <div>
                                                        <table cellpadding="5" width="100%">
                                                            <tr>
                                                                <td>
                                                                    <div id="DivCTM" runat="server" style="font-weight: bold;">
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="HQAONMSR" runat="server" />
                                <%--<asp:HiddenField ID="HQAONPIF" runat="server" />--%>
                                <asp:HiddenField ID="HCTMStatus" runat="server" />
                                <asp:HiddenField ID="HOpenString" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>

    <script type="text/javascript" src="Script/jquery.searchabledropdown-1.0.7.min.js"></script>

    <script type="text/javascript" language="javascript">
    
     $(document).ready(function()
     {
       $("#ctl00_CPHLAMBDA_Ddlsubject").searchable();
     });
    function pageLoad()
    {
      $("#ctl00_CPHLAMBDA_Ddlsubject").css("height","20px")
      $("#ctl00_CPHLAMBDA_Ddlsubject").css("width","300px")
      $('#ctl00_CPHLAMBDA_Ddlsubject option').each(function(){
        var str=$(this).val();
        var cRejectionFlag=str.split(",");
          if(cRejectionFlag[1]=="Y")
            $(this).css('color','red');
      });
    }
    
     function SetValue(vProjNo,vWorkSpaceId)
    {
         $find('MPEId').hide();
         var radio = document.getElementById('<%=DivCTM.clientid %>').getElementsByTagName('input');
         for (var i = 0; i < radio.length; i++) 
         {
                if (radio[i].checked)
                {
                    radio[i].checked=false ;
                }
          }                   
               
        var str= document.getElementById('<%=HOpenString.ClientId %>').value;       
        window.open(str.toString ()+vWorkSpaceId.toString()+ "&ProjectNo=" +vProjNo.toString ()+"&SchemaId="+document.getElementById('<%= HSchemaId.ClientId%>').value);
        
        
    }
    function accordionlidiv(ele,vWorkSpaceId,iParentId,iPeriod,vActivityId,cSubjectWiseFlag,vNodeDisplayName)
    {
        if($(ele).parent()[0].children.length == 1)
        {
            PageMethods.getChildActivity
            (   
                 vWorkSpaceId.toString(),  
                 document.getElementById ('<%= HCTMStatus.ClientId %>').value ,
                  iParentId ,
                  iPeriod,
                  vActivityId,
                  cSubjectWiseFlag,
                  vNodeDisplayName,
                  document.getElementById('<%= HSchemaId.ClientId%>').value, 
                function(Result)
                {
                    $(ele).parent().append(Result);
                    if(false == $(ele).next().is(':visible')) 
                    {
                        $('#accordion ul').slideUp(300);
                    }
                     $(ele).next().slideToggle(300);
                    
                },
                function(eerror)
                {
                     msgalert(eerror); 
                }
            );
        }
        else
        {
            if(false == $(ele).next().is(':visible')) 
            {
                $('#accordion ul').slideUp(300);
            }
            $(ele).next().slideToggle(300);
        }
    }
    function ddlIndexChange()
    {
       var ele=document.getElementById('img1');
       ele.src="images/collapse_blue.jpg";
       funParentActivity(ele)
  
       document.getElementById ('divsubject').style.display='block';   
       $("#ProjectActivity").slideToggle(500);
 
      
    }
    function accordionlidivSubject(ele,vWorkSapceId,iNodeId,iPeriod,vActivityId,cSubjectWiseFlag)
    {
          var str="";
          
           if(document.getElementById('ctl00_CPHLAMBDA_Ddlsubject').selectedIndex > 0)
           {
                 str=$("#ctl00_CPHLAMBDA_Ddlsubject").val();
                 var subject=str.split(",");
                 str=subject[0].toString(); 
           } 
           PageMethods.getSubject
           (   
                vWorkSapceId.toString (),               
                document.getElementById ('<%= HCTMStatus.ClientId %>').value ,
                iNodeId  ,
                iPeriod,
                vActivityId,
                cSubjectWiseFlag,
                str,
                document.getElementById('<%= HSchemaId.ClientId%>').value, 
                function(Result)
                {
                    $(ele).parent().append(Result);
                    if(false == $(ele).next().is(':visible')) 
                    {
                         $('#accordion ul ul').slideUp(300);
                    }
                     $(ele).next().slideToggle(300);
                 },
                function(eerror)
                {
                     msgalert(eerror); 
                }
            );
    }
    function funParentActivity(ele)
    {
       $("#ctl00_CPHLAMBDA_Ddlsubject").searchable();
        if(ele.src.toString().toUpperCase().search("EXPAND")==-1)
        {
           document.getElementById('ProjectActivity').innerHTML.trim()=="";
                PageMethods.GetProc_TreeViewOfNodes
                (   
                    document.getElementById('<%= HProjectId.ClientId%>').value,
                    document.getElementById('<%= HSchemaId.ClientId%>').value, 
                    function(Result)
                    {
                        $("#ProjectActivity").append(Result);
                    },
                    function(eerror)
                    {
                         msgalert(eerror); 
                    }
                );
               
       }
        
    }

    function funParentActivityCTM(ele,ProjectActivity,ProjectId)
    {
        if(ele.src.toString().toUpperCase().search("EXPAND")==-1)
        {
            if ( document.getElementById(ProjectActivity).innerHTML=='')
            {
                PageMethods.GetProc_TreeViewOfNodes
                (   
                    ProjectId,
                    function(Result)
                    {
                        $("#" + ProjectActivity).append(Result);
                    },
                    function(eerror)
                    {
                         msgalert(eerror); 
                    }
                );
         }
        
       }
        
    }
    function funcheckLabData(str,Flag)
    {
        PageMethods.GetLabData
        (
            document.getElementById('<%= HSchemaId.ClientId%>').value,
            document.getElementById('<%= HProjectId.ClientId%>').value,
            function(Result)
            {
                var RowCount=parseInt(Result);
                if(RowCount > 0)
                {
                    funReport(str,Flag,1);
                } 
                else
                {
                    funReport(str,Flag,0);
                }
                   
            },
            function(eerror)
            {
            msgalert(eerror);  
            }
        );
    
    }
         
    function funReport(str,Flag,LabFlag)
    {
    if( document.getElementById('<%=ChkBClient.ClientId%>').checked==true && document.getElementById('<%= HCTMStatus.ClientId %>').value=='Y')
    {              
        $find('MPEId').show();
        document.getElementById('<%=HOpenString.ClientId %>').value=str.toString();
              
     }
      else
     {
        if (Flag == 2)
        {
            window.open(str.toString ()+document.getElementById('<%= HProjectId.ClientId%>').value+ "&ProjectNo=" + document .getElementById ('<%=txtProject.ClientId %>').value);
        }
        else 
        {
            if(Flag == 0)
            {
                var Status=document.getElementById ('<%= HLabArchiveFlag.ClientId %>').value;
                if(Status == "Y")
                {
                    msgalert("Lab Report is Archived To see its Details unArchive It !");
                }
                else
                {
                    if(LabFlag.toString()=="0")
                        msgalert("For This project Lab Report is Not Archived !");                              
                    else            
                        window.open(str.toString ()+document.getElementById('<%= HProjectId.ClientId%>').value + "&ProjectNo=" + document .getElementById ('<%=txtProject.ClientId %>').value+"&SchemaId="+document.getElementById('<%= HSchemaId.ClientId%>').value);
                }

            }
            else 
            {
                var Status=document.getElementById ('<%= HLabArchiveFlag.ClientId %>').value;
                window.open(str.toString ()+document.getElementById('<%= HProjectId.ClientId%>').value+"&NodeId="+document.getElementById('<%= HQAONMSR.ClientId%>').value+"&SchemaId="+document.getElementById('<%= HSchemaId.ClientId%>').value+"&LabDataStatus="+Status.toString()+"&LabDataFlag="+LabFlag.toString ());
            }
        }        
    }
    }
    
    function funOpen(str)
    {
        str+="&SchemaId=" + document.getElementById('<%= HSchemaId.ClientId %>').value; 
        window.open(str);
    }
   
    function ClientPopulated(sender, e)
    {
       ProjectClientShowingSchema('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
    }

    function OnSelected(sender, e)
    {
       ProjectOnItemSelectedSchema(e.get_value(), $get('<%= txtProject.clientid %>'),
       $get('<%= HProjectId.clientid %>'), $get('<%= HSchemaId.clientid %>'),$get('<%=HArchivedFlag.ClientId %>'),
       $get('<%=HLabArchiveFlag.clientID %>'),
       document.getElementById('<%= btnSetProject.ClientId %>'));
    }    
    function displayProjectInfo(ele)
    {
        if(ele.src.toString().toUpperCase().search("EXPAND")!=-1)
        {
                 $("#ProjectInfo").slideToggle(500);   
                 ele.src="images/collapse_blue.jpg";
        }
        else
        {
                $("#ProjectInfo").slideToggle(500);
                ele.src = "images/expand.jpg";
         }
     }
    function displayReports(ele)
    {
     if(ele.src.toString().toUpperCase().search("EXPAND")!=-1)
        {
                 $("#Reports").slideToggle(500);   
                 ele.src="images/collapse_blue.jpg";
        }
        else
        {
                $("#Reports").slideToggle(500);
                ele.src = "images/expand.jpg";
         }
    
    }
    function displayProjectInfoCTM(ele)
    {
        if(ele.src.toString().toUpperCase().search("EXPAND")!=-1)
        {
                 $("#ProjectInfo").slideToggle(500);   
                 ele.src="images/collapse_blue.jpg";
        }
        else
        {
                $("#ProjectInfo").slideToggle(500);
                ele.src = "images/expand.jpg";
         }
     }
     
     function displayProjectActivity(ele)
     {
         $("#ctl00_CPHLAMBDA_Ddlsubject").css("height","20px")
        $("#ctl00_CPHLAMBDA_Ddlsubject").css("width","300px")
        if(ele.src.toString().toUpperCase().search("EXPAND")!=-1)
        {
                
                 $("#ProjectActivity").slideToggle(500);
                 document.getElementById ('divsubject').style.display='block';   
                 ele.src="images/collapse_blue.jpg";
        }
        else
        {
          
                $("#ProjectActivity").slideToggle(500);
                document.getElementById ('divsubject').style.display='none';   
                ele.src = "images/expand.jpg";
         }
     }
     
     function displayProjectActivityCTM(ele,ProjectActivityId,childId)
     {
        if(ele.src.toString().toUpperCase().search("EXPAND")!=-1)
        {
                 
                 $("#" + ProjectActivityId).slideToggle(500);   
                 ele.src="images/collapse_blue.jpg";
                 $("#" + childId).css('height', function(index) 
                    {
                        return $("#" + childId).height() + 400;
                    });
        }
        else
        {
          
                $("#" + ProjectActivityId).slideToggle(500);
                ele.src = "images/expand.jpg";
                $("#" + childId).css('height', function(index) 
                    {
                        return $("#" + childId).height() - 400;
                    });
         }
     }


     function displayChildCTM(ele,ProjectActivityId,ChildEle,ChildInfo)
     {
        if(ele.src.toString().toUpperCase().search("EXPAND")!=-1)
        {
          
                 $("#" + ProjectActivityId).slideToggle(500);   
                 ele.src="images/collapse_blue.jpg";
                 
                 if (document.getElementById(ChildEle).src.toString().toUpperCase().search("EXPAND")!=-1)
                 {
                     $("#" + ChildInfo).slideToggle(500);   
                     document.getElementById(ChildEle).src="images/collapse_blue.jpg";
                 }
        }
        else
        {
          
                $("#" + ProjectActivityId).slideToggle(500);
                ele.src = "images/expand.jpg";
 
                if (document.getElementById(ChildEle).src.toString().toUpperCase().search("EXPAND")==-1)
                {
                    $("#" + ChildInfo).slideToggle(500);
                    document.getElementById(ChildEle).src = "images/expand.jpg";
                }
         }
     }


     function displayChildInfoCTM(ele,ProjectActivityId,childId)
     {
        if(ele.src.toString().toUpperCase().search("EXPAND")!=-1)
        {
          
                 $("#" + ProjectActivityId).slideToggle(500);   
                 ele.src="images/collapse_blue.jpg";
                 
                $("#" + childId).css('height', function(index) 
                {
                    return $("#" + childId).height() + 100;
                });
       }
        else
        {
          
                $("#" + ProjectActivityId).slideToggle(500);
                ele.src = "images/expand.jpg";
                
                $("#" + childId).css('height', function(index) 
                {
                    return $("#" + childId).height() - 100;
                });
        }
     }
    </script>

</asp:Content>
