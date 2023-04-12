
Partial Class frmUnderConstruction
    Inherits System.Web.UI.Page

    Protected Sub BtnHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnHome.Click
        Me.Response.Redirect("frmMainpage.aspx")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = ":: Lambda Therapeutic Research  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
    End Sub
End Class
