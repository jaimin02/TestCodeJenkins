Imports Microsoft.VisualBasic

Public Class ClsFolderPath
#Region "Folder path"
    Public Overloads ReadOnly Property Param_FolderPath() As String
        Get
            Try
                Param_FolderPath = HttpContext.Current.Server.MapPath("XMLData/" & Trim(HttpContext.Current.Session("PositionNo")))
            Catch ex As FileNotFoundException
                Throw New Exception("Your MSL are not available,Please Add MSL")
            End Try
        End Get
    End Property
    Public Overloads ReadOnly Property Param_FolderPath(ByVal SubjectDetail As String, _
                                                        ByVal WsId As String, _
                                                        ByVal DCId As String, _
                                                        ByVal SubId As String, _
                                                        ByVal NId As String, _
                                                        ByVal TNo As String) As String
        Get
            'Dim SubjectDetail As String = System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail")
            Try
                Param_FolderPath = HttpContext.Current.Server.MapPath(SubjectDetail & WsId & "/" & DCId & "/" & SubId & "/" & NId & "/" & TNo)
            Catch ex As FileNotFoundException
                Throw New Exception("Error While Create Folder")
            End Try
        End Get
    End Property

    Public Overloads ReadOnly Property Param_FolderPath(ByVal Path As String) As String
        Get
            Try
                Param_FolderPath = HttpContext.Current.Server.MapPath(Path)
            Catch ex As FileNotFoundException
                Throw New Exception("Error While Create Folder")
            End Try
        End Get
    End Property

#End Region

#Region "UploadFile"

    Public Function UploadFile(ByVal FileByte As Byte(), _
                               ByVal Path As String, _
                               ByVal FileName As String) As String


        Dim ms As New System.IO.MemoryStream(FileByte)
        Dim fs As System.IO.FileStream
        Dim Dir As DirectoryInfo

        Try
            '---Create folder and save file on server------
            Path = HttpContext.Current.Server.MapPath(Path)

            Dir = New DirectoryInfo(Path)
            If Not Dir.Exists() Then
                Dir.Create()
            End If
            Path += "\" + FileName

            fs = New System.IO.FileStream(Path, IO.FileMode.OpenOrCreate)
            ms.WriteTo(fs)
            ms.Close()
            fs.Close()
            fs.Dispose()

        Catch ex As Exception
            Throw New Exception("Error While Create Folder")
        End Try

    End Function

#End Region

#Region "UploadFileForSubjects"

    Public Function UploadFileForSubjects(ByVal FileByte As Byte(), _
                               ByVal Path As String, _
                               ByVal FileName As String) As String


        Dim ms As New System.IO.MemoryStream(FileByte)
        Dim fs As System.IO.FileStream
        Dim Dir As DirectoryInfo

        Try
            '---Create folder and save file on server------
            Path = HttpContext.Current.Server.MapPath(Path)

            Dir = New DirectoryInfo(Path)
            If Not Dir.Exists() Then
                Dir.Create()
            End If
            Path += "\" + FileName

            fs = New System.IO.FileStream(Path, IO.FileMode.OpenOrCreate)
            ms.WriteTo(fs)
            ms.Close()
            fs.Close()
            fs.Dispose()

        Catch ex As Exception
            Throw New Exception("Error While Create Folder")
        End Try

    End Function

#End Region

#Region "ReadFile"

    Public Function ReadFile(ByRef FileByte As Byte(), _
                               ByVal Path As String) As String


        Try
            '---Create folder and save file on server------
            Dim binReader As New BinaryReader(File.Open(HttpContext.Current.Server.MapPath(Path), FileMode.Open, FileAccess.Read))
            binReader.BaseStream.Position = 0
            Path = HttpContext.Current.Server.MapPath(Path)
            FileByte = BinReader.ReadBytes(Convert.ToInt32(BinReader.BaseStream.Length))
            BinReader.Close()

        Catch ex As Exception
            Throw New Exception("Error While Access File")
        End Try

    End Function

#End Region

End Class
