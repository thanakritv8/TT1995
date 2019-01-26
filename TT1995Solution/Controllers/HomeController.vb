Imports System.Data.SqlClient
Imports System.Web.Mvc
Imports System.Web.Script.Serialization

Namespace Controllers
    Public Class HomeController
        Inherits Controller

        ' GET: Home
        Function Index() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        Function License() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

#Region "License"
        Public Function GetLicense() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password)
            Dim _SQL As String = "SELECT * FROM [TT1995].[dbo].[license] ORDER BY number_car"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetColumnChooser() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password)
            'Dim _SQL As String = "SELECT cc1.column_id, cc1.name_column AS dataField, cc1.display AS caption, cc1.data_type AS dataType, cc1.alignment, cc1.width, ISNULL(cc2.visible,0) AS visible FROM [TT1995].[dbo].[config_column] AS cc1 LEFT JOIN [TT1995].[dbo].[chooser_column] AS cc2 ON cc1.column_id = cc2.column_id WHERE cc2.user_id = " & Session("UserId")
            Dim _SQL As String = "SELECT name_column AS dataField, display AS caption, data_type AS dataType, alignment, width, ISNULL(visible,0) AS visible, fixed FROM [TT1995].[dbo].[config_column] WHERE name_column <> 'license_id'"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertColumnChooser(ByVal ColumnId As Integer) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password)
            Dim _SQL As String = "SELECT * FROM [TT1995].[dbo].[chooser_column] WHERE user_id = " & Session("UserId") & " AND column_id = " & ColumnId
            Dim DtCC As DataTable = objDB.SelectSQL(_SQL, cn)
            ''''''''''''''''''''''''''''''''''

            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateLicense() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertLicense() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteLicense() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


#End Region

    End Class
End Namespace