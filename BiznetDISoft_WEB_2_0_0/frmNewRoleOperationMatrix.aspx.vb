
Partial Class frmNewRoleOperationMatrix
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Choice As WS_Lambda.DataObjOpenSaveModeEnum
    Private MasterEntry As WS_Lambda.MasterEntriesEnum

    Private oRetMess As String = String.Empty
    Private sHelpFile As String = String.Empty
    Private NodeItem As String = String.Empty
    Private NodeNo As Integer = 0

    Private dt As New DataTable()
    Private intNodeFound As Boolean
    Private estr_retu As String = String.Empty

    Private Const VS_OpMst As String = "dtOprtnmst"
    Private Const VS_Memo As String = "dtMemoDocument"
    Private Const VS_dtRoleOperMtx As String = "dtRoleOperationMatrix"
    Private Const VS_dtRoleOperMtxHistory As String = "dtRoleOperationMatrixHistory"
    Private Const VS_dtRoleAudit As String = "CurrentSubject"
    Private Const VS_dtRoleReport As String = "RoleReport"

#End Region

#Region "TreeTypeEnum "

    Private Enum TreeTypeEnum
        RoleOperation = 0
        'MemoDocumentRight = 1
        'Both = 2
    End Enum

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = ":: RoleOperation Matrix  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Not IsPostBack Then
            Me.rbtnlstApplicationType.Items(0).Selected = True
            Me.rbtnlselecttype.Items(0).Selected = True
            Me.GenCall()
        End If

    End Sub

#End Region

#Region "GENCALL "

    Private Function GenCall() As Boolean
        Try

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Role Operation Matrix"

            If Not Me.GenCall_Data() Then
                Exit Function
            End If
            If Not Me.fillRole() Then
                Exit Function
            End If
            Me.fillTreeTable()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..GenCall")
        End Try
    End Function

#End Region

#Region "GENCALL_DATA "

    Private Function GenCall_Data() As Boolean
        Dim dsRoleOperMtx As DataSet = Nothing
        Dim eStr As String = String.Empty

        Try



            If Not Me.objhelpDb.getroleOperationMatrix("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                         dsRoleOperMtx, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From RoleOperationMatrix", estr_retu)
                Exit Function
            End If

            If dsRoleOperMtx Is Nothing Then
                Throw New Exception(oRetMess)
            End If

            Me.ViewState(VS_dtRoleOperMtx) = dsRoleOperMtx.Tables(0)

            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage("...GenCall_Data", estr_retu)
        End Try
    End Function

#End Region

#Region "Fill Role "

    Private Function fillRole() As Boolean
        Dim oDs As New DataSet
        Try
            If Not Me.objhelpDb.getUserTypeMst("cstatusindi <> 'D' Order By vUserTypeName ASC", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, oDs, estr_retu) Then
                Me.ShowErrorMessage("Error While Getting Data From UsertypeMst", estr_retu)
                Exit Function
            End If

            Me.ddlRole.DataSource = Nothing
            Me.ddlRole.DataSource = oDs
            Me.ddlRole.DataTextField = "vUserTypeName"
            Me.ddlRole.DataValueField = "vUserTypeCode"
            Me.ddlRole.DataBind()
            Me.ddlRole.Items.Insert(0, New ListItem("Please Select User", "0"))
            Me.ddlRole.SelectedIndex = 0

            ' added by prayag for Role_Report
            Me.ddlProfile.DataSource = Nothing
            Me.ddlProfile.DataSource = oDs
            Me.ddlProfile.DataTextField = "vUserTypeName"
            Me.ddlProfile.DataValueField = "vUserTypeCode"
            Me.ddlProfile.DataBind()
            Me.ddlProfile.SelectedIndex = 0

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillRole")
        End Try
    End Function

#End Region

#Region "FillTree, Other Events & Fucnction "

    Private Sub fillTreeTable()
        Dim oDs As New DataSet
        Dim wStr As String = ""
        Try



            wStr = "cStatusIndi <> 'D'"
            If Me.rbtnlstApplicationType.SelectedValue = "0" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_BizNetWeb & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "1" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_BizNetDesktop & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "2" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_BizNetLIMS & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "3" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_Biolyte & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "4" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_PharmacyManagement & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "5" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_MedicalImaging & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "6" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_SDTM & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "7" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_OIMS & "'"
            End If


            If Not Me.objhelpDb.getOperationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                oDs, estr_retu) Then

                Me.ShowErrorMessage("Error While Getting Data From OperationMst", estr_retu)
                Exit Sub

            End If

            Me.ViewState(VS_OpMst) = oDs.Tables(0)
            Me.trVwrpt.Nodes.Clear()
            NodeNo = 1
            Me.TestLast("0", New TreeNode(), TreeTypeEnum.RoleOperation)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillTreeTable")
        End Try
    End Sub

    Private Sub TestLast(ByVal parentId As String, ByVal tn As TreeNode, ByVal TrvType As TreeTypeEnum)
        Dim dr() As DataRow
        Dim dt_new As DataTable
        Dim parentNode As New TreeNode

        Try



            dt = CType(Me.ViewState(VS_OpMst), DataTable)
            dt_new = dt.Copy()

            For Each dr_change As DataRow In dt_new.Rows

                If dr_change("vParentOperationCode") = -999 Then
                    dr_change("vParentOperationCode") = "0"
                    dr_change.AcceptChanges()
                End If

            Next dr_change

            dt_new.AcceptChanges()

            dr = dt_new.Select("vParentOperationCode=" & parentId)

            For index As Integer = 0 To dr.Length - 1

                parentNode = New TreeNode
                parentNode.Text = dr(index)("vOperationName").ToString  '& "(" & dr(index)("iNodeId") & ")" & "(" & dr(index)("iNodeId") & ")"
                parentNode.Value = dr(index)("vOperationCode").ToString
                If dt_new.Select("vParentOperationCode=" + dr(index)("vOperationCode") + "").Length = 0 Then

                    NodeNo += 1
                    parentNode.ToolTip = NodeNo

                ElseIf NodeNo <> 1 Then ' ElseIf condition adeed by prayag for random issue on 17-March-2016
                    NodeNo += 1
                    parentNode.ToolTip = NodeNo
                End If

                If parentId = "0" Then
                    trVwrpt.Nodes.Add(parentNode)
                Else
                    tn.ChildNodes.Add(parentNode)
                End If

                TestLast(dr(index)("vOperationCode"), parentNode, TrvType)

            Next index

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...TestLast")
        End Try
    End Sub

    Private Function ResetAll() As Boolean
        Dim index1 As Integer
        Dim index2 As Integer
        Dim index3 As Integer

        Me.gvwUserTypeaudit.DataSource = Nothing ' added by prayag
        Me.gvwUserTypeaudit.DataBind()


        Try


            For index1 = 0 To trVwrpt.Nodes.Count - 1

                For index2 = 0 To trVwrpt.Nodes(index1).ChildNodes.Count - 1

                    For index3 = 0 To trVwrpt.Nodes(index1).ChildNodes(index2).ChildNodes.Count - 1

                        If trVwrpt.Nodes.Item(index1).ChildNodes(index2).ChildNodes(index3).Checked = True Then

                            trVwrpt.Nodes.Item(index1).ChildNodes(index2).ChildNodes(index3).Checked = False
                            trVwrpt.Nodes.Item(index1).ChildNodes(index2).Expanded = False

                        End If

                    Next index3

                Next index2

                trVwrpt.Nodes.Item(index1).Expanded = False

            Next index1
            trVwrpt.Nodes.Clear()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "ClearItem", "ClearItem('0');", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

    Private Sub FindNodeByValue(ByVal n As TreeNodeCollection, ByVal Val As String)
        Try



            If (intNodeFound) Then
                intNodeFound = False

            End If

            For i As Integer = 0 To n.Count - 1

                If n(i).Value = Val Then
                    If n(i).ToolTip <> "" Then
                        NodeItem = NodeItem + CType(n(i).ToolTip, String) + ","
                    End If

                    n(i).Checked = True
                    n(i).Expand()
                    ExpandTreeNodelevel(n(i))
                    intNodeFound = True
                Else
                    intNodeFound = False 'Added
                    FindNodeByValue(n(i).ChildNodes, Val)
                End If
                If (intNodeFound) Then
                    intNodeFound = False
                    Return
                End If

            Next

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FindNodeByValue")
        End Try
    End Sub

    Private Function ExpandTreeNodelevel(ByVal CurrentNode As TreeNode) As Boolean
        Dim TreeNode1 As New TreeNode
        Dim Index As Integer = 0

        Try


            If CurrentNode.Parent Is Nothing Then
                TreeNode1 = CurrentNode
            Else
                TreeNode1 = CurrentNode.Parent
            End If

            While Not TreeNode1 Is Nothing

                TreeNode1.Expand()
                Return True
            End While

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....ExpandTreeNodelevel")
        End Try
    End Function

    Private Function ConvertDsuserTOreport(ByVal ds As DataSet) As String
        ' develope by prayag patel
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet
        Dim Status As String = String.Empty
        Try
            Status = "Role Report"
            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(Status)
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....ConvertDsuserTO")
            Return ""
        End Try
    End Function

    Private Function ConvertDsuserTOAudit(ByVal ds As DataSet) As String
        ' added by prayag patel
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet
        Dim Status As String = String.Empty
        Try
            Status = "Audit Trail OF :" + Me.ddlRole.SelectedItem.Text.Trim()
            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""6""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(Status)
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "cOperationType,UserType,ChildName,vStatus,dModifyOn,dModifyOff,vRemarkActive,vRemarkInActive,ModifyBy".Split(",")).DefaultView.ToTable())
            dsConvert.AcceptChanges()
            dsConvert.Tables(0).Columns(0).ColumnName = "App.Type"
            dsConvert.Tables(0).Columns(1).ColumnName = "Profile"
            dsConvert.Tables(0).Columns(2).ColumnName = "Operation"
            dsConvert.Tables(0).Columns(3).ColumnName = "Status"
            dsConvert.Tables(0).Columns(4).ColumnName = "FromDate"
            dsConvert.Tables(0).Columns(5).ColumnName = "ToDate"
            dsConvert.Tables(0).Columns(6).ColumnName = "RemarkActive"
            dsConvert.Tables(0).Columns(7).ColumnName = "RemarkInActive"
            dsConvert.Tables(0).Columns(8).ColumnName = "ModifyBy"
            dsConvert.AcceptChanges()

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

                strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    strMessage.Append("<td><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")

                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....ConvertDsuserTO")
            Return ""
        End Try
    End Function
#End Region

#Region "ListBox Event "

    Protected Sub ddlRole_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRole.SelectedIndexChanged
        Dim ds As New DataSet
        Dim dvMenu As New DataView
        Dim qstr As String = String.Empty

        Try
            Me.ResetAll()

            ds = Me.objhelpDb.ProcedureExecute("dbo.Proc_getexistinguser", ddlRole.SelectedValue.ToString)

            If ds Is Nothing Then
                objCommon.ShowAlert("No User Exist For Selected User Type", Me)
                Me.ddlRole.SelectedIndex = 0
                Exit Sub
            End If

            If Convert.ToInt32(ds.Tables(0).Rows(0)("CNT")) = 0 Then
                objCommon.ShowAlert("No User Exist For Selected User Type!", Me)
                Me.ddlRole.SelectedIndex = 0
                Exit Sub
            End If

            Me.fillTreeTable()

            If Me.ddlRole.SelectedValue = 0 Then
                Exit Sub
            End If

            ds = Me.objhelpDb.GetMenu(ddlRole.SelectedValue.ToString)

            'Added on 09-Jul-2009 as suggested by Nikur Sir 
            dvMenu = ds.Tables(0).Copy().DefaultView
            'dvMenu.RowFilter = "cOperationType='" & GeneralModule.OpType_BizNetWeb & "'"

            'Added on 11-Jan-2010 For Managing desktop application menu items as well -By Chandresh Vanker
            If Me.rbtnlstApplicationType.Items(0).Selected Then
                dvMenu.RowFilter = "cOperationType='" & GeneralModule.OpType_BizNetWeb & "'"
            ElseIf Me.rbtnlstApplicationType.Items(1).Selected Then
                dvMenu.RowFilter = "cOperationType='" & GeneralModule.OpType_BizNetDesktop & "'"
            ElseIf Me.rbtnlstApplicationType.Items(2).Selected Then
                dvMenu.RowFilter = "cOperationType='" & GeneralModule.OpType_BizNetLIMS & "'"
            ElseIf Me.rbtnlstApplicationType.Items(3).Selected Then
                dvMenu.RowFilter = "cOperationType='" & GeneralModule.OpType_Biolyte & "'"
            ElseIf Me.rbtnlstApplicationType.Items(4).Selected Then
                dvMenu.RowFilter = "cOperationType='" & GeneralModule.OpType_PharmacyManagement & "'"
            ElseIf Me.rbtnlstApplicationType.Items(5).Selected Then
                dvMenu.RowFilter = "cOperationType='" & GeneralModule.OpType_MedicalImaging & "'"
            ElseIf Me.rbtnlstApplicationType.Items(6).Selected Then
                dvMenu.RowFilter = "cOperationType='" & GeneralModule.OpType_SDTM & "'"
            ElseIf Me.rbtnlstApplicationType.Items(7).Selected Then
                dvMenu.RowFilter = "cOperationType='" & GeneralModule.OpType_OIMS & "'"
            End If
            '********************************

            ds = Nothing
            ds = New DataSet
            ds.Tables.Add(dvMenu.ToTable.Copy())
            '**********************************************************
            NodeItem = ""
            For index As Integer = 0 To ds.Tables(0).Rows.Count - 1

                FindNodeByValue(trVwrpt.Nodes, ds.Tables(0).Rows(index).Item(0).ToString)

            Next
            If NodeItem.Length = 0 Then
                objCommon.ShowAlert("No Rights Found For The Selected User", Me.Page)
                Exit Try
            End If
            NodeItem = NodeItem.Substring(0, NodeItem.Length - 1)
            ''commented by prayag for random issue this script get randomly more rights 
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "checkItem", "checkItem('" + CType(NodeItem, String) + "');", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ddlRole_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "Save Button "

    Protected Sub btnSaveremarks_Click(sender As Object, e As EventArgs) Handles btnSaveremarks.Click
        Dim ds_save As New DataSet
        Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
        MasterEntry = WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_roleOperationMatrix
        Dim ds As New DataSet
        Try

            If Not AssignNodeByValue() Then
                Exit Sub
            End If

            ds_save.Tables.Add(CType(Me.ViewState(VS_dtRoleOperMtx), DataTable).Copy)
            ds_save.Tables(0).TableName = "ROLEOPERATIONMATRIX"

            ds_save.Tables.Add(CType(Me.ViewState(VS_dtRoleOperMtxHistory), DataTable).Copy)
            ds_save.Tables(1).TableName = "ROLEOPERATIONMATRIXHiSTORY"

            If ds_save.Tables(0).Rows.Count = 0 Then
                Me.objCommon.ShowAlert("Please Select Atleast One Operation", Me)
                Exit Sub
            End If

            If Not Me.objLambda.Save_InsertroleOperationMatrix(Choice, MasterEntry, ds_save, Me.Session(S_UserID).ToString, estr_retu) Then

                Me.objCommon.ShowAlert("Error Occured While Saving In Roleoperationmatrix", Me)
                Exit Sub

            End If
            ModalRemarks.Hide()
            txtRemarks.Text = String.Empty
            objCommon.ShowAlert("Role Assigned Successfully !", Me)
            Me.GenCall_Data()
            Me.ddlRole.SelectedIndex = 0
            Me.ResetAll()
            Me.fillTreeTable()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSave_Click")
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clearemarks", "validatecancel()", True)
        ModalRemarks.Show()
        ' added and commented  by prayag for audit trail and remarks (code apply at save remarks button)

        'Dim ds_save As New DataSet
        'Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
        'MasterEntry = WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_roleOperationMatrix
        'Dim ds As New DataSet
        'Try

        '    If Not AssignNodeByValue() Then
        '        Exit Sub
        '    End If

        '    ds_save.Tables.Add(CType(Me.ViewState(VS_dtRoleOperMtx), DataTable).Copy)
        '    ds_save.Tables(0).TableName = "ROLEOPERATIONMATRIX"

        '    If ds_save.Tables(0).Rows.Count = 0 Then
        '        Me.objCommon.ShowAlert("Please Select Atleast One Operation", Me)
        '        Exit Sub
        '    End If

        '    If Not Me.objLambda.Save_InsertroleOperationMatrix(Choice, MasterEntry, ds_save, Me.Session(S_UserID).ToString, estr_retu) Then

        '        Me.objCommon.ShowAlert("Error Occured While Saving In Roleoperationmatrix", Me)
        '        Exit Sub

        '    End If

        '    objCommon.ShowAlert("Roll Assigned Successfully", Me)
        '    Me.GenCall_Data()
        '    Me.ddlRole.SelectedIndex = 0
        '    Me.ResetAll()
        '    Me.fillTreeTable()

        'Catch ex As Exception
        '    Me.ShowErrorMessage(ex.Message, "...btnSave_Click")
        'End Try
    End Sub

    Private Function AssignNodeByValue() As Boolean
        Dim j As Integer = 0
        Dim cnt As Integer = 0
        Dim CountNode As New TreeNode
        Dim CountNode1 As New TreeNode
        Dim StrGroup(6) As String
        Dim dt As DataTable
        Dim dv As New DataView
        Dim ds_roleoperation As New DataSet
        Dim drow As DataRow
        Dim Find As Boolean = False

        Dim dv_ParentNode As New DataView
        Dim Dt_New As New DataTable
        'Dim dr_find() As DataRow
        Dim ds_newroleoperation As New DataSet

        Dim ds_newroleoperationhistory As New DataSet
        Dim dthistory As DataTable
        Dim dvhistory As New DataView
        Dim wStr As String = String.Empty
        Try



            StrGroup(0) = "nRoleOperationNo"
            StrGroup(1) = "vUserTypeCode"
            StrGroup(2) = "vOperationCode"
            StrGroup(3) = "cActiveFlag"
            StrGroup(4) = "iModifyBy"
            StrGroup(5) = "dModifyOn"
            StrGroup(6) = "cStatusIndi"

            wStr = "cStatusIndi <> 'D' and cActiveflag = 'Y' And vUserTypeCode= '" + Me.ddlRole.SelectedValue.ToString + "'"
            If Me.rbtnlstApplicationType.SelectedValue = "0" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_BizNetWeb & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "1" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_BizNetDesktop & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "2" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_BizNetLIMS & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "3" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_Biolyte & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "4" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_PharmacyManagement & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "5" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_MedicalImaging & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "6" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_SDTM & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "7" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_OIMS & "'"
            End If

            If Not Me.objhelpDb.GetViewRoleOperationMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_roleoperation, estr_retu) Then
                Me.ShowErrorMessage("", estr_retu)
                Exit Function
            End If

            For Each dr_old As DataRow In ds_roleoperation.Tables(0).Rows
                dr_old("cStatusIndi") = "D"
                dr_old("cActiveFlag") = "N"
                dr_old.AcceptChanges()
            Next dr_old

            ds_roleoperation.Tables(0).AcceptChanges()

            For Index As Integer = 0 To trVwrpt.CheckedNodes.Count - 1
                CountNode = trVwrpt.CheckedNodes(Index)
                Find = False

                For cnt = 0 To ds_roleoperation.Tables(0).Rows.Count - 1

                    If ds_roleoperation.Tables(0).Rows(cnt)("vOperationCode") = CountNode.Value.ToString Then

                        ds_roleoperation.Tables(0).Rows(cnt)("cStatusIndi") = "E"
                        ds_roleoperation.Tables(0).Rows(cnt)("cActiveFlag") = "Y"
                        ds_roleoperation.Tables(0).Rows(cnt)("iModifyBy") = Me.Session(S_UserID).ToString
                        ds_roleoperation.Tables(0).Rows(cnt)("dModifyOn") = Now.Date()

                        Find = True
                        Exit For
                    End If

                Next cnt

                If Find = False Then
                    drow = ds_roleoperation.Tables(0).NewRow()
                    drow("nRoleOperationNo") = -1
                    drow("vUserTypeCode") = ddlRole.SelectedValue
                    drow("vOperationCode") = CountNode.Value
                    drow("cStatusIndi") = "N"
                    drow("cActiveFlag") = "Y"
                    drow("iModifyBy") = Me.Session(S_UserID).ToString
                    drow("dModifyOn") = Now.Date()
                    ds_roleoperation.Tables(0).Rows.Add(drow)
                    ds_roleoperation.Tables(0).AcceptChanges()

                    'ds_roleoperation = HasParentNode(CountNode, ds_roleoperation)

                End If

            Next Index

            ds_newroleoperationhistory = ds_roleoperation.Copy()
            dvhistory = ds_newroleoperationhistory.Tables(0).DefaultView
            dvhistory.RowFilter = "cStatusIndi='N' or cstatusindi = 'D'"

            For Each dr_old In dvhistory
                dr_old.BeginEdit()
                dr_old.Item("iModifyBy") = Me.Session(S_UserID).ToString
                dr_old.EndEdit()
            Next

            dthistory = dvhistory.ToTable(True, StrGroup)
            Dim vRemark As New Data.DataColumn("vRemark", GetType(System.String))
            vRemark.DefaultValue = Me.txtRemarks.Text
            dthistory.Columns.Add(vRemark)



            Me.ViewState(VS_dtRoleOperMtxHistory) = dthistory


            'new code to remove parent node if it has no active child node.
            'dv_ParentNode = ds_roleoperation.Tables(0).DefaultView
            'dv_ParentNode.RowFilter = "vParentOperationCode=-999 and (vOperationName <> 'Home' and vOperationName <> 'Logout') and cstatusindi <> 'D'"
            'Dt_New = dv_ParentNode.ToTable()

            '*****************Parent Checking Code*******************
            'Dim ds_Check As New DataSet
            'Dim wStr As String = ""
            'Dim eStr As String = ""

            'wStr = "vOperationCode in(Select distinct(vParentOperationCode) From OperationMst Where"
            'wStr += " vParentOperationCode in(Select vOperationCode From OperationMst) ) and cstatusindi <> 'D'"

            'If Not Me.objhelpDb.getOperationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                ds_Check, eStr) Then

            'End If

            'Dt_New = ds_Check.Tables(0)

            Dim flag As Boolean = False
            Dim flag2 As Boolean = False
            Dim dt_Parent As New DataTable
            Dim dt_ParentNode As New DataTable
            Dim ds_parentroleoperation As New DataSet

            'Dim dr_ParenNode As DataRow

            dv_ParentNode = ds_roleoperation.Tables(0).DefaultView
            dv_ParentNode.RowFilter = "vParentOperationCode='-999' or (vOperationName <> 'Home' and vOperationName <> 'Logout') and cstatusindi <> 'D'"
            Dt_New = dv_ParentNode.ToTable(True, "vOperationCode")

            For index As Integer = 0 To Dt_New.Rows.Count - 1

                If Not CheckParent(ds_roleoperation, Dt_New.Rows(index)("vOperationCode").ToString()) Then

                    For Each dr_change1 As DataRow In ds_roleoperation.Tables(0).Rows
                        If dr_change1("vOperationCode") = Dt_New.Rows(index)("vOperationCode").ToString() Then
                            dr_change1("cActiveFlag") = "N"
                            dr_change1("cStatusIndi") = "D"
                            dr_change1.AcceptChanges()
                            Exit For
                        End If
                    Next dr_change1

                End If


            Next index

            ds_roleoperation.AcceptChanges()
            'added on 12-10-09====by deepak singh====
            dt_ParentNode = ds_roleoperation.Tables(0).Copy()
            For Index1 As Integer = 0 To trVwrpt.CheckedNodes.Count - 1
                CountNode1 = trVwrpt.CheckedNodes(Index1)
                For Each dr_ParentNode As DataRow In dt_ParentNode.Rows
                    '=========added on 21-10-09=== by deepak singh=====

                    If dr_ParentNode("nRoleOperationNo") = -1 Then
                        If CountNode1.Value.ToString = dr_ParentNode("vOperationCode") Then

                            ds_roleoperation = HasParentNode(CountNode1, ds_roleoperation)
                        End If
                    ElseIf dr_ParentNode("vOperationName").ToString.Trim.ToUpper <> "HOME" Or dr_ParentNode("vOperationName").ToString.Trim.ToUpper <> "LOGOUT" Then
                        If CountNode1.Value.ToString = dr_ParentNode("vOperationCode") Then

                            ds_roleoperation = HasParentNode(CountNode1, ds_roleoperation)
                        End If
                    End If
                Next dr_ParentNode
            Next Index1

            ds_roleoperation.AcceptChanges()
            '===================

            ds_newroleoperation = ds_roleoperation.Copy()

            dv = ds_newroleoperation.Tables(0).DefaultView

            dt = New DataTable()
            dt = dv.ToTable(True, StrGroup)

            Me.ViewState(VS_dtRoleOperMtx) = dt
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, estr_retu)
        End Try
    End Function

    Private Function CheckParent(ByRef ds_roleOperation As DataSet, ByVal OperationCode As String) As Boolean
        Dim dv_ParentNode1 As New DataView
        Dim dv_Node1 As New DataView
        Dim dt As New DataTable
        Dim flg As Boolean = False
        Dim index1 As Integer = 0
        Try

            dv_ParentNode1 = ds_roleOperation.Tables(0).Copy().DefaultView
            dv_ParentNode1.RowFilter = "vParentOperationCode = '" + OperationCode + "'"

            dt = dv_ParentNode1.ToTable().Copy()

            If dt.Rows.Count <= 0 Then

                dv_Node1 = ds_roleOperation.Tables(0).Copy().DefaultView
                dv_Node1.RowFilter = "vOperationCode = '" + OperationCode + "'"

                If dv_Node1.ToTable().Rows(0)("cStatusIndi") <> "D" And _
                        dv_Node1.ToTable().Rows(0)("cActiveFlag") <> "N" Then
                    Return True

                Else
                    Return False
                End If
            End If

            For index1 = 0 To dt.Rows.Count - 1

                If Not CheckParent(ds_roleOperation, dt.Rows(index1)("vOperationCode").ToString()) Then

                    For Each dr_change As DataRow In ds_roleOperation.Tables(0).Rows
                        If dr_change("vOperationCode") = dt.Rows(index1)("vOperationCode").ToString() Then
                            dr_change("cActiveFlag") = "N"
                            dr_change("cStatusIndi") = "D"
                            dr_change.AcceptChanges()
                            Exit For
                        End If
                    Next dr_change
                    ds_roleOperation.AcceptChanges()
                    flg = False
                Else
                    flg = True
                End If

            Next index1

            If flg = False Then
                Return False
            End If

            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Function HasParentNode(ByVal CountNode1 As TreeNode, ByVal ds_roleoperation As DataSet) As DataSet
        Dim Dt_Parent As DataTable
        Dim dv_parent As DataView
        Dim parent As String
        Dim dRow As DataRow
        Try


            If Not CountNode1.Parent Is Nothing Then

                Dt_Parent = ds_roleoperation.Tables(0).Copy()
                dv_parent = Dt_Parent.DefaultView
                parent = CType(CountNode1.Parent.Value, String)
                '==========modified on 12-10-09=-By Deepak Singh==========
                dv_parent.RowFilter = "vOperationCode= " + parent + "and cStatusIndi<>'D' and cActiveFlag<>'N'"
                '=====================================

                If dv_parent.ToTable().Rows.Count = 0 Then

                    dRow = ds_roleoperation.Tables(0).NewRow()
                    dRow("nRoleOperationNo") = -1
                    dRow("vUserTypeCode") = ddlRole.SelectedValue
                    dRow("vOperationCode") = CountNode1.Parent.Value.ToString
                    dRow("cStatusIndi") = "N"
                    dRow("cActiveFlag") = "Y"
                    dRow("iModifyBy") = Me.Session(S_UserID).ToString
                    dRow("dModifyOn") = Now.Date()
                    ds_roleoperation.Tables(0).Rows.Add(dRow)
                    ds_roleoperation.Tables(0).AcceptChanges()
                    HasParentNode(CountNode1.Parent, ds_roleoperation)

                End If

            End If

            Return ds_roleoperation

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...HasParentNode")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click
        ' aded and develop by prayag
        Dim OperationType As String = String.Empty
        Dim Dsreport As New DataSet
        Dim Param As String = String.Empty



        If Me.rbtnlstApplicationType.SelectedValue = "0" Then
            OperationType += GeneralModule.OpType_BizNetWeb.ToString
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "1" Then
            OperationType += GeneralModule.OpType_BizNetDesktop
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "2" Then
            OperationType += GeneralModule.OpType_BizNetLIMS
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "3" Then
            OperationType += GeneralModule.OpType_Biolyte
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "4" Then
            OperationType += GeneralModule.OpType_PharmacyManagement
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "5" Then
            OperationType += GeneralModule.OpType_MedicalImaging
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "6" Then
            OperationType += GeneralModule.OpType_SDTM
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "7" Then
            OperationType += GeneralModule.OpType_OIMS
        End If

        Param = "" + hdnForm.Value.ToString() + "" + "##" + OperationType.ToString + "##"

        Dsreport = Me.objhelpDb.ProcedureExecute("dbo.Proc_RoleReport", Param.ToString)

        If Dsreport Is Nothing Then
            objCommon.ShowAlert("Error while call Proc_RoleReport!", Me)
            Exit Sub
        ElseIf Dsreport.Tables(0).Rows.Count = 0 Then
            objCommon.ShowAlert("No record Found!", Me)
            Exit Sub
        End If

        Me.ViewState(VS_dtRoleReport) = Dsreport.Tables(0)
        Me.gvwreport.DataSource = Dsreport.Tables(0)
        Me.gvwreport.DataBind()

        Me.mperolereport.Show()

    End Sub

    Protected Sub btnAudit_Click(sender As Object, e As EventArgs) Handles btnAudit.Click
        'develope by prayag

        Dim OperationType As String = String.Empty
        Dim userid As String = Me.Session(S_UserID)
        Dim Dsaudit As New DataSet
        Me.gvwUserTypeaudit.DataSource = Nothing
        Me.gvwUserTypeaudit.DataBind()
        Dim estr As String = String.Empty

        Dim ddlUsertype As String = ddlRole.SelectedValue.ToString

        If Me.rbtnlstApplicationType.SelectedValue = "0" Then
            OperationType += GeneralModule.OpType_BizNetWeb.ToString
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "1" Then
            OperationType += GeneralModule.OpType_BizNetDesktop
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "2" Then
            OperationType += GeneralModule.OpType_BizNetLIMS
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "3" Then
            OperationType += GeneralModule.OpType_Biolyte
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "4" Then
            OperationType += GeneralModule.OpType_PharmacyManagement
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "5" Then
            OperationType += GeneralModule.OpType_MedicalImaging
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "6" Then
            OperationType += GeneralModule.OpType_SDTM
        ElseIf Me.rbtnlstApplicationType.SelectedValue = "7" Then
            OperationType += GeneralModule.OpType_OIMS
        End If

        If Not Me.objHelp.Proc_OperationRollAudit(ddlUsertype, OperationType, userid, Dsaudit, estr) Then
            Me.ShowErrorMessage("Error While Getting Data From proc_sendsample", estr)
            Exit Sub
        End If

        If Dsaudit Is Nothing Then
            objCommon.ShowAlert("Error while call Proc_OperationRollAudit!", Me)
            Exit Sub
        ElseIf Dsaudit.Tables(0).Rows.Count = 0 Then
            objCommon.ShowAlert("No record Found!", Me)
            Exit Sub
        End If
        Me.ViewState(VS_dtRoleAudit) = Dsaudit.Tables(0)
        Me.gvwUserTypeaudit.DataSource = Dsaudit.Tables(0)
        Me.gvwUserTypeaudit.DataBind()

        mpeAuditTrail.Show()

    End Sub

    Protected Sub btnexit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Response.Redirect("frmmainpage.aspx")
    End Sub

    Protected Sub btnexportreport_Click(sender As Object, e As EventArgs) Handles btnexportreport.Click
        ' added by prayag
        Me.mperolereport.Show()
        Dim fileName As String = String.Empty
        Dim ds As New DataSet
        Try

            If Me.gvwreport.Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "Role Report"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            ds.Tables.Add(CType(Me.ViewState(VS_dtRoleReport), DataTable).Copy())
            ds.AcceptChanges()

            Context.Response.Write(ConvertDsuserTOreport(ds))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnexcel_Click(sender As Object, e As EventArgs) Handles btnexcel.Click
        ' added by prayag
        Me.mpeAuditTrail.Show()
        Dim fileName As String = String.Empty
        Dim ds As New DataSet
        Try

            If Me.gvwUserTypeaudit.Rows.Count < 1 Then
                Me.objCommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "Audit Report"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            ds.Tables.Add(CType(Me.ViewState(VS_dtRoleAudit), DataTable).Copy())
            ds.AcceptChanges()

            Context.Response.Write(ConvertDsuserTOAudit(ds))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            'Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "Radio Button ApplicationType Events"

    Protected Sub rbtnlstApplicationType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnlstApplicationType.SelectedIndexChanged
        Me.GenCall()
    End Sub

    Protected Sub rbtnlselecttype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnlselecttype.SelectedIndexChanged
        'Added and develop by prayag
        Me.GenCall()
        If Me.rbtnlselecttype.Items(0).Selected = True Then
            Me.divreportview.Visible = False
            Me.divtreeview.Visible = True
        Else
            Me.divreportview.Visible = True
            Me.divtreeview.Visible = False
        End If

    End Sub
#End Region

End Class