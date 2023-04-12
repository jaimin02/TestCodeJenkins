Imports System.IO
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Imports System.Xml
Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Class clsTrackingTableGenerator

    Private destinationDir As DirectoryInfo
    Private euDTDVersion As String
    Private workspaceId As String
    Private doc As XmlDocument

    Public Shared Sub addTrackingTable(ByVal euRegional As FileType, ByVal workspaceId As String, _
                                       ByVal submissionId As String, ByVal euDTDVersion As String, _
                                       ByVal BaseWorkFolder As String, _
                                       ByRef eStr_Retu As String)

        Dim trackingTableGenerator As clsTrackingTableGenerator = Nothing

        Dim sourceLocation As DirectoryInfo = Nothing
        Dim targetLocation As DirectoryInfo = Nothing

        Dim trackingTableXml As FileType

        Dim euRegionalDoc As XmlDocument = Nothing

        Dim euPath As String
        Dim relativePath As String = "10-cover/common"

        Try
            eStr_Retu = ""
            euPath = Path.GetDirectoryName(euRegional.FullName)

            trackingTableGenerator = New clsTrackingTableGenerator
            trackingTableGenerator.euDTDVersion = euDTDVersion
            trackingTableGenerator.workspaceId = workspaceId
            trackingTableGenerator.destinationDir = Directory.CreateDirectory(euPath + "\" + relativePath)

            trackingTableXml = trackingTableGenerator.generateXML(eStr_Retu)

            sourceLocation = New DirectoryInfo(BaseWorkFolder + "/util_tt/util")

            targetLocation = Directory.CreateDirectory(euPath + "/util")


            For Each sDir As DirectoryInfo In sourceLocation.GetDirectories

                If Not Directory.Exists(targetLocation.FullName + "\" + sDir.Name) Then
                    Directory.CreateDirectory(targetLocation.FullName + "\" + sDir.Name)
                End If

                For Each sFile As FileInfo In sDir.GetFiles
                    File.Copy(sFile.FullName, targetLocation.FullName + "\" + sDir.Name + "\" + sFile.Name, True)
                Next sFile

            Next sDir


            trackingTableGenerator.addCurrentSequenceDetails(trackingTableXml, submissionId, eStr_Retu)

            Try
                Dim nodeContentsList As List(Of clsNodeContents)
                Dim attr As List(Of String)
                Dim node As clsNodeContents
                Dim specificCommon As XmlNode
                Dim ttLeaf As XmlElement
                Dim title As XmlElement

                Dim ttChecksum As String

                euRegionalDoc = New XmlDocument()
                euRegionalDoc.Load(euRegional.FullName)

                nodeContentsList = New List(Of clsNodeContents)
                node = New clsNodeContents("eu:eu-backbone", Nothing, Nothing)
                nodeContentsList.Add(node)
                node = New clsNodeContents("m1-eu", Nothing, Nothing)
                nodeContentsList.Add(node)
                node = New clsNodeContents("m1-0-cover", Nothing, Nothing)
                nodeContentsList.Add(node)

                attr = New List(Of String)
                attr.Add("country=common")

                node = New clsNodeContents("specific", Nothing, attr)
                nodeContentsList.Add(node)

                specificCommon = clsXmlUtilities.createIfNotExists(euRegionalDoc, euRegionalDoc, nodeContentsList)

                ttLeaf = euRegionalDoc.CreateElement("leaf")
                ttLeaf.SetAttribute("ID", "node-tt")
                'ttLeaf.SetAttribute("xlink:href", relativePath + "/" + trackingTableXml.FileName)
                'ttLeaf.SetAttribute("xlink:href", "http://www.w3c.org/1999/xlink", relativePath + "/" + trackingTableXml.FileName)
                ttLeaf.SetAttributeNode(AddAttrib("xlink:href", relativePath + "/" + trackingTableXml.FileName, eStr_Retu))
                ttLeaf.SetAttribute("operation", "new")
                ttLeaf.SetAttribute("checksum-type", "md5")

                ttChecksum = clsCommon.CalculateChecksum(trackingTableXml.FullName)
                ttLeaf.SetAttribute("checksum", ttChecksum)

                title = euRegionalDoc.CreateElement("title")
                title.InnerText = "Tracking Table in XML"

                ttLeaf.AppendChild(title)

                specificCommon.AppendChild(ttLeaf)

                euRegionalDoc.Save(euRegional.FullName)

            Catch ex As Exception
                eStr_Retu += ex.Message + vbCrLf + "Error occured while saving addTrackingTable"
            End Try
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while saving addTrackingTable"
        End Try

    End Sub

    Private Function generateXML(ByRef eStr_Retu As String) As FileType
        generateXML = Nothing
        Dim trackingTableXml As File = Nothing
        Dim objHelpDB As New clsHelpDB

        Dim XmlDecl As XmlDeclaration
        Dim doctype As XmlDocumentType
        Dim pi As XmlProcessingInstruction
        Dim rootEle As XmlElement

        Dim dtoXmlWs As DataTable = Nothing
        Dim xmlRootNodeDtl As DataTable = Nothing
        Dim lstAttr As DataTable = Nothing
        Dim dtosub As DataTable = Nothing
        Dim lstXmlRootChildren As DataTable = Nothing

        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ttName As String = String.Empty
        Dim xmlHeaders() As String = Nothing
        Dim systemId As String = Nothing
        Dim publicId As String = Nothing
        Dim qualifiedName As String = Nothing
        Dim target As String = String.Empty
        Dim data As String = String.Empty

        Dim rootNodeId As Short = 1


        Try
            eStr_Retu = ""
            doc = New XmlDocument
            XmlDecl = doc.CreateXmlDeclaration("1.0", "UTF-8", "")
            doc.AppendChild(XmlDecl)

            If euDTDVersion.Equals("") Then
                ttName = WorkspaceFile.TRACKING_TABLE_XML
            ElseIf euDTDVersion.Equals("14") Then
                ttName = WorkspaceFile.TRACKING_TABLE_XML_FOR_EU14
            End If

            wStr = "vXmlWorkspaceName = '" + ttName + "'"
            If Not objHelpDB.getXmlWorkspaceDtl(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                dtoXmlWs, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving XmlWorkspaceMst, " + eStr
                Exit Function
            End If

            xmlHeaders = Regex.Split(dtoXmlWs.Rows(0)("vXmlHeader"), ">[" + Space(1) + "]*<")

            For Each xmlHeader As String In xmlHeaders
                If Not xmlHeader.EndsWith(">") Then
                    xmlHeader = xmlHeader + ">"
                End If

                If Not xmlHeader.StartsWith("<") Then
                    xmlHeader = "<" + xmlHeader
                End If

                If xmlHeader.Contains("!DOCTYPE") Then
                    If xmlHeader.Contains(" SYSTEM ") Then
                        systemId = xmlHeader.Substring(xmlHeader.IndexOf("""") + 1, xmlHeader.LastIndexOf("""") - 2 - xmlHeader.IndexOf("""") + 1)
                        qualifiedName = xmlHeader.Substring(xmlHeader.IndexOf("<!DOCTYPE ") + 10, xmlHeader.IndexOf(" SYSTEM ") - (xmlHeader.IndexOf("<!DOCTYPE ") + 10))
                    ElseIf xmlHeader.Contains(" PUBLIC ") Then
                        publicId = xmlHeader.Substring(xmlHeader.IndexOf("""") + 1, xmlHeader.LastIndexOf("""") - 2 - xmlHeader.IndexOf("""") + 1)
                        qualifiedName = xmlHeader.Substring(xmlHeader.IndexOf("<!DOCTYPE ") + 10, xmlHeader.IndexOf(" PUBLIC ") - (xmlHeader.IndexOf("<!DOCTYPE ") + 10))
                    End If

                    doctype = doc.CreateDocumentType(qualifiedName, publicId, systemId, Nothing)

                    doc.AppendChild(doctype)

                ElseIf xmlHeader.StartsWith("<?") Then
                    target = xmlHeader.Substring(xmlHeader.IndexOf("<?") + 2, xmlHeader.IndexOf(" ") - 2)
                    data = xmlHeader.Substring(xmlHeader.IndexOf(" ") + 1, xmlHeader.Length - xmlHeader.IndexOf(" ") - 3)

                    pi = doc.CreateProcessingInstruction(target, data)
                    doc.AppendChild(pi)
                End If

            Next xmlHeader

            'Create the root element 
            wStr = "iXmlWorkspaceId = " + dtoXmlWs.Rows(0)("ixmlWorkspaceId").ToString + _
                   " AND iXmlNodeId = " + rootNodeId.ToString
            If Not objHelpDB.getXmlNodeDtl(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                           xmlRootNodeDtl, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving View_XmlNodeDtl, " + eStr
                Exit Function
            End If

            rootEle = doc.CreateElement(xmlRootNodeDtl.Rows(0)("vXmlNodeName"))

            'Get root node's attributes
            wStr = "iXmlWorkspaceId = " + dtoXmlWs.Rows(0)("ixmlWorkspaceId").ToString + _
                   " AND iXmlNodeId = " + xmlRootNodeDtl.Rows(0)("iXmlNodeId").ToString


            If Not objHelpDB.getXmlNodeAttrDtl(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                               lstAttr, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving XmlNodeAttrDtl, " + eStr
                Exit Function
            End If

            For Each dr_dtoAttr As DataRow In lstAttr.Rows
                If dr_dtoAttr("cFixed") = "Y" Then

                    rootEle.SetAttribute(dr_dtoAttr("vAttrName"), dr_dtoAttr("vDefaultAttrValue"))

                ElseIf dr_dtoAttr("vAttrName").ToString.ToLower.Equals("title") Then
                    wStr = "vWorkspaceId = '" + workspaceId + "'"
                    If Not objHelpDB.getSubmissionInfo(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                       dtosub, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while retrieving view_AllWorkspaceSubmissionInfo, " + eStr
                        Exit Function
                    End If

                    If dtosub.Rows(0)("vProcedureType").Equals("mutual-recognition") Then
                        rootEle.SetAttribute(dr_dtoAttr("vAttrName"), "MRP Tracking Table")
                    ElseIf dtosub.Rows(0)("vProcedureType").Equals("decentralised") Then
                        rootEle.SetAttribute(dr_dtoAttr("vAttrName"), "DCP Tracking Table")
                    End If
                End If
            Next dr_dtoAttr

            doc.AppendChild(rootEle)

            'Add all children for 'rootEle' Element.
            wStr = "iXmlWorkspaceId = " + dtoXmlWs.Rows(0)("iXmlWorkspaceId").ToString + _
                   " AND iParentNodeId = " + rootNodeId.ToString

            If Not objHelpDB.getXmlChildNodeDtl(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                lstXmlRootChildren, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving View_XmlNodeDtl, " + eStr
                Exit Function
            End If


            addChildren(dtoXmlWs.Rows(0)("iXmlWorkspaceId"), lstXmlRootChildren, rootEle, New Dictionary(Of String, String), eStr_Retu)

            'destinationDir.mkdirs();
            doc.Save(destinationDir.FullName + "\" + dtoXmlWs.Rows(0)("vXmlFileName").ToString)

            generateXML.FullName = destinationDir.FullName + "\" + dtoXmlWs.Rows(0)("vXmlFileName").ToString

        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while saving GenerateXML"
        End Try

    End Function

    Private Sub addCurrentSequenceDetails(ByVal trackingTableXml As FileType, ByVal submissionId As String, _
                                          ByRef eStr_Retu As String)
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim objHelpDB As New clsHelpDB
        Dim submissionDescription As String = Nothing
        Dim currentSeqNumber As String = Nothing
        Dim countryCode As String = Nothing
        Dim RMSSubmited As Char = "N"

        Dim dateOfSubmission As Date = Nothing

        Dim submissionInfoEUDtl As DataTable = Nothing
        Dim submissionInfoEU14Dtl As DataTable = Nothing
        Dim cmsDtl As DataTable = Nothing
        Dim dtoRms As DataTable = Nothing
        Dim rmsCountry As DataTable = Nothing
        Dim submissionInfoEUSubDtl As DataTable = Nothing
        Dim submissionInfoEU14SubDtl As DataTable = Nothing

        Try
            eStr_Retu = ""
            If euDTDVersion.Equals("") Then
                wStr = "SubmissionInfoEUDtlId='" + submissionId + "'"
                If Not objHelpDB.getWorkspaceSubmissionInfoEUDtlBySubmissionId(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                               submissionInfoEUDtl, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving View_SubmissionInfoEUDtl, " + eStr
                    Exit Sub
                End If

                currentSeqNumber = submissionInfoEUDtl.Rows(0)("CurrentSeqNumber")
                RMSSubmited = submissionInfoEUDtl.Rows(0)("RMSSubmited")
                countryCode = submissionInfoEUDtl.Rows(0)("CountryCode")
                dateOfSubmission = CType(submissionInfoEUDtl.Rows(0)("DateOfSubmission"), Date)
            ElseIf euDTDVersion.Equals("14") Then
                wStr = "SubmissionInfoEU14DtlId='" + submissionId + "'"
                If Not objHelpDB.getWorkspaceSubmissionInfoEU14DtlBySubmissionId(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                                 submissionInfoEU14Dtl, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving View_SubmissionInfoEU14Dtl, " + eStr
                    Exit Sub
                End If
                currentSeqNumber = submissionInfoEU14Dtl.Rows(0)("CurrentSeqNumber")
                RMSSubmited = submissionInfoEU14Dtl.Rows(0)("RMSSubmited")
                countryCode = submissionInfoEU14Dtl.Rows(0)("CountryCode")
                dateOfSubmission = CType(submissionInfoEU14Dtl.Rows(0)("DateOfSubmission"), Date)
            End If


            If euDTDVersion.Equals("") Then
                If Not objHelpDB.getWorkspaceCMSSubmissionInfo(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               cmsDtl, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving View_WorkspaceCMSSubmissionDtl, " + eStr
                    Exit Sub
                End If
                If cmsDtl.Rows.Count > 0 Then
                    submissionDescription = cmsDtl.Rows(0)("vSubmissionDescription")
                Else
                    If Not objHelpDB.getWorkspaceRMSSubmissionInfo(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                   dtoRms, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while retrieving SubmissionInfoEUSubDtl, " + eStr
                        Exit Sub
                    End If

                    submissionDescription = dtoRms.Rows(0)("vSubmissionDescription")
                End If
            ElseIf euDTDVersion.Equals("14") Then
                wStr = "SubmissionId = '" + submissionId + "'"
                If Not objHelpDB.getWorkspaceCMSSubmissionInfoEU14(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                   cmsDtl, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving View_WorkspaceCMSSubmissionDtlForEU14, " + eStr
                    Exit Sub
                End If
                If cmsDtl.Rows.Count > 0 Then
                    submissionDescription = cmsDtl.Rows(0)("vSubmissionDescription")
                Else
                    If Not objHelpDB.getWorkspaceRMSSubmissionInfoEU14(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                       dtoRms, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while retrieving SubmissionInfoEU14SubDtl, " + eStr
                        Exit Sub
                    End If

                    submissionDescription = dtoRms.Rows(0)("vSubmissionDescription")
                End If
            End If


            Dim trackingXmlDoc As XmlDocument = Nothing
            Dim trackingRoot As XmlNode = Nothing
            Dim rootChild As XmlNode = Nothing

            Dim newSequence As XmlElement = Nothing
            Dim description As XmlElement = Nothing
            Dim rms As XmlElement = Nothing
            Dim cms As XmlElement = Nothing
            trackingXmlDoc = New XmlDocument

            trackingXmlDoc.Load(trackingTableXml.FullName)

            newSequence = trackingXmlDoc.CreateElement("sequence")
            newSequence.SetAttribute("number", currentSeqNumber)

            description = trackingXmlDoc.CreateElement("description")
            description.InnerText = submissionDescription
            newSequence.AppendChild(description)


            If RMSSubmited = "Y" Then

                rms = trackingXmlDoc.CreateElement("submission")
                wStr = "vCountryID = '" + countryCode + "'"

                If Not objHelpDB.getCountry(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            rmsCountry, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving CountryMst, " + eStr
                    Exit Sub
                End If

                rms.SetAttribute("code", rmsCountry.Rows(0)("vCountryCode").ToString.ToUpper)
                rms.SetAttribute("month", dateOfSubmission.ToString("MMM"))
                rms.SetAttribute("year", dateOfSubmission.ToString("yyyy"))

                newSequence.AppendChild(rms)
            End If
            If euDTDVersion.Equals("") Then
                For Each dr_cms In cmsDtl.Rows
                    cms = trackingXmlDoc.CreateElement("submission")
                    cms.SetAttribute("code", dr_cms("CountryCodeName").ToString.ToUpper)
                    cms.SetAttribute("month", CType(dr_cms("dDateOfSubmission"), Date).ToString("MMM"))
                    cms.SetAttribute("year", CType(dr_cms("dDateOfSubmission"), Date).ToString("yyyy"))
                    newSequence.AppendChild(cms)
                Next dr_cms
            ElseIf euDTDVersion.Equals("14") Then
                For Each dr_cms In cmsDtl.Rows
                    cms = trackingXmlDoc.CreateElement("submission")
                    cms.SetAttribute("code", dr_cms("CountryCodeName").ToString.ToUpper)
                    cms.SetAttribute("month", CType(dr_cms("dDateOfSubmission"), Date).ToString("MMM"))
                    cms.SetAttribute("year", CType(dr_cms("dDateOfSubmission"), Date).ToString("yyyy"))
                    newSequence.AppendChild(cms)
                Next dr_cms
            End If


            trackingRoot = clsXmlUtilities.getFirstChild(trackingXmlDoc)
            rootChild = clsXmlUtilities.getFirstChild(trackingRoot)
            While True
                rootChild = clsXmlUtilities.getNextSibling(rootChild)
                If (IsNothing(rootChild) Or (Not IsNothing(rootChild) AndAlso rootChild.Name = "sequence")) Then
                    Exit While
                End If
            End While

            If IsNothing(rootChild) Then
                trackingRoot.AppendChild(newSequence)
            Else
                trackingRoot.InsertBefore(newSequence, rootChild)
            End If


            trackingXmlDoc.Save(trackingTableXml.FullName)

        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while saving AddCurrentSequenceDetails"
        End Try
    End Sub

    Public Shared Sub removeAll(ByVal node As XmlNode, ByVal nodeType As Short, ByVal name As String, _
                                ByRef eStr_Retu As String)

        Try
            eStr_Retu = ""
            If (node.NodeType = nodeType And (name = Nothing Or node.Name = name)) Then
                node.ParentNode().RemoveChild(node)
            Else
                For Each childNode As XmlNode In node
                    removeAll(childNode, nodeType, name, eStr_Retu)
                Next childNode
            End If
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while saving RemoveAll"
        End Try
    End Sub

    Private Sub addChildren(ByVal xmlWorkspaceId As Long, ByVal lstXmlNodes As DataTable, ByVal parentEle As XmlElement, _
                            ByVal inputFields As Dictionary(Of String, String), _
                            ByRef eStr_Retu As String)

        Dim objHelpDB As New clsHelpDB

        Dim nodePrimaryAttr As DataTable = Nothing
        Dim primaryAttrValues As DataTable = Nothing
        Dim lstAttr As DataTable = Nothing
        Dim dt_reversed As DataTable = Nothing
        Dim attrValues As DataTable = Nothing
        Dim lstXmlChildren As DataTable = Nothing
        Dim nodeValues As DataTable = Nothing

        Dim dr_primaryAttrValues As DataRow = Nothing

        Dim childInputFields As Dictionary(Of String, String)
        Dim nodeEle As XmlElement = Nothing
        Dim nodeText As XmlNode = Nothing

        Dim nodeTable As String = String.Empty
        Dim nodeColumn As String = String.Empty
        Dim primaryAttrTable As String = Nothing
        Dim primaryAttrColumn As String = "Column1"
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim primaryAttrValue As String = String.Empty
        Dim attrTable As String = String.Empty
        Dim attrColumn As String = String.Empty
        Dim attrValueToWrite As String = String.Empty

        Try
            eStr_Retu = ""
            For Each dtoXmlNode As DataRow In lstXmlNodes.Rows
                nodeTable = IIf(IsDBNull(dtoXmlNode("vTableName")), Nothing, dtoXmlNode("vTableName"))
                nodeColumn = IIf(IsDBNull(dtoXmlNode("vColumnName")), Nothing, dtoXmlNode("vColumnName"))

                If dtoXmlNode("cRepeatable") = "Y" Then
                    wStr = " iXmlNodeAttrDtlId = " + dtoXmlNode("iPrimaryXmlAttrId").ToString
                    If Not objHelpDB.getXmlAttrDtl(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                   nodePrimaryAttr, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while retrieving XmlNodeAttrDtl, " + eStr
                        Exit Sub
                    End If


                    If nodePrimaryAttr.Rows.Count > 0 Then

                        primaryAttrTable = nodePrimaryAttr.Rows(0)("vTableName")
                        primaryAttrColumn = nodePrimaryAttr.Rows(0)("vColumnName")

                        wStr = "Select Distinct " + primaryAttrColumn + " From " + primaryAttrTable + _
                               " Where vWorkspaceId = '" + workspaceId + "'"

                        For Each Input As KeyValuePair(Of String, String) In inputFields
                            wStr += " AND " + Input.Key() + " = '" + Input.Value() + "' "
                        Next Input

                        If Not objHelpDB.getXmlAttrValuesForRepeatableNode(wStr, primaryAttrValues, eStr) Then
                            eStr_Retu += vbCrLf + "Error occured while retrieving XmlAttrValuesForRepeatableNode, " + eStr
                            Exit Sub
                        End If

                    End If
                Else
                    primaryAttrValues = New DataTable
                    primaryAttrValues.Columns.Add(primaryAttrColumn, GetType(System.String))
                    dr_primaryAttrValues = primaryAttrValues.NewRow()
                    dr_primaryAttrValues(primaryAttrColumn) = ""
                    primaryAttrValues.Rows.Add(dr_primaryAttrValues)
                End If




                If dtoXmlNode("vXmlNodeName") = "sequence" Then
                    dt_reversed = primaryAttrValues.Clone()
                    For row As Integer = primaryAttrValues.Rows.Count - 1 To row >= 0
                        dt_reversed.ImportRow(primaryAttrValues.Rows(row))
                        row -= row
                    Next row

                    primaryAttrValues = Nothing
                    primaryAttrValues = dt_reversed.Copy
                End If

                dtoXmlNode("iPrimaryXmlAttrId") = IIf(IsDBNull(dtoXmlNode("iPrimaryXmlAttrId")), 0, dtoXmlNode("iPrimaryXmlAttrId"))
                For Each dr_primaryAttrValue As DataRow In primaryAttrValues.Rows
                    primaryAttrValue = dr_primaryAttrValue(primaryAttrColumn)

                    nodeEle = doc.CreateElement(dtoXmlNode("vXmlNodeName"))

                    wStr = " iXmlWorkspaceId = " + xmlWorkspaceId.ToString + " AND iXmlNodeId = " + dtoXmlNode("ixmlNodeId").ToString
                    If Not objHelpDB.getXmlNodeAttrDtl(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                       lstAttr, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while retrieving XmlNodeAttrDtl, " + eStr
                        Exit Sub
                    End If

                    For Each dr_XmlNodeAttr As DataRow In lstAttr.Rows
                        If dr_XmlNodeAttr("iXmlNodeAttrDtlId") = dtoXmlNode("iPrimaryXmlAttrId") Then
                            If dr_XmlNodeAttr("vAttrName").Equals("code") Then
                                primaryAttrValue = primaryAttrValue.ToUpper
                            End If
                            nodeEle.SetAttribute(dr_XmlNodeAttr("vAttrName"), primaryAttrValue)
                        Else
                            attrTable = dr_XmlNodeAttr("vTableName")
                            attrColumn = dr_XmlNodeAttr("vColumnName")

                            If dr_XmlNodeAttr("cFixed") = "Y" Then
                                nodeEle.SetAttribute(dr_XmlNodeAttr("vAttrName"), dr_XmlNodeAttr("vDefaultAttrValue"))
                            ElseIf attrTable IsNot Nothing AndAlso attrColumn IsNot Nothing Then
                                wStr = "Select Distinct " + attrColumn + " From " + attrTable + _
                                       " Where vWorkspaceId = '" + workspaceId + "'"

                                For Each Input As KeyValuePair(Of String, String) In inputFields
                                    wStr += " AND " + Input.Key() + " = '" + Input.Value() + "' "
                                Next Input
                                If Not objHelpDB.getXmlAttrValue(wStr, attrValues, eStr) Then
                                    eStr_Retu += vbCrLf + "Error occured while retrieving workspaceNodeDetail, " + eStr
                                    Exit Sub
                                End If

                                If attrValues.Rows.Count > 0 Then
                                    attrValueToWrite = attrValues.Rows(0)(0)

                                    If dr_XmlNodeAttr("vAttrName").Equals("code") Then
                                        attrValueToWrite = attrValueToWrite.ToUpper()
                                    End If
                                    nodeEle.SetAttribute(dr_XmlNodeAttr("vAttrName"), attrValueToWrite)
                                End If
                            End If
                        End If


                        wStr = " iXmlWorkspaceId = " + xmlWorkspaceId.ToString + " AND iParentNodeId = " + dtoXmlNode("iXmlNodeId").ToString
                        If Not objHelpDB.getXmlChildNodeDtl(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                            lstXmlChildren, eStr) Then
                            eStr_Retu += vbCrLf + "Error occured while retrieving View_XmlNodeDtl, " + eStr
                            Exit Sub
                        End If

                        If lstXmlChildren.Rows.Count > 0 Then
                            childInputFields = inputFields '======
                            childInputFields.Add(primaryAttrColumn, primaryAttrValue)

                            addChildren(xmlWorkspaceId, lstXmlChildren, nodeEle, childInputFields, eStr_Retu)
                        End If

                        If nodeTable IsNot Nothing AndAlso nodeColumn IsNot Nothing Then
                            wStr = "Select Distinct " + nodeColumn + " From " + nodeTable + _
                                   " Where vWorkspaceId = '" + workspaceId + "'"

                            For Each Input As KeyValuePair(Of String, String) In inputFields
                                wStr += " AND " + Input.Key() + " = '" + Input.Value() + "' "
                            Next Input

                            If Not objHelpDB.getXmlNodeValue(wStr, nodeValues, eStr) Then
                                eStr_Retu += vbCrLf + "Error occured while XmlNodeValue, " + eStr
                                Exit Sub
                            End If

                            For Each dr_val As DataRow In nodeValues.Rows
                                If dtoXmlNode("cempty") = "N" AndAlso (dr_val(0) IsNot Nothing AndAlso (Not dr_val(0).Equals(""))) Then
                                    nodeText = doc.CreateTextNode(Not dr_val(0))
                                    nodeEle.AppendChild(nodeText)

                                    If dtoXmlNode("crepeatable") = "N" Then
                                        Exit For
                                    End If
                                End If
                            Next

                        End If

                        parentEle.AppendChild(nodeEle)

                    Next dr_XmlNodeAttr
                Next dr_primaryAttrValue
            Next dtoXmlNode
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while addChildren"
        End Try
    End Sub
    Private Shared Function AddAttrib(ByVal attrName As String, ByVal attrValue As String, _
                                      ByRef eStr_Retu As String) As XmlAttribute
        AddAttrib = Nothing
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

            Return Attribute

        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while AddAttrib"
        Finally
            doc = Nothing
        End Try
    End Function

End Class
