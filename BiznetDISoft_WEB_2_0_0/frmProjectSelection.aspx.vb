
Partial Class frmProjectSelection
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

#End Region

#Region "Form Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.HideMenu()
            If Not GenCall_ShowUI() Then
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region " GenCall_showUI() "

    Private Function GenCall_ShowUI() As Boolean

        Page.Title = ":: Project Selection  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        Try
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Selection"
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Not ViewProjectSummary() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.Session.Add("WorkspaceId", Me.HProjectId.Value.Trim())
            Me.Response.Redirect("frmMainPage.aspx")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "ViewProjectSummary"

    Private Function ViewProjectSummary() As Boolean
        Dim workSpaceId As String = ""
        Dim dt_ProjectSummary As New DataTable
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim ds_ProjectsDetail As New DataSet
        Dim strProjectSummary As String = ""

        Try
            wStr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "'"
            If Not ObjHelp.GetViewgetWorkspaceDetailForHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_ProjectsDetail, eStr) Then
                ObjCommon.ShowAlert("Error While Getting Data From View_getWorkspaceDetailForHdr : " + eStr, Me)
                Exit Function
            End If

            dt_ProjectSummary = ds_ProjectsDetail.Tables(0)
            strProjectSummary = "<table style=""font-size:9pt"" width=""100%"" align=""center"" cellpadding=""2px""><tr><td colspan=""4""><strong>Project Details</strong><hr></td></tr>"

            strProjectSummary += "<tr><td align=""right"" style=""width:25%"">Project No:</td><td style=""width:25%"" align=""left"">" & dt_ProjectSummary.Rows(0)("vProjectNo").ToString & "</td>" & _
                                 "<td align=""right"" >Drug:</td><td  align=""left""> " & dt_ProjectSummary.Rows(0)("vDrugName").ToString & "</td></tr>" & _
                                 "<tr><td align=""right"" >Sponsor:</td><td align=""left"">" & dt_ProjectSummary.Rows(0)("vClientName").ToString & "</td>" & _
                                 "<td align=""right"">Submissions:</td><td align=""left"">" & dt_ProjectSummary.Rows(0)("vRegionName").ToString & "</td></tr>" & _
                                 "<tr><td align=""right"" >No. of Subjects:</td><td align=""left"" >" & dt_ProjectSummary.Rows(0)("iNoOfSubjects").ToString & "</td></tr>" & _
                                 "<tr><td align=""right"">Project Manager:</td><td align=""left"">" & dt_ProjectSummary.Rows(0)("vProjectManager").ToString & "</td>" & _
                                 "<td align=""right"">Project Co-ordinator:</td><td align=""left"">" & dt_ProjectSummary.Rows(0)("vProjectCoordinator").ToString & "</td></tr>"

            strProjectSummary += "</table><hr>"
            lblProjectSummary.Text = strProjectSummary

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "HideMenu"

    Private Sub HideMenu()
        Dim tmpMenu As Menu
        tmpMenu = CType(Me.Master.FindControl("menu1"), Menu)

        If Not tmpMenu Is Nothing Then
            'tmpMenu.Items.Clear()
            tmpMenu.Visible = False
        End If
    End Sub

#End Region

End Class
