Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<System.Web.Script.Services.ScriptService()> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class AutoComplete
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetProjectCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String

        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 31-Aug-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        'contextKey = "nScopeNo=" & System.Web.HttpContext.Current.Session(S_ScopeNo)
        item = DBHelp.GetProjectCompletionList(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function view_ArchiveProjectList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String

        Dim Wstr_ScopeValue As String = ""

        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        item = DBHelp.view_ArchiveProjectList(prefixText, count, contextKey)
        Return item
    End Function

    <WebMethod(EnableSession:=True)> _
   <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetCrfVersionProjectList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String

        Dim Wstr_ScopeValue As String = ""

        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        item = DBHelp.GetCrfVersionProjectList(prefixText, count, contextKey)
        Return item
    End Function

    <WebMethod(EnableSession:=True)> _
 <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetProjectForTrainingGuideline(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String

        Dim Wstr_ScopeValue As String = ""

        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        item = DBHelp.GetProjectForTrainingGuideline(prefixText, count, contextKey)

        Return item
    End Function

    <WebMethod(EnableSession:=True)> _
<System.Web.Script.Services.ScriptMethod()> _
Public Function GetParentProjectCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 31-Aug-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        item = DBHelp.GetParentProjectCompletionList(prefixText, count, contextKey)
        Return item

    End Function


    <WebMethod(EnableSession:=True)> _
<System.Web.Script.Services.ScriptMethod()> _
Public Function GetProjectCompletionListWithOutSponser(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String

        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 31-Aug-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        item = DBHelp.GetProjectCompletionListWithOutSponser(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetActivityCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        contextKey = Wstr_ScopeValue
        'contextKey = "nScopeNo=" & System.Web.HttpContext.Current.Session(S_ScopeNo)
        item = DBHelp.GetActivityCompletionList(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod()> _
   <System.Web.Script.Services.ScriptMethod()> _
   Public Function GetSubjectCompletionList_NotRejected(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetSubjectCompletionList_NotRejected(prefixText, count, contextKey)
        Return item

    End Function

    'Created By Chandresh Vanker on 10-Feb-2010 For getting only assigned subjects
    <WebMethod()> _
   <System.Web.Script.Services.ScriptMethod()> _
   Public Function GetSubjectCompletionList_Assigned_NotRejected(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetSubjectCompletionList_Assigned_NotRejected(prefixText, count, contextKey)
        Return item

    End Function
    '****************************************

    <WebMethod()> _
   <System.Web.Script.Services.ScriptMethod()> _
   Public Function GetAllSubjectCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetAllSubjectCompletionList(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetTemplateCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'contextKey = Wstr_ScopeValue
        'Added by Pratiksha Date : 25-Feb-2011
        'Reason : If contexkey is allready there it is concatinate to where condition
        contextKey += Wstr_ScopeValue

        item = DBHelp.GetTemplateCompletionList(prefixText, count, contextKey)
        Return item
    End Function
    'Added By Naimesh Dave
    <WebMethod()> _
      <System.Web.Script.Services.ScriptMethod()> _
      Public Function GetDrugCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetDrugCompletionList(prefixText, count, contextKey)
        Return item

    End Function

    'Added By Naimesh Dave
    <WebMethod()> _
      <System.Web.Script.Services.ScriptMethod()> _
      Public Function GetSubjectCompletionList_Dynamic(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetSubjectCompletionList_Dynamic(prefixText, count, contextKey)
        Return item

    End Function

    'Added By Naimesh Dave
    <WebMethod(EnableSession:=True)> _
      <System.Web.Script.Services.ScriptMethod()> _
      Public Function GetMedexList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If
        contextKey = Wstr_ScopeValue

        item = DBHelp.GetMedexList(prefixText, count, contextKey)
        Return item

    End Function
    'Added By Satyam 28-May-2009
    <WebMethod()> _
    Public Function UpLoadFile(ByVal FileByte As Byte(), _
                               ByVal Path As String, _
                               ByVal FileName As String) As String

        Dim objFolderPath As ClsFolderPath = Nothing

        objFolderPath = New ClsFolderPath
        UpLoadFile = objFolderPath.UploadFile(FileByte, Path, FileName)
    End Function

    <WebMethod()> _
    Public Function UpLoadFileForSubjects(ByVal FileByte As Byte(), _
                               ByVal Path As String, _
                               ByVal FileName As String) As String

        Dim objFolderPath As ClsFolderPath = Nothing

        objFolderPath = New ClsFolderPath
        UpLoadFileForSubjects = objFolderPath.UploadFileForSubjects(FileByte, Path, FileName)
    End Function 'Added by Chandresh Vanker on 24-08-2009

    'Added By Satyam 29-May-2009
    <WebMethod()> _
    Public Function ReadFile(ByRef FileByte As Byte(), _
                             ByVal Path As String) As String

        Dim objFolderPath As ClsFolderPath = Nothing

        objFolderPath = New ClsFolderPath
        ReadFile = objFolderPath.ReadFile(FileByte, Path)
    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetMyProjectCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String

        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        item = DBHelp.GetMyProjectCompletionList(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)> _
   <System.Web.Script.Services.ScriptMethod()> _
   Public Function GetMyProjectCompletionListForArchive(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        item = DBHelp.GetMyProjectCompletionListForArchive(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetOldProjectsList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()


        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String

        If IsNothing(contextKey) Then
            contextKey = ""
        End If
        '*****************************************

        item = DBHelp.GetOldProjectsList(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetMyProjectCompletionListForProjectTrack(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        item = DBHelp.GetMyProjectCompletionListForProjectTrack(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetMyProjectCompletionListForDashboard(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String

        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        item = DBHelp.GetMyProjectCompletionListForDashboard(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetClientRequestProjectCompletionListForDashboard(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        item = DBHelp.GetClientRequestProjectCompletionListForDashboard(prefixText, count, contextKey)
        Return item

    End Function

    '' Added By Pratiksha Date : 25-Feb-2011
    ''Purpose: For  In House Subject 
    <WebMethod()> _
   <System.Web.Script.Services.ScriptMethod()> _
   Public Function GetSubjectCompletionList_NotRejected_InHouse(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetSubjectCompletionList_NotRejected_InHouse(prefixText, count, contextKey)
        Return item

    End Function

    '' Added By Pratiksha Date : 15-Mar-2011
    ''Purpose: For Attribute Group 
    <WebMethod()> _
   <System.Web.Script.Services.ScriptMethod()> _
   Public Function GetAttributeGroup(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetAttributeGroup(prefixText, count, contextKey)
        Return item

    End Function

    '' Added By Pratiksha Date : 16-Mar-2011
    ''Purpose: For Attribute sub Group 
    <WebMethod()> _
   <System.Web.Script.Services.ScriptMethod()> _
   Public Function GetAttributeSubGroup(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetAttributeSubGroup(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetAllProjectList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String

        item = DBHelp.GetAllProjectList(prefixText, count, contextKey)
        Return item

    End Function

    ''Added By Mrunal Parekh Date : 10-Nov-2011
    ''Purpose: For Project Name String change

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetMyProjectCompletionListwithworkspacedesc(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String

        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        item = DBHelp.GetMyProjectCompletionListwithworkspacedesc(prefixText, count, contextKey)
        Return item

    End Function
    <WebMethod(EnableSession:=True)> _
 <System.Web.Script.Services.ScriptMethod()> _
 Public Function GetMyProjectCompletionListForProjectSpScr(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String

        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        item = DBHelp.GetMyProjectCompletionListForProjectSpScr(prefixText, count, contextKey)
        Return item

    End Function
    'Added by akhilesh for showing BA child Project Instead of  parent Project
    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetBASubProjects(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue + " AND cStatusIndi <>'D' "
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        item = DBHelp.GetBASubProjects(prefixText, count, contextKey)
        Return item

    End Function


    'Added by Vimal Ghoniya for Listing of CDMS Subject
    <WebMethod()> _
   <System.Web.Script.Services.ScriptMethod()> _
   Public Function GetCDMSSubjectCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()


        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetCDMSSubjectCompletionList(prefixText, count, contextKey)
        Return item

    End Function


    <WebMethod()> _
  <System.Web.Script.Services.ScriptMethod()> _
  Public Function GetCDMSSubjectCompletionListActive(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetCDMSSubjectCompletionListActive(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetMyProjectCompletionListParentOnly(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        Dim Wstr_ScopeValue As String = ""

        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If


        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If

        item = DBHelp.GetMyProjectCompletionListParentOnly(prefixText, count, contextKey)
        Return item

    End Function

    'Added By Pratik Soni On 28-Apr-2014 to allow search only by SubjectId
    <WebMethod()> _
<System.Web.Script.Services.ScriptMethod()> _
    Public Function GetSubjectCompletionList_NotRejected_OnlyID(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()


        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetSubjectCompletionList_NotRejected_OnlyId(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod()> _
<System.Web.Script.Services.ScriptMethod()> _
    Public Function GetProjectCompletionListForDMS(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetProjectCompletionListForDMS(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)> _
        <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetMyProjectCompletionListwithoutInitial(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        contextKey += " AND cProjectStatus <> 'I' "

        item = DBHelp.GetMyProjectCompletionList(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetMyProjectCompletionListwithInitial(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        contextKey += " AND cProjectStatus = 'I' "

        item = DBHelp.GetMyProjectCompletionList(prefixText, count, contextKey)
        Return item

    End Function

    '' added by prayag for rejection module changes
    <WebMethod()> _
   <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetSubjectCompletionList_NotRejected_BlockPeriod(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetSubjectCompletionList_NotRejected_BlockPeriod(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod()> _
   <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetSubjectCompletionList_NotRejected_InHouse_BlockPeriod(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetSubjectCompletionList_NotRejected_InHouse_BlockPeriod(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod()> _
<System.Web.Script.Services.ScriptMethod()> _
    Public Function GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriod(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriod(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod()> _
       <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetSubjectCompletionList_NotRejectedDataMerg(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetSubjectCompletionList_NotRejectedDataMerg(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod()> _
       <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetSubjectCompletionList_NotRejected_BlockPeriodDataMerg(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetSubjectCompletionList_NotRejected_BlockPeriodDataMerg(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod()> _
       <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriodDataMerg(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriodDataMerg(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod()> _
       <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetSubjectCompletionList_NotRejected_OnlyIDDataMerg(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetSubjectCompletionList_NotRejected_OnlyIDDataMerg(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod()> _
     <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetOperationName(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        item = DBHelp.GetOperationName(prefixText, count, contextKey)
        Return item

    End Function

    <WebMethod(EnableSession:=True)>
    <System.Web.Script.Services.ScriptMethod()>
    Public Function GetTemplateCompletionListPreviewAttribute(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String
        Dim Wstr_ScopeValue As String = ""
        Dim Temptaletype As String = ""

        Temptaletype = "and cTemplateType in ('O','U')"

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'contextKey = Wstr_ScopeValue
        'Added by Pratiksha Date : 25-Feb-2011
        'Reason : If contexkey is allready there it is concatinate to where condition
        contextKey += Wstr_ScopeValue
        contextKey += Temptaletype

        item = DBHelp.GetTemplateCompletionList(prefixText, count, contextKey)
        Return item
    End Function

    'THIS IS WEB SERVICE FOR AUTOCOMPLETE EXTENDER FOR PROJECT which are under User's Rights
    <WebMethod(EnableSession:=True)>
    <System.Web.Script.Services.ScriptMethod()>
    Public Function GetMyScreeingNoCompletionList(ByVal prefixText As String) As String()

        Dim items As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)()

        Dim ds As New DataSet()
        Dim estr As String = ""
        Dim result As Boolean = False
        Dim dr As DataRow = Nothing
        Dim whereCondition As String = "vMySubjectNo " + " Like '%"
        whereCondition += prefixText + "%'"
        '+ IIf(contextKey.Trim() <> "", " AND " & contextKey.Trim(), "")
        ''whereCondition += " AND  (CASE WHEN  cIsTrainingAssign <> 'Y' THEN 1 ELSE   CASE WHEN  vTrainingFinished = 'YES' THEN 1 ELSE 0 END   END ) =1  AND (CASE WHEN  cIsTrainingAssign = 'Y' THEN 1 ELSE  CASE WHEN vTrainingFinished = 'No' THEN 1 END END ) =1 "


        Dim Objcommon As New clsCommon
        Dim objHlp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()

        result = objHlp.GetFieldsOfTable("WorkSpaceSubjectMst", " * ",
            whereCondition, ds, estr)

        For index As Integer = 0 To ds.Tables(0).Rows.Count - 1
            dr = ds.Tables(0).Rows(index)
            'items.Add("'" + dr.Item("vWorkspaceid").ToString + "#" + dr.Item("vProjectNo").ToString + "#" + dr.Item("vRequestId").ToString() + "#" + dr.Item("ParentWorkspaceId").ToString.Trim())
            items.Add("'" + dr.Item("vWorkspaceId").ToString + "#" + dr.Item("vMySubjectNo").ToString)
            '+ "#" + dr.Item("vProjectNo").ToString) '+ "#" + dr.Item("vRequestId").ToString())
        Next

        Return items.ToArray()
    End Function

    <WebMethod(EnableSession:=True)>
    <System.Web.Script.Services.ScriptMethod()>
    Public Function GetScreeningNoCompletionlistUserWise(ByVal prefixText As String, ByVal contextKey As String) As String()
        Dim items As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)()
        Dim ds As New DataSet()
        Dim estr As String = ""
        Dim result As Boolean = False
        Dim dr As DataRow = Nothing
        Dim whereCondition As String = " vWorkspaceId in (SELECT Distinct vWorkspaceId FROM View_Myprojects where " + contextKey + ")" + " AND vMySubjectNo" + " Like '%" + prefixText + "%'"
        Dim Objcommon As New clsCommon
        Dim objHlp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()

        result = objHlp.GetFieldsOfTable("WorkSpaceSubjectMst", " * ", whereCondition, ds, estr)
        For index As Integer = 0 To ds.Tables(0).Rows.Count - 1
            dr = ds.Tables(0).Rows(index)
            items.Add("'" + dr.Item("vWorkspaceId").ToString + "#" + dr.Item("vMySubjectNo").ToString)
        Next

        Return items.ToArray()
    End Function

    <WebMethod(EnableSession:=True)>
    <System.Web.Script.Services.ScriptMethod()>
    Public Function GetMyChildProjectCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim Objcommon As New clsCommon
        Dim DBHelp As WS_HelpDbTable.WS_HelpDbTable = Objcommon.GetHelpDbTableRef()
        Dim item() As String

        Dim Wstr_ScopeValue As String = ""

        'To Get Where condition of ScopeVales( Project Type )
        If Not Objcommon.GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue + " AND iUserId=" + HttpContext.Current.Session(S_UserID)
        Else
            contextKey = Wstr_ScopeValue + " AND iUserId=" + HttpContext.Current.Session(S_UserID)
        End If
        '*****************************************

        item = DBHelp.GetChildProjectCompletionList(prefixText, count, contextKey)
        Return item

    End Function

End Class











