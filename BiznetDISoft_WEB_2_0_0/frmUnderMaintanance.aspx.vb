
Partial Class frmUnderMaintanance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Dim ObjCommon As New clsCommon
            Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
            Dim ds_DownTime As DataSet = Nothing


            ds_DownTime = objHelp.ProcedureExecute("dbo.Proc_DownTimeMaster", Session(S_UserID))

            If Not ds_DownTime Is Nothing AndAlso ds_DownTime.Tables(0).Rows.Count > 0 Then
                lblText.Text = "This site will not accessible during Time " + ds_DownTime.Tables(0).Rows(0)(5).ToString() + "  To  " + ds_DownTime.Tables(0).Rows(0)(6).ToString()
            Else
                'lblText.Text = "This site will not accessible during Time " + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "  To  " + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm")
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class
