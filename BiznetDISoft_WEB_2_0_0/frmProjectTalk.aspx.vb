Imports System.Net.Mail
Partial Class frmProjectTalk
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_WorkSpaceId As String = "vWorkSpaceId"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_WorkspaceCommentMst As String = "WorkspaceCommentMst"
    Private Const VS_WorkspaceCommentDetail As String = "WorkspaceCommentDetail"
    Private Const VS_NodeId As String = "NodeId"
    Private Const VS_CommentId As String = "CommentId"
    Private Const VS_DsMessage As String = "ds_Message"
    Private Const VS_sortExpression As String = "sortExpression"
    Private Const VS_sortDirection As String = "sortDirection"

    Private Const GVC_ChkSelect As Integer = 0
    Private Const GVC_Title As Integer = 1
    Private Const GVC_Comment As Integer = 2
    Private Const GVC_GivenBy As Integer = 3
    Private Const GVC_GivenOn As Integer = 4
    Private Const GVC_Subject As Integer = 5
    Private Const GVC_CommentId As Integer = 6

#End Region
    
#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GenCall()
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getData", "getData();", True)
    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim ds1 As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try

           
            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_WorkSpaceId) = Me.Request.QueryString("WorkSpaceId").ToString
            Me.ViewState(VS_NodeId) = Me.Request.QueryString("NodeId").ToString
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_CommentId) = Me.Request.QueryString("Value").ToString
               
            End If

            ''''Check for Valid User''''''''''''''

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_WorkspaceCommentMst) = ds.Tables(0)   ' adding blank DataTable in viewstate

            If Not GenCall_Data1(ds1) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_WorkspaceCommentDetail) = ds1.Tables(0)   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".GenCall")

        Finally

        End Try

    End Function

#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try



            Val = Me.ViewState(VS_CommentId) 'Me.ViewState(VS_WorkSpaceId).ToString.Trim() + Me.ViewState(VS_NodeId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vCommentId = '" + Val.ToString + "'"
            End If


            If Not objHelp.GetWorkspaceCommentMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            ds_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")

        Finally

        End Try
    End Function

    Private Function GenCall_Data1(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try



            Val = Me.ViewState(VS_CommentId) 'Me.ViewState(VS_WorkSpaceId).ToString.Trim() + Me.ViewState(VS_NodeId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vCommentId = '" + Val.ToString + "'"
            End If

            If Not objHelp.GetWorkspaceCommentDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found For Selected Operation")
            End If

            ds_Retu = ds
            GenCall_Data1 = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data1")

        Finally

        End Try
    End Function

#End Region

#Region " GenCall_showUI() "
    Private Function GenCall_ShowUI() As Boolean

        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: Project Talk  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Talk"

            Choice = Me.ViewState("Choice")


            If Not FillControls() Then
                Exit Function
            End If

            If Not FillGrid("Read") Then
                Exit Function
            End If

            If Not FillGrid("UnRead") Then
                Exit Function
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
        End Try
    End Function
#End Region

#Region "FillControls"

    Private Function FillControls() As Boolean
        Dim ds_User As New Data.DataSet
        Dim ds_Project As New Data.DataSet
        Dim dt_Project As New DataTable
        Dim estr As String = String.Empty
        Dim WorkSpaceId As String = String.Empty
        Dim NodeId As String = String.Empty
        Try

            WorkSpaceId = Me.ViewState(VS_WorkSpaceId).ToString.Trim()
            NodeId = Me.ViewState(VS_NodeId).ToString.Trim()

            objHelp.GetViewProjectNodeUserRightsDetails("vWorkSpaceID='" & WorkSpaceId & "' and iNodeId=" & NodeId, _
                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Project, estr)

            If ds_Project.Tables(0).Rows.Count < 1 Then
                Me.ObjCommon.ShowAlert("No Record Found For This Activity", Me.Page)
                Return True
            End If

            dt_Project = ds_Project.Tables(0).DefaultView.ToTable(True, "vProjectNo,vActivityName".Split(","))
            Me.txtProjNo.Text = dt_Project.Rows(0).Item("vProjectNo")
            Me.txtActivityName.Text = dt_Project.Rows(0).Item("vActivityName")

            Me.chklstUser.DataSource = ds_Project.Tables(0).DefaultView.ToTable(True, "iUserId,vUserName".Split(","))
            Me.chklstUser.DataTextField = "vUserName"
            Me.chklstUser.DataValueField = "iUserId"
            Me.chklstUser.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillControls")
            Return False
        End Try
    End Function

    Private Function FillGrid(ByVal type As String) As Boolean
        Dim ds_Messages As New DataSet
        Dim dv_Messages As DataView = Nothing
        Dim estr As String = String.Empty
        Dim WorkSpaceId As String = String.Empty
        Dim NodeId As String = String.Empty
        Try
            WorkSpaceId = Me.ViewState(VS_WorkSpaceId).ToString.Trim()
            NodeId = Me.ViewState(VS_NodeId).ToString.Trim()

            objHelp.GetViewWorkspaceComments("vWorkSpaceID='" & WorkSpaceId & "' and iNodeId=" & _
                        NodeId & " and iToUserId = " & Me.Session(S_UserID), WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_Messages, estr)
            Me.ViewState(VS_DsMessage) = ds_Messages

            If type.ToUpper = "READ" Then

                If (Not ViewState(VS_sortExpression) Is Nothing) And (Not ViewState(VS_sortDirection) Is Nothing) Then
                    dv_Messages = New DataView()
                    dv_Messages = ds_Messages.Tables(0).DefaultView
                    dv_Messages.RowFilter = "cRead<>'N'"
                    dv_Messages.Sort = ViewState(VS_sortExpression).ToString & " " & ViewState(VS_sortDirection)

                    Me.Gv_Read.DataSource = dv_Messages
                    Me.Gv_Read.DataBind()

                Else

                    dv_Messages = New DataView()

                    dv_Messages = ds_Messages.Tables(0).DefaultView
                    dv_Messages.RowFilter = "cRead<>'N'"
                    If dv_Messages.Count <> 0 Then
                        Me.Gv_Read.DataSource = dv_Messages
                        Me.Gv_Read.DataBind()
                        Me.LblRead.Text = "Read Messages"

                    Else
                        Me.LblRead.Text = "No Read Messages"

                    End If

                End If

            End If
            If type.ToUpper = "UNREAD" Then

                If (Not ViewState(VS_sortExpression) Is Nothing) And (Not ViewState(VS_sortDirection) Is Nothing) Then
                    dv_Messages = New DataView()
                    dv_Messages = ds_Messages.Tables(0).DefaultView
                    dv_Messages.RowFilter = "cRead='N'"
                    dv_Messages.Sort = ViewState(VS_sortExpression).ToString & " " & ViewState(VS_sortDirection)

                    Me.GV_UnRead.DataSource = dv_Messages
                    Me.GV_UnRead.DataBind()

                Else

                    dv_Messages = New DataView()
                    dv_Messages = ds_Messages.Tables(0).DefaultView
                    dv_Messages.RowFilter = "cRead='N'"
                    If dv_Messages.Count <> 0 Then
                        Me.GV_UnRead.DataSource = dv_Messages
                        Me.GV_UnRead.DataBind()
                        Me.LblUnRead.Text = "UnRead Messages"
                        Me.btnRead.Enabled = True
                    Else
                        Me.LblUnRead.Text = "No UnRead Messages"
                        Me.btnRead.Enabled = False
                    End If

                End If

            End If
            Return True

        Catch ex As Exception
            Return False
            Me.ShowErrorMessage(ex.Message, "..........FillGrid")
        End Try
    End Function

    Private Function CreateTable() As Boolean
        Dim Dt_WSSub As New DataTable
        Dim dc As DataColumn
        Try

            dc = New DataColumn
            dc.ColumnName = "vWorkspaceId"
            dc.DataType = GetType(String)
            Dt_WSSub.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "NoOfSubjects"
            dc.DataType = GetType(Integer)
            Dt_WSSub.Columns.Add(dc)


            dc = New DataColumn
            dc.ColumnName = "iModifyBy"
            dc.DataType = GetType(Integer)
            Dt_WSSub.Columns.Add(dc)

            'Me.ViewState(VS_DtWSSub) = Dt_WSSub
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...CreateTable")
            Return False
        End Try
    End Function

#End Region

#Region "AssignValue"
    Protected Sub AssignValues()
        Dim dr As DataRow
        Dim dr1 As DataRow
        Dim dt_Mst As New DataTable
        Dim dt_Dtl As New DataTable
        Dim i As Integer
        Try

            dt_Mst = CType(Me.ViewState(VS_WorkspaceCommentMst), DataTable)
            dt_Dtl = CType(Me.ViewState(VS_WorkspaceCommentDetail), DataTable)
            'vCommentId,vWorkspaceId,iNodeId,vTitle,dSendDate,iModifyBy,dModifyOn,cStatusIndi
            'For Master
            dr = dt_Mst.NewRow()
            dr("vCommentId") = "0001"
            dr("vWorkspaceId") = Me.ViewState(VS_WorkSpaceId).ToString.Trim()
            dr("iNodeId") = Me.ViewState(VS_NodeId).ToString.Trim()
            dr("vTitle") = Me.TxtTitle.Text.Trim()
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"
            dt_Mst.Rows.Add(dr)
            Me.ViewState(VS_WorkspaceCommentMst) = dt_Mst

            For i = 0 To Me.chklstUser.Items.Count - 1
                If Me.chklstUser.Items(i).Selected = True Then
                    'vCommentId,iTranNo,iToUserId,iFromUserId,vTitle,vSubject,cRead,iModifyBy,dModifyOn,cStatusIndi
                    'For Detail
                    dr1 = dt_Dtl.NewRow()
                    dr1("vCommentId") = i
                    dr1("iTranNo") = i
                    dr1("iToUserId") = Me.chklstUser.Items(i).Value
                    dr1("iFromUserId") = Me.Session(S_UserID)
                    dr1("vSubject") = Me.TxtAComment.Value.Trim()
                    dr1("cRead") = "N"
                    'dr1("cRead") = "SA"
                    dr1("iModifyBy") = Me.Session(S_UserID)
                    dr1("cStatusIndi") = "N"
                    dt_Dtl.Rows.Add(dr1)
                End If
            Next
            Me.ViewState(VS_WorkspaceCommentDetail) = dt_Dtl
        Catch ex As Exception
            Me.ObjCommon.ShowAlert("Error while Assigning Values", Me.Page)
        End Try

    End Sub
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Dim i As Integer
        'Me.txtProjNo.Text = ""
        'Me.txtActivityName.Text = ""
        Me.TxtAComment.Value = ""
        Me.TxtTitle.Text = ""
        For i = 0 To Me.chklstUser.Items.Count - 1
            Me.chklstUser.Items(i).Selected = False
        Next
        Me.ViewState(VS_DsMessage) = Nothing
        Me.ViewState(VS_WorkspaceCommentMst) = Nothing
        Me.ViewState(VS_WorkspaceCommentDetail) = Nothing
        'Me.ViewState(VS_NodeId) = Nothing
        'Me.ViewState(VS_CommentId) = Nothing
        'Me.Response.Redirect("frmProjectTalk.aspx?mode=1&WorkSpaceId=" & Me.ViewState(VS_WorkSpaceId) & "&NodeId=" & Me.ViewState(VS_NodeId))
        GenCall()
    End Sub
#End Region

#Region "Grid for Read and Unread"

    Protected Sub GV_UnRead_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_UnRead.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("LnkMsg"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("LnkMsg"), LinkButton).CommandName = "ReadComment"
            'CType(e.Row.FindControl("LnkMsg"), LinkButton).OnClientClick = " return ShowDiv(event,'divMsg','block');"
        End If
    End Sub

    Protected Sub Gv_Read_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Gv_Read.RowCommand
        Dim Index As Integer = CInt(e.CommandArgument)

        If e.CommandName.ToUpper = "READCOMMENT" Then

            Me.lblMsg.Text = GV_UnRead.Rows(Index).Cells(GVC_Subject).Text
            Me.lblMsg.Visible = True
            Me.divMsg.Visible = True
        End If
    End Sub

    Protected Sub Gv_Read_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gv_Read.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("LnkReadMsg"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("LnkReadMsg"), LinkButton).CommandName = "ReadComment"

            CType(e.Row.FindControl("LblSr"), Label).Text = e.Row.RowIndex + 1

            'CType(e.Row.FindControl("LnkReadMsg"), LinkButton).OnClientClick = " return ShowDiv(event,'divMsg','block');"
        End If
    End Sub

    Protected Sub GV_UnRead_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GV_UnRead.Sorting
        If ViewState(VS_sortExpression) Is Nothing Then
            ViewState(VS_sortExpression) = e.SortExpression
        End If

        If ViewState(VS_sortDirection) Is Nothing Then
            ViewState(VS_sortDirection) = "ASC"
        Else
            If ViewState(VS_sortExpression).ToString <> e.SortExpression Then
                ViewState(VS_sortExpression) = e.SortExpression
                ViewState(VS_sortDirection) = "ASC"
            Else
                If ViewState(VS_sortDirection).ToString.Contains("ASC") Then
                    ViewState(VS_sortDirection) = "DESC"
                Else
                    ViewState(VS_sortDirection) = "ASC"
                End If
            End If

        End If
        FillGrid("UnRead")
    End Sub

    Protected Sub Gv_Read_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles Gv_Read.Sorting
        If ViewState(VS_sortExpression) Is Nothing Then
            ViewState(VS_sortExpression) = e.SortExpression
        End If

        If ViewState(VS_sortDirection) Is Nothing Then
            ViewState(VS_sortDirection) = "ASC"
        Else
            If ViewState(VS_sortExpression).ToString <> e.SortExpression Then
                ViewState(VS_sortExpression) = e.SortExpression
                ViewState(VS_sortDirection) = "ASC"
            Else
                If ViewState(VS_sortDirection).ToString.Contains("ASC") Then
                    ViewState(VS_sortDirection) = "DESC"
                Else
                    ViewState(VS_sortDirection) = "ASC"
                End If
            End If

        End If
        FillGrid("Read")
    End Sub

    Protected Sub GV_UnRead_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim Index As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "READCOMMENT" Then
            Me.lblMsg.Text = GV_UnRead.Rows(Index).Cells(GVC_Subject).Text
            Me.divMsg.Visible = True
        End If
    End Sub

    Protected Sub Gv_Read_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(GVC_Subject).Visible = False
        e.Row.Cells(GVC_CommentId).Visible = False
    End Sub

    Protected Sub GV_UnRead_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(GVC_Subject).Visible = False
        e.Row.Cells(GVC_CommentId).Visible = False
    End Sub

#End Region

#Region "Buttons Events"

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        ResetPage()
    End Sub

    Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSend.Click
        Dim Ds_Save As New DataSet
        Dim Dt_WSCM As New DataTable
        Dim Dt_WSCD As New DataTable
        Dim estr As String = String .Empty 
        Dim ChkLstCnt, i As Integer
        Dim SelectedUserIdList As New ListItemCollection
        Dim selectedUserId As String = String.Empty
        Dim TotalCnt As Integer = 0
        Dim Emailcount, index As Integer
        Dim Wstr As String = String.Empty
        Dim MultipleEmail As String = String.Empty
        Dim Ds_Email As DataSet
        Dim Dv_Email As DataView
        Dim SubjectLine As String = String.Empty

        Try
            ChkLstCnt = chklstUser.Items.Count
            For i = 0 To ChkLstCnt - 1 Step 1

                If chklstUser.Items(i).Selected = True Then

                    SelectedUserIdList.Insert(TotalCnt, chklstUser.Items(i).Value.ToString().Trim())
                    TotalCnt = TotalCnt + 1

                End If

            Next

            For i = 0 To SelectedUserIdList.Count - 1 Step 1

                If i = SelectedUserIdList.Count - 1 Then

                    selectedUserId += SelectedUserIdList.Item(i).Value().ToString().Trim()

                Else

                    selectedUserId += SelectedUserIdList.Item(i).Value().ToString().Trim() & ","

                End If

            Next

            Wstr = "iUserId in(" & selectedUserId & ") And cStatusIndi <> 'D'"
            Ds_Email = New DataSet
            If Not objHelp.getuserMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_Email, estr) Then
                Exit Sub
            End If
            Dv_Email = Ds_Email.Tables(0).DefaultView.ToTable(True, "vEmailId").DefaultView()

            Emailcount = Dv_Email.Count
            For index = 0 To Emailcount - 1

                If MultipleEmail.Trim() <> "" AndAlso _
                                Convert.ToString(Dv_Email.Item(index)("vEmailId")).Trim() <> "" Then
                    MultipleEmail += ","
                End If

                MultipleEmail += Convert.ToString(Dv_Email.Item(index)("vEmailId")).Trim()

            Next
            If MultipleEmail = String.Empty Then
                ObjCommon.ShowAlert("E-mail Id not available for any of the selected user", Me.Page)
            Else
                SubjectLine = "<html><table><tr><td align=left>Project No :</td><td align=left>"
                SubjectLine += txtProjNo.Text.Trim()
                SubjectLine += "</td></tr><tr><td align=left>Activity Name :</td><td align=left>"
                SubjectLine += txtActivityName.Text.Trim()
                SubjectLine += "</td></tr><tr><td align=left>Comment :</td><td align=left>"
                SubjectLine += TxtAComment.Value.Trim()
                SubjectLine += "</td></tr></table></html>"
                SendMail(SubjectLine, MultipleEmail, TxtTitle.Text.ToString().Trim())
                Me.ObjCommon.ShowAlert("Mail Sent", Me)
            End If

            AssignValues()

            Ds_Save = New DataSet
            Dt_WSCM = CType(Me.ViewState(VS_WorkspaceCommentMst), DataTable)
            Dt_WSCM.TableName = "WorkspaceCommentMst"
            Ds_Save.Tables.Add(Dt_WSCM)

            Dt_WSCD = CType(Me.ViewState(VS_WorkspaceCommentDetail), DataTable)
            Dt_WSCD.TableName = "WorkspaceCommentDetail"
            Ds_Save.Tables.Add(Dt_WSCD)

            If Not objLambda.Save_InsertWorkspaceComments(Me.ViewState(VS_Choice), Ds_Save, Me.Session(S_UserID), estr) Then

                ObjCommon.ShowAlert("Error While Sending ", Me.Page)
                Exit Sub

            End If

            ObjCommon.ShowAlert("Comment Send SuccessFully", Me.Page)
            
            ResetPage()

        Catch ex As Exception
            ObjCommon.ShowAlert("Error While Saving WorkSpaceComments", Me.Page)
        End Try
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.divMsg.Visible = False
    End Sub

    Protected Sub btnRead_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_Mst As New DataSet
        Dim ds_Detail As New DataSet
        Dim ds_Save As New DataSet
        Dim dt_Mst As New DataTable
        Dim dt_Dtl As New DataTable
        Dim dr As DataRow
        Dim Index As Integer
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim WorkSpaceId As String = String.Empty
        Dim NodeId As String = String.Empty
        Try
            WorkSpaceId = Me.ViewState(VS_WorkSpaceId).ToString.Trim()
            NodeId = Me.ViewState(VS_NodeId).ToString.Trim()

            wstr = "vWorkSpaceID='" & WorkSpaceId & "' and iNodeId=" & NodeId

            objHelp.GetWorkspaceCommentMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Mst, estr)
            objHelp.Proc_GetWorkspaceCommentDetail(WorkSpaceId, NodeId, Me.Session(S_UserID), ds_Detail, estr)
            dt_Dtl = ds_Detail.Tables(0)
            dt_Mst = ds_Mst.Tables(0)

            For Index = 0 To Me.GV_UnRead.Rows.Count - 1

                If CType(Me.GV_UnRead.Rows(Index).FindControl("ChkSelect"), CheckBox).Checked = True Then
                    For Each dr In dt_Dtl.Rows
                        If dr("vCommentId") = Me.GV_UnRead.Rows(Index).Cells(GVC_CommentId).Text Then
                            dr("cRead") = "Y"
                            dr("iModifyBy") = Me.Session(S_UserID)
                            dr.AcceptChanges()
                        End If
                    Next
                End If

            Next
            dt_Dtl.AcceptChanges()

            ds_Save = New DataSet
            dt_Mst.TableName = "WorkspaceCommentMst"
            ds_Save.Tables.Add(dt_Mst.Copy())

            dt_Dtl.TableName = "WorkspaceCommentDetail"
            ds_Save.Tables.Add(dt_Dtl.Copy())
            If Not objLambda.Save_InsertWorkspaceComments(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Save, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving WorkSpaceComments", Me.Page)
                Exit Sub
            Else
                ObjCommon.ShowAlert("Comment Marked as Read", Me.Page)
                ResetPage()

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnRead_Click")
        End Try
    End Sub

    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        Dim DMS As String = String.Empty
        Me.Response.Redirect(Me.Request.QueryString("Page2").Trim() & ".aspx?WorkSpaceId=" & Me.Request.QueryString("WorkSpaceId").Trim() & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))
    End Sub

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
    Protected Sub SendMail(ByVal Body As String, ByVal EmailId As String, ByVal SubjectForEmail As String)
        Dim fromEmailId As String = String.Empty
        Dim toEmailId As String = String.Empty
        Dim password As String = String.Empty
        Dim ccEmailId As String = String.Empty
        Dim smtp As New SmtpClient
        Dim mailmsg As New MailMessage
        Dim wStr As String = String.Empty
        Dim StrTo() As String
        Dim StrCC() As String

        Try
            If EmailId = "" Then
                Me.ObjCommon.ShowAlert("There is no any e-mail Id exist, at least one E-mail Should be there", Me)
            Else
                fromEmailId = ConfigurationSettings.AppSettings("Username")
                password = ConfigurationSettings.AppSettings("Password")
                smtp = New SmtpClient
                smtp.Credentials = New Net.NetworkCredential(fromEmailId, password)
                smtp.EnableSsl = ConfigurationSettings.AppSettings("SslValue") '"smtp.gmail.com"
                smtp.Host = ConfigurationSettings.AppSettings("smtpServer") '"smtp.gmail.com"
                smtp.Port = ConfigurationSettings.AppSettings("ServerPort") '587
                mailmsg = New MailMessage
                mailmsg.IsBodyHtml = True
                mailmsg.From = New MailAddress(fromEmailId)

                StrTo = EmailId.Trim.Split(",")
                For count As Integer = 0 To StrTo.Length - 1
                    mailmsg.To.Add(New MailAddress(StrTo(count).Trim()))
                Next

                mailmsg.Subject = SubjectForEmail
                mailmsg.Body = Body
                smtp.Send(mailmsg)

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....SendMail")
        End Try

    End Sub
    'Add by shivani pandya for Project lock
#Region "LockImpact"
    <Services.WebMethod()> _
    Public Shared Function LockImpact(ByVal WorkspaceID As String) As String
        Dim ds_Check As DataSet = Nothing
        Dim dtProjectStatus As New DataTable
        Dim ProjectStatus As String = String.Empty
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim iTran As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

        wStr = "vWorkspaceId = '" + WorkspaceID.ToString() + "' And cStatusIndi <> 'D'"

        If Not objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, eStr) Then
            Throw New Exception(eStr)
        End If

        If ds_Check.Tables(0).Rows.Count > 0 Then

            dtProjectStatus = ds_Check.Tables(0)
            iTran = dtProjectStatus.Compute("Max(iTranNo)", String.Empty)
            dtProjectStatus.DefaultView.RowFilter = "iTranNo =" + iTran.ToString()

            If dtProjectStatus.DefaultView.ToTable.Rows.Count > 0 Then
                ProjectStatus = dtProjectStatus.DefaultView.ToTable().Rows(0)("cLockFlag").ToString()
            End If
        Else
            ProjectStatus = "U"
        End If

        Return ProjectStatus
    End Function
#End Region

End Class
