
Partial Class frmPrint
    Inherits System.Web.UI.Page

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("frmEctdDetails.aspx?type=d")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = Page.Title = ":: Print Page :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
    End Sub
End Class
