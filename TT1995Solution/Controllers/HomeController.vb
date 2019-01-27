Imports System.Data.SqlClient
Imports System.Web.Mvc
Imports System.Web.Script.Serialization

Namespace Controllers
    Public Class HomeController
        Inherits Controller

        ' GET: Home
        Function Index() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View("../Home/License")
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
            Dim _SQL As String = "SELECT name_column AS dataField, display AS caption, data_type AS dataType, alignment, width, ISNULL(visible,0) AS visible, fixed, colSpan FROM [TT1995].[dbo].[config_column] WHERE name_column <> 'license_id' ORDER BY sort ASC"
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

        Public Function UpdateLicense(ByVal number_car As String, ByVal license_car As String, ByVal province As String _
                                      , ByVal type_fuel As String, ByVal type_car As String, ByVal style_car As String _
                                      , ByVal brand_car As String, ByVal model_car As String, ByVal color_car As String _
                                      , ByVal number_body As String, ByVal number_engine As String, ByVal number_engine_point_1 As String _
                                      , ByVal number_engine_point_2 As String, ByVal brand_engine As String, ByVal pump As String _
                                      , ByVal horse_power As String, ByVal shaft As String, ByVal wheel As String, ByVal tire As String _
                                      , ByVal license_date As String, ByVal weight_car As String, ByVal weight_lade As String _
                                      , ByVal weight_total As String, ByVal ownership As String, ByVal transport_operator As String, ByVal transport_type As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")

            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertLicense(ByVal number_car As String, ByVal license_car As String, ByVal province As String _
                                      , ByVal type_fuel As String, ByVal type_car As String, ByVal style_car As String _
                                      , ByVal brand_car As String, ByVal model_car As String, ByVal color_car As String _
                                      , ByVal number_body As String, ByVal number_engine As String, ByVal number_engine_point_1 As String _
                                      , ByVal number_engine_point_2 As String, ByVal brand_engine As String, ByVal pump As String _
                                      , ByVal horse_power As String, ByVal shaft As String, ByVal wheel As String, ByVal tire As String _
                                      , ByVal license_date As String, ByVal weight_car As String, ByVal weight_lade As String _
                                      , ByVal weight_total As String, ByVal ownership As String, ByVal transport_operator As String, ByVal transport_type As String) As String
            If Not license_car Is Nothing Then

            End If
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password)
            Dim _SQL As String = "INSERT INTO [TT1995].[dbo].[license] ([number_car],[license_car],[province],[type_fuel],[type_car],[style_car],[brand_car],[model_car],[color_car],[number_body],[number_engine],[number_engine_point_1],[number_engine_point_2],[brand_engine],[pump],[horse_power],[shaft],[wheel],[tire],[license_date],[weight_car],[weight_lade],[weight_total],[ownership],[transport_operator],[transport_type])"
            _SQL &= " VALUES (" & IIf(number_car Is Nothing, 0, number_car) & ","
            _SQL &= "N'" & IIf(license_car Is Nothing, String.Empty, license_car) & "',"
            _SQL &= "N'" & IIf(province Is Nothing, String.Empty, province) & "',"
            _SQL &= "N'" & IIf(type_fuel Is Nothing, String.Empty, type_fuel) & "',"
            _SQL &= "N'" & IIf(type_car Is Nothing, String.Empty, type_car) & "',"
            _SQL &= "N'" & IIf(style_car Is Nothing, String.Empty, style_car) & "',"
            _SQL &= "N'" & IIf(brand_car Is Nothing, String.Empty, brand_car) & "',"
            _SQL &= "N'" & IIf(model_car Is Nothing, String.Empty, model_car) & "',"
            _SQL &= "N'" & IIf(color_car Is Nothing, String.Empty, color_car) & "',"
            _SQL &= "N'" & IIf(number_body Is Nothing, String.Empty, number_body) & "',"
            _SQL &= "N'" & IIf(number_engine Is Nothing, String.Empty, number_engine) & "',"
            _SQL &= "N'" & IIf(number_engine_point_1 Is Nothing, String.Empty, number_engine_point_1) & "',"
            _SQL &= "N'" & IIf(number_engine_point_2 Is Nothing, String.Empty, number_engine_point_2) & "',"
            _SQL &= "N'" & IIf(brand_engine Is Nothing, String.Empty, brand_engine) & "',"
            _SQL &= IIf(pump Is Nothing, 0, pump) & ","
            _SQL &= IIf(horse_power Is Nothing, 0, horse_power) & ","
            _SQL &= IIf(shaft Is Nothing, 0, shaft) & ","
            _SQL &= IIf(wheel Is Nothing, 0, wheel) & ","
            _SQL &= IIf(tire Is Nothing, 0, tire) & ","
            _SQL &= "'" & IIf(license_date Is Nothing, Now, license_date) & "',"
            _SQL &= IIf(weight_car Is Nothing, 0, weight_car) & ","
            _SQL &= IIf(weight_lade Is Nothing, 0, weight_lade) & ","
            _SQL &= IIf(weight_total Is Nothing, 0, weight_total) & ","
            _SQL &= "N'" & IIf(ownership Is Nothing, String.Empty, ownership) & "',"
            _SQL &= "N'" & IIf(transport_operator Is Nothing, String.Empty, transport_operator) & "',"
            _SQL &= "N'" & IIf(transport_type Is Nothing, String.Empty, transport_type) & "')"
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

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