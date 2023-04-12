Imports VB = Microsoft.VisualBasic
Imports Newtonsoft.Json
Partial Class frmMedExTemplateMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()


    Private eStr As String = String.Empty

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtBlankMedExTemplateDtl As String = "DtBlankMedExTemplateDtl"
    Private Const VS_MedexCode As String = "MedexCode"
    Private Const VS_MedExOrg As String = "DtMedExOrg"
    Private Const VS_MedExTemplateId As String = "MedExTemplateId"
    Private Const VS_AllMedex As String = "AllMedexCode"

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not IsPostBack Then
                GenCall()
            End If

            If (Me.ddlProjectType.SelectedIndex > 0 Or Me.ddlMedExGroup.SelectedIndex > 0 Or Me.txtTemplate.Text.Trim.Length > 0) And Me.HTemplateId.Value.Trim.Length() <= 0 Then
                Me.btnsave.Visible = True
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        'Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try



            ' Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Me.Request.QueryString("Mode")   'To use it while saving

            'If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            '    Me.ViewState(VS_MedexCode) = Me.Request.QueryString("Value").ToString
            'End If

            If Not GenCall_Data() Then ' For Data Retrieval
                Exit Function
            End If

            'Me.ViewState(VS_DtBlankMedExTemplateDtl) = ds.Tables(0)   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data() As Boolean

        'Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        Dim dsBlankFormula As New DataSet
        Dim StrFormula As String = String.Empty
        Try



            'Val = Me.ViewState(VS_MedexCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            'If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            '    wStr = "1=2"
            'Else
            '    wStr = "vMedExCode=" + Me.Request.QueryString("Value").ToString
            'End If
            If Not GetBlankFormula() Then
                objcommon.ShowAlert("Error While Getting Blank Formula Structure", Me.Page)
                Exit Function
            End If
            
            If Not objhelp.GetMedExTemplateDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds, eStr_Retu) Then
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found For Selected Operation")
            End If

            'ds_DWR_Retu = ds
            Me.ViewState(VS_DtBlankMedExTemplateDtl) = ds.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........GenCall_Data")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try

            'Page.Title = ":: Attribute Template Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            'CType(Me.Master.FindControl("lblHeading"), Label).Text = "Attribute Template Master"

            Page.Title = ":: Template Designing Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Template Designing Master"

            dt_OpMst = Me.ViewState(VS_DtBlankMedExTemplateDtl)

            Choice = Me.ViewState(VS_Choice)



            If Not fillddlMedExGroup() Then
                Return False
            End If

            If Not fillddlMedExSubGroup() Then
                Return False
            End If

            If Not FillProjectType() Then
                GenCall_ShowUI = False
                Exit Function
            End If

            'Me.pnlorg.Visible = False
            'Me.btnintocopy.Visible = False
            'Me.btnfrmcpy.Visible = False

            'Me.ImgBtnLeft.Visible = False
            'Me.ImgBtnRight.Visible = False

            'Me.ImgBtnLeft.OnClientClick = "return CheckOneOnly();"
            'Me.ImgBtnRight.OnClientClick = "return CheckOneOnly();"

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillProjectType"

    Private Function FillProjectType() As Boolean
        Dim ds_ProjectType As New Data.DataSet
        Dim dv_ProjectType As New Data.DataView
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Try
            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(Wstr) Then
                Return False
            End If
            'Wstr = "nScopeNo=" & Me.Session(S_ScopeNo)

            If Not objhelp.GetviewProjectTypeMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                ds_ProjectType, estr) Then
                Return False
            End If

            dv_ProjectType = ds_ProjectType.Tables(0).DefaultView.ToTable(True, "vProjectTypeCode,vProjectTypeName".Split(",")).DefaultView
            dv_ProjectType.Sort = "vProjectTypeName"
            Me.ddlProjectType.DataSource = dv_ProjectType
            Me.ddlProjectType.DataValueField = "vProjectTypeCode"
            Me.ddlProjectType.DataTextField = "vProjectTypeName"
            Me.ddlProjectType.DataBind()
            Me.ddlProjectType.Items.Insert(0, New ListItem("Select Project Type", "0"))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........FillProjectType")
            Return False
        End Try
    End Function

#End Region

#Region "Dropdown Selected Index Changed"

    Protected Sub ddlMedExSubGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)


        If Not AddDeletedMedEx() Then
            Exit Sub
        End If

        If Not AddNewMedEx() Then
            Exit Sub
        End If

        If Not SerilazeMedEx() Then
            Exit Sub
        End If

        BindchkdupMedEx()
        
        CheckBoxList()
    End Sub

    Protected Sub ddlMedExGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMedExGroup.SelectedIndexChanged
        If Me.ddlProjectType.SelectedIndex = 0 Then
            objcommon.ShowAlert("Please Select Project Type", Me.Page)
            Me.ddlMedExGroup.SelectedIndex = 0
            Exit Sub
        End If

        If Me.txtTemplate.Text.Trim = "" Then
            objcommon.ShowAlert("Please Enter/Add Template", Me.Page)
            Me.ddlMedExGroup.SelectedIndex = 0
            Exit Sub
        End If

        fillddlMedExSubGroup()

        If Not AddDeletedMedEx() Then
            Exit Sub
        End If

        If Not AddNewMedEx() Then
            Exit Sub
        End If

        If Not SerilazeMedEx() Then
            Exit Sub
        End If

        BindchkdupMedEx()
        CheckBoxList()

        Me.lblSeq.Text = "Attribute Group Name -" + Me.ddlMedExGroup.SelectedItem.Text.ToString()

    End Sub

    Protected Sub ddlProjectType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProjectType.SelectedIndexChanged
        Dim ContexKeyString As String = String.Empty

        If Me.ddlProjectType.SelectedIndex > 0 Then
            ContexKeyString = " vProjectTypeCode = '" & Me.ddlProjectType.SelectedValue & "' AND "
            Me.AutoCompleteExtender1.ContextKey = ContexKeyString
        End If

    End Sub

#End Region

#Region "Fill Controls"

    Private Function fillddlMedExGroup() As Boolean
        Dim dsMedExGroupMst As New DataSet
        Dim dvMedExGroupMst As New DataView
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty

        Try
            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(Wstr) Then
                Return False
            End If
            'Wstr = "nScopeNo=" & Me.Session(S_ScopeNo)

            If Not Me.objhelp.GetMedExGroupMst("cActiveFlag <>'N' And cStatusIndi<>'D' And " + Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsMedExGroupMst, estr) Then

                Me.objcommon.ShowAlert("Error while getting Data from Attribute GroupMst.", Me.Page)
                Return False

            End If

            dvMedExGroupMst = dsMedExGroupMst.Tables(0).DefaultView
            dvMedExGroupMst.Sort = "vMedExGroupDesc"
            Me.ddlMedExGroup.DataSource = dvMedExGroupMst.ToTable()
            Me.ddlMedExGroup.DataValueField = "vMedExGroupCode"
            Me.ddlMedExGroup.DataTextField = "vMedExGroupDesc"
            Me.ddlMedExGroup.DataBind()
            Me.ddlMedExGroup.Items.Insert(0, New ListItem("Select Attribute Group", 0))
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....fillddlMedExGroup")
        Finally
        End Try

    End Function

    Private Function fillddlMedExSubGroup() As Boolean
        Dim dsMedExSubGroupMst As New DataSet
        Dim dvMedExSubGroupMst As New DataView
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty

        Try
            If Me.ddlMedExGroup.SelectedIndex <= 0 Then
                Me.ddlMedExSubGroup.Items.Clear()
                Me.ddlMedExSubGroup.Items.Insert(0, New ListItem("Select Attribute Sub Group", 0))
                Return True
            End If

            Me.ddlMedExSubGroup.Items.Clear()

            'Wstr = "cActiveFlag <> 'N' And cStatusIndi <> 'D'"
            ' only subgroup of selected medex group
            Wstr = "vMedExSubGroupCode In (Select medexmst.vMedExSubGroupCode From medexmst Where medexmst.vMedExGroupCode = '" & Me.ddlMedExGroup.SelectedValue.Trim() & "') And cActiveFlag <> 'N' And cStatusIndi <> 'D'"


            If Not Me.objhelp.GetMedExSubGroupMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            dsMedExSubGroupMst, estr) Then

                Me.objcommon.ShowAlert("Error while getting Data from Attribute SubGroupMst.", Me.Page)
                Return False

            End If

            If dsMedExSubGroupMst.Tables(0).Rows.Count > 0 Then
                dvMedExSubGroupMst = dsMedExSubGroupMst.Tables(0).DefaultView
                dvMedExSubGroupMst.Sort = "vMedExSubGroupDesc"
                Me.ddlMedExSubGroup.DataSource = dvMedExSubGroupMst.ToTable()
                Me.ddlMedExSubGroup.DataValueField = "vMedExSubGroupCode"
                Me.ddlMedExSubGroup.DataTextField = "vMedExSubGroupDesc"
                Me.ddlMedExSubGroup.DataBind()
                Me.ddlMedExSubGroup.SelectedValue = dsMedExSubGroupMst.Tables(0).Rows(0).Item("vMedExSubGroupCode")
            End If
            Me.ddlMedExSubGroup.Items.Insert(0, New ListItem("Select Attribute Sub Group", 0))
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....fillddlMedExSubGroup")
        Finally
        End Try


    End Function

    Private Sub CheckBoxList()
        Dim eStr_Retu As String = String.Empty
        Dim Ds_FillCheck As New DataSet
        Dim ds As New DataSet
        'Dim Dupindex As Integer
        ' Dim Orgindex As Integer
        Dim wStr As String = String.Empty
        Dim dv As New DataView
        Dim dt_DtMedExDtl As New DataTable
        Dim dt_ExistMedExDtl As New DataTable
        Dim dt_SortDtMedExDtl As New DataTable


        Try


            wStr = "cActiveFlag<>'N' and vMedExGroupCode='" & Me.ddlMedExGroup.SelectedValue.Trim() & "'"
            If Me.chkAllSubGroups.Checked = False Then
                wStr += " And vMedExSubGroupCode='" & Me.ddlMedExSubGroup.SelectedValue.Trim() & "'"
            End If
            wStr += " order by vMedExDesc desc"

            If objhelp.GetMedExMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                Ds_FillCheck, eStr_Retu) Then

                'If objhelp.GetMedExMst("1=1", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                '                    ds, eStr_Retu) Then


                'Me.ViewState(VS_AllMedex) = ds.Tables(0).Copy
                'dv = ds.Tables(0).DefaultView
                'dv.RowFilter = wStr
                'dv.Sort = "vMedExDesc"
                'Dim dt_filterMedxCode As DataTable = dv.ToTable
                'Ds_FillCheck.Tables.Add(dt_filterMedxCode)
                'Ds_FillCheck.AcceptChanges()


                ' Me.pnlorg.Visible = True
                'Me.btnintocopy.Visible = True
                'Me.btnfrmcpy.Visible = True
                'Me.chkOrgMedex.Visible = True

                'Me.ViewState(VS_MedExOrg) = Ds_FillCheck.Tables(0).DefaultView.ToTable(True, "MedExDescWithSubGroup,vMedExDesc,vMedExCode,vDefaultValue,cActiveFlag,vMedExType,vMedexCodeForFormula,vMedexFormula,vmedexsubGroupDesc,vmedexgroupDesc".Split(","))
                dt_DtMedExDtl = Ds_FillCheck.Tables(0).DefaultView.ToTable(True, "MedExDescWithSubGroup,vMedExDesc,vMedExCode,vDefaultValue,cActiveFlag,vMedExType,vMedexCodeForFormula,vMedexFormula,vmedexsubGroupDesc,vmedexgroupDesc".Split(","))

                'Me.chkOrgMedex.DataSource = Ds_FillCheck.Tables(0).DefaultView.ToTable(True, "MedExDescWithSubGroup,vMedExDesc,vMedExCode,vDefaultValue,cActiveFlag,vMedExType,vMedexCodeForFormula,vMedexFormula".Split(","))
                'Me.chkOrgMedex.DataValueField = "vMedExCode"
                'Me.chkOrgMedex.DataTextField = "MedExDescWithSubGroup"
                'Me.chkOrgMedex.DataBind()

                'For Dupindex = Me.chkdupMedEx.Items.Count - 1 To 0 Step -1

                '    For Orgindex = Me.chkOrgMedex.Items.Count - 1 To 0 Step -1

                '        If Me.chkdupMedEx.Items(Dupindex).Value = Me.chkOrgMedex.Items(Orgindex).Value Then
                '            Me.chkOrgMedex.Items.RemoveAt(Orgindex)
                '        End If

                '    Next Orgindex

                'Next Dupindex

                dt_ExistMedExDtl = CType(Me.ViewState(VS_DtBlankMedExTemplateDtl), DataTable)
                If Not dt_ExistMedExDtl Is Nothing Then
                    If dt_ExistMedExDtl.Rows.Count > 0 Then
                        For i As Integer = 0 To dt_ExistMedExDtl.Rows.Count - 1
                            For j As Integer = 0 To dt_DtMedExDtl.Rows.Count - 1
                                If dt_DtMedExDtl.Rows(j).Item("vMedExCode") = dt_ExistMedExDtl.Rows(i).Item("vMedExCode") Then
                                    dt_DtMedExDtl.Rows.Remove(dt_DtMedExDtl.Rows(j))
                                    dt_DtMedExDtl.AcceptChanges()
                                    Exit For
                                End If
                            Next
                        Next
                    End If
                End If
                Me.ViewState(VS_MedExOrg) = dt_DtMedExDtl
                dt_DtMedExDtl.DefaultView.Sort = "vMedExDesc"
                dt_SortDtMedExDtl = dt_DtMedExDtl.DefaultView.ToTable()
                For noOfrows As Integer = 0 To dt_SortDtMedExDtl.Rows.Count - 1
                    Dim li As New HtmlGenericControl("li")
                    Dim div As New HtmlGenericControl("div")
                    li.Attributes.Add("class", "ui-state-default")
                    'li.Style.Add("cursor", "move")
                    li.Style.Add("text-align", "left")
                    li.Style.Add("margin-bottom", "1%")
                    li.Style.Add("margin-left", "2%")
                    li.Style.Add("height", "40px")
                    li.Style.Add("cursor", "pointer")
                    li.Style.Add("border-radius", "7px 7px 7px 7px")
                    div.Attributes.Add("class", "innerdiv")
                    div.Style.Add("font-size", "0.9em")
                    div.Style.Add("font-weight", "bold")
                    div.Style.Add("margin-left", "5%")
                    div.Style.Add("line-height", "3.2em")
                    div.Style.Add("height", "35px")
                    div.Style.Add("overflow", "hidden")
                    div.InnerText = dt_SortDtMedExDtl.Rows(noOfrows)("vMedExDesc")
                    li.ID = "mev_" + dt_SortDtMedExDtl.Rows(noOfrows)("vMedExCode")
                    li.Attributes.Add("title", dt_SortDtMedExDtl.Rows(noOfrows)("vMedExDesc") + " (" + dt_SortDtMedExDtl.Rows(noOfrows)("vmedexgroupDesc") + ") " + "[" + dt_SortDtMedExDtl.Rows(noOfrows)("vmedexsubGroupDesc") + "] " + "(" + dt_SortDtMedExDtl.Rows(noOfrows)("vMedexType") + ") ")
                    li.Controls.Add(div)
                    li.Attributes.Add("class", "allmed")
                    Me.SeqMedex.Controls.Add(li)

                Next


                Me.SeqMedex.Attributes.Add("class", "quad")

                Me.pnltable2.Style.Add("display", "")
                Me.pheader2.Style.Add("display", "")

            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "............. CheckBoxList")
        End Try
    End Sub

#End Region

#Region "Button Events"

    Protected Sub btnedit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnedit.Click
        Dim eStr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim Ds_TemplateName As New DataSet
        Dim Dt_TemplateName As New DataTable

        Try


            Me.HdnNewAddedMedex.Value = ""
            Me.HdnExistMedex.Value = ""
            Me.HdnBlankMedex.Value = ""
            Me.HdnSequenceMedex.Value = ""
            Me.HdnExistMedexRemoved.Value = ""

            If Not GetBlankFormula() Then
                objcommon.ShowAlert("Error While Getting Blank Formula Structure", Me.Page)
                Exit Sub
            End If

            Me.ViewState(VS_MedExTemplateId) = Me.HTemplateId.Value
            Wstr = "vMedExTemplateId='" & Me.HTemplateId.Value & "' order by iSeqNo"

            If objhelp.GetMedExTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_TemplateName, eStr) Then

                If Ds_TemplateName.Tables(0).Rows.Count > 0 Then
                    Dt_TemplateName = Ds_TemplateName.Tables(0)
                End If

                Me.ViewState(VS_DtBlankMedExTemplateDtl) = Ds_TemplateName.Tables(0)
                'Me.Pnldup.Visible = True
                'Me.chkdupMedEx.Visible = True
                'Me.ImgBtnLeft.Visible = True
                'Me.ImgBtnRight.Visible = True

            End If

            Me.btnupdate.Visible = True
            'Me.btnnew.Visible = True
            Me.btnsave.Visible = False
            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

            If Not AddNewMedEx() Then
                Exit Sub
            End If

            BindchkdupMedEx()
            CheckBoxList()
            Me.lblTemp.Text = "Template Name -" + Me.txtTemplate.Text.Trim.ToString()
        Catch ex As Exception
            ShowErrorMessage(ex.Message, " ......btnedit_Click")
        End Try
    End Sub

    'Protected Sub btnnew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnnew.Click
    '    Response.Redirect("frmMedExTemplateMst.aspx?mode=1")
    'End Sub

    Protected Sub btnupdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = String.Empty
        Dim ds_TemplateUpdate As New DataSet
        Dim dt_TemplateUpdate As New DataTable
        Dim dt_TemplateBlankUpdate As New DataTable
        Dim strupdateMedExCode As String = String.Empty
        Dim strupdateMedExDesc As String = String.Empty
        Me.ViewState(VS_Choice) = 2
        Dim ds_Delete As New DataSet
        Me.ViewState(VS_MedExTemplateId) = Me.HTemplateId.Value
        Try

            If Not AddDeletedMedEx() Then
                Exit Sub
            End If


            If Not AddNewMedEx() Then
                Exit Sub
            End If

            If Not SerilazeMedEx() Then
                Exit Sub
            End If

            dt_TemplateUpdate = CType(Me.ViewState(VS_DtBlankMedExTemplateDtl), DataTable)
            ds_TemplateUpdate = New DataSet
            ds_TemplateUpdate.Tables.Add(dt_TemplateUpdate)
            ds_TemplateUpdate.Tables(0).TableName = "View_MedExTemplateDtl"

            'For Validation of Duplication template :Start
            If Not IsDuplicate() Then
                Exit Sub
            End If
            'For Validation of Duplication template :Start

            ' to check the value if changed after data is changed : Start
            If dt_TemplateUpdate.Rows(0).Item("vTemplateName") <> Me.txtTemplate.Text.Trim() Then
                ChangeDataset(dt_TemplateUpdate, "vTemplateName", Me.txtTemplate.Text.Trim())
            End If
            If dt_TemplateUpdate.Rows(0).Item("vProjectTypeCode") <> Me.ddlProjectType.SelectedValue.Trim() Then
                ChangeDataset(dt_TemplateUpdate, "vProjectTypeCode", Me.ddlProjectType.SelectedValue.Trim())
            End If
            ' to check the value if changed after data is changed : End

            If Not objLambda.Save_MedExTemplateDtl(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExTemplateDtl, ds_TemplateUpdate, Session(S_UserID), eStr) Then

                objcommon.ShowAlert("Error While Saving Attribute Template Dtl", Me.Page)
                Exit Sub

            End If

            objcommon.ShowAlert("Attribute Template Updated SuccessFully", Me.Page)
            ResetPage()
            'Me.Pnldup.Visible = False
            'Me.ImgBtnLeft.Visible = False
            'Me.ImgBtnRight.Visible = False
            'Me.btnnew.Visible = False

            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            Me.pheader2.Style.Add("display", "none")
            Me.pnltable2.Style.Add("display", "none")

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "......btnupdate_Click")
        End Try
    End Sub

    'Protected Sub btnintocopy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim cntIndex As Integer = 0
    '    Dim ArrlstIndex As New ArrayList
    '    Dim index As Integer
    '    Dim dtmedextemplate As DataTable = Nothing
    '    Dim eStr_Retu As String = String.Empty
    '    Dim Dt_Fill As New DataTable
    '    Dim Dt_MedexMst As New DataTable
    '    Dim Dv_MedexMst As New DataView
    '    Dim Dv_MedexMstFormula As New DataView
    '    Dim Dr As DataRow
    '    Dim lstItem As ListItem
    '    Try

    '        

    '        Dt_Fill = CType(Me.ViewState(VS_DtBlankMedExTemplateDtl), DataTable)

    '        For index = 0 To Me.chkOrgMedex.Items.Count - 1 ' Move Records from Destination to Source

    '            If Me.chkOrgMedex.Items(index).Selected = True Then

    '                lstItem = New ListItem
    '                lstItem = Me.chkOrgMedex.Items(index)
    '                Dt_MedexMst = CType(Me.ViewState(VS_MedExOrg), DataTable)
    '                Dv_MedexMst = Dt_MedexMst.DefaultView()
    '                Dv_MedexMst.RowFilter = "vMedExCode = '" + Me.chkOrgMedex.Items(index).Value + "'"

    '                If Dv_MedexMst.ToTable.Rows(0).Item("vMedexType").ToString = "Formula" Then
    '                    Dv_MedexMst.RowFilter = "vMedExCode = '" + Me.chkOrgMedex.Items(index).Value + "'and vMedexType='Formula' and ( vMedexFormula is  null or  vMedexFormula='') "
    '                    If Dv_MedexMst.ToTable.Rows.Count > 0 Then
    '                        objcommon.ShowAlert("You cannot Add blank  Formula", Me.Page)
    '                        Exit Sub
    '                    End If
    '                End If
    '                Dr = Dt_Fill.NewRow()
    '                'nMedExTemplateDtlNo, vMedExTemplateId, vTemplateName, iSeqNo, vMedExCode, vMedExDesc, vMedExValues, vMedExSubGroupCode, 
    '                'vDefaultValue, vHighRange, vLowRange, vAlertonvalue, vAlertMessage, cAlertType, cActiveFlag, cStatusIndi, iModifyBy
    '                Dr("nMedExTemplateDtlNo") = Dt_Fill.Rows.Count + 1
    '                Dr("vMedExTemplateId") = "0001"

    '                Dr("cStatusIndi") = "N"

    '                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
    '                    Dr("vMedExTemplateId") = Me.ViewState(VS_MedExTemplateId)
    '                    Dr("cStatusIndi") = "E"
    '                End If

    '                Dr("vTemplateName") = Me.txtTemplate.Text
    '                Dr("iSeqNo") = 1
    '                If Dt_Fill.Rows.Count > 0 Then
    '                    Dr("iSeqNo") = Dt_Fill.Compute("Max(iSeqNo)", "1=1") + 1
    '                End If
    '                Dr("vProjectTypeCode") = Me.ddlProjectType.SelectedValue.Trim()
    '                Dr("vMedExCode") = Me.chkOrgMedex.Items(index).Value
    '                Dr("vMedExDesc") = Me.chkOrgMedex.Items(index).Text
    '                Dr("MedExDescWithSubGroup") = Me.chkOrgMedex.Items(index).Text
    '                Dr("vMedExValues") = Dv_MedexMst.Table.Rows(0).Item("vDefaultValue")
    '                Dr("cActiveFlag") = "Y"
    '                Dr("iModifyBy") = Session(S_UserID)
    '                Dt_Fill.Rows.Add(Dr)
    '                Dt_Fill.AcceptChanges()

    '                ArrlstIndex.Add(index)

    '            End If
    '        Next index

    '        For index = ArrlstIndex.Count - 1 To 0 Step -1
    '            Me.chkOrgMedex.Items.RemoveAt(ArrlstIndex(index))
    '        Next index

    '        Dt_Fill.DefaultView.Sort = "iSeqNo"
    '        Me.ViewState(VS_DtBlankMedExTemplateDtl) = Dt_Fill.DefaultView.ToTable()

    '        Me.Pnldup.Visible = True
    '        'Me.ImgBtnLeft.Visible = True
    '        'Me.ImgBtnRight.Visible = True

    '        BindchkdupMedEx()

    '    Catch ex As Exception
    '        ShowErrorMessage(ex.Message, "... btnintocopy_Click")
    '    End Try
    'End Sub

    'Protected Sub btnfrmcpy_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim Dt_Fill As New DataTable
    '    Dim Tblindex As Integer
    '    Dim Deleted As Boolean = False

    '    Dt_Fill = CType(Me.ViewState(VS_DtBlankMedExTemplateDtl), DataTable)

    '    For index As Integer = Me.chkdupMedEx.Items.Count - 1 To 0 Step -1 'Copy from one checkboxlist to another(Source To Destination) 

    '        If Me.chkdupMedEx.Items(index).Selected = True Then
    '            Dim lstitem As ListItem = Me.chkdupMedEx.Items(index)

    '            For Tblindex = 0 To Dt_Fill.Rows.Count - 1

    '                If Dt_Fill.Rows(Tblindex).Item("vMedExCode") = Me.chkdupMedEx.Items(index).Value Then

    '                    Dt_Fill.Rows(Tblindex).Item("cActiveFlag") = "N"
    '                    Dt_Fill.Rows(Tblindex).Item("iSeqNo") = 0
    '                    Dt_Fill.Rows(Tblindex).Item("cStatusIndi") = "D"
    '                    Deleted = True
    '                    Continue For

    '                End If

    '                If Deleted = True Then

    '                    Dt_Fill.Rows(Tblindex).Item("iSeqNo") = Dt_Fill.Rows(Tblindex).Item("iSeqNo") - 1

    '                End If

    '            Next Tblindex

    '            Me.chkOrgMedex.Items.Add(lstitem)
    '        End If

    '    Next index

    '    Dt_Fill.DefaultView.Sort = "iSeqNo"
    '    Me.ViewState(VS_DtBlankMedExTemplateDtl) = Dt_Fill.DefaultView.ToTable()
    '    BindchkdupMedEx()

    '    If Me.chkdupMedEx.Items.Count = 0 Then
    '        Me.Pnldup.Visible = False
    '        'Me.ImgBtnLeft.Visible = False
    '        'Me.ImgBtnRight.Visible = False
    '    End If

    'End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMedExTemplateMst.aspx?mode=1")
    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Dim ds As New DataSet
        Dim ds_save As New DataSet
        Dim Ds_Empty As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim strMedExCode As String = String.Empty
        Dim strMedExDesc As String = String.Empty
        Dim ds_Template As New DataSet
        Dim Dt_Template As New DataTable
        Dim Dt_Templatefirsttry As New DataTable
        Dim Wstr As String = String.Empty
        Me.ViewState(VS_Choice) = 1
        Dim dv_Medex As New DataView
        Dim dt_Medex As New DataTable
        Dim dt_vMedexCodeForFormula As DataTable = Nothing
        Dim vMedexcode As String = String.Empty
        Dim ds_require As New DataSet
        Try

           

            If Not AddNewMedEx() Then
                Exit Sub
            End If

            If Not SerilazeMedEx() Then
                Exit Sub
            End If

            'For Validation of Duplication template :Start
            If Not IsDuplicate() Then
                Exit Sub
            End If
            'For Validation of Duplication template :Start

            Dt_Template = CType(Me.ViewState(VS_DtBlankMedExTemplateDtl), DataTable)
            If Not Dt_Template.Rows.Count > 0 Then
                objcommon.ShowAlert("No Attribute Found To Be Saved In DataBAse", Me.Page)
                Me.ddlMedExGroup.SelectedIndex = 0
                Me.ddlMedExSubGroup.SelectedIndex = 0
                Me.pnltable2.Style.Add("display", "none")
                Me.pheader2.Style.Add("display", "none")
                Me.chkAllSubGroups.Checked = False
                Exit Sub
            End If
            Dt_Template.Rows(0).Item("vMedEXTemplateId") = "0000"

            ' to check the value if changed after data is changed : Start
            If Dt_Template.Rows(0).Item("vTemplateName") <> Me.txtTemplate.Text.Trim() Then
                ChangeDataset(Dt_Template, "vTemplateName", Me.txtTemplate.Text.Trim())
            End If
            If Dt_Template.Rows(0).Item("vProjectTypeCode") <> Me.ddlProjectType.SelectedValue.Trim() Then
                ChangeDataset(Dt_Template, "vProjectTypeCode", Me.ddlProjectType.SelectedValue.Trim())
            End If
            ' to check the value if changed after data is changed : End

            Dt_Template.AcceptChanges()

            ds_Template = New DataSet
            ds_Template.Tables.Add(Dt_Template)
            ds_Template.Tables(0).TableName = "View_MedExTemplateDtl"
            '============================================================================
            Dim ds_All As New DataSet
            If Not objhelp.GetMedExMst("1=1", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_All, eStr) Then
                objcommon.ShowAlert("Error While getting Data from view_MedexMst:" & eStr_Retu, Me.Page)
            End If


            'dt_vMedexCodeForFormula = Me.ViewState(VS_AllMedex)
            dt_vMedexCodeForFormula = ds_All.Tables(0)

            For i As Integer = 0 To ds_Template.Tables(0).Rows.Count - 1
                vMedexcode += "'" + ds_Template.Tables(0).Rows(i).Item("vMedexCode").ToString.Trim + "',"
            Next
            If Not vMedexcode = "" Then
                vMedexcode = vMedexcode.Substring(0, vMedexcode.LastIndexOf(","))
            End If
            Dim dr2 As DataRow() = dt_vMedexCodeForFormula.Select("vMedexCode in(" + vMedexcode + ")")
            'For Each row As DataRow In dr2

            'Next
            Dim dv_co As New DataView(dt_vMedexCodeForFormula)
            dv_co.RowFilter = "vMedexCode in(" + vMedexcode + ") and vMedexType='Formula' and vMedexCodeForFormula is Not Null"
            Dim dt_filterMedxCode As DataTable = dv_co.ToTable
            Dim ds_FilterMedex As New DataSet
            ds.Tables.Add(dt_filterMedxCode)
            ds.AcceptChanges()
            Dim vMedexCodeUsed As String = String.Empty

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                vMedexCodeUsed += ds.Tables(0).Rows(i).Item("vMedexCodeForFormula")
            Next
            If Not vMedexCodeUsed = "" Then
                vMedexCodeUsed = vMedexCodeUsed.Substring(0, vMedexCodeUsed.LastIndexOf(","))
            End If
            If vMedexCodeUsed <> "" Then


                Dim arrvMedxCode() As String
                Dim Arr As New ArrayList
                arrvMedxCode = vMedexCodeUsed.Split(",")
                For nDx As Integer = 0 To arrvMedxCode.Length - 1
                    If Not Arr.Contains(arrvMedxCode(nDx)) Then
                        Arr.Add(arrvMedxCode(nDx))
                    End If
                Next
                Dim vMedex As String = String.Empty
                Dim ArrMedex As New ArrayList
                ' Dim dataview As New DataView
                ' dataview = ds_Template.Tables(0).DefaultView
                For Index1 As Integer = 0 To Arr.Count - 1
                    'For Index2 As Integer = 0 To ds_Template.Tables(0).Rows.Count - 1
                    'If Not ds_Template.Tables(0).Rows(Index2).Item("vMedexCode") = Arr(Index1) Then
                    ds_Template.Tables(0).DefaultView().RowFilter = "vMedexCode='" + Arr(Index1) + "'"
                    If ds_Template.Tables(0).DefaultView().ToTable.Rows.Count = 0 Then
                        vMedex += "'" + Arr(Index1) + "',"
                    End If

                    'If Not ArrMedex.Contains("'" + Arr(Index1) + "',") Then
                    '    ArrMedex.Add("'" + Arr(Index1) + "',")
                    '    vMedex += "'" + Arr(Index1) + "',"
                    'End If
                    'End If
                    'Next
                Next
                'Dim ds_require As New DataSet
                Dim medexstring As String = String.Empty
                If Not vMedex = "" Then
                    vMedex = vMedex.Substring(0, vMedex.LastIndexOf(","))
                    Dim dv_New As New DataView(dt_vMedexCodeForFormula)
                    dv_New.RowFilter = "vMedexCode in(" + vMedex + ")"
                    Dim dt_filter As DataTable = dv_New.ToTable
                    ds_require.Tables.Add(dt_filter)
                    ds_require.AcceptChanges()

                    'If Not objhelp.GetMedExMst("vMedexCode in(" + vMedex + ")", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_require, eStr) Then
                    '    objcommon.ShowAlert("Error While getting Data from view_MedexMst:" & eStr_Retu, Me.Page)
                    'End If
                    If ds_require.Tables(0).Rows.Count > 0 Then
                        For index As Integer = 0 To ds_require.Tables(0).Rows.Count - 1
                            medexstring += ds_require.Tables(0).Rows(index).Item("MedExDescWithSubGroup").ToString + " and "
                        Next
                    End If
                    medexstring = medexstring.Substring(0, medexstring.LastIndexOf("and"))
                    'Me.objcommon.ShowAlert("Please Add Require Attribute For Formula :" + medexstring, Me.Page())
                    Me.objcommon.ShowAlert("Please Add Require Attribute For Formula", Me.Page())
                    Exit Sub
                End If
            End If
            'Dim dv As New DataView(dt_vMedexCodeForFormula)
            'dv.RowFilter = "vMedexCode in(" + vMedex + ")"
            'Dim ds_require As New DataSet
            'Dim dt_require As DataTable
            'Dim medexstring As String = String.Empty
            'dt_require = dv_Medex.ToTable
            'ds_require.Tables.Add(dt_require)
            'For index As Integer = 0 To ds_require.Tables(0).Rows.Count - 1
            '    ' medexstring = ds_require.Tables(0).Rows(index).items
            'Next



            '============================================================================

            If Not objLambda.Save_MedExTemplateDtl(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExTemplateDtl, ds_Template, Session(S_UserID), eStr_Retu) Then

                objcommon.ShowAlert("Error While Saving Attribute Template Dtl:" & eStr_Retu, Me.Page)
                Exit Sub

            End If

            objcommon.ShowAlert("Attribute Template Saved SuccessFully", Me.Page)
            ResetPage() 'this change is due to Reset dupcheckbox after save
            'Me.Pnldup.Visible = False

            'Me.ImgBtnLeft.Visible = False
            'Me.ImgBtnRight.Visible = False

        Catch threadEx As Threading.ThreadAbortException

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....btnsave_Click")
        End Try
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Helper Function "

    Private Sub BindchkdupMedEx()
        Dim Dt_FillChkDup As New DataTable
        Dim Dv_FillChkDup As New DataView
        Dim index As Integer
        Dim dt_DtMedExDtl As New DataTable
        Dim Strcrfsubdtl As String = String.Empty
        Try

            

            Dt_FillChkDup = CType(Me.ViewState(VS_DtBlankMedExTemplateDtl), DataTable)

            If Me.ViewState(VS_Choice) = WS_HelpDbTable.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                For index = Dt_FillChkDup.Rows.Count - 1 To 0 Step -1

                    If Dt_FillChkDup.Rows(index).Item("cActiveFlag") = "N" Then
                        Dt_FillChkDup.Rows(index).Delete()
                    End If

                Next index

            End If

            If Dt_FillChkDup.Rows.Count <= 0 Then
                'Me.chkdupMedEx.Items.Clear()
                Exit Sub
            End If
            Dv_FillChkDup = Dt_FillChkDup.DefaultView()
            Dv_FillChkDup.RowFilter = "cActiveFlag<>'N'"
            Dv_FillChkDup.Sort = "iSeqNo"
            'Me.chkdupMedEx.Visible = True
            'Me.chkdupMedEx.Items.Clear()
            'Me.chkdupMedEx.DataSource = Dv_FillChkDup.ToTable()
            'Me.chkdupMedEx.DataValueField = "vMedExCode"
            'Me.chkdupMedEx.DataTextField = "vMedExDesc"
            'Me.chkdupMedEx.DataTextField = "MedExDescWithSubGroup"
            'Me.chkdupMedEx.DataBind()
            dt_DtMedExDtl = Dv_FillChkDup.ToTable

            For noOfrows As Integer = 0 To dt_DtMedExDtl.Rows.Count - 1
                Dim li As New HtmlGenericControl("li")
                Dim div As New HtmlGenericControl("div")
                li.Attributes.Add("class", "ui-state-default")
                'li.Style.Add("cursor", "move")
                li.Style.Add("text-align", "left")
                li.Style.Add("margin-bottom", "1%")
                li.Style.Add("margin-left", "2%")
                li.Style.Add("height", "40px")
                li.Style.Add("cursor", "pointer")
                li.Style.Add("border-radius", "7px 7px 7px 7px")
                div.Attributes.Add("class", "innerdiv")
                div.Style.Add("font-size", "0.9em")
                div.Style.Add("font-weight", "bold")
                div.Style.Add("margin-left", "5%")
                div.Style.Add("line-height", "3.2em")
                div.Style.Add("height", "35px")
                div.Style.Add("overflow", "hidden")
                div.InnerText = dt_DtMedExDtl.Rows(noOfrows)("vMedExDesc")
                li.ID = dt_DtMedExDtl.Rows(noOfrows)("vMedExCode")
                li.Attributes.Add("title", dt_DtMedExDtl.Rows(noOfrows)("vMedExDesc") + " (" + dt_DtMedExDtl.Rows(noOfrows)("vmedexgroupDesc") + ") " + "[" + dt_DtMedExDtl.Rows(noOfrows)("vmedexsubGroupDesc") + "] " + "(" + dt_DtMedExDtl.Rows(noOfrows)("vMedexType") + ") ")
                li.Controls.Add(div)
                li.Attributes.Add("class", "Savemed")
                Me.TempMedex.Controls.Add(li)
                Strcrfsubdtl += dt_DtMedExDtl.Rows(noOfrows)("vMedExCode") + ","
            Next

            If Strcrfsubdtl <> "" Then
                Strcrfsubdtl = Strcrfsubdtl.Substring(0, Strcrfsubdtl.LastIndexOf(","))
            End If

            'Me.TempMedex.Attributes.Add("class", "quad")

            Me.HdnExistMedex.Value = Strcrfsubdtl

            Me.pnltable2.Style.Add("display", "")
            Me.pheader2.Style.Add("display", "")

            Me.ddlProjectType.SelectedValue = Dt_FillChkDup.Rows(0).Item("vProjectTypeCode")

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....BindchkdupMedEx")
        End Try
    End Sub

    Private Function AddDeletedMedEx() As Boolean
        Dim DeletedMedEx() As String
        Dim index As Integer
        Dim Dt_FillMedex As New DataTable
        Dim Dt_MedexMst As New DataTable
        Dim Dv_MedexMst As New DataView
        Try
            If Me.HdnExistMedexRemoved.Value <> "" Then
                Dt_FillMedex = CType(Me.ViewState(VS_DtBlankMedExTemplateDtl), DataTable)
                DeletedMedEx = Me.HdnExistMedexRemoved.Value.Split(",")
                For index = 0 To DeletedMedEx.Length - 1
                    If DeletedMedEx(index) <> "" Then
                        For i As Integer = 0 To Dt_FillMedex.Rows.Count - 1
                            If Dt_FillMedex.Rows(i)("vMedExCode") = DeletedMedEx(index) Then
                                Dt_FillMedex.Rows(i)("cStatusIndi") = "D"
                                Dt_FillMedex.Rows(i)("cActiveFlag") = "N"
                                Dt_FillMedex.AcceptChanges()
                            End If
                        Next
                    End If
                Next
                Me.ViewState(VS_DtBlankMedExTemplateDtl) = Dt_FillMedex
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....AddDeletedMedEx")
        End Try
    End Function

    Private Function AddNewMedEx() As Boolean
        Dim NEwMEdex() As String
        Dim index As Integer
        Dim Dt_FillMedex As New DataTable
        Dim Dr As DataRow
        Dim Dt_MedexMst As New DataTable
        Dim Dv_MedexMst As New DataView
        Try
            If Me.HdnNewAddedMedex.Value <> "" Then
                Dt_FillMedex = CType(Me.ViewState(VS_DtBlankMedExTemplateDtl), DataTable)
                NEwMEdex = Me.HdnNewAddedMedex.Value.Split(",")
                Dt_MedexMst = CType(Me.ViewState(VS_MedExOrg), DataTable)
                'Dv_MedexMst = Dt_MedexMst.DefaultView()
                'Dv_MedexMst.RowFilter = "vMedExCode = '" + "" + "'"

                '                If Dv_MedexMst.ToTable.Rows(0).Item("vMedexType").ToString = "Formula" Then
                '                    Dv_MedexMst.RowFilter = "vMedExCode = '" + Me.chkOrgMedex.Items(index).Value + "'and vMedexType='Formula' and ( vMedexFormula is  null or  vMedexFormula='') "
                '                    If Dv_MedexMst.ToTable.Rows.Count > 0 Then
                '                        objcommon.ShowAlert("You cannot Add blank  Formula", Me.Page)
                '                        Exit Sub
                '                    End If
                '                End If

                For index = 0 To NEwMEdex.Length - 1
                    If NEwMEdex(index) <> "" Then
                        Dv_MedexMst = Dt_MedexMst.DefaultView()
                        Dv_MedexMst.RowFilter = "vMedExCode = '" + NEwMEdex(index) + "'"
                        If Dv_MedexMst.ToTable.Rows.Count > 0 Then
                            Dr = Dt_FillMedex.NewRow()
                            Dr("nMedExTemplateDtlNo") = Dt_FillMedex.Rows.Count + 1
                            Dr("vMedExTemplateId") = "0001"
                            Dr("cStatusIndi") = "N"
                            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                                Dr("vMedExTemplateId") = Me.ViewState(VS_MedExTemplateId)
                                'Dr("cStatusIndi") = "E"
                            End If
                            Dr("vTemplateName") = Me.txtTemplate.Text
                            Dr("iSeqNo") = 1
                            If Dt_FillMedex.Rows.Count > 0 Then
                                Dr("iSeqNo") = Dt_FillMedex.Compute("Max(iSeqNo)", "1=1") + 1
                            End If
                            Dr("vProjectTypeCode") = Me.ddlProjectType.SelectedValue.Trim()
                            Dr("vMedExCode") = Dv_MedexMst.ToTable.Rows(0).Item("vMedExCode")
                            Dr("vMedExDesc") = Dv_MedexMst.ToTable.Rows(0).Item("vMedExDesc")
                            Dr("MedExDescWithSubGroup") = Dv_MedexMst.ToTable.Rows(0).Item("MedExDescWithSubGroup")
                            Dr("vMedExValues") = Dv_MedexMst.ToTable.Rows(0).Item("vDefaultValue")
                            Dr("cActiveFlag") = "Y"
                            Dr("iModifyBy") = Session(S_UserID)
                            Dt_FillMedex.Rows.Add(Dr)
                            Dt_FillMedex.AcceptChanges()
                        End If
                    End If
                Next
                Me.ViewState(VS_DtBlankMedExTemplateDtl) = Dt_FillMedex
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....AddNewMedEx")
        End Try
    End Function

    Private Function SerilazeMedEx() As Boolean
        Dim Dt_SeqenceMedex As New DataTable
        Dim JSONString As String = String.Empty
        Dim ds_Field As New DataSet
        Try

            If Not Me.HdnSequenceMedex.Value = "" Then
                Dt_SeqenceMedex = CType(Me.ViewState(VS_DtBlankMedExTemplateDtl), DataTable)
                JSONString = Me.HdnSequenceMedex.Value
                ds_Field.Tables.Add(JsonConvert.DeserializeObject(JSONString, GetType(System.Data.DataTable)))
                If ds_Field.Tables(0).Rows.Count > 0 Then
                    For index As Integer = 0 To ds_Field.Tables(0).Rows.Count - 1
                        For NewIndex As Integer = 0 To Dt_SeqenceMedex.Rows.Count - 1
                            If Dt_SeqenceMedex.Rows(NewIndex)("vMedExCode") = ds_Field.Tables(0).Rows(index)("vMedExCode") Then
                                Dt_SeqenceMedex.Rows(NewIndex)("iSeqNo") = ds_Field.Tables(0).Rows(index)("iSeqNo")
                            End If
                        Next
                    Next
                    Dt_SeqenceMedex.AcceptChanges()
                End If
                Me.ViewState(VS_DtBlankMedExTemplateDtl) = Dt_SeqenceMedex
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....SerilazeMedEx")
        End Try
    End Function

    Private Function GetBlankFormula() As Boolean
        Dim dsBlankFormula As New DataSet
        Dim StrFormula As String = String.Empty
        Try
            dsBlankFormula = objhelp.GetResultSet("SELECT vMedexCode FROM VIEW_MEDEXMST WHERE  vMedexType='Formula' AND ( vMedexFormula IS  NULL OR  vMedexFormula = '')", "VIEW_MEDEXMST")

            If dsBlankFormula.Tables(0).Rows.Count > 0 Then
                For index As Integer = 0 To dsBlankFormula.Tables(0).Rows.Count - 1
                    StrFormula += dsBlankFormula.Tables(0).Rows(index)("vMedExCode") + ","
                Next

                If StrFormula <> "" Then
                    StrFormula = StrFormula.Substring(0, StrFormula.LastIndexOf(","))
                End If
                Me.HdnBlankMedex.Value = StrFormula
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GetBlankFormula")
        End Try
    End Function

#End Region

#Region "Checkbox All check changed"

    Protected Sub chkAllSubGroups_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)


        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
            BindchkdupMedEx()
        End If

        CheckBoxList()
    End Sub

#End Region

#Region "check duplicate templates"
    'For Validation of Duplication template :Start
    Private Function IsDuplicate() As Boolean
        Dim ds_Check As New DataSet
        Dim Wstr As String = String.Empty

        If Not Me.txtTemplate.Text.Trim() = "" Then

            Wstr = "cStatusIndi <> 'D' And vTemplateName='" & Me.txtTemplate.Text.Trim() & "' And vProjectTypeCode = '" & Me.ddlProjectType.SelectedValue.Trim() & "'"
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                Wstr += " And vMedExTemplateId <> '" + Me.ViewState(VS_MedExTemplateId).ToString() + "'"
            End If

            If Not objhelp.GetMedExTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From MedExTemplateDtl", eStr)
                Return False
            End If
            '====

            If ds_Check.Tables(0).Rows.Count > 0 Then
                objcommon.ShowAlert("Entered Template already exist.", Me)
                Return False

            End If
        End If
        Return True
    End Function
    'For Validation of Duplication template :Start
#End Region

#Region "Change dataset"

    Public Sub ChangeDataset(ByRef Dt As DataTable, ByVal ColumnToChange As String, ByVal ValToChange As String)
        Dim row As Integer
        For row = 0 To Dt.Rows.Count - 1 Step 1
            Dt.Rows(row).Item(ColumnToChange) = ValToChange
        Next row
    End Sub

#End Region

#Region "ResetPage"
    Private Sub ResetPage()
        'Me.chkdupMedEx.Items.Clear()
        Me.txtTemplate.Text = ""
        Me.HTemplateId.Value = ""

        If Not Me.ViewState(VS_DtBlankMedExTemplateDtl) Is Nothing Then
            CType(Me.ViewState(VS_DtBlankMedExTemplateDtl), DataTable).Rows.Clear()
        End If
        'Me.chkOrgMedex.Items.Clear()
        Me.ddlMedExGroup.SelectedIndex = 0
        Me.ddlMedExSubGroup.SelectedIndex = 0
        'Me.ddlProjectType.SelectedIndex = 0
        Me.chkAllSubGroups.Checked = False
        'Me.btnfrmcpy.Visible = False
        'Me.btnintocopy.Visible = False
        'Me.chkOrgMedex.Visible = False
        'Me.pnlorg.Visible = False
        Me.btnupdate.Visible = False
        Me.btnsave.Visible = False

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

#Region "Move Right & Left"

    'Protected Sub ImgBtnLeft_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim estr_retu As String = ""

    '    If Not MoveRightLeft("LEFT", estr_retu) Then

    '        Me.objcommon.ShowAlert("Error while changeing sequence to Left:" + estr_retu, Me.Page)
    '        Exit Sub

    '    End If

    'End Sub

    'Protected Sub ImgBtnRight_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    Dim estr_retu As String = ""

    '    If Not MoveRightLeft("RIGHT", estr_retu) Then

    '        Me.objcommon.ShowAlert("Error while changeing sequence to Right:" + estr_retu, Me.Page)
    '        Exit Sub

    '    End If

    'End Sub

    'Private Function MoveRightLeft(ByVal Direction As String, ByRef estr_retu As String) As Boolean
    '    Dim Dt_FillChkDup As New DataTable
    '    Dim CurrSeq As Integer
    '    Dim Tblindex As Integer
    '    Dim CheckedMedex As String = String.Empty
    '    Dim Dv_FillChkDup As DataView
    '    Dim Dt_Table As DataTable
    '    Try
    '        Dt_FillChkDup = CType(Me.ViewState(VS_DtBlankMedExTemplateDtl), DataTable)
    '        ' Added
    '        Dv_FillChkDup = Dt_FillChkDup.DefaultView()
    '        Dv_FillChkDup.RowFilter = "cActiveFlag<>'N'"

    '        Dt_Table = Dv_FillChkDup.ToTable.Copy()

    '        For Tblindex = 0 To Dt_Table.Rows.Count - 1

    '            If Dt_Table.Rows(Tblindex).Item("vMedExCode") = Me.chkdupMedEx.SelectedValue.Trim() Then

    '                CheckedMedex = Dt_Table.Rows(Tblindex).Item("vMedExCode")

    '                If Direction.ToUpper.Trim = "LEFT" Then

    '                    CurrSeq = Dt_Table.Rows(Tblindex).Item("iSeqNo")

    '                    If CurrSeq <= 1 Then
    '                        Me.objcommon.ShowAlert("This Is The First Position", Me.Page)
    '                        Exit For
    '                    End If

    '                    Dt_Table.Rows(Tblindex).Item("iSeqNo") = Dt_Table.Rows(Tblindex - 1).Item("iSeqNo")
    '                    Dt_Table.Rows(Tblindex - 1).Item("iSeqNo") = CurrSeq
    '                    Dt_Table.AcceptChanges()
    '                    Exit For

    '                ElseIf Direction.ToUpper.Trim = "RIGHT" Then

    '                    CurrSeq = Dt_Table.Rows(Tblindex).Item("iSeqNo")

    '                    If CurrSeq >= Dt_Table.Compute("Max(iSeqNo)", "1=1") Then
    '                        Me.objcommon.ShowAlert("This Is The Last Position", Me.Page)
    '                        Exit For
    '                    End If

    '                    Dt_Table.Rows(Tblindex).Item("iSeqNo") = Dt_Table.Rows(Tblindex + 1).Item("iSeqNo")
    '                    Dt_Table.Rows(Tblindex + 1).Item("iSeqNo") = CurrSeq
    '                    Dt_Table.AcceptChanges()
    '                    Exit For

    '                End If


    '            End If

    '        Next Tblindex

    '        Dt_Table.DefaultView.Sort = "iSeqNo"
    '        'Dt_FillChkDup.DefaultView.Sort = "iSeqNo"
    '        Me.ViewState(VS_DtBlankMedExTemplateDtl) = Dt_Table.DefaultView.ToTable() 'Dt_FillChkDup.DefaultView.ToTable()

    '        BindchkdupMedEx()

    '        Me.chkdupMedEx.SelectedValue = CheckedMedex

    '        Return True
    '    Catch ex As Exception
    '        estr_retu = ex.Message
    '        Return False
    '    End Try
    'End Function

#End Region

End Class
