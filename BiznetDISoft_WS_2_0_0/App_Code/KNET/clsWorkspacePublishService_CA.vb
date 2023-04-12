Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Xml

Public Class clsWorkspacePublishService_CA
    Private objHelpDb As clsHelpDB

    Private xmlwriter As XmlWriter = Nothing

    Private childNodeForParent As DataTable = Nothing
    Private childAttrForNode As DataTable = Nothing
    Private parentNodes As DataTable = Nothing
    Private childAttrId As DataTable = Nothing
    Private attrDetail As DataTable = Nothing
    Private attrNameForNode As DataTable = Nothing
    Private STFXMLLocation As DataTable = Nothing
    Private pathlst As DataTable = Nothing
    Private trannolst As DataTable = Nothing
    Private AllNodesofHistory As DataTable = Nothing
    Private fileNameDtl As DataTable = Nothing

    Private md5 As String = String.Empty
    Private wsId As String = String.Empty
    Private workspaceDesc As String = String.Empty
    Private folderName As String = String.Empty
    Private Retu_Str As String = String.Empty
    Private newPath As String = String.Empty
    Private fileName As String = String.Empty
    Private folderStructure As String = String.Empty

    Private publishDestFolderName As String = String.Empty
    Private absolutePathToCreate As String = String.Empty
    Private sourceFolderName As String = String.Empty
    Private LastPublishedVersion As String = String.Empty
    Private currentSequenceNumber As String = String.Empty
    Public projectPublishType As String = String.Empty
    Private relativePathToCreate As String = String.Empty
    Private baseWorkFolder As String = String.Empty
    Private attrValue As String = String.Empty
    Private attrName As String = String.Empty

    Private nodeDisplayName As String = String.Empty
    Private nodeName As String = String.Empty
    Private nodetypeindi As String = String.Empty

    Public leafIds() As Integer = Nothing
    Private iParentId As Integer = Nothing
    Private nodeId As Integer = Nothing
    Private attrId As Integer = Nothing
    Private upCount As Integer = 0

    Dim dos As Date

    Friend Function workspacePublish(ByVal workspaceId As String, _
                                     ByRef publishForm As DataTable, _
                                     ByVal userId As String, _
                                     ByVal dos As Date, _
                                     ByVal wsDesc As String, _
                                     ByVal baseWorkFolder As String, _
                                     ByRef eStr_Retu As String) As Boolean

        Dim parentDir As DirectoryInfo = Nothing
        Dim out As FileStream = Nothing
        Dim WriterSettings As XmlWriterSettings

        Dim Dtl_getChildNodeByParentForPublishForM1 As New DataTable

        Dim labelNo As Integer
        Dim stype As String = String.Empty
        Dim eStr As String = String.Empty
        Dim leafIdsString As String = String.Empty
        Dim query As String = String.Empty


        Try
            workspacePublish = False
            eStr_Retu = ""
            objHelpDb = New clsHelpDB

            wsId = workspaceId
            workspaceDesc = wsDesc
            Me.dos = dos
            Me.baseWorkFolder = baseWorkFolder

            labelNo = publishForm.Rows(0)("LabelNo")
            userId = userId


            'Change for New Submission Path
            publishDestFolderName = publishForm.Rows(0)("PublishDestinationPath")

            createBaseFolder(publishDestFolderName, publishForm, eStr_Retu)

            stype = publishForm.Rows(0)("SubmissionFlag")
            currentSequenceNumber = publishForm.Rows(0)("SeqNumber")


            If projectPublishType = "P" Then
                'get all nodes and its parent nodes where file attached
                If Not objHelpDb.getAllNodesFromHistoryForRevisedSubmission(workspaceId + "#" + _
                                                                            labelNo.ToString + "#", AllNodesofHistory, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving Proc_WorkSpaceNodeForRevisedSubmission_Doc, " + eStr
                    Exit Function
                End If
            ElseIf projectPublishType = "M" Then

                For Each id As Integer In leafIds
                    leafIdsString += id.ToString() + ","
                Next id

                If Not leafIdsString = String.Empty Then
                    If Not objHelpDb.getWorkspaceTreeNodesForLeafs(workspaceId + "#" + _
                                                                   leafIdsString.Remove(leafIdsString.Length - 1) + "#", _
                                                                   AllNodesofHistory, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while retrieving Proc_WorkspaceTreeNodesForLeafs, " + eStr
                        Exit Function
                    End If
                End If
            End If

            iParentId = 1
            parentDir = New DirectoryInfo(absolutePathToCreate + "/m1/" + stype + "/")
            If Not parentDir.Exists Then
                parentDir.Create()
            End If

            out = File.Create(absolutePathToCreate + "/m1/" + stype + "/" + stype + "-regional.xml")
            WriterSettings = New XmlWriterSettings()
            WriterSettings.ConformanceLevel = ConformanceLevel.Fragment
            xmlwriter = System.Xml.XmlWriter.Create(out, WriterSettings)

            writeToXmlFile(stype, publishForm, eStr) 'Develop By Bharat & Suhani

            query = "SELECT vWorkspaceId,iNodeId,vNodeName,vNodeDisplayName," + _
                    "       vFolderName,cNodeTypeIndi,vRemark " + _
                    " FROM workspaceNodeDetail " + _
                    " WHERE iParentNodeId=" + iParentId.ToString + _
                    " and vWorkspaceId='" + workspaceId + "' and iNodeNo= 1"
            If Not objHelpDb.getChildNodeByParentForPublishForM1(query, Dtl_getChildNodeByParentForPublishForM1, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving getChildNodeByParentForPublishForM1, " + eStr
                Exit Function
            End If

            getChildNode(Dtl_getChildNodeByParentForPublishForM1, absolutePathToCreate, iParentId, stype, publishForm, eStr_Retu) 'Int IdValue is removed which is available in JAVA Code.

            xmlwriter.WriteString("</hcsc:ectd>")

            xmlwriter.Flush()
            xmlwriter.Close()
            out.Flush()
            out.Close()

            UpdateXMLFile(absolutePathToCreate + "/m1/" + stype + "/" + stype + "-regional.xml")


            'CODE FOR INDEX.XML
            out = File.Create(absolutePathToCreate + "/index.xml")
            xmlwriter = System.Xml.XmlWriter.Create(out, WriterSettings)

            writeToXmlFile(stype + "m2-m5", publishForm, eStr_Retu)

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


            getChildNode(Dtl_getChildNodeByParentForPublishFromM2toM5, absolutePathToCreate, iParentId, stype, publishForm, eStr) 'Int IdValue is removed which is available in JAVA Code.
            xmlwriter.WriteString("</ectd:ectd>")
            xmlwriter.Flush()
            xmlwriter.Close()
            out.Flush()
            out.Close()

            UpdateXMLFile(absolutePathToCreate + "/index.xml")


            checkSumForindexFile(eStr_Retu)
            addutilFolder(stype, eStr_Retu)

            workspacePublish = True
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while workspacePublish"
            workspacePublish = False
        End Try
    End Function

    Private Sub addutilFolder(ByVal stype As String, ByRef eStr_Retu As String)
        Try
            eStr_Retu = ""
            Dim sourceLocation As DirectoryInfo = Nothing
            Dim targetLocation As DirectoryInfo = Nothing

            sourceLocation = New DirectoryInfo(baseWorkFolder + "/util_ca/util")

            targetLocation = Directory.CreateDirectory(absolutePathToCreate + "/util")

            For Each sDir As DirectoryInfo In sourceLocation.GetDirectories

                If Not Directory.Exists(targetLocation.FullName + "\" + sDir.Name) Then
                    Directory.CreateDirectory(targetLocation.FullName + "\" + sDir.Name)
                End If

                For Each sFile As FileInfo In sDir.GetFiles
                    System.IO.File.Copy(sFile.FullName, targetLocation.FullName + "\" + sDir.Name + "\" + sFile.Name, True)
                Next sFile

            Next sDir

        Catch e As Exception
            eStr_Retu += e.Message + vbCrLf + "Error occured while addutilFolder"
        End Try
    End Sub

    Private Sub createBaseFolder(ByVal bfoldername As String, ByVal publishForm As DataTable, _
                                 ByRef eStr_Retu As String)
        Dim folderDtl As New DataTable
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty

        Try
            wStr = "select Distinct vBaseWorkFolder,vBasePublishFolder,vLastPublishedVersion " + _
                   " from View_CommonWorkspaceDetail " + _
                   " where vworkspaceid='" + wsId + "'"
            If Not objHelpDb.getFolderByWorkSpaceId(wStr, DataRetrievalModeEnum.DatatTable_Query, folderDtl, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving View_CommonWorkspaceDetail, " + eStr
                Exit Sub
            End If


            If folderDtl IsNot Nothing Then
                sourceFolderName = folderDtl.Rows(0)("vBaseWorkFolder").ToString()
                LastPublishedVersion = folderDtl.Rows(0)("vLastPublishedVersion").ToString()
            End If
            'Change for New Submission Path

            absolutePathToCreate = bfoldername & "/" & publishForm.Rows(0)("seqNumber").ToString
            absolutePathToCreate = absolutePathToCreate.Replace("/", "\")

            Console.WriteLine("absolutePathToCreate: " & absolutePathToCreate)
            If Not Directory.Exists(absolutePathToCreate) Then
                Directory.CreateDirectory(absolutePathToCreate)
            End If

        Catch e As Exception
            eStr_Retu += e.Message + vbCrLf + "Error occured while saveSubmissionDtl"
        End Try
    End Sub

    Private Sub createSubFolders(ByVal pathToCreate As String, ByVal folderName As String, ByRef eStr_Retu As String)
        Try
            eStr_Retu = ""
            relativePathToCreate = pathToCreate + "/" + folderName
            If Not Directory.Exists(relativePathToCreate) AndAlso Not relativePathToCreate.Contains("m1/ca") Then
                Directory.CreateDirectory(relativePathToCreate)
            End If
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while CreateSubFolders"
        End Try
    End Sub

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

    Private Function copyFileforPublish(ByVal publishForm As DataTable, ByVal pathToCreate As String, _
                                        ByRef eStr_Retu As String) As String

        Dim absolutePath As String = String.Empty
        Dim absolutePathLink As String = String.Empty
        Dim wStr As String = String.Empty
        Dim nIds As String = String.Empty
        Dim iIds As Integer()

        Try
            Retu_Str = String.Empty
            newPath = String.Empty
            absolutePath = pathToCreate
            absolutePathLink = pathToCreate.Replace(absolutePathToCreate, String.Empty)


            'In case of Manual Mode Publish
            If projectPublishType = "M" Then
                iIds = New Integer() {nodeId}

                For i As Integer = 0 To iIds.Length - 1
                    nIds += iIds(i).ToString + ","
                Next

                If Not nIds = String.Empty Then
                    wStr = "vworkspaceId IN (" + wsId + ") AND inodeId IN (" + nIds.Remove(nIds.Length - 1) + ")"
                    If Not objHelpDb.getAllNodesLastHistory(wStr, fileNameDtl, Retu_Str) Then
                        eStr_Retu += vbCrLf + "Error occured while getAllNodesLastHistory, " + Retu_Str
                        Exit Function
                    End If
                End If
            Else
                wStr = wsId + "#" + nodeId.ToString + "#" + publishForm.Rows(0)("LabelNo").ToString

                If Not objHelpDb.getFileNameForNodeForPublish(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                              fileNameDtl, Retu_Str) Then
                    eStr_Retu += vbCrLf + "Error occured while getFileNameForNodeForPublish, " + Retu_Str
                    Exit Function
                End If

            End If

            If fileNameDtl.Rows.Count > 0 Then

                fileName = fileNameDtl.Rows(0)("vFileName").ToString
                folderStructure = fileNameDtl.Rows(0)("vFolderName").ToString
                folderStructure = (sourceFolderName.Trim() & folderStructure.Trim() & "/" & fileName)

                addFiles(folderStructure, absolutePath, eStr_Retu)

                Return (absolutePathLink.Substring(1) & "/" & fileName)
            Else
                Return (absolutePathLink.Substring(1))
            End If
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while CopyFilesForPublish"
            Return Nothing
        End Try
    End Function

    Private Sub checkSumForindexFile(ByRef eStr_Retu As String)
        Try
            Dim indexmd5 As String = clsCommon.CalculateChecksum(absolutePathToCreate & "/index.xml")
            Dim indexHashFileout As New StreamWriter(absolutePathToCreate & "/index-md5.txt")

            indexHashFileout.Write(indexmd5)
            indexHashFileout.Close()
            indexHashFileout = Nothing
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while checkSumForindexFile"
        End Try
    End Sub

#Region " getChildNode "
    Public Sub getChildNode(ByVal childNodes As DataTable, ByVal pathToCreate As String, _
                            ByVal parentId As Integer, ByVal stype As String, _
                            ByVal publishForm As DataTable, _
                            ByRef eStr_Retu As String)

        Dim parentNodeDetail As DataTable = Nothing
        Dim dr As DataRow = Nothing
        Dim drmodifiedFile As DataRow = Nothing
        Dim dr_STF As DataRow = Nothing

        Dim isM1AdminNode As Boolean = False
        Dim cntNodeID As Integer = childNodes.Rows.Count

        Dim wStr As String = String.Empty
        Dim Retu_Str As String = String.Empty

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

                        If Not objHelpDb.getLatestNodeAttrHistory(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                  childAttrId, Retu_Str) Then
                            eStr_Retu += vbCrLf + "Error occured while view_NodeVersionHistoryDetail, " + Retu_Str
                            Exit Sub
                        End If
                        'DTOSubmittedWorkspaceNodeDetail dtoSub= new DTOSubmittedWorkspaceNodeDetail();
                        'dtoSub.setLastPublishVersion(request.getParameter("refSeq_"+nodeId));
                        'dtoSub.setIndexId(request.getParameter("refID_"+nodeId));
                        'modifiedFile = new Vector();
                        'modifiedFile.add(dtoSub);
                    Else
                        wStr = nodeId.ToString + "#" + wsId + "#" + publishForm.Rows(0)("labelNo").ToString + "#" + "EL"
                        If Not objHelpDb.getAttributesForNodeForPublish(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                       childAttrId, Retu_Str) Then
                            eStr_Retu += vbCrLf + "Error occured while Proc_AttributesForNodeForPublish, " + Retu_Str
                            Exit Sub
                        End If

                        wStr = "vworkspaceId=" + wsId + " AND iNodeId=" + nodeId.ToString + ""
                        If Not objHelpDb.getAttributeValueOfModifiedFile(wStr, modifiedFile, Retu_Str) Then
                            eStr_Retu += vbCrLf + "Error occured while retrieving submittedworkspacenodedetail, " + Retu_Str
                            Exit Sub
                        End If

                    End If
                    'hardcoding cover letter and fda form as new
                    If childAttrId.Rows.Count > 0 Then
                        Dim operationValue As String = ""
                        'If Regex.IsMatch(pathToCreate, ".*/m1/eu/.*cover.*", RegexOptions.IgnoreCase) Then
                        '    For i As Integer = 0 To childAttrId.Rows.Count - 1
                        '        dr = childAttrId.Rows(i)
                        '        If dr("vattrName").ToString.ToUpper.Equals("operation".ToUpper) Then
                        '            dr("vattrValue") = "new"
                        '            Console.WriteLine("pathToCreate" & pathToCreate)
                        '        End If
                        '    Next i
                        'End If
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

                            ElseIf attrName.ToUpper.Equals("operation".ToUpper) AndAlso (Not attrValue.Equals("new".ToUpper)) Then
                                Dim query As New StringBuilder()
                                For j As Integer = 0 To modifiedFile.Rows.Count - 1
                                    drmodifiedFile = modifiedFile.Rows(j)
                                    Dim lastPubVersion As String = drmodifiedFile("vLastPublishVersion")
                                    Dim indexId As String = drmodifiedFile("indexId")

                                    ' IMP Change done By Ashmak Shah 
                                    If pathToCreate.Contains("m1/ca") Then
                                        query.Append("../../../")
                                        query.Append("" & lastPubVersion & "")
                                        query.Append("/m1/ca/ca-regional.xml#")
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
                                    Dim m1Index = pathToCreate.IndexOf("m1/ca")

                                    If m1Index <> -1 Then
                                        pathToCreate = pathToCreate.Substring(0, m1Index + 5)
                                    End If

                                    attrValue = copyFileforPublish(publishForm, pathToCreate, eStr_Retu)
                                    attrValue = attrValue.ToLower()
                                    stype = stype.ToLower()

                                    'Change for New Submission Path
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
                            eStr_Retu += vbCrLf + "Error occured while retrieving from isLeafNodes, " + Retu_Str
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
                                generateSTFFile(wsId)
                            End If
                        End If


                        If nodeName.Trim().Equals("m1-administrative-information-and-prescribing-information") Then

                            wStr = "vworkspaceId=" + wsId + " AND iNodeId=" + parentId.ToString + ""

                            If Not objHelpDb.getNodeDetail(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                       parentNodeDetail, Retu_Str) Then
                                eStr_Retu += vbCrLf + "Error occured while retrieving workspaceNodeDetail, " + Retu_Str
                                Exit Sub
                            End If

                            If parentNodeDetail.Rows.Count > 0 AndAlso _
                               nodeName.Equals(parentNodeDetail.Rows(0)("vNodeName")) Then
                                isM1AdminNode = True
                            End If
                        End If

                        If Not isM1AdminNode Then
                            If isLeaf = 0 Then
                                If (Not nodetypeindi.ToUpper.Trim.Equals("T")) AndAlso (Not nodetypeindi.ToUpper.Trim().Equals("F")) Then

                                    'In case of nodes like 'CRFs' under STF no parent node will create like '<case-report-forms>' in index.xml
                                    'All crfs will be displayed with other stf nodes 

                                    If nodetypeindi.Trim().Equals("E") Then
                                        xmlwriter.WriteStartElement("node-extension")

                                        xmlwriter.WriteString("title")
                                        xmlwriter.WriteString(remark)
                                        xmlwriter.WriteEndElement() 'title
                                    Else
                                        xmlwriter.WriteStartElement(nodeName.Trim())
                                    End If

                                    ' For nodeTypeIndi = 'P' 

                                    Dim country As String = "", language As String = ""
                                    Dim PathForAttrFolder As String = ""
                                    Dim nodeAttribute As DataTable = Nothing

                                    wStr = "vWorkspaceId='" + wsId + "' and cAttrForIndi = 'ES'"
                                    If Not nodeId = -1 Then
                                        wStr += "and iNodeId=" + nodeId.ToString
                                    End If
                                    If Not objHelpDb.getNodeAttributes(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                       nodeAttribute, Retu_Str) Then
                                        eStr_Retu += vbCrLf + "Error occured while workspaceNodeAttrDetail, " + Retu_Str
                                        Exit Sub
                                    End If

                                    If nodeAttribute.Rows.Count > 0 Then
                                        For k As Integer = 0 To nodeAttribute.Rows.Count - 1
                                            drmodifiedFile = nodeAttribute.Rows(k)
                                            Dim attrname As String = drmodifiedFile("vAttrName")
                                            Dim attrvalue As String = drmodifiedFile("vAttrValue")

                                            'For nodeTypeIndi = 'P' 
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

                                        'nodeTypeIndi = 'P' (pi-doc)
                                        'Description : Folder Name will be 'country name'
                                        'Sub folder Name will be 'language'
                                        If nodetypeindi.Trim().Equals("P") Then
                                            PathForAttrFolder = pathToCreate & "/" & country
                                            MergeAttributeStr = language
                                            'nodeTypeIndi = 'D' (No Folder to be created)
                                            'Description : Folder will be created from attribute values
                                            'But not from 'vFolderName'
                                        ElseIf nodetypeindi.Trim().Equals("D") Then
                                            PathForAttrFolder = pathToCreate
                                            'No change in 'MergeAttributeStr'
                                            'nodeTypeIndi = 'N' (normal node)
                                            'Description : Folder will be created from vFolderName
                                        Else
                                            PathForAttrFolder = pathToCreate & "/" & filepathelement
                                        End If

                                        Console.WriteLine("PathForAttrFolder:" & PathForAttrFolder)
                                        createSubFolders(PathForAttrFolder, MergeAttributeStr, eStr_Retu)

                                        wStr = "vWorkspaceId = '" + wsId + "' and iParentNodeId =" + nodeId.ToString
                                        If Not objHelpDb.getChildNodeByParent(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, parentNodes, Retu_Str) Then
                                            Exit Sub
                                        End If

                                        'Here, inodeid passed as a parameter in function will be parentnodeid
                                        getChildNode(parentNodes, relativePathToCreate, nodeId, stype, publishForm, eStr_Retu)
                                        xmlwriter.WriteEndElement()
                                    Else
                                        wStr = "vWorkspaceId = '" + wsId + "' and iParentNodeId =" + nodeId.ToString
                                        If Not objHelpDb.getChildNodeByParent(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, parentNodes, Retu_Str) Then
                                            Exit Sub
                                        End If

                                        'same query for workspacenodedetail as above
                                        getChildNode(parentNodes, relativePathToCreate, nodeId, stype, publishForm, eStr_Retu)
                                        xmlwriter.WriteEndElement()
                                    End If
                                Else
                                    wStr = "vWorkspaceId = '" + wsId + "' and iParentNodeId =" + nodeId.ToString
                                    If Not objHelpDb.getChildNodeByParent(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, parentNodes, Retu_Str) Then
                                        eStr_Retu += vbCrLf + "Error occured while getchildNodeByParent, " + Retu_Str
                                        Exit Sub
                                    End If
                                    'same query for workspacenodedetail as above
                                    getChildNode(parentNodes, relativePathToCreate, nodeId, stype, publishForm, eStr_Retu)
                                End If
                            Else
                                wStr = "vWorkspaceId = '" + wsId + "' and iParentNodeId =" + nodeId.ToString
                                If Not objHelpDb.getChildNodeByParent(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, parentNodes, Retu_Str) Then
                                    eStr_Retu += vbCrLf + "Error occured while getchildNodeByparent, " + Retu_Str
                                    Exit Sub
                                End If
                                'same query for workspacenodedetail as above
                                getChildNode(parentNodes, relativePathToCreate, nodeId, stype, publishForm, eStr_Retu)
                            End If
                        Else
                            wStr = "vWorkspaceId = '" + wsId + "' and iParentNodeId =" + nodeId.ToString
                            If Not objHelpDb.getChildNodeByParent(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, parentNodes, Retu_Str) Then
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
            eStr_Retu += ex.Message + vbCrLf + "Error occured while getChildNode"
        End Try
    End Sub
#End Region

    Public Function generateSTFFile(ByVal wsId As String) As Boolean
        Dim objclsHelpDB As New clsHelpDB()
        Dim getallfirstnode As New DataTable
        Dim eStr_Retu As String = String.Empty
        Dim whr As String = String.Empty

        Try
            generateSTFFile = False
            eStr_Retu = ""
            whr = "vWorkspaceId='" + wsId + "' and cNodeTypeIndi='T'"
            If Not objclsHelpDB.getAllSTFFirstNodes(whr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    getallfirstnode, eStr_Retu) Then
                eStr_Retu += vbCrLf + "Error occured while View_CommonWorkspaceDetail, " + eStr_Retu
                Exit Function
            End If

            generateStudyDocument(wsId, getallfirstnode, eStr_Retu)
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while generateSTFFile"
            generateSTFFile = False
        End Try
    End Function

    Public Function generateStudyDocument(ByVal wsId As String, ByVal dtstffirstnodes As DataTable, _
                                          ByRef eStr_Retu As String) As Boolean
        generateStudyDocument = False
        Dim objclsHelpDB As New clsHelpDB()

        Dim dtAllChildNodes As New DataTable
        Dim dtstfHistory As New DataTable
        Dim dtallMultipleTypeChildNodes As New DataTable
        Dim dtsiteIdVector As New DataTable
        Dim dtisLeaf As New DataTable

        Dim estr As String = String.Empty
        Dim rows() As DataRow = Nothing
        Dim out As StreamWriter

        Dim nodeName As String = String.Empty
        Dim nodecategory As String = String.Empty
        Dim stfdata As String = String.Empty
        Dim fileName As String = String.Empty
        Dim whr As String = String.Empty

        Dim stfparentnodeid As Integer = 0
        Dim nodeId As Integer = 0
        Dim nodeno As Integer = 0
        Dim isLeaf As Integer = 0

        Try
            eStr_Retu = ""
            For i = 0 To dtstffirstnodes.Rows.Count - 1

                stfparentnodeid = dtstffirstnodes.Rows(i)("iNodeId")
                whr = "vWorkspaceId='" + wsId + "' and iParentNodeId=" + stfparentnodeid.ToString
                If Not objclsHelpDB.isLeafNodes(whr, DataRetrievalModeEnum.DataTable_WithWhereCondition, isLeaf, estr) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving workspacenodedetail, " + estr
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
                        eStr_Retu += vbCrLf + "Error occured while retrieving view_commonworkspacedetail, " + estr
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
                                    eStr_Retu += vbCrLf + "Error occured while retrieving View_CommonWorkspaceDetail, " + estr
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
                                        eStr_Retu += vbCrLf + "Error occured while retrieving view_commonworkspacedetail, " + estr
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
                                                eStr_Retu += vbCrLf + "Error occured while retrieving stfstudyIdentifiermst, " + estr
                                                Exit Function
                                            End If


                                            stfdata += "<" + dtsiteIdVector.Rows(0)("vTagName") + " " ';//start property tag
                                            stfdata += dtsiteIdVector.Rows(0)("vAttrName") + "=""" + dtsiteIdVector.Rows(0)("vAttrValue") + """ "

                                            stfdata += dtsiteIdVector.Rows(1)("vAttrName") + "=""" + dtsiteIdVector.Rows(1)("vAttrValue") + """>"
                                            stfdata += dtsiteIdVector.Rows(1)("vNodeContent") ';//Site-Identifier value
                                            stfdata += "</" + dtsiteIdVector.Rows(1)("vTagName") + ">" + Chr(13) ';//end property1 tag


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
            eStr_Retu += ex.Message + vbCrLf + "Error occured while generateStudyDocument"
            generateStudyDocument = False
        End Try
    End Function

    Public Function getSTFFileLocation(ByVal nodeId As Integer, ByRef eStr_Retu As String) As String
        Try
            Dim filelocation As String = String.Empty

            For i = 0 To STFXMLLocation.Rows.Count - 1
                If STFXMLLocation.Rows(i)("iNodeId") = nodeId Then
                    filelocation = STFXMLLocation.Rows(i)("vBaseWorkFolder")
                    Return filelocation
                End If
            Next
            Return filelocation
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while getSTFFileLocation"
            Return Nothing
        End Try
    End Function

    Public Function getUpCount(ByVal filelocation As String, ByRef eStr_Retu As String) As Integer
        Try
            Dim count As Integer = 1
            filelocation = filelocation.Substring((absolutePathToCreate.Length()) + 1)
            For j = 0 To filelocation.Length() - 1
                If "\" = filelocation.Chars(j) Or "/" = filelocation.Chars(j) Then
                    count += 1
                End If
            Next

            Return count
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while getUpCount"
        End Try
    End Function

    Public Function generateStudyIndetifier(ByVal firstnodeId As String, ByVal wsId As String, _
                                            ByRef eStr_Retu As String) As String
        Dim dtstudyidentifierdata As New DataTable
        Dim objclsHelpDB As New clsHelpDB

        Dim previoustagid As Integer = 0
        Dim currenttagid As Integer = 0
        Dim tagname As String = String.Empty
        Dim attrName As String = String.Empty
        Dim attrValue As String = String.Empty
        Dim nodeContent As String = String.Empty
        Dim previoustagname As String = String.Empty
        Dim previoustagcontent As String = String.Empty
        Dim studyId As String = String.Empty
        Dim Whr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim studyidendata As String = "<?xml version=""1.0"" encoding=""UTF-8""?>" + Chr(13)

        Try
            eStr_Retu = ""
            generateStudyIndetifier = ""
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
                eStr_Retu += vbCrLf + "Error occured while stfstudyIdentifiermst, " + eStr
                Exit Function
            End If
            For i = 0 To dtstudyidentifierdata.Rows.Count - 1

                currenttagid = dtstudyidentifierdata.Rows(i)("iTagSequenceId")
                tagname = dtstudyidentifierdata.Rows(i)("vTagName")
                attrName = dtstudyidentifierdata.Rows(i)("vAttrName")
                attrValue = dtstudyidentifierdata.Rows(i)("vAttrValue")
                nodeContent = dtstudyidentifierdata.Rows(i)("vNodeContent")
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
            eStr_Retu += ex.Message + vbCrLf + "Error occured while generateStudyIndetifier"
            Return ""
        End Try
    End Function

    Private Sub writeToXmlFile(ByVal stype As String, ByVal publishForm As DataTable, ByRef eStr_Retu As String)
        Dim StringBuilder As String = String.Empty

        Try
            eStr_Retu = ""
            If stype.Equals("ca") Then
                StringBuilder += "<?xml version=""1.0"" encoding=""UTF-8""?>"
                StringBuilder += Environment.NewLine.ToString + "<!DOCTYPE hcsc:ectd SYSTEM ""../../util/dtd/ca-regional-1-0.dtd"">"
                StringBuilder += Environment.NewLine.ToString + "<hcsc:ectd xml:lang=""EN"" xmlns:hcsc=""http://www.hc-sc.gc.ca/hpb-dgps/therapeut/ectd"" xmlns:xlink=""http://www.w3c.org/1999/xlink"">"


                StringBuilder += Environment.NewLine.ToString + "<introduction>"
                StringBuilder += Environment.NewLine.ToString + "<applicant>" + publishForm.Rows(0)("Applicant") + "</applicant>"
                StringBuilder += Environment.NewLine.ToString + "<product-name>" + publishForm.Rows(0)("ProductName") + "</product-name>"

                StringBuilder += Environment.NewLine.ToString + "<ectd-submission-info submission-type=""" + publishForm.Rows(0)("SubmissionType_ca") + """>"
                StringBuilder += Environment.NewLine.ToString + "<submission-identifier>" + publishForm.Rows(0)("ApplicationNumber") + "</submission-identifier>"
                StringBuilder += Environment.NewLine.ToString + "<sequence-number>" + publishForm.Rows(0)("SeqNumber") + "</sequence-number>"

                If publishForm.Rows(0)("RelatedSeqNumber") IsNot Nothing AndAlso _
                    (Not publishForm.Rows(0)("RelatedSeqNumber").ToString.Equals("")) Then

                    Dim allRelatedSequences() As String = publishForm.Rows(0)("RelatedSeqNumber").ToString.Split(",")
                    For iNum As Integer = 0 To allRelatedSequences.Length - 1
                        StringBuilder += Environment.NewLine.ToString & "<related-sequence-number>" & _
                                          allRelatedSequences(iNum).Replace("-", "/").Trim() & "</related-sequence-number>"
                    Next iNum
                End If
                StringBuilder += Environment.NewLine.ToString + "<submission-date>" + dos.ToString("yyyy-MM-dd") + "</submission-date>"
                StringBuilder += Environment.NewLine.ToString + " </ectd-submission-info>"
                StringBuilder += Environment.NewLine.ToString + "</introduction>"
                StringBuilder += Environment.NewLine.ToString

            ElseIf stype.Equals("cam2-m5") Then
                StringBuilder += "<?xml version=""1.0"" encoding=""ISO-8859-1""?>"
                StringBuilder += Environment.NewLine.ToString + "<!DOCTYPE ectd:ectd SYSTEM ""util/dtd/ich-ectd-3-2.dtd"">"
                StringBuilder += Environment.NewLine.ToString + "<?xml-stylesheet type=""text/xsl"" href=""util/style/ectd-2-0.xsl""?>"
                StringBuilder += Environment.NewLine.ToString + "<ectd:ectd xmlns:ectd=""http://www.ich.org/ectd"" xmlns:xlink=""http://www.w3c.org/1999/xlink"" dtd-version=""3.2"">"
                StringBuilder += Environment.NewLine.ToString + "<m1-administrative-information-and-prescribing-information>"

                Dim eumd5 As String = clsCommon.CalculateChecksum(absolutePathToCreate & "/m1/ca/ca-regional.xml").ToString

                StringBuilder += Environment.NewLine.ToString + "<leaf xml:lang=""en"" checksum-type=""md5"" operation=""new"" application-version="""" xlink:href=""m1/ca/ca-regional.xml"" checksum=""" & eumd5 & """ ID=""node-999"" xlink:type=""simple"">"
                StringBuilder += "<title>"
                StringBuilder += "m1-administrative-information-and-prescribing-information"
                StringBuilder += "</title>"
                StringBuilder += "</leaf>"
                StringBuilder += "</m1-administrative-information-and-prescribing-information>" + Environment.NewLine
            End If
            xmlwriter.WriteString(StringBuilder)
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while writeToXmlFile"
        End Try
    End Sub

    Private Sub UpdateXMLFile(ByVal path As String)
        Dim xmlString As String = String.Empty

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
    End Sub

    Private Sub AddAttrib(ByVal attrName As String, ByVal attrValue As String, _
                          ByRef eStr_Retu As String)
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

        End Try

    End Sub

End Class
