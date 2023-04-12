'****
' This class contain the information for particular line for one cell.
'****
<System.Serializable()> _
Public Class RepoLine
    Inherits clsCommon
#Region "Enum"
    '-- how to display the line
    '
    Public Enum LineTypeEnum
        NoLine = 0
        SingleLine = 1
        DoubleLine = 2
    End Enum
#End Region
#Region "Private variable"
    Private _LineType As LineTypeEnum
    Private _LineWidth As Single
    Private _LineColor As System.Drawing.Color
#End Region
#Region "Constructor"
    '-- default constructor
    Public Sub New()

        Me.New(LineTypeEnum.NoLine, 0.5, Drawing.Color.Black)

    End Sub
    Public Sub New(ByVal LineType_1 As LineTypeEnum, _
                   ByVal LineWidth_1 As Single, _
                   ByVal LineColor_1 As System.Drawing.Color)

        _LineType = LineType_1
        _LineWidth = LineWidth_1
        _LineColor = LineColor_1

    End Sub
#End Region
#Region "Public Properties"
    Public Property LineType() As LineTypeEnum
        Get
            Return _LineType
        End Get
        Set(ByVal value As LineTypeEnum)
            _LineType = value
        End Set
    End Property
    Public Property LineWidth() As Single
        Get
            Return _LineWidth
        End Get
        Set(ByVal value As Single)
            _LineWidth = value
        End Set
    End Property
    Public Property LineColor() As System.Drawing.Color
        Get
            Return _LineColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            _LineColor = value
        End Set
    End Property
    Protected Overrides Sub Finalize()
        _LineColor = Nothing
    End Sub
#End Region
End Class
'****
' This class is desing one row per page. 
'****
<System.Serializable()> _
Public Class RepoRow
    Inherits clsCommon
#Region "Private variables"
    Private RepoCellArray() As RepoCell
#End Region
#Region "Constructor"
    Public Sub New()

    End Sub
#End Region
#Region "Method"
    Public Function AddCell(ByVal Name_1 As String) As RepoCell

        Dim rCell As RepoCell
        rCell = New RepoCell(Name_1)
        Return Me.AddCell(rCell)

    End Function
    Public Function AddCell(ByVal Cell_1 As RepoCell) As RepoCell

        Dim tLen_Array As Integer

        Try
            tLen_Array = -1
            tLen_Array = RepoCellArray.GetUpperBound(0)

            For Each rCell As RepoCell In RepoCellArray

                If rCell.CellName.Trim().ToUpper() = Cell_1.CellName.ToUpper().Trim() Then
                    Throw New Exception("<" + Cell_1.CellName + "> cell is already exist.")
                End If

            Next rCell

        Catch ex As Exception
        End Try

        tLen_Array += 1
        ReDim Preserve RepoCellArray(tLen_Array)
        RepoCellArray(tLen_Array) = Cell_1

        Return Cell_1

    End Function
    Public ReadOnly Property CellCount() As Integer
        Get
            CellCount = RepoCellArray.Length
        End Get
    End Property
    Public ReadOnly Property Cell(ByVal Index_1 As String) As RepoCell
        Get
            For Each rCell As RepoCell In RepoCellArray
                If rCell.CellName.Trim().ToUpper() = Index_1.ToUpper().Trim() Then
                    Cell = rCell
                    Exit Property
                End If
            Next rCell
        End Get
    End Property
    Public ReadOnly Property Cell(ByVal Index_1 As Integer) As RepoCell
        Get
            Return RepoCellArray(Index_1)
        End Get
    End Property

    Public Overrides Function ToString() As String

        Dim retuVal As String = ""
        Dim MaxHeight1 As Single

        For Each rCell As RepoCell In RepoCellArray

            retuVal += rCell.ToString()

            If MaxHeight1 <= rCell.ActualCellHeight Then
                MaxHeight1 = rCell.ActualCellHeight
            End If

        Next rCell

        retuVal = "<TR Style='mso-height-source:userset; height:" + MaxHeight1.ToString() + "pt'>" + vbCrLf + _
                  retuVal + vbCrLf + _
                  "</TR>" + vbCrLf

        Return retuVal
    End Function
    Friend Sub SetBlankValues2Cells()
        For Each rCell As RepoCell In RepoCellArray
            rCell.Value = ""
        Next rCell
    End Sub
    Public Function Copy() As RepoRow
        Dim r As RepoRow = New RepoRow
        Dim nCell As RepoCell

        For Each rCell As RepoCell In RepoCellArray

            nCell = New RepoCell(rCell.CellName, "", rCell.Visible, rCell.Width, rCell.Height, rCell.Alignment, _
                                 rCell.NoofCellContain, rCell.FontSize, rCell.FontName, rCell.FontBold, _
                                 rCell.FontItalic, rCell.FontUnderLine, rCell.FontColor, rCell.BackgroundColor, _
                                 rCell.LineLeft, rCell.LineRight, rCell.LineTop, rCell.LineBottom)

            r.AddCell(nCell)
        Next rCell

        Return r

    End Function
#End Region

End Class

Public Class ReportGeneralFunction
    Public Enum ExportFileAsEnum
        ExportAsXLS = 0
        ExportAsPDF = 1
        ExportAsDoc = 2
    End Enum
    Public Shared Function ExportFileAtWeb(ByVal ExportIn_1 As ExportFileAsEnum, _
                                           ByVal Response_1 As Object, _
                                           ByVal ReportFileName_1 As String, _
                                           Optional ByVal CanDelete_1 As Boolean = True) As Boolean
        Dim SendFile As FileInfo
        Dim line_1 As Integer

        If ReportFileName_1 = "" Then
            Throw New Exception("Invalid File Name")
        End If

        Try

            SendFile = New FileInfo(ReportFileName_1)

            Response_1.ClearContent()

            Response_1.ClearHeaders()

            Response_1.AppendHeader("Content-Disposition", "attachment; filename =" + SendFile.Name)
            'Response_1.AppendHeader("Content-Length:", SendFile.Length.ToString())

            If ExportIn_1 = ExportFileAsEnum.ExportAsXLS Then
                Response_1.ContentType = "application/vnd.ms-excel"
            ElseIf ExportIn_1 = ExportFileAsEnum.ExportAsDoc Then
                Response_1.ContentType = "application/vnd.ms-word"
            End If
            Response_1.TransmitFile(ReportFileName_1)
            Response_1.Flush()
            Response_1.End()



            Return True
        Catch ex As Exception
            If Not TypeOf ex Is System.Threading.ThreadAbortException Then
                Throw New Exception(ex.Message + vbCrLf + "Error occured while Exporting File " + line_1.ToString())
            End If
        Finally


            Try
                If File.Exists(ReportFileName_1) And CanDelete_1 Then
                    File.Delete(ReportFileName_1)
                End If
            Catch ex As Exception
            End Try
        End Try

    End Function


End Class

'***
' This class contain the particular one cell properties.
'***
<System.Serializable()> _
Public Class RepoCell
#Region "Enum"
    '-- enum represent the alignment of how the particular cell is alligned
    '
    Public Enum AlignmentEnum
        LeftTop = 0
        LeftMiddle = 1
        LeftBottom = 2
        CenterTop = 3
        CenterMiddle = 4
        CenterBottom = 5
        RightTop = 6
        RightMiddle = 7
        RightBottom = 8
    End Enum
#End Region
#Region "Private Variable"
    Private _Name As String
    Private _Value As String
    Private _Width As Single
    Private _Height As Single
    Private _Alignment As AlignmentEnum
    Private _FontSize As Single
    Private _FontName As String
    Private _FontBold As Boolean
    Private _FontItalic As Boolean
    Private _FontUnderLine As Boolean
    Private _FontColor As System.Drawing.Color
    Private _BackgroundColor As System.Drawing.Color
    Private _LineLeft As RepoLine
    Private _LineRight As RepoLine
    Private _LineTop As RepoLine
    Private _LineBottom As RepoLine
    Private _NoofCellContain As Integer
    Private _Visible As Boolean
    Private _ClassName As String ' This property is set from repopage only
#End Region
#Region "Constructor"
    Public Sub New()
        Me.New("")
    End Sub
    Public Sub New(ByVal Name_1 As String)
        Me.New(Name_1, "", True, 0, 11, AlignmentEnum.LeftTop, 1, 11, "Verdana", False, False, False, Drawing.Color.Black, _
             Drawing.Color.Transparent, New RepoLine, New RepoLine, New RepoLine, New RepoLine)
    End Sub
    Public Sub New(ByVal Name_1 As String, ByVal Value_1 As String, ByVal Visible_1 As Boolean, ByVal Width_1 As Single, _
                   ByVal Height_1 As Single, ByVal Alignment_1 As AlignmentEnum, ByVal NoOfCellContain_1 As Integer, _
                   ByVal FontSize_1 As Single, ByVal FontName_1 As String, ByVal FontBold_1 As Boolean, _
                   ByVal FontItalic_1 As Boolean, ByVal FontUnderLine_1 As Boolean, ByVal FontColor_1 As System.Drawing.Color, _
                   ByVal BackgroundColor_1 As System.Drawing.Color, ByVal LineLeft_1 As RepoLine, _
                   ByVal LineRight_1 As RepoLine, ByVal LineTop_1 As RepoLine, ByVal LineBottom_1 As RepoLine)

        _Name = Name_1
        _Value = Value_1
        _Width = Width_1
        _Height = Height_1
        _Alignment = Alignment_1
        _FontSize = FontSize_1
        _FontName = FontName_1
        _FontBold = FontBold_1
        _FontItalic = FontItalic_1
        _FontUnderLine = FontUnderLine_1
        _FontColor = FontColor_1
        _BackgroundColor = BackgroundColor_1
        _LineLeft = LineLeft_1
        _LineRight = LineRight_1
        _LineTop = LineTop_1
        _LineBottom = LineBottom_1
        _NoofCellContain = NoOfCellContain_1
        _Visible = Visible_1
    End Sub
#End Region
#Region "Public Properties"
    Friend WriteOnly Property ClassName() As String
        Set(ByVal value As String)
            _ClassName = value
        End Set
    End Property
    Public Property CellName() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property
    Public Property Value() As String
        Get
            Return _Value
        End Get
        Set(ByVal value_1 As String)
            _Value = value_1
        End Set
    End Property
    Public Property Width() As Single
        Get
            Return _Width
        End Get
        Set(ByVal value As Single)
            If value >= 0 Then
                _Width = value
            End If
        End Set
    End Property
    Public Property Height() As Single
        Get
            Return _Height
        End Get
        Set(ByVal value As Single)
            If value >= 0 Then
                _Height = value
            End If
        End Set
    End Property
    Public Property Alignment() As AlignmentEnum
        Get
            Return _Alignment
        End Get
        Set(ByVal value As AlignmentEnum)
            _Alignment = value
        End Set
    End Property
    Public Property FontSize() As String
        Get
            Return _FontSize
        End Get
        Set(ByVal value As String)
            If FontSize > 0 Then
                _FontSize = value

                If _Height <= _FontSize Then
                    _Height = _FontSize
                End If

            End If
        End Set
    End Property
    Public Property FontName() As String
        Get
            Return _FontName
        End Get
        Set(ByVal value As String)
            If Trim(value) <> "" Then
                _FontName = value
            End If
        End Set
    End Property
    Public Property FontBold() As Boolean
        Get
            Return _FontBold
        End Get
        Set(ByVal value As Boolean)
            _FontBold = value
        End Set
    End Property
    Public Property FontItalic() As Boolean
        Get
            Return _FontItalic
        End Get
        Set(ByVal value As Boolean)
            _FontItalic = value
        End Set
    End Property
    Public Property FontUnderLine() As Boolean
        Get
            Return _FontUnderLine
        End Get
        Set(ByVal value As Boolean)
            _FontUnderLine = value
        End Set
    End Property
    Public Property FontColor() As System.Drawing.Color
        Get
            Return _FontColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            _FontColor = value
        End Set
    End Property
    Public Property BackgroundColor() As System.Drawing.Color
        Get
            Return _BackgroundColor
        End Get
        Set(ByVal value As System.Drawing.Color)
            _BackgroundColor = value
        End Set
    End Property
    Public Property LineLeft() As RepoLine
        Get
            Return _LineLeft
        End Get
        Set(ByVal value As RepoLine)
            _LineLeft = value
        End Set
    End Property
    Public Property LineTop() As RepoLine
        Get
            Return _LineTop
        End Get
        Set(ByVal value As RepoLine)
            _LineTop = value
        End Set
    End Property
    Public Property LineBottom() As RepoLine
        Get
            Return _LineBottom
        End Get
        Set(ByVal value As RepoLine)
            _LineBottom = value
        End Set
    End Property
    Public Property LineRight() As RepoLine
        Get
            Return _LineRight
        End Get
        Set(ByVal value As RepoLine)
            _LineRight = value
        End Set
    End Property
    Public Property NoofCellContain() As Integer
        Get
            Return _NoofCellContain
        End Get
        Set(ByVal value As Integer)
            _NoofCellContain = value
        End Set
    End Property
    Public Property Visible() As Boolean
        Get
            Return _Visible
        End Get
        Set(ByVal value As Boolean)
            _Visible = value
        End Set
    End Property
#End Region
#Region "Method"
    Public Overrides Function ToString() As String

        Dim retuval As String = ""


        If Not _Visible Then
            Return ""
        End If

        retuval = "<td " + _
                  "colspan=" + IIf(_NoofCellContain <= 0, 1, _NoofCellContain).ToString() + "  " + _
                  "class=" + _ClassName + ">" + _Value + "</td>"
        _ClassName = ""
        Return retuval
    End Function
    Protected Overrides Sub Finalize()
        _FontColor = Nothing
        _BackgroundColor = Nothing
        _LineLeft = Nothing
        _LineRight = Nothing
        _LineTop = Nothing
        _LineBottom = Nothing
    End Sub
    Friend Function ActualCellHeight() As Single

        If Not _Visible Then
            Return 0
        End If

        ActualCellHeight = _Height + 2.5 + IIf(_FontBold, 1.5, 0)
    End Function
    Public Function Copy() As RepoCell
        Dim rCell As New RepoCell

        rCell.Alignment = _Alignment
        rCell.BackgroundColor = _BackgroundColor
        rCell.CellName = _Name
        rCell.FontBold = _FontBold
        rCell.FontColor = _FontColor
        rCell.FontItalic = _FontItalic
        rCell.FontName = _FontName
        rCell.FontSize = _FontSize
        rCell.FontUnderLine = _FontUnderLine
        rCell.Height = _Height

        rCell.LineBottom.LineColor = _LineBottom.LineColor
        rCell.LineBottom.LineType = _LineBottom.LineType
        rCell.LineBottom.LineWidth = _LineBottom.LineWidth

        rCell.LineTop.LineColor = _LineTop.LineColor
        rCell.LineTop.LineType = _LineTop.LineType
        rCell.LineTop.LineWidth = _LineTop.LineWidth

        rCell.LineLeft.LineColor = _LineLeft.LineColor
        rCell.LineLeft.LineType = _LineLeft.LineType
        rCell.LineLeft.LineWidth = _LineLeft.LineWidth

        rCell.LineRight.LineColor = _LineRight.LineColor
        rCell.LineRight.LineType = _LineRight.LineType
        rCell.LineRight.LineWidth = _LineRight.LineWidth

        rCell.NoofCellContain = _NoofCellContain
        rCell.Value = _Value
        rCell.Visible = _Visible
        rCell.Width = _Width

        Return rCell

    End Function
#End Region

End Class
Public Class RepoPage
    Inherits clsCommon
#Region "Private variable"
    Private Const ReportCloseError As String = "User can’t add any information. Report is already closed or not open."
    Private RepoFile As System.IO.StreamWriter
    Private IsReportOpen As Boolean
    Private _dsStyleSheet As Data.DataTable
    Private _ReportFileName As String
#End Region
#Region "Loading Methods and Constructor"
    Public Sub New(ByVal FileName_1 As String)

        Try
            CreateAndFillStyleSheet()

            If System.IO.File.Exists(FileName_1) Then
                System.IO.File.SetAttributes(FileName_1, IO.FileAttributes.Normal)
                System.IO.File.Delete(FileName_1)
            End If

            RepoFile = New System.IO.StreamWriter(FileName_1, False)
            IsReportOpen = True
            _ReportFileName = FileName_1

        Catch ex As Exception

            If Not RepoFile Is Nothing Then
                RepoFile.Close()
                RepoFile = Nothing
            End If
            IsReportOpen = False

            Throw New Exception(ex.Message + vbCrLf + "Error occured while creating Report file")
        End Try

    End Sub
    Private Function CreateAndFillStyleSheet() As Boolean

        Dim r As Data.DataRow

        _dsStyleSheet = New Data.DataTable

        _dsStyleSheet.Columns.Add("StyleNo", System.Type.GetType("System.Int32"))
        _dsStyleSheet.Columns.Add("ClassName", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("Alignment", System.Type.GetType("System.Int32"))
        _dsStyleSheet.Columns.Add("CellHeight", System.Type.GetType("System.Double"))
        _dsStyleSheet.Columns.Add("CellWidth", System.Type.GetType("System.Double"))
        _dsStyleSheet.Columns.Add("FontName", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("FontSize", System.Type.GetType("System.Single"))
        _dsStyleSheet.Columns.Add("FontBold", System.Type.GetType("System.Boolean"))
        _dsStyleSheet.Columns.Add("FontItalic", System.Type.GetType("System.Boolean"))
        _dsStyleSheet.Columns.Add("FontUnderLine", System.Type.GetType("System.Boolean"))
        _dsStyleSheet.Columns.Add("FontColor", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("FontColorName", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("BackColor", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("BackColorName", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("LineTopStyle", System.Type.GetType("System.Int32"))
        _dsStyleSheet.Columns.Add("LineTopWidth", System.Type.GetType("System.Single"))
        _dsStyleSheet.Columns.Add("LineTopColor", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("LineTopColorName", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("LineLeftStyle", System.Type.GetType("System.Int32"))
        _dsStyleSheet.Columns.Add("LineLeftWidth", System.Type.GetType("System.Single"))
        _dsStyleSheet.Columns.Add("LineLeftColor", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("LineLeftColorName", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("LineBottomStyle", System.Type.GetType("System.Int32"))
        _dsStyleSheet.Columns.Add("LineBottomWidth", System.Type.GetType("System.Single"))
        _dsStyleSheet.Columns.Add("LineBottomColor", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("LineBottomColorName", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("LineRightStyle", System.Type.GetType("System.Int32"))
        _dsStyleSheet.Columns.Add("LineRightWidth", System.Type.GetType("System.Single"))
        _dsStyleSheet.Columns.Add("LineRightColor", System.Type.GetType("System.String"))
        _dsStyleSheet.Columns.Add("LineRightColorName", System.Type.GetType("System.String"))

        r = _dsStyleSheet.NewRow
        r("StyleNo") = 1
        r("ClassName") = "SSPL1"
        r("Alignment") = CType(RepoCell.AlignmentEnum.LeftTop, Integer)
        r("CellHeight") = 11
        r("CellWidth") = 0
        r("FontName") = "Verdana"
        r("FontSize") = 11
        r("FontBold") = False
        r("FontItalic") = False
        r("FontUnderLine") = False
        r("FontColor") = Drawing.Color.Black.ToString()
        r("FontColorName") = System.Drawing.ColorTranslator.ToHtml(Drawing.Color.Black)
        r("BackColor") = Drawing.Color.Transparent.ToString()
        r("BackColorName") = System.Drawing.ColorTranslator.ToHtml(Drawing.Color.Transparent)

        r("LineTopStyle") = RepoLine.LineTypeEnum.NoLine
        r("LineTopWidth") = 0.5
        r("LineTopColor") = Drawing.Color.Black.ToString()
        r("LineTopColorName") = System.Drawing.ColorTranslator.ToHtml(Drawing.Color.Black)

        r("LineLeftStyle") = RepoLine.LineTypeEnum.NoLine
        r("LineLeftWidth") = 0.5
        r("LineLeftColor") = Drawing.Color.Black.ToString()
        r("LineLeftColorName") = System.Drawing.ColorTranslator.ToHtml(Drawing.Color.Black)

        r("LineBottomStyle") = RepoLine.LineTypeEnum.NoLine
        r("LineBottomWidth") = 0.5
        r("LineBottomColor") = Drawing.Color.Black.ToString()
        r("LineBottomColorName") = System.Drawing.ColorTranslator.ToHtml(Drawing.Color.Black)

        r("LineRightStyle") = RepoLine.LineTypeEnum.NoLine
        r("LineRightWidth") = 0.5
        r("LineRightColor") = Drawing.Color.Black.ToString()
        r("LineRightColorName") = System.Drawing.ColorTranslator.ToHtml(Drawing.Color.Black)

        _dsStyleSheet.Rows.Add(r)


    End Function
    Private Function GetStyleSheet() As String

        Dim retuVal As String

        retuVal = "tr " + vbCrLf
        retuVal += "{mso-height-source:auto;} " + vbCrLf

        retuVal += " col " + vbCrLf
        retuVal += "{mso-width-source:auto;} " + vbCrLf

        retuVal += " br " + vbCrLf
        retuVal += "{mso-data-placement:same-cell;} " + vbCrLf

        retuVal += " .style0 " + vbCrLf
        retuVal += "{mso-number-format:General; " + vbCrLf
        retuVal += "text-align:general; " + vbCrLf
        retuVal += "vertical-align:top; " + vbCrLf
        retuVal += "white-space:normal; " + vbCrLf
        retuVal += "mso-rotate:0; " + vbCrLf
        retuVal += "mso-background-source:auto; " + vbCrLf
        retuVal += "mso-pattern:auto; " + vbCrLf
        retuVal += "color:black; " + vbCrLf
        retuVal += "font-size:11.0pt; " + vbCrLf
        retuVal += "font-weight:400; " + vbCrLf
        retuVal += "font-style:normal; " + vbCrLf
        retuVal += "text-decoration:none; " + vbCrLf
        retuVal += "font-family:Calibri, sans-serif; " + vbCrLf
        retuVal += "mso-font-charset:0; " + vbCrLf
        retuVal += "border:none; " + vbCrLf
        retuVal += "mso-protection:locked visible; " + vbCrLf
        retuVal += "mso-style-name:Normal; " + vbCrLf
        retuVal += "mso-style-id:0;} " + vbCrLf

        retuVal += "td " + vbCrLf
        retuVal += "{mso-style-parent:style0; " + vbCrLf
        retuVal += "padding-top:1pt; " + vbCrLf
        retuVal += "padding-right:1pt; " + vbCrLf
        retuVal += "padding-left:1pt; " + vbCrLf
        retuVal += "mso-ignore:padding; " + vbCrLf
        retuVal += "color:black; " + vbCrLf
        retuVal += "font-size:11.0pt; " + vbCrLf
        retuVal += "font-weight:400; " + vbCrLf
        retuVal += "font-style:normal; " + vbCrLf
        retuVal += "text-decoration:none; " + vbCrLf
        retuVal += "font-family:Calibri, sans-serif; " + vbCrLf
        retuVal += "mso-font-charset:0; " + vbCrLf
        retuVal += "mso-number-format:General; " + vbCrLf
        retuVal += "text-align:general; " + vbCrLf
        retuVal += "vertical-align:bottom; " + vbCrLf
        retuVal += "border:none; " + vbCrLf
        retuVal += "mso-background-source:auto; " + vbCrLf
        retuVal += "mso-pattern:auto; " + vbCrLf
        retuVal += "mso-protection:locked visible; " + vbCrLf
        retuVal += "white-space:nowrap; " + vbCrLf
        retuVal += "mso-rotate:0;} " + vbCrLf


        Return retuVal

    End Function
    Private Sub WriteCustomeStyleSheet(ByVal NewRepoFile As System.IO.StreamWriter)

        Dim hAlign As String = ""
        Dim vAlign As String = ""
        Dim _Alignment As RepoCell.AlignmentEnum
        Dim retuVal As String = ""

        NewRepoFile.WriteLine("<Style>")
        NewRepoFile.WriteLine(Me.GetStyleSheet())
        NewRepoFile.WriteLine("")

        For RowId As Integer = 0 To _dsStyleSheet.Rows.Count - 1

            retuVal = ""
            _Alignment = _dsStyleSheet.Rows(RowId)("Alignment")

            If _Alignment = RepoCell.AlignmentEnum.LeftTop Then
                hAlign = "left"
                vAlign = "top"
            ElseIf _Alignment = RepoCell.AlignmentEnum.LeftMiddle Then
                hAlign = "left"
                vAlign = "middle"
            ElseIf _Alignment = RepoCell.AlignmentEnum.LeftBottom Then
                hAlign = "left"
                vAlign = "bottom"
            ElseIf _Alignment = RepoCell.AlignmentEnum.CenterTop Then
                hAlign = "center"
                vAlign = "top"
            ElseIf _Alignment = RepoCell.AlignmentEnum.CenterMiddle Then
                hAlign = "center"
                vAlign = "middle"
            ElseIf _Alignment = RepoCell.AlignmentEnum.CenterBottom Then
                hAlign = "center"
                vAlign = "bottom"
            ElseIf _Alignment = RepoCell.AlignmentEnum.RightTop Then
                hAlign = "right"
                vAlign = "top"
            ElseIf _Alignment = RepoCell.AlignmentEnum.RightMiddle Then
                hAlign = "right"
                vAlign = "middle"
            ElseIf _Alignment = RepoCell.AlignmentEnum.RightBottom Then
                hAlign = "right"
                vAlign = "bottom"
            End If

            retuVal = "mso-style-parent:style0; " + vbCrLf
            retuVal += "mso-height-source:userset; " + vbCrLf
            retuVal += "height:" + _dsStyleSheet.Rows(RowId)("CellHeight").ToString() + "pt;" + vbCrLf

            If _dsStyleSheet.Rows(RowId)("CellWidth") <> 0 Then
                retuVal += "mso-width-source:userset;" + vbCrLf
                retuVal += "width:" + _dsStyleSheet.Rows(RowId)("CellWidth").ToString() + "in;" + vbCrLf
            End If

            retuVal += "vertical-align:" + vAlign + ";" + vbCrLf
            retuVal += "text-align:" + hAlign + ";" + vbCrLf
            retuVal += "font-family:" + _dsStyleSheet.Rows(RowId)("FontName") + ";" + vbCrLf
            retuVal += "font-size:" + _dsStyleSheet.Rows(RowId)("FontSize").ToString() + "pt;" + vbCrLf

            If _dsStyleSheet.Rows(RowId)("FontBold") Then
                retuVal += "font-weight:bold;" + vbCrLf
            End If

            If _dsStyleSheet.Rows(RowId)("FontItalic") Then
                retuVal += "font-style:italic;" + vbCrLf
            End If

            If _dsStyleSheet.Rows(RowId)("FontUnderLine") Then
                retuVal += "text-decoration:underline;" + vbCrLf
            End If

            If _dsStyleSheet.Rows(RowId)("FontColor") <> Drawing.Color.Black.ToString() Then
                retuVal += "color:" + _dsStyleSheet.Rows(RowId)("FontColorName") + ";" + vbCrLf
            End If

            If _dsStyleSheet.Rows(RowId)("BackColor") <> Drawing.Color.Transparent.ToString() Then
                retuVal += "background:" + _dsStyleSheet.Rows(RowId)("BackColorName") + ";" + vbCrLf
                retuVal += "mso-pattern:black none;" + vbCrLf
            End If

            If _dsStyleSheet.Rows(RowId)("LineTopStyle") <> CType(RepoLine.LineTypeEnum.NoLine, Integer) Then
                retuVal += "border-top-style:" + IIf(_dsStyleSheet.Rows(RowId)("LineTopStyle") = RepoLine.LineTypeEnum.DoubleLine, "Double", "solid") + ";" + vbCrLf
                retuVal += "border-top-width:" + _dsStyleSheet.Rows(RowId)("LineTopWidth").ToString() + "pt;" + vbCrLf
                retuVal += "border-top-color:" + _dsStyleSheet.Rows(RowId)("LineTopColorName") + ";" + vbCrLf
            End If

            If _dsStyleSheet.Rows(RowId)("LineLeftStyle") <> CType(RepoLine.LineTypeEnum.NoLine, Integer) Then
                retuVal += "border-left-style:" + IIf(_dsStyleSheet.Rows(RowId)("LineLeftStyle") = RepoLine.LineTypeEnum.DoubleLine, "Double", "solid") + ";" + vbCrLf
                retuVal += "border-left-width:" + _dsStyleSheet.Rows(RowId)("LineLeftWidth").ToString() + "pt;" + vbCrLf
                retuVal += "border-left-color:" + _dsStyleSheet.Rows(RowId)("LineLeftColorName") + ";" + vbCrLf
            End If

            If _dsStyleSheet.Rows(RowId)("LineBottomStyle") <> CType(RepoLine.LineTypeEnum.NoLine, Integer) Then
                retuVal += "border-bottom-style:" + IIf(_dsStyleSheet.Rows(RowId)("LineBottomStyle") = RepoLine.LineTypeEnum.DoubleLine, "Double", "solid") + ";" + vbCrLf
                retuVal += "border-bottom-width:" + _dsStyleSheet.Rows(RowId)("LineBottomWidth").ToString() + "pt;" + vbCrLf
                retuVal += "border-bottom-color:" + _dsStyleSheet.Rows(RowId)("LineBottomColorName") + ";" + vbCrLf
            End If

            If _dsStyleSheet.Rows(RowId)("LineRightStyle") <> CType(RepoLine.LineTypeEnum.NoLine, Integer) Then
                retuVal += "border-right-style:" + IIf(_dsStyleSheet.Rows(RowId)("LineRightStyle") = RepoLine.LineTypeEnum.DoubleLine, "Double", "solid") + ";" + vbCrLf
                retuVal += "border-right-width:" + _dsStyleSheet.Rows(RowId)("LineRightWidth").ToString() + "pt;" + vbCrLf
                retuVal += "border-right-color:" + _dsStyleSheet.Rows(RowId)("LineRightColorName") + ";" + vbCrLf
            End If

            retuVal += "mso-number-format:"
            retuVal += """" + "\@" + """" + ";" + vbCrLf
            retuVal = "." + _dsStyleSheet.Rows(RowId)("ClassName").ToString() + vbCrLf + _
                      "{" + retuVal + "}"

            NewRepoFile.WriteLine(retuVal)
            NewRepoFile.WriteLine("")

        Next RowId

        NewRepoFile.WriteLine("</Style>")

    End Sub
    Private Function GetStyleSheetClassName(ByVal rCell As RepoCell) As String

        Dim IsValid As Boolean
        Dim r As Data.DataRow
        Dim MaxNo1 As Integer

        For RowId As Integer = 0 To _dsStyleSheet.Rows.Count - 1

            IsValid = True
            MaxNo1 = _dsStyleSheet.Rows(RowId)("StyleNo")

            If _dsStyleSheet.Rows(RowId)("Alignment") <> CType(rCell.Alignment, Integer) Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("CellHeight") <> rCell.ActualCellHeight() Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("CellWidth") <> rCell.Width Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("FontName") <> rCell.FontName Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("FontSize") <> rCell.FontSize Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("FontBold") <> rCell.FontBold Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("FontItalic") <> rCell.FontItalic Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("FontUnderLine") <> rCell.FontUnderLine Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("FontColor") <> rCell.FontColor.ToString() Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("BackColor") <> rCell.BackgroundColor.ToString() Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("LineTopStyle") <> CType(rCell.LineTop.LineType, Integer) Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("LineTopWidth") <> rCell.LineTop.LineWidth Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("LineTopColor") <> rCell.LineTop.LineColor.ToString() Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("LineLeftStyle") <> CType(rCell.LineLeft.LineType, Integer) Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("LineLeftWidth") <> rCell.LineLeft.LineWidth Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("LineLeftColor") <> rCell.LineLeft.LineColor.ToString() Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("LineBottomStyle") <> CType(rCell.LineBottom.LineType, Integer) Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("LineBottomWidth") <> rCell.LineBottom.LineWidth Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("LineBottomColor") <> rCell.LineBottom.LineColor.ToString() Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("LineRightStyle") <> CType(rCell.LineRight.LineType, Integer) Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("LineRightWidth") <> rCell.LineRight.LineWidth Then
                IsValid = False
            End If

            If IsValid And _dsStyleSheet.Rows(RowId)("LineRightColor") <> rCell.LineRight.LineColor.ToString() Then
                IsValid = False
            End If

            If IsValid Then
                GetStyleSheetClassName = _dsStyleSheet.Rows(RowId)("ClassName")
                Exit Function
            End If

        Next RowId

        MaxNo1 = MaxNo1 + 1
        r = _dsStyleSheet.NewRow

        r("StyleNo") = MaxNo1
        r("ClassName") = "SSPL" + MaxNo1.ToString()
        r("Alignment") = CType(rCell.Alignment, Integer)
        r("CellHeight") = rCell.ActualCellHeight()
        r("CellWidth") = rCell.Width
        r("FontName") = rCell.FontName
        r("FontSize") = rCell.FontSize
        r("FontBold") = rCell.FontBold
        r("FontItalic") = rCell.FontItalic
        r("FontUnderLine") = rCell.FontUnderLine
        r("FontColor") = rCell.FontColor.ToString()
        r("FontColorName") = System.Drawing.ColorTranslator.ToHtml(rCell.FontColor)
        r("BackColor") = rCell.BackgroundColor.ToString()
        r("BackColorName") = System.Drawing.ColorTranslator.ToHtml(rCell.BackgroundColor)

        r("LineTopStyle") = CType(rCell.LineTop.LineType, Integer)
        r("LineTopWidth") = rCell.LineTop.LineWidth
        r("LineTopColor") = rCell.LineTop.LineColor.ToString()
        r("LineTopColorName") = System.Drawing.ColorTranslator.ToHtml(rCell.LineTop.LineColor)

        r("LineLeftStyle") = CType(rCell.LineLeft.LineType, Integer)
        r("LineLeftWidth") = rCell.LineLeft.LineWidth
        r("LineLeftColor") = rCell.LineLeft.LineColor.ToString()
        r("LineLeftColorName") = System.Drawing.ColorTranslator.ToHtml(rCell.LineLeft.LineColor)

        r("LineBottomStyle") = CType(rCell.LineBottom.LineType, Integer)
        r("LineBottomWidth") = rCell.LineBottom.LineWidth
        r("LineBottomColor") = rCell.LineBottom.LineColor.ToString()
        r("LineBottomColorName") = System.Drawing.ColorTranslator.ToHtml(rCell.LineBottom.LineColor)

        r("LineRightStyle") = CType(rCell.LineRight.LineType, Integer)
        r("LineRightWidth") = rCell.LineRight.LineWidth
        r("LineRightColor") = rCell.LineRight.LineColor.ToString()
        r("LineRightColorName") = System.Drawing.ColorTranslator.ToHtml(rCell.LineRight.LineColor)

        _dsStyleSheet.Rows.Add(r)

        GetStyleSheetClassName = r("ClassName")
    End Function
    Private Sub CopyContentToFile()
        Dim NewRepoFile As System.IO.StreamWriter
        Dim RepoFileRead As System.IO.StreamReader
        Dim NewFileName1 As String
        Dim FilePathArr() As String

        NewFileName1 = ""
        FilePathArr = Split(_ReportFileName, "\")

        For i As Integer = 0 To FilePathArr.Length - 2
            NewFileName1 = NewFileName1 + FilePathArr(i) + "\"
        Next i

        Try
            RepoFile.Flush()
            RepoFile.Close()
        Catch ex As Exception
            Throw New Exception("Error occured while closing report file for copy" + vbCrLf + ex.Message)
        End Try

        Try
            NewFileName1 = NewFileName1 + GetReportName()
            NewRepoFile = New System.IO.StreamWriter(NewFileName1, False)
        Catch ex As Exception
            Throw New Exception("Error occured while Opening new file for copy " + vbCrLf + ex.Message)
        End Try

        Try
            RepoFileRead = New System.IO.StreamReader(_ReportFileName, False)
        Catch ex As Exception
            Throw New Exception("Error occured while opening file for reading for copy content" + vbCrLf + ex.Message)
        End Try

        Try

            NewRepoFile.WriteLine("<Html>")
            NewRepoFile.WriteLine("<Head>")
            WriteCustomeStyleSheet(NewRepoFile)
            NewRepoFile.WriteLine("</Head>")
            NewRepoFile.WriteLine("<Body>")
            NewRepoFile.WriteLine("<Table " + _
                                  " border=0 cellpadding=0 cellspacing=0 " + _
                                  " style='border-collapse: collapse;table-layout:fixed;'>")

            NewRepoFile.WriteLine(RepoFileRead.ReadToEnd())

            NewRepoFile.WriteLine("</Table>")
            NewRepoFile.WriteLine("</Body>")



            NewRepoFile.WriteLine("</Html>")
            NewRepoFile.Flush()

        Catch ex As Exception
            Throw New Exception("Error occured while coping Report information" + vbCrLf + ex.Message)
        Finally
            NewRepoFile.Close()
            NewRepoFile = Nothing

            RepoFileRead.Close()
            RepoFileRead = Nothing
        End Try

        Try
            System.IO.File.Delete(_ReportFileName)
            System.IO.File.Move(NewFileName1, _ReportFileName)
        Catch ex As Exception
            Throw New Exception("Error occured while coping" + vbCrLf + ex.Message)
        End Try

    End Sub
#End Region
#Region "Public Method"
    Public Sub Say(ByVal rRow As RepoRow)

        If Not IsReportOpen Then
            Throw New Exception(ReportCloseError)
        End If

        For CellId As Integer = 0 To rRow.CellCount - 1
            rRow.Cell(CellId).ClassName = Me.GetStyleSheetClassName(rRow.Cell(CellId))
        Next CellId

        RepoFile.WriteLine(rRow.ToString())
        rRow.SetBlankValues2Cells()

    End Sub
    Public Sub SayBlankRow()

        If Not IsReportOpen Then
            Throw New Exception(ReportCloseError)
        End If

        Me.SayBlankRow(1)
    End Sub
    Public Sub SayBlankRow(ByVal NoOfRow_1 As Integer)

        If Not IsReportOpen Then
            Throw New Exception(ReportCloseError)
        End If

        For i As Integer = 1 To NoOfRow_1
            RepoFile.WriteLine("<Tr><Td>&nbsp</Td></Tr>")
        Next i

    End Sub
    Public Sub CloseReport()

        If IsReportOpen Then
            CopyContentToFile()
        End If

        IsReportOpen = False

    End Sub
    Protected Overrides Sub Finalize()
        Try
            RepoFile.Close()
        Catch ex As Exception
        Finally
            RepoFile = Nothing
        End Try
    End Sub
#End Region

End Class