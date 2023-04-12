
Partial Class frmDrugRegionAnalytesReport
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private eStr_Retu As String
    Private VS_ReportString As String = "ReportString"

    Private Const VS_Choice As String = "Choice"
#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Page.Title = Page.Title = ":: Drug Region Analyst Report :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            If Not Page.IsPostBack Then

                GenCall()
            End If

        Catch ex As Exception
            ObjCommon.ShowAlert(ex.Message, Me)
        End Try
    End Sub

#End Region
    
#Region "GenCall "
    Private Function GenCall() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim dsDrugRegionAnalytesRpt As New DataSet
        Dim wStr As String = ""

        Try

            If Not Request.QueryString("Mode") Is Nothing Then
                Choice = Request.QueryString("Mode")
            End If

            If Not objHelp.GetViewDrugRegionAnalytesRpt("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsDrugRegionAnalytesRpt, eStr_Retu) Then
                ObjCommon.ShowAlert(eStr_Retu, Me)
                Exit Function
            End If

            chkSelectAll.Attributes.Add("onclick", "CheckBoxListSelection(" + Me.chklstDrug.ClientID + ",this);")

            If Not FillDrugCheckBoxList() Then
                Return False
            End If

            If Not Me.ViewState(VS_ReportString) Is Nothing Then
                Me.btnExportToExcel.Visible = True
            Else
                Me.btnExportToExcel.Visible = False
            End If

        Catch ex As Exception
            ObjCommon.ShowAlert(ex.Message, Me.Page)
        End Try
    End Function
#End Region

#Region "FillDrugCheckBoxList "

    Private Function FillDrugCheckBoxList() As Boolean
        Dim dsDrug As New DataSet

        CType(Me.Master.FindControl("lblHeading"), Label).Text = "Drug Region Analytes Report"

        If Not objHelp.getdrugmst(" cActiveFlag<>'N' And cStatusIndi<>'D' ORDER BY vDrugName ASC ", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsDrug, eStr_Retu) Then
            ObjCommon.ShowAlert(eStr_Retu, Me)
            Exit Function
        End If

        Me.chklstDrug.DataSource = dsDrug.Tables(0)
        Me.chklstDrug.DataValueField = "vDrugCode"
        Me.chklstDrug.DataTextField = "vDrugName"
        Me.chklstDrug.DataBind()

        For Each lstItem As ListItem In chklstDrug.Items

            lstItem.Attributes.Add("onclick", "SetCheckUncheckAll(document.getElementById('" + _
                                            Me.chklstDrug.ClientID + "'), document.getElementById('" + _
                                            Me.chkSelectAll.ClientID + "'));")

        Next lstItem

        Return True

    End Function

#End Region

#Region "REPORT GENERATION "

    Private Sub GenerateReport(ByVal dsReportData As DataSet)
        Dim strReport As String = ""
        Dim dtReport As New DataTable
        Dim strDrugRow As String = ""
        Dim currentDrugCode As String = "0"
        Dim currentRegionCode As String = "0"
        Dim dataViewTemp As New DataView
        Dim rowCount As Integer = 0
        dtReport = dsReportData.Tables(0)

        strReport = "<table style=""border:solid 1px black;font-name:Arial; font-size:10pt; color:white; font-weight:bold;"" " + _
            " cellspacing=""0px"" cellpadding=""5px"" >"

        strReport += GetHeader()

        For Each dr As DataRow In dtReport.Rows
            If dr("vDrugCode").ToString <> currentDrugCode Then
                Dim dtview As New DataView
                dtview = dtReport.DefaultView
                dtview.RowFilter = "vDrugCode='" & dr("vDrugCode").ToString() & "'"

                currentDrugCode = dr("vDrugCode").ToString
                currentRegionCode = dr("vRegionCode").ToString
                rowCount = dtview.ToTable(True, New String() {"vDrugCode", "vRegionCode"}).Rows.Count
                If dtview.ToTable().Rows.Count > 1 Then
                    strDrugRow = "<tr><td  rowspan=" & rowCount & " style=""background-color:#FFC894;color:navy;border:solid 1px black; vertical-align:middle"">"
                Else
                    strDrugRow = "<tr><td style=""border:solid 1px black;background-color:#FFC894;color:navy"">"
                End If

                strDrugRow += GetDrugRow(dr)

                'strDrugRow += "<td style=""background-color:#FFC894;color:navy"">"
                strDrugRow += GetRegionRow(dr)

                strDrugRow += "</tr>"
            ElseIf (dr("vDrugCode").ToString = currentDrugCode And dr("vRegionCode") = currentRegionCode) Then
                currentDrugCode = dr("vDrugCode").ToString
                currentRegionCode = dr("vRegionCode").ToString
                Continue For
            Else
                currentDrugCode = dr("vDrugCode").ToString
                currentRegionCode = dr("vRegionCode").ToString
                strDrugRow = "<tr>"
                'New Region add TD
                strDrugRow += GetRegionRow(dr)
                strDrugRow += "</tr>"
            End If
            '   

            strReport += strDrugRow
        Next

        strReport += "</table>"
        Me.ViewState(VS_ReportString) = strReport
        Me.lblReport.Text = strReport
    End Sub

    Private Function GetHeader() As String
        Dim strHeader As String = ""

        'strHeader = "<tr style=""border:solid 1px black;background-color:#FF9900;"" >" + _
        strHeader = "<tr >" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Drug Name</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Country</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">RLD</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">MFG BY</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Drug</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">CMAX</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">TMAX</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">t<sub>1/2</sub></td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Linearity</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Analysis Method</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Analyte</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">CMAX</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">TMAX </td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">t<sub>1/2</sub></td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Linearity</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Analysis Method</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Analyte</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">CMAX</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">TMAX </td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">t<sub>1/2</sub></td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Linearity</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Analysis Method</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Analyte</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">CMAX</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">TMAX </td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">t<sub>1/2</sub></td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Linearity</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Analysis Method</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Analyte</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">CMAX</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">TMAX </td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">t<sub>1/2</sub></td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Linearity</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Analysis Method</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">Food Effect</td>" + _
            "<td style=""border:solid 1px black;background-color:#FF9900;"">COMMENTS</td>" + _
            "</tr>"

        Return strHeader
    End Function

    Private Function GetDrugRow(ByVal dt As DataRow) As String
        Dim strDrugRow As String = ""
        strDrugRow += IIf(dt("vDrugName") Is DBNull.Value, "&nbsp;", dt("vDrugName")) + "</td>"

        Return strDrugRow

    End Function

    Private Function GetRegionRow(ByVal dr As DataRow) As String
        Dim strRegionRow As String = ""
        If Not dr("vRegionName") Is Nothing Then
            strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vRegionName") Is DBNull.Value, "&nbsp;", dr("vRegionName").ToString) + "</td>"
        Else
            strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + "Not Available" + "</td>"
        End If
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vRLDDetails") Is DBNull.Value, "&nbsp;", dr("vRLDDetails")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vManufacturer") Is DBNull.Value, "&nbsp;", dr("vManufacturer")) + "</td>"


        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf((dr("Drug") Is DBNull.Value Or dr("Drug").ToString = ""), "&nbsp;", dr("Drug")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf((dr("vCMaxValue") Is DBNull.Value Or dr("vCMaxValue").ToString = ""), "&nbsp;", dr("vCMaxValue")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vTMaxValue") Is DBNull.Value, "&nbsp;", dr("vTMaxValue")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vT1/2Value") Is DBNull.Value, "&nbsp;", dr("vT1/2Value")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vLinearityRange") Is DBNull.Value, "&nbsp;", dr("vLinearityRange")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vAnalysisMethod") Is DBNull.Value, "&nbsp;", dr("vAnalysisMethod")) + "</td>"

        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf((dr("Analyte1") Is DBNull.Value Or dr("Analyte1").ToString = ""), "&nbsp;", dr("Analyte1")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf((dr("vCMaxValue1") Is DBNull.Value Or dr("vCMaxValue1").ToString = ""), "&nbsp;", dr("vCMaxValue1")) + "</td>"
        'strRegionRow += "<td style=""background-color:#FFC894;color:navy"">" + dr("vCMaxValue") + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vTMaxValue1") Is DBNull.Value, "&nbsp;", dr("vTMaxValue1")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vT1/2Value1") Is DBNull.Value, "&nbsp;", dr("vT1/2Value1")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vLinearityRange1") Is DBNull.Value, "&nbsp;", dr("vLinearityRange1")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vAnalysisMethod1") Is DBNull.Value, "&nbsp;", dr("vAnalysisMethod1")) + "</td>"

        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf((dr("Analyte2") Is DBNull.Value Or dr("Analyte2").ToString = ""), "&nbsp;", dr("Analyte2")) + "</td>"
        'strRegionRow += "<td style=""background-color:#FFC894;color:navy"">" + dr("vCMaxValue2") + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf((dr("vCMaxValue2") Is DBNull.Value Or dr("vCMaxValue2").ToString = ""), "&nbsp;", dr("vCMaxValue2")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vTMaxValue2") Is DBNull.Value, "&nbsp;", dr("vTMaxValue2")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vT1/2Value2") Is DBNull.Value, "&nbsp;", dr("vT1/2Value2")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vLinearityRange2") Is DBNull.Value, "&nbsp;", dr("vLinearityRange2")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vAnalysisMethod2") Is DBNull.Value, "&nbsp;", dr("vAnalysisMethod2")) + "</td>"

        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf((dr("Analyte3") Is DBNull.Value Or dr("Analyte3").ToString = ""), "&nbsp;", dr("Analyte3")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf((dr("vCMaxValue3") Is DBNull.Value Or dr("vCMaxValue3").ToString = ""), "&nbsp;", dr("vCMaxValue3")) + "</td>"
        'strRegionRow += "<td style=""background-color:#FFC894;color:navy"">" + dr("vCMaxValue3") + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vTMaxValue3") Is DBNull.Value, "&nbsp;", dr("vTMaxValue3")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vT1/2Value3") Is DBNull.Value, "&nbsp;", dr("vT1/2Value3")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vLinearityRange3") Is DBNull.Value, "&nbsp;", dr("vLinearityRange3")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vAnalysisMethod3") Is DBNull.Value, "&nbsp;", dr("vAnalysisMethod3")) + "</td>"

        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf((dr("Analyte4") Is DBNull.Value Or String.IsNullOrEmpty(dr("Analyte4"))), "&nbsp;", dr("Analyte4")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf((dr("vCMaxValue4") Is DBNull.Value Or dr("vCMaxValue4").ToString = ""), "&nbsp;", dr("vCMaxValue4")) + "</td>"
        'strRegionRow += "<td style=""background-color:#FFC894;color:navy"">" + dr("vCMaxValue4") + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf((dr("vTMaxValue4") Is DBNull.Value Or String.IsNullOrEmpty(dr("vTMaxValue4").ToString)), "&nbsp;", dr("vTMaxValue4")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vT1/2Value4") Is DBNull.Value, "&nbsp;", dr("vT1/2Value4")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vLinearityRange4") Is DBNull.Value, "&nbsp;", dr("vLinearityRange4")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vAnalysisMethod4") Is DBNull.Value, "&nbsp;", dr("vAnalysisMethod4")) + "</td>"

        'strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + "TEMP" + "</td>"
        'strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + "TEMP" + "</td>"
        'strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + "TEMP" + "</td>"

        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf((dr("vFoodEffect") Is DBNull.Value Or String.IsNullOrEmpty(dr("vFoodEffect").ToString)), "&nbsp;", dr("vFoodEffect")) + "</td>"
        'strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vAnalysisMethod") Is DBNull.Value, "&nbsp;", dr("vAnalysisMethod")) + "</td>"
        'strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vLinearityRange") Is DBNull.Value, "&nbsp;", dr("vLinearityRange")) + "</td>"
        strRegionRow += "<td style=""border:solid 1px black;background-color:#FFC894;color:navy"">" + IIf(dr("vComments") Is DBNull.Value, "&nbsp;", dr("vComments")) + "</td>"

        Return strRegionRow
    End Function

#End Region

#Region "BUTTON EVENTS "

    Protected Sub btnViewReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewReport.Click
        Dim strInCondition As String = ""
        Dim dsReportData As New DataSet

        For Each lstItem As ListItem In chklstDrug.Items
            If lstItem.Selected Then
                strInCondition += IIf(strInCondition = "", lstItem.Value, "," + lstItem.Value)
            End If
        Next

        If strInCondition.Length > 0 Then
            If Not objHelp.GetViewDrugRegionAnalytesRpt(" vDrugCode in (" + strInCondition + ") And cStatusIndi <> 'D'", _
                                     WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsReportData, eStr_Retu) Then
                ObjCommon.ShowAlert(eStr_Retu, Me)
            Else
                Me.GenerateReport(dsReportData)
            End If
        End If

        If Not Me.ViewState(VS_ReportString) Is Nothing Then
            Me.btnExportToExcel.Visible = True
        Else
            Me.btnExportToExcel.Visible = False
        End If



    End Sub

    Protected Sub btnExportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportToExcel.Click
        Dim strReportString As String
        Dim fileName As String = "Drug-Region-Analytes_" & DateTime.Now.ToString("dd-MMM-yy")
        If Me.ViewState(VS_ReportString) Is Nothing Then
            Exit Sub
        End If

        strReportString = Me.ViewState(VS_ReportString)

        Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
        Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
        Context.Response.Write(strReportString.ToString)
        Context.Response.Flush()
        Context.Response.End()
        System.IO.File.Delete(fileName)

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.chkSelectAll.Checked = False
        Me.chklstDrug.ClearSelection()
        Me.ViewState(VS_ReportString) = Nothing
        Me.lblReport.Text = ""
        If Not GenCall() Then
            Exit Sub
        End If
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "chkSelectAll_CheckedChanged"
    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        chklstDrug.ClearSelection()
        For Each lstItem As ListItem In chklstDrug.Items
            lstItem.Selected = chkSelectAll.Checked
        Next
    End Sub
#End Region

End Class
