Imports Microsoft.VisualBasic
Imports System.IO
Imports System.IO.File
Imports System.Xml


Public Class clsWorkspaceRevisedPublishService
    Private objHelpDb As clsHelpDB
    Private objCommon As clsCommon
    Private objTrackingTableGenerator As clsTrackingTableGenerator
    Public projectPublishType As String = Nothing
    Public leafIds() As Integer = Nothing
    Public iParentId As Integer = Nothing
    Public absolutePathToCreate As String = Nothing
    Public euDTDVersion As String = Nothing
    Private baseWorkFolder As String = Nothing

    Private AllNodesofHistory As DataTable = Nothing
    Public childAttrId As DataTable = Nothing
    Public parentNodes As DataTable = Nothing
    Private STFXMLLocation As DataTable = Nothing

    Public parentDir As DirectoryInfo = Nothing
    Public parentNodeId As String = Nothing
    Public file As FileType
    Public nodeId As Integer
    Public nodeName As String
    Public nodeDisplayName As String


    Public writer As HttpWriter
    Public xmlwriter As XmlWriter = Nothing
    Public folderDtl As New DataTable
    Public fileNameDtl As New DataTable
    Public fileName As String
    Public folderStructure As String
    Public sourceFolderName As String
    Public publishDestFolderName As String
    Public attrId As Integer?
    Public attrValue, attrName As String
    Public childNode As Integer
    Public childNodeSize As Integer = 0
    Public wsId As String = String.Empty
    Public stype As String = String.Empty
    Public folderName As StringBuilder
    Public folderStruct As FileStream

    Public LastPublishedVersion As String

    Private relativePathToCreate As String
    Private nodetypeindi As String
    'Added For giving space
    Friend newPath As String = ""
    Friend studyId As String
    Private upCount As Integer = 0
    Public workspaceDesc As String
    Public baseLocation As String
    Public workspaceLabelId As String
    Public userId As Integer?

    Public pathlst As ArrayList

    Public trannolst As ArrayList
    Public tranno As Integer
    Public folder As String

    Private currentSequenceNumber As String

    Dim DTOSubmissionMst As New DataTable
    Dim Retu_Str As String = String.Empty
    Dim dos As Date



    Friend Function workspacePublish(ByVal workspaceId As String, _
                                     ByRef publishForm As DataTable, _
                                     ByVal userId As String, _
                                     ByVal dos As Date, _
                                     ByVal wsDesc As String, _
                                     ByVal baseWorkFolder As String, _
                                     ByRef eStr_Retu As String) As Boolean

        Dim fileType As FileType
        Dim file As File
        Dim LabelNo As Integer = Nothing
        Dim sType As String
        Dim query As String
        Dim eStr As String

        Try
            workspacePublish = False
            eStr_Retu = ""
            wsId = workspaceId
            Me.baseWorkFolder = baseWorkFolder
            workspaceDesc = wsDesc
            Me.dos = dos

            objHelpDb = New clsHelpDB
            LabelNo = publishForm.Rows(0)("LabelNo")

            'Change for New Submission Path
            publishDestFolderName = publishForm.Rows(0)("PublishDestinationPath")
            createBaseFolder(publishDestFolderName, publishForm, eStr_Retu) 'Develop by Bharat & SUHANI
            sType = publishForm.Rows(0)("SubmissionFlag")
            currentSequenceNumber = publishForm.Rows(0)("SeqNumber")


            STFXMLLocation = New DataTable
            STFXMLLocation.Columns.Add("iNodeId", GetType(System.Int32))
            STFXMLLocation.Columns.Add("vBaseWorkFolder", GetType(System.String))
            STFXMLLocation.AcceptChanges()


            If projectPublishType = "P" Then
                'Get all nodes and its parent nodes where file attached
                If Not objHelpDb.getAllNodesFromHistoryForRevisedSubmission(workspaceId + "#" + _
                                                                            LabelNo.ToString + "#", AllNodesofHistory, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving Proc_WorkSpaceNodeForRevisedSubmission_Doc, " + eStr
                    Exit Function
                End If
                'Call for Proc_WorkSpaceNodeForRevisedSubmission_Doc			
            ElseIf projectPublishType = "M" Then
                'In case of Manual Mode getting all parents of the selected nodes
                Dim leafIdsString As String
                For i As Integer = 0 To leafIds.Length - 1
                    leafIdsString += leafIds(i).ToString() + ","
                Next
                If Not leafIdsString = String.Empty Then
                    If Not objHelpDb.getWorkspaceTreeNodesForLeafs(workspaceId + "#" + _
                                                                   leafIdsString.Remove(leafIdsString.Length - 1) + "#", _
                                                                   AllNodesofHistory, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while retrieving Proc_WorkspaceTreeNodesForLeafs, " + eStr
                        Exit Function
                    End If
                End If
                'Call for Proc_WorkSpaceTreeNodesForLeafs
            End If
            AllNodesofHistory.PrimaryKey = New DataColumn() {AllNodesofHistory.Columns("inodeid")}
            'COPY UTIL FOLDER
            addutilFolder(sType, eStr_Retu) 'Develop by Bharat & SUHANI

            If sType = "eu" Or sType = "us" Then
                iParentId = 1
                parentDir = New DirectoryInfo(absolutePathToCreate + "/m1/" + sType + "/")
                If Not parentDir.Exists Then
                    parentDir.Create()
                End If


                Dim out As FileStream = file.Create(absolutePathToCreate + "/m1/" + sType + "/" + sType + "-regional.xml")
                Dim WriterSettings As New XmlWriterSettings()
                WriterSettings.ConformanceLevel = ConformanceLevel.Fragment
                xmlwriter = System.Xml.XmlWriter.Create(out, WriterSettings)

                writeToXmlFile(sType, publishForm, eStr_Retu) 'Develop By Bharat & Suhani
                Dim Dtl_getChildNodeByParentForPublishForM1 As New DataTable()
                query = "SELECT vWorkspaceId,iNodeId,vNodeName,vNodeDisplayName," + _
                        "       vFolderName,cNodeTypeIndi,vRemark " + _
                        " FROM workspaceNodeDetail " + _
                        " WHERE iParentNodeId=" + iParentId.ToString + _
                        " and vWorkspaceId='" + workspaceId + "' and iNodeNo= 1"
                If Not objHelpDb.getChildNodeByParentForPublishForM1(query, Dtl_getChildNodeByParentForPublishForM1, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving getChildNodeByParentForPublishForM1, " + eStr
                    Exit Function
                End If

                getChildNode(Dtl_getChildNodeByParentForPublishForM1, absolutePathToCreate, iParentId, sType, publishForm, eStr_Retu) 'Int IdValue is removed which is available in JAVA Code.

                If sType = "eu" Then
                    xmlwriter.WriteString("</eu:eu-backbone>")
                End If
                If sType = "us" Then
                    xmlwriter.WriteString("</fda-regional:fda-regional>")
                End If

                xmlwriter.Flush()
                xmlwriter.Close()
                out.Flush()
                out.Close()

                UpdateXMLFile(absolutePathToCreate + "/m1/" + sType + "/" + sType + "-regional.xml", eStr_Retu)

                fileType.File = file
                fileType.FullName = absolutePathToCreate + "/m1/" + sType + "/" + sType + "-regional.xml"

                If publishForm.Rows(0)("AddTT") = "Y" Then
                    clsTrackingTableGenerator.addTrackingTable(fileType, workspaceId, publishForm.Rows(0)("SubmissionId"), euDTDVersion, baseWorkFolder, eStr_Retu)
                End If

                'CODE FOR INDEX.XML
                out = file.Create(absolutePathToCreate + "/index.xml")
                xmlwriter = System.Xml.XmlWriter.Create(out, WriterSettings)

                writeToXmlFile(sType + "m2-m5", publishForm, eStr_Retu)

                Dim Dtl_getChildNodeByParentForPublishFromM2toM5 As New DataTable()
                query = "select vWorkspaceId,iNodeId,vNodeName,vNodeDisplayName,vFolderName," + _
                        "       cNodeTypeIndi,vRemark " + _
                        " from workspaceNodeDetail " + _
                        " where iParentNodeId=" + iParentId.ToString + _
                        " and vWorkspaceId='" + workspaceId + "' and iNodeNo <> 1 order by iNodeNo"
                If Not objHelpDb.getChildNodeByParentForPublishFromM2toM5(query, Dtl_getChildNodeByParentForPublishFromM2toM5, "Error") Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving getChildNodeByParentForPublishFromM2toM5, " + eStr
                    Exit Function
                End If

                For Each DTOWorkSpaceNodeDetail As DataRow In Dtl_getChildNodeByParentForPublishFromM2toM5.Rows
                    nodeId = DTOWorkSpaceNodeDetail("iNodeId")
                    nodeName = DTOWorkSpaceNodeDetail("vNodeName")
                    nodeDisplayName = DTOWorkSpaceNodeDetail("vNodeDisplayName")
                Next DTOWorkSpaceNodeDetail

                getChildNode(Dtl_getChildNodeByParentForPublishFromM2toM5, absolutePathToCreate, iParentId, sType, publishForm, eStr_Retu) 'Int IdValue is removed which is available in JAVA Code.
                xmlwriter.WriteString("</ectd:ectd>")
                xmlwriter.Flush()
                xmlwriter.Close()
                out.Flush()
                out.Close()

                UpdateXMLFile(absolutePathToCreate + "/index.xml", eStr_Retu)
            End If

            checkSumForindexFile(eStr_Retu)
            workspacePublish = True

        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while WorkspacePublish"
            workspacePublish = False
        End Try
    End Function
    Public Function generateSTFFile(ByVal wsId As String, ByRef eStr_Retu As String) As Boolean
        Dim objclsHelpDB As New clsHelpDB()
        Dim getallfirstnode As New DataTable
        Dim eStr As String = String.Empty
        Dim whr As String = String.Empty
        Try
            eStr_Retu = ""
            whr = "vWorkspaceId='" + wsId + "' and cNodeTypeIndi='T'"
            If Not objclsHelpDB.getAllSTFFirstNodes(whr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    getallfirstnode, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving View_CommonWorkspaceDetail, " + eStr
                Exit Function
            End If

            generateStudyDocument(wsId, getallfirstnode, eStr_Retu)
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while GenerateSTFFile"
            generateSTFFile = False
        End Try
    End Function
    Public Function getUpCount(ByVal filelocation As String, ByRef eStr_Retu As String) As Integer
        Dim count As Integer = 1
        Try
            eStr_Retu = ""
        
            filelocation = filelocation.Substring((absolutePathToCreate.Length()) + 1)
            For j = 0 To filelocation.Length() - 1
                If "\" = filelocation.Chars(j) Or "/" = filelocation.Chars(j) Then
                    count += 1
                End If
            Next

            Return count
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while GetUpCount"
        End Try
    End Function
    Public Function generateStudyDocument(ByVal wsId As String, ByVal dtstffirstnodes As DataTable, _
                                          ByRef eStr_Retu As String) As Boolean
        generateStudyDocument = False
        Dim objclsHelpDB As New clsHelpDB()
        Dim estr As String = String.Empty
        Dim nodeName As String = String.Empty
        Dim nodeId As Integer = 0
        Dim nodecategory As String = String.Empty
        Dim stfdata As String = String.Empty
        Dim stfparentnodeid As Integer = 0
        Dim fileName As String = String.Empty
        Dim nodeno As Integer = 0
        Dim isLeaf As Integer = 0
        Dim rows() As DataRow = Nothing
        Dim out As StreamWriter

        Dim dtAllChildNodes As New DataTable
        Dim dtstfHistory As New DataTable
        Dim dtallMultipleTypeChildNodes As New DataTable
        Dim dtsiteIdVector As New DataTable
        Dim dtisLeaf As New DataTable


        Dim whr As String = String.Empty
        Try
            For i = 0 To dtstffirstnodes.Rows.Count - 1

                stfparentnodeid = dtstffirstnodes.Rows(i)("iNodeId")
                whr = "vWorkspaceId='" + wsId + "' and iParentNodeId=" + stfparentnodeid.ToString
                If Not objclsHelpDB.isLeafNodes(whr, DataRetrievalModeEnum.DataTable_WithWhereCondition, isLeaf, estr) Then
                    eStr_Retu += vbCrLf + "Error occured while IsLeafNodes, " + estr
                    Exit Function
                End If

                Dim filelocation As String = getSTFFileLocation(stfparentnodeid, eStr_Retu)
                If filelocation.Equals("") Then
                    'This node is not going to publish as no STF docs were changed
                    Continue For
                End If
                upCount = getUpCount(filelocation, eStr_Retu)

                If isLeaf = 0 Then 'start of isleaf
                    Dim studyidendata As String = ""
                    stfdata = "<study-document>"

                    whr = "SELECT " + _
                        "   Distinct iNodeId, vWorkspaceId, iNodeNo, iParentNodeId, " + _
                        "   vNodeName,vNodeDisplayName,vFolderName,vNodeCategory,cNodeTypeIndi, " + _
                        "   (Case When cNodeTypeIndi = 'F' then '' else vAttrName end) as vAttrName, " + _
                        "   (Case When cNodeTypeIndi = 'F' then '' else vAttrValue end) as vAttrValue " + _
                        " From View_CommonWorkspaceDetail " + _
                        "   WHERE vWorkspaceId='" + wsId + "'" + " AND iParentNodeId =" + stfparentnodeid.ToString + _
                        "   AND ((cNodeTypeIndi= 'S' AND (vattrname= 'operation' OR vattrname='')) OR cNodeTypeIndi = 'F') " + _
                        "   order by iNodeNo"


                    If Not objclsHelpDB.getAllChildSTFNodes(whr, dtAllChildNodes, estr) Then
                        eStr_Retu += vbCrLf + "Error occured while retrieving  view_commonworkspacedetail, " + estr
                        Exit Function
                    End If

                    For j = 0 To dtAllChildNodes.Rows.Count - 1 'start of getAllChildNodes
                        nodeId = CType(dtAllChildNodes.Rows(j)("iNodeId"), Integer)
                        nodeName = dtAllChildNodes.Rows(j)("vNodeName")
                        nodecategory = dtAllChildNodes.Rows(j)("vNodeCategory")
                        nodeno = CType(dtAllChildNodes.Rows(j)("iNodeNo"), Integer)
                        If nodeno = 1 Then
                            rows = AllNodesofHistory.Select("inodeid = " + nodeId.ToString)
                            If rows.Length > 0 Then
                                studyidendata = generateStudyIndetifier(nodeId, wsId, eStr_Retu)
                                whr = "vWorkspaceId='" + wsId + "' and iNodeId=" + nodeId.ToString
                                If Not objclsHelpDB.getLastNodeHistory(whr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                       dtstfHistory, estr) Then
                                    eStr_Retu += vbCrLf + "Error occured while retrieving  View_CommonWorkspaceDetail, " + estr
                                    Exit Function
                                End If
                                fileName = dtstfHistory.Rows(0)("vFileName")
                                out = New StreamWriter(filelocation + "/" + fileName)
                                out.Write(studyidendata)
                            Else
                                Exit For
                            End If

                        Else
                            rows = AllNodesofHistory.Select("inodeid = " + nodeId.ToString)

                            If rows.Length > 0 And Not dtAllChildNodes.Rows(j)("vAttrValue").ToString().ToLower.Contains("delete") Then
                                If dtAllChildNodes.Rows(j)("cNodeTypeIndi") = "S" Then '//if node is leaf node (old code)
                                    stfdata += "<doc-content xlink:href="""
                                    For icount = 0 To upCount
                                        stfdata += "../"
                                    Next
                                    stfdata += currentSequenceNumber + "/" + "index.xml" + "#node-" + nodeId.ToString + """>" + Chr(13)
                                    stfdata += "<file-tag name=""" + nodeName + """ info-type=""" + nodecategory + """/>" + Chr(13)
                                    stfdata += "</doc-content>" + Chr(13)

                                ElseIf dtAllChildNodes.Rows(j)("cNodeTypeIndi") = "F" Then
                                    whr = "SELECT " + _
                                           "   Distinct iNodeId, vWorkspaceId, iNodeNo, iParentNodeId, " + _
                                           "   vNodeName,vNodeDisplayName,vFolderName,vNodeCategory,cNodeTypeIndi, " + _
                                           "   (Case When cNodeTypeIndi = 'F' then '' else vAttrName end) as vAttrName, " + _
                                           "   (Case When cNodeTypeIndi = 'F' then '' else vAttrValue end) as vAttrValue " + _
                                           " From View_CommonWorkspaceDetail " + _
                                           "   WHERE vWorkspaceId='" + wsId + "'" + " AND iParentNodeId =" + nodeId.ToString + _
                                           "   AND ((cNodeTypeIndi= 'S' AND (vattrname= 'operation' OR vattrname='')) OR cNodeTypeIndi = 'F') " + _
                                           "   order by iNodeNo"

                                    If Not objclsHelpDB.getAllChildSTFNodes(whr, dtallMultipleTypeChildNodes, estr) Then
                                        eStr_Retu += vbCrLf + "Error occured while retrieving  view_commonworkspacedetail, " + estr
                                        Exit Function
                                    End If


                                    For k = 0 To dtallMultipleTypeChildNodes.Rows.Count - 1
                                        rows = AllNodesofHistory.Select("inodeid = " + dtallMultipleTypeChildNodes.Rows(k)("iNodeId").ToString)
                                        If rows.Length > 0 And Not dtallMultipleTypeChildNodes.Rows(k)("vAttrValue").ToString().ToLower().Contains("delete") Then
                                            'if file attached at node or not and also the 'operation' is not 'delete'

                                            stfdata += "<doc-content xlink:href="""
                                            For icount = 0 To upCount
                                                stfdata += "../"
                                            Next
                                            stfdata += currentSequenceNumber + "/" + "index.xml" + "#node-" + _
                                                        dtallMultipleTypeChildNodes.Rows(k)("iNodeId").ToString + """>" + Chr(13)

                                            whr = "vWorkspaceId ='" + wsId + "' and iNodeId=" + dtallMultipleTypeChildNodes.Rows(k)("iNodeId").ToString
                                            If Not objclsHelpDB.getSTFIdentifierByNodeId(whr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                                            dtsiteIdVector, estr) Then
                                                eStr_Retu += vbCrLf + "Error occured while retrieving  stfstudyIdentifiermst, " + estr
                                                Exit Function
                                            End If


                                            stfdata += "<" + dtsiteIdVector.Rows(0)("vTagName").ToString.ToLower + " " ';//start property tag
                                            stfdata += dtsiteIdVector.Rows(0)("vAttrName").ToString.ToLower + "=""" + dtsiteIdVector.Rows(0)("vAttrValue").ToString.ToLower + """ "

                                            stfdata += dtsiteIdVector.Rows(1)("vAttrName").ToString.ToLower + "=""" + dtsiteIdVector.Rows(1)("vAttrValue").ToString.ToLower + """>"
                                            stfdata += dtsiteIdVector.Rows(1)("vNodeContent").ToString.ToLower  ';//Site-Identifier value
                                            stfdata += "</" + dtsiteIdVector.Rows(1)("vTagName").ToString.ToLower + ">" + Chr(13) ';//end property1 tag


                                            stfdata += "<file-tag name=""" + dtallMultipleTypeChildNodes.Rows(k)("vNodeName") + _
                                                        """ info-type=""" + dtallMultipleTypeChildNodes.Rows(k)("vNodeCategory") _
                                                        + """/>" + Chr(13)
                                            stfdata += "</doc-content>" + Chr(13)

                                        End If
                                    Next
                                End If
                            End If
                        End If 'end of nodeno
                        'stfNodeDtl = Nothing
                    Next 'end of getAllChildNodes
                    stfdata += "</study-document></ectd:study>" + Chr(13)

                    'if STF XML is not in history then 'out' will be null.
                    If Not out Is "" Or out Is Nothing Then
                        out.Write(stfdata)
                        out.Close()
                    End If
                    dtAllChildNodes = Nothing
                End If 'end of isleaf
            Next
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while GenerateStudyDocument"
        End Try
    End Function
    Public Function generateStudyIndetifier(ByVal firstnodeId As String, ByVal wsId As String, ByRef eStr_Retu As String) As String
        Dim previoustagid As Integer = 0
        Dim currenttagid As Integer = 0
        Dim tagname As String = String.Empty
        Dim attrName As String = String.Empty
        Dim attrValue As String = String.Empty
        Dim nodeContent As String = String.Empty
        Dim previoustagname As String = String.Empty
        Dim previoustagcontent As String = String.Empty
        Dim studyidendata As String = "<?xml version=""1.0"" encoding=""UTF-8""?>" + Chr(13)
        Dim studyId As String = String.Empty
        Dim dtstudyidentifierdata As New DataTable
        Dim objclsHelpDB As New clsHelpDB
        Dim Whr As String = String.Empty
        Dim eStr As String = String.Empty

        Try
            eStr_Retu = ""
        
            studyidendata += "<!DOCTYPE ectd:study SYSTEM """

            For icount = 0 To upCount - 1
                studyidendata += "../"
            Next
            studyidendata += "util/dtd/ich-stf-v2-2.dtd"">" + Chr(13)
            studyidendata += "<?xml-stylesheet type=""text/xsl"" href="""
            For icount = 0 To upCount - 1
                studyidendata += "../"
            Next
            studyidendata += "util/style/ich-stf-stylesheet-2-2.xsl""?>" + Chr(13)
            studyidendata += "<ectd:study xmlns:ectd=""http://www.ich.org/ectd"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xml:lang=""en"" dtd-version=""2.2"">" + Chr(13)
            studyidendata += "<study-identifier>" + Chr(13)

            Whr = "vWorkspaceId ='" + wsId + "' and iNodeId=" + firstnodeId
            If Not objclsHelpDB.getSTFIdentifierByNodeId(Whr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                         dtstudyidentifierdata, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving stfstudyIdentifiermst, " + eStr
                Exit Function
            End If
            For i = 0 To dtstudyidentifierdata.Rows.Count - 1

                currenttagid = dtstudyidentifierdata.Rows(i)("iTagSequenceId")
                tagname = dtstudyidentifierdata.Rows(i)("vTagName").ToString.ToLower
                attrName = dtstudyidentifierdata.Rows(i)("vAttrName").ToString.ToLower
                attrValue = dtstudyidentifierdata.Rows(i)("vAttrValue").ToString.ToLower
                nodeContent = dtstudyidentifierdata.Rows(i)("vNodeContent").ToString.ToLower
                If tagname.ToLower().Contains("study-id") Then
                    studyId = nodeContent
                End If
                If previoustagid > 0 Then
                    If previoustagid = currenttagid Then
                        studyidendata += " " + attrName + "=""" + attrValue + """"
                    Else
                        '// previous tag close with nodecontent.
                        studyidendata += ">" + previoustagcontent + "</" + previoustagname + ">" + Chr(13)
                        '// new tag start.// create a tag.
                        If attrName <> "" And Not attrName.Equals("") Then
                            studyidendata += "<" + tagname + " " + attrName + "=""" + attrValue + """"
                        Else
                            studyidendata += "<" + tagname
                        End If
                    End If
                Else
                    '// creata tag.
                    If attrName <> "" And Not attrName.Equals("") Then
                        studyidendata += "<" + tagname + " " + attrName + "=""" + attrValue + """"
                    Else
                        studyidendata += "<" + tagname
                    End If
                End If
                previoustagname = tagname
                previoustagcontent = nodeContent
                previoustagid = currenttagid
            Next
            '// close last tag.
            studyidendata += ">" + previoustagcontent + "</" + previoustagname + ">" + Chr(13)
            studyidendata += "</study-identifier>"
            Return studyidendata
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while GenerateStudyIdentifier"
        End Try
    End Function
    Public Function getSTFFileLocation(ByVal nodeId As Integer, ByRef eStr_Retu As String) As String
        Try
            eStr_Retu = ""
        
            Dim filelocation As String = String.Empty

            For i = 0 To STFXMLLocation.Rows.Count - 1
                If STFXMLLocation.Rows(i)("iNodeId") = nodeId Then
                    filelocation = STFXMLLocation.Rows(i)("vBaseWorkFolder")
                    Return filelocation
                End If
            Next
            Return filelocation
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while GetSTFFileLocation"
            Return ""
        End Try
    End Function
    Public Function copyUSDTDFile(ByVal stype As String) As Boolean


    End Function


    Private Sub createBaseFolder(ByVal bfoldername As String, ByVal publishForm As DataTable, _
                                 ByRef eStr_Retu As String)
        Dim Retu_Str As String = String.Empty
        Dim wStr As String = String.Empty
        Try
            eStr_Retu = ""
            wStr = "select Distinct vBaseWorkFolder,vBasePublishFolder,vLastPublishedVersion " + _
                   " from View_CommonWorkspaceDetail " + _
                   " where vworkspaceid='" + wsId + "'"
            If Not objHelpDb.getFolderByWorkSpaceId(wStr, DataRetrievalModeEnum.DatatTable_Query, folderDtl, Retu_Str) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving View_CommonWorkspaceDetail, " + Retu_Str
                Exit Sub
            End If


            If folderDtl IsNot Nothing Then
                ' Source Folder Path
                sourceFolderName = folderDtl.Rows(0)("vBaseWorkFolder").ToString()
                LastPublishedVersion = folderDtl.Rows(0)("vLastPublishedVersion").ToString()

            End If
            'Change for New Submission Path

            'createBaseFolder = New File(bfoldername & "/" & publishForm.seqNumber)

            absolutePathToCreate = bfoldername & "/" & publishForm.Rows(0)("seqNumber").ToString
            absolutePathToCreate = absolutePathToCreate.Replace("/", "\")

            Console.WriteLine("absolutePathToCreate: " & absolutePathToCreate)
            If Not Directory.Exists(absolutePathToCreate) Then ' Check folder already present or not
                Directory.CreateDirectory(absolutePathToCreate)
            End If

        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while CreateBaseFolder"
        End Try
    End Sub

    Private Function copyFileforPublish(ByVal publishForm As DataTable, ByVal pathToCreate As String, _
                                        ByRef eStr_Retu As String) As String

        Dim absolutePath As String = String.Empty
        Dim absolutePathLink As String = String.Empty
        Dim wStr As String = String.Empty
        Retu_Str = String.Empty
        newPath = String.Empty
        absolutePath = pathToCreate
        absolutePathLink = pathToCreate.Replace(absolutePathToCreate, String.Empty)
        Dim nIds As String = String.Empty
        Dim iIds As Integer()

        Try
            eStr_Retu = ""
        
            'In case of Manual Mode Publish
            If projectPublishType = "M" Then
                'List(Of DTOWorkSpaceNodeHistory)(nodeHistory = docMgmtInt.getAllNodesLastHistory(wsId, New Integer() {nodeId}))
                iIds = New Integer() {nodeId}

                For i As Integer = 0 To iIds.Length - 1
                    nIds += iIds(i).ToString + ","
                Next

                If Not nIds = String.Empty Then
                    wStr = "vworkspaceId IN (" + wsId + ") AND inodeId IN (" + nIds.Remove(nIds.Length - 1) + ")"
                    If Not objHelpDb.getAllNodesLastHistory(wStr, fileNameDtl, Retu_Str) Then
                        eStr_Retu += vbCrLf + "Error occured while View_NodesLatestHistory, " + Retu_Str
                        Exit Function
                    End If
                End If

                'select vWorkspaceId,vWorkSpaceDesc,vBaseWorkFolder,iNodeId,iTranNo,vFileName,vFolderName,
                'iStageId,vStageDesc,iModifyBy,vUserName,dModifyOn,vNodeName,vNodeFolderName,vUserDefineVersionId
                'from View_NodesLatestHistory where vWorkspaceId = '"+wsId+"' and nodeId in (nodeIDs[]) order by vWorkspaceId,iNodeId
            Else
                wStr = wsId + "#" + nodeId.ToString + "#" + publishForm.Rows(0)("LabelNo").ToString
                'fileNameDtl = docMgmtInt.getFileNameForNodeForPublish(nodeId, wsId, publishForm.labelNo)
                If Not objHelpDb.getFileNameForNodeForPublish(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, fileNameDtl, Retu_Str) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving Proc_FileNameForPublish, " + Retu_Str
                    Exit Function
                End If
                'Call for Proc_FileNameForPublish
            End If

            If fileNameDtl.Rows.Count > 0 Then
                'Dim dtowsand As DTOWorkSpaceNodeHistory = CType(fileNameDtl(0), DTOWorkSpaceNodeHistory)
                'fileName = dtowsand.FileName

                fileName = fileNameDtl.Rows(0)("vFileName").ToString
                folderStructure = fileNameDtl.Rows(0)("vFolderName").ToString
                folderStructure = (sourceFolderName.Trim() & folderStructure.Trim() & "/" & fileName)
                'folderStruct = New File(sourceFolderName.Trim() & folderStructure.Trim() & "/" & fileName)

                addFiles(folderStructure, absolutePath, eStr_Retu)

                Return (absolutePathLink.Substring(1) & "/" & fileName)
            Else
                Return (absolutePathLink.Substring(1))
            End If
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while CopyFileForPublish"
        End Try
    End Function
    '    
    '     * Add file at Publish Folder Path 
    '     *  folderStruct = Source Folder File 
    '     *  publishFolderStuct = Destination Folder File
    '     
    Private Sub addFiles(ByVal folderStruct As String, ByVal publishFolderStuct As String, ByRef eStr_Retu As String)
        Try
            eStr_Retu = ""
            If System.IO.Directory.Exists(publishFolderStuct) Then
                System.IO.File.Copy(folderStruct, publishFolderStuct + folderStruct.Substring(folderStruct.Replace("/", "\").LastIndexOf("\")))
            End If
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while AddFiles"
        End Try
    End Sub

    Private Sub createSubFolders(ByVal pathToCreate As String, ByVal folderName As String, _
                                 ByRef eStr_Retu As String)
        Try
            eStr_Retu = ""
        
            relativePathToCreate = pathToCreate + "/" + folderName
            If Not Directory.Exists(relativePathToCreate) Then
                Directory.CreateDirectory(relativePathToCreate)
            End If
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while CreateSubFolders"
        End Try
    End Sub

    Private Sub checkSumForindexFile(ByRef eStr_Retu As String)
        Try
            eStr_Retu = ""
            Dim indexmd5 As String = clsCommon.CalculateChecksum(absolutePathToCreate & "/index.xml")
            Dim indexHashFileout As New StreamWriter(absolutePathToCreate & "/index-md5.txt")

            indexHashFileout.Write(indexmd5)
            indexHashFileout.Close()
            indexHashFileout = Nothing
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while checkSumForindexFile"
        End Try
    End Sub

    Private Sub addutilFolder(ByVal stype As String, ByRef eStr_Retu As String)
        Try
            eStr_Retu = ""
            Dim sourceLocation As DirectoryInfo = Nothing
            Dim targetLocation As DirectoryInfo = Nothing

            If stype.Equals("us") Then
                sourceLocation = New DirectoryInfo(baseWorkFolder + "/util_us/util")
            Else
                sourceLocation = New DirectoryInfo(baseWorkFolder + "/util_eu/util")
            End If

            targetLocation = Directory.CreateDirectory(absolutePathToCreate + "/util")

            For Each sDir As DirectoryInfo In sourceLocation.GetDirectories

                If Not Directory.Exists(targetLocation.FullName + "\" + sDir.Name) Then
                    Directory.CreateDirectory(targetLocation.FullName + "\" + sDir.Name)
                End If

                For Each sFile As FileInfo In sDir.GetFiles
                    System.IO.File.Copy(sFile.FullName, targetLocation.FullName + "\" + sDir.Name + "\" + sFile.Name, True)
                Next sFile

            Next sDir

        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while AddUtilFolder"
        End Try
    End Sub
    Private Sub writeToXmlFile(ByVal stype As String, ByVal publishForm As DataTable, _
                               ByRef eStr_Retu As String)

        Dim StringBuilder As String
        Dim cmsdtl As New DataTable
        Dim wStr As String = String.Empty
        Dim Retu_Str As String = String.Empty
        Dim dtoRms As New DataTable
        Dim dos As String = String.Empty

        Try
            eStr_Retu = ""
        
            If stype.Equals("eu") Then
                StringBuilder += "<?xml version=""1.0"" encoding=""UTF-8""?>"
                StringBuilder += Environment.NewLine.ToString + "<!DOCTYPE eu:eu-backbone SYSTEM ""../../util/dtd/eu-regional.dtd"">"
                StringBuilder += Environment.NewLine.ToString + "<?xml-stylesheet type=""text/xsl"" href=""../../util/style/eu-regional.xsl""?>"
                StringBuilder += Environment.NewLine.ToString + "<eu:eu-backbone xmlns:eu=""http://europa.eu.int"" xmlns:xlink=""http://www.w3c.org/1999/xlink"" dtd-version=""1.3"">"
                StringBuilder += Environment.NewLine.ToString + Environment.NewLine.ToString + "<eu-envelope>"

                wStr = "SubmissionId=" + publishForm.Rows(0)("SubmissionId").ToString
                If Not objHelpDb.getWorkspaceCMSSubmissionInfo(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, cmsdtl, Retu_Str) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving View_WorkspaceCMSSubmissionDtl, " + Retu_Str
                    Exit Sub
                End If


                If cmsdtl.Rows.Count > 0 Then
                    publishForm.Rows(0)("submissionDescription") = cmsdtl.Rows(0)("submissionDescription")
                Else
                    wStr = "vSubmissionInfoEUDtlId=" + publishForm.Rows(0)("SubmissionId").ToString
                    If Not objHelpDb.getWorkspaceRMSSubmissionInfo(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, dtoRms, Retu_Str) Then
                        eStr_Retu += vbCrLf + "Error occured while retrieving SubmissionInfoEUSubDtl, " + Retu_Str
                        Exit Sub
                    End If

                    If dtoRms.Rows.Count > 0 Then
                        publishForm.Rows(0)("submissionDescription") = dtoRms.Rows(0)("submissionDescription")
                    End If
                End If

                'Envelope for RMS
                If publishForm.Rows(0)("rMSSelected").ToString = "Y" Then

                    StringBuilder += Environment.NewLine.ToString + "<envelope country=""" & publishForm.Rows(0)("country").ToString.ToLower() & """>"
                    StringBuilder += Environment.NewLine.ToString + "<application>"
                    If publishForm.Rows(0)("applicationNumber") IsNot Nothing AndAlso (Not publishForm.Rows(0)("applicationNumber").ToString.Equals("")) Then
                        Dim appNums() As String = publishForm.Rows(0)("applicationNumber").ToString.Split(",")
                        For iNum As Integer = 0 To appNums.Length - 1
                            StringBuilder += Environment.NewLine.ToString & "<number>" & appNums(iNum).Replace("-", "/").Trim() & "</number>"
                        Next iNum
                    End If
                    StringBuilder += Environment.NewLine.ToString + "</application>"
                    StringBuilder += Environment.NewLine.ToString + "<applicant>" & publishForm.Rows(0)("applicant").ToString & "</applicant>"
                    StringBuilder += Environment.NewLine.ToString + "<agency code=" & """" & publishForm.Rows(0)("agencyName").ToString & """" & " />"
                    If publishForm.Rows(0)("atc").ToString IsNot Nothing AndAlso (Not publishForm.Rows(0)("atc").ToString.Equals("")) Then
                        Dim atcCodes() As String = publishForm.Rows(0)("atc").ToString.Split(",")
                        For iAtc As Integer = 0 To atcCodes.Length - 1
                            StringBuilder += Environment.NewLine.ToString + "<atc>" & atcCodes(iAtc).Trim() & "</atc>"
                        Next iAtc
                    End If
                    StringBuilder += Environment.NewLine.ToString + "<submission type=""" & publishForm.Rows(0)("submissionType_eu").ToString + """/>"
                    StringBuilder += Environment.NewLine.ToString + "<procedure type=""" & publishForm.Rows(0)("procedureType").ToString & """/>"
                    StringBuilder += Environment.NewLine.ToString + "<invented-name>" & publishForm.Rows(0)("inventedName").ToString & "</invented-name>"
                    If publishForm.Rows(0)("inn") IsNot Nothing AndAlso (Not publishForm.Rows(0)("inn").ToString.Equals("")) Then
                        Dim INNs() As String = publishForm.Rows(0)("inn").ToString.Split(",")
                        For iInn As Integer = 0 To INNs.Length - 1
                            StringBuilder += Environment.NewLine.ToString + "<inn>" & INNs(iInn).Trim() & "</inn>"
                        Next iInn
                    End If
                    StringBuilder += Environment.NewLine.ToString + "<sequence>" & publishForm.Rows(0)("seqNumber").ToString & "</sequence>"
                    If publishForm.Rows(0)("relatedSeqNumber") IsNot DBNull.Value AndAlso (Not publishForm.Rows(0)("relatedSeqNumber").ToString.Equals("")) Then
                        Dim relatedSeqs() As String = publishForm.Rows(0)("relatedSeqNumber").ToString.Split(",")
                        For iSeq As Integer = 0 To relatedSeqs.Length - 1
                            StringBuilder += Environment.NewLine.ToString + "<related-sequence>" & relatedSeqs(iSeq).Trim() & "</related-sequence>"
                        Next iSeq
                    End If
                    StringBuilder += Environment.NewLine.ToString + "<submission-description>" & publishForm.Rows(0)("submissionDescription").ToString & "</submission-description>"
                    StringBuilder += Environment.NewLine.ToString + "</envelope>"
                    StringBuilder += Environment.NewLine.ToString

                End If
                If publishForm.Rows(0)("procedureType").ToString.ToUpper = "mutual-recognition".ToString.ToUpper OrElse publishForm.Rows(0)("procedureType").ToString.ToUpper = "decentralised".ToUpper Then
                    For i As Integer = 0 To cmsdtl.Rows.Count - 1
                        StringBuilder += Environment.NewLine.ToString + "<envelope country=""" & cmsdtl.Rows(i)("countryCodeName").ToString.ToLower() & """>"
                        StringBuilder += Environment.NewLine.ToString + "<application>"
                        If publishForm.Rows(0)("applicationNumber") IsNot Nothing AndAlso (Not publishForm.Rows(0)("applicationNumber").Equals("")) Then
                            Dim appNums() As String = publishForm.Rows(0)("applicationNumber").ToString.Split(",")
                            For iNum As Integer = 0 To appNums.Length - 1
                                StringBuilder += Environment.NewLine.ToString + "<number>" & appNums(iNum).Replace("-", "/").Trim() & "</number>"
                            Next iNum
                        End If
                        StringBuilder += Environment.NewLine.ToString + "</application>"
                        StringBuilder += Environment.NewLine.ToString + "<applicant>" & publishForm.Rows(0)("applicant").ToString & "</applicant>"
                        StringBuilder += Environment.NewLine.ToString + "<agency code=" & """" & cmsdtl.Rows(i)("agencyName").ToString & """" & " />"
                        If publishForm.Rows(0)("atc") IsNot Nothing AndAlso (Not publishForm.Rows(0)("atc").Equals("")) Then
                            Dim atcCodes() As String = publishForm.Rows(0)("atc").ToString.Split(",")
                            For iAtc As Integer = 0 To atcCodes.Length - 1
                                StringBuilder += Environment.NewLine.ToString + "<atc>" & atcCodes(iAtc).Trim() & "</atc>"
                            Next iAtc
                        End If
                        StringBuilder += Environment.NewLine.ToString + "<submission type=""" & publishForm.Rows(0)("submissionType_eu").ToString & """/>"
                        StringBuilder += Environment.NewLine.ToString + "<procedure type=""" & publishForm.Rows(0)("procedureType").ToString & """/>"
                        StringBuilder += Environment.NewLine.ToString + publishForm.Rows(0)("inventedName").ToString + "</invented-name>"
                        If publishForm.Rows(0)("inn") IsNot Nothing AndAlso (Not publishForm.Rows(0)("inn").Equals("")) Then
                            Dim INNs() As String = publishForm.Rows(0)("inn").ToString.Split(",")
                            For iInn As Integer = 0 To INNs.Length - 1
                                StringBuilder += Environment.NewLine.ToString + "<inn>" & INNs(iInn).Trim() & "</inn>"
                            Next iInn
                        End If
                        StringBuilder += Environment.NewLine.ToString + "<sequence>" & publishForm.Rows(0)("seqNumber").ToString & "</sequence>"
                        If publishForm.Rows(0)("relatedSeqNumber") IsNot DBNull.Value AndAlso (Not publishForm.Rows(0)("relatedSeqNumber").Equals("")) Then
                            Dim relatedSeqs() As String = publishForm.Rows(0)("relatedSeqNumber").ToString.Split(",")
                            For iSeq As Integer = 0 To relatedSeqs.Length - 1
                                StringBuilder += Environment.NewLine.ToString + "<related-sequence>" & relatedSeqs(iSeq).Trim() & "</related-sequence>"
                            Next iSeq
                        End If
                        StringBuilder += Environment.NewLine.ToString + "<submission-description>" & publishForm.Rows(0)("submissionDescription").ToString & "</submission-description>"
                        StringBuilder += Environment.NewLine.ToString + "</envelope>"
                        StringBuilder += Environment.NewLine.ToString
                    Next i
                End If
                StringBuilder += Environment.NewLine.ToString + "</eu-envelope>"
            End If

            If stype.Equals("us") Then

                StringBuilder += "<?xml version=""1.0"" encoding=""US-ASCII"" standalone=""no""?>"
                StringBuilder += Environment.NewLine.ToString + "<!DOCTYPE fda-regional:fda-regional SYSTEM ""../../util/dtd/us-regional-v2-01.dtd"">"
                StringBuilder += Environment.NewLine.ToString + "<?xml-stylesheet type=""text/xsl"" href=""../../util/style/us-regional.xsl""?>"
                StringBuilder += Environment.NewLine.ToString + "<fda-regional:fda-regional xmlns:fda-regional=""http://www.ich.org/fda"" dtd-version=""2.01"" xmlns:xlink=""http://www.w3c.org/1999/xlink"">"
                StringBuilder += Environment.NewLine.ToString


                StringBuilder += Environment.NewLine.ToString + "<admin>"
                StringBuilder += Environment.NewLine.ToString + "<applicant-info>"
                StringBuilder += Environment.NewLine.ToString + "<company-name>" & publishForm.Rows(0)("companyName").ToString & "</company-name>"

                StringBuilder += Environment.NewLine.ToString + "<date-of-submission><date format=""yyyymmdd"">" + Me.dos.ToString("yyyyMMdd") + "</date></date-of-submission>"
                StringBuilder += Environment.NewLine.ToString + "</applicant-info>"
                StringBuilder += Environment.NewLine.ToString + "<product-description>"
                StringBuilder += Environment.NewLine.ToString + "<application-number>" & publishForm.Rows(0)("applicationNumber").ToString & "</application-number>"
                StringBuilder += Environment.NewLine.ToString + "<prod-name type=""" & publishForm.Rows(0)("productType").ToString & """>" & publishForm.Rows(0)("productName").ToString & "</prod-name>"
                StringBuilder += Environment.NewLine.ToString + "</product-description>"
                StringBuilder += Environment.NewLine.ToString + "<application-information application-type=""" & publishForm.Rows(0)("applicationType").ToString & """>"
                StringBuilder += Environment.NewLine.ToString + "<submission submission-type=""" & publishForm.Rows(0)("submissionType_us").ToString & """>"
                StringBuilder += Environment.NewLine.ToString + "<sequence-number>" & publishForm.Rows(0)("seqNumber") & "</sequence-number>"

                If publishForm.Rows(0)("relatedSeqNumber") IsNot DBNull.Value AndAlso (Not publishForm.Rows(0)("relatedSeqNumber").Equals("")) Then
                    Dim allRelatedSequences() As String = publishForm.Rows(0)("relatedSeqNumber").ToString.Split(",")
                    For i As Integer = 0 To allRelatedSequences.Length - 1
                        StringBuilder += Environment.NewLine.ToString + "<related-sequence-number>" & allRelatedSequences(i) & "</related-sequence-number>"
                    Next i
                End If
                StringBuilder += Environment.NewLine.ToString + "</submission>"
                StringBuilder += Environment.NewLine.ToString + "</application-information>"
                StringBuilder += Environment.NewLine.ToString + "</admin>"
                StringBuilder += Environment.NewLine.ToString


            ElseIf stype.Equals("without") Then
                StringBuilder += "<?xml version=""1.0"" encoding=""ISO-8859-1""?>"
                StringBuilder += Environment.NewLine.ToString + "<!DOCTYPE ectd:ectd SYSTEM ""util/dtd/ich-ectd-3-0.dtd"">"
                StringBuilder += Environment.NewLine.ToString + "<?xml-stylesheet type=""text/xsl"" href=""util/style/ectd-2-0.xsl""?>"
                StringBuilder += Environment.NewLine.ToString + "<ectd:ectd xmlns:ectd=""http://www.ich.org/ectd"" xmlns:xlink=""http://www.w3c.org/1999/xlink"" dtd-version=""3.00"">"

            ElseIf stype.Equals("eum2-m5") OrElse stype.Equals("usm2-m5") Then
                StringBuilder += "<?xml version=""1.0"" encoding=""ISO-8859-1""?>"
                StringBuilder += Environment.NewLine.ToString + "<!DOCTYPE ectd:ectd SYSTEM ""util/dtd/ich-ectd-3-2.dtd"">"
                StringBuilder += Environment.NewLine.ToString + "<?xml-stylesheet type=""text/xsl"" href=""util/style/ectd-2-0.xsl""?>"
                StringBuilder += Environment.NewLine.ToString + "<ectd:ectd xmlns:ectd=""http://www.ich.org/ectd"" xmlns:xlink=""http://www.w3c.org/1999/xlink"" dtd-version=""3.2"">"
                StringBuilder += Environment.NewLine.ToString + "<m1-administrative-information-and-prescribing-information>"

                Dim ENC As New UTF8Encoding()
                If stype.Equals("eum2-m5") Then
                    Dim eumd5 As String = clsCommon.CalculateChecksum(absolutePathToCreate & "/m1/eu" & "/eu-regional.xml").ToString
                    StringBuilder += Environment.NewLine.ToString + "<leaf xml:lang=""en"" checksum-type=""md5"" operation=""new"" application-version="""" xlink:href=""m1/eu/eu-regional.xml"" checksum=""" & eumd5 & """ ID=""node-999"" xlink:type=""simple"">"
                Else
                    Dim usmd5 As String = clsCommon.CalculateChecksum(absolutePathToCreate & "/m1/us" & "/us-regional.xml").ToString
                    StringBuilder += Environment.NewLine.ToString + "<leaf xml:lang=""en"" checksum-type=""md5"" operation=""new"" application-version="""" xlink:href=""m1/us/us-regional.xml"" checksum=""" & usmd5 & """ ID=""node-999"" xlink:type=""simple"">"
                End If
                StringBuilder += "<title>"
                If stype.Equals("eum2-m5") Then
                    StringBuilder += "EU Module 1"
                ElseIf stype.Equals("usm2-m5") Then
                    StringBuilder += "m1-administrative-information-and-prescribing-information"
                End If

                StringBuilder += "</title>"
                StringBuilder += "</leaf>"
                StringBuilder += "</m1-administrative-information-and-prescribing-information>" + Environment.NewLine

            End If

            Try
                xmlwriter.WriteString(StringBuilder)

            Catch ex As Exception

            End Try
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '     The End on 20-Aug-2011 by Bharat Patel
            '
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while WriteToXmlFile"
        End Try
    End Sub 'main class end

#Region "getChildNode"
    Public Sub getChildNode(ByVal childNodes As DataTable, ByVal pathToCreate As String, _
                            ByVal parentId As Integer, ByVal stype As String, _
                            ByVal publishForm As DataTable, ByRef eStr_Retu As String)

        Dim wStr As String = String.Empty
        Retu_Str = String.Empty
        Dim dr As DataRow = Nothing
        Dim drmodifiedFile As DataRow = Nothing
        Dim cntNodeID = childNodes.Rows.Count
        Dim dr_STF As DataRow = Nothing

        Try
            eStr_Retu = ""
            If cntNodeID = 0 Then


                Dim rows() As DataRow = AllNodesofHistory.Select("inodeid = " + nodeId.ToString)
                If rows.Length > 0 Then

                    'if file attached at node or its parent node

                    xmlwriter.WriteStartElement("leaf")
                    Dim modifiedFile As DataTable = Nothing

                    'In case of Manual Mode Publish
                    If projectPublishType = "M" Then

                        wStr = "WorkspaceId ='" + wsId + "' AND NodeId = " + nodeId + " AND AttrForIndiId='0001' AND TranNo = ( SELECT MAX(TranNo) AS TranNo FROM view_NodeVersionHistoryDetail WHERE WorkspaceId ='" + wsId + "' AND NodeId = " + nodeId + "  GROUP BY WorkspaceId,NodeId ) AND AttrName <> 'FileLastModified'"
                        'childAttrId = docMgmtInt.getLatestNodeAttrHistory(wsId, nodeId, "0001")
                        If Not objHelpDb.getLatestNodeAttrHistory(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                  childAttrId, Retu_Str) Then
                            eStr_Retu += vbCrLf + "Error occured while retrieving view_NodeVersionHistoryDetail, " + Retu_Str
                            Exit Sub
                        End If

                        'Dim dtoSub As New DTOSubmittedWorkspaceNodeDetail()
                        'dtoSub.lastPublishVersion = request.getParameter("refSeq_" & nodeId)
                        'dtoSub.IndexId = request.getParameter("refID_" & nodeId)
                        'modifiedFile = New ArrayList()
                        'modifiedFile.Add(dtoSub)
                    Else
                        'childAttrId = docMgmtInt.getAttributesForNodeForPublish(nodeId, wsId, publishForm.labelNo, "0001")
                        wStr = nodeId.ToString + "#" + wsId + "#" + publishForm.Rows(0)("labelNo").ToString + "#" + "EL"
                        If Not objHelpDb.getAttributesForNodeForPublish(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                       childAttrId, Retu_Str) Then
                            eStr_Retu += vbCrLf + "Error occured while retrieving Proc_AttributesForNodeForPublish, " + Retu_Str
                            Exit Sub
                        End If

                        Retu_Str = String.Empty
                        wStr = "vworkspaceId=" + wsId + " AND iNodeId=" + nodeId.ToString + ""
                        If Not objHelpDb.getAttributeValueOfModifiedFile(wStr, modifiedFile, Retu_Str) Then
                            eStr_Retu += vbCrLf + "Error occured while retrieving submittedworkspacenodedetail, " + Retu_Str
                            Exit Sub
                        End If
                    End If
                    'hardcoding cover letter and fda form as new
                    If childAttrId.Rows.Count > 0 Then
                        Dim operationValue As String = ""
                        If Regex.IsMatch(pathToCreate, ".*/m1/us/.*cover.*", RegexOptions.IgnoreCase) OrElse _
                            Regex.IsMatch(pathToCreate, ".*/m1/us/.*fda.*", RegexOptions.IgnoreCase) Then
                            For i As Integer = 0 To childAttrId.Rows.Count - 1
                                dr = childAttrId.Rows(i)
                                If dr("vattrName").ToString.ToUpper.Equals("operation".ToUpper) Then
                                    dr("vattrValue") = "new"
                                    Console.WriteLine("pathToCreate" & pathToCreate)
                                End If
                            Next i
                        End If
                        For i As Integer = 0 To childAttrId.Rows.Count - 1
                            dr = childAttrId.Rows(i)
                            attrId = dr("iAttrId")
                            attrValue = dr("vattrValue")
                            attrName = dr("vattrName")

                            'In case of Manual Mode
                            'If attrName.Equals("operation") AndAlso projectPublishType = "M" Then
                            '    attrValue = request.getParameter("operation_" & nodeId)
                            'End If

                            If attrName.ToUpper.Equals("operation".ToUpper) AndAlso attrValue.ToUpper.Equals("new".ToUpper) Then
                                xmlwriter.WriteAttributeString(attrName, attrValue.Trim())
                                'xmlwriter.writeAttribute("modified-file","");
                            ElseIf attrName.ToUpper.Equals("operation".ToUpper) AndAlso (Not attrValue.Equals("new".ToUpper)) Then
                                Dim query As New StringBuilder()
                                For j As Integer = 0 To modifiedFile.Rows.Count - 1
                                    drmodifiedFile = modifiedFile.Rows(j)
                                    Dim lastPubVersion As String = drmodifiedFile("vLastPublishVersion")
                                    Dim indexId As String = drmodifiedFile("indexId")

                                    ' IMP Change done By Ashmak Shah 
                                    If pathToCreate.Contains("m1/eu") Then
                                        query.Append("../../../")
                                        query.Append("" & lastPubVersion & "")
                                        query.Append("/m1/eu/eu-regional.xml#")
                                    ElseIf pathToCreate.Contains("m1/us") Then
                                        query.Append("../../../")
                                        query.Append("" & lastPubVersion & "")
                                        query.Append("/m1/us/us-regional.xml#")
                                    Else
                                        query.Append("../")
                                        query.Append("" & lastPubVersion & "")
                                        query.Append("/index.xml#")
                                    End If

                                    query.Append("" & indexId & "")
                                    xmlwriter.WriteAttributeString(attrName, attrValue.Trim())
                                    xmlwriter.WriteAttributeString("modified-file", query.ToString())
                                Next j

                                If attrValue.Equals("delete") Then
                                    operationValue = attrValue
                                End If
                            End If

                            If attrName.ToUpper.Equals("xlink:href".ToUpper) Then
                                If operationValue.Equals("delete") Then
                                    xmlwriter.WriteAttributeString("xlink:href", "")
                                    xmlwriter.WriteAttributeString("checksum", "")
                                    Dim IDValue As String = "node-" & nodeId.ToString()
                                    xmlwriter.WriteAttributeString("ID", IDValue)
                                Else
                                    attrValue = copyFileforPublish(publishForm, pathToCreate, eStr_Retu)
                                    attrValue = attrValue.ToLower()
                                    stype = stype.ToLower()

                                    'Change for New Submission Path
                                    'String FilePathForCheckSum = publishDestFolderName + File.separator + publishForm.getApplicationNumber() +"/"+ publishForm.getSeqNumber() + "/"+attrValue;
                                    Dim FilePathForCheckSum As String = publishDestFolderName + "/" & "/" & publishForm.Rows(0)("seqNumber").ToString & "/" & attrValue

                                    attrValue = attrValue.Replace("m1/" & stype & "/", "")
                                    attrValue = attrValue.Replace("//", "/")
                                    AddAttrib(attrName, attrValue.Trim, eStr_Retu)

                                    Dim md5HashCodeforFile As String = clsCommon.CalculateChecksum(FilePathForCheckSum)
                                    xmlwriter.WriteAttributeString("checksum", md5HashCodeforFile)
                                    Dim IDValue As String = "node-" & nodeId.ToString()
                                    xmlwriter.WriteAttributeString("ID", IDValue)
                                End If
                            ElseIf (Not attrName.Equals("xlink:href")) AndAlso _
                                   (Not attrName.Equals("operation")) AndAlso _
                                   (Not attrName.Equals("modified-file")) AndAlso _
                                   (Not attrName.Equals("checksum")) AndAlso _
                                   (Not attrName.Equals("Keywords")) AndAlso _
                                   (Not attrName.Equals("Author")) AndAlso _
                                   (Not attrName.Equals("Description")) AndAlso (Not attrName.Equals("ID")) Then

                                If attrName.IndexOf(":") > 1 Then
                                    AddAttrib(attrName, attrValue.Trim, eStr_Retu)
                                Else
                                    xmlwriter.WriteAttributeString(attrName, attrValue.Trim())
                                End If
                            End If
                        Next i
                    End If
                    xmlwriter.WriteStartElement("title")
                    xmlwriter.WriteString(nodeDisplayName.Trim())
                    xmlwriter.WriteEndElement()
                    xmlwriter.WriteEndElement() 'end entity for <leaf>

                    'if end of history check
                End If
            Else
                Dim MergeAttributeStr As String = ""
                Dim dtLeafNode As DataTable = Nothing
                Retu_Str = String.Empty
                For i As Integer = 0 To childNodes.Rows.Count - 1
                    dr = childNodes.Rows(i)
                    nodeId = dr("iNodeId").ToString
                    nodeName = dr("vNodeName").ToString
                    nodeDisplayName = dr("vNodeDisplayName").ToString
                    Dim filepathelement As String = dr("vFolderName").ToString
                    Dim remark As String = dr("vRemark").ToString.Trim
                    'if file attached at node or its parent node	
                    Dim rows() As DataRow = AllNodesofHistory.Select("inodeid = " + nodeId.ToString)
                    If rows.Length > 0 Then
                        wStr = "vWorkspaceId='" + wsId + "' and iParentNodeId=" + nodeId.ToString
                        Dim isLeaf As Integer
                        If Not objHelpDb.isLeafNodes(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, isLeaf, Retu_Str) Then
                            eStr_Retu += vbCrLf + "Error occured while isLeafNodes, " + Retu_Str
                            Exit Sub
                        End If

                        nodetypeindi = dr("cNodeTypeIndi").ToString
                        If isLeaf = 0 Then
                            If nodetypeindi.ToUpper.Equals("T") Then
                                Dim intchar As Integer = filepathelement.IndexOf(".")
                                Dim finalstr As String
                                If intchar <> -1 Then
                                    finalstr = filepathelement.Substring(0, intchar)
                                Else
                                    finalstr = filepathelement
                                End If
                                createSubFolders(pathToCreate, finalstr, eStr_Retu)
                            Else
                                createSubFolders(pathToCreate, filepathelement, eStr_Retu)
                            End If
                        End If
                        If isLeaf = 0 Then
                            If nodetypeindi.ToUpper.Equals("T") Then
                                dr_STF = STFXMLLocation.NewRow
                                dr_STF("iNodeId") = nodeId.ToString

                                'As we are adding stf on leaf node folder name of the leaf node would be abc.pdf 
                                'so we need to remove .pdf extension 
                                Dim intchar As Integer = filepathelement.IndexOf(".")
                                Dim finalstr As String
                                If intchar <> -1 Then
                                    finalstr = filepathelement.Substring(0, intchar)
                                Else
                                    finalstr = filepathelement
                                End If


                                dr_STF("vBaseWorkFolder") = pathToCreate & "/" & finalstr

                                STFXMLLocation.Rows.Add(dr_STF)
                                generateSTFFile(wsId, eStr_Retu)
                            End If
                        End If
                        If Not nodeName.Trim().Equals("m1-administrative-information-and-prescribing-information") Then
                            If isLeaf = 0 Then
                                If (Not nodetypeindi.ToUpper.Trim.Equals("T")) AndAlso (Not nodetypeindi.ToUpper.Trim().Equals("F")) Then
                                    '<summary>
                                    '&& !nodetypeindi.trim().equals("F")'  is added by Ashmak Shah
                                    '
                                    'In case of nodes like 'CRFs' under STF no parent node will create like '<case-report-forms>' in index.xml
                                    'All crfs will be displayed with other stf nodes 
                                    '</summary>
                                    'System.out.println("node name : " + nodeName.trim() + " NodeTypeIndi: " + nodetypeindi.trim());
                                    'change by hardik (For Node-Extension)

                                    If nodetypeindi.Trim().Equals("E") Then
                                        xmlwriter.WriteStartElement("node-extension")

                                        xmlwriter.WriteString("title")
                                        xmlwriter.WriteString(remark)
                                        xmlwriter.WriteEndElement() 'title
                                    Else
                                        xmlwriter.WriteStartElement(nodeName.Trim())
                                    End If

                                    ' <summary>
                                    ' For nodeTypeIndi = 'P' </summary>

                                    Dim country As String = "", language As String = ""
                                    Dim PathForAttrFolder As String = ""
                                    Dim nodeAttribute As DataTable = Nothing
                                    wStr = "vWorkspaceId='" + wsId + "' and cAttrForIndi = 'ES'"
                                    If Not nodeId = -1 Then
                                        wStr += "and iNodeId=" + nodeId.ToString

                                    End If
                                    If Not objHelpDb.getNodeAttributes(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, nodeAttribute, Retu_Str) Then
                                        eStr_Retu += vbCrLf + "Error occured while retrieving workspaceNodeAttrDetail, " + Retu_Str
                                        Exit Sub
                                    End If
                                    'Dim nodeAttribute As ArrayList = docMgmtInt.getNodeAttributes(wsId, nodeId)

                                    If nodeAttribute.Rows.Count > 0 Then
                                        For k As Integer = 0 To nodeAttribute.Rows.Count - 1
                                            drmodifiedFile = nodeAttribute.Rows(k)
                                            Dim attrname As String = drmodifiedFile("vAttrName")
                                            Dim attrvalue As String = drmodifiedFile("vAttrValue")

                                            '<summary>
                                            'For nodeTypeIndi = 'P' 
                                            ' </summary>
                                            If attrname.ToUpper() = "country".ToUpper() Then
                                                country = attrvalue
                                            End If
                                            If attrname.ToUpper() = "xml:lang".ToUpper() Then
                                                language = attrvalue
                                            End If

                                            If attrvalue IsNot Nothing AndAlso (Not attrvalue.Equals("")) Then
                                                If attrname.IndexOf(":") > 1 Then
                                                    AddAttrib(attrname, attrvalue.Trim, eStr_Retu)
                                                Else
                                                    xmlwriter.WriteAttributeString(attrname, attrvalue)
                                                End If
                                            End If

                                            'remove space and give - in attribute value
                                            If attrvalue.Length > 4 AndAlso (Not attrvalue.ToUpper() = "common".ToUpper()) Then
                                                Console.WriteLine("attrvalue: " & attrvalue)
                                                attrvalue = attrvalue.Trim().Replace(" ", "-")
                                                attrvalue = attrvalue.Trim().Replace("\.", "-")
                                                attrvalue = attrvalue.Trim().Replace(",", "-")
                                                attrvalue = attrvalue.Substring(0, 4)
                                                Console.WriteLine("attrvalue substring: " & attrvalue)
                                            Else
                                                attrvalue = attrvalue.Trim().Replace(" ", "-")
                                            End If

                                            If k = 0 Then
                                                MergeAttributeStr = attrvalue.ToLower()
                                            Else
                                                MergeAttributeStr = MergeAttributeStr & "-" & attrvalue.ToLower()
                                            End If
                                        Next k

                                        '<summary>
                                        'nodeTypeIndi = 'P' (pi-doc)
                                        'Description : Folder Name will be 'country name'
                                        'Sub folder Name will be 'language'
                                        '</summary>
                                        If nodetypeindi.Trim().Equals("P") Then
                                            PathForAttrFolder = pathToCreate & "/" & country
                                            MergeAttributeStr = language
                                            '<summary>
                                            'nodeTypeIndi = 'D' (No Folder to be created)
                                            'Description : Folder will be created from attribute values
                                            'But not from 'vFolderName'
                                            '</summary>
                                        ElseIf nodetypeindi.Trim().Equals("D") Then
                                            PathForAttrFolder = pathToCreate
                                            'No change in 'MergeAttributeStr'
                                            '<summary>
                                            'nodeTypeIndi = 'N' (normal node)
                                            'Description : Folder will be created from vFolderName
                                            '</summary>
                                        Else
                                            PathForAttrFolder = pathToCreate & "/" & filepathelement
                                        End If
                                        Console.WriteLine("PathForAttrFolder:" & PathForAttrFolder)
                                        createSubFolders(PathForAttrFolder, MergeAttributeStr, eStr_Retu)
                                        'parentNodes = docMgmtInt.getChildNodeByParent(nodeId, wsId)
                                        wStr = "vWorkspaceId = '" + wsId + "' and iParentNodeId =" + nodeId.ToString
                                        If Not objHelpDb.getChildNodeByParent(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, parentNodes, Retu_Str) Then
                                            eStr_Retu += vbCrLf + "Error occured while retrieving GetChildNodeByParent, " + Retu_Str
                                            Exit Sub
                                        End If
                                        'Here, inodeid passed as a parameter in function will be parentnodeid
                                        getChildNode(parentNodes, relativePathToCreate, nodeId, stype, publishForm, eStr_Retu)
                                        xmlwriter.WriteEndElement()
                                    Else
                                        ' parentNodes = docMgmtInt.getChildNodeByParent(nodeId, wsId)
                                        wStr = "vWorkspaceId = '" + wsId + "' and iParentNodeId =" + nodeId.ToString
                                        If Not objHelpDb.getChildNodeByParent(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, parentNodes, Retu_Str) Then
                                            eStr_Retu += vbCrLf + "Error occured while retrieving GetChildNodeByParent, " + Retu_Str
                                            Exit Sub
                                        End If
                                        'same query for workspacenodedetail as above
                                        getChildNode(parentNodes, relativePathToCreate, nodeId, stype, publishForm, eStr_Retu)
                                        xmlwriter.WriteEndElement()
                                    End If
                                Else
                                    'parentNodes = docMgmtInt.getChildNodeByParent(nodeId, wsId)
                                    wStr = "vWorkspaceId = '" + wsId + "' and iParentNodeId =" + nodeId.ToString
                                    If Not objHelpDb.getChildNodeByParent(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, parentNodes, Retu_Str) Then
                                        eStr_Retu += vbCrLf + "Error occured while retrieving GetChildNodeByParent, " + Retu_Str
                                        Exit Sub
                                    End If
                                    'same query for workspacenodedetail as above
                                    getChildNode(parentNodes, relativePathToCreate, nodeId, stype, publishForm, eStr_Retu)
                                End If
                            Else
                                'parentNodes = docMgmtInt.getChildNodeByParent(nodeId, wsId)
                                wStr = "vWorkspaceId = '" + wsId + "' and iParentNodeId =" + nodeId.ToString
                                If Not objHelpDb.getChildNodeByParent(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, parentNodes, Retu_Str) Then
                                    eStr_Retu += vbCrLf + "Error occured while retrieving GetChildNodeByParent, " + Retu_Str
                                    Exit Sub
                                End If
                                'same query for workspacenodedetail as above
                                getChildNode(parentNodes, relativePathToCreate, nodeId, stype, publishForm, eStr_Retu)
                            End If
                        Else
                            'parentNodes = docMgmtInt.getChildNodeByParent(nodeId, wsId)
                            wStr = "vWorkspaceId = '" + wsId + "' and iParentNodeId =" + nodeId.ToString
                            If Not objHelpDb.getChildNodeByParent(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, parentNodes, Retu_Str) Then
                                eStr_Retu += vbCrLf + "Error occured while retrieving GetChildNodeByParent, " + Retu_Str
                                Exit Sub
                            End If
                            'same query for workspacenodedetail as above
                            getChildNode(parentNodes, relativePathToCreate, nodeId, stype, publishForm, eStr_Retu)
                        End If
                        'Added by Ashmak Shah
                        'STF Nodes After CRFs were copied into the crf folder to avoid this below line is added.

                        relativePathToCreate = pathToCreate
                    End If 'if end of history vector

                Next i 'for loop end
            End If 'main else end

        Catch ex As Exception
            ex.StackTrace.ToCharArray()
        End Try
    End Sub
#End Region

    Private Sub UpdateXMLFile(ByVal path As String, ByRef eStr_Retu As String)
        Dim xmlString As String = String.Empty
        Try
            eStr_Retu = ""
        
            Using downloader As New Net.WebClient()

                Using reader As TextReader = New StreamReader(downloader.OpenRead(path))
                    xmlString = reader.ReadToEnd()
                End Using
            End Using

            If xmlString.Contains("&lt;") Then
                xmlString = xmlString.Replace("&lt;", "<")
            End If

            If xmlString.Contains("&gt;") Then
                xmlString = xmlString.Replace("&gt;", ">")
            End If

            If xmlString.Contains("href = ""m") Then
                xmlString = xmlString.Replace("href = ""m", "xlink:href = ""m")
            End If


            Using downloader As New Net.WebClient()

                Using Write As TextWriter = New StreamWriter(downloader.OpenWrite(path))
                    Write.Write(xmlString)
                End Using
            End Using
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while UpdateXmlFile"
        End Try
    End Sub

    Private Sub AddAttrib(ByVal attrName As String, ByVal attrValue As String, ByRef eStr_Retu As String)
        Dim doc As New XmlDocument
        Dim Attribute As XmlAttribute
        Dim namespaceURI As String = String.Empty

        Try
            eStr_Retu = ""
            If attrName.Split(":")(0) = "xmlns" Then
                namespaceURI = "http://www.w3.org/2000/xmlns/"
            ElseIf attrName.Split(":")(0) = "xml" Then
                namespaceURI = "http://www.w3.org/XML/1998/namespace"
            ElseIf attrName.Split(":")(0) = "xlink" Then
                namespaceURI = "http://www.w3c.org/1999/xlink"
            End If

            Attribute = doc.CreateAttribute(attrName.Split(":")(0), attrName.Split(":")(1), namespaceURI)

            Attribute.Value = attrValue.Trim

            Attribute.WriteTo(xmlwriter)

        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while AddAttrib"
        Finally
            doc = Nothing
        End Try
    End Sub

End Class