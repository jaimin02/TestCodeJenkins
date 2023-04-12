Partial Class frmParameterList
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private ObjLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    Private Const vs_ParameterList As String = "TBL_PARALIST"
    Private Const vs_ParameterDeptList As String = "TBL_PARADIV"
    Private Const Vs_Choice As String = "Choice"
    Private Const vs_Dept As String = "Dept"
    Private Const vs_ParaDbType As String = "PARADBTYPE"

    Private Const Vs_ParaNo As String = "ParaNo"
    Private cnt As Integer
    Private eStr As String = ""

    Private DGC_No As Integer = 0
    Private DGC_Name As Integer = 1
    Private DGC_Desc As Integer = 2
    Private DGC_Type As Integer = 3
    Private DGC_IsCorporateLevel As Integer = 4
    Private DGC_Edit As Integer = 5

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            BindParameterGrid()
            BindDept()
            CType(Me.Master.FindControl("lblheading"), Label).Text = "Parameter Master"
            Page.Title = ":: Parameter List :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        End If
        If dgParameter.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIdgParameter", "UIdgParameter(); ", True)
        End If

    End Sub
#End Region

#Region "GenCall"
    'Pass parameter in Function for Corporate Level Parameter
    Private Function GenCall_2(ByVal IsCorporateLevelParameter As String) As Boolean
        Dim tbl_ParameterList As Data.DataTable = Nothing
        Dim tbl_ParameterDeptMatrix As Data.DataTable = Nothing
        Dim tbl_Dept_Retu As Data.DataTable = Nothing

        Try

            If Not GetData(tbl_ParameterList, tbl_ParameterDeptMatrix, tbl_Dept_Retu) Then
                Exit Function
            End If


            If Not GenCall_ShowUI(tbl_ParameterList, tbl_ParameterDeptMatrix) Then
                Exit Function
            End If

            Me.ViewState(vs_ParameterList) = tbl_ParameterList
            Me.ViewState(vs_ParameterDeptList) = tbl_ParameterDeptMatrix

            CType(Me.Master.FindControl("lblheading"), Label).Text = "Parameter Master"

            Me.pnlParamerter.Visible = False
            Me.pnlParamerterDATA.Visible = True

            'Added for Corporate Level Parameter
            Me.PanelPlanned1.Visible = (IsCorporateLevelParameter.ToUpper.Trim() = "NO")
            Me.PanDept.Visible = (IsCorporateLevelParameter.ToUpper.Trim() = "NO")

            BindDept()

            Me.dgDept.Visible = (IsCorporateLevelParameter.ToUpper.Trim() = "NO")
            '***********************************

            GenCall_2 = True
        Catch ex As Exception
            objCommon.ShowAlert(ex.Message, Me.Page)
            objCommon.ShowAlert("Please Restart The Application", Me.Page)
        End Try
    End Function

    Private Function GetData(ByRef tbl_ParameteList_Retu As Data.DataTable, _
                             ByRef tbl_PArameterDeptMatrix_Retu As Data.DataTable, _
                             ByRef tbl_Dept_Retu As Data.DataTable) As Boolean


        Dim ds As Data.DataSet = Nothing
        Dim dt As Data.DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim fndRowIndex As Integer
        Dim eStr As String = String.Empty
        Dim wStr As String

        Try


            'objHelp = New WS_HelpDbTable.WS_HelpDbTable

            wStr = " nParameterNo= " + Me.ViewState(Vs_ParaNo).ToString()

            If Not objHelp.GetParameterList(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                Throw New Exception(eStr + vbCrLf + "Error Occured While Retrieving Parameter Information")
                Exit Function
            End If

            tbl_ParameteList_Retu = ds.Tables(0)


            If tbl_ParameteList_Retu.Rows.Count = 0 Then
                Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            Else
                Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
            End If
            Me.ViewState(Vs_Choice) = Choice


            ds = Nothing
            wStr += " AND cActiveFlag='Y'"
            If Not objHelp.GetParameterDeptMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                Throw New Exception(eStr + vbCrLf + "Error Occured While Retrieving Parameter Dept Matrix Information")
                Exit Function
            End If

            tbl_PArameterDeptMatrix_Retu = ds.Tables(0)
            tbl_PArameterDeptMatrix_Retu.Columns.Add(New Data.DataColumn("DeptName", System.Type.GetType("System.String")))


            'For Dept
            If Not objHelp.GetDeptMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds, eStr) Then
                Throw New Exception(eStr + vbCrLf + "Error Occured While Retrieving Parameter DeptMaster")
                Exit Function
            End If

            tbl_Dept_Retu = ds.Tables(0)

            Me.ViewState(vs_Dept) = tbl_Dept_Retu

            tbl_Dept_Retu.DefaultView.Sort = "vDeptCode"

            For Each dr As Data.DataRow In tbl_PArameterDeptMatrix_Retu.Rows

                fndRowIndex = tbl_Dept_Retu.DefaultView.Find(dr("vDeptCode"))

                If fndRowIndex >= 0 Then
                    dr("DeptName") = tbl_Dept_Retu.DefaultView(fndRowIndex)("vDeptName")
                End If

            Next dr

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, ".....GetData")
        End Try

    End Function

    Private Function GenCall_ShowUI(ByVal tbl_ParameterList As Data.DataTable, _
                            ByVal tbl_ParameterDeptMatrix As Data.DataTable) As Boolean

        Dim paraInfo As ClsParameterList
        Dim Para As ParameterInfo

        ResetPage()
        paraInfo = New ClsParameterList
        Para = paraInfo.ParameterInfoByParameterNo(Me.ViewState("ParaNo"))

        chkFlag.Checked = True

        Me.ViewState(vs_ParaDbType) = Para.ParameterDbType

        If Para.ParameterDbType = ClsParameterList.ParameterDbTypeEnum.DbType_Boolean Then
            ddlboolean.Visible = True
            txtDeptValue.Visible = False
            ddlvaluemain.Visible = True
            txtValue.Visible = False
        Else
            ddlboolean.Visible = False
            txtDeptValue.Visible = True
            ddlvaluemain.Visible = False
            txtValue.Visible = True
        End If


        Me.txtName.Text = Para.ParameterName
        Me.txtDescription.Text = Para.ParameterDesc

        If Para.ParameterDbType = ClsParameterList.ParameterDbTypeEnum.DbType_Boolean Then
            Me.txtType.Text = "Boolean"
        ElseIf Para.ParameterDbType = ClsParameterList.ParameterDbTypeEnum.DbType_Char Then
            Me.txtType.Text = "Char"
        ElseIf Para.ParameterDbType = ClsParameterList.ParameterDbTypeEnum.DbType_varchar Then
            Me.txtType.Text = "varchar"
        ElseIf Para.ParameterDbType = ClsParameterList.ParameterDbTypeEnum.DbType_Integer Then
            Me.txtType.Text = "Integer"
        ElseIf Para.ParameterDbType = ClsParameterList.ParameterDbTypeEnum.DbType_Date Then
            Me.txtType.Text = "Date"
        ElseIf Para.ParameterDbType = ClsParameterList.ParameterDbTypeEnum.DbType_Double Then
            Me.txtType.Text = "Double"
        ElseIf Para.ParameterDbType = ClsParameterList.ParameterDbTypeEnum.DbType_TinyInteger Then
            Me.txtType.Text = "Tiny Integer"
        End If

        If tbl_ParameterList.Rows.Count > 0 Then
            Me.pnlParamerterDATA.Visible = True
            Me.pnlParamerter.Visible = True

            If Para.ParameterDbType = ClsParameterList.ParameterDbTypeEnum.DbType_Boolean Then
                Me.ddlvaluemain.SelectedValue = IIf(tbl_ParameterList.Rows(0)("vParameterValue") = "Y", 1, 0)
            Else
                Me.txtValue.Text = tbl_ParameterList.Rows(0)("vParameterValue").ToString()
            End If

            Me.txtRelatedProcess.Text = tbl_ParameterList.Rows(0)("vRelatedProcess").ToString()

        End If

        Me.dgDept.DataSource = tbl_ParameterDeptMatrix
        Me.dgDept.DataBind()

        Return True

    End Function

#End Region

#Region "Reset Page "

    Private Sub ResetPage()
        Me.pnlParamerterDATA.Visible = False
        Me.pnlParamerter.Visible = True
        Me.txtName.Text = ""
        Me.txtType.Text = ""
        Me.txtValue.Text = ""
        Me.txtRelatedProcess.Text = ""
        Me.txtRemark.Text = ""
        Me.txtDescription.Text = ""
        Me.txtDeptValue.Text = ""
        If Me.ddlDept.SelectedIndex > 0 Then
            Me.ddlDept.SelectedIndex = 0
        End If
        Me.txtDeptValue.Text = ""
        Me.dgDept.DataSource = Nothing
        Me.brnExitMain.Visible = True
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

    End Sub

#End Region

#Region "Bind Dept"
    Private Sub BindDept()

        Dim ds_Dept As New DataSet
        Try
            If Not objHelp.GetDeptMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                                       ds_Dept, eStr) Then

                Me.objCommon.ShowAlert("Error While Getting Data From DeptMst", Me.Page)
                Exit Sub

            End If

            Me.ddlDept.DataSource = ds_Dept
            Me.ddlDept.DataTextField = "vDeptName"
            Me.ddlDept.DataValueField = "vDeptCode"
            Me.ddlDept.DataBind()
            If dgDept.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIdgDept", "UIdgDept(); ", True)
            End If

        Catch ex As Exception
            ex.Message.ToString()
        End Try
    End Sub
#End Region

#Region "Save"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Ds As DataSet
        Dim eStr As String = String.Empty
        Dim objOPws As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
        Dim tbl_ParaDiv As Data.DataTable = Nothing

        If Not AssignValues(tbl_ParaDiv) Then
            Exit Sub
        End If

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Ds = New DataSet
            Ds.Tables.Add(CType(Me.ViewState(vs_ParameterList), Data.DataTable).Copy())
            Ds.Tables(0).TableName = "ParameterList"   ' New Values on the form to be updated

            Ds.Tables.Add(tbl_ParaDiv)
            Ds.Tables(1).TableName = "ParameterDeptMatrix"   ' New Values on the form to be updated
            Ds.Tables(1).Columns.Remove("DeptName")


            If Not objOPws.Save_ParameterList(Me.ViewState(Vs_Choice), Ds, Me.Session(GeneralModule.S_UserID), eStr) Then
                ShowErrorMessage(eStr, "")
                Exit Sub
            Else
                objCommon.ShowAlert("Record Saved Successfully !", Me)
                ResetPage()
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try

        Me.ResetPage()
    End Sub

#End Region

#Region "Button Events"

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim estr As String = String.Empty
        Dim dr As DataRow
        Dim dtadd As DataTable
        Dim dbtype As ClsParameterList.ParameterDbTypeEnum
        Dim valdivtext As String
        Dim dtsave As New DataTable

        dtadd = Me.ViewState(vs_ParameterDeptList)

        For Each dr In dtadd.Rows

            If dr("vDeptCode") = Me.ddlDept.SelectedValue And _
               dr("cActiveFlag") <> "N" Then
                ShowErrorMessage("Dept Already Exist !", "")
                Exit Sub
            End If

        Next dr


        dbtype = ViewState(vs_ParaDbType)

        If dbtype <> ClsParameterList.ParameterDbTypeEnum.DbType_Boolean Then
            valdivtext = Me.txtDeptValue.Text

        Else

            Me.txtDeptValue.Text = Me.ddlboolean.SelectedItem.Text
            valdivtext = CType(Me.ddlboolean.SelectedItem.Value, Boolean)
        End If

        If Me.txtDeptValue.Text = "" Then
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = "Insert Value"
            Exit Sub
        End If


        Try

            If dbtype = ClsParameterList.ParameterDbTypeEnum.DbType_Boolean Then
                estr = "Please Enter Valid Parameter Value In Boolean Datatype Format"
                valdivtext = CType(valdivtext, Boolean)
            ElseIf dbtype = ClsParameterList.ParameterDbTypeEnum.DbType_Date Then
                estr = "Please Enter Valid Parameter Value In Date Datatype Format"
                valdivtext = CType(valdivtext, Date)
            ElseIf dbtype = ClsParameterList.ParameterDbTypeEnum.DbType_Char Then
                estr = "Please Enter Valid Parameter Value In Char Datatype Format"
                valdivtext = CType(valdivtext, Char)
            ElseIf dbtype = ClsParameterList.ParameterDbTypeEnum.DbType_Integer Then
                estr = "Please Enter Valid Parameter Value In Integer Datatype Format"
                valdivtext = CType(valdivtext, Integer)
            ElseIf dbtype = ClsParameterList.ParameterDbTypeEnum.DbType_Double Then
                estr = "Please Enter Valid Parameter Value In Double Datatype Format"
                valdivtext = CType(valdivtext, Double)
            ElseIf dbtype = ClsParameterList.ParameterDbTypeEnum.DbType_TinyInteger Then
                estr = "Please Enter Valid Parameter Value In Double Datatype Format"
                valdivtext = CType(valdivtext, Short)
            ElseIf dbtype = ClsParameterList.ParameterDbTypeEnum.DbType_varchar Then
                estr = "Please Enter Valid Parameter Value In Varchar Datatype Format"
                valdivtext = CType(valdivtext, String)
            Else
                CType(Me.Master.FindControl("lblerrormsg"), Label).Text = "Not Valid DataType"
                Exit Sub
            End If

            dr = dtadd.NewRow

            dr("nParameterNo") = Me.ViewState("ParaNo")
            dr("vDeptCode") = Me.ddlDept.SelectedValue
            dr("DeptName") = Me.ddlDept.SelectedItem.Text
            dr("vParameterValue") = Me.txtDeptValue.Text
            dr("vParameterType") = Me.txtType.Text
            dr("nParameterDeptNo") = -1
            dr("cActiveFlag") = "Y"

            dtadd.Rows.Add(dr)
            dtadd.DefaultView.RowFilter = "cActiveFlag='Y'"

            Me.dgDept.DataSource = dtadd
            Me.dgDept.DataBind()

            Me.ddlDept.SelectedIndex = 0
            Me.txtDeptValue.Text = ""

        Catch ex As Exception
            Me.ShowErrorMessage(estr, ".....btnAdd_Click")
        Finally

        End Try

    End Sub

#End Region

#Region "Assign Values"

    Private Function AssignValues(ByRef DtParameterDeptMatrix_Retu As Data.DataTable) As Boolean

        Dim tbl As Data.DataTable
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim dr As Data.DataRow
        Dim drsave As Data.DataRow
        Dim ParaDbType As ClsParameterList.ParameterDbTypeEnum

        Try



            tbl = Me.ViewState(vs_ParameterList)
            Choice = Me.ViewState(Vs_Choice)
            ParaDbType = Me.ViewState(vs_ParaDbType)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                tbl.Rows.Clear()
                dr = tbl.NewRow
                tbl.Rows.Add(dr)
            Else
                dr = tbl.Rows(0)
            End If

            dr("nParameterNo") = Me.ViewState("ParaNo")
            dr("vParameterName") = Me.txtName.Text
            dr("vParameterValue") = IIf(ParaDbType = ClsParameterList.ParameterDbTypeEnum.DbType_Boolean, Me.ddlvaluemain.SelectedItem.Text, Me.txtValue.Text)
            dr("vParameterType") = Me.txtType.Text
            dr("vParameterDesc") = Me.txtDescription.Text
            dr("vRelatedProcess") = Me.txtRelatedProcess.Text
            dr("vRemark") = Me.txtRemark.Text
            dr("cActiveFlag") = IIf(Me.chkFlag.Checked, "Y", "N")

            dr.AcceptChanges()

            tbl = Me.ViewState(vs_ParameterDeptList)
            DtParameterDeptMatrix_Retu = tbl.Clone

            For Each dr In tbl.Rows

                If dr("cActiveFlag") <> "N" Or dr("nParameterNo") <> -1 Then

                    drsave = DtParameterDeptMatrix_Retu.NewRow

                    drsave("nParameterNo") = dr("nParameterNo")
                    drsave("vDeptCode") = dr("vDeptCode")
                    drsave("DeptName") = dr("DeptName")
                    drsave("vParameterValue") = dr("vParameterValue")
                    drsave("vParameterType") = dr("vParameterType")
                    drsave("nParameterDeptNo") = dr("nParameterDeptNo")
                    dr("vRemark") = Me.txtRemark.Text
                    drsave("cActiveFlag") = dr("cActiveFlag")

                    DtParameterDeptMatrix_Retu.Rows.Add(drsave)

                End If
            Next dr

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "Error Occured While Assiging Values")
        End Try

    End Function

#End Region

#Region "BindParameterGrid"

    Private Sub BindParameterGrid()
        Dim ParaInfo As ClsParameterList
        Dim dc As DataColumn
        Dim dt As DataTable
        Dim dr As DataRow

        dt = New DataTable
        dc = New DataColumn
        dc.ColumnName = "ParameterNo"
        dt.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "ParameterName"
        dt.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "ParameterDesc"
        dt.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "ParameterDbType"
        dt.Columns.Add(dc)

        'Added for Corporate Level Parameter
        dc = New DataColumn
        dc.ColumnName = "IsCorporateLevelParameter"
        dt.Columns.Add(dc)
        '*****************************

        ParaInfo = New ClsParameterList
        For cnt = 1 To ParaInfo.ParameterCount

            dr = dt.NewRow
            dr("ParameterNo") = CType(ParaInfo.ParameterInfoByIndex(cnt).ParameterNo, Integer)
            dr("ParameterName") = ParaInfo.ParameterInfoByIndex(cnt).ParameterName
            dr("ParameterDesc") = ParaInfo.ParameterInfoByIndex(cnt).ParameterDesc
            dr("ParameterDbType") = ParaInfo.ParameterInfoByIndex(cnt).ParameterDbType
            'Added for Corporate Level Parameter
            dr("IsCorporateLevelParameter") = IIf(ParaInfo.ParameterInfoByIndex(cnt).IsCorporateLevelParameter, "YES", "NO")
            '*************************************
            dt.Rows.Add(dr)
        Next cnt

        Me.dgParameter.DataSource = dt
        Me.dgParameter.DataBind()
        Me.pnlParamerterDATA.Visible = False
        Me.pnlParamerter.Visible = True
        If dgParameter.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIdgParameter", "UIdgParameter(); ", True)
        End If
    End Sub

#End Region

#Region "Dept Grid Event"

    Protected Sub dgDept_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDept.RowCommand
        Dim i As Int32 = CType(e.CommandArgument, Int32)
        Dim dt As New DataTable

        If e.CommandName.ToString.ToUpper = "DELETE" Then


            dt = CType(Me.ViewState(vs_ParameterDeptList), DataTable)
            dt.Rows(i).Item("cActiveFlag") = "N"
            dt.DefaultView.RowFilter = "cActiveFlag='Y'"

            Me.ViewState(vs_ParameterDeptList) = dt

        End If
    End Sub

    Protected Sub dgDept_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgDept.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(0).FindControl("imgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
        End If
    End Sub

    Protected Sub dgDept_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        e.Cancel = True
    End Sub

    Protected Sub dgDept_RowDeleting1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)
        Me.dgDept.DataSource = Me.ViewState(vs_ParameterDeptList)
        Me.dgDept.DataBind()
    End Sub

    Protected Sub dgDept_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs)

    End Sub

#End Region

#Region "Parameter Grid Events"

    Protected Sub dgParameter_PreRender(sender As Object, e As EventArgs) Handles dgParameter.PreRender

    End Sub

    Protected Sub dgParameter_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgParameter.RowCommand

        If e.CommandName.ToString.ToUpper = "EDIT" Then
            Me.ViewState(Vs_ParaNo) = Me.dgParameter.Rows(CType(e.CommandArgument, Int32)).Cells(0).Text

            'Pass parameter in Function for Corporate Level Parameter
            GenCall_2(Me.dgParameter.Rows(CType(e.CommandArgument, Int32)).Cells(DGC_IsCorporateLevel).Text)
            '*************

        End If

        Me.pnlParamerterDATA.Visible = True
        Me.pnlParamerter.Visible = False
        Me.brnExitMain.Visible = False

    End Sub

    Protected Sub dgParameter_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgParameter.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(0).FindControl("imgEdit"), ImageButton).CommandArgument = e.Row.RowIndex
        End If

    End Sub

    Protected Sub dgParameter_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        e.Cancel = True
    End Sub

#End Region

#Region "Main Exit"
    Protected Sub brnExitMain_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub
#End Region

#Region "Error Message"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
    End Sub
#End Region

End Class
