
Partial Class ExportWorkspaceNodedeatail
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private eStr_Retu As String
    Private rPage As RepoPage
    Private index1 As Integer = 0

    Private dt As DataTable = Nothing
    Private Const VS_DtWSSub As String = "DtWSSub"
    Private Const VS_ActivityId As String = "ActivityId"
    Private Const VS_ActivityName As String = "ActivityName"
    Dim parentNode As New TreeNode
    Dim eStr As String = ""
#End Region

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            GenCall()

        End If
        Exit Sub
    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim dt_WorkspaceNodeDetail As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim wStr As String = "1=2"
        Dim ds_WorkspacenodeDetail As New Data.DataSet
        Try

            Choice = CType("1", WS_Lambda.DataObjOpenSaveModeEnum)

            Me.ViewState("Choice") = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState("vActivityId") = Me.Request.QueryString("Value").ToString

            End If

            ''Check for Valid User''
            If Not GenCall_Data(Choice, dt_WorkspaceNodeDetail) Then ' For Data Retrieval
                Exit Function
            End If

            wStr = " vWorkspaceid = '" + Me.HProjectId.Value + "'"

            If Not objHelp.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkspacenodeDetail, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            Me.ViewState("dtWorkspaceNodeDetail") = dt_WorkspaceNodeDetail ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, dt_WorkspaceNodeDetail) Then 'For Displaying Data 
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data(ByVal Choice As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = ""
        Dim wStr As String = ""
        Dim ds As DataSet = Nothing
        Dim i As Integer

        Try

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vActivityId=" + Me.ViewState("vActivityId").ToString() 'Value of where condition
                wStr = "vWorkspaceId='" & Me.TVWorkspace.CheckedNodes(i).Value & "' and iNodeId=" & Me.TVWorkspace.CheckedNodes(i).ImageToolTip
            End If

            If Not objHelp.getWorkSpaceNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Throw New Exception(eStr)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
               Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found for Selected role")
            End If

            dt_Dist_Retu = ds.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI(ByVal Choice As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_Dist As DataTable) As Boolean
        Dim dt_treenodedetail As DataTable = Nothing

        CType(Master.FindControl("lblHeading"), Label).Text = "Export Workspace Node Detail"
        Page.Title = " :: Export WorkspaceNode Detail ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
        'If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

        '    Me.ddlAddActivity.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_treenodedetail.Rows(0)("vActivityId"), dt_treenodedetail.Rows(0)("vActivityId").GetType)
        '    Me.ddlAct.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_treenodedetail.Rows(0)("vActivityId"), dt_treenodedetail.Rows(0)("vActivityId").GetType)

        'End If

        'Me.BtnBack.Visible = False
        'Added on 10-Jul-2009
        If Not IsNothing(Me.Request.QueryString("WorkspaceId")) AndAlso Me.Request.QueryString("WorkspaceId") <> "" Then

            If Not IsNothing(Me.Request.QueryString("WorkspaceName")) AndAlso Me.Request.QueryString("WorkspaceName") <> "" Then

                Me.txtproject.Text = Me.Request.QueryString("WorkspaceName").ToString()

            End If

            Me.HProjectId.Value = Me.Request.QueryString("WorkspaceId")



            'Me.BtnBack.Visible = True

        End If
        '**************************************

        '==added on 11-Nov-2011 by Mrunal Parekh to show project according to user
        Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
        '========
        BindTree()
        'If Not FillActivityGroup() Then
        '    Return False
        'End If

        Return True
    End Function
#End Region

#Region "Bindtree"

    Private Sub BindTree()

        Dim Ds_WorkSpaceNode As New DataSet
        Dim dsVacantPosition As New DataSet
        Dim dsWorkspace As New DataSet
        Dim dt_Workspace As New DataTable
        Dim CurrentNode As New TreeNode
        Dim WorkspaceId As String = ""
        Dim ParentNode As New TreeNode
        Dim bln As Boolean = False
        Dim cnt As Integer = 0
        Dim eStr As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.TVWorkspace.Nodes.Clear()
            WorkspaceId = Me.HProjectId.Value.Trim()
            Ds_WorkSpaceNode = Me.objHelp.GetProc_TreeViewOfNodes(WorkspaceId)

            dt = Ds_WorkSpaceNode.Tables(0)
            Me.ViewState("Dt") = dt
            TestLast("0", New TreeNode())

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Sub TestLast(ByVal parentId As String, ByVal tn As TreeNode)
        Dim dr() As DataRow = dt.Select("iParentNodeId=" & parentId)
        Dim parentNode As New TreeNode
        Dim cnt As Integer

        Try

            For index As Integer = 0 To dr.Length - 1

                parentNode = New TreeNode

                parentNode.Text = Replace(dr(index)("vNodeDisplayName").ToString, "*", "") & "(" & dr(index)("vNodeName") & ")" '& "(" & dr(index)("iNodeId") & ")" & "(" & dr(index)("iNodeId") & ")"
                'parentNode.Value = Replace(dr(index)("vNodeDisplayName").ToString, "*", "")
                parentNode.Value = Replace(dr(index)("vActivityId").ToString, "*", "")
                parentNode.ImageToolTip = dr(index)("iNodeId")
                'parentNode.ToolTip = dr(index)("vActivityId").ToString.Trim()
                parentNode.NavigateUrl = "javascript:void(0);"

                If parentId = "0" Then
                    TVWorkspace.Nodes.Add(parentNode)
                    cnt = TVWorkspace.Nodes.Count
                Else
                    tn.ChildNodes.Add(parentNode)
                End If

                TestLast(dr(index)("iNodeId"), parentNode)


            Next index

            'Me.TVWorkspace.Attributes.Add("OnClick", "return ParentChildCheck('" + TVWorkspace.ClientID + "');")
            Me.TVWorkspace.Attributes.Add("OnClick", "OnTreeClick(event)")
            '            OnTreeClick()

        Catch threadex As Threading.ThreadAbortException
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    

#End Region


    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        BindTree()
    End Sub



#Region "Report Helper Functions"

    Private Function OpenReport(ByVal ReportFileName_1 As String) As Boolean
        '' This Function open file on physical memory(In HardDist)          
        If File.Exists(ReportFileName_1) = True Then
            File.Delete(ReportFileName_1)
        End If
        rPage = New RepoPage(ReportFileName_1)
    End Function

    Private Sub ReportHeader()
        Dim rRow As RepoRow
        Dim rCell As RepoCell

        rRow = New RepoRow
        rCell = rRow.AddCell("CompanyTitle")
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 14
        rCell.Value = System.Configuration.ConfigurationManager.AppSettings("Client")
        rCell.NoofCellContain = 10
        rCell.FontColor = Drawing.Color.Maroon
        rCell.BackgroundColor = Drawing.Color.White
        rPage.Say(rRow)

        rRow = New RepoRow
        rCell = rRow.AddCell("ReportTitle")
        rCell.Value = "WorkSpaceNodeDetails"
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 10
        rPage.Say(rRow)

      
        rPage.SayBlankRow()

    End Sub

    Private Sub PrintHeader()
        Dim rRow As RepoRow
        rRow = New RepoRow

        rRow = masterRow()
        rRow.Cell("Tree").Value = "Project Structure"
        rRow = masterRow()
        rRow.Cell("Tree").Value = ""

        rRow.Cell(0).FontBold = True
        rRow.Cell(0).Alignment = RepoCell.AlignmentEnum.CenterTop
        rPage.Say(rRow)

    End Sub

    Private Sub ReportDetail(ByVal TVW As TreeNodeCollection)

        Dim rRow As RepoRow
        Dim dt_Report As New DataTable
        Dim PreviousCompany As String = ""
        Dim PreviousProject As String = ""
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dr As DataRow
        Dim i As Integer = 0

        Try
            rRow = masterRow()
            PrintHeader()
            dt = CType(ViewState("Dt"), DataTable)
            
            Do While index1 <= TVWorkspace.CheckedNodes.Count - 1

                rRow.Cell("Tree").Value = TVWorkspace.CheckedNodes(index1).Text.ToString




                rPage.Say(rRow)
                index1 += 1
            Loop ''detail loop ending
            ' ReportDetail(index1 + 1)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "Error While Print Detail", "")
        End Try
    End Sub

    Private Function masterRow() As RepoRow
        Dim rRow As RepoRow
        Dim rCell As RepoCell
        Dim i As Integer

        rRow = New RepoRow

        rCell = New RepoCell("Tree")
        rRow.AddCell(rCell)

        For i = 0 To rRow.CellCount - 1
            rRow.Cell(i).FontSize = 11

        Next i

        Return rRow

    End Function

#End Region

    'Protected Sub btnexportpdf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexportpdf.Click
    '    Dim q As String = ""

    '    Try
    '        Do While index1 <= TVWorkspace.CheckedNodes.Count - 1


    '            Me.ViewState(VS_ActivityId) += IIf(TVWorkspace.CheckedNodes(index1).Value.ToString = "", "", TVWorkspace.CheckedNodes(index1).Value.ToString + ",")
    '            index1 += 1

    '        Loop

    '        q = "window.open(""" + "frmReportViewer.aspx?WorkspaceId=" + Me.HProjectId.Value + "&ActivityId=" + Me.ViewState(VS_ActivityId) + "&FileExportIn=Pdf" + """)"
    '        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", q, True)
    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, eStr)

    '    End Try

    'End Sub
    Protected Sub BtnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExport.Click
        Dim str As String = ""
        Dim FileName As String = ""
        Dim isReportComplete As Boolean = False
        Dim cnt As Integer = 0

        Try


            Dim a As New ArrayList
            a.Add("sad")

            FileName = GetReportName() + ".xls"
            FileName = Server.MapPath("~/ExcelReportFile/" + FileName)

            OpenReport(FileName)

            ReportHeader()

            ReportDetail(TVWorkspace.Nodes)

            isReportComplete = True

        Catch ex As Exception
            isReportComplete = False
            Me.ShowErrorMessage(ex.Message, "")
        Finally
            If Not rPage Is Nothing Then
                rPage.CloseReport()
                rPage = Nothing
            End If
        End Try

        If isReportComplete = True Then
            ReportGeneralFunction.ExportFileAtWeb(ReportGeneralFunction.ExportFileAsEnum.ExportAsXLS, HttpContext.Current.Response, FileName)
        End If

    End Sub

    Private Function SelectAll(ByVal n As TreeNodeCollection) As Boolean
        For index As Integer = 0 To n.Count - 1
            Me.TVWorkspace.Nodes(index).Checked = True
        Next
    End Function

    Protected Sub TVWorkspace_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TVWorkspace.SelectedNodeChanged
        If Me.TVWorkspace.CheckedNodes.Count > 0 Then

            For Each node As TreeNode In Me.TVWorkspace.Nodes
                node.Checked = True
            Next

        End If
    End Sub

    Protected Sub btnExitPage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExitPage.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub



#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

    
End Class
