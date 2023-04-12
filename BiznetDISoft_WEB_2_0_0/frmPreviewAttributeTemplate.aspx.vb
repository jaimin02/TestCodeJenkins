Imports Newtonsoft.Json
Imports Microsoft.Office.Interop

Partial Class frmPreviewAttributeTemplate
    Inherits System.Web.UI.Page

#Region "Varible Declaration"
    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()


    Private Const VS_DtMedExTemplateDtl As String = "MedExTemplateDtl" 'gvwMedEx
    Private Const Vs_UOM As String = "UOMMst"
    Private Const VS_DtMedExMst As String = "MedExMst" 'ddlMedEx
    Private Const VS_Choice As String = "Choice"
    Private Const vs_MedExTemplateId As String = "vMedExTemplateId"
    Private Const vs_MedExTemplateName As String = "vMedExTemplateName"
    Private Const vs_ProjectTypeCode As String = "ProjectTypeCode"
    Private Const Vs_Status As String = "Status"

    Private Const AddToGrid As String = "AddToGrid"
    Private Const AddToDatabase As String = "AddToDatabase"

    Private StrValidation As String = ""

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_AttTemplate As Integer = 1
    Private Const GVC_TemplateID As Integer = 2
    Private Const GVC_Edit As Integer = 3
    Private Const GVC_Preview As Integer = 4
    Private Const GVC_ProjectTypeCode As Integer = 5
    Private Const GVC_Reorder As Integer = 6

    Private Const GvwMedEx_Select As Integer = 0
    Private Const GvwMedEx_Details As Integer = 1
    Private Const GvwMedEx_MedExTemplateDtlNo As Integer = 2
    Private Const GvwMedEx_MedExTemplateId As Integer = 3
    Private Const GvwMedEx_MedExCode As Integer = 4
    Private Const GvwMedEx_MedExDesc As Integer = 5
    Private Const GvwMedEx_MedExType As Integer = 6
    Private Const GvwMedEx_MedExValue As Integer = 7
    Private Const GvwMedEx_DefaultValue As Integer = 8
    Private Const GvwMedEx_Alertonvalue As Integer = 9
    Private Const GvwMedEx_AlertMessage As Integer = 10
    Private Const GvwMedEx_LowRang As Integer = 11
    Private Const GvwMedEx_HighRange As Integer = 12
    Private Const GvwMedEx_ActiveFlag As Integer = 13
    Private Const GvwMedEx_SeqNo As Integer = 14
    Private Const GvwMedEx_vMedExGroupCode As Integer = 15
    Private Const GvwMedEx_vmedexgroupDesc As Integer = 16
    Private Const GvwMedEx_vMedexGroupCDISCValue As Integer = 17
    Private Const GvwMedEx_vmedexGroupOtherValue As Integer = 18
    Private Const GvwMedEx_vMedExSubGroupCode As Integer = 19
    Private Const GvwMedEx_vmedexsubGroupDesc As Integer = 20
    Private Const GvwMedEx_vMedexSubGroupCDISCValue As Integer = 21
    Private Const GvwMedEx_vmedexsubGroupOtherValue As Integer = 22
    Private Const GvwMedEx_vUOM As Integer = 23
    Private Const GvwMedEx_vValidationType As Integer = 24
    Private Const GvwMedEx_Length As Integer = 25
    Private Const GvwMedEx_cAlertType As Integer = 26
    Private Const GvwMedEx_cRefType As Integer = 27
    Private Const GvwMedEx_vRefTable As Integer = 28
    Private Const GvwMedEx_vRefColumn As Integer = 29
    Private Const GvwMedEx_vRefFilePath As Integer = 30
    Private Const GvwMedEx_vCDISCValues As Integer = 30
    Private Const GvwMedEx_vOtherValues As Integer = 31

    'Private Const GvwMedEx_Delete As Integer = 32
    Private Const GvwMedEx_vUOMHdn As Integer = 33
    Private Const GvwMedEx_vValidationTypeHdn As Integer = 34
    Private Const GvwMedEx_cAlertTypeHdn As Integer = 35
    Private Const GvwMedEx_vMedExTypeHdn As Integer = 36

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Fillgrid(False)
        End If

        Page.Title = ":: Preview Attribute Template  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        CType(Master.FindControl("lblHeading"), Label).Text = "Preview Attribute Template"
        'CType(Master.FindControl("lblMandatory"), Label).Text = ""

        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
    End Sub
#End Region

#Region "Grid Events"

#Region "AtrributeTemplate Grid Events"

    Private Function Fillgrid(ByVal SearchedResult As Boolean) As Boolean
        Dim ds_MedexTemplate As New DataSet
        Dim dv_MedexTemplate As DataView
        Dim wstr As String = " 1=1 "
        Dim eStr_Retu As String = String.Empty
        'to show all templates
        If Me.txtTemplate.Text.Trim = "" Then
            SearchedResult = False
        End If
        '===========
        'to show selected template 
        If SearchedResult = True Then
            wstr = " vMedExTemplateId='" & Me.HTemplateId.Value & "' "
        End If
        '==========
        '==========
        If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
            wstr += " And vMedExType <> 'IMPORT'"
        End If

        wstr += " and vProjectTypeCode in (" & Me.Session(S_ScopeValue) & ") And cstatusindi <>'D' AND cTemplateType in ('O','U') Order By vTemplateName"
        'Change to have only required data 
        'If Not objHelp.GetMedExTemplateDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedexTemplate, eStr_Retu) Then
        '    Me.ShowErrorMessage("Error While Getting Data From ViewReasonMst", eStr_Retu)
        '    Return False
        'End If
        If Not objHelp.GetFieldsOfTable("VIEW_MEDEXTEMPLATEDTL", "distinct vMedExTemplateId,vTemplateName,vprojecttypecode", wstr, ds_MedexTemplate, eStr_Retu) Then
            Me.ShowErrorMessage("Error While Getting Data From ViewReasonMst", eStr_Retu)
            Return False
        End If

        dv_MedexTemplate = ds_MedexTemplate.Tables(0).DefaultView.ToTable(True, "vTemplateName,vMedExTemplateId,vProjectTypeCode".Split(",")).DefaultView
        GV_PreviewAtrributeTemplate.DataSource = dv_MedexTemplate.ToTable
        GV_PreviewAtrributeTemplate.DataBind()
        If GV_PreviewAtrributeTemplate.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_PreviewAtrributeTemplateGV_UserType", "UIGV_PreviewAtrributeTemplate(); ", True)
        End If
        Fillgrid = True
    End Function

    Protected Sub GV_PreviewAtrributeTemplate_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GV_PreviewAtrributeTemplate.PageIndexChanging
        Dim dt_ViewReasonMst As New DataTable
        GV_PreviewAtrributeTemplate.PageIndex = e.NewPageIndex
        If Not Fillgrid(False) Then
            Exit Sub
        End If
    End Sub

    Protected Sub GV_PreviewAtrributeTemplate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_PreviewAtrributeTemplate.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim JsStr As String = String.Empty
        Dim dt_DtMedExDtl As New DataTable
        Try

            If Not Me.ViewState(Vs_Status) Is Nothing Then
                objcommon.ShowAlert("Please save or cancel the previous changes", Me.Page)
                Exit Sub
            End If

            If e.CommandName.ToUpper = "EDIT" Then

                If Not FillddlMedexGroup() Then
                    Exit Sub
                End If

                If Not FillddlMedex() Then
                    Exit Sub
                End If

                If Not FillUOM() Then
                    Exit Sub
                End If
                Me.ViewState(vs_MedExTemplateName) = HttpUtility.HtmlDecode(Me.GV_PreviewAtrributeTemplate.Rows(index).Cells(GVC_AttTemplate).Text.Trim())
                Me.ViewState(vs_ProjectTypeCode) = Me.GV_PreviewAtrributeTemplate.Rows(index).Cells(GVC_ProjectTypeCode).Text.Trim()

                If Not FillGridgvwMedEx(Me.GV_PreviewAtrributeTemplate.Rows(index).Cells(GVC_TemplateID).Text.Trim()) Then
                    Throw New Exception()
                End If

                Me.lblProject.Text = "Attribute Template : " + Me.ViewState(vs_MedExTemplateName)
                Dim message As String = "Attribute Template : " + Me.ViewState(vs_MedExTemplateName)



                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "OpenAttributeTemplate", "OpenAttributeTemplate(); ", True)

                'Me.MPEditMedex.Show()

            ElseIf e.CommandName.ToUpper = "REORDER" Then
                Me.lblMedexCount.Text = ""

                Me.ViewState(vs_MedExTemplateName) = Me.GV_PreviewAtrributeTemplate.Rows(index).Cells(GVC_AttTemplate).Text.Trim()
                Me.ViewState(vs_ProjectTypeCode) = Me.GV_PreviewAtrributeTemplate.Rows(index).Cells(GVC_ProjectTypeCode).Text.Trim()

                If Not FillMedExDesc(Me.GV_PreviewAtrributeTemplate.Rows(index).Cells(GVC_TemplateID).Text.Trim()) Then
                    Exit Sub
                End If

                dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExTemplateDtl), DataTable).Copy

                If Not dt_DtMedExDtl.Rows.Count > 0 Then
                    objcommon.ShowAlert("No Attributes Found For This Template", Me.Page)
                    Exit Sub
                End If


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
                    li.Attributes.Add("title", dt_DtMedExDtl.Rows(noOfrows)("vMedExDesc") + " (" + dt_DtMedExDtl.Rows(noOfrows)("vmedexsubGroupDesc") + ") " + "[" + dt_DtMedExDtl.Rows(noOfrows)("vmedexgroupDesc") + "] ")
                    li.Controls.Add(div)
                    li.Attributes.Add("class", "allmed")
                    Me.SeqMedex.Controls.Add(li)

                Next


                Me.SeqMedex.Attributes.Add("class", "six")
                Me.lblMedexCount.Text = "Total No Of Attributes : " + dt_DtMedExDtl.Rows.Count.ToString()
                Me.hdnMedexList.Value = JsonConvert.SerializeObject(dt_DtMedExDtl)

                'Me.PanelGridHeader.Style.Add("display", "none")
                'Me.pnlMedEx.Style.Add("display", "none")

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CloseAttributeTemplate", "CloseAttributeTemplate(); ", True)
                Me.MPEMedexSequence.Show()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......GV_PreviewAtrributeTemplate_RowCommand")
        End Try


    End Sub

    Protected Sub GV_PreviewAtrributeTemplate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_PreviewAtrributeTemplate.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1 + (Me.GV_PreviewAtrributeTemplate.PageSize * Me.GV_PreviewAtrributeTemplate.PageIndex)
            CType(e.Row.FindControl("ImbMPreview"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbMPreview"), ImageButton).CommandName = "PREVIEW"
            CType(e.Row.FindControl("ImbMPreview"), ImageButton).Attributes.Add("OnClick", "return PreviewTemplate('frmPreviewattributesForm.aspx?TemplateId=" & e.Row.Cells(GVC_TemplateID).Text.Trim() & "');")

            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "Edit"

            CType(e.Row.FindControl("ImbReorder"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbReorder"), ImageButton).CommandName = "REORDER"
        End If
    End Sub

    Protected Sub GV_PreviewAtrributeTemplate_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_PreviewAtrributeTemplate.RowCreated
        If e.Row.RowType = DataControlRowType.Footer Or _
            e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Header Then

            e.Row.Cells(GVC_TemplateID).Visible = False
            e.Row.Cells(GVC_ProjectTypeCode).Visible = False

        End If
    End Sub

    Protected Sub GV_PreviewAtrributeTemplate_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GV_PreviewAtrributeTemplate.RowEditing
        e.Cancel = True
    End Sub

#End Region

#Region "Medex Grid Events"

    Private Function FillEditedgrid(ByVal dt As DataTable) As Boolean
        Dim dvTemp As New DataView

        dvTemp = dt.DefaultView
        dvTemp.RowFilter = "cStatusIndi <> 'D' And cActiveFlag <> 'N'"
        dvTemp.Sort = "iSeqNo,vMedExTemplateId"
        dt = dvTemp.ToTable()

        Me.gvwMedEx.DataSource = dt
        Me.gvwMedEx.DataBind()
        Return True

    End Function

    Protected Sub gvwMedEx_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwMedEx.RowCommand
        'Dim indexGV As Integer = e.CommandArgument
        'Dim index As Integer = 0
        'Dim dt_DtMedExDtl As New DataTable

        'If e.CommandName.ToUpper = "DELETE" Then

        '    Try
        '        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete




        '        dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExTemplateDtl), DataTable)

        '        For index = 0 To dt_DtMedExDtl.Rows.Count - 1

        '            If dt_DtMedExDtl.Rows(index).Item("vMedExCode").ToString.Trim() = Me.gvwMedEx.Rows(indexGV).Cells(GvwMedEx_MedExCode).Text.Trim() Then
        '                Exit For
        '            End If

        '        Next index

        '        If dt_DtMedExDtl.Rows(index).Item("nMedExTemplateDtlNo") < 0 Then
        '            dt_DtMedExDtl.Rows(index).Delete()
        '            dt_DtMedExDtl.AcceptChanges()
        '        Else
        '            dt_DtMedExDtl.Rows(index).Item("cStatusIndi") = "D"
        '            dt_DtMedExDtl.Rows(index).Item("cActiveFlag") = "N"
        '            dt_DtMedExDtl.AcceptChanges()
        '        End If

        '        Me.ViewState(VS_DtMedExTemplateDtl) = dt_DtMedExDtl

        '        If Not FillEditedgrid(dt_DtMedExDtl.Copy()) Then
        '            Exit Sub
        '        End If
        '        'Me.MPEditMedex.Show()

        '    Catch ex As Exception
        '        Me.ShowErrorMessage(ex.Message, "............gvwMedEx_RowCommand")
        '    Finally
        '        dt_DtMedExDtl = Nothing
        '    End Try
        '    objcommon.ShowAlert("Attributes Details deleted Sucessfully", Me.Page)
        'End If

    End Sub

    Protected Sub gvwMedEx_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwMedEx.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
        e.Row.RowType = DataControlRowType.Header Or _
        e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GvwMedEx_MedExTemplateDtlNo).Visible = False
            e.Row.Cells(GvwMedEx_MedExTemplateId).Visible = False
            e.Row.Cells(GvwMedEx_MedExCode).Visible = False
            e.Row.Cells(GvwMedEx_ActiveFlag).Visible = False
            e.Row.Cells(GvwMedEx_SeqNo).Visible = False
            e.Row.Cells(GvwMedEx_vMedExGroupCode).Visible = False
            e.Row.Cells(GvwMedEx_vmedexgroupDesc).Style.Add("display", "none")
            e.Row.Cells(GvwMedEx_vmedexsubGroupDesc).Style.Add("display", "none")
            e.Row.Cells(GvwMedEx_vMedExSubGroupCode).Visible = False
            e.Row.Cells(GvwMedEx_cRefType).Visible = False
            e.Row.Cells(GvwMedEx_vRefTable).Visible = False
            e.Row.Cells(GvwMedEx_vRefColumn).Visible = False
            e.Row.Cells(GvwMedEx_vRefFilePath).Visible = False
            e.Row.Cells(GvwMedEx_vUOMHdn).Visible = False
            e.Row.Cells(GvwMedEx_vMedExTypeHdn).Visible = False
            e.Row.Cells(GvwMedEx_vValidationTypeHdn).Visible = False
            e.Row.Cells(GvwMedEx_cAlertTypeHdn).Visible = False
        End If

    End Sub

    Protected Sub gvwMedEx_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwMedEx.RowDataBound
        Dim arrStrValidation As Array
        Dim ds_TablesMedExType As DataSet
        Dim attrdetails As String = String.Empty
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                attrdetails = "Attribute Group : " + e.Row.Cells(GvwMedEx_vmedexgroupDesc).Text + " Attribute SubGroup : " + e.Row.Cells(GvwMedEx_vmedexsubGroupDesc).Text
                CType(e.Row.Cells(GvwMedEx_Details).FindControl("ImbDetails"), Image).Attributes.Add("title", attrdetails)

                CType(e.Row.Cells.Item(GvwMedEx_MedExDesc).FindControl("txtMedexDesc"), TextBox).Attributes.Add("OnBlur", "return ValidateMedexDesc(" & CType(e.Row.Cells.Item(GvwMedEx_MedExDesc).FindControl("txtMedexDesc"), TextBox).ClientID & ");")

                CType(e.Row.Cells.Item(GvwMedEx_Alertonvalue).FindControl("txtAlertOn"), TextBox).Attributes.Add("OnBlur", "return ValidateAlertOn(" & CType(e.Row.Cells.Item(GvwMedEx_MedExValue).FindControl("txtValue"), TextBox).ClientID & "," & CType(e.Row.Cells.Item(GvwMedEx_Alertonvalue).FindControl("txtAlertOn"), TextBox).ClientID & ");")
                CType(e.Row.Cells.Item(GvwMedEx_MedExValue).FindControl("txtValue"), TextBox).Attributes.Add("OnBlur", "return ValidateAlertOn(" & CType(e.Row.Cells.Item(GvwMedEx_MedExValue).FindControl("txtValue"), TextBox).ClientID & "," & CType(e.Row.Cells.Item(GvwMedEx_Alertonvalue).FindControl("txtAlertOn"), TextBox).ClientID & ");")

                'CType(e.Row.FindControl("imgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                'CType(e.Row.FindControl("imgDelete"), ImageButton).CommandName = "Delete"

                ds_TablesMedExType = objHelp.GetResultSet("Select DISTINCT vMedExType From MedExMst Where cStatusIndi <> 'D'ORDER BY vMedExType ", "MedExMst")
                CType(e.Row.FindControl("ddlMedExAttributeType"), DropDownList).DataSource = ds_TablesMedExType
                CType(e.Row.FindControl("ddlMedExAttributeType"), DropDownList).DataValueField = "vMedExType"
                CType(e.Row.FindControl("ddlMedExAttributeType"), DropDownList).DataTextField = "vMedExType"
                CType(e.Row.FindControl("ddlMedExAttributeType"), DropDownList).DataBind()


                For i As Integer = 0 To ds_TablesMedExType.Tables(0).Rows.Count - 1
                    CType(e.Row.FindControl("ddlMedExAttributeType"), DropDownList).Items(i).Attributes.Add("title", ds_TablesMedExType.Tables(0).Rows(i)("vMedExType").ToString)
                Next i
                CType(e.Row.FindControl("ddlMedExAttributeType"), DropDownList).Items.Insert(0, New ListItem("-- Select Attribute Type --", 0))

                CType(e.Row.FindControl("ddlMedExAttributeType"), DropDownList).SelectedValue = HttpContext.Current.Server.HtmlDecode(Convert.ToString(e.Row.Cells.Item(GvwMedEx_vMedExTypeHdn).Text))
                CType(e.Row.FindControl("ddlMedExAttributeType"), DropDownList).ToolTip = Convert.ToString(e.Row.Cells.Item(GvwMedEx_vMedExTypeHdn).Text).Replace("&#181;", "µ")

                Dim Dt_UOM As DataTable = CType(Me.ViewState(Vs_UOM), DataTable)
                CType(e.Row.FindControl("ddlUOMDesc"), DropDownList).DataSource = Dt_UOM
                CType(e.Row.FindControl("ddlUOMDesc"), DropDownList).DataValueField = "vUOMDesc"
                CType(e.Row.FindControl("ddlUOMDesc"), DropDownList).DataTextField = "vUOMDesc"
                CType(e.Row.FindControl("ddlUOMDesc"), DropDownList).DataBind()

                For i As Integer = 0 To Dt_UOM.Rows.Count - 1
                    CType(e.Row.FindControl("ddlUOMDesc"), DropDownList).Items(i).Attributes.Add("title", Dt_UOM.Rows(i).Item("vUOMDesc").ToString)
                Next i

                CType(e.Row.FindControl("ddlUOMDesc"), DropDownList).Items.Insert(0, New ListItem("Select UOM", 0))


                If e.Row.Cells.Item(GvwMedEx_vUOMHdn).Text.Trim.ToString() <> "&nbsp;" AndAlso e.Row.Cells.Item(GvwMedEx_vUOMHdn).Text.Length > 0 Then
                    CType(e.Row.FindControl("ddlUOMDesc"), DropDownList).SelectedValue = HttpContext.Current.Server.HtmlDecode(Convert.ToString(e.Row.Cells.Item(GvwMedEx_vUOMHdn).Text))
                    CType(e.Row.FindControl("ddlUOMDesc"), DropDownList).ToolTip = Convert.ToString(e.Row.Cells.Item(GvwMedEx_vUOMHdn).Text).Replace("&#181;", "µ")
                End If

                If e.Row.Cells.Item(GvwMedEx_vValidationTypeHdn).Text.Trim.ToString() <> "&nbsp;" Then
                    StrValidation = e.Row.Cells.Item(GvwMedEx_vValidationTypeHdn).Text.Trim.ToString()
                End If


                If e.Row.Cells.Item(GvwMedEx_vValidationTypeHdn).Text.Trim.ToString() <> "&nbsp;" And e.Row.Cells.Item(GvwMedEx_vValidationTypeHdn).Text.Length > 0 Then
                    'CType(e.Row.FindControl("ddlValidation"), DropDownList).SelectedValue = StrValidation.Substring(0, StrValidation.LastIndexOf(","))
                    arrStrValidation = StrValidation.Split(",")
                    Hdn_ValidationType.Value = arrStrValidation(0)
                    If arrStrValidation(0) = "NU" And arrStrValidation.Length > 2 Then

                        CType(e.Row.FindControl("ddlValidation"), DropDownList).SelectedValue = arrStrValidation(0).ToString()
                        If arrStrValidation(0).ToString() = "NA" Then
                            CType(e.Row.FindControl("ddlValidation"), DropDownList).ToolTip = "Not Applicable"
                        ElseIf arrStrValidation(0).ToString() = "AN" Then
                            CType(e.Row.FindControl("ddlValidation"), DropDownList).ToolTip = "Alpha Numeric"
                        ElseIf arrStrValidation(0).ToString() = "NU" Then
                            CType(e.Row.FindControl("ddlValidation"), DropDownList).ToolTip = "Numeric"
                        ElseIf arrStrValidation(0).ToString() = "IN" Then
                            CType(e.Row.FindControl("ddlValidation"), DropDownList).ToolTip = "Integer"
                        ElseIf arrStrValidation(0).ToString() = "AL" Then
                            CType(e.Row.FindControl("ddlValidation"), DropDownList).ToolTip = "Alphabate"
                        End If
                    Else

                        CType(e.Row.FindControl("ddlValidation"), DropDownList).SelectedValue = StrValidation.Substring(0, StrValidation.LastIndexOf(","))
                        If StrValidation.Substring(0, StrValidation.LastIndexOf(",")).ToString() = "NA" Then
                            CType(e.Row.FindControl("ddlValidation"), DropDownList).ToolTip = "Not Applicable"
                        ElseIf StrValidation.Substring(0, StrValidation.LastIndexOf(",")).ToString() = "AN" Then
                            CType(e.Row.FindControl("ddlValidation"), DropDownList).ToolTip = "Alpha Numeric"
                        ElseIf StrValidation.Substring(0, StrValidation.LastIndexOf(",")).ToString() = "NU" Then
                            CType(e.Row.FindControl("ddlValidation"), DropDownList).ToolTip = "Numeric"
                        ElseIf StrValidation.Substring(0, StrValidation.LastIndexOf(",")).ToString() = "IN" Then
                            CType(e.Row.FindControl("ddlValidation"), DropDownList).ToolTip = "Integer"
                        ElseIf StrValidation.Substring(0, StrValidation.LastIndexOf(",")).ToString() = "AL" Then
                            CType(e.Row.FindControl("ddlValidation"), DropDownList).ToolTip = "Alphabate"
                        End If
                    End If
                End If

                CType(e.Row.Cells.Item(GvwMedEx_Length).FindControl("txtLength"), TextBox).Text = ""



                If StrValidation.Substring(StrValidation.LastIndexOf(",") + 1) <> "" And StrValidation.Substring(StrValidation.LastIndexOf(",") + 1) <> System.DBNull.Value.ToString And StrValidation.Substring(StrValidation.LastIndexOf(",") + 1) <> "&nbsp;" Then
                    If Integer.Parse(StrValidation.Substring(StrValidation.LastIndexOf(",") + 1)) > 0 Then
                        If arrStrValidation(0) = "NU" And arrStrValidation.Length > 2 Then
                            Hdn_LengthNumeric.Value = CType(e.Row.Cells.Item(GvwMedEx_Length).FindControl("txtLength"), TextBox).ClientID
                            CType(e.Row.Cells.Item(GvwMedEx_Length).FindControl("txtLength"), TextBox).Text = arrStrValidation(1) + "," + arrStrValidation(2)
                            CType(e.Row.Cells.Item(GvwMedEx_Length).FindControl("txtLength"), TextBox).ToolTip = arrStrValidation(1) + "," + arrStrValidation(2)
                        Else
                            Hdn_LengthNumeric.Value = StrValidation.Substring(StrValidation.LastIndexOf(",") + 1)
                            CType(e.Row.Cells.Item(GvwMedEx_Length).FindControl("txtLength"), TextBox).Text = StrValidation.Substring(StrValidation.LastIndexOf(",") + 1)
                            CType(e.Row.Cells.Item(GvwMedEx_Length).FindControl("txtLength"), TextBox).ToolTip = StrValidation.Substring(StrValidation.LastIndexOf(",") + 1)
                        End If
                    End If
                End If

                'CType(e.Row.Cells.Item(GvwMedEx_Length).FindControl("txtLength"), TextBox).Attributes.Add("OnBlur", "return ValidateNumeric(" & CType(e.Row.Cells.Item(GvwMedEx_Length).FindControl("txtLength"), TextBox).ClientID & ",'Please Enter numeric values');")
                CType(e.Row.Cells.Item(GvwMedEx_Length).FindControl("txtLength"), TextBox).Attributes.Add("OnBlur", "return ValidateNumeric(" & CType(e.Row.Cells.Item(GvwMedEx_Length).FindControl("txtLength"), TextBox).ClientID & "," & CType(e.Row.FindControl("ddlValidation"), DropDownList).ClientID & " , 'Plese Enter Numeric Values');")

                If e.Row.Cells.Item(GvwMedEx_cAlertTypeHdn).Text.Trim.ToString() = "Y" Then
                    CType(e.Row.FindControl("ChkAlertType"), CheckBox).Checked = True
                End If


            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..........gvwMedEx_RowDataBound")
        Finally
        End Try

    End Sub

    Private Function FillGridgvwMedEx(ByVal TemplateId As String) As Boolean
        Dim ds_gvwMedEx As New Data.DataSet
        Dim dt_gvwMedEx As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try



            Me.ViewState(vs_MedExTemplateId) = TemplateId

            wstr = "vMedExTemplateId = " + TemplateId + " And cStatusIndi <> 'D'  And cActiveFlag <> 'N' "

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            wstr += " Order By iSeqNo"

            'And vMedExType <> 'IMPORT'

            If Not objHelp.GetMedExTemplateDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_MedExTemplateDtl", estr)
                Return False
            End If

            Me.gvwMedEx.DataSource = ds_gvwMedEx
            Me.gvwMedEx.DataBind()

            Me.ViewState(VS_DtMedExTemplateDtl) = ds_gvwMedEx.Tables(0)

            If Me.gvwMedEx.Rows.Count < 1 Then

                Me.pnlMedExGrid.Visible = False
                'Me.gvwMedEx.Visible = False
                Me.btnSaveMedEx.Visible = False

            ElseIf Me.gvwMedEx.Rows.Count > 0 Then

                Me.pnlMedExGrid.Visible = True
                Me.gvwMedEx.Visible = True
                Me.btnSaveMedEx.Visible = True

            End If


            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........FillGridgvwMedEx")
            Return False
        End Try
    End Function

    Protected Sub gvwMedEx_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwMedEx.RowDeleting

    End Sub

#End Region

#End Region

#Region "FillDropDown For MedEx"

    Private Function FillddlMedexGroup() As Boolean
        Dim ds_MedexGroup As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(wstr) Then
                Return False
            End If

            Me.ddlMedexGroup.Items.Clear()

            wstr += " And cStatusIndi <> 'D' order by vMedExGroupDesc"

            If Not objHelp.GetMedExMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_MedexGroup, estr) Then
                Me.objcommon.ShowAlert("Error While Getting Data from Attribute GroupMst:" + estr, Me.Page)
                Return False
            End If

            If ds_MedexGroup.Tables(0).Rows.Count > 0 Then
                Me.ddlMedexGroup.DataSource = ds_MedexGroup.Tables(0).DefaultView.ToTable(True, "vMedExGroupCode,vMedExGroupDesc".Split(","))
                Me.ddlMedexGroup.DataValueField = "vMedExGroupCode"
                Me.ddlMedexGroup.DataTextField = "vMedExGroupDesc"
                Me.ddlMedexGroup.DataBind()
                Me.ddlMedexGroup.Items.Insert(0, New ListItem("Select Attribute Group", 0))

                ' tooltip
                For iMedexGroup As Integer = 0 To ddlMedexGroup.Items.Count - 1
                    ddlMedexGroup.Items(iMedexGroup).Attributes.Add("title", ddlMedexGroup.Items(iMedexGroup).Text)
                Next iMedexGroup
                '=========

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...........FillddlMedexGroup()")
            Return False
        End Try

    End Function

    Private Function FillddlMedex() As Boolean
        Dim ds_MedexMst As New Data.DataSet
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Try
            If Me.ddlMedexGroup.SelectedValue = 0 Then
                Me.ddlMedex.Items.Clear()
                Me.ddlMedex.Items.Insert(0, New ListItem("Select Attribute", 0))
                Return True
                Exit Function
            End If

            wstr = " vMedexGroupCode='" + Convert.ToString(Me.ddlMedexGroup.SelectedValue) + "'"
            wstr += " And cStatusIndi <> 'D' and cActiveFlag <> 'N' order by vMedExDesc"

            If Not objHelp.GetMedExMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedexMst, estr) Then
                Me.objcommon.ShowAlert("Error While Getting Data from Attribute:" + estr, Me.Page)
                Return False
            End If

            If ds_MedexMst.Tables(0).Rows.Count > 0 Then
                Me.ddlMedex.DataSource = ds_MedexMst
                Me.ddlMedex.DataValueField = "vMedExCode"
                Me.ddlMedex.DataTextField = "vMedExDesc"
                Me.ddlMedex.DataBind()

                ' tooltip
                For iMedex As Integer = 0 To ddlMedex.Items.Count - 1
                    ddlMedex.Items(iMedex).Attributes.Add("title", ds_MedexMst.Tables(0).Rows(iMedex).Item("vMedExDesc").ToString + "-" + ds_MedexMst.Tables(0).Rows(iMedex).Item("vMedexType").ToString)
                Next iMedex

                '=========
                Me.ddlMedex.Items.Insert(0, New ListItem("Select Attribute", 0))
                Me.ViewState(VS_DtMedExMst) = ds_MedexMst.Tables(0)
                Return True
                Exit Function
            End If
            Me.ddlMedex.DataSource = Nothing
            Me.ddlMedex.SelectedIndex = -1
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......FillddlMedex()")
            Return False
        End Try
    End Function

    Private Function FillUOM() As Boolean
        Dim Ds_FillUOMMst As New Data.DataSet
        Dim dt_UOMMst As New Data.DataTable
        Dim estr As String = String.Empty
        Try

            If Not objHelp.GetUOMMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillUOMMst, estr) Then
                Throw New Exception(estr)
            End If

            dt_UOMMst = Ds_FillUOMMst.Tables(0)
            Me.ViewState(Vs_UOM) = dt_UOMMst
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......FillUOM()")
            Return False
        End Try
    End Function

    Private Function FillMedExDesc(ByVal TemplateId As String) As Boolean
        Dim ds_gvwMedEx As New Data.DataSet
        Dim dt_gvwMedEx As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try
            Me.ViewState(vs_MedExTemplateId) = TemplateId

            Me.gvwMedEx.DataSource = Nothing
            Me.gvwMedEx.DataBind()

            wstr = "vMedExTemplateId = " + TemplateId + " And cStatusIndi <> 'D'  And cActiveFlag <> 'N' "

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            wstr += " Order By iSeqNo"

            If Not objHelp.GetMedExTemplateDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_MedExTemplateDtl", estr)
                Return False
            End If

            Me.ViewState(VS_DtMedExTemplateDtl) = ds_gvwMedEx.Tables(0)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........FillMedExDesc")
            Return False
        End Try
    End Function

    Protected Sub ddlMedexGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try



            If Not FillddlMedex() Then
                Throw New Exception
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "OpenAttributeTemplate", "OpenAttributeTemplate(); ", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..........ddlMedexGroup_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "Button Events"

    Protected Sub btnAddMedEx_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddMedEx.Click
        Dim ds_gvwMedEx As New DataSet
        Dim dt_DtMedExDtl As New DataTable
        Dim estr As String = String.Empty

        Try

            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add



            AssignValuesMedEx(AddToGrid)
            dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExTemplateDtl), DataTable)

            If Not FillEditedgrid(dt_DtMedExDtl) Then
                Exit Sub
            End If

            Me.btnSaveMedEx.Visible = False


            If Me.gvwMedEx.Rows.Count > 0 Then
                Me.gvwMedEx.Visible = True
                Me.btnSaveMedEx.Visible = True
                Me.ddlMedex.SelectedIndex = 0
                Me.ViewState(Vs_Status) = "Active"
            End If

            If Not FillddlMedex() Then
                Throw New Exception
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "OpenAttributeTemplate", "OpenAttributeTemplate(); ", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnAddMedEx_Click")
        End Try
    End Sub

    Private Sub AssignValuesMedEx(ByVal mode As String)
        Dim dr As DataRow = Nothing
        Dim drMedEx As DataRow = Nothing
        Dim drMedExWorkSpace As DataRow = Nothing
        Dim dt_MedEx As New DataTable
        Dim dt_DtMedExDtl As New DataTable
        Dim ds_save As New DataSet
        Dim estr_Retu As String = String.Empty
        'Dim wstr As String = String.Empty

        Dim dvAdd As New DataView
        Dim dvDelete As New DataView
        Dim dvEdit As New DataView
        Dim dsTemp As New DataSet

        Dim dt_ViewMedExWorkSpaceDtl As New DataTable
        Dim dt_MedExWorkSpaceDtl As New DataTable
        Dim Index As Integer
        Dim dsDelete As New DataSet
        Dim dtTemp As DataTable
        Dim MaxSeqNo As Integer = 0
        Dim ds_SaveForFormula As New DataSet
        Dim dv_MedexFormula As New DataView


        Try


            If mode = AddToGrid Then

                dt_MedEx = CType(Me.ViewState(VS_DtMedExMst), DataTable)
                dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExTemplateDtl), DataTable)

                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                    For Each dr In dt_MedEx.Rows

                        If (dr("vMedExCode").ToString = Me.ddlMedex.SelectedValue.ToString) Then
                            'If dr("vMedExType").ToString = "Formula" And (dr("vMedexFormula") = "" OrElse dr("vMedexFormula") Is DBNull.Value) Then
                            If dr("vMedExType").ToString = "Formula" And IIf(dr("vMedexFormula") Is DBNull.Value Or dr("vMedexFormula").ToString.Trim = "", True, False) Then
                                objcommon.ShowAlert("You Can not Add Blank Formula", Me.Page)
                                Exit Sub
                            End If
                            drMedEx = dr
                            Exit For
                        End If

                    Next dr
                    '----------------------------------------------------------------

                    '----------------------------------------------------------------

                    MaxSeqNo = 0
                    If dt_DtMedExDtl.Rows.Count > 0 Then
                        MaxSeqNo = dt_DtMedExDtl.Compute("Max(iSeqNo)", "1=1")
                    End If

                    '--------Checking for duplicate MedEx Entry
                    For Each dr In dt_DtMedExDtl.Rows
                        If (dr("vMedExCode") = Me.ddlMedex.SelectedValue.ToString And dr("cStatusIndi") <> "D") Then
                            objcommon.ShowAlert("Selected Attribute is already added !", Me.Page)
                            Exit Sub
                        End If

                    Next dr
                    '----------------Change On 01-July-2009------------------------

                    dr = dt_DtMedExDtl.NewRow()
                    dr("nMedExTemplateDtlNo") = 0 - dt_DtMedExDtl.Rows.Count - 1
                    dr("vMedExTemplateId") = Me.ViewState(vs_MedExTemplateId).ToString()
                    dr("vProjectTypeCode") = Me.ViewState(vs_ProjectTypeCode).ToString()
                    dr("vTemplateName") = Me.ViewState(vs_MedExTemplateName)
                    dr("iSeqNo") = MaxSeqNo + 1
                    dr("vMedExCode") = drMedEx("vMedExCode")
                    dr("vMedExDesc") = drMedEx("vMedExDesc")
                    dr("vMedExType") = drMedEx("vMedExType")
                    dr("vMedExValues") = drMedEx("vMedExValues")
                    dr("vDefaultValue") = drMedEx("vDefaultValue")
                    dr("vLowRange") = drMedEx("vLowRange")
                    dr("vHighRange") = drMedEx("vHighRange")
                    dr("vAlertonvalue") = drMedEx("vAlertonvalue")
                    dr("vAlertMessage") = drMedEx("vAlertMessage")
                    dr("cActiveFlag") = drMedEx("cActiveFlag")
                    dr("iModifyBy") = Me.Session(S_UserID).ToString()
                    dr("cStatusIndi") = "N"
                    dr("vMedExGroupCode") = drMedEx("vMedExGroupCode")
                    dr("vmedexgroupDesc") = drMedEx("vmedexgroupDesc")
                    dr("vMedexGroupCDISCValue") = drMedEx("vMedexGroupCDISCValue")
                    dr("vmedexGroupOtherValue") = drMedEx("vmedexGroupOtherValue")
                    dr("vMedExSubGroupCode") = drMedEx("vMedExSubGroupCode")
                    dr("vmedexsubGroupDesc") = drMedEx("vmedexsubGroupDesc")
                    dr("vMedexSubGroupCDISCValue") = drMedEx("vMedexSubGroupCDISCValue")
                    dr("vmedexsubGroupOtherValue") = drMedEx("vmedexsubGroupOtherValue")
                    dr("vMedExType") = drMedEx("vMedExType")
                    dr("vMedExValues") = drMedEx("vMedExValues")
                    dr("vUOM") = drMedEx("vUOM")
                    dr("vValidationType") = drMedEx("vValidationType")
                    dr("cAlertType") = drMedEx("cAlertType")
                    dr("cRefType") = drMedEx("cRefType")
                    dr("vRefTable") = drMedEx("vRefTable")
                    dr("vRefColumn") = drMedEx("vRefColumn")
                    dr("vRefFilePath") = drMedEx("vRefFilePath")
                    dr("vCDISCValues") = drMedEx("vCDISCValues")
                    dr("vOtherValues") = drMedEx("vOtherValues")
                    dt_DtMedExDtl.Rows.Add(dr)
                    dt_DtMedExDtl.AcceptChanges()

                End If
                Me.ViewState(VS_DtMedExTemplateDtl) = dt_DtMedExDtl

            ElseIf mode = AddToDatabase Then

                '------Added 02-july-2009---------------------------------
                dt_MedExWorkSpaceDtl = CType(Me.ViewState(VS_DtMedExTemplateDtl), DataTable).Copy()
                dt_MedExWorkSpaceDtl.Rows.Clear()

                For Index = 0 To gvwMedEx.Rows.Count - 1
                    dr = dt_MedExWorkSpaceDtl.NewRow
                    dr("nMedExTemplateDtlNo") = Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_MedExTemplateDtlNo).Text.ToString
                    dr("vMedExTemplateId") = Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_MedExTemplateId).Text.ToString
                    dr("vProjectTypeCode") = Me.ViewState(vs_ProjectTypeCode)
                    dr("vTemplateName") = Me.ViewState(vs_MedExTemplateName)
                    dr("iSeqNo") = Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_SeqNo).Text.ToString
                    dr("vMedExCode") = Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_MedExCode).Text.ToString
                    dr("vMedExDesc") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_MedExDesc).FindControl("txtMedexDesc"), TextBox).Text.Trim() 'Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_MedExDesc).Text.ToString
                    dr("vDefaultValue") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_DefaultValue).FindControl("txtDefaultValue"), TextBox).Text.Trim() 'drMedExWorkSpace("vDefaultValue")
                    dr("vLowRange") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_LowRang).FindControl("txtLowRange"), TextBox).Text.Trim()
                    dr("vHighRange") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_HighRange).FindControl("txtHighRange"), TextBox).Text.Trim()
                    dr("vAlertonvalue") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_Alertonvalue).FindControl("txtAlertOn"), TextBox).Text.Trim() 'drMedExWorkSpace("vDefaultValue")
                    dr("vAlertMessage") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_AlertMessage).FindControl("txtAlertMsg"), TextBox).Text.Trim() 'drMedExWorkSpace("vDefaultValue")
                    dr("cActiveFlag") = Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_ActiveFlag).Text.ToString
                    dr("iModifyBy") = Me.Session(S_UserID).ToString()
                    dr("cStatusIndi") = "N"
                    If (Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_MedExTemplateDtlNo).Text.ToString) > 0 Then
                        dr("cStatusIndi") = "E"
                    End If
                    dr("vMedExGroupCode") = IIf(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vMedExGroupCode).Text = "&nbsp;", System.DBNull.Value, Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vMedExGroupCode).Text.ToString)
                    dr("vmedexgroupDesc") = IIf(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vmedexgroupDesc).Text = "&nbsp;", System.DBNull.Value, Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vmedexgroupDesc).Text.ToString)
                    'dr("vmedexgroupDesc") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vmedexgroupDesc).FindControl("txtmedexgroupDesc"), TextBox).Text.Trim()
                    dr("vMedexGroupCDISCValue") = IIf(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vMedexGroupCDISCValue).Text = "&nbsp;", System.DBNull.Value, Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vMedexGroupCDISCValue).Text.ToString)
                    'dr("vMedexGroupCDISCValue") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vMedexGroupCDISCValue).FindControl("txtMedexGroupCDISCValue"), TextBox).Text.Trim()
                    dr("vmedexGroupOtherValue") = IIf(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vmedexGroupOtherValue).Text = "&nbsp;", System.DBNull.Value, Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vmedexGroupOtherValue).Text.ToString)
                    'dr("vmedexGroupOtherValue") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vmedexGroupOtherValue).FindControl("txtGroupOtherValue"), TextBox).Text.Trim()
                    dr("vMedExSubGroupCode") = IIf(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vMedExSubGroupCode).Text = "&nbsp;", System.DBNull.Value, Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vMedExSubGroupCode).Text.ToString)
                    dr("vmedexsubGroupDesc") = IIf(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vmedexsubGroupDesc).Text = "&nbsp;", System.DBNull.Value, Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vmedexsubGroupDesc).Text.ToString)
                    'dr("vmedexsubGroupDesc") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vmedexsubGroupDesc).FindControl("txtsubGroup"), TextBox).Text.Trim()
                    dr("vMedexSubGroupCDISCValue") = IIf(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vMedexSubGroupCDISCValue).Text = "&nbsp;", System.DBNull.Value, Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vMedexSubGroupCDISCValue).Text.ToString)
                    'dr("vMedexSubGroupCDISCValue") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vMedexSubGroupCDISCValue).FindControl("txtSubGroupCDISCValue"), TextBox).Text.Trim()
                    dr("vmedexsubGroupOtherValue") = IIf(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vmedexsubGroupOtherValue).Text = "&nbsp;", System.DBNull.Value, Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vmedexsubGroupOtherValue).Text.ToString)
                    'dr("vmedexsubGroupOtherValue") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vmedexsubGroupOtherValue).FindControl("txtSubGroupOtherValue"), TextBox).Text.Trim()
                    dr("vMedExType") = ""
                    If CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_MedExType).FindControl("ddlMedExAttributeType"), DropDownList).SelectedValue.ToString() <> "0" Then
                        dr("vMedExType") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_MedExType).FindControl("ddlMedExAttributeType"), DropDownList).SelectedValue.ToString()
                    End If

                    dr("vMedExValues") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_MedExValue).FindControl("txtValue"), TextBox).Text.Trim()
                    dr("vUOM") = ""

                    If CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vUOM).FindControl("ddlUOMDesc"), DropDownList).SelectedValue.ToString() <> "0" Then
                        dr("vUOM") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vUOM).FindControl("ddlUOMDesc"), DropDownList).SelectedValue.ToString()
                    End If
                    dr("vValidationType") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vValidationType).FindControl("ddlValidation"), DropDownList).SelectedValue.ToString() + "," + CType(Me.gvwMedEx.Rows(Index).Cells(0).FindControl("txtLength"), TextBox).Text.ToString()
                    dr("cAlertType") = IIf(CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_cAlertType).FindControl("ChkAlertType"), CheckBox).Checked, "Y", "N")
                    dr("cRefType") = IIf(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_cRefType).Text = "&nbsp;", System.DBNull.Value, Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_cRefType).Text.ToString)

                    dr("vRefTable") = IIf(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vRefTable).Text = "&nbsp;", System.DBNull.Value, Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vRefTable).Text.ToString)
                    dr("vRefColumn") = IIf(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vRefColumn).Text = "&nbsp;", System.DBNull.Value, Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vRefColumn).Text.ToString)
                    dr("vRefFilePath") = IIf(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vRefFilePath).Text = "&nbsp;", System.DBNull.Value, Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vRefFilePath).Text.ToString)
                    dr("vCDISCValues") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vCDISCValues).FindControl("txtCDISCValues"), TextBox).Text.Trim()
                    dr("vOtherValues") = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_vOtherValues).FindControl("txtotherValues"), TextBox).Text.Trim()


                    dt_MedExWorkSpaceDtl.Rows.Add(dr)
                    dt_MedExWorkSpaceDtl.AcceptChanges()
                Next Index

                dsTemp.Tables.Add(CType(dt_MedExWorkSpaceDtl, DataTable).Copy())
                dvAdd = dsTemp.Tables(0).DefaultView

                '============Added By Mani======================================

                ds_SaveForFormula.Tables.Add(CType(dt_MedExWorkSpaceDtl, DataTable).Copy())
                ds_SaveForFormula.Tables(0).TableName = "MedExWorkSpaceDtl"

                If ds_SaveForFormula.Tables(0).Rows.Count > 0 Then
                    If Not MedexUsedForFormula(ds_SaveForFormula) Then
                        Exit Sub
                    End If
                End If


                '======================================================

                '-----For Deleted Row---------------------------------
                ds_save = Nothing
                dt_MedExWorkSpaceDtl.Rows.Clear()

                dsDelete.Tables.Add(CType(Me.ViewState(VS_DtMedExTemplateDtl), DataTable).Copy())
                dvDelete = dsDelete.Tables(0).DefaultView
                dvDelete.RowFilter = "cStatusIndi = 'D'"
                dtTemp = dvDelete.ToTable()

                For Each drMedEx In dtTemp.Rows
                    dr = dt_MedExWorkSpaceDtl.NewRow
                    dr("nMedExTemplateDtlNo") = drMedEx("nMedExTemplateDtlNo")
                    dr("vTemplateName") = Me.ViewState(vs_MedExTemplateName)
                    dr("vProjectTypeCode") = Me.ViewState(vs_ProjectTypeCode).ToString()
                    dr("vMedExTemplateId") = drMedEx("vMedExTemplateId")
                    dr("iSeqNo") = drMedEx("iSeqNo")
                    dr("vMedExCode") = drMedEx("vMedExCode")
                    dr("vMedExDesc") = drMedEx("vMedExDesc")
                    dr("vDefaultValue") = drMedEx("vDefaultValue")
                    dr("vLowRange") = drMedEx("vLowRange")
                    dr("vHighRange") = drMedEx("vHighRange")
                    dr("vAlertonvalue") = drMedEx("vAlertonvalue")
                    dr("vAlertMessage") = drMedEx("vAlertMessage")
                    dr("cActiveFlag") = drMedEx("cActiveFlag")
                    dr("iModifyBy") = Me.Session(S_UserID).ToString()
                    dr("cStatusIndi") = "D"
                    dt_MedExWorkSpaceDtl.Rows.Add(dr)
                    dt_MedExWorkSpaceDtl.AcceptChanges()
                Next drMedEx

                ds_save = New DataSet
                ds_save.Tables.Add(dt_MedExWorkSpaceDtl)
                ds_save.Tables(0).TableName = "VIEW_MEDEXTEMPLATEDTL"


                If ds_save.Tables(0).Rows.Count > 0 Then

                    If Not objLambda.Save_MedExTemplateDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                            WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExTemplateDtl, ds_save, Me.Session(S_UserID), estr_Retu) Then
                        objcommon.ShowAlert("Error While Saving Attribute Details In MedexTemplateDtl" + estr_Retu, Me.Page)
                        Exit Sub
                    End If

                End If


                '-------For Add New Row----------
                dvAdd.RowFilter = "nMedExTemplateDtlNo < 0"

                ds_save = New DataSet
                ds_save.Tables.Add(dvAdd.ToTable().Copy())
                ds_save.Tables(0).TableName = "VIEW_MEDEXTEMPLATEDTL"

                If ds_save.Tables(0).Rows.Count > 0 Then

                    If Not objLambda.Save_MedExTemplateDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                            WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExTemplateDtl, ds_save, Me.Session(S_UserID), estr_Retu) Then
                        objcommon.ShowAlert("Error While Saving Attribute Details In MedexTemplateDtl" + estr_Retu, Me.Page)
                        Exit Sub
                    End If

                End If

                '-------For EDIT Row----------
                ds_save = Nothing

                dvEdit = dsTemp.Tables(0).DefaultView
                dvEdit.RowFilter = "nMedExTemplateDtlNo > 0"

                ds_save = New DataSet
                ds_save.Tables.Add(dvAdd.ToTable().Copy())
                ds_save.Tables(0).TableName = "VIEW_MEDEXTEMPLATEDTL"


                If ds_save.Tables(0).Rows.Count > 0 Then
                    If Not objLambda.Save_MedExTemplateDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                            WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExTemplateDtl, ds_save, Me.Session(S_UserID), estr_Retu) Then
                        objcommon.ShowAlert("Error While Saving Attribute Details In AttributeTemplateDtl" + estr_Retu, Me.Page)
                        Exit Sub
                    End If

                End If

                objcommon.ShowAlert("Attribute Details Saved SuccessFully !", Me.Page)

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValuesMedEx")
        End Try
    End Sub

    Protected Sub BtnViewAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnViewAll.Click
        If Not Fillgrid(False) Then
            Exit Sub
        End If
        Me.HTemplateId.Value = ""
        Me.txtTemplate.Text = ""
    End Sub

    Protected Sub btnSaveMedEx_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveMedEx.Click
        Dim eStr_Retu As String = String.Empty
        Try


            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                AssignValuesMedEx(AddToDatabase)
            End If
            If Not Fillgrid(True) Then
                Exit Try
            End If


            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CloseAttributeTemplate", "CloseAttributeTemplate(); ", True)
            Me.ViewState(Vs_Status) = Nothing
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr_Retu)
        End Try
    End Sub

    Protected Sub btnSaveSequence_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSequence.Click
        Dim ds_Field As New DataSet
        Dim JSONString As String = String.Empty
        Dim DvMedExWorkSpaceDtl As DataView = Nothing
        Dim estr As String = String.Empty
        Dim dsConvert As New DataSet
        Dim dt_Blank As New DataTable
        Dim dr As DataRow = Nothing
        Dim ds_save As New DataSet
        Try

            If Me.hdnMedexList.Value <> "" Then
                JSONString = Me.hdnMedexList.Value
                ds_Field.Tables.Add(JsonConvert.DeserializeObject(JSONString, GetType(System.Data.DataTable)))

                If ds_Field.Tables(0).Rows.Count > 0 Then

                    dt_Blank = CType(Me.ViewState(VS_DtMedExTemplateDtl), DataTable).Copy()
                    dt_Blank.Rows.Clear()

                    For Index = 0 To ds_Field.Tables(0).Rows.Count - 1
                        dr = dt_Blank.NewRow
                        dr("nMedExTemplateDtlNo") = ds_Field.Tables(0).Rows(Index)("nMedExTemplateDtlNo")
                        dr("vMedExTemplateId") = ds_Field.Tables(0).Rows(Index)("vMedExTemplateId")
                        dr("vProjectTypeCode") = Me.ViewState(vs_ProjectTypeCode)
                        dr("vTemplateName") = Me.ViewState(vs_MedExTemplateName)
                        dr("iSeqNo") = ds_Field.Tables(0).Rows(Index)("iSeqNo")
                        dr("vMedExCode") = ds_Field.Tables(0).Rows(Index)("vMedExCode")
                        dr("vMedExDesc") = ds_Field.Tables(0).Rows(Index)("vMedExDesc")
                        dr("vDefaultValue") = ds_Field.Tables(0).Rows(Index)("vDefaultValue")
                        dr("vLowRange") = ds_Field.Tables(0).Rows(Index)("vLowRange")
                        dr("vHighRange") = ds_Field.Tables(0).Rows(Index)("vHighRange")
                        dr("vAlertonvalue") = ds_Field.Tables(0).Rows(Index)("vAlertonvalue")
                        dr("vAlertMessage") = ds_Field.Tables(0).Rows(Index)("vAlertMessage")
                        dr("cActiveFlag") = ds_Field.Tables(0).Rows(Index)("cActiveFlag")
                        dr("iModifyBy") = Me.Session(S_UserID).ToString()
                        dr("cStatusIndi") = ds_Field.Tables(0).Rows(Index)("cStatusIndi")
                        dr("vMedExGroupCode") = ds_Field.Tables(0).Rows(Index)("vMedExGroupCode")
                        dr("vmedexgroupDesc") = ds_Field.Tables(0).Rows(Index)("vmedexgroupDesc")
                        dr("vMedexGroupCDISCValue") = ds_Field.Tables(0).Rows(Index)("vMedexGroupCDISCValue")
                        dr("vmedexGroupOtherValue") = ds_Field.Tables(0).Rows(Index)("vmedexGroupOtherValue")
                        dr("vMedExSubGroupCode") = ds_Field.Tables(0).Rows(Index)("vMedExSubGroupCode")
                        dr("vmedexsubGroupDesc") = ds_Field.Tables(0).Rows(Index)("vmedexsubGroupDesc")
                        dr("vMedexSubGroupCDISCValue") = ds_Field.Tables(0).Rows(Index)("vMedexSubGroupCDISCValue")
                        dr("vmedexsubGroupOtherValue") = ds_Field.Tables(0).Rows(Index)("vmedexsubGroupOtherValue")
                        dr("vMedExType") = ds_Field.Tables(0).Rows(Index)("vMedExType")
                        dr("vMedExValues") = ds_Field.Tables(0).Rows(Index)("vMedExValues")
                        dr("vUOM") = ds_Field.Tables(0).Rows(Index)("vUOM")
                        dr("vValidationType") = ds_Field.Tables(0).Rows(Index)("vValidationType")
                        dr("cAlertType") = ds_Field.Tables(0).Rows(Index)("cAlertType")
                        dr("cRefType") = ds_Field.Tables(0).Rows(Index)("cRefType")
                        dr("vRefTable") = ds_Field.Tables(0).Rows(Index)("vRefTable")
                        dr("vRefColumn") = ds_Field.Tables(0).Rows(Index)("vRefColumn")
                        dr("vRefFilePath") = ds_Field.Tables(0).Rows(Index)("vRefFilePath")
                        dr("vCDISCValues") = ds_Field.Tables(0).Rows(Index)("vCDISCValues")
                        dr("vOtherValues") = ds_Field.Tables(0).Rows(Index)("vOtherValues")


                        dt_Blank.Rows.Add(dr)
                        dt_Blank.AcceptChanges()
                    Next Index

                    ds_save.Tables.Add(dt_Blank.Copy())
                    ds_save.Tables(0).TableName = "VIEW_MEDEXTEMPLATEDTL"

                    If ds_save.Tables(0).Rows.Count > 0 Then
                        If Not objLambda.Save_MedExTemplateDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExTemplateDtl, ds_save, Me.Session(S_UserID), estr) Then
                            objcommon.ShowAlert("Error While Saving Attribute Details In AttributeTemplateDtl" + estr, Me.Page)
                            Exit Sub
                        End If
                    End If
                End If
            End If

            objcommon.ShowAlert("Attribute Sequence Saved SuccessFully !", Me.Page)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnSaveSequence_Click")
        End Try
    End Sub

    Protected Sub btndeleteMedex_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndeleteMedex.Click
        Dim DtMedExWorkSpaceDtl As DataTable = Nothing
        Dim eStr As String = String.Empty
        Dim dt_DtMedExDtl As New DataTable
        Dim Deleted_dt As New DataTable
        Dim dr As DataRow = Nothing
        Dim chk As CheckBox
        Dim CheckedCheckBox As Boolean = False
        Try

            dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExTemplateDtl), DataTable).Copy
            DtMedExWorkSpaceDtl = dt_DtMedExDtl.Copy
            If Not dt_DtMedExDtl Is Nothing Then
                If dt_DtMedExDtl.Rows.Count > 0 Then
                    For Index = 0 To gvwMedEx.Rows.Count - 1
                        chk = CType(Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_Select).FindControl("ChkMove"), CheckBox)
                        If chk.Checked Then
                            For I = 0 To dt_DtMedExDtl.Rows.Count - 1
                                If dt_DtMedExDtl.Rows(I).Item("vMedExCode").ToString.Trim() = Me.gvwMedEx.Rows(Index).Cells(GvwMedEx_MedExCode).Text.Trim() Then
                                    dt_DtMedExDtl.Rows(I).Item("cStatusIndi") = "D"
                                    dt_DtMedExDtl.Rows(I).Item("cActiveFlag") = "N"
                                    dt_DtMedExDtl.AcceptChanges()
                                    CheckedCheckBox = True
                                End If
                            Next
                        End If
                    Next Index


                    If CheckedCheckBox = False Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "OpenAttributeTemplate", "OpenAttributeTemplate(); ", True)
                        objcommon.ShowAlert("Please Select Atleast One Attribute To Delete", Me.Page)
                        Exit Sub
                    End If

                    dt_DtMedExDtl.DefaultView.RowFilter = "cStatusIndi = 'D'"
                    Deleted_dt = dt_DtMedExDtl.DefaultView.ToTable()

                    If Not DtMedExWorkSpaceDtl.Rows.Count = Deleted_dt.Rows.Count Then
                        Me.ViewState(VS_DtMedExTemplateDtl) = dt_DtMedExDtl
                        If Not FillEditedgrid(dt_DtMedExDtl.Copy()) Then
                            Exit Sub
                        End If
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "OpenAttributeTemplate", "OpenAttributeTemplate(); ", True)
                        objcommon.ShowAlert("You Cannot Delete All From Here,Please Detach The Attribute Template", Me.Page)
                        Exit Sub
                    End If
                    Me.ViewState(Vs_Status) = "Active"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "OpenAttributeTemplate", "OpenAttributeTemplate(); ", True)
                    objcommon.ShowAlert("Selected Attributes Deleted From The Grid", Me.Page)
                End If
            End If
            Exit Try
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......btndeleteMedex")
        End Try
    End Sub

    Protected Sub txtTemplate_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTemplate.Init
        Fillgrid(False)
    End Sub

    Protected Sub btnSetTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Fillgrid(True)
        Me.BtnViewAll.Visible = True
    End Sub

    Protected Sub btnCancelMedEx_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelMedEx.Click
        Try
            Me.gvwMedEx.DataSource = Nothing
            Me.gvwMedEx.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CloseAttributeTemplate", "CloseAttributeTemplate(); ", True)

            Me.ViewState(Vs_Status) = Nothing
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "btnCancelMedEx_Click")
        End Try

    End Sub

#End Region

#Region "FindusedMedexForFormula"

    Protected Function MedexUsedForFormula(ByVal ds_save As DataSet) As Boolean
        Dim ds_MedexGroup As New DataSet
        Dim vMedexcode As String = String.Empty
        Dim wstr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            '======================Added By Mani========================

            wstr = "cStatusIndi <> 'D' order by vMedExGroupDesc"

            If Not objHelp.GetMedExMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_MedexGroup, eStr) Then
                Me.objcommon.ShowAlert("Error While Getting Data from Attribute GroupMst:" + eStr, Me.Page)
                Exit Function
            End If
            For i As Integer = 0 To ds_save.Tables(0).Rows.Count - 1
                vMedexcode += "'" + ds_save.Tables(0).Rows(i).Item("vMedexCode").ToString.Trim + "',"
            Next
            If Not vMedexcode = "" Then
                vMedexcode = vMedexcode.Substring(0, vMedexcode.LastIndexOf(","))
            End If

            Dim dv_co As New DataView
            dv_co = ds_MedexGroup.Tables(0).DefaultView
            dv_co.RowFilter = "vMedexCode in(" + vMedexcode + ") and vMedexType='Formula' and vMedexCodeForFormula is Not Null"
            Dim dt_filterMedxCode As DataTable = dv_co.ToTable

            Dim ds_FilterMedex As New DataSet
            ds_FilterMedex.Tables.Add(dt_filterMedxCode)
            ds_FilterMedex.AcceptChanges()
            Dim vMedexCodeUsed As String = String.Empty

            For i As Integer = 0 To ds_FilterMedex.Tables(0).Rows.Count - 1
                vMedexCodeUsed += ds_FilterMedex.Tables(0).Rows(i).Item("vMedexCodeForFormula")
            Next
            If Not vMedexCodeUsed = "" Then
                vMedexCodeUsed = vMedexCodeUsed.Substring(0, vMedexCodeUsed.LastIndexOf(","))
            End If
            If vMedexCodeUsed <> "" Then
                Dim arrvMedxCode() As String
                arrvMedxCode = vMedexCodeUsed.Split(",")
                Dim Arr As New ArrayList

                For nDx As Integer = 0 To arrvMedxCode.Length - 1
                    If Not Arr.Contains(arrvMedxCode(nDx)) Then
                        Arr.Add(arrvMedxCode(nDx))
                    End If
                Next


                Dim vMedex As String = String.Empty
                Dim ArrMedex As New ArrayList
                Dim dataview As New DataView
                dataview = ds_save.Tables(0).DefaultView

                For Index1 As Integer = 0 To Arr.Count - 1
                    dataview.RowFilter = "vMedexCode='" + Arr(Index1) + "'"
                    If dataview.ToTable.Rows.Count = 0 Then
                        vMedex += "'" + Arr(Index1) + "',"
                    End If
                Next

                Dim medexstring As String = String.Empty
                Dim ds_require As New DataSet
                If Not vMedex = "" Then
                    vMedex = vMedex.Substring(0, vMedex.LastIndexOf(","))
                    Dim dv_New As New DataView
                    dv_New = ds_MedexGroup.Tables(0).DefaultView
                    dv_New.RowFilter = "vMedexCode in(" + vMedex + ")"
                    Dim dt_filter As DataTable = dv_New.ToTable
                    ds_require.Tables.Add(dt_filter)
                    ds_require.AcceptChanges()

                    If ds_require.Tables(0).Rows.Count > 0 Then
                        For index1 As Integer = 0 To ds_require.Tables(0).Rows.Count - 1
                            medexstring += ds_require.Tables(0).Rows(index1).Item("MedExDescWithSubGroup").ToString + " and "
                        Next
                    End If
                    medexstring = medexstring.Substring(0, medexstring.LastIndexOf("and"))
                    Me.objcommon.ShowAlert("Please Add Attribute For Related Formula", Me.Page())
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "OpenAttributeTemplate", "OpenAttributeTemplate(); ", True)
                    'MPEditMedex.Show()
                    Return False
                End If
            End If
            '================================================================================================
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            eStr = ex.Message
            Return False
        End Try
    End Function
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

    '========added on 11-Nov-2009 By Deepak Singh======
    'Protected Sub BtnEditMedex_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnEditMedex.DataBinding

    'End Sub

    'Protected Sub BtnEditMedex_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnEditMedex.Load

    'End Sub

    '#Region "Move Up - Down"

    '    Protected Sub ImgBtnUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnUp.Click
    '        MoveUp_Down("UP")
    '        Me.MPEditMedex.Show()

    '    End Sub

    '    Protected Sub ImgBtnDown_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnDown.Click
    '        MoveUp_Down("DOWN")
    '        Me.MPEditMedex.Show()

    '    End Sub

    '    Private Sub MoveUp_Down(ByVal Move As String)
    '        Dim index As Integer
    '        Dim CntCheck As Integer
    '        Dim SeqNo As Integer
    '        Dim MedexCode As String = String.Empty
    '        Dim DtMedExTemplateDtl As New DataTable
    '        Dim DtMedExTemplateDtlDel As New DataTable
    '        Dim DvMedExWorkSpaceDtl As New DataView
    '        Dim iDr As Integer
    '        Try


    '            CntCheck = 0

    '            For index = 0 To Me.gvwMedEx.Rows.Count - 1
    '                If CType(Me.gvwMedEx.Rows(index).FindControl("ChkMove"), CheckBox).Checked = True Then
    '                    CntCheck += 1
    '                    If CntCheck > 1 Then
    '                        Me.objcommon.ShowAlert("Please select only one", Me.Page)
    '                        Exit Sub
    '                    End If

    '                    SeqNo = Me.gvwMedEx.Rows(index).Cells(GvwMedEx_SeqNo).Text
    '                    MedexCode = Me.gvwMedEx.Rows(index).Cells(GvwMedEx_MedExCode).Text

    '                End If
    '            Next

    '            If CntCheck = 0 Then
    '                Me.objcommon.ShowAlert("Please select Atleast one", Me.Page)
    '                Exit Sub
    '            End If

    '            ' changed to solve problem of seqno
    '            DvMedExWorkSpaceDtl = CType(Me.ViewState(VS_DtMedExTemplateDtl), DataTable).Copy.DefaultView
    '            DvMedExWorkSpaceDtl.RowFilter = " cStatusIndi <> 'D'"
    '            DtMedExTemplateDtl = DvMedExWorkSpaceDtl.ToTable()

    '            For iDr = 0 To DtMedExTemplateDtl.Rows.Count - 1
    '                If DtMedExTemplateDtl.Rows(iDr).Item("iSeqNo") = SeqNo AndAlso _
    '                DtMedExTemplateDtl.Rows(iDr).Item("vMedexCode") = MedexCode Then

    '                    If Move.ToUpper = "UP" Then

    '                        If iDr <= 0 Then
    '                            Me.objcommon.ShowAlert("This Is The First Position", Me.Page)
    '                            Exit For
    '                        End If

    '                        DtMedExTemplateDtl.Rows(iDr).Item("iSeqNo") = DtMedExTemplateDtl.Rows(iDr - 1).Item("iSeqNo")
    '                        DtMedExTemplateDtl.Rows(iDr - 1).Item("iSeqNo") = SeqNo

    '                    End If

    '                    If Move.ToUpper = "DOWN" Then

    '                        If iDr >= DtMedExTemplateDtl.Rows.Count - 1 Then
    '                            Me.objcommon.ShowAlert("This is at Last position", Me.Page)
    '                            Exit For
    '                        End If

    '                        DtMedExTemplateDtl.Rows(iDr).Item("iSeqNo") = DtMedExTemplateDtl.Rows(iDr + 1).Item("iSeqNo")
    '                        DtMedExTemplateDtl.Rows(iDr + 1).Item("iSeqNo") = SeqNo

    '                    End If
    '                    DtMedExTemplateDtl.AcceptChanges()

    '                    Exit For
    '                End If
    '            Next

    '            'To have deleted data again in view state to delete in database : Start
    '            DvMedExWorkSpaceDtl = CType(Me.ViewState(VS_DtMedExTemplateDtl), DataTable).Copy.DefaultView
    '            DvMedExWorkSpaceDtl.RowFilter = " cStatusIndi = 'D'"
    '            DtMedExTemplateDtlDel = DvMedExWorkSpaceDtl.ToTable()

    '            DtMedExTemplateDtl.Merge(DtMedExTemplateDtlDel)
    '            'To have deleted data again in view state to delete in database : End

    '            DvMedExWorkSpaceDtl = DtMedExTemplateDtl.DefaultView
    '            DvMedExWorkSpaceDtl.Sort = "iSeqNo,nMedExTemplateDtlNo" '"iNodeIndex"

    '            Me.ViewState(VS_DtMedExTemplateDtl) = DvMedExWorkSpaceDtl.ToTable()

    '            DvMedExWorkSpaceDtl.RowFilter = "cStatusIndi <> 'D' And cActiveFlag <> 'N'"

    '            Me.gvwMedEx.DataSource = DvMedExWorkSpaceDtl.ToTable()
    '            Me.gvwMedEx.DataBind()

    '            For cntgv As Integer = 0 To Me.gvwMedEx.Rows.Count - 1
    '                If Me.gvwMedEx.Rows(cntgv).Cells(GvwMedEx_MedExCode).Text = MedexCode Then
    '                    CType(Me.gvwMedEx.Rows(cntgv).FindControl("ChkMove"), CheckBox).Checked = True
    '                    Exit For
    '                End If
    '            Next cntgv

    '        Catch ex As Exception
    '            ShowErrorMessage(ex.Message, "........MoveUp_Down")
    '        End Try
    '    End Sub

    '#End Region
End Class
