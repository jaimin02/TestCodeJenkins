Imports System.IO
Imports System.Xml
Imports Microsoft.VisualBasic

Public Class clsPublishLogic
    Dim objHelpDb As New clsHelpDB
    Dim writer As StringWriter
    Public eStr As String

#Region "saveSubmissionDtl"
    Friend Function saveSubmissionDtl(ByVal userId As Integer, _
                                      ByVal subType As String, _
                                      ByVal workspaceId As String, _
                                      ByVal applicationNumber As String, _
                                      ByVal projectPublishType As Char, _
                                      ByVal currentSeqNumber As String, _
                                      ByVal lastPublishedVersion As String, _
                                      ByVal dos As Date, _
                                      ByVal relatedSeqNumber As String, _
                                      ByVal subMode As String, _
                                      ByVal isRMSSelected As Char, _
                                      ByVal subVariationMode As String, _
                                      ByVal addTT As Char, _
                                      ByVal lastConfirmedSubmissionPath As String, _
                                      ByVal baseWorkFolder As String, _
                                      ByVal subDesc As String, _
                                      ByVal selectedCMS() As Integer, _
                                      ByVal trackCMS() As String, _
                                      ByRef eStr_Retu As String) As Boolean

        saveSubmissionDtl = False
        Dim dtoSubmissionMst As DataTable = Nothing
        Dim dtoInternalLabelMst As DataTable = Nothing
        Dim dtoSubmissionInfoUSDtl As DataTable = Nothing
        Dim dtoSubmissionInfoEUDtl As DataTable = Nothing
        Dim dtoSubmissionInfoEUSubDtl As DataTable = Nothing
        Dim dtoSubmissionInfoEU14Dtl As DataTable = Nothing
        Dim dtoSubmissionInfoEU14SubDtl As DataTable = Nothing
        Dim dtoSubmissionInfoCADtl As DataTable = Nothing
        Dim dr_SubmissionInfoDtl As DataRow = Nothing
        Dim objSaveDb As New clsSaveDB

        Dim submissionId As String = Nothing
        Dim newLabelId As String = Nothing
        Dim wStr As String = String.Empty
        Dim destinationPath As String = String.Empty
        'Dim trackCMS() As String
        'Dim selectedCMS() As Integer


        Try
            eStr_Retu = ""
            If subType = "-1" Then
                Return "Save"
            End If

            If Not objHelpDb.getSubmissionInfo("vWorkspaceId='" + workspaceId + "'", _
                                                 DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                 dtoSubmissionMst, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving view_AllWorkspaceSubmissionInfo, " + eStr
                Exit Function
            End If

            dtoSubmissionMst.Rows(0)("vApplicationNo") = applicationNumber.ToString()
            dtoSubmissionMst.Rows(0)("vTrackingNo") = applicationNumber.ToString()
            dtoSubmissionMst.Rows(0)("SubmissionID") = ""


            '************************************Publish Logic US************************************
            If dtoSubmissionMst.Rows(0)("CountryRegionName").ToString.ToUpper = "US" Then

                'Creating label for workspace with PublishType 'P' only 

                If projectPublishType = "M" Then

                Else
                    If Not createWorkspaceLabel(workspaceId, userId, dtoInternalLabelMst, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while createWorkspaceLabel," + eStr
                        Exit Function
                    End If
                    newLabelId = dtoInternalLabelMst.Rows(0)("LabelId")
                End If

                If Not objHelpDb.getWorkspaceSubmissionInfoUSDtlBySubmissionId("", DataRetrievalModeEnum.DataTable_Empty, _
                                                                               dtoSubmissionInfoUSDtl, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving View_SubmissionInfoUSDtl, " + eStr
                    Exit Function
                End If

                dr_SubmissionInfoDtl = dtoSubmissionInfoUSDtl.NewRow

                dr_SubmissionInfoDtl("SubmissionInfoUSDtlId") = ""
                dr_SubmissionInfoDtl("WorkspaceId") = workspaceId
                dr_SubmissionInfoDtl("CountryCode") = dtoSubmissionMst.Rows(0)("vCountryCode").ToString()
                dr_SubmissionInfoDtl("CurrentSeqNumber") = currentSeqNumber
                dr_SubmissionInfoDtl("LastPublishedVersion") = lastPublishedVersion
                dr_SubmissionInfoDtl("SubmissionPath") = dtoSubmissionMst.Rows(0)("vApplicationNo").ToString()
                dr_SubmissionInfoDtl("SubmitedBy") = userId
                dr_SubmissionInfoDtl("SubmissionType") = subType

                dr_SubmissionInfoDtl("DateOfSubmission") = dos
                dr_SubmissionInfoDtl("RelatedSeqNo") = relatedSeqNumber
                dr_SubmissionInfoDtl("Confirm") = "N"
                dr_SubmissionInfoDtl("ModifyBy") = userId
                dr_SubmissionInfoDtl("LabelId") = newLabelId
                dr_SubmissionInfoDtl("SubmissionMode") = subMode
                dr_SubmissionInfoDtl("ApplicationNo") = applicationNumber

                dtoSubmissionInfoUSDtl.Rows.Add(dr_SubmissionInfoDtl)

                If Not objSaveDb.Insert_SubmissionInfoUSDtl(submissionId, DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                            dtoSubmissionInfoUSDtl, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while saving in Insert_SubmissionInfoUSDtl, " + eStr
                    Exit Function
                End If

                If Not objHelpDb.getWorkspaceSubmissionInfoUSDtlBySubmissionId("SubmissionInfoUSDtlId='" + submissionId + "'", _
                                                                               DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                               dtoSubmissionInfoUSDtl, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while Retrieving View_SubmissionInfoUSDtl, " + eStr
                    Exit Function
                End If

                destinationPath = dtoSubmissionInfoUSDtl.Rows(0)("SubmissionPath").ToString()

                If Not publishWorkspaceSubmission(dtoSubmissionMst, dtoInternalLabelMst, currentSeqNumber, relatedSeqNumber, _
                                               destinationPath, lastConfirmedSubmissionPath, isRMSSelected, _
                                               subVariationMode, addTT, Nothing, dos, userId, subType, baseWorkFolder, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while publishWorkspaceSubmission, " + eStr
                    Exit Function
                End If


                '''''''Project Published Successfully...''''''


                '************************************Publish Logic EU************************************
            ElseIf dtoSubmissionMst.Rows(0)("CountryRegionName").Equals("eu") Then

                If projectPublishType = "M" Then

                Else
                    If Not createWorkspaceLabel(workspaceId, userId, dtoInternalLabelMst, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while createWorkspaceLabel, " + eStr
                        Exit Function
                    End If
                    newLabelId = dtoInternalLabelMst.Rows(0)("LabelId")
                End If

                relatedSeqNumber = relatedSeqNumber.Replace(" ", "")

                'EU v1.3
                If (dtoSubmissionMst.Rows(0)("vRegionalDTDVersion").Equals("")) Then
                    If Not objHelpDb.getWorkspaceSubmissionInfoEUDtlBySubmissionId("", DataRetrievalModeEnum.DataTable_Empty, _
                                                                                     dtoSubmissionInfoEUDtl, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while retrieving View_SubmissionInfoEUDtl, " + eStr
                        Exit Function
                    End If

                    dr_SubmissionInfoDtl = dtoSubmissionInfoEUDtl.NewRow

                    dr_SubmissionInfoDtl("SubmissionInfoEUDtlId") = ""
                    dr_SubmissionInfoDtl("WorkspaceId") = workspaceId
                    dr_SubmissionInfoDtl("CountryCode") = dtoSubmissionMst.Rows(0)("vCountryCode").ToString()
                    dr_SubmissionInfoDtl("CurrentSeqNumber") = currentSeqNumber
                    dr_SubmissionInfoDtl("LastPublishedVersion") = lastPublishedVersion
                    dr_SubmissionInfoDtl("SubmissionPath") = dtoSubmissionMst.Rows(0)("vWorkspaceDesc").ToString()
                    dr_SubmissionInfoDtl("SubmitedBy") = userId
                    dr_SubmissionInfoDtl("SubmissionType") = subType

                    dr_SubmissionInfoDtl("DateOfSubmission") = dos
                    dr_SubmissionInfoDtl("RelatedSeqNo") = relatedSeqNumber
                    dr_SubmissionInfoDtl("Confirm") = "N"
                    dr_SubmissionInfoDtl("ModifyBy") = userId
                    dr_SubmissionInfoDtl("LabelId") = newLabelId
                    dr_SubmissionInfoDtl("SubmissionMode") = subMode
                    dr_SubmissionInfoDtl("ApplicationNo") = applicationNumber

                    If isRMSSelected = "N" Then
                        dr_SubmissionInfoDtl("RMSSubmited") = "N"    'For DCP and MRP
                    Else
                        isRMSSelected = "Y"                          'For NP,CP ,DCP and MRP 
                        dr_SubmissionInfoDtl("RMSSubmited") = "Y"
                    End If

                    dtoSubmissionInfoEUDtl.Rows.Add(dr_SubmissionInfoDtl)
                    If Not objSaveDb.Insert_SubmissionInfoEUDtl(submissionId, DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                                dtoSubmissionInfoEUDtl, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while saving Insert_SubmissionInfoEUDtl, " + eStr
                        Exit Function
                    End If

                    'EU v1.4
                ElseIf dtoSubmissionMst.Rows(0)("vRegionalDTDVersion").Equals("14") Then
                    If Not objHelpDb.getWorkspaceSubmissionInfoEU14DtlBySubmissionId("", DataRetrievalModeEnum.DataTable_Empty, _
                                                                                     dtoSubmissionInfoEU14Dtl, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while Retrieving View_SubmissionInfoEU14Dtl, " + eStr
                        Exit Function
                    End If

                    dr_SubmissionInfoDtl = dtoSubmissionInfoEU14Dtl.NewRow

                    dr_SubmissionInfoDtl("SubmissionInfoEU14DtlId") = ""
                    dr_SubmissionInfoDtl("WorkspaceId") = workspaceId
                    dr_SubmissionInfoDtl("CountryCode") = dtoSubmissionMst.Rows(0)("vCountryCode").ToString()
                    dr_SubmissionInfoDtl("CurrentSeqNumber") = currentSeqNumber
                    dr_SubmissionInfoDtl("LastPublishedVersion") = lastPublishedVersion
                    dr_SubmissionInfoDtl("SubmissionPath") = dtoSubmissionMst.Rows(0)("vWorkspaceDesc").ToString()
                    dr_SubmissionInfoDtl("SubmitedBy") = userId
                    dr_SubmissionInfoDtl("SubmissionType") = subType

                    dr_SubmissionInfoDtl("DateOfSubmission") = dos
                    dr_SubmissionInfoDtl("RelatedSeqNo") = relatedSeqNumber
                    dr_SubmissionInfoDtl("Confirm") = "N"
                    dr_SubmissionInfoDtl("ModifyBy") = userId
                    dr_SubmissionInfoDtl("LabelId") = newLabelId
                    dr_SubmissionInfoDtl("SubmissionMode") = subMode
                    dr_SubmissionInfoDtl("SubVariationMode") = IIf(subVariationMode.Equals("-1"), "", subVariationMode)
                    'DO if subVariationMode is worksharing or grouping set highleveltrackingno
                    dr_SubmissionInfoDtl("TrackingNo") = applicationNumber

                    If isRMSSelected = "N" Then
                        dr_SubmissionInfoDtl("RMSSubmited") = "N"    'For DCP and MRP
                    Else
                        isRMSSelected = "Y"                          'For NP,CP ,DCP and MRP 
                        dr_SubmissionInfoDtl("RMSSubmited") = "Y"
                    End If

                    dtoSubmissionInfoEU14Dtl.Rows.Add(dr_SubmissionInfoDtl)
                    If Not objSaveDb.Insert_SubmissionInfoEU14Dtl(submissionId, DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                                  dtoSubmissionInfoEU14Dtl, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while saving Insert_SubmissionInfoEU14Dtl, " + eStr
                        Exit Function
                    End If

                End If
                'Inserting CMS/RMS info into SubmissionInfoEUSubDtl 
                If IsDBNull(selectedCMS) Or IsNothing(selectedCMS) Then
                    'inserting for RMS in case of 'only RMS is selected'
                    'Passing '0' for RMS
                    selectedCMS = New Integer() {0}
                    trackCMS = New String() {applicationNumber}
                End If


                If dtoSubmissionMst.Rows(0)("vRegionalDTDVersion").Equals("") AndAlso _
                    Not objHelpDb.getWorkspaceSubmissionInfoEUSubDtl("", DataRetrievalModeEnum.DataTable_Empty, _
                                                                     dtoSubmissionInfoEUSubDtl, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while Retrieving View_SubmissionInfoEUSubDtl, " + eStr
                    Exit Function
                ElseIf dtoSubmissionMst.Rows(0)("vRegionalDTDVersion").Equals("14") AndAlso _
                    Not objHelpDb.getWorkspaceSubmissionInfoEU14SubDtl("", DataRetrievalModeEnum.DataTable_Empty, _
                                                                       dtoSubmissionInfoEU14SubDtl, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while Retrieving View_SubmissionInfoEU14SubDtl, " + eStr
                    Exit Function
                End If


                For i = 0 To selectedCMS.Length - 1 Step 1
                    'EU v1.3
                    If dtoSubmissionMst.Rows(0)("vRegionalDTDVersion").Equals("") Then
                        dtoSubmissionInfoEUSubDtl.Rows.Clear()
                        dr_SubmissionInfoDtl = dtoSubmissionInfoEUSubDtl.NewRow

                        dr_SubmissionInfoDtl("iWorkspaceCMSId") = selectedCMS(i)
                        dr_SubmissionInfoDtl("dDateOfSubmission") = dos
                        dr_SubmissionInfoDtl("vSubmissionDescription") = subDesc
                        dr_SubmissionInfoDtl("iModifyBy") = userId
                        dr_SubmissionInfoDtl("vSubmissionInfoEUDtlId") = submissionId

                        dtoSubmissionInfoEUSubDtl.Rows.Add(dr_SubmissionInfoDtl)
                        If Not objSaveDb.Insert_SubmissionInfoEUSubDtl(DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                                       dtoSubmissionInfoEUSubDtl, eStr) Then
                            eStr_Retu += vbCrLf + "Error occured while saving Insert_SubmissioInfoEUSubDtl, " + eStr
                            Exit Function
                        End If
                    End If

                    'EU v1.4
                    If dtoSubmissionMst.Rows(0)("vRegionalDTDVersion").Equals("14") Then
                        dtoSubmissionInfoEU14SubDtl.Rows.Clear()
                        dr_SubmissionInfoDtl = dtoSubmissionInfoEU14SubDtl.NewRow

                        dr_SubmissionInfoDtl("iWorkspaceCMSId") = selectedCMS(i)
                        dr_SubmissionInfoDtl("dDateOfSubmission") = dos
                        dr_SubmissionInfoDtl("vSubmissionDescription") = subDesc
                        dr_SubmissionInfoDtl("iModifyBy") = userId
                        dr_SubmissionInfoDtl("vSubmissionInfoEU14DtlId") = submissionId
                        dr_SubmissionInfoDtl("vPublishCMSTrackingNo") = trackCMS(i).Trim()

                        dtoSubmissionInfoEU14SubDtl.Rows.Add(dr_SubmissionInfoDtl)
                        If Not objSaveDb.Insert_SubmissionInfoEU14SubDtl(DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                                         dtoSubmissionInfoEU14SubDtl, eStr) Then
                            eStr_Retu += vbCrLf + "Error occured while saving Insert_SubmissionInfoEU14SubDtl, " + eStr
                            Exit Function
                        End If
                    End If
                Next i

                dtoSubmissionMst.Rows(0)("vSubmissionDescription") = subDesc 'for 'EU' submission only
                dtoSubmissionMst.Rows(0)("SubmissionId") = submissionId

                'Publishing Workspace
                If dtoSubmissionMst.Rows(0)("vRegionalDTDVersion").Equals("") Then
                    wStr = "SubmissionInfoEUDtlId='" + submissionId + "'"
                    If Not objHelpDb.getWorkspaceSubmissionInfoEUDtlBySubmissionId(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                                    dtoSubmissionInfoEUDtl, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while Retrieving View_SubmissionInfoEUDtl, " + eStr
                        Exit Function
                    End If
                    destinationPath = dtoSubmissionInfoEUDtl.Rows(0)("SubmissionPath").ToString()
                Else
                    wStr = "SubmissionInfoEU14DtlId='" + submissionId + "'"
                    If Not objHelpDb.getWorkspaceSubmissionInfoEU14DtlBySubmissionId(wStr, DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                                     dtoSubmissionInfoEU14Dtl, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while Retrieving View_SubmissionInfoEU14Dtl, " + eStr
                        Exit Function
                    End If
                    destinationPath = dtoSubmissionInfoEU14Dtl.Rows(0)("SubmissionPath").ToString()
                End If

                If Not publishWorkspaceSubmission(dtoSubmissionMst, dtoInternalLabelMst, currentSeqNumber, relatedSeqNumber, _
                                           destinationPath, lastConfirmedSubmissionPath, isRMSSelected, _
                                           subVariationMode, addTT, Nothing, dos, userId, subType, baseWorkFolder, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while PublishWorkspaceSubmission, " + eStr
                    Exit Function
                End If


                Console.WriteLine("Project Published Successfully...")

                '************************************Publish Logic CA************************************
            ElseIf dtoSubmissionMst.Rows(0)("CountryRegionName").Equals("ca") Then
                If projectPublishType = "M" Then

                Else
                    If Not createWorkspaceLabel(workspaceId, userId, dtoInternalLabelMst, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while createWorkspaceLabel, " + eStr
                        Exit Function
                    End If
                    newLabelId = dtoInternalLabelMst.Rows(0)("LabelId")
                End If

                relatedSeqNumber = relatedSeqNumber.Replace(" ", "")

                If Not objHelpDb.getWorkspaceSubmissionInfoCADtlBySubmissionId("", DataRetrievalModeEnum.DataTable_Empty, _
                                                                               dtoSubmissionInfoCADtl, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while View_SubmissionInfoCADtl, " + eStr
                    Exit Function
                End If

                dr_SubmissionInfoDtl = dtoSubmissionInfoCADtl.NewRow

                dr_SubmissionInfoDtl("SubmissionInfoCADtlId") = ""
                dr_SubmissionInfoDtl("WorkspaceId") = workspaceId
                dr_SubmissionInfoDtl("CountryCode") = dtoSubmissionMst.Rows(0)("vCountryCode").ToString()
                dr_SubmissionInfoDtl("CurrentSeqNumber") = currentSeqNumber
                dr_SubmissionInfoDtl("LastPublishedVersion") = lastPublishedVersion
                dr_SubmissionInfoDtl("SubmissionPath") = dtoSubmissionMst.Rows(0)("vApplicationNo").ToString()
                dr_SubmissionInfoDtl("SubmitedBy") = userId
                dr_SubmissionInfoDtl("SubmissionType") = subType

                dr_SubmissionInfoDtl("DateOfSubmission") = dos
                dr_SubmissionInfoDtl("RelatedSeqNo") = relatedSeqNumber
                dr_SubmissionInfoDtl("Confirm") = "N"
                dr_SubmissionInfoDtl("ModifyBy") = userId
                dr_SubmissionInfoDtl("LabelId") = newLabelId
                dr_SubmissionInfoDtl("SubmissionMode") = subMode
                dr_SubmissionInfoDtl("ApplicationNo") = applicationNumber

                dtoSubmissionInfoCADtl.Rows.Add(dr_SubmissionInfoDtl)

                If Not objSaveDb.Insert_SubmissionInfoCADtl(submissionId, DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                            dtoSubmissionInfoCADtl, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while saving Insert_SubmissionInfoCADtl, " + eStr
                    Exit Function
                End If

                If Not objHelpDb.getWorkspaceSubmissionInfoCADtlBySubmissionId("SubmissionInfoCADtlId='" + submissionId + "'", _
                                                                               DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                               dtoSubmissionInfoCADtl, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while retrieving View_SubmissionInfoCADtl, " + eStr
                    Exit Function
                End If

                destinationPath = dtoSubmissionInfoCADtl.Rows(0)("SubmissionPath").ToString()

                If Not publishWorkspaceSubmission(dtoSubmissionMst, dtoInternalLabelMst, currentSeqNumber, relatedSeqNumber, _
                                           destinationPath, lastConfirmedSubmissionPath, isRMSSelected, _
                                           subVariationMode, addTT, Nothing, dos, userId, subType, baseWorkFolder, eStr) Then
                    eStr_Retu += vbCrLf + "Error occured while publishWorkspaceSubmission, " + eStr
                    Exit Function
                End If

                Console.WriteLine("Project Published Successfully...")
            End If

            Return True
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while saveSubmissionDtl"
            saveSubmissionDtl = False
        End Try
    End Function
#End Region
#Region "createWorkspaceLabel"
    Friend Function createWorkspaceLabel(ByVal workspaceId As String, _
                                         ByVal userId As String, _
                                         ByRef dtoInternalLabelMst As DataTable, _
                                         ByRef eStr_Retu As String) As Boolean
        createWorkspaceLabel = False
        Dim objSaveDb As clsSaveDB = Nothing

        Dim maxLabelId As String = String.Empty
        Dim tempLabelIdString As String = String.Empty

        Dim tempLabelIdInteger As Integer
        eStr_Retu = ""

        Try
            If Not objHelpDb.getMaxWorkspaceLabel("WorkspaceId='" + workspaceId + "'", _
                                                  DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                  dtoInternalLabelMst, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving View_GetMaxWorkspaceLabel, " + eStr
                Exit Function
            End If

            maxLabelId = dtoInternalLabelMst.Rows(0)("LabelId").ToString()

            'Increment labelId by 1, e.g. L0001 to L0002
            tempLabelIdInteger = Convert.ToInt32(maxLabelId.Substring(maxLabelId.Length - 4)) 'Integer 0001
            tempLabelIdInteger = tempLabelIdInteger + 1 'integer 0002
            tempLabelIdString = "000" + tempLabelIdInteger.ToString() 'string 0002
            tempLabelIdString = maxLabelId.Substring(0, maxLabelId.Length - 4) + _
                                tempLabelIdString.Substring(tempLabelIdString.Length - 4) 'string L0001

            dtoInternalLabelMst.Rows(0)("LabelId") = tempLabelIdString
            dtoInternalLabelMst.Rows(0)("ModifyBy") = userId

            objSaveDb = New clsSaveDB
            If Not objSaveDb.createInternalLabel(DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                 dtoInternalLabelMst, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while saving createInternalLabel, " + eStr
                Exit Function
            End If

            If Not objHelpDb.getMaxWorkspaceLabel("WorkspaceId='" + workspaceId + "'", _
                                                  DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                  dtoInternalLabelMst, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving View_GetMaxWorkspaceLabel, " + eStr
                Exit Function
            End If

            Return True
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while createWorkspaceLabel"
            createWorkspaceLabel = False
        End Try
    End Function
#End Region
#Region "publishWorkspaceSubmission"
    Friend Function publishWorkspaceSubmission(ByRef dtoSubmissionMst As DataTable, _
                                               ByRef dtoInternalLabelMst As DataTable, _
                                               ByVal currentSeqNumber As String, _
                                               ByVal relatedSeqNumber As String, _
                                               ByVal publishDestinationPath As String, _
                                               ByVal sourcePath As String, _
                                               ByVal isRMSSelected As Char, _
                                               ByVal subVariationMode As String, _
                                               ByVal addTT As Char, _
                                               ByVal leafIds() As Integer, _
                                               ByVal dos As Date, _
                                               ByVal userId As Integer, _
                                               ByVal subType As String, _
                                               ByVal baseWorkFolder As String, _
                                               ByRef eStr_Retu As String) As Boolean
        publishWorkspaceSubmission = False

        Dim objworkspacePublishService_EU14 As clsWorkspacePublishService_EU14
        Dim objWorkspaceRevisedPublishService As clsWorkspaceRevisedPublishService
        Dim objWorkspacePublishService_ca As clsWorkspacePublishService_CA

        Dim publishForm As New DataTable()
        Dim dtoWorkspaceMst As New DataTable()

        Dim dr_publishForm As DataRow = Nothing

        Dim WorkspaceId As String = Nothing
        Dim wsDesc As String = Nothing
        Dim projectType As Char = Nothing


        Try
            eStr_Retu = ""
            WorkspaceId = dtoSubmissionMst.Rows(0)("vWorkspaceId")

            If Not Create_publishForm(publishForm, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while Create_PublishForm, " + eStr
                Exit Function
            End If
            dr_publishForm = publishForm.NewRow()

            'For EU Submission
            dr_publishForm("SubmissionId") = dtoSubmissionMst.Rows(0)("SubmissionId")
            dr_publishForm("WorkspaceId") = WorkspaceId
            dr_publishForm("LabelNo") = dtoInternalLabelMst.Rows(0)("LabelNo")
            dr_publishForm("LabelId") = dtoInternalLabelMst.Rows(0)("LabelId")

            dr_publishForm("SubmissionFlag") = dtoSubmissionMst.Rows(0)("CountryRegionName") 'for e.g. 'us', 'eu', 'ca' etc.
            dr_publishForm("SeqNumber") = currentSeqNumber
            dr_publishForm("PublishDestinationPath") = publishDestinationPath 'Changed for New Submission Path

            dr_publishForm("ApplicationNumber") = dtoSubmissionMst.Rows(0)("vApplicationNo")
            dr_publishForm("AgencyName") = dtoSubmissionMst.Rows(0)("vAgencyName")
            dr_publishForm("CompanyName") = dtoSubmissionMst.Rows(0)("vCompanyName")
            dr_publishForm("ApplicationType") = dtoSubmissionMst.Rows(0)("vApplicationType")
            dr_publishForm("ProcedureType") = dtoSubmissionMst.Rows(0)("vProcedureType")
            dr_publishForm("ProductType") = dtoSubmissionMst.Rows(0)("vProductType")

            dr_publishForm("SubmissionType_us") = subType
            dr_publishForm("ProductName") = dtoSubmissionMst.Rows(0)("vProductName")
            dr_publishForm("relatedseqnumber") = IIf(relatedSeqNumber Is Nothing, DBNull.Value, relatedSeqNumber)

            'For 'eu' submission
            dr_publishForm("Country") = dtoSubmissionMst.Rows(0)("CountryCodeName")
            dr_publishForm("Applicant") = dtoSubmissionMst.Rows(0)("vApplicant")
            dr_publishForm("Atc") = dtoSubmissionMst.Rows(0)("vAtc")

            dr_publishForm("SubmissionType_eu") = subType
            dr_publishForm("InventedName") = dtoSubmissionMst.Rows(0)("vInventedName")
            dr_publishForm("Inn") = dtoSubmissionMst.Rows(0)("vInn")
            dr_publishForm("SubmissionDescription") = dtoSubmissionMst.Rows(0)("vSubmissionDescription")
            dr_publishForm("SubmissionMode") = subVariationMode      'For EU v1.4
            dr_publishForm("HighLvlNo") = dtoSubmissionMst.Rows(0)("vHighLvlNo")
            dr_publishForm("PublishCMSTrackingNo") = dtoSubmissionMst.Rows(0)("vTrackingNo")

            'For 'ca' submission
            dr_publishForm("SubmissionType_ca") = subType

            If isRMSSelected = "Y" Or isRMSSelected = "N" Then ' // Column of saveSubmisisonDtl
                dr_publishForm("RMSSelected") = isRMSSelected
                dr_publishForm("AddTT") = addTT
            End If
            publishForm.Rows.Add(dr_publishForm)

            If Not objHelpDb.getWorkSpaceDetail("vWorkspaceId='" + WorkspaceId + "'", _
                                         DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                         dtoWorkspaceMst, eStr) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving View_CommonWorkspaceDetail, " + eStr
                Exit Function
            End If

            projectType = dtoWorkspaceMst.Rows(0)("cProjectType")
            wsDesc = dtoWorkspaceMst.Rows(0)("vWorkSpaceDesc")

            If projectType = "P" Or projectType = "M" Then
                If dtoSubmissionMst.Rows(0)("CountryRegionName") = "eu" And _
                   dtoSubmissionMst.Rows(0)("vRegionalDTDVersion") = "14" Then

                    objworkspacePublishService_EU14 = New clsWorkspacePublishService_EU14
                    objworkspacePublishService_EU14.projectPublishType = projectType

                    If projectType = "M" Then
                        objworkspacePublishService_EU14.leafIds = leafIds
                    End If

                    publishForm.Rows(0)("ApplicationNumber") = dtoSubmissionMst.Rows(0)("vTrackingNo")

                    If Not objworkspacePublishService_EU14.workspacePublish(WorkspaceId, publishForm, userId, dos, wsDesc, baseWorkFolder, eStr_Retu) Then
                        eStr_Retu += vbCrLf + "Error occured while workspacePublish, " + eStr
                        Exit Function
                    End If

                ElseIf dtoSubmissionMst.Rows(0)("CountryRegionName") = "us" Or _
                       dtoSubmissionMst.Rows(0)("CountryRegionName") = "eu" And _
                       dtoSubmissionMst.Rows(0)("vRegionalDTDVersion") = "" Then

                    objWorkspaceRevisedPublishService = New clsWorkspaceRevisedPublishService
                    objWorkspaceRevisedPublishService.projectPublishType = projectType

                    If projectType = "M" Then
                        objWorkspaceRevisedPublishService.leafIds = leafIds
                    End If

                    If Not objWorkspaceRevisedPublishService.workspacePublish(WorkspaceId, publishForm, userId, dos, wsDesc, baseWorkFolder, eStr_Retu) Then
                        eStr_Retu += vbCrLf + "Error occured while workspacePublish, " + eStr
                        Exit Function
                    End If

                ElseIf dtoSubmissionMst.Rows(0)("CountryRegionName") = "ca" Then
                    objWorkspacePublishService_ca = New clsWorkspacePublishService_CA
                    objWorkspacePublishService_ca.projectPublishType = projectType

                    If projectType = "M" Then
                        objWorkspacePublishService_ca.leafIds = leafIds
                    End If

                    If Not objWorkspacePublishService_ca.workspacePublish(WorkspaceId, publishForm, userId, dos, wsDesc, baseWorkFolder, eStr) Then
                        eStr_Retu += vbCrLf + "Error occured while workspacePublish, " + eStr
                        Exit Function
                    End If
                End If

                'Copy Previous Submission Sequence Folders
                If Not sourcePath Is Nothing AndAlso sourcePath <> "" Then
                    CopyDirectory(sourcePath, publishDestinationPath)
                End If
            End If
            publishWorkspaceSubmission = True

        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while publishWorkspaceSubmission"
            publishWorkspaceSubmission = False
        End Try
    End Function
    Private Function Create_publishForm(ByRef publishForm As DataTable, ByRef eStr_Retu As String) As Boolean
        Create_publishForm = False

        Try
            eStr_Retu = ""
            publishForm.Columns.Add(New DataColumn("SubmissionId", GetType(String)))
            publishForm.Columns.Add(New DataColumn("WorkspaceId", GetType(String)))
            publishForm.Columns.Add(New DataColumn("LabelNo", GetType(String)))
            publishForm.Columns.Add(New DataColumn("LabelId", GetType(String)))

            publishForm.Columns.Add(New DataColumn("SubmissionFlag", GetType(String)))
            publishForm.Columns.Add(New DataColumn("SeqNumber", GetType(String)))
            publishForm.Columns.Add(New DataColumn("PublishDestinationPath", GetType(String)))

            publishForm.Columns.Add(New DataColumn("ApplicationNumber", GetType(String)))
            publishForm.Columns.Add(New DataColumn("AgencyName", GetType(String)))
            publishForm.Columns.Add(New DataColumn("CompanyName", GetType(String)))
            publishForm.Columns.Add(New DataColumn("ApplicationType", GetType(String)))
            publishForm.Columns.Add(New DataColumn("ProcedureType", GetType(String)))
            publishForm.Columns.Add(New DataColumn("ProductType", GetType(String)))

            publishForm.Columns.Add(New DataColumn("SubmissionType_us", GetType(String)))
            publishForm.Columns.Add(New DataColumn("ProductName", GetType(String)))
            publishForm.Columns.Add(New DataColumn("RelatedSeqNumber", GetType(String)))

            publishForm.Columns.Add(New DataColumn("Country", GetType(String)))
            publishForm.Columns.Add(New DataColumn("Applicant", GetType(String)))
            publishForm.Columns.Add(New DataColumn("Atc", GetType(String)))

            publishForm.Columns.Add(New DataColumn("SubmissionType_eu", GetType(String)))
            publishForm.Columns.Add(New DataColumn("InventedName", GetType(String)))
            publishForm.Columns.Add(New DataColumn("Inn", GetType(String)))

            publishForm.Columns.Add(New DataColumn("SubmissionDescription", GetType(String)))
            publishForm.Columns.Add(New DataColumn("SubmissionMode", GetType(String)))
            publishForm.Columns.Add(New DataColumn("HighLvlNo", GetType(String)))

            publishForm.Columns.Add(New DataColumn("SubmissionType_ca", GetType(String)))
            publishForm.Columns.Add(New DataColumn("RMSSelected", GetType(String)))
            publishForm.Columns.Add(New DataColumn("AddTT", GetType(String)))
            publishForm.Columns.Add(New DataColumn("PublishCMSTrackingNo", GetType(String)))

            Return True
        Catch ex As Exception
            eStr_Retu += ex.Message + vbCrLf + "Error occured while Create_PublishForm"
            Create_publishForm = False
        End Try
    End Function
#End Region

    Public Function CopyDirectory(ByVal Src As String, ByVal Dest As String, Optional ByVal bQuiet As Boolean = False) As Boolean
        If Not Directory.Exists(Src) Then
            Throw New DirectoryNotFoundException("The directory " & Src & " does not exists")
        End If

        'add Directory Seperator Character (\) for the string concatenation shown later
        If Dest.Substring(Dest.Length - 1, 1) <> Path.DirectorySeparatorChar Then
            Dest += Path.DirectorySeparatorChar
        End If

        If Not Directory.Exists(Dest) Then Directory.CreateDirectory(Dest)

        Dim Files As String()
        Files = Directory.GetFileSystemEntries(Src)
        Dim element As String

        For Each element In Files
            If Directory.Exists(element) Then
                'if the current FileSystemEntry is a directory,
                'call this function recursively
                CopyDirectory(element, Dest & Path.GetFileName(element), True)
            Else
                'the current FileSystemEntry is a file so just copy it
                File.Copy(element, Dest & Path.GetFileName(element), True)
            End If
        Next
        Return True
    End Function


End Class