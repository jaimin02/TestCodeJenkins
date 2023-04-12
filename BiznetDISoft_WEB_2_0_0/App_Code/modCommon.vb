Imports Microsoft.VisualBasic

Public Module modCommon
    Public Function ConvertDbNullToDbTypeDefaultValue(ByVal Val As Object, ByVal DbType_1 As System.Type) As Object
        Dim DbTypeName_1 As String = ""
        Dim defDateTime As DateTime
        Dim DefChar As Char

        DbTypeName_1 = DbType_1.Name.ToUpper

        If DbTypeName_1 = "STRING" Then

            Try
                ConvertDbNullToDbTypeDefaultValue = Val.ToString
            Catch ex As Exception
                ConvertDbNullToDbTypeDefaultValue = ""
            End Try

        ElseIf DbTypeName_1 = "CHAR" Then

            Try
                ConvertDbNullToDbTypeDefaultValue = Convert.ToChar(Val)
            Catch ex As Exception
                ConvertDbNullToDbTypeDefaultValue = DefChar
            End Try

        ElseIf DbTypeName_1 = "DATETIME" Then

            Try
                ConvertDbNullToDbTypeDefaultValue = Convert.ToDateTime(Val)
            Catch ex As Exception
                ConvertDbNullToDbTypeDefaultValue = defDateTime
            End Try

        ElseIf DbTypeName_1 = "BOOLEAN" Then

            Try
                ConvertDbNullToDbTypeDefaultValue = Convert.ToBoolean(Val)
            Catch ex As Exception
                ConvertDbNullToDbTypeDefaultValue = False
            End Try

        ElseIf DbTypeName_1 = "DECIMAL" Or DbTypeName_1 = "INT16" Or DbTypeName_1 = "INT32" Or _
               DbTypeName_1 = "INT64" Or DbTypeName_1 = "DOUBLE" Or DbTypeName_1 = "BYTE" Then

            If IsDBNull(Val) Then
                ConvertDbNullToDbTypeDefaultValue = 0
            Else
                ConvertDbNullToDbTypeDefaultValue = Val
            End If

        End If


    End Function
End Module
