
Partial Class UserControls_MultiSelectDropDown
    Inherits System.Web.UI.UserControl

    Public Property WidthCheckListBox() As Integer
        Get

        End Get
        Set(ByVal value As Integer)
            chkList.Width = value
            Panel111.Width = value + 20
        End Set
    End Property

    Public Property Width() As Integer
        Get
            Return CType(txtCombo.Width.Value, Integer)
        End Get
        Set(ByVal value As Integer)
            txtCombo.Width = value
        End Set
    End Property

    Public Property Enabled() As Boolean
        Get

        End Get
        Set(ByVal value As Boolean)
            txtCombo.Enabled = value
        End Set
    End Property

    'Public Property fontSizeCheckBoxList() As FontUnit
    '    Get
    '        Return chkList.Font.Size
    '    End Get
    '    Set(ByVal value As FontUnit)
    '        chkList.Font.Size = value
    '    End Set
    'End Property

    'Public Property fontSizeTextBox() As FontUnit
    '    Get

    '    End Get
    '    Set(ByVal value As FontUnit)
    '        txtCombo.Font.Size = value
    '    End Set
    'End Property

    Public Property Text() As String
        Get
            Return hidVal.Value
            'Return hidVal.Text
        End Get
        Set(ByVal value As String)
            txtCombo.Text = value
        End Set
    End Property

    Public Function AddItems(ByVal array As ArrayList) As Boolean
        For i As Integer = 0 To array.Count - 1
            chkList.Items.Add(array(i).ToString())
        Next
        Return True
    End Function

    Public Function AddItems(ByVal dt As DataTable, ByVal textField As String, ByVal valueField As String) As Boolean
        ClearAll()
        cbList.Visible = True
        chkList.DataSource = dt
        chkList.DataTextField = textField
        chkList.DataValueField = valueField
        chkList.DataBind()
        Me.txtCombo.Enabled = True
        Me.extender.Enabled = True
        AssignText("Please Select Customer")
        Return True
    End Function

    Public Function unselectAllItems() As Boolean
        Me.cbList.Checked = False
        For i As Integer = 0 To chkList.Items.Count - 1
            chkList.Items(i).Selected = False
        Next
        Return True
    End Function
    Public Function selectAllItems() As Boolean
        Me.cbList.Checked = True
        For i As Integer = 0 To chkList.Items.Count - 1
            chkList.Items(i).Selected = True
        Next
        Return True
    End Function

    Public Function ClearAll() As Boolean
        txtCombo.Text = ""
        chkList.Items.Clear()
        cbList.Visible = False
    End Function

    Public Function AssignText(ByVal msg As String) As Boolean
        txtWaterMark.WatermarkText = msg
        Panel111.Width = txtCombo.Width
    End Function

    Public Function GetChkListValue() As String
        Dim Str As String = String.Empty
        For Each items As ListItem In Me.chkList.Items
            If items.Selected Then
                Str += IIf(Str = "", "", ",") + items.Value
            End If
        Next
        Return Str
    End Function

    Public Sub txtCombo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCombo.TextChanged
        If txtCombo.Text <> "" Then
            For Each Item As ListItem In chkList.Items
            Next
        End If
    End Sub

    Public Function SumChkListValue() As String
        Dim Str As String = String.Empty
        For Each items As ListItem In Me.chkList.Items
            If items.Selected Then
                Str += IIf(Str = "", "sum(", ",sum(") + items.Value + ") as " + items.Value
            End If
        Next
        Return Str
    End Function

    Public Function Enable(ByVal status As Boolean) As Boolean
        Me.cbList.Enabled = status
        For i As Integer = 0 To chkList.Items.Count - 1
            chkList.Items(i).Enabled = status
        Next
        Return True
    End Function

End Class
