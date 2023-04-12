Imports System.IO
Imports System.Text.RegularExpressions
'Namespace ReadFile
Public Class clsReadFile

    Public Sub New()

    End Sub
    Public Enum SepratedBy
        Comma = 1
        Tab = 2
        SemiColon = 3
    End Enum
    Private strSourcePath As String
    Private strDestPath As String
    Public Property SourcePath() As String
        Get
            Return SourcePath
        End Get
        Set(ByVal value As String)
            strSourcePath = value
        End Set
    End Property
    Public Property DestPath() As String
        Get
            Return DestPath
        End Get
        Set(ByVal value As String)
            strDestPath = value
        End Set
    End Property
    ''' <summary>
    ''' Choose this function to get datatable from Source File
    ''' </summary>
    ''' <param name="Delimiter">The Character by which elements of File seperated{as in Enumration}</param>
    ''' <param name="ReadStartLineNo">No of Lines before columns line started</param>
    ''' <returns>Data in tabular format After reading file</returns>
    ''' <remarks>Set a Source Path full path of file</remarks>
    Public Function GetDataTableFromSource(ByVal Delimiter As SepratedBy, Optional ByVal ReadStartLineNo As Integer = 1) As DataTable
        If Not (strSourcePath.Trim = Nothing Or strSourcePath.Trim = "") Then
            If Not File.Exists(strSourcePath.Trim) Then
                Throw New Exception("Source File not Found")
            End If

            Dim sr As New StreamReader(strSourcePath.Trim)
            GetDataTableFromSource = GetTableFromStream(sr, ReadStartLineNo, Delimiter)
            sr.Close()
        Else
            Throw New Exception("Source path is blank or Invalid")
        End If
    End Function
    'Public Overridable Function GetDataTableFromPDF(ByRef dtToOut As DataTable, ByVal strPDF As String) As Boolean
    '    'Dim reachedSampleID As Boolean = False
    '    Dim StrLines() As String
    '    StrLines = strPDF.Split(vbNewLine)

    '    For index As Integer = 0 To StrLines.Length - 1
    '        If StrLines(index).ToUpper.Contains("SAMPLE ID") Then

    '            For Each strSampleId As String In StrLines(index).Split(" ")
    '                If Not (strSampleId.ToUpper.Contains("SAMPLE") Or strSampleId.ToUpper.Contains("ID")) Then
    '                    Dim dr As DataRow = dtToOut.NewRow()
    '                    dr("Sample Id") = strSampleId
    '                    dtToOut.Rows.Add(dr)
    '                End If
    '            Next
    '            dtToOut.AcceptChanges()
    '        ElseIf StrLines(index).ToUpper.Contains("NG/ML") Then
    '            For indexSampleId As Integer = 0 To StrLines(index).Split(" ").Count - 1
    '                If Not StrLines(index).Split(" ")(indexSampleId).ToUpper.Contains("NG/ML") Then
    '                    dtToOut.Rows(indexSampleId)("Calculated Concentration") = StrLines(index).Split(" ")(indexSampleId - 1)
    '                End If

    '            Next
    '        End If

    '        dtToOut.AcceptChanges()
    '    Next



    '    Return True
    'End Function
    ''' <summary>
    ''' Only for Machine Waters and Analyst
    ''' </summary>
    ''' <param name="dtToOut">return output</param>
    ''' <param name="fileName">The Text File Converted from PDF for getting data </param>
    ''' <returns>Indication Of If Every thing goes right or not</returns>
    ''' <remarks>Made by Akhilesh</remarks>
    Public Function GetDataTableFromTextFileAnalyst(ByRef dtToOut As DataTable) As Boolean
        If Not (strSourcePath.Trim = Nothing Or strSourcePath.Trim = "") Then
            If Not File.Exists(strSourcePath.Trim) Then
                Throw New Exception("Source File not Found")
            End If

            Dim sr As New StreamReader(strSourcePath.Trim)
            'dtToOut = GetTableFromStreamForAnalystAndWaters(sr, SampleIdStaring, CalcConcStarting)
            If Not GetTableFromStreamForAnalyst(sr, dtToOut) Then
                sr.Close()
                Return False
            End If
            sr.Close()
            Return True
        Else
            Throw New Exception("Source path is blank or Invalid")
        End If
        Return True
    End Function
    Public Function GetDataTableFromTextFileThermo(ByRef dtToOut As DataTable) As Boolean
        If Not (strSourcePath.Trim = Nothing Or strSourcePath.Trim = "") Then
            If Not File.Exists(strSourcePath.Trim) Then
                Throw New Exception("Source File not Found")
            End If

            Dim sr As New StreamReader(strSourcePath.Trim)
            'dtToOut = GetTableFromStreamForAnalystAndWaters(sr, SampleIdStaring, CalcConcStarting)
            If Not GetTableFromStreamForThermo(sr, dtToOut) Then
                sr.Close()
                Return False
            End If
            sr.Close()
            Return True
        Else
            Throw New Exception("Source path is blank or Invalid")
        End If
        Return True
    End Function
    'Not Used
    Public Function GetDataTableFromTextFileWatersAndShimadzu(ByRef dtToOut As DataTable) As Boolean
        If Not (strSourcePath.Trim = Nothing Or strSourcePath.Trim = "") Then
            If Not File.Exists(strSourcePath.Trim) Then
                Throw New Exception("Source File not Found")
            End If

            Dim sr As New StreamReader(strSourcePath.Trim)
            'dtToOut = GetTableFromStreamForAnalystAndWaters(sr, SampleIdStaring, CalcConcStarting)
            If Not GetTableFromStreamWatersAndShimadzu(sr, dtToOut) Then
                sr.Close()
                Return False

            End If
            sr.Close()
            Return True
        Else
            Throw New Exception("Source path is blank or Invalid")
        End If
        Return True
    End Function
    'getGetDataTableFromTextShedule
    'Not Used
    Public Function getGetDataTableFromTextShedule(ByRef dtToOut As DataTable) As Boolean
        If Not (strSourcePath.Trim = Nothing Or strSourcePath.Trim = "") Then
            If Not File.Exists(strSourcePath.Trim) Then
                Throw New Exception("Source File not Found")
            End If

            Dim sr As New StreamReader(strSourcePath.Trim)
            'dtToOut = GetTableFromStreamForAnalystAndWaters(sr, SampleIdStaring, CalcConcStarting)
            If Not GetTableFromStreamShedule(sr, dtToOut) Then
                sr.Close()
                Return False

            End If
            sr.Close()
            Return True
        Else
            Throw New Exception("Source path is blank or Invalid")
        End If
        Return True
    End Function
    'Not Used
    Public Function GetTableFromStream(ByVal StreamToRead As StreamReader, ByVal lineNo As Integer, ByVal seperater As SepratedBy) As DataTable
        Dim dtResultTable As New Data.DataTable
        Dim currLine() As String
        Dim ColumnsRead As Boolean = False
        Dim strTemp As String = ""
        If lineNo > 1 Then
            For index As Integer = 1 To lineNo
                strTemp = StreamToRead.ReadLine()
            Next
        Else
            For index As Integer = 1 To StreamToRead.Peek
                If Not strTemp.ToUpper.Contains("SAMPLE ID") Then
                    strTemp = StreamToRead.ReadLine()
                Else
                    Exit For
                End If
            Next
        End If

        Do While StreamToRead.Peek >= 0
            'strTemp = StreamToRead.ReadLine()
            If seperater = SepratedBy.Comma Then
                currLine = strTemp.Trim.Split(",")
            ElseIf seperater = SepratedBy.Tab Then
                currLine = strTemp.Split(vbTab)
            ElseIf seperater = SepratedBy.SemiColon Then
                currLine = strTemp.Split(";")
            End If

            'For Column Names
            If Not ColumnsRead Then
                For Each Str As String In currLine
                    dtResultTable.Columns.Add(Str, GetType(String))
                Next
                ColumnsRead = True
                dtResultTable.AcceptChanges()
            Else 'For Rows
                If Not (currLine.Length < dtResultTable.Columns.Count - 2) Then
                    Dim drNew As DataRow = dtResultTable.NewRow()
                    For ClmIndex As Integer = 0 To dtResultTable.Columns.Count - 2
                        drNew(ClmIndex) = currLine(ClmIndex)
                    Next
                    dtResultTable.Rows.Add(drNew)
                    dtResultTable.AcceptChanges()
                End If

            End If
            strTemp = StreamToRead.ReadLine()
        Loop

        'Added by nipun khant (2014-01-24 14:11)
        'Reason : for added last row into datatable
        If StreamToRead.Peek() = -1 And strTemp <> Nothing Then
            If seperater = SepratedBy.Comma Then
                currLine = strTemp.Trim.Split(",")
            ElseIf seperater = SepratedBy.Tab Then
                currLine = strTemp.Split(vbTab)
            ElseIf seperater = SepratedBy.SemiColon Then
                currLine = strTemp.Split(";")
            End If
            If Not (currLine.Length < dtResultTable.Columns.Count - 2) Then
                Dim drNew As DataRow = dtResultTable.NewRow()
                For ClmIndex As Integer = 0 To dtResultTable.Columns.Count - 2
                    drNew(ClmIndex) = currLine(ClmIndex)
                Next
                dtResultTable.Rows.Add(drNew)
                dtResultTable.AcceptChanges()
            End If
        End If
        Return dtResultTable
    End Function
    'Not used
    Public Function GetTableFromStreamForAnalystAndWaters(ByRef StreamToRead As StreamReader, ByVal SampleIdStaring As Integer, ByVal CalcConcStarting As Integer) As DataTable
        Dim dtResultTable As New Data.DataTable
        dtResultTable.Columns.Add("Sample ID", GetType(String))
        dtResultTable.Columns.Add("NG/ML", GetType(String))
        dtResultTable.AcceptChanges()
        Dim ColumnsRead As Boolean = False
        Dim strTemp As String = ""
        strTemp = StreamToRead.ReadLine()

        For index As Integer = 1 To StreamToRead.Peek
            If Not strTemp.ToUpper.Contains("SAMPLE ID") Then
                strTemp = StreamToRead.ReadLine()
            Else
                Exit For
            End If
        Next



        Do While StreamToRead.Peek >= 0
            strTemp = StreamToRead.ReadLine()
            If strTemp.Trim.IndexOf(" ") > 0 AndAlso IsNumeric(strTemp.Trim.Substring(0, strTemp.Trim.IndexOf(" "))) Then 'Ensure that this row is a valid data row having a valid row no at starting
                If Not strTemp.Length < SampleIdStaring Then
                    Dim drResult As DataRow = dtResultTable.NewRow() 'The logic is to search for sample id from starting point defined and capture the string till first three white space found
                    Dim strSampleIdTemp As String = strTemp.Substring(SampleIdStaring, strTemp.IndexOf("   ", SampleIdStaring + 3) - SampleIdStaring)


                    drResult("Sample Id") = strSampleIdTemp.Trim()
                    If Not strTemp.Length < CalcConcStarting Then
                        Dim strCalcConcTemp As String = strTemp.Substring(CalcConcStarting, strTemp.IndexOf("  ", CalcConcStarting + 2) - CalcConcStarting)
                        drResult("NG/ML") = strCalcConcTemp.Trim()
                    Else
                        drResult("NG/ML") = DBNull.Value
                    End If
                    dtResultTable.Rows.Add(drResult)
                    dtResultTable.AcceptChanges()
                End If
            End If



        Loop
        Return dtResultTable
    End Function
    Public Function GetTableFromStreamForAnalyst(ByRef StreamToRead As StreamReader, ByRef dt As DataTable) As Boolean
        Try
            Dim dtResultTable As New Data.DataTable
            dtResultTable.Columns.Add("vSampleID", GetType(String))
            dtResultTable.Columns.Add("vResultValue", GetType(String))
            dtResultTable.Columns.Add("vSampleName", GetType(String))
            dtResultTable.Columns.Add("vAnalyteName", GetType(String))
            dtResultTable.AcceptChanges()
            Dim ColumnsRead As Boolean = False
            Dim strTemp As String = ""
            strTemp = StreamToRead.ReadLine()
            For index As Integer = 1 To StreamToRead.Peek
                If Not strTemp.ToUpper.Trim.Replace(" ", "").Contains("SAMPLEID") Then
                    strTemp = StreamToRead.ReadLine()
                Else
                    Exit For
                End If
            Next
            Do While StreamToRead.Peek >= 0
                strTemp = StreamToRead.ReadLine()
                If (strTemp.Trim.IndexOf(" ") > 0 AndAlso IsNumeric(strTemp.Trim.Substring(0, strTemp.Trim.IndexOf(" ")))) Or strTemp.ToUpper.Contains("METHOD") Then 'Ensure that this row is a valid data row having a valid row no at starting
                    Dim drResult As DataRow = dtResultTable.NewRow()
                    'The logic is to search for sample id from starting point defined and capture the string till first three white space found
                    Dim strSampleIdTemp() As String = Regex.Split(strTemp.Trim, "   ")
                    If IsNumeric(strSampleIdTemp(0)) Then 'Assumption 1 -->analyst must have numerical starting  2-->Thermo must have "method" in the valid line
                        Dim index As Integer = 1
                        While (drResult("vSampleID") Is DBNull.Value) Or (drResult("vResultValue") Is DBNull.Value) Or (drResult("vSampleName") Is DBNull.Value) Or (drResult("vAnalyteName") Is DBNull.Value)
                            If strSampleIdTemp(index).Trim() <> "" Then
                                If drResult("vSampleID") Is DBNull.Value Then
                                    drResult("vSampleID") = strSampleIdTemp(index).Trim
                                ElseIf drResult("vResultValue") Is DBNull.Value Then
                                    drResult("vResultValue") = strSampleIdTemp(index).Trim
                                ElseIf drResult("vAnalyteName") Is DBNull.Value Then
                                    drResult("vAnalyteName") = strSampleIdTemp(index).Trim
                                ElseIf drResult("vSampleName") Is DBNull.Value Then
                                    drResult("vSampleName") = strSampleIdTemp(index).Trim
                                    Exit While
                                End If
                            End If
                            index = index + 1
                        End While
                    End If
                    If Not (drResult("vSampleID") Is DBNull.Value Or drResult("vSampleID").ToString.Trim = "") AndAlso Not (drResult("vResultValue") Is DBNull.Value Or drResult("vResultValue").ToString.Trim = "") Then
                        dtResultTable.Rows.Add(drResult)
                    End If
                    dtResultTable.AcceptChanges()
                End If
            Loop
            dt = dtResultTable
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GetTableFromStreamForThermo(ByRef StreamToRead As StreamReader, ByRef dt As DataTable) As Boolean
        Try
            Dim dtResultTable As New Data.DataTable
            dtResultTable.Columns.Add("vSampleID", GetType(String))
            dtResultTable.Columns.Add("vResultValue", GetType(String))
            dtResultTable.Columns.Add("vSampleName", GetType(String))
            dtResultTable.Columns.Add("vAnalyteName", GetType(String))
            dtResultTable.AcceptChanges()
            Dim ColumnsRead As Boolean = False
            Dim strTemp As String = ""
            strTemp = StreamToRead.ReadLine()

            For index As Integer = 1 To StreamToRead.Peek
                If Not strTemp.ToUpper.Trim.Replace(" ", "").Contains("SAMPLEID") Then
                    strTemp = StreamToRead.ReadLine()
                Else
                    Exit For
                End If
            Next
            Do While StreamToRead.Peek >= 0
                strTemp = StreamToRead.ReadLine()
                If (strTemp.Trim.IndexOf(" ") > 0 AndAlso IsNumeric(strTemp.Trim.Substring(0, strTemp.Trim.IndexOf(" ")))) Or strTemp.ToUpper.Contains("METHOD") Then 'Ensure that this row is a valid data row having a valid row no at starting
                    Dim drResult As DataRow = dtResultTable.NewRow()
                    'The logic is to search for sample id from starting point defined and capture the string till first three white space found
                    Dim strSampleIdTemp() As String = Regex.Split(strTemp.Trim, "   ")
                    If strTemp.ToUpper.Contains("METHOD") And (Not strTemp.ToUpper.Trim.Replace(" ", "").Contains("INSTRUMENTMETHOD:")) Then 'Assumption 1 -->analyst must have numerical starting  2-->Thermo must have "method" in the valid line
                        Dim index As Integer = 0
                        While (drResult("vSampleID") Is DBNull.Value) Or (drResult("vResultValue") Is DBNull.Value) Or (drResult("vSampleName") Is DBNull.Value) Or (drResult("vAnalyteName") Is DBNull.Value)
                            If strSampleIdTemp(index).Trim() <> "" Then
                                If drResult("vSampleID") Is DBNull.Value Then
                                    drResult("vSampleID") = strSampleIdTemp(index).Trim
                                ElseIf drResult("vResultValue") Is DBNull.Value Then
                                    drResult("vResultValue") = strSampleIdTemp(index).Trim
                                ElseIf drResult("vAnalyteName") Is DBNull.Value Then
                                    drResult("vAnalyteName") = strSampleIdTemp(index).Trim
                                ElseIf drResult("vSampleName") Is DBNull.Value Then
                                    drResult("vSampleName") = strSampleIdTemp(index).Trim
                                    Exit While
                                End If

                            End If
                            index = index + 1
                        End While
                    End If
                    If Not (drResult("vSampleID") Is DBNull.Value Or drResult("vSampleID").ToString.Trim = "") AndAlso Not (drResult("vResultValue") Is DBNull.Value Or drResult("vResultValue").ToString.Trim = "") Then
                        dtResultTable.Rows.Add(drResult)
                    End If

                    dtResultTable.AcceptChanges()

                End If
            Loop
            dt = dtResultTable
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    'Not Used
    Public Function GetTableFromStreamWatersAndShimadzu(ByRef StreamToRead As StreamReader, ByRef dt As DataTable) As Boolean 'Configured only for waters for the sequence of Sample ID,SampleName,Calculated Concentration.
        Try
            Dim dtResultTable As New Data.DataTable
            Dim strCurrentAnalyte As String = String.Empty
            dtResultTable.Columns.Add("vSampleID", GetType(String))
            dtResultTable.Columns.Add("vResultValue", GetType(String))
            dtResultTable.Columns.Add("vSampleName", GetType(String))
            dtResultTable.Columns.Add("vAnalyteName", GetType(String))
            dtResultTable.AcceptChanges()
            Dim ColumnsRead As Boolean = False
            Dim strTemp As String = ""
            strTemp = StreamToRead.ReadLine()

            'For index As Integer = 1 To StreamToRead.Peek

            '    If Not strTemp.Trim.ToUpper.Contains("COMPOUND NAME") Then

            '        'End If
            '        'If Not strTemp.ToUpper.Contains("SAMPLE ID") Then
            '        '    strTemp = StreamToRead.ReadLine()
            '        strTemp = StreamToRead.ReadLine()
            '    Else
            '        strTemp = strTemp.Trim.Substring(strTemp.Trim.ToUpper.IndexOf("COMPOUND NAME:") + 14) ' here  14 is the lenght of Compound name itself which lso need to remove.
            '        strCurrentAnalyte = strTemp.Trim
            '        Exit For
            '    End If
            'Next
            Do While StreamToRead.Peek >= 0
                strTemp = StreamToRead.ReadLine()
                If strTemp.Trim.ToUpper.Contains("COMPOUND NAME") Then
                    strTemp = strTemp.Trim.Substring(strTemp.Trim.ToUpper.LastIndexOf("COMPOUND NAME:") + 14)
                    strCurrentAnalyte = strTemp.Trim
                End If
                If (strTemp.Trim.IndexOf(" ") > 0 AndAlso IsNumeric(strTemp.Trim.Substring(0, strTemp.Trim.IndexOf(" ")))) Or IsNumeric(Regex.Split(strTemp, "   ")(Regex.Split(strTemp, "   ").Length - 1).Trim) Then 'Ensure that this row is a valid data row having a valid row no at starting
                    Dim drResult As DataRow = dtResultTable.NewRow()                                                                'Added this condition for shimadzu which must have last numerical value for a valid row
                    'The logic is to search for sample id from starting point defined and capture the string till first three white space found
                    Dim strSampleIdTemp() As String = Regex.Split(strTemp.Trim, "   ")

                    drResult("vAnalyteName") = strCurrentAnalyte
                    'End If
                    If IsNumeric(strSampleIdTemp(0)) Or IsNumeric(Regex.Split(strTemp, "   ")(Regex.Split(strTemp, "   ").Length - 1).Trim) Then 'Added this condition for shimadzu which must have last numerical value for a valid row

                        Dim index As Integer = 1
                        While (drResult("vSampleID") Is DBNull.Value) Or (drResult("vResultValue") Is DBNull.Value) Or (drResult("vSampleName") Is DBNull.Value)
                            If strSampleIdTemp(index).Trim() <> "" Then
                                If drResult("vSampleID") Is DBNull.Value Then
                                    drResult("vSampleID") = strSampleIdTemp(index).Trim
                                ElseIf drResult("vSampleName") Is DBNull.Value Then
                                    drResult("vSampleName") = strSampleIdTemp(index).Trim
                                ElseIf drResult("vResultValue") Is DBNull.Value Then
                                    'If IsNumeric(strSampleIdTemp(index).Trim) Then
                                    drResult("vResultValue") = strSampleIdTemp(index).Trim
                                    'Else
                                    '    drResult("vResultValue") = "0"
                                    'End If
                                    Exit While
                                End If
                            End If
                            index = index + 1
                        End While
                    End If
                    If Not (drResult("vSampleID") Is DBNull.Value Or drResult("vSampleID").ToString.Trim = "") AndAlso Not (drResult("vResultValue") Is DBNull.Value Or drResult("vResultValue").ToString.Trim = "") Then
                        dtResultTable.Rows.Add(drResult)
                    End If

                    dtResultTable.AcceptChanges()

                End If
            Loop
            dt = dtResultTable
            Return True


        Catch ex As Exception
            Return False
        End Try
    End Function
    'GetTableFromStreamShedule
    'Only for Processing Sequence schedule
    Public Function GetTableFromStreamShedule(ByRef StreamToRead As StreamReader, ByRef dt As DataTable) As Boolean
        Try
            Dim dtResultTable As New Data.DataTable
            dtResultTable.Columns.Add("vSampleID", GetType(String))
            dtResultTable.Columns.Add("vResultValue", GetType(String))
            dtResultTable.Columns.Add("vSampleName", GetType(String))
            dtResultTable.Columns.Add("vAnalyteName", GetType(String))
            dtResultTable.AcceptChanges()
            Dim ColumnsRead As Boolean = False
            Dim strTemp As String = ""
            strTemp = StreamToRead.ReadLine()

            For index As Integer = 1 To StreamToRead.Peek
                If Not strTemp.ToUpper.Replace(" ", "").Contains("SAMPLEID") Then
                    strTemp = StreamToRead.ReadLine()
                Else
                    Exit For
                End If
            Next
            Do While StreamToRead.Peek >= 0
                strTemp = StreamToRead.ReadLine()
                If strTemp.Trim.ToUpper.Replace(" ", "").Contains("TAB:QUANT") Then 'specially for analyst machine
                    Exit Do
                End If
                If (strTemp.Trim.IndexOf(" ") > 0 AndAlso IsNumeric(strTemp.Trim.Substring(0, strTemp.Trim.IndexOf(" ")))) Or strTemp.ToUpper.Contains("METHOD") Then 'Ensure that this row is a valid data row having a valid row no at starting
                    Dim drResult As DataRow = dtResultTable.NewRow()
                    'The logic is to search for sample id from starting point defined and capture the string till first three white space found
                    Dim strSampleIdTemp() As String = Regex.Split(strTemp.Trim, "   ")
                    If IsNumeric(strSampleIdTemp(0)) Then 'Assumption 1 -->analyst must have numerical starting  2-->Thermo must have "method" in the valid line

                        Dim index As Integer = 1
                        While (drResult("vSampleID") Is DBNull.Value) Or (drResult("vSampleName") Is DBNull.Value)
                            If strSampleIdTemp(index).Trim() <> "" Then
                                If drResult("vSampleName") Is DBNull.Value Then
                                    drResult("vSampleName") = strSampleIdTemp(index).Trim
                                ElseIf drResult("vSampleID") Is DBNull.Value Then
                                    drResult("vSampleID") = strSampleIdTemp(index).Trim
                                    Exit While
                                End If

                            End If
                            index = index + 1
                        End While
                    End If
                    If Not (drResult("vSampleID") Is DBNull.Value Or drResult("vSampleID").ToString.Trim = "") AndAlso Not (drResult("vSampleName") Is DBNull.Value Or drResult("vSampleName").ToString.Trim = "") Then
                        dtResultTable.Rows.Add(drResult)
                    End If

                    dtResultTable.AcceptChanges()

                End If
            Loop
            dt = dtResultTable
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
'End Namespace
