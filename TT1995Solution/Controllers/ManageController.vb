Imports System.Data.SqlClient
Imports System.Web.Mvc
Imports System.Web.Script.Serialization

Namespace Controllers
    Public Class ManageController
        Inherits Controller

        ' GET: Manage
        Function Lookup() As ActionResult
            Return View()
        End Function

        Public Function DeleteLookup(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [lookup] WHERE lookup_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetLookup() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT ct.name_table, cc.name_column, lu.data_list FROM [lookup] as lu join config_column as cc on lu.column_id = cc.column_id join config_table as ct on cc.table_id = ct.table_id"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertLicense() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = ""
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
    End Class
End Namespace