Partial Class frmtreenode
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    'Private Const VS_DtWSSub As String = "DtWSSub"
    Private Const VS_dtTemplateNodeDetail As String = "dtTemplateNodeDetail"
    Private Const VS_dtTemplateNodeDetail1 As String = "dtTemplateNodeDetail1"
    Private dt As DataTable = Nothing
    Private eStr_Retu As String

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Me.Request.QueryString("Rights") = "Yes" Then
                objCommon.ShowAlert("Record Saved SuccessFully !", Me.Page)
            End If
            If Not GenCall() Then
                Exit Sub
            End If
        Else
            CreateTemplateNodeTable()
        End If

    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Dim dt_TemplateNodeDetail As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim wStr As String
        Dim ds_TemplatenodeDetail As New Data.DataSet
        wStr = "1=2"
        Try



            Choice = CType("1", WS_Lambda.DataObjOpenSaveModeEnum)

            Me.ViewState("Choice") = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState("vActivityId") = Me.Request.QueryString("Value").ToString

            End If

            ''Check for Valid User''
            If Not GenCall_Data(Choice, dt_TemplateNodeDetail) Then ' For Data Retrieval
                Exit Function
            End If

            wStr = " vTemplateid = " + Me.Request.QueryString("vTemplateid")
            Me.ViewState("vTemplateid") = Me.Request.QueryString("vTemplateid")

            If Not objHelp.getTemplateNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_TemplatenodeDetail, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            Me.ViewState(VS_dtTemplateNodeDetail) = dt_TemplateNodeDetail ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, dt_TemplateNodeDetail) Then 'For Displaying Data 
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".......GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data(ByVal Choice As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds As DataSet = Nothing
        'Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim i As Integer

        Try

            'Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit ' Always in edit Mode
            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vActivityId=" + Me.ViewState("vActivityId").ToString() 'Value of where condition
                wStr = "vTemplateId='" & Me.TVTemplate.CheckedNodes(i).Value & "' and iNodeId=" & Me.TVTemplate.CheckedNodes(i).ImageToolTip
            End If

            If Not objHelp.getTemplateNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Throw New Exception(eStr)
                Exit Function
            End If
            If ds Is Nothing Then
                Throw New Exception(eStr)
            End If
            If ds.Tables(0).Rows.Count <= 0 And _
               Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                'Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found For Selected Role")
            End If

            dt_Dist_Retu = ds.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI(ByVal Choice As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_Dist As DataTable) As Boolean
        Dim dt_treenodedetail As DataTable = Nothing



        Page.Title = ":: Edit Template Structure  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

        CType(Master.FindControl("lblHeading"), Label).Text = "Template Tree Node"

        'Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit 'Always in Edit Mode

        If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            Me.ddlAddActivity.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_treenodedetail.Rows(0)("vActivityId"), dt_treenodedetail.Rows(0)("vActivityId").GetType)
            Me.ddlAct.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_treenodedetail.Rows(0)("vActivityId"), dt_treenodedetail.Rows(0)("vActivityId").GetType)

        End If

        BindTree()

        If Not FillActivityGroup() Then
            Return False
        End If

        Return True
    End Function
#End Region

#Region "Bindtree"

    Private Sub BindTree()
        Dim objTemplate As New DataSet
        Dim dsVacantPosition As New DataSet
        Dim dsTemplate As New DataSet
        Dim dt_Template As New DataTable
        Dim CurrentNode As New TreeNode
        Dim templatetype As String = String.Empty
        Dim ParentNode As New TreeNode
        Dim bln As Boolean = False
        Dim cnt As Integer = 0
        Dim eStr As String = String.Empty
        Dim dv As New DataView
        Try
           
            If Not Me.Request.QueryString("TemplateName") Is Nothing Then

                If Me.Request.QueryString("TemplateName").ToString().Length > 0 Then
                    CType(Master.FindControl("lblHeading"), Label).Text = "Edit Template Structure" & "</br></br>Template : " & Me.Request.QueryString("TemplateName").ToString()
                End If

            End If

            templatetype = Me.Request.QueryString("vTemplateId")
            Me.Session("vTemplateId") = templatetype
            Me.TVTemplate.Nodes.Clear()

            If Not objHelp.Proc_TemplateTreeView(templatetype, objTemplate, eStr) Then
                CType(Master.FindControl("lblHeading"), Label).Text = "Problem While Generating Tree view"
                Exit Sub
            End If

            dt = objTemplate.Tables(0)

            dv = New DataView(dt)
            dv.Sort = "iNodeNo ASC"
            dt = dv.ToTable()
            TestLast("0", New TreeNode())

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....BindTree")
        End Try
    End Sub

    Private Sub TestLast(ByVal parentId As String, ByVal tn As TreeNode)
        Dim dr() As DataRow = dt.Select("iParentNodeId=" & parentId)
        Dim parentNode As New TreeNode
        Try



            For index As Integer = 0 To dr.Length - 1

                parentNode = New TreeNode

                'parentNode.ChildNodes.Item(0).ChildNodes.Attribute.add("onClick", "NoTemplate()")
                'parentNode.Text = dr(index)("vNodeDisplayName").ToString.Trim() & "(" & dr(index)("vActivityName") & ")"
                'parentNode.NavigateUrl = "Javascript: NoTemplate();"
                parentNode.Text = "<a Style=""display: inline-block"" href=""javascript:window.msgalert('No Templates Attached With This Activity !');""  >" & dr(index)("vNodeDisplayName").ToString.Trim() & "(" & dr(index)("vActivityName") & ")</a>"

                If dr(index)("vMedExTemplateId").ToString <> "" Then
                    parentNode.NavigateUrl = "Javascript: ShowTemplate('frmPreviewattributesForm.aspx?TemplateId=" & dr(index)("vMedExTemplateId").ToString.Trim() & "');"
                    parentNode.Value = dr(index)("vMedExTemplateId").ToString.Trim()
                    parentNode.Text = "<a href=""frmPreviewattributesForm.aspx?TemplateId=" & dr(index)("vMedExTemplateId").ToString.Trim() & """  target='_blank' >" & dr(index)("vNodeDisplayName").ToString.Trim() & "(" & dr(index)("vActivityName") & ")</a>"

                    '<a href=""frmPreviewattributesForm.aspx?TemplateId=" & dr(index)("vMedExTemplateId").ToString.Trim() & "">" & dr(index)("vNodeDisplayName").ToString.Trim() & "(" & dr(index)("vActivityName") & ")</a>

                Else
                    '  objCommon.ShowAlert("No Template is Attached", Me.Page)
                End If

                parentNode.Value = dr(index)("vNodeDisplayName").ToString.Trim()
                parentNode.ImageToolTip = dr(index)("iNodeId").ToString.Trim()
                parentNode.ToolTip = dr(index)("vActivityId").ToString.Trim()

                If parentId = "0" Then

                    TVTemplate.Nodes.Add(parentNode)
                Else
                    tn.ChildNodes.Add(parentNode)
                End If
                TestLast(dr(index)("iNodeId"), parentNode)
            Next index

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Fill ActivityGroup"

    Private Function FillActivityGroup() As Boolean
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim estr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Dim ds As New Data.DataSet
        Dim ds_Group As New Data.DataSet
        Dim dv_Activity As New DataView
        Try



            'To Get Where condition of ScopeVales( Project Type )
            If Not objCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Return False
            End If
            Wstr_Scope += " And cStatusIndi <> 'D'"
            If Not objHelp.GetviewActivityGroupMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Group, estr) Then
                ShowErrorMessage("", estr)
            End If

            dv_Activity = ds_Group.Tables(0).DefaultView
            dv_Activity.Sort = "vActivityGroupName"
            Me.ddlActivityGroup2.DataSource = dv_Activity.ToTable()
            Me.ddlActivityGroup2.DataTextField = "vActivityGroupName"
            Me.ddlActivityGroup2.DataValueField = "vActivityGroupId"
            Me.ddlActivityGroup2.DataBind()
            Me.ddlActivityGroup2.Items.Insert(0, New ListItem("Select ActivityGroup", "1"))
            ' tooltip
            For iMedexGroup As Integer = 0 To ddlActivityGroup2.Items.Count - 1
                ddlActivityGroup2.Items(iMedexGroup).Attributes.Add("title", ddlActivityGroup2.Items(iMedexGroup).Text)
            Next iMedexGroup
            '=========

            Me.ddlActivityGroup.DataSource = dv_Activity.ToTable()
            Me.ddlActivityGroup.DataTextField = "vActivityGroupName"
            Me.ddlActivityGroup.DataValueField = "vActivityGroupId"
            Me.ddlActivityGroup.DataBind()
            Me.ddlActivityGroup.Items.Insert(0, New ListItem("Select ActivityGroup", "1"))
            ' tooltip
            For iMedexGroup As Integer = 0 To ddlActivityGroup.Items.Count - 1
                ddlActivityGroup.Items(iMedexGroup).Attributes.Add("title", ddlActivityGroup.Items(iMedexGroup).Text)
            Next iMedexGroup
            '=========

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......FillActivityGroup")
            Return False
            Exit Try
            Exit Function

        End Try
        Return True
    End Function

#End Region

#Region "DropDown SelectedIndex Chaged"

    Protected Sub ddlActivityGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        FillActivity("DDLACTIVITYGROUP")
        Me.mpeaddactivity.Show()
        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.divTV.ClientID.ToString.Trim() + _
        '                                      "');", True)
    End Sub

    Protected Sub ddlActivityGroup2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivityGroup2.SelectedIndexChanged
        FillActivity("DDLACTIVITYGROUP2")
        mpeDialogAddActivity.Show()
        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.divact.ClientID.ToString.Trim() + _
        '                                     "');", True)
    End Sub

    Protected Sub ddlAddActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAddActivity.SelectedIndexChanged
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim estr As String = String.Empty
        Dim ds As New Data.DataSet
        Try



            If Not objHelp.GetActivityStageDetailByActivityId(Me.ddlAddActivity.SelectedValue, ds, estr) Then
                ShowErrorMessage("", estr)
            End If

            Me.GVActivityName.DataSource = ds
            Me.GVActivityName.DataBind()
            mpeaddactivity.Show()

            'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + Me.divTV.ClientID.ToString.Trim() + _
            '                                             "');", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......ddlAddActivity_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "Fill Activity"

    Private Function FillActivity(ByVal FromDropdown As String) As Boolean

        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim ds As New Data.DataSet
        Dim ds_type As New Data.DataSet
        Dim dv_Activity As New DataView
        Try



            If FromDropdown.ToUpper = "DDLACTIVITYGROUP2" Then
                wstr = "vActivityGroupId='" & Me.ddlActivityGroup2.SelectedValue.Trim() & "'"
            ElseIf FromDropdown.ToUpper = "DDLACTIVITYGROUP" Then
                wstr = "vActivityGroupId='" & Me.ddlActivityGroup.SelectedValue.Trim() & "'"
            End If

            wstr += " And cStatusIndi <> 'D'"

            If Not objHelp.getActivityMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_type, estr) Then
                ShowErrorMessage("", estr)
            End If

            dv_Activity = ds_type.Tables(0).DefaultView
            dv_Activity.Sort = "vActivityName"

            If FromDropdown.ToUpper = "DDLACTIVITYGROUP" Then
                Me.ddlAddActivity.DataSource = dv_Activity.ToTable()
                Me.ddlAddActivity.DataValueField = "vActivityId"
                Me.ddlAddActivity.DataTextField = "vActivityName"
                Me.ddlAddActivity.DataBind()
                Me.ddlAddActivity.Items.Insert(0, New ListItem("Please Select Activity", "0"))
                ' tooltip
                For iMedexGroup As Integer = 0 To ddlAddActivity.Items.Count - 1
                    ddlAddActivity.Items(iMedexGroup).Attributes.Add("title", ddlAddActivity.Items(iMedexGroup).Text)
                Next iMedexGroup
                '=========

            ElseIf FromDropdown.ToUpper = "DDLACTIVITYGROUP2" Then

                Me.ddlAct.DataSource = dv_Activity.ToTable()
                Me.ddlAct.DataValueField = "vActivityId"
                Me.ddlAct.DataTextField = "vActivityName"
                Me.ddlAct.DataBind()
                Me.ddlAct.Items.Insert(0, New ListItem("Please Select Activity", "0"))
                ' tooltip
                For iMedexGroup As Integer = 0 To ddlAct.Items.Count - 1
                    ddlAct.Items(iMedexGroup).Attributes.Add("title", ddlAct.Items(iMedexGroup).Text)
                Next iMedexGroup
                '=========
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......FillActivity")
        End Try
    End Function

#End Region

#Region "AssingValues"

    Private Sub AssignValue()
        Dim dtTemplateNodeDetail As New DataTable
        Dim dr As DataRow
        Dim index As Integer
        Dim templatetype As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_ActivityMst As New DataSet
        Dim ds_TemplatenodeDetail As New Data.DataSet
        Dim Wstr As String = "vActivityId= " + ddlAddActivity.SelectedValue

        Try

         
            dtTemplateNodeDetail = Me.ViewState(VS_dtTemplateNodeDetail)
            dtTemplateNodeDetail.Rows.Clear()
            templatetype = Me.Request.QueryString("vtemplateId")
            dr = dtTemplateNodeDetail.NewRow

            dr("iNodeId") = Me.ViewState("iNodeId")
            dr("vTemplateId") = templatetype
            dr("vNodeName") = Me.ddlAddActivity.SelectedItem.Text.Trim
            dr("vActivityId") = Me.ddlAddActivity.SelectedValue

            'Get Period Specific and milestone from activity master
            If (Me.ddlAddActivity.SelectedIndex > 0) Then

                If Not objHelp.getActivityMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ActivityMst, estr) Then

                    ShowErrorMessage(eStr_Retu, "")

                End If

                If (ds_ActivityMst.Tables(0).Rows(0)("cPeriodSpecific").ToString() <> "") Then
                    dr("cPeriodSpecific") = ds_ActivityMst.Tables(0).Rows(0)("cPeriodSpecific").ToString()
                End If
                If (ds_ActivityMst.Tables(0).Rows(0)("nMilestone").ToString() <> "") Then
                    dr("nMilestone") = ds_ActivityMst.Tables(0).Rows(0)("nMilestone").ToString()
                End If

            End If

            dr("vNodeDisplayName") = Me.ddlAddActivity.SelectedItem.Text.Trim
            dr("vFolderName") = Me.ddlAddActivity.SelectedItem.Text.Trim
            dr("vRemark") = ""
            dr("iModifyBy") = Me.Session(S_UserID)
            '
            For index = 0 To Me.TVTemplate.CheckedNodes.Count - 1
                Me.ViewState("vTemplateid") = Me.TVTemplate.CheckedNodes(index).Value
                Me.ViewState("iNodeId") = Me.TVTemplate.CheckedNodes(index).ImageToolTip
            Next index

            dtTemplateNodeDetail.Rows.Add(dr)
            Me.ViewState(VS_dtTemplateNodeDetail) = dtTemplateNodeDetail

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....AssignValue")
        End Try
    End Sub

#End Region

#Region "Add Last"

    Protected Sub btnAddLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLast.Click
        'Dim objLambda As New WS_Lambda.WS_Lambda
        Dim ds_addlst As New DataSet

        Dim ds_CanStart As New DataSet
        Dim ds_CanStartActvity As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim CanStartActivities As String = String.Empty

        Try



            'Start To check Canstart after activity on 19-Nov-2008
            wStr = "  cStatusindi<>'D' and vTemplateId='" + Me.Request.QueryString("vtemplateId").Trim() + "'" + _
                    " and Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
                    Me.ddlAddActivity.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAddActivity.SelectedValue.Trim() + _
                    "') end)>0 "

            If Not Me.objHelp.getTemplateNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_CanStart, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Sub
            End If

            If ds_CanStart.Tables(0).Rows.Count <= 0 Then

                wStr = " Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
                                    Me.ddlAddActivity.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
                                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAddActivity.SelectedValue.Trim() + _
                                    "') end)>0 "

                If Not Me.objHelp.getActivityMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        ds_CanStartActvity, eStr) Then
                    Me.ShowErrorMessage("", eStr)
                    Exit Sub
                End If

                For index As Integer = 0 To ds_CanStartActvity.Tables(0).Rows.Count - 1
                    CanStartActivities += IIf(CanStartActivities = "", "", ", ") + _
                                        "'" + ds_CanStartActvity.Tables(0).Rows(index)("vActivityName") + "'"
                Next index

                Me.objCommon.ShowAlert("Please, First Add: " + CanStartActivities + " Activities Then Only You Can Add This Activity.", Me.Page)
                Exit Sub
            End If

            '***********End Check

            'Me.divTV.Visible = False
            mpeaddactivity.Hide()
            AssignValue()
            ds_addlst = New DataSet
            ds_addlst.Tables.Add(Me.ViewState(VS_dtTemplateNodeDetail))
            ds_addlst.Tables(0).TableName = "TemplateNodeDetail"   ' New Values on the form to be updated

            If Not objLambda.Save_InsertTemplateLeafNode(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_addlst, Me.Session("UserCode"), eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Sub
            End If

            BindTree()
            TVTemplate.ExpandAll()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnAddLast_Click")
        End Try
    End Sub

#End Region

#Region "Add Before"
    Protected Sub btnAddBefore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddBefore.Click
        Dim ds_addbfr As DataSet
        'Dim objLambda As New WS_Lambda.WS_Lambda

        Dim ds_CanStart As New DataSet
        Dim ds_CanStartActvity As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim CanStartActivities As String = String.Empty
        Try



            'Start To check Canstart after activity on 19-Nov-2008
            wStr = " cStatusindi<>'D' and vTemplateId='" + Me.Request.QueryString("vtemplateId").Trim() + "'" + _
                    " and Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
                    Me.ddlAddActivity.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAddActivity.SelectedValue.Trim() + _
                    "') end)>0 "

            If Not Me.objHelp.getTemplateNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_CanStart, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Sub
            End If

            If ds_CanStart.Tables(0).Rows.Count <= 0 Then

                wStr = " Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
                                    Me.ddlAddActivity.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
                                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAddActivity.SelectedValue.Trim() + _
                                    "') end)>0 "

                If Not Me.objHelp.getActivityMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        ds_CanStartActvity, eStr) Then
                    Me.ShowErrorMessage("", eStr)
                    Exit Sub
                End If

                For index As Integer = 0 To ds_CanStartActvity.Tables(0).Rows.Count - 1
                    CanStartActivities += IIf(CanStartActivities = "", "", ", ") + _
                                        "'" + ds_CanStartActvity.Tables(0).Rows(index)("vActivityName") + "'"
                Next index

                Me.objCommon.ShowAlert("Please, First Add: " + CanStartActivities + " Activities Then Only You Can Add This Activity.", Me.Page)
                Exit Sub

            End If

            '***********End Check

            'Me.divTV.Visible = False
            mpeaddactivity.Hide()
            AssignValue()
            ds_addbfr = New DataSet
            ds_addbfr.Tables.Add(Me.ViewState(VS_dtTemplateNodeDetail))
            ds_addbfr.Tables(0).TableName = "TemplateNodeDetail"   ' New Values on the form to be updated

            If Not objLambda.Save_InsertTemplateNodeBefore(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_addbfr, Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Sub

            End If
            BindTree()
            TVTemplate.ExpandAll()

        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....btnAddBefore_Click")
        End Try
    End Sub

#End Region

#Region "Add After"
    Protected Sub btnAddAfter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAfter.Click
        Dim ds_addaft As DataSet
        'Dim objLambda As New WS_Lambda.WS_Lambda

        Dim ds_CanStart As New DataSet
        Dim ds_CanStartActvity As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim CanStartActivities As String = String.Empty
        Try




            'Start To check Canstart after activity on 19-Nov-2008
            wStr = " cStatusindi<>'D' and vTemplateId='" + Me.Request.QueryString("vtemplateId").Trim() + "'" + _
                    " and Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
                    Me.ddlAddActivity.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAddActivity.SelectedValue.Trim() + _
                    "') end)>0 "

            If Not Me.objHelp.getTemplateNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_CanStart, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Sub
            End If

            If ds_CanStart.Tables(0).Rows.Count <= 0 Then
                wStr = " Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
                                    Me.ddlAddActivity.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
                                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAddActivity.SelectedValue.Trim() + _
                                    "') end)>0 "

                If Not Me.objHelp.getActivityMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        ds_CanStartActvity, eStr) Then
                    Me.ShowErrorMessage("", eStr)
                    Exit Sub
                End If

                For index As Integer = 0 To ds_CanStartActvity.Tables(0).Rows.Count - 1

                    CanStartActivities += IIf(CanStartActivities = "", "", ", ") + _
                                        "'" + ds_CanStartActvity.Tables(0).Rows(index)("vActivityName") + "'"

                Next index

                Me.objCommon.ShowAlert("Please, First Add: " + CanStartActivities + " Activities Then Only You Can Add This Activity.", Me.Page)
                Exit Sub
            End If

            '***********End Check
            'Me.divTV.Visible = False
            mpeaddactivity.Hide()
            AssignValue()
            ds_addaft = New DataSet
            ds_addaft.Tables.Add(Me.ViewState(VS_dtTemplateNodeDetail))
            ds_addaft.Tables(0).TableName = "TemplateNodeDetail"   ' New Values on the form to be updated

            If Not objLambda.Save_InsertTemplateNodeAfter(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_addaft, Me.Session(S_UserID), eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Sub

            End If

            BindTree()
            TVTemplate.ExpandAll()

        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....btnAddAfter_Click")
        End Try
    End Sub
#End Region

#Region "Edit Activity"

    Private Function AssignValue2() As Boolean
        Dim dtTemplateNodeDetail As New DataTable
        Dim dr As DataRow
        Dim index As Integer
        Dim templatetype As String = String.Empty
        Dim ds_TemplatenodeDetail As New Data.DataSet
        Dim dtOld As DataTable
        Try



            dtOld = Me.ViewState(VS_dtTemplateNodeDetail1)
            If Me.ViewState("Choice") = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                dtOld.Rows.Clear()
                dr = dtOld.NewRow
                dtOld.Rows.Add(dr)
            Else
                dr = dtOld.Rows(0)
            End If

            dr("iNodeId") = Me.TVTemplate.CheckedNodes(index).ImageToolTip 'Me.ViewState("iNodeId")
            dr("vNodeName") = Me.ddlAct.SelectedItem.Text.Trim ' Me.ViewState("vnodename")
            dr("vNodeDisplayName") = Me.txtDisplayName.Text.Trim()
            dr("vActivityId") = Me.ddlAct.SelectedValue
            dr.AcceptChanges()

            Me.ViewState(VS_dtTemplateNodeDetail1) = dtOld
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....AssignValue2")
        End Try
    End Function

#End Region

#Region "Attach Activity"

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim ds As DataSet
        Dim ds_CanStart As New DataSet
        Dim ds_CanStartActvity As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim CanStartActivities As String = String.Empty
        Dim index As Integer
        Try



            'Start To check Canstart after activity on 19-Nov-2008
            wStr = " cStatusindi<>'D' and vTemplateId='" + Me.Request.QueryString("vtemplateId").Trim() + "'" + _
                    " and Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
                    Me.ddlAct.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAct.SelectedValue.Trim() + _
                    "') end)>0 "

            If Not Me.objHelp.getTemplateNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_CanStart, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Sub
            End If

            If ds_CanStart.Tables(0).Rows.Count <= 0 Then
                wStr = " Charindex(vActivityId,case (select VcanStartAfter from ActivityMst where vActivityId='" + _
                                    Me.ddlAct.SelectedValue.Trim() + "') " + " when '0000' then vActivityId else " + _
                                    " (select VcanStartAfter from ActivityMst where vActivityId='" + Me.ddlAct.SelectedValue.Trim() + _
                                    "') end)>0 "

                If Not Me.objHelp.getActivityMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        ds_CanStartActvity, eStr) Then
                    Me.ShowErrorMessage("", eStr)
                    Exit Sub
                End If

                For index = 0 To ds_CanStartActvity.Tables(0).Rows.Count - 1

                    CanStartActivities += IIf(CanStartActivities = "", "", ", ") + _
                                        "'" + ds_CanStartActvity.Tables(0).Rows(index)("vActivityName") + "'"

                Next index

                Me.objCommon.ShowAlert("Please, First Add: " + CanStartActivities + " Activities Then Only You Can Add This Activity.", Me.Page)
                Exit Sub
            End If
            '***********End Check

            AssignValue2()
            ds = New DataSet
            ds.Tables.Add(Me.ViewState(VS_dtTemplateNodeDetail1))
            ds.Tables(0).TableName = "TemplateNodeDetail"   ' New Values on the form to be updated

            If Not objLambda.Save_InsertTemplateNodeDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                                            ds, Me.Session("UserCode"), eStr) Then

                Me.ShowErrorMessage("", eStr)
                Exit Sub

            End If

            BindTree()
            TVTemplate.ExpandAll()
            'Me.divact.Visible = False
            mpeDialogAddActivity.Hide()
            Me.TVTemplate.Enabled = True

        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnOK_Click")
        End Try
    End Sub

#End Region

#Region "Create Table"
    Private Sub CreateTemplateNodeTable()
        Dim dtinacttemplatenode As New DataTable
        Dim dc As DataColumn
        Try
            dc = New DataColumn
            dc.ColumnName = "inodeId"
            dc.DataType = GetType(String)
            dtinacttemplatenode.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vTemplateId"
            dc.DataType = GetType(String)
            dtinacttemplatenode.Columns.Add(dc)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "..CreateTemplateNodeTable")

        End Try

        Me.ViewState("dtinacttemplatenode") = dtinacttemplatenode
    End Sub
#End Region

#Region "Button Click Events"

    Protected Sub btneditact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btneditact.Click
        Dim Wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dstemplatenodedetail As New DataSet
        Dim index As Integer

        If Me.TVTemplate.CheckedNodes.Count > 1 Then

            Me.objCommon.ShowAlert("Please Select Only One Activity", Me)
            Me.TVTemplate.Enabled = True
            Exit Sub

        ElseIf Me.TVTemplate.CheckedNodes.Count = 1 Then

            Wstr = " vActivityId=" + Me.ddlAct.SelectedValue() 'Value of where condition
            Wstr = " vTemplateId='" & Me.Request.QueryString("vTemplateid") & "' and iNodeId=" & _
                    Me.TVTemplate.CheckedNodes(index).ImageToolTip

            If Not objHelp.getTemplateNodeDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dstemplatenodedetail, eStr_Retu) Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_dtTemplateNodeDetail1) = dstemplatenodedetail.Tables(0)
            'Me.divact.Visible = True
            mpeDialogAddActivity.Show()
            Me.TVTemplate.Enabled = False

        End If

        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
        '                                        Me.divact.ClientID.ToString.Trim() + "');", True)
    End Sub

    Protected Sub BtnAddActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddActivity.Click
        Dim index As Integer
        'Me.divTV.Visible = True
       

        mpeaddactivity.Show()
        Me.ddlActivityGroup.SelectedIndex = -1
        Me.ddlAddActivity.Items.Clear()
        Me.GVActivityName.DataSource = Nothing
        Me.GVActivityName.DataBind()

        'If Me.TVTemplate.CheckedNodes.Count > 1 Then
        '    Me.objCommon.ShowAlert("Please Select Only One Activity", Me)
        '    Me.TVTemplate.Enabled = True
        '    Me.divTV.Visible = False
        '    Exit Sub
        'End If
        'If Me.TVTemplate.CheckedNodes.Count < 1 Then
        '    Me.objCommon.ShowAlert("Please Select Atleast One Activity", Me)
        '    Me.TVTemplate.Enabled = True
        '    Me.divTV.Visible = False
        '    Exit Sub
        'End If

        For index = 0 To Me.TVTemplate.CheckedNodes.Count - 1

            Me.ViewState("vTemplateid") = Me.TVTemplate.CheckedNodes(index).Value
            Me.ViewState("iNodeId") = Me.TVTemplate.CheckedNodes(index).ImageToolTip

        Next index
        mpeaddactivity.Show()

        'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
        '                                        Me.divTV.ClientID.ToString.Trim() + "');", True)
    End Sub

    Protected Sub btnDeleteActivity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteActivity.Click
        Dim dtupdatenode As New DataTable
        Dim dr As DataRow
        Dim ds_updatenode As New DataSet
        Dim index As Integer
        Dim index1 As Integer

        Dim ds_CanStart As New DataSet
        Dim ds_CanStartActvity As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim CanStartActivities As String = String.Empty
        Try

            
            'If Me.TVTemplate.CheckedNodes.Count < 1 Then
            '    Me.objCommon.ShowAlert("Please Select Atleast One Activity To Delete", Me)
            '    Me.TVTemplate.Enabled = True
            '    Me.divTV.Visible = False
            '    Exit Sub
            'End If

            If Me.TVTemplate.CheckedNodes(index).ImageToolTip = 1 Then
                Me.objCommon.ShowAlert("Template Should Have Atleast One Activity. So, You Can Not Delete This Activity", Me)
                Me.TVTemplate.Enabled = True
                'Me.divTV.Visible = False
                mpeaddactivity.Hide()
                Exit Sub
            End If

          

            CreateTemplateNodeTable()

            dtupdatenode = Me.ViewState("dtinacttemplatenode")

            For index = 0 To Me.TVTemplate.CheckedNodes.Count - 1

                'Start To check Canstart after activity on 19-Nov-2008
                wStr = " cStatusindi<>'D' and vTemplateId='" + Me.Request.QueryString("vtemplateId").Trim() + "'" + _
                        " and vActivityId in (select vActivityId from ActivityMst where vcanstartAfter like '%" + _
                        Me.TVTemplate.CheckedNodes(index).ToolTip + "%')"

                If Not Me.objHelp.getTemplateNodeDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        ds_CanStart, eStr) Then
                    Me.ShowErrorMessage("", eStr)
                    Exit Sub
                End If

                If ds_CanStart.Tables(0).Rows.Count > 0 Then
                    wStr = "  vcanstartAfter like '%" + _
                        Me.TVTemplate.CheckedNodes(index).ToolTip + "%'"

                    If Not Me.objHelp.getActivityMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                            ds_CanStartActvity, eStr) Then
                        Me.ShowErrorMessage("", eStr)
                        Exit Sub
                    End If

                    For index1 = 0 To ds_CanStartActvity.Tables(0).Rows.Count - 1

                        CanStartActivities += IIf(CanStartActivities = "", "", ",") + _
                                            ds_CanStartActvity.Tables(0).Rows(index1)("vActivityName")

                    Next index1

                    Me.objCommon.ShowAlert("Please First Delete: " + CanStartActivities + " Activities Then Only You Can Delete This Activity.", Me.Page)
                    Exit Sub
                End If

                '***********End Check

                dr = dtupdatenode.NewRow
                dr("vTemplateId") = Request.QueryString("vTemplateId").ToString
                dr("inodeId") = Me.TVTemplate.CheckedNodes(index).ImageToolTip
                dtupdatenode.Rows.Add(dr)

            Next index


            ds_updatenode.Tables.Add(dtupdatenode)
            ds_updatenode.Tables(0).TableName = "TemplateNodeDetail"   ' New Values on the form to be updated


            If Not objLambda.Inactive_TemplateNodeDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_updatenode, Me.Session("UserCode"), eStr) Then

                Me.ShowErrorMessage("", eStr)
                Exit Sub

            End If
            BindTree()
            TVTemplate.ExpandAll()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnDeleteActivity_Click")
        End Try
    End Sub

    Protected Sub btnuserrights_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnuserrights.Click
        Response.Redirect("frmuserRights.aspx?mode=1&TemplateId=" & Me.Request.QueryString("vTemplateid") & "&default=yes")
    End Sub

    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        Response.Redirect("frmAddTemplateMst.aspx?mode=1")
    End Sub

    Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'divTV.Visible = False
        mpeaddactivity.Hide()
        BindTree()

        TVTemplate.ExpandAll()
    End Sub

    Protected Sub btnsetuserrights_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsetuserrights.Click
        Dim index As Integer

        For index = 0 To Me.TVTemplate.CheckedNodes.Count - 1

            Me.ViewState("vTemplateid") = Me.TVTemplate.CheckedNodes(index).Value
            Me.ViewState("iNodeId") = Me.TVTemplate.CheckedNodes(index).ImageToolTip

        Next index

        Response.Redirect("frmEditUserRights.aspx?mode=1&TemplateId=" & Me.Request.QueryString("vTemplateid") & _
                            "&NodeId=" & Me.ViewState("iNodeId") & "&Page2=frmtreenodeMst")

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click

        'Me.divact.Visible = False
        mpeDialogAddActivity.Hide()
        Me.TVTemplate.Enabled = True
    End Sub

#End Region

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