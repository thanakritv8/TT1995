Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Mvc
Imports System.Web.Script.Serialization

Namespace Controllers
    Public Class HomeController
        Inherits Controller

        ' GET: Home
        Function Index() As ActionResult
            If Session("StatusLogin") = "1" Then
                SetDataOfConfigColumnData()
                Return View("../Home/Index")
            Else
                Return View("../Account/Login")
            End If
        End Function

#Region "Develop by Thung"

#Region "Index"

        Public Function GetIndex()
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT * from license"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function GetSubIndex(ByVal license_id As Integer)
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "select l.license_id, N'ภาษี' as tablename, '' as data_number, t.tax_startdate as date_start, t.tax_expire as date_expire, isnull(t.tax_status, '') as data_status from license as l join tax as t on l.license_id = t.license_id where l.license_id = " & license_id
            _SQL &= "union select l.license_id, N'ใบประกอบการภายในประเทศ' as tablename, bi.business_number as data_number, bi.business_start as date_start, bi.business_expire as date_expire, isnull(bi.business_status, '') as data_status from license as l join business_in_permission as bip on l.license_id = bip.license_id join business_in as bi on bip.business_id = bi.business_id where l.license_id = " & license_id
            _SQL &= "union select l.license_id, N'ใบประกอบการภายนอกประเทศ' as tablename, bo.business_number as data_number, bo.business_start as date_start, bo.business_expire as date_expire, isnull(bo.business_status, '') as data_status from license as l join business_out_permission as bop on l.license_id = bop.license_id join business_out as bo on bop.business_id = bo.business_id where l.license_id = " & license_id
            _SQL &= "union select l.license_id, N'ใบอนุญาตกัมพูชา' as tablename, lc.lc_number as data_number, lc.lc_start as date_start, lc.lc_expire as date_expire, isnull(lc.lc_status, '') as data_status from license as l join license_cambodia_permission as lcp on l.license_id = lcp.license_id_head join license_cambodia as lc on lcp.lc_id = lc.lc_id where l.license_id = " & license_id
            _SQL &= "union select l.license_id, N'ใบอนุญาตลุ่มแม่น้ำโขง' as tablename, lmr.lmr_number as data_number, lmr.lmr_start as date_start, lmr.lmr_expire as date_expire, isnull(lmr.lmr_status, '') as data_status from license as l join license_mekong_river_permission as lmrp on l.license_id = lmrp.license_id_head join license_mekong_river as lmr on lmrp.lmr_id = lmr.lmr_id where l.license_id = " & license_id
            _SQL &= "union select l.license_id, N'ใบอนุญาต(วอ.8)' as tablename, lv8.lv8_number as data_number, lv8.lv8_start as date_start, lv8.lv8_expire as date_expire, isnull(lv8.lv8_status, '') as data_status from license as l join license_v8 as lv8 on l.license_id = lv8.license_id where l.license_id = " & license_id
            _SQL &= "union select l.license_id, N'ประกันพรบ.' as tablename, ai.policy_number as data_number, ai.start_date as date_start, ai.end_date as date_expire, isnull(ai.status, '') as data_status from license as l join act_insurance as ai on l.license_id = ai.license_id where l.license_id = " & license_id
            _SQL &= "union select l.license_id, N'ประกันภัยรถยนต์' as tablename, mi.policy_number as data_number, mi.start_date as date_start, mi.end_date as date_expire, isnull(mi.status, '') as data_status from license as l join main_insurance as mi on l.license_id = mi.license_id where l.license_id = " & license_id
            _SQL &= "union select l.license_id, N'ประกันภัยสินค้าภายในประเทศ' as tablename, dpi.policy_number as data_number, dpi.start_date as date_start, dpi.end_date as date_expire, isnull(dpi.status, '') as data_status from license as l join domestic_product_insurance as dpi on l.license_id = dpi.license_id where l.license_id = " & license_id
            _SQL &= "union select l.license_id, N'ประกันภัยสิ่งแวดล้อม' as tablename, ei.policy_number as data_number, ei.start_date as date_start, ei.end_date as date_expire, isnull(ei.status, '') as data_status from license as l join environment_insurance as ei on l.license_id = ei.license_id where l.license_id = " & license_id
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function GetLicensePic(ByVal license_id As Integer)
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "select top 1 CASE WHEN (select count(file_id) from files_all where table_id = 1 and fk_id = " & license_id & " and type_file = 'pic' and position in ('1','2','3','4','5','6','7','8')) >= 8 THEN N'สมบูรณ์' ELSE N'ยังไม่มีการดำเนินการ' END as data_status, N'รูปรถ' as tablename, f1.name_file as n1, f1.path_file as p1, f2.name_file as n2, f2.path_file as p2, f3.name_file as n3, f3.path_file as p3, f4.name_file as n4, f4.path_file as p4, f5.name_file as n5, f5.path_file as p5, f6.name_file as n6, f6.path_file as p6, f7.name_file as n7, f7.path_file as p7, f8.name_file as n8, f8.path_file as p8 from files_all as f1 join files_all as f2 on f1.fk_id = f2.fk_id join files_all as f3 on f1.fk_id = f3.fk_id join files_all as f4 on f1.fk_id = f4.fk_id join files_all as f5 on f1.fk_id = f5.fk_id join files_all as f6 on f1.fk_id = f6.fk_id join files_all as f7 on f1.fk_id = f7.fk_id join files_all as f8 on f1.fk_id = f8.fk_id join files_all as f on f1.fk_id = f.fk_id where f1.position = '1' and f2.position = '2' and f3.position = '3' and f4.position = '4' and f5.position = '5' and f6.position = '6' and f7.position = '7' and f8.position = '8' and f1.table_id = 1 and f1.fk_id = " & license_id & " and f1.type_file = 'pic' "
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetDocAll(ByVal license_id As Integer)
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT N'ทั่วไป' as kind, name_file, path_file from files_all where table_id = 1 and type_file = 'pdf' and fk_id = " & license_id & " union all
SELECT N'ภาษี' as kind, name_file, path_file FROM files_all as f join tax as l on f.fk_id = l.license_id where f.table_id = 3 and type_file <> 'folder' and l.license_id = " & license_id & " union all
SELECT N'ทางด่วน' as kind, name_file, path_file FROM files_all as f join expressway as l on f.fk_id = l.epw_id where f.table_id = 9 and type_file <> 'folder' and l.license_id = " & license_id & " union all
SELECT N'ประกันภัยรถยนต์' as kind, name_file, path_file FROM files_all as f join main_insurance as l on f.fk_id = l.mi_id where f.table_id = 15 and type_file <> 'folder' and l.license_id = " & license_id & " union all
SELECT N'ประกันภัยสิ่งแวดล้อม' as kind, name_file, path_file FROM files_all as f join environment_insurance as l on f.fk_id = l.ei_id where f.table_id = 18 and type_file <> 'folder' and l.license_id = " & license_id & " union all
SELECT N'GPS ติดรถ' as kind, name_file, path_file FROM files_all as f join gps_car as l on f.fk_id = l.gps_car_id where f.table_id = 10 and type_file <> 'folder' and l.license_id = " & license_id & " union all
SELECT N'บันทึกเจ้าหน้าที่' as kind, name_file, path_file FROM files_all as f join officer_records as l on f.fk_id = l.or_id where f.table_id = 4 and type_file <> 'folder' and l.license_id = " & license_id & " union all
SELECT N'ประกันพรบ.' as kind, name_file, path_file FROM files_all as f join act_insurance as l on f.fk_id = l.ai_id where f.table_id = 17 and type_file <> 'folder' and l.license_id = " & license_id & " union all
SELECT N'ประกอบการภายใน' as kind, business_number as name_file, business_path as path_file FROM business_in_permission as bip join business_in as bi on bip.business_id = bi.business_id where bi.business_path is not null and bip.license_id = " & license_id & " union all
SELECT N'ประกอบการภายนอก' as kind, business_number as name_file, business_path as path_file FROM business_out_permission as bop join business_out as bo on bop.business_id = bo.business_id where bo.business_path is not null and bop.license_id = " & license_id & " union all
SELECT N'ใบอนุญาตกัมพูชา' as kind, lc_number as name_file, lc_path as path_file FROM license_cambodia_permission as lcp join license_cambodia as lc on lcp.lc_id	= lc.lc_id where lc.lc_path is not null and lcp.license_id_head = " & license_id & " union all
SELECT N'ใบอนุญาตลุ่มแม่น้ำโขง' as kind, lmr_number as name_file, lmr_path as path_file FROM license_mekong_river_permission as lmrp join license_mekong_river as lmr on lmrp.lmr_id	= lmr.lmr_id where lmr.lmr_path is not null and lmrp.license_id_head = " & license_id & " union all
SELECT N'ใบอนุญาต(วอ.8)' as kind, lv8_number as name_file, [path] as path_file from license_v8 where [path] is not null and license_id = " & license_id
            Dim DtDocAll As DataTable = objDB.SelectSQL(_SQL, cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtDocAll.Rows Select DtDocAll.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
#End Region

#Region "License"

        Function License() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
            'Session("UserId") = 1
            'Return View()
        End Function

        Public Function GetColumnChooserLicense(ByVal table_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            'Dim _SQL As String = "Select cc1.column_id, cc1.name_column As dataField, cc1.display As caption, cc1.data_type As dataType, cc1.alignment, cc1.width, ISNULL(cc2.visible, 0) As visible FROM [config_column] As cc1 LEFT JOIN [chooser_column] As cc2 On cc1.column_id = cc2.column_id WHERE cc2.user_id = " & Session("UserId")
            'Dim _SQL As String = "Select column_id, name_column As dataField, display As caption, data_type As dataType, alignment, width, ISNULL(visible, 0) As visible, fixed, Format, colSpan FROM [config_column] WHERE name_column <> 'license_id' ORDER BY sort ASC"
            Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(cc.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id WHERE cc.name_column <> 'license_id' AND table_id = " & table_id & " ORDER BY cc.sort ASC"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetLicense() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT [license_id],[number_car],[license_car],[province],[type_fuel],[type_car],[style_car],[brand_car],[model_car],[color_car],[number_body],[number_engine],[number_engine_point_1],[number_engine_point_2],[brand_engine],[pump],[horse_power],[shaft],[wheel],[tire],[license_date],[weight_car],[weight_lade],[weight_total],[ownership],[transport_operator],[transport_type],FORMAT([create_date], 'yyyy-MM-dd'),[create_by_user_id],[update_date],[update_by_user_id], [seq], [nationality], [id_card], [address], [license_expiration], [possessory_right], [license_no], [license_status],N'ประวัติ' as history,[fleet], license_location, internal_call FROM [license] ORDER BY LEN(number_car),number_car"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        'Public Function InsertColumnChooser(ByVal ColumnId As Integer) As String
        '    Dim DtJson As DataTable = New DataTable
        '    DtJson.Columns.Add("Status")
        '    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        '    Dim _SQL As String = "SELECT * FROM [chooser_column] WHERE user_id = " & Session("UserId") & " AND column_id = " & ColumnId
        '    Dim DtCC As DataTable = objDB.SelectSQL(_SQL, cn)
        '    ''''''''''''''''''''''''''''''''''

        '    Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        'End Function

        Public Function UpdateLicense(ByVal number_car As String, ByVal license_car As String, ByVal province As String _
                                      , ByVal type_fuel As String, ByVal type_car As String, ByVal style_car As String _
                                      , ByVal brand_car As String, ByVal model_car As String, ByVal color_car As String _
                                      , ByVal number_body As String, ByVal number_engine As String, ByVal number_engine_point_1 As String _
                                      , ByVal number_engine_point_2 As String, ByVal brand_engine As String, ByVal pump As String _
                                      , ByVal horse_power As String, ByVal shaft As String, ByVal wheel As String, ByVal tire As String _
                                      , ByVal license_date As String, ByVal weight_car As String, ByVal weight_lade As String _
                                      , ByVal weight_total As String, ByVal ownership As String, ByVal transport_operator As String, ByVal transport_type As String, ByVal key As String _
                                      , ByVal seq As String, ByVal nationality As String, ByVal id_card As String, ByVal address As String _
                                      , ByVal license_expiration As String, ByVal possessory_right As String, ByVal license_no As String, ByVal license_status As String, ByVal IdTable As String, ByVal fleet As String, ByVal license_location As String, ByVal internal_call As String) As String

            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [license] SET "
            Dim StrTbLicense() As String = {"number_car", "license_car", "province", "type_fuel", "type_car", "style_car", "brand_car", "model_car", "color_car", "number_body", "number_engine", "number_engine_point_1", "number_engine_point_2", "brand_engine", "pump", "horse_power", "shaft", "wheel", "tire", "license_date", "weight_car", "weight_lade", "weight_total", "ownership", "transport_operator", "transport_type", "seq", "nationality", "id_card", "address", "license_expiration", "possessory_right", "license_no", "license_status", "fleet", "license_location", "internal_call"}
            Dim TbLicense() As Object = {number_car, license_car, province, type_fuel, type_car, style_car, brand_car, model_car, color_car, number_body, number_engine, number_engine_point_1, number_engine_point_2, brand_engine, pump, horse_power, shaft, wheel, tire, license_date, weight_car, weight_lade, weight_total, ownership, transport_operator, transport_type, seq, nationality, id_card, address, license_expiration, possessory_right, license_no, license_status, fleet, license_location, internal_call}
            For n As Integer = 0 To TbLicense.Length - 1
                If Not TbLicense(n) Is Nothing Then
                    _SQL &= StrTbLicense(n) & "=N'" & TbLicense(n).Insert(TbLicense(n).IndexOf("'") + 1, "'") & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE license_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbLicense.Length - 1
                    If Not TbLicense(n) Is Nothing Then
                        GbFn.KeepLog(StrTbLicense(n), TbLicense(n), "Editing", IdTable, key)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertLicense(ByVal number_car As String, ByVal license_car As String, ByVal province As String _
                                      , ByVal type_fuel As String, ByVal type_car As String, ByVal style_car As String _
                                      , ByVal brand_car As String, ByVal model_car As String, ByVal color_car As String _
                                      , ByVal number_body As String, ByVal number_engine As String, ByVal number_engine_point_1 As String _
                                      , ByVal number_engine_point_2 As String, ByVal brand_engine As String, ByVal pump As String _
                                      , ByVal horse_power As String, ByVal shaft As String, ByVal wheel As String, ByVal tire As String _
                                      , ByVal license_date As String, ByVal weight_car As String, ByVal weight_lade As String _
                                      , ByVal weight_total As String, ByVal ownership As String, ByVal transport_operator As String, ByVal transport_type As String _
                                      , ByVal seq As String, ByVal nationality As String, ByVal id_card As String, ByVal address As String _
                                      , ByVal license_expiration As String, ByVal possessory_right As String, ByVal license_no As String, ByVal license_status As String, ByVal IdTable As String, ByVal fleet As String, ByVal license_location As String, ByVal internal_call As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO [license] ([number_car],[license_car],[province],[type_fuel],[type_car],[style_car],[brand_car],[model_car],[color_car],[number_body],[number_engine],[number_engine_point_1],[number_engine_point_2],[brand_engine],[pump],[horse_power],[shaft],[wheel],[tire],[license_date],[weight_car],[weight_lade],[weight_total],[ownership],[transport_operator],[transport_type], [seq], [nationality], [id_card], [address], [license_expiration], [possessory_right], [license_no], [license_status],[fleet],[license_location],[create_by_user_id],[internal_call]) OUTPUT Inserted.license_id"
            _SQL &= " VALUES ('" & IIf(number_car Is Nothing, 0, number_car) & "',"
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
            _SQL &= "N'" & IIf(transport_type Is Nothing, String.Empty, transport_type) & "',"
            _SQL &= IIf(seq Is Nothing, 0, seq) & ","
            _SQL &= "N'" & IIf(nationality Is Nothing, String.Empty, nationality) & "',"
            _SQL &= "N'" & IIf(id_card Is Nothing, String.Empty, id_card) & "',"
            _SQL &= "N'" & IIf(address Is Nothing, String.Empty, address) & "',"
            _SQL &= "N'" & IIf(license_expiration Is Nothing, String.Empty, license_expiration) & "',"
            _SQL &= "N'" & IIf(possessory_right Is Nothing, String.Empty, possessory_right) & "',"
            _SQL &= "N'" & IIf(license_no Is Nothing, String.Empty, license_no) & "',"
            _SQL &= "N'" & IIf(license_status Is Nothing, String.Empty, license_status) & "',"
            _SQL &= "N'" & IIf(fleet Is Nothing, String.Empty, fleet) & "',"
            _SQL &= "N'" & IIf(license_location Is Nothing, String.Empty, license_location) & "',"
            _SQL &= Session("UserId") & ","
            _SQL &= "N'" & internal_call.Insert(internal_call.IndexOf("'") + 1, "'") & "')"

            If Not number_car Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                'If objDB.ExecuteSQL(_SQL, cn) Then
                '    DtJson.Rows.Add("1")
                'Else
                '    DtJson.Rows.Add("0")
                'End If
                If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                    Dim StrTbLicense() As String = {"number_car", "license_car", "province", "type_fuel", "type_car", "style_car", "brand_car", "model_car", "color_car", "number_body", "number_engine", "number_engine_point_1", "number_engine_point_2", "brand_engine", "pump", "horse_power", "shaft", "wheel", "tire", "license_date", "weight_car", "weight_lade", "weight_total", "ownership", "transport_operator", "transport_type", "seq", "nationality", "id_card", "address", "license_expiration", "possessory_right", "license_no", "license_status", "fleet", "license_location"}
                    Dim TbLicense() As Object = {number_car, license_car, province, type_fuel, type_car, style_car, brand_car, model_car, color_car, number_body, number_engine, number_engine_point_1, number_engine_point_2, brand_engine, pump, horse_power, shaft, wheel, tire, license_date, weight_car, weight_lade, weight_total, ownership, transport_operator, transport_type, seq, nationality, id_card, address, license_expiration, possessory_right, license_no, license_status, fleet, license_location}
                    For n As Integer = 0 To TbLicense.Length - 1
                        If Not TbLicense(n) Is Nothing Then
                            GbFn.KeepLog(StrTbLicense(n), TbLicense(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                        End If
                    Next
                End If

            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertFileLicense() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim fk_id As String = String.Empty
                Dim parentDirId As String = String.Empty
                Dim newFolder As String = String.Empty
                Dim position As String = String.Empty
                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "parentDirId" Then
                            parentDirId = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "newFolder" Then
                            newFolder = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "position" Then
                            position = Request.Form(i)
                        End If
                    Next

                    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                    If Request.Files.Count = 0 Then
                        Dim _Path As String = fnGetPath(parentDirId)
                        Dim pathServer As String = Server.MapPath("~/Files/License/Root/" & fk_id & "/" & IIf(_Path = String.Empty, String.Empty, _Path & "/") & newFolder)
                        If (Not System.IO.Directory.Exists(pathServer)) Then
                            System.IO.Directory.CreateDirectory(pathServer)
                        End If
                        Dim _SQL As String = "INSERT INTO [files_all] ([fk_id],[table_id],[name_file],[type_file],[path_file],[parentDirId],[icon],[create_by_user_id],[position]) VALUES (" & fk_id & ",1,N'" & newFolder & "','folder',N'','" & parentDirId & "','../Img/folder.png'," & Session("UserId") & ", '" & position & "')"
                        objDB.ExecuteSQL(_SQL, cn)
                    Else
                        'Create File
                        For i As Integer = 0 To Request.Files.Count - 1
                            Dim file = Request.Files(i)
                            Dim _Path As String = fnGetPath(parentDirId)
                            Dim PathFile As String = "/Files/License/Root/" & fk_id & "/" & IIf(_Path = String.Empty, String.Empty, _Path & "/") & file.FileName
                            Dim type_file As String = Path.GetExtension(PathFile)
                            Dim pathServer As String = Server.MapPath("~" & PathFile)
                            Dim name_icon As String = String.Empty

                            Dim pathCheckId = Server.MapPath("~/Files/License/Root/" & fk_id)
                            If (Not System.IO.Directory.Exists(pathCheckId)) Then
                                System.IO.Directory.CreateDirectory(pathCheckId)
                            End If

                            If type_file = ".pdf" Then
                                name_icon = "pdf"
                            Else
                                name_icon = "pic"
                            End If
                            file.SaveAs(pathServer)
                            Dim _SQL As String = "INSERT INTO [files_all] ([fk_id], [table_id], [name_file], [type_file], [path_file], [parentDirId], [icon], [create_by_user_id],[position]) VALUES (" & fk_id & ",1, N'" & file.FileName & "','" & name_icon & "',N'.." & PathFile & "','" & parentDirId & "','../Img/" & name_icon & ".png'," & Session("UserId") & ", '" & position & "')"
                            objDB.ExecuteSQL(_SQL, cn)
                        Next
                    End If

                    objDB.DisconnectDB(cn)
                    DtJson.Rows.Add(fk_id)
                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function fnRenameLicense() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim dtStatus As DataTable = New DataTable
                Dim file_id As String = String.Empty
                Dim NewName As String = String.Empty
                Dim fk_id As String = String.Empty

                dtStatus.Columns.Add("Status")
                Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                cn.Open()
                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "file_id" Then
                            file_id = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "rename" Then
                            NewName = Request.Form(i)
                        End If
                    Next
                Else
                    DtJson.Rows.Add("0")
                End If
                Dim _Path As String = fnGetPath(file_id)
                Dim PathServer As String = Server.MapPath("~/Files/License/Root/" & fk_id & "/" & _Path)
                Try
                    If Directory.Exists(PathServer) Then
                        FileIO.FileSystem.RenameDirectory(PathServer, NewName)
                    Else
                        If System.IO.File.Exists(PathServer) Then
                            FileIO.FileSystem.RenameFile(PathServer, NewName & Path.GetExtension(_Path))
                        End If
                    End If
                Catch ex As Exception

                End Try

                Dim LastNameFile As String = Path.GetExtension(_Path)
                Dim NewNameFull As String = String.Empty

                If LastNameFile = ".pdf" Or LastNameFile = ".png" Or LastNameFile = ".jpg" Then
                    NewNameFull = NewName & LastNameFile
                Else
                    NewNameFull = NewName
                End If

                Dim ArrPath() As String = _Path.Split("/")
                Dim PathOld As String = ("../Files/License/Root/" & fk_id & "/" & _Path)
                Dim PathNew As String = ("../Files/License/Root/" & fk_id & "/")
                If ArrPath.Length > 1 Then
                    For i As Integer = 0 To ArrPath.Length - 2
                        If i = ArrPath.Length - 2 Then
                            PathNew &= ArrPath(i) & "/" & NewNameFull
                        Else
                            PathNew &= ArrPath(i) & "/"
                        End If
                    Next
                Else
                    'For child Root
                    PathNew = ("../Files/License/Root/" & fk_id & "/" & NewNameFull)
                End If


                Dim _SQL As String = "Update files_all Set name_file = N'" & NewNameFull & "', expanded = 1 WHERE file_id = '" & file_id & "'"
                If objDB.ExecuteSQL(_SQL, cn) Then
                    _SQL = "Update files_all SET path_file = REPLACE(path_file,N'" & PathOld & "', N'" & PathNew & "') WHERE path_file like N'%" & PathOld & "%'"
                    objDB.ExecuteSQL(_SQL, cn)
                    DtJson.Rows.Add(fk_id)
                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteLicense(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [license] WHERE license_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteFileLicense(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "SELECT * FROM files_all WHERE file_id = " & keyId
            Dim dtFKId As DataTable = objDB.SelectSQL(_SQL, cn)
            If dtFKId.Rows.Count > 0 Then
                Dim Path As String = fnGetPath(keyId)
                Path = "/Files/License/Root/" & dtFKId.Rows(0)("fk_id") & "/" & Path
                Dim PathServer As String = Server.MapPath("~" & Path)
                If System.IO.File.Exists(PathServer) = True Then
                    System.IO.File.Delete(PathServer)
                Else
                    Directory.Delete(PathServer, True)
                End If
                _SQL = "SELECT file_id FROM files_all where file_id = '" & keyId & "'"
                Dim dtId As DataTable = objDB.SelectSQL(_SQL, cn)
                While dtId.Rows.Count > 0
                    Dim id As String = String.Empty
                    For i As Integer = 0 To dtId.Rows.Count - 1
                        If i = dtId.Rows.Count - 1 Then
                            id &= "'" & dtId.Rows(i)("file_id") & "'"
                        Else
                            id &= "'" & dtId.Rows(i)("file_id") & "',"
                        End If
                    Next
                    _SQL = "SELECT file_id FROM files_all where parentDirId in (" & id & ")"
                    dtId = objDB.SelectSQL(_SQL, cn)
                    _SQL = "DELETE files_all WHERE file_id in (" & id & ")"
                    objDB.ExecuteSQL(_SQL, cn)
                End While
                DtJson.Rows.Add(dtFKId.Rows(0)("fk_id"))
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#Region "Tax"
        Function Tax() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View("../Home/Tax")
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function GetColumnChooserTax(ByVal table_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(cc.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id WHERE table_id = " & table_id & " ORDER BY cc.sort ASC"
            Dim DtTax As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtTax.Rows Select DtTax.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetTax() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT t.tax_id,t.tax_expire,t.tax_startdate,t.tax_rate,t.tax_status,l.number_car, l.license_car, l.license_id, t.special_expenses_1, t.special_expenses_2, t.contract_wages,N'ประวัติ' as history,t.note, t.group_update FROM tax as t join license as l on t.license_id = l.license_id order by LEN(l.number_car),l.number_car"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateTax(ByVal tax_id As String, ByVal tax_expire As String, ByVal tax_rate As String, ByVal tax_startdate As String, ByVal tax_status As String, ByVal special_expenses_1 As String, ByVal special_expenses_2 As String, ByVal contract_wages As String, ByVal IdTable As String, ByVal note As String, ByVal group_update As String) As String

            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [tax] SET "
            Dim StrTbTax() As String = {"tax_expire", "tax_rate", "tax_startdate", "tax_status", "special_expenses_1", "special_expenses_2", "contract_wages", "note", "group_update"}
            Dim TbTax() As Object = {tax_expire, tax_rate, tax_startdate, tax_status, special_expenses_1, special_expenses_2, contract_wages, note, group_update}
            For n As Integer = 0 To TbTax.Length - 1
                If Not TbTax(n) Is Nothing Then
                    _SQL &= StrTbTax(n) & "=N'" & TbTax(n) & "',"
                End If
                If StrTbTax(n) = "tax_status" Then
                    If Not TbTax(n) Is Nothing Then
                        _SQL &= "flag_status = 0, update_status = GETDATE(),"
                    End If
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE tax_id = " & tax_id
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbTax.Length - 1
                    If Not TbTax(n) Is Nothing Then
                        GbFn.KeepLog(StrTbTax(n), TbTax(n), "Editing", IdTable, tax_id)
                    End If
                Next
                'Group Update
                If Not tax_status Is Nothing Then
                    DtJson.Rows.Add(group_update_("tax", tax_status, "tax_status", tax_id, "tax_id"))
                End If
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertTax(ByVal license_id As String, ByVal tax_expire As String, ByVal tax_rate As String, ByVal tax_startdate As String, ByVal tax_status As String, ByVal special_expenses_1 As String, ByVal special_expenses_2 As String, ByVal contract_wages As String, ByVal IdTable As String, ByVal note As String, ByVal group_update As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO tax (tax_expire, tax_startdate, tax_rate, tax_status, license_id, create_by_user_id, create_date, special_expenses_1, special_expenses_2, contract_wages, note, group_update) OUTPUT Inserted.tax_id VALUES "
            _SQL &= "('" & tax_expire & "', '" & tax_startdate & "', '" & tax_rate & "', N'" & tax_status & "', '" & license_id & "', '" & Session("UserId") & "', GETDATE(), " & IIf(special_expenses_1 Is Nothing, 0, special_expenses_1) & ", " & IIf(special_expenses_2 Is Nothing, 0, special_expenses_2) & ", " & IIf(contract_wages Is Nothing, 0, contract_wages) & ",N'" & note & "',N'" & group_update & "')"
            If Not license_id Is Nothing And Not tax_expire Is Nothing And Not tax_startdate Is Nothing Then
                Try
                    DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                    If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                        Dim StrTbTax() As String = {"tax_expire", "tax_rate", "tax_startdate", "tax_status", "special_expenses_1", "special_expenses_2", "contract_wages", "group_update"}
                        Dim TbTax() As Object = {tax_expire, tax_rate, tax_startdate, tax_status, special_expenses_1, special_expenses_2, contract_wages, group_update}
                        For n As Integer = 0 To TbTax.Length - 1
                            If Not TbTax(n) Is Nothing Then
                                GbFn.KeepLog(StrTbTax(n), TbTax(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                            End If
                        Next
                    End If
                Catch ex As Exception
                    DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
                End Try
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertFileTax() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim fk_id As String = String.Empty
                Dim parentDirId As String = String.Empty
                Dim newFolder As String = String.Empty
                Dim table_id As String = String.Empty
                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "parentDirId" Then
                            parentDirId = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "newFolder" Then
                            newFolder = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "table_id" Then
                            table_id = Request.Form(i)
                        End If
                    Next
                    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                    If Request.Files.Count = 0 Then
                        Dim _Path As String = fnGetPath(parentDirId)
                        Dim pathServer As String = Server.MapPath("~/Files/Tax/Root/" & fk_id & "/" & IIf(_Path = String.Empty, String.Empty, _Path & "/") & newFolder)
                        If (Not System.IO.Directory.Exists(pathServer)) Then
                            System.IO.Directory.CreateDirectory(pathServer)
                        End If
                        Dim _SQL As String = "INSERT INTO [files_all] ([fk_id],[table_id],[name_file],[type_file],[path_file],[parentDirId],[icon],[create_by_user_id]) VALUES (" & fk_id & "," & table_id & ",N'" & newFolder & "','folder',N'','" & parentDirId & "','../Img/folder.png'," & Session("UserId") & ")"
                        objDB.ExecuteSQL(_SQL, cn)
                    Else
                        'Create File
                        For i As Integer = 0 To Request.Files.Count - 1
                            Dim file = Request.Files(i)
                            Dim _Path As String = fnGetPath(parentDirId)
                            Dim PathFile As String = "/Files/Tax/Root/" & fk_id & "/" & IIf(_Path = String.Empty, String.Empty, _Path & "/") & file.FileName
                            Dim type_file As String = Path.GetExtension(PathFile)
                            Dim pathServer As String = Server.MapPath("~" & PathFile)
                            Dim name_icon As String = String.Empty

                            Dim pathCheckId = Server.MapPath("~/Files/Tax/Root/" & fk_id)
                            If (Not System.IO.Directory.Exists(pathCheckId)) Then
                                System.IO.Directory.CreateDirectory(pathCheckId)
                            End If

                            If type_file = ".pdf" Then
                                name_icon = "pdf"
                            Else
                                name_icon = "pic"
                            End If
                            file.SaveAs(pathServer)
                            Dim _SQL As String = "INSERT INTO [files_all] ([fk_id], [table_id], [name_file], [type_file], [path_file], [parentDirId], [icon], [create_by_user_id]) VALUES (" & fk_id & "," & table_id & ", N'" & file.FileName & "','" & name_icon & "',N'.." & PathFile & "','" & parentDirId & "','../Img/" & name_icon & ".png'," & Session("UserId") & ")"
                            objDB.ExecuteSQL(_SQL, cn)
                        Next
                    End If

                    objDB.DisconnectDB(cn)
                    DtJson.Rows.Add(fk_id)
                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add(ex.Message)
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function fnRenameTax() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim dtStatus As DataTable = New DataTable
                Dim file_id As String = String.Empty
                Dim NewName As String = String.Empty
                Dim fk_id As String = String.Empty

                dtStatus.Columns.Add("Status")
                Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                cn.Open()
                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "file_id" Then
                            file_id = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "rename" Then
                            NewName = Request.Form(i)
                        End If
                    Next
                Else
                    DtJson.Rows.Add("0")
                End If
                Dim _Path As String = fnGetPath(file_id)
                Dim PathServer As String = Server.MapPath("~/Files/Tax/Root/" & fk_id & "/" & _Path)
                Try
                    If Directory.Exists(PathServer) Then
                        FileIO.FileSystem.RenameDirectory(PathServer, NewName)
                    Else
                        If System.IO.File.Exists(PathServer) Then
                            FileIO.FileSystem.RenameFile(PathServer, NewName & Path.GetExtension(_Path))
                        End If
                    End If
                Catch ex As Exception

                End Try

                Dim LastNameFile As String = Path.GetExtension(_Path)
                Dim NewNameFull As String = String.Empty

                If LastNameFile = ".pdf" Or LastNameFile = ".png" Or LastNameFile = ".jpg" Then
                    NewNameFull = NewName & LastNameFile
                Else
                    NewNameFull = NewName
                End If

                Dim ArrPath() As String = _Path.Split("/")
                Dim PathOld As String = ("../Files/Tax/Root/" & fk_id & "/" & _Path)
                Dim PathNew As String = ("../Files/Tax/Root/" & fk_id & "/")
                If ArrPath.Length > 1 Then
                    For i As Integer = 0 To ArrPath.Length - 2
                        If i = ArrPath.Length - 2 Then
                            PathNew &= ArrPath(i) & "/" & NewNameFull
                        Else
                            PathNew &= ArrPath(i) & "/"
                        End If
                    Next
                Else
                    'For child Root
                    PathNew = ("../Files/Tax/Root/" & fk_id & "/" & NewNameFull)
                End If


                Dim _SQL As String = "Update files_all Set name_file = N'" & NewNameFull & "', expanded = 1 WHERE file_id = '" & file_id & "'"
                If objDB.ExecuteSQL(_SQL, cn) Then
                    _SQL = "Update files_all SET path_file = REPLACE(path_file,N'" & PathOld & "', N'" & PathNew & "') WHERE path_file like N'%" & PathOld & "%'"
                    objDB.ExecuteSQL(_SQL, cn)
                    DtJson.Rows.Add(fk_id)
                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteTax(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE tax WHERE tax_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteFileTax(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "SELECT * FROM files_all WHERE file_id = " & keyId
            Dim dtFKId As DataTable = objDB.SelectSQL(_SQL, cn)
            If dtFKId.Rows.Count > 0 Then
                Dim Path As String = fnGetPath(keyId)
                Path = "/Files/Tax/Root/" & dtFKId.Rows(0)("fk_id") & "/" & Path
                Dim PathServer As String = Server.MapPath("~" & Path)
                If System.IO.File.Exists(PathServer) = True Then
                    System.IO.File.Delete(PathServer)
                Else
                    Directory.Delete(PathServer, True)
                End If
                _SQL = "SELECT file_id FROM files_all where file_id = '" & keyId & "'"
                Dim dtId As DataTable = objDB.SelectSQL(_SQL, cn)
                While dtId.Rows.Count > 0
                    Dim id As String = String.Empty
                    For i As Integer = 0 To dtId.Rows.Count - 1
                        If i = dtId.Rows.Count - 1 Then
                            id &= "'" & dtId.Rows(i)("file_id") & "'"
                        Else
                            id &= "'" & dtId.Rows(i)("file_id") & "',"
                        End If
                    Next
                    _SQL = "SELECT file_id FROM files_all where parentDirId in (" & id & ")"
                    dtId = objDB.SelectSQL(_SQL, cn)
                    _SQL = "DELETE files_all WHERE file_id in (" & id & ")"
                    objDB.ExecuteSQL(_SQL, cn)
                End While
                DtJson.Rows.Add(dtFKId.Rows(0)("fk_id"))
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetLastTax(ByVal license_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT tax_id FROM tax WHERE license_id = " & license_id & " ORDER BY tax_id DESC"
            Dim DtTax As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtTax.Rows Select DtTax.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#Region "Officer Records"
        Function OfficerRecords() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View("../Home/OfficerRecords")
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function GetColumnChooserOR(ByVal table_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(cc.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id WHERE table_id = " & table_id & " ORDER BY cc.sort ASC"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetOR() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT o.or_list, o.or_id,o.registrar,o.record_date,l.number_car, l.license_car, l.license_id,N'ประวัติ' as history,'View' as or_list_view,'View' as registrar_view FROM officer_records as o join license as l on o.license_id = l.license_id order by LEN(l.number_car),l.number_car"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateOR(ByVal or_id As String, ByVal registrar As String, ByVal record_date As String, ByVal or_list As String, ByVal IdTable As String) As String

            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [officer_records] SET "
            Dim StrTbTax() As String = {"registrar", "record_date", "or_list"}
            Dim TbTax() As Object = {registrar, record_date, or_list}
            For n As Integer = 0 To TbTax.Length - 1
                If Not TbTax(n) Is Nothing Then
                    _SQL &= StrTbTax(n) & "=N'" & TbTax(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE or_id = " & or_id
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbTax.Length - 1
                    If Not TbTax(n) Is Nothing Then
                        GbFn.KeepLog(StrTbTax(n), TbTax(n), "Editing", IdTable, or_id)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertOR(ByVal license_id As String, ByVal record_date As String, ByVal registrar As String, ByVal or_list As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO officer_records (license_id, registrar, record_date, or_list, create_by_user_id, create_date) OUTPUT Inserted.or_id VALUES "
            _SQL &= "('" & license_id & "', N'" & registrar & "', '" & record_date & "', N'" & or_list & "', '" & Session("UserId") & "', GETDATE())"
            If Not license_id Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                'If objDB.ExecuteSQL(_SQL, cn) Then
                '    DtJson.Rows.Add("1")
                'Else
                '    DtJson.Rows.Add("0")
                'End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbTax() As String = {"registrar", "record_date", "or_list"}
                Dim TbTax() As Object = {registrar, record_date, or_list}
                For n As Integer = 0 To TbTax.Length - 1
                    If Not TbTax(n) Is Nothing Then
                        GbFn.KeepLog(StrTbTax(n), TbTax(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertFileOR() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim fk_id As String = String.Empty
                Dim parentDirId As String = String.Empty
                Dim newFolder As String = String.Empty
                Dim table_id As String = String.Empty
                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "parentDirId" Then
                            parentDirId = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "newFolder" Then
                            newFolder = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "table_id" Then
                            table_id = Request.Form(i)
                        End If
                    Next
                    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                    If Request.Files.Count = 0 Then
                        Dim _Path As String = fnGetPath(parentDirId)
                        Dim pathServer As String = Server.MapPath("~/Files/Officer_Records/Root/" & fk_id & "/" & IIf(_Path = String.Empty, String.Empty, _Path & "/") & newFolder)
                        If (Not System.IO.Directory.Exists(pathServer)) Then
                            System.IO.Directory.CreateDirectory(pathServer)
                        End If
                        Dim _SQL As String = "INSERT INTO [files_all] ([fk_id],[table_id],[name_file],[type_file],[path_file],[parentDirId],[icon],[create_by_user_id]) VALUES (" & fk_id & "," & table_id & ",N'" & newFolder & "','folder',N'','" & parentDirId & "','../Img/folder.png'," & Session("UserId") & ")"
                        objDB.ExecuteSQL(_SQL, cn)
                    Else
                        'Create File
                        For i As Integer = 0 To Request.Files.Count - 1
                            Dim file = Request.Files(i)
                            Dim _Path As String = fnGetPath(parentDirId)
                            Dim PathFile As String = "/Files/Officer_Records/Root/" & fk_id & "/" & IIf(_Path = String.Empty, String.Empty, _Path & "/") & file.FileName
                            Dim type_file As String = Path.GetExtension(PathFile)
                            Dim pathServer As String = Server.MapPath("~" & PathFile)
                            Dim name_icon As String = String.Empty

                            Dim pathCheckId = Server.MapPath("~/Files/Officer_Records/Root/" & fk_id)
                            If (Not System.IO.Directory.Exists(pathCheckId)) Then
                                System.IO.Directory.CreateDirectory(pathCheckId)
                            End If

                            If type_file = ".pdf" Then
                                name_icon = "pdf"
                            Else
                                name_icon = "pic"
                            End If
                            file.SaveAs(pathServer)
                            Dim _SQL As String = "INSERT INTO [files_all] ([fk_id], [table_id], [name_file], [type_file], [path_file], [parentDirId], [icon], [create_by_user_id]) VALUES (" & fk_id & "," & table_id & ", N'" & file.FileName & "','" & name_icon & "',N'.." & PathFile & "','" & parentDirId & "','../Img/" & name_icon & ".png'," & Session("UserId") & ")"
                            objDB.ExecuteSQL(_SQL, cn)
                        Next
                    End If

                    objDB.DisconnectDB(cn)
                    DtJson.Rows.Add(fk_id)
                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function fnRenameOR() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim dtStatus As DataTable = New DataTable
                Dim file_id As String = String.Empty
                Dim NewName As String = String.Empty
                Dim fk_id As String = String.Empty

                dtStatus.Columns.Add("Status")
                Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                cn.Open()
                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "file_id" Then
                            file_id = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "rename" Then
                            NewName = Request.Form(i)
                        End If
                    Next
                Else
                    DtJson.Rows.Add("0")
                End If
                Dim _Path As String = fnGetPath(file_id)
                Dim PathServer As String = Server.MapPath("~/Files/Officer_Records/Root/" & fk_id & "/" & _Path)
                Try
                    If Directory.Exists(PathServer) Then
                        FileIO.FileSystem.RenameDirectory(PathServer, NewName)
                    Else
                        If System.IO.File.Exists(PathServer) Then
                            FileIO.FileSystem.RenameFile(PathServer, NewName & Path.GetExtension(_Path))
                        End If
                    End If
                Catch ex As Exception

                End Try

                Dim LastNameFile As String = Path.GetExtension(_Path)
                Dim NewNameFull As String = String.Empty

                If LastNameFile = ".pdf" Or LastNameFile = ".png" Or LastNameFile = ".jpg" Then
                    NewNameFull = NewName & LastNameFile
                Else
                    NewNameFull = NewName
                End If

                Dim ArrPath() As String = _Path.Split("/")
                Dim PathOld As String = ("../Files/Officer_Records/Root/" & fk_id & "/" & _Path)
                Dim PathNew As String = ("../Files/Officer_Records/Root/" & fk_id & "/")
                If ArrPath.Length > 1 Then
                    For i As Integer = 0 To ArrPath.Length - 2
                        If i = ArrPath.Length - 2 Then
                            PathNew &= ArrPath(i) & "/" & NewNameFull
                        Else
                            PathNew &= ArrPath(i) & "/"
                        End If
                    Next
                Else
                    'For child Root
                    PathNew = ("../Files/Officer_Records/Root/" & fk_id & "/" & NewNameFull)
                End If


                Dim _SQL As String = "Update files_all Set name_file = N'" & NewNameFull & "', expanded = 1 WHERE file_id = '" & file_id & "'"
                If objDB.ExecuteSQL(_SQL, cn) Then
                    _SQL = "Update files_all SET path_file = REPLACE(path_file,N'" & PathOld & "', N'" & PathNew & "') WHERE path_file like N'%" & PathOld & "%'"
                    objDB.ExecuteSQL(_SQL, cn)
                    DtJson.Rows.Add(fk_id)
                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteOR(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE officer_records WHERE or_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteFileOR(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "SELECT * FROM files_all WHERE file_id = " & keyId
            Dim dtFKId As DataTable = objDB.SelectSQL(_SQL, cn)
            If dtFKId.Rows.Count > 0 Then
                Dim Path As String = fnGetPath(keyId)
                Path = "/Files/Officer_Records/Root/" & dtFKId.Rows(0)("fk_id") & "/" & Path
                Dim PathServer As String = Server.MapPath("~" & Path)
                If System.IO.File.Exists(PathServer) = True Then
                    System.IO.File.Delete(PathServer)
                Else
                    Directory.Delete(PathServer, True)
                End If
                _SQL = "SELECT file_id FROM files_all where file_id = '" & keyId & "'"
                Dim dtId As DataTable = objDB.SelectSQL(_SQL, cn)
                While dtId.Rows.Count > 0
                    Dim id As String = String.Empty
                    For i As Integer = 0 To dtId.Rows.Count - 1
                        If i = dtId.Rows.Count - 1 Then
                            id &= "'" & dtId.Rows(i)("file_id") & "'"
                        Else
                            id &= "'" & dtId.Rows(i)("file_id") & "',"
                        End If
                    Next
                    _SQL = "SELECT file_id FROM files_all where parentDirId in (" & id & ")"
                    dtId = objDB.SelectSQL(_SQL, cn)
                    _SQL = "DELETE files_all WHERE file_id in (" & id & ")"
                    objDB.ExecuteSQL(_SQL, cn)
                End While
                DtJson.Rows.Add(dtFKId.Rows(0)("fk_id"))
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetLastOR(ByVal license_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT or_id FROM officer_records WHERE license_id = " & license_id & " ORDER BY or_id DESC"
            Dim DtOR As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtOR.Rows Select DtOR.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateOrList(ByVal or_id As String, ByVal data As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "UPDATE [dbo].[officer_records] SET [or_list] = N'" & data & "' WHERE or_id = '" & or_id & "'"
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                GbFn.KeepLog("or_list", data, "Editing", IdTable, or_id)
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))

        End Function

        Public Function UpdateRegistrar(ByVal or_id As String, ByVal data As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "UPDATE [dbo].[officer_records] SET [registrar] = N'" & data & "' WHERE or_id = '" & or_id & "'"
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                GbFn.KeepLog("registrar", data, "Editing", IdTable, or_id)
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))

        End Function

#End Region

#Region "Lookup"
        Public Function GetLookUp(ByVal column_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT lookup_id, data_list FROM [lookup] WHERE column_id = " & column_id
            Dim DtFiles As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtFiles.Rows Select DtFiles.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#Region "Driver"
        Function Driver() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function GetColumnChooserDrive(ByVal table_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(cc.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id WHERE table_id = " & table_id & " ORDER BY cc.sort ASC"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteDriver(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [driver] WHERE driver_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetDriver() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT d.driver_id, d.driver_name, d.start_work_date, li.license_id as license_id_head, d.license_id_tail, (select license_car from license where license_id = li.license_id) as license_car_head, (select license_car from license where license_id = d.license_id_tail) as license_car_tail,N'ประวัติ' as history ,li.number_car FROM driver as d right join license li on d.license_id_head = li.license_id where li.number_car Not like 'T%' order by LEN(li.number_car),li.number_car"
            Dim DtDriver As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtDriver.Rows Select DtDriver.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertDriver(ByVal driver_name As String, ByVal start_work_date As String, ByVal license_id_head As String, ByVal license_id_tail As String, ByVal IdTable As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO driver (driver_name, start_work_date, license_id_head, license_id_tail, create_by_user_id) OUTPUT Inserted.driver_id VALUES (N'" & driver_name & "', '" & start_work_date & "', '" & license_id_head & "', '" & license_id_tail & "', '" & Session("UserId") & "')"
            If license_id_head <> Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            'Dim StrTbDriver() As String = {"driver_name", "start_work_date", "license_id_head", "license_id_tail"}
            'Dim TbDriver() As Object = {driver_name, start_work_date, license_id_head, license_id_tail}
            'For n As Integer = 0 To TbDriver.Length - 1
            '    If Not TbDriver(n) Is Nothing Then
            '        _SQL &= StrTbDriver(n) & "=N'" & TbDriver(n) & "',"
            '        GbFn.KeepLog(StrTbDriver(n), TbDriver(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
            '    End If
            'Next
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateDriver(ByVal driver_id As String, ByVal driver_name As String, ByVal start_work_date As String, ByVal license_id_head As String, ByVal license_id_tail As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [driver] SET "
            Dim StrTbDriver() As String = {"driver_name", "start_work_date", "license_id_head", "license_id_tail"}
            Dim TbDriver() As Object = {driver_name, start_work_date, license_id_head, license_id_tail}
            For n As Integer = 0 To TbDriver.Length - 1
                If Not TbDriver(n) Is Nothing Then
                    _SQL &= StrTbDriver(n) & "=N'" & TbDriver(n) & "',"
                    'GbFn.KeepLog(StrTbDriver(n), TbDriver(n), "Editing", IdTable, driver_id)
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE driver_id = " & driver_id
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        'GetNumberCar

#End Region

#Region "LicenseMekongRiver"
        Function LicenseMekongRiver() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function GetColumnChooserLmr(ByVal table_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(cc.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id WHERE table_id = " & table_id & " ORDER BY cc.sort ASC"
            Dim DtTax As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtTax.Rows Select DtTax.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetLmr() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT lmr_path, lmr_id, lmr_number, lmr_expire, lmr_start, country_code, benefit, lmr_status, note, N'ประวัติ' as history, group_update FROM license_mekong_river"
            Dim DtLmr As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLmr.Rows Select DtLmr.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetLmrPermission() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "  SELECT lmrp.lmrp_id, lmrp.lmr_id, lmrp.license_id_head, lmrp.license_id_tail, (select license_car from license where license_id = lmrp.license_id_head) as license_car_head, (select license_car from license where license_id = lmrp.license_id_tail) as license_car_tail, (select style_car from license where license_id = lmrp.license_id_tail) as style_car, (select shaft from license where license_id = lmrp.license_id_tail) as shaft FROM license_mekong_river_permission as lmrp"
            Dim DtDriver As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtDriver.Rows Select DtDriver.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateLmr(ByVal lmr_id As String, ByVal lmr_number As String, ByVal lmr_expire As String, ByVal lmr_start As String, ByVal lmr_path As String, ByVal country_code As String, ByVal benefit As String, ByVal lmr_status As String, ByVal note As String, ByVal IdTable As String, ByVal group_update As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [license_mekong_river] SET "
            Dim StrTbLc() As String = {"lmr_number", "lmr_expire", "lmr_start", "lmr_path", "country_code", "benefit", "lmr_status", "note", "group_update"}
            Dim TbLc() As Object = {lmr_number, lmr_expire, lmr_start, lmr_path, country_code, benefit, lmr_status, note, group_update}
            For n As Integer = 0 To TbLc.Length - 1
                If Not TbLc(n) Is Nothing Then
                    _SQL &= StrTbLc(n) & "=N'" & TbLc(n) & "',"
                End If
                If StrTbLc(n) = "lmr_status" Then
                    If Not TbLc(n) Is Nothing Then
                        _SQL &= "flag_status = 0, update_status = GETDATE(),"
                    End If
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE lmr_id = " & lmr_id
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbLc.Length - 1
                    If Not TbLc(n) Is Nothing Then
                        GbFn.KeepLog(StrTbLc(n), TbLc(n), "Editing", IdTable, lmr_id)
                    End If
                Next
                'Group Update
                If Not lmr_status Is Nothing Then
                    DtJson.Rows.Add(group_update_("license_mekong_river", lmr_status, "lmr_status", lmr_id, "lmr_id"))
                End If
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertLmr(ByVal lmr_number As String, ByVal lmr_expire As DateTime, ByVal lmr_start As DateTime, ByVal country_code As String, ByVal benefit As String, ByVal lmr_status As String, ByVal note As String, ByVal IdTable As String, ByVal group_update As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO license_mekong_river (lmr_number, lmr_expire, lmr_start, country_code, benefit, lmr_status, note, create_by_user_id, group_update) OUTPUT Inserted.lmr_id VALUES "
            _SQL &= "(N'" & lmr_number & "', '" & lmr_expire & "', '" & lmr_start & "', N'" & country_code & "', N'" & benefit & "', N'" & lmr_status & "', N'" & note & "', '" & Session("UserId") & "',N'" & group_update & "')"
            If Not lmr_number Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                    Dim StrTbLc() As String = {"lmr_number", "lmr_expire", "lmr_start", "country_code", "benefit", "lmr_status", "note"}
                    Dim TbLc() As Object = {lmr_number, lmr_expire, lmr_start, country_code, benefit, lmr_status, note}
                    For n As Integer = 0 To TbLc.Length - 1
                        If Not TbLc(n) Is Nothing Then
                            GbFn.KeepLog(StrTbLc(n), TbLc(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                        End If
                    Next
                End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


        Public Function InsertLmrPermission(ByVal lmr_id As Integer, ByVal license_id_head As Integer, ByVal license_id_tail As Integer) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO license_mekong_river_permission (license_id_head, license_id_tail, lmr_id, create_by_user_id) OUTPUT Inserted.lmrp_id VALUES (" & license_id_head & ", " & license_id_tail & ", " & lmr_id & ", '" & Session("UserId") & "')"
            If license_id_head <> Nothing And license_id_tail <> Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertFileLmr() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim fk_id As String = String.Empty
                Dim newFile As String = String.Empty

                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        End If
                    Next
                    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                    If Request.Files.Count <> 0 Then

                        Dim pathServer As String = Server.MapPath("~/Files/LMR/" & fk_id)
                        If (Not System.IO.Directory.Exists(pathServer)) Then
                            System.IO.Directory.CreateDirectory(pathServer)
                        End If
                        Dim fileName As String = String.Empty
                        For i As Integer = 0 To Request.Files.Count - 1
                            Dim file = Request.Files(i)
                            fileName = file.FileName
                            file.SaveAs(pathServer & "/" & fileName)
                            Dim _SQL As String = "UPDATE license_mekong_river SET lmr_path = N'../Files/LMR/" & fk_id & "/" & file.FileName & "' WHERE lmr_id = " & fk_id
                            objDB.ExecuteSQL(_SQL, cn)
                        Next

                        DtJson.Rows.Add("../Files/LMR/" & fk_id & "/" & fileName)
                    Else
                        DtJson.Rows.Add("0")
                    End If

                    objDB.DisconnectDB(cn)

                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteFileLmr(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "SELECT * FROM license_mekong_river WHERE lmr_id = " & keyId
            Dim dtLc As DataTable = objDB.SelectSQL(_SQL, cn)
            If dtLc.Rows.Count > 0 Then
                Dim pathServer As String = Server.MapPath(dtLc.Rows(0)("lmr_path").Replace("..", "~"))
                If System.IO.File.Exists(pathServer) = True Then
                    System.IO.File.Delete(pathServer)
                End If
                'DtJson.Rows.Add("1")
                _SQL = "DELETE log_monitor WHERE send_status is null and table_id = 23 and fk_id = " & keyId
                If objDB.ExecuteSQL(_SQL, cn) Then
                    DtJson.Rows.Add("1")
                Else
                    DtJson.Rows.Add("0")
                End If
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteLmrPermission(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "DELETE license_mekong_river_permission WHERE lmrp_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteLmr(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "DELETE license_mekong_river WHERE lmr_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#Region "LicenseCambodia"
        Function LicenseCambodia() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function GetColumnChooserLc(ByVal table_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(cc.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id WHERE table_id = " & table_id & " ORDER BY cc.sort ASC"
            Dim DtTax As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtTax.Rows Select DtTax.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetLc() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT lc_path, lc_id, lc_number, lc_expire, lc_start, country_code, benefit, lc_status, note ,N'ประวัติ' as history,group_update FROM license_cambodia"
            Dim DtLc As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLc.Rows Select DtLc.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetLcPermission() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "  SELECT lcp.lcp_id, lcp.lc_id, lcp.license_id_head, lcp.license_id_tail, (select license_car from license where license_id = lcp.license_id_head) as license_car_head, (select license_car from license where license_id = lcp.license_id_tail) as license_car_tail, (select style_car from license where license_id = lcp.license_id_tail) as style_car,(select shaft from license where license_id = lcp.license_id_tail) as shaft  FROM license_cambodia_permission as lcp"
            Dim DtDriver As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtDriver.Rows Select DtDriver.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateLc(ByVal lc_id As String, ByVal lc_number As String, ByVal lc_expire As String, ByVal lc_start As String, ByVal lc_path As String, ByVal country_code As String, ByVal benefit As String, ByVal lc_status As String, ByVal note As String, ByVal IdTable As String, ByVal group_update As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [license_cambodia] SET "
            Dim StrTbLc() As String = {"lc_number", "lc_expire", "lc_start", "lc_path", "country_code", "benefit", "lc_status", "note", "group_update"}
            Dim TbLc() As Object = {lc_number, lc_expire, lc_start, lc_path, country_code, benefit, lc_status, note, group_update}
            For n As Integer = 0 To TbLc.Length - 1
                If Not TbLc(n) Is Nothing Then
                    _SQL &= StrTbLc(n) & "=N'" & TbLc(n) & "',"
                End If
                If StrTbLc(n) = "lc_status" Then
                    If Not TbLc(n) Is Nothing Then
                        _SQL &= "flag_status = 0, update_status = GETDATE(),"
                    End If
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE lc_id = " & lc_id
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbLc.Length - 1
                    If Not TbLc(n) Is Nothing Then
                        GbFn.KeepLog(StrTbLc(n), TbLc(n), "Editing", IdTable, lc_id)
                    End If
                Next
                'Group Update
                If Not lc_status Is Nothing Then
                    DtJson.Rows.Add(group_update_("license_cambodia", lc_status, "lc_status", lc_id, "lc_id"))
                End If
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertLc(ByVal lc_number As String, ByVal lc_expire As String, ByVal lc_start As String, ByVal country_code As String, ByVal benefit As String, ByVal lc_status As String, ByVal note As String, ByVal IdTable As String, ByVal group_update As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO license_cambodia (lc_number, lc_expire, lc_start, country_code, benefit, lc_status, note, create_by_user_id,group_update) OUTPUT Inserted.lc_id VALUES "
            _SQL &= "(N'" & lc_number & "', '" & lc_expire & "', '" & lc_start & "', N'" & country_code & "', N'" & benefit & "', N'" & lc_status & "',N'" & note & "', '" & Session("UserId") & "',N'" & group_update & "')"
            If Not lc_number Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                    Dim StrTbLc() As String = {"lc_number", "lc_expire", "lc_start", "country_code", "benefit", "lc_status", "note"}
                    Dim TbLc() As Object = {lc_number, lc_expire, lc_start, country_code, benefit, lc_status, note}
                    For n As Integer = 0 To TbLc.Length - 1
                        If Not TbLc(n) Is Nothing Then
                            GbFn.KeepLog(StrTbLc(n), TbLc(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                        End If
                    Next
                End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


        Public Function InsertLcPermission(ByVal lc_id As Integer, ByVal license_id_head As Integer, ByVal license_id_tail As Integer) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO license_cambodia_permission (license_id_head, license_id_tail, lc_id, create_by_user_id) OUTPUT Inserted.lcp_id VALUES (" & license_id_head & ", " & license_id_tail & ", " & lc_id & ", '" & Session("UserId") & "')"
            If license_id_head <> Nothing And license_id_tail <> Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertFileLc() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim fk_id As String = String.Empty
                Dim newFile As String = String.Empty

                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        End If
                    Next
                    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                    If Request.Files.Count <> 0 Then

                        Dim pathServer As String = Server.MapPath("~/Files/LC/" & fk_id)
                        If (Not System.IO.Directory.Exists(pathServer)) Then
                            System.IO.Directory.CreateDirectory(pathServer)
                        End If
                        Dim fileName As String = String.Empty
                        For i As Integer = 0 To Request.Files.Count - 1
                            Dim file = Request.Files(i)
                            fileName = file.FileName
                            file.SaveAs(pathServer & "/" & fileName)
                            Dim _SQL As String = "UPDATE license_cambodia SET lc_path = N'../Files/LC/" & fk_id & "/" & file.FileName & "' WHERE lc_id = " & fk_id
                            objDB.ExecuteSQL(_SQL, cn)
                        Next

                        DtJson.Rows.Add("../Files/LC/" & fk_id & "/" & fileName)
                    Else
                        DtJson.Rows.Add("0")
                    End If

                    objDB.DisconnectDB(cn)

                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteFileLc(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "SELECT * FROM license_cambodia WHERE lc_id = " & keyId
            Dim dtLc As DataTable = objDB.SelectSQL(_SQL, cn)
            If dtLc.Rows.Count > 0 Then
                Dim pathServer As String = Server.MapPath(dtLc.Rows(0)("lc_path").Replace("..", "~"))
                If System.IO.File.Exists(pathServer) = True Then
                    System.IO.File.Delete(pathServer)
                End If
                'DtJson.Rows.Add("1")
                _SQL = "DELETE log_monitor WHERE send_status is null and table_id = 22 and fk_id = " & keyId
                If objDB.ExecuteSQL(_SQL, cn) Then
                    DtJson.Rows.Add("1")
                Else
                    DtJson.Rows.Add("0")
                End If
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteLcPermission(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "DELETE license_cambodia_permission WHERE lcp_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteLc(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "DELETE license_cambodia WHERE lc_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#Region "BusinessIn"
        Function BusinessIn() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function GetColumnChooserBusinessIn(ByVal table_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(cc.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id WHERE table_id = " & table_id & " ORDER BY cc.sort ASC"
            Dim DtTax As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtTax.Rows Select DtTax.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetBusinessInType() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT * FROM business_in_type"
            Dim DtBitType As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtBitType.Rows Select DtBitType.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetBusinessIn() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT business_path, business_id, business_number, business_expire, business_start, business_name, business_address, business_type, country_code, benefit, business_status,note, N'ประวัติ' as history FROM business_in"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetNumberCarBusiness() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT l.number_car, l.license_id, l.license_car, l.brand_car, l.number_body, l.number_engine, l.style_car, t.tax_expire, [bit].bit_name FROM license as l left join tax as t on l.license_id = t.license_id left join business_in_permission as bip on l.license_id = bip.license_id left join business_in_type as [bit] on bip.bit_id = [bit].bit_id order by LEN(l.number_car),l.number_car"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetBusinessInPermission(ByVal BusinessId As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT bip.bip_id, bip.business_id, [bit].bit_id as bit_name, l.license_id, l.number_car, l.license_car, l.brand_car, l.number_body, l.number_engine, t.tax_expire, l.style_car FROM business_in_permission as bip join business_in_type as [bit] on bip.bit_id = [bit].bit_id join license as l on bip.license_id = l.license_id join tax as t on l.license_id = t.license_id where bip.business_id = " & BusinessId & " order by LEN(l.number_car),l.number_car"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateBusinessIn(ByVal business_id As String, ByVal business_number As String, ByVal business_expire As String, ByVal business_start As String, ByVal business_name As String, ByVal business_address As String, ByVal business_type As String, ByVal business_path As String, ByVal business_status As String, ByVal benefit As String, ByVal IdTable As String, ByVal note As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [business_in] SET "
            Dim StrTbBusiness() As String = {"business_number", "business_expire", "business_start", "business_name", "business_address", "business_type", "business_path", "business_status", "benefit", "note"}
            Dim TbBusiness() As Object = {business_number, business_expire, business_start, business_name, business_address, business_type, business_path, business_status, benefit, note}
            For n As Integer = 0 To TbBusiness.Length - 1
                If Not TbBusiness(n) Is Nothing Then
                    _SQL &= StrTbBusiness(n) & "=N'" & TbBusiness(n) & "',"
                End If
                If StrTbBusiness(n) = "business_status" Then
                    If Not TbBusiness(n) Is Nothing Then
                        _SQL &= "flag_status = 0, update_status = GETDATE(),"
                    End If
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE business_id = " & business_id
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbBusiness.Length - 1
                    If Not TbBusiness(n) Is Nothing Then
                        GbFn.KeepLog(StrTbBusiness(n), TbBusiness(n), "Editing", IdTable, business_id)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        'Public Function UpdateBusinessInPermission(ByVal bip_id As String, ByVal bit_id As String, ByVal business_id As String, ByVal license_id As String) As String
        '    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        '    Dim DtJson As DataTable = New DataTable
        '    DtJson.Columns.Add("Status")
        '    Dim _SQL As String = "UPDATE [business_in] SET "
        '    Dim StrTbBusiness() As String = {"bit_id", "business_id", "license_id"}
        '    Dim TbBusiness() As Object = {bit_id, business_id, license_id}
        '    For n As Integer = 0 To TbBusiness.Length - 1
        '        If Not TbBusiness(n) Is Nothing Then
        '            _SQL &= StrTbBusiness(n) & "=N'" & TbBusiness(n) & "',"
        '        End If
        '    Next
        '    _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE bip_id = " & bip_id
        '    If objDB.ExecuteSQL(_SQL, cn) Then
        '        DtJson.Rows.Add("1")
        '    Else
        '        DtJson.Rows.Add("0")
        '    End If
        '    objDB.DisconnectDB(cn)
        '    Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        'End Function

        Public Function InsertBusinessIn(ByVal business_number As String, ByVal business_expire As String, ByVal business_start As String, ByVal business_name As String, ByVal business_address As String, ByVal business_type As String, ByVal country_code As String, ByVal benefit As String, ByVal business_status As String, ByVal IdTable As String, ByVal note As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO business_in (business_number, business_expire, business_start, business_name, business_address, business_type, country_code, benefit, business_status,note, create_by_user_id) OUTPUT Inserted.business_id VALUES "
            _SQL &= "(N'" & business_number & "', '" & business_expire & "', '" & business_start & "', N'" & business_name & "', N'" & business_address & "', N'" & business_type & "', N'" & country_code & "', N'" & benefit & "', N'" & business_status & "',N'" & note & "', '" & Session("UserId") & "')"
            If Not business_number Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbBusiness() As String = {"business_number", "business_expire", "business_start", "business_name", "business_address", "business_type", "note"}
                Dim TbBusiness() As Object = {business_number, business_expire, business_start, business_name, business_address, business_type, note}
                For n As Integer = 0 To TbBusiness.Length - 1
                    If Not TbBusiness(n) Is Nothing Then
                        GbFn.KeepLog(StrTbBusiness(n), TbBusiness(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertBusinessInPermission(ByVal bit_name As String, ByVal business_id As String, ByVal number_car As String, ByVal IdTable As String, ByVal BiId As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)

            Dim _SQL As String = "SELECT license_id FROM license WHERE number_car = '" & number_car & "'"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            If DtLicense.Rows.Count > 0 Then
                _SQL = "INSERT INTO business_in_permission (bit_id, business_id, license_id, create_by_user_id) OUTPUT Inserted.bip_id VALUES "
                _SQL &= "('" & bit_name & "', '" & business_id & "', '" & DtLicense.Rows(0)("license_id") & "', '" & Session("UserId") & "')"
                If Not bit_name Is Nothing And Not business_id Is Nothing And Not number_car Is Nothing Then
                    DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                Else
                    DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
                End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbBusiness() As String = {"number_car"}
                Dim TbBusiness() As Object = {number_car}
                For n As Integer = 0 To TbBusiness.Length - 1
                    If Not TbBusiness(n) Is Nothing Then
                        GbFn.KeepLog(StrTbBusiness(n), TbBusiness(n), "Add", IdTable, BiId)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertFileBusinessIn() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim fk_id As String = String.Empty
                Dim newFile As String = String.Empty

                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        End If
                    Next
                    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                    If Request.Files.Count <> 0 Then

                        Dim pathServer As String = Server.MapPath("~/Files/BI/" & fk_id)
                        If (Not System.IO.Directory.Exists(pathServer)) Then
                            System.IO.Directory.CreateDirectory(pathServer)
                        End If
                        Dim fileName As String = String.Empty
                        For i As Integer = 0 To Request.Files.Count - 1
                            Dim file = Request.Files(i)
                            fileName = file.FileName
                            file.SaveAs(pathServer & "/" & fileName)
                            Dim _SQL As String = "UPDATE business_in SET business_path = N'../Files/BI/" & fk_id & "/" & file.FileName & "' WHERE business_id = " & fk_id
                            objDB.ExecuteSQL(_SQL, cn)
                        Next

                        DtJson.Rows.Add("../Files/BI/" & fk_id & "/" & fileName)
                    Else
                        DtJson.Rows.Add("0")
                    End If

                    objDB.DisconnectDB(cn)

                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteFileBusinessIn(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "SELECT * FROM business_in WHERE business_id = " & keyId
            Dim dtbi As DataTable = objDB.SelectSQL(_SQL, cn)
            If dtbi.Rows.Count > 0 Then
                Dim pathServer As String = Server.MapPath(dtbi.Rows(0)("business_path").Replace("..", "~"))
                If System.IO.File.Exists(pathServer) = True Then
                    System.IO.File.Delete(pathServer)
                End If
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteBusinessInPermission(ByVal keyId As String, ByVal BiId As String, ByVal IdTable As String, ByVal NumberCar As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "DELETE business_in_permission WHERE bip_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                GbFn.KeepLog("number_car", NumberCar, "Delete", IdTable, BiId)
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteBusinessIn(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "DELETE business_in WHERE business_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


#End Region

#Region "BusinessOut"
        Function BusinessOut() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function GetColumnChooserBusinessOut(ByVal table_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(cc.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id WHERE table_id = " & table_id & " ORDER BY cc.sort ASC"
            Dim DtTax As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtTax.Rows Select DtTax.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetBusinessOutType() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT * FROM business_out_type"
            Dim DtBitType As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtBitType.Rows Select DtBitType.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetBusinessOut() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT business_path, business_id, business_number, business_expire, business_start, business_name, business_address, business_type, country_code, benefit, business_status, N'ประวัติ' as history, note FROM business_out"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetNumberCarBusinessOut() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT l.number_car, l.license_id, l.license_car, l.brand_car, l.number_body, l.number_engine, l.style_car, t.tax_expire, [bot].bot_name FROM license as l left join tax as t on l.license_id = t.license_id left join business_out_permission as bop on l.license_id = bop.license_id left join business_out_type as [bot] on bop.bot_id = [bot].bot_id order by LEN(l.number_car),l.number_car"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetBusinessOutPermission(ByVal BusinessId As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT bop.bop_id, bop.business_id, [bot].bot_id as bot_name, l.license_id, l.number_car, l.license_car, l.brand_car, l.number_body, l.number_engine, t.tax_expire, l.style_car, l.license_id FROM business_out_permission as bop join business_out_type as [bot] on bop.bot_id = [bot].bot_id join license as l on bop.license_id = l.license_id join tax as t on l.license_id = t.license_id where bop.business_id = " & BusinessId & " order by LEN(l.number_car),l.number_car"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateBusinessOut(ByVal business_id As String, ByVal business_number As String, ByVal business_expire As String, ByVal business_start As String, ByVal business_name As String, ByVal business_address As String, ByVal business_type As String, ByVal business_path As String, ByVal business_status As String, ByVal benefit As String, ByVal IdTable As String, ByVal note As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [business_out] SET "
            Dim StrTbBusiness() As String = {"business_number", "business_expire", "business_start", "business_name", "business_address", "business_type", "business_path", "business_status", "benefit", "note"}
            Dim TbBusiness() As Object = {business_number, business_expire, business_start, business_name, business_address, business_type, business_path, business_status, benefit, note}
            For n As Integer = 0 To TbBusiness.Length - 1
                If Not TbBusiness(n) Is Nothing Then
                    _SQL &= StrTbBusiness(n) & "=N'" & TbBusiness(n) & "',"
                End If
                If StrTbBusiness(n) = "business_status" Then
                    If Not TbBusiness(n) Is Nothing Then
                        _SQL &= "flag_status = 0, update_status = GETDATE(),"
                    End If
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE business_id = " & business_id
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbBusiness.Length - 1
                    If Not TbBusiness(n) Is Nothing Then
                        GbFn.KeepLog(StrTbBusiness(n), TbBusiness(n), "Editing", IdTable, business_id)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


        Public Function InsertBusinessOut(ByVal business_number As String, ByVal business_expire As String, ByVal business_start As String, ByVal business_name As String, ByVal business_address As String, ByVal business_type As String, ByVal country_code As String, ByVal benefit As String, ByVal business_status As String, ByVal IdTable As String, ByVal note As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO business_out (business_number, business_expire, business_start, business_name, business_address, business_type, country_code, benefit, business_status, note, create_by_user_id) OUTPUT Inserted.business_id VALUES "
            _SQL &= "(N'" & business_number & "', '" & business_expire & "', '" & business_start & "', N'" & business_name & "', N'" & business_address & "', N'" & business_type & "', N'" & country_code & "', N'" & benefit & "',N'" & note & "', N'" & business_status & "', '" & Session("UserId") & "')"
            If Not business_number Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbBusiness() As String = {"business_number", "business_expire", "business_start", "business_name", "business_address", "business_type", "note"}
                Dim TbBusiness() As Object = {business_number, business_expire, business_start, business_name, business_address, business_type, note}
                For n As Integer = 0 To TbBusiness.Length - 1
                    If Not TbBusiness(n) Is Nothing Then
                        GbFn.KeepLog(StrTbBusiness(n), TbBusiness(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertBusinessOutPermission(ByVal bot_name As String, ByVal business_id As String, ByVal number_car As String, ByVal IdTable As String, ByVal BiId As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)

            Dim _SQL As String = "SELECT license_id FROM license WHERE number_car = '" & number_car & "'"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            If DtLicense.Rows.Count > 0 Then
                _SQL = "INSERT INTO business_out_permission (bot_id, business_id, license_id, create_by_user_id) OUTPUT Inserted.bop_id VALUES "
                _SQL &= "('" & bot_name & "', '" & business_id & "', '" & DtLicense.Rows(0)("license_id") & "', '" & Session("UserId") & "')"
                If Not bot_name Is Nothing And Not business_id Is Nothing And Not number_car Is Nothing Then
                    DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                Else
                    DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
                End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbBusiness() As String = {"number_car"}
                Dim TbBusiness() As Object = {number_car}
                For n As Integer = 0 To TbBusiness.Length - 1
                    If Not TbBusiness(n) Is Nothing Then
                        GbFn.KeepLog(StrTbBusiness(n), TbBusiness(n), "Add", IdTable, BiId)
                    End If
                Next
            End If


            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertFileBusinessOut() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim fk_id As String = String.Empty
                Dim newFile As String = String.Empty

                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        End If
                    Next
                    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                    If Request.Files.Count <> 0 Then

                        Dim pathServer As String = Server.MapPath("~/Files/BO/" & fk_id)
                        If (Not System.IO.Directory.Exists(pathServer)) Then
                            System.IO.Directory.CreateDirectory(pathServer)
                        End If
                        Dim fileName As String = String.Empty
                        For i As Integer = 0 To Request.Files.Count - 1
                            Dim file = Request.Files(i)
                            fileName = file.FileName
                            file.SaveAs(pathServer & "/" & fileName)
                            Dim _SQL As String = "UPDATE business_out SET business_path = N'../Files/BO/" & fk_id & "/" & file.FileName & "' WHERE business_id = " & fk_id
                            objDB.ExecuteSQL(_SQL, cn)
                        Next

                        DtJson.Rows.Add("../Files/BO/" & fk_id & "/" & fileName)
                    Else
                        DtJson.Rows.Add("0")
                    End If

                    objDB.DisconnectDB(cn)

                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteFileBusinessOut(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "SELECT * FROM business_out WHERE business_id = " & keyId
            Dim dtbi As DataTable = objDB.SelectSQL(_SQL, cn)
            If dtbi.Rows.Count > 0 Then
                Dim pathServer As String = Server.MapPath(dtbi.Rows(0)("business_path").Replace("..", "~"))
                If System.IO.File.Exists(pathServer) = True Then
                    System.IO.File.Delete(pathServer)
                End If
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteBusinessOutPermission(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "DELETE business_out_permission WHERE bop_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteBusinessOut(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "DELETE business_out WHERE business_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


#End Region

#Region "LicenseFactory"
        Function LicenseFactory() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function GetDriverName() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "select driver_id, driver_name, l.number_car, l.license_car from driver as d join license as l on d.license_id_head = l.license_id"
            Dim Dt As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In Dt.Rows Select Dt.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetColumnChooserLicenseFactory(ByVal table_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(cc.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id WHERE table_id = " & table_id & " ORDER BY cc.sort ASC"
            Dim Dt As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In Dt.Rows Select Dt.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetLicenseFactory() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "select N'ประวัติ' as history, d.driver_id as driver_name, l.number_car, l.license_car, l.license_id, lf.IdNo, lf.[start_date], lf.[expire_date], lf.name_factory, lf.license_factory_id, lf.license_factory_status, lf.path from license_factory as lf join driver as d on lf.driver_id = d.driver_id join license as l on d.license_id_head = l.license_id"
            Dim Dt As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In Dt.Rows Select Dt.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateLicenseFactory(ByVal license_factory_id As String, ByVal driver_name As String, ByVal IdNo As String, ByVal start_date As String, ByVal expire_date As String, ByVal name_factory As String, ByVal license_factory_status As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [license_factory] SET "
            Dim StrTbLc() As String = {"driver_id", "IdNo", "start_date", "expire_date", "name_factory", "license_factory_status"}
            Dim TbLc() As Object = {driver_name, IdNo, start_date, expire_date, name_factory, license_factory_status}
            For n As Integer = 0 To TbLc.Length - 1
                If Not TbLc(n) Is Nothing Then
                    _SQL &= StrTbLc(n) & "=N'" & TbLc(n) & "',"
                End If
                If StrTbLc(n) = "license_factory_status" Then
                    If Not TbLc(n) Is Nothing Then
                        _SQL &= "flag_status = 0, update_status = GETDATE(),"
                    End If
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE license_factory_id = " & license_factory_id
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbLc.Length - 1
                    If Not TbLc(n) Is Nothing Then
                        GbFn.KeepLog(StrTbLc(n), TbLc(n), "Editing", IdTable, license_factory_id)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertLicenseFactory(ByVal driver_name As String, ByVal IdNo As String, ByVal start_date As DateTime, ByVal expire_date As DateTime, ByVal name_factory As String, ByVal license_factory_status As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO license_factory ([IdNo],[driver_id],[start_date],[expire_date],[name_factory],[license_factory_status],[create_by_user_id]) OUTPUT Inserted.license_factory_id VALUES "
            _SQL &= "(N'" & IdNo & "', '" & driver_name & "', '" & start_date & "', '" & expire_date & "', N'" & name_factory & "', N'" & license_factory_status & "', '" & Session("UserId") & "')"
            If Not driver_name Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbLc() As String = {"IdNo", "driver_id", "start_date", "expire_date", "name_factory", "license_factory_status"}
                Dim TbLc() As Object = {IdNo, driver_name, start_date, expire_date, name_factory, license_factory_status}
                For n As Integer = 0 To TbLc.Length - 1
                    If Not TbLc(n) Is Nothing Then
                        GbFn.KeepLog(StrTbLc(n), TbLc(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertFileLicenseFactory() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim fk_id As String = String.Empty
                Dim newFile As String = String.Empty

                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        End If
                    Next
                    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                    If Request.Files.Count <> 0 Then

                        Dim pathServer As String = Server.MapPath("~/Files/LF/" & fk_id)
                        If (Not System.IO.Directory.Exists(pathServer)) Then
                            System.IO.Directory.CreateDirectory(pathServer)
                        End If
                        Dim fileName As String = String.Empty
                        For i As Integer = 0 To Request.Files.Count - 1
                            Dim file = Request.Files(i)
                            fileName = file.FileName
                            file.SaveAs(pathServer & "/" & fileName)
                            Dim _SQL As String = "UPDATE license_factory SET path = N'../Files/LF/" & fk_id & "/" & file.FileName & "' WHERE license_factory_id = " & fk_id
                            objDB.ExecuteSQL(_SQL, cn)
                        Next

                        DtJson.Rows.Add("../Files/LF/" & fk_id & "/" & fileName)
                    Else
                        DtJson.Rows.Add("0")
                    End If

                    objDB.DisconnectDB(cn)

                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        'Public Function DeleteFileLicenseFactory(ByVal keyId As String) As String
        '    Dim DtJson As DataTable = New DataTable
        '    DtJson.Columns.Add("Status")
        '    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        '    Dim _SQL As String = String.Empty
        '    _SQL = "SELECT * FROM license_v8 WHERE lv8_id = " & keyId
        '    Dim dtLc As DataTable = objDB.SelectSQL(_SQL, cn)
        '    If dtLc.Rows.Count > 0 Then
        '        Dim pathServer As String = Server.MapPath(dtLc.Rows(0)("lv8_path").Replace("..", "~"))
        '        If System.IO.File.Exists(pathServer) = True Then
        '            System.IO.File.Delete(pathServer)
        '        End If
        '        DtJson.Rows.Add("1")
        '    Else
        '        DtJson.Rows.Add("0")
        '    End If

        '    objDB.DisconnectDB(cn)
        '    Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        'End Function

        Public Function DeleteLicenseFactory(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "DELETE license_factory WHERE license_factory_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#Region "License_V8"
        Function LicenseV8() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function GetColumnChooserLv8(ByVal table_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(cc.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id WHERE table_id = " & table_id & " ORDER BY cc.sort ASC"
            Dim DtTax As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtTax.Rows Select DtTax.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetLv8() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT lv8.*,N'ประวัติ' as history, l.license_car FROM license_v8 as lv8 join license as l on lv8.license_id = l.license_id order by LEN(l.number_car),l.number_car"
            Dim DtLmr As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLmr.Rows Select DtLmr.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateLv8(ByVal license_id As String, ByVal ownership As String, ByVal tax_Id As String, ByVal lv8_mpa As String, ByVal lv8_rd As String, ByVal lv8_id As String, ByVal lv8_number As String, ByVal lv8_expire As String, ByVal lv8_start As String, ByVal lv8_status As String, ByVal name_hazmat1 As String, ByVal name_hazmat2 As String, ByVal name_hazmat3 As String, ByVal name_hazmat4 As String, ByVal name_hazmat5 As String, ByVal name_hazmat6 As String, ByVal IdTable As String, ByVal note As String, ByVal group_update As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [license_v8] SET "
            Dim StrTbLc() As String = {"lv8_number", "license_id", "ownership", "tax_Id", "lv8_start", "lv8_expire", "lv8_mpa", "lv8_rd", "lv8_status", "name_hazmat1", "name_hazmat2", "name_hazmat3", "name_hazmat4", "name_hazmat5", "name_hazmat6", "note", "group_update"}
            Dim TbLc() As Object = {lv8_number, license_id, ownership, tax_Id, lv8_start, lv8_expire, lv8_mpa, lv8_rd, lv8_status, name_hazmat1, name_hazmat2, name_hazmat3, name_hazmat4, name_hazmat5, name_hazmat6, note, group_update}
            For n As Integer = 0 To TbLc.Length - 1
                If Not TbLc(n) Is Nothing Then
                    _SQL &= StrTbLc(n) & "=N'" & TbLc(n) & "',"
                End If
                If StrTbLc(n) = "lv8_status" Then
                    If Not TbLc(n) Is Nothing Then
                        _SQL &= "flag_status = 0, update_status = GETDATE(),"
                    End If
                End If

            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE lv8_id = " & lv8_id
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbLc.Length - 1
                    If Not TbLc(n) Is Nothing Then
                        GbFn.KeepLog(StrTbLc(n), TbLc(n), "Editing", IdTable, lv8_id)
                    End If
                Next
                'Group Update
                If Not lv8_status Is Nothing Then
                    DtJson.Rows.Add(group_update_("license_v8", lv8_status, "lv8_status", lv8_id, "lv8_id"))
                End If
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertLv8(ByVal license_id As String, ByVal ownership As String, ByVal tax_Id As String, ByVal lv8_mpa As String, ByVal lv8_rd As String, ByVal lv8_number As String, ByVal lv8_expire As DateTime, ByVal lv8_start As DateTime, ByVal lv8_status As String, ByVal IdTable As String, ByVal note As String, ByVal group_update As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO license_v8 ([lv8_number],[license_id],[ownership],[tax_Id],[lv8_start],[lv8_expire],[lv8_mpa],[lv8_rd],[lv8_status],[note],[create_by_user_id],[group_update]) OUTPUT Inserted.lv8_id VALUES "
            _SQL &= "(N'" & lv8_number & "', '" & license_id & "', N'" & ownership & "', N'" & tax_Id & "', '" & lv8_start & "', '" & lv8_expire & "', N'" & lv8_mpa & "', N'" & lv8_rd & "', N'" & lv8_status & "', N'" & note & "', '" & Session("UserId") & "', N'" & group_update & "' )"
            If Not license_id Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbLc() As String = {"lv8_number", "license_id", "ownership", "tax_Id", "lv8_start", "lv8_expire", "lv8_mpa", "lv8_rd", "lv8_status", "note"}
                Dim TbLc() As Object = {lv8_number, license_id, ownership, tax_Id, lv8_start, lv8_expire, lv8_mpa, lv8_rd, lv8_status, note}
                For n As Integer = 0 To TbLc.Length - 1
                    If Not TbLc(n) Is Nothing Then
                        GbFn.KeepLog(StrTbLc(n), TbLc(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertFileLv8() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim fk_id As String = String.Empty
                Dim newFile As String = String.Empty

                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        End If
                    Next
                    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                    If Request.Files.Count <> 0 Then

                        Dim pathServer As String = Server.MapPath("~/Files/LV8/" & fk_id)
                        If (Not System.IO.Directory.Exists(pathServer)) Then
                            System.IO.Directory.CreateDirectory(pathServer)
                        End If
                        Dim fileName As String = String.Empty
                        For i As Integer = 0 To Request.Files.Count - 1
                            Dim file = Request.Files(i)
                            fileName = file.FileName
                            file.SaveAs(pathServer & "/" & fileName)
                            Dim _SQL As String = "UPDATE license_v8 SET path = N'../Files/LV8/" & fk_id & "/" & file.FileName & "' WHERE lv8_id = " & fk_id
                            objDB.ExecuteSQL(_SQL, cn)
                        Next

                        DtJson.Rows.Add("../Files/LV8/" & fk_id & "/" & fileName)
                    Else
                        DtJson.Rows.Add("0")
                    End If

                    objDB.DisconnectDB(cn)

                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteFileLv8(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "SELECT * FROM license_v8 WHERE lv8_id = " & keyId
            Dim dtLc As DataTable = objDB.SelectSQL(_SQL, cn)
            If dtLc.Rows.Count > 0 Then
                Dim pathServer As String = Server.MapPath(dtLc.Rows(0)("lv8_path").Replace("..", "~"))
                If System.IO.File.Exists(pathServer) = True Then
                    System.IO.File.Delete(pathServer)
                End If
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteLv8(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "DELETE license_v8 WHERE lv8_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#Region "Utility"

        Public Function ConvertBase64ToByteArray(base64 As String) As Byte()
            Return Convert.FromBase64String(base64) 'Convert the base64 back to byte array.
        End Function

        'Here's the part of your code (which works)
        Private Function convertbytetoimage(ByVal BA As Byte())
            Dim ms As MemoryStream = New MemoryStream(BA)
            Dim image = System.Drawing.Image.FromStream(ms)
            Return image
        End Function

        Private Function fnGetPath(ByVal Id As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _Path As String = String.Empty
            Dim _SQL As String = "SELECT parentDirId, name_file FROM files_all WHERE file_id = '" & Id & "'"
            Dim dtPdi As DataTable = objDB.SelectSQL(_SQL, cn)
            If dtPdi.Rows.Count > 0 Then
                _Path = dtPdi.Rows(0)("name_file")
                While dtPdi.Rows.Count > 0
                    _SQL = "SELECT parentDirId, name_file FROM files_all WHERE file_id = '" & dtPdi.Rows(0)("parentDirId") & "'"
                    dtPdi = objDB.SelectSQL(_SQL, cn)
                    If dtPdi.Rows.Count > 0 Then
                        _Path = dtPdi.Rows(0)("name_file") & "/" & _Path
                    End If
                End While
            End If
            objDB.DisconnectDB(cn)
            Return _Path
        End Function

        Public Function GetFiles(ByVal table_id As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT * FROM [files_all] WHERE table_id = " & table_id
            Dim DtFiles As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtFiles.Rows Select DtFiles.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetNumberCar() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "select l.* from (SELECT distinct(number_car), license_id, license_car,style_car,shaft FROM license) l order by LEN(l.number_car),l.number_car"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetLicenseCar(ByVal license_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT license_car FROM license WHERE license_id = " & license_id
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#End Region

#Region "Develop by Tew"
        'Global Function(Tew)
        Dim GbFn As GlobalFunction = New GlobalFunction


#Region "Main Insurance"
        Function MainInsurance() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table act_insurance
        Public Function GetMIData() As String
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history FROM [dbo].[main_insurance] mi , [dbo].[license] li where mi.license_id = li.license_id order by LEN(li.number_car),li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameMI() As String
            Return GbFn.fnRename(Request, "main_insurance")
        End Function

        Public Function UpdateMI(ByVal number_car As String, ByVal insurance_company As String, ByVal start_date As String, ByVal end_date As String, ByVal note As String _
                                  , ByVal policy_number As String, ByVal assured As String, ByVal current_cowrie As String, ByVal previous_cowrie As String, ByVal status As String, ByVal key As String, ByVal IdTable As String, ByVal group_update As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [main_insurance] SET "
            Dim StrTbAI() As String = {"insurance_company", "start_date", "end_date", "note", "policy_number", "assured", "current_cowrie", "previous_cowrie", "status", "group_update"}
            Dim TbAI() As Object = {insurance_company, start_date, end_date, note, policy_number, assured, current_cowrie, previous_cowrie, status, group_update}
            For n As Integer = 0 To TbAI.Length - 1
                If Not TbAI(n) Is Nothing Then
                    _SQL &= StrTbAI(n) & "=N'" & TbAI(n) & "',"
                End If
                If StrTbAI(n) = "status" Then
                    If Not TbAI(n) Is Nothing Then
                        _SQL &= "flag_status = 0, update_status = GETDATE(),"
                    End If
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE mi_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbAI.Length - 1
                    If Not TbAI(n) Is Nothing Then
                        GbFn.KeepLog(StrTbAI(n), TbAI(n), "Editing", IdTable, key)
                    End If
                Next
                'Group Update
                If Not status Is Nothing Then
                    DtJson.Rows.Add(group_update_("main_insurance", status, "status", key, "mi_id"))
                End If
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertMI(ByVal number_car As String, ByVal insurance_company As String, ByVal start_date As String, ByVal end_date As String, ByVal note As String _
                                  , ByVal policy_number As String, ByVal assured As String, ByVal current_cowrie As String, ByVal previous_cowrie As String, ByVal status As String, ByVal key As String, ByVal IdTable As String, ByVal group_update As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [main_insurance] ([license_id],[insurance_company],[start_date],[end_date],[policy_number],[assured],[current_cowrie],[previous_cowrie],[status],[note],[create_date],[create_by_user_id],[group_update]) OUTPUT Inserted.mi_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(insurance_company Is Nothing, String.Empty, insurance_company) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(end_date Is Nothing, String.Empty, end_date) & "',"
            _SQL &= "N'" & IIf(policy_number Is Nothing, String.Empty, policy_number) & "',"
            _SQL &= "N'" & IIf(assured Is Nothing, String.Empty, assured) & "',"
            _SQL &= "N'" & IIf(current_cowrie Is Nothing, String.Empty, current_cowrie) & "',"
            _SQL &= "N'" & IIf(previous_cowrie Is Nothing, String.Empty, previous_cowrie) & "',"
            _SQL &= "N'" & IIf(status Is Nothing, String.Empty, status) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ","
            _SQL &= "N'" & group_update & "')"

            If Not number_car Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbMI() As String = {"insurance_company", "insurance_date", "expire_date", "note", "policy_number", "assured", "current_cowrie", "previous_cowrie", "status"}
                Dim TbMI() As Object = {insurance_company, start_date, end_date, note, policy_number, assured, current_cowrie, previous_cowrie, status}
                For n As Integer = 0 To TbMI.Length - 1
                    If Not TbMI(n) Is Nothing Then
                        _SQL &= StrTbMI(n) & "=N'" & TbMI(n) & "',"
                        GbFn.KeepLog(StrTbMI(n), TbMI(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function DeleteMI(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [main_insurance] WHERE mi_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#Region "Domestic Product Insurance"
        Function DomProIns() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table act_insurance
        Public Function GetDPIData() As String
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history,N'View' as first_damages_view FROM [dbo].[domestic_product_insurance] dpi , [dbo].[license] li where dpi.license_id = li.license_id order by LEN(li.number_car),li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameDPI() As String
            Return GbFn.fnRename(Request, "domestic_product_insurance")
        End Function

        Public Function UpdateDPI(ByVal number_car As String, ByVal insurance_company As String, ByVal policy_number As String, ByVal start_date As String, ByVal end_date As String, ByVal current_cowrie As String, ByVal previous_cowrie As String, ByVal note As String _
                                  , ByVal status As String, ByVal first_damages As String, ByVal key As String, ByVal IdTable As String, ByVal group_update As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [domestic_product_insurance] SET "
            Dim StrTbDPI() As String = {"insurance_company", "policy_number", "start_date", "end_date", "current_cowrie", "previous_cowrie", "note", "status", "first_damages", "group_update"}
            Dim TbDPI() As Object = {insurance_company, policy_number, start_date, end_date, current_cowrie, previous_cowrie, note, status, first_damages, group_update}
            For n As Integer = 0 To TbDPI.Length - 1
                If Not TbDPI(n) Is Nothing Then
                    _SQL &= StrTbDPI(n) & "=N'" & TbDPI(n) & "',"
                End If
                If StrTbDPI(n) = "status" Then
                    If Not TbDPI(n) Is Nothing Then
                        _SQL &= "flag_status = 0, update_status = GETDATE(),"
                    End If
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE dpi_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbDPI.Length - 1
                    If Not TbDPI(n) Is Nothing Then
                        GbFn.KeepLog(StrTbDPI(n), TbDPI(n), "Editing", IdTable, key)
                    End If
                Next
                'Group Update
                If Not status Is Nothing Then
                    DtJson.Rows.Add(group_update_("domestic_product_insurance", status, "status", key, "dpi_id"))
                End If
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertDPI(ByVal number_car As String, ByVal insurance_company As String, ByVal policy_number As String, ByVal start_date As String, ByVal end_date As String, ByVal current_cowrie As String, ByVal previous_cowrie As String, ByVal note As String _
                                  , ByVal status As String, ByVal first_damages As String, ByVal key As String, ByVal IdTable As String, ByVal group_update As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [domestic_product_insurance] ([license_id],[insurance_company],[policy_number],[start_date],[end_date],[current_cowrie],[previous_cowrie],[note],[status],[first_damages],[create_date],[create_by_user_id],[group_update]) OUTPUT Inserted.dpi_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(insurance_company Is Nothing, String.Empty, insurance_company) & "',"
            _SQL &= "N'" & IIf(policy_number Is Nothing, String.Empty, policy_number) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(end_date Is Nothing, String.Empty, end_date) & "',"
            _SQL &= "N'" & IIf(current_cowrie Is Nothing, String.Empty, current_cowrie) & "',"
            _SQL &= "N'" & IIf(previous_cowrie Is Nothing, String.Empty, previous_cowrie) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "N'" & IIf(status Is Nothing, String.Empty, status) & "',"
            _SQL &= "N'" & IIf(first_damages Is Nothing, String.Empty, first_damages) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ","
            _SQL &= "N'" & group_update & "')"
            If Not number_car Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbDPI() As String = {"insurance_company", "policy_number", "start_date", "end_date", "current_cowrie", "previous_cowrie", "note", "status", "first_damages"}
                Dim TbDPI() As Object = {insurance_company, policy_number, start_date, end_date, current_cowrie, previous_cowrie, note, status, first_damages}
                For n As Integer = 0 To TbDPI.Length - 1
                    If Not TbDPI(n) Is Nothing Then
                        GbFn.KeepLog(StrTbDPI(n), TbDPI(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function DeleteDPI(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [domestic_product_insurance] WHERE dpi_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function updateFirstDamages(ByVal dpi_id As String, ByVal data As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "UPDATE [dbo].[domestic_product_insurance] SET [first_damages] = N'" & data & "' WHERE dpi_id = '" & dpi_id & "'"
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                GbFn.KeepLog("first_damages", data, "Editing", IdTable, dpi_id)
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))

        End Function
#End Region

#Region "ACT Insurance"
        Function ActInsurance() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table act_insurance
        Public Function GetAIData() As String
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history FROM [dbo].[act_insurance] ai , [dbo].[license] li where ai.license_id = li.license_id order by LEN(li.number_car),li.number_car ")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameAI() As String
            Return GbFn.fnRename(Request, "act_insurance")
        End Function

        Public Function UpdateAI(ByVal number_car As String, ByVal insurance_company As String, ByVal start_date As String, ByVal end_date As String, ByVal note As String _
                                  , ByVal status As String, ByVal price As String, ByVal policy_number As String, ByVal first_damages As String, ByVal add_damages As String, ByVal key As String, ByVal IdTable As String, ByVal group_update As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [act_insurance] SET "
            Dim StrTbAI() As String = {"insurance_company", "start_date", "end_date", "note", "status", "price", "policy_number", "first_damages", "add_damages", "group_update"}
            Dim TbAI() As Object = {insurance_company, start_date, end_date, note, status, price, policy_number, first_damages, add_damages, group_update}
            For n As Integer = 0 To TbAI.Length - 1
                If Not TbAI(n) Is Nothing Then
                    _SQL &= StrTbAI(n) & "=N'" & TbAI(n) & "',"
                End If
                If StrTbAI(n) = "status" Then
                    If Not TbAI(n) Is Nothing Then
                        _SQL &= "flag_status = 0, update_status = GETDATE(),"
                    End If
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE ai_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbAI.Length - 1
                    If Not TbAI(n) Is Nothing Then
                        GbFn.KeepLog(StrTbAI(n), TbAI(n), "Editing", IdTable, key)
                    End If
                Next
                'Group Update
                If Not status Is Nothing Then
                    DtJson.Rows.Add(group_update_("act_insurance", status, "status", key, "ai_id"))
                End If
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertAI(ByVal number_car As String, ByVal insurance_company As String, ByVal start_date As String, ByVal end_date As String, ByVal note As String _
                                  , ByVal status As String, ByVal price As String, ByVal policy_number As String, ByVal first_damages As String, ByVal add_damages As String, ByVal key As String, ByVal IdTable As String, ByVal group_update As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [act_insurance] ([license_id],[insurance_company],[start_date],[end_date],[note],[status],[price],[policy_number],[first_damages],[add_damages],[create_date],[create_by_user_id],[group_update]) OUTPUT Inserted.ai_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(insurance_company Is Nothing, String.Empty, insurance_company) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(end_date Is Nothing, String.Empty, end_date) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "N'" & IIf(status Is Nothing, String.Empty, status) & "',"
            _SQL &= "N'" & IIf(price Is Nothing, String.Empty, price) & "',"
            _SQL &= "N'" & IIf(policy_number Is Nothing, String.Empty, policy_number) & "',"
            _SQL &= "N'" & IIf(first_damages Is Nothing, String.Empty, first_damages) & "',"
            _SQL &= "N'" & IIf(add_damages Is Nothing, String.Empty, add_damages) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ","
            _SQL &= "N'" & group_update & "')"
            If Not number_car Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbAI() As String = {"insurance_company", "start_date", "end_date", "note", "status", "price", "policy_number", "first_damages", "add_damages"}
                Dim TbAI() As Object = {insurance_company, start_date, end_date, note, price, policy_number, first_damages, add_damages}
                For n As Integer = 0 To TbAI.Length - 1
                    If Not TbAI(n) Is Nothing Then
                        GbFn.KeepLog(StrTbAI(n), TbAI(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function DeleteAI(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [act_insurance] WHERE ai_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
#End Region

#Region "Environment Insurance"
        Function EnvironmentInsurance() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table act_insurance
        Public Function GetEIData() As String
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history FROM [dbo].[environment_insurance] ei , [dbo].[license] li where ei.license_id = li.license_id order by LEN(li.number_car),li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameEI() As String
            Return GbFn.fnRename(Request, "environment_insurance")
        End Function

        Public Function UpdateEI(ByVal number_car As String, ByVal insurance_company As String, ByVal start_date As String, ByVal end_date As String, ByVal note As String _
                                  , ByVal key As String, ByVal IdTable As String, ByVal current_cowrie As String, ByVal previous_cowrie As String, ByVal first_damages As String, ByVal policy_number As String, ByVal status As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [environment_insurance] SET "
            Dim StrTbEI() As String = {"insurance_company", "start_date", "end_date", "note", "current_cowrie", "previous_cowrie", "first_damages", "policy_number", "status"}
            Dim TbEI() As Object = {insurance_company, start_date, end_date, note, current_cowrie, previous_cowrie, first_damages, policy_number, status}
            For n As Integer = 0 To TbEI.Length - 1
                If Not TbEI(n) Is Nothing Then
                    _SQL &= StrTbEI(n) & "=N'" & TbEI(n) & "',"
                End If
                If StrTbEI(n) = "status" Then
                    If Not TbEI(n) Is Nothing Then
                        _SQL &= "flag_status = 0, update_status = GETDATE(),"
                    End If
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE ei_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbEI.Length - 1
                    If Not TbEI(n) Is Nothing Then
                        GbFn.KeepLog(StrTbEI(n), TbEI(n), "Editing", IdTable, key)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertEI(ByVal number_car As String, ByVal insurance_company As String, ByVal start_date As String, ByVal end_date As String, ByVal note As String _
                                  , ByVal key As String, ByVal IdTable As String, ByVal current_cowrie As String, ByVal previous_cowrie As String, ByVal first_damages As String, ByVal policy_number As String, ByVal status As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [environment_insurance] ([license_id],[insurance_company],[start_date],[end_date],[note],[create_date],[create_by_user_id],[current_cowrie],[previous_cowrie],[first_damages],[policy_number],[status]) OUTPUT Inserted.ei_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(insurance_company Is Nothing, String.Empty, insurance_company) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(end_date Is Nothing, String.Empty, end_date) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ","
            _SQL &= "N'" & IIf(current_cowrie Is Nothing, String.Empty, current_cowrie) & "',"
            _SQL &= "N'" & IIf(previous_cowrie Is Nothing, String.Empty, previous_cowrie) & "',"
            _SQL &= "N'" & IIf(first_damages Is Nothing, String.Empty, first_damages) & "',"
            _SQL &= "N'" & IIf(policy_number Is Nothing, String.Empty, policy_number) & "',"
            _SQL &= "N'" & IIf(status Is Nothing, String.Empty, status) & "')"
            If Not number_car Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbEI() As String = {"insurance_company", "start_date", "end_date", "note", "current_cowrie", "previous_cowrie", "first_damages", "policy_number", "status"}
                Dim TbEI() As Object = {insurance_company, start_date, end_date, note, current_cowrie, previous_cowrie, first_damages, policy_number, status}
                For n As Integer = 0 To TbEI.Length - 1
                    If Not TbEI(n) Is Nothing Then
                        GbFn.KeepLog(StrTbEI(n), TbEI(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function DeleteEI(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [environment_insurance] WHERE ei_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
#End Region

#Region "Act Insurance Company (Tew)"
        Function ActInsCom() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table Main Insurance Company
        Public Function GetAICData() As String
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history,N'View' as protection_view FROM [dbo].[act_insurance_company] ai")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameAIC() As String
            Return GbFn.fnRename(Request, "act_insurance_company")
        End Function
        Public Function UpdateAIC(ByVal name As String, ByVal protection As String, ByVal protection_limit As String _
                                      , ByVal note As String, ByVal key As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [act_insurance_company] SET "
            Dim StrTbAIC() As String = {"name", "protection", "protection_limit", "note"}
            Dim TbAIC() As Object = {name, protection, protection_limit, note}
            For n As Integer = 0 To TbAIC.Length - 1
                If Not TbAIC(n) Is Nothing Then
                    _SQL &= StrTbAIC(n) & "=N'" & TbAIC(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE aic_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbAIC.Length - 1
                    If Not TbAIC(n) Is Nothing Then
                        GbFn.KeepLog(StrTbAIC(n), TbAIC(n), "Editing", IdTable, key)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertAIC(ByVal name As String, ByVal protection As String, ByVal protection_limit As String _
                                      , ByVal note As String, ByVal key As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO [act_insurance_company] ([name],[protection],[protection_limit],[note],[create_date],[create_by_user_id]) OUTPUT Inserted.aic_id"
            _SQL &= " VALUES (N'" & IIf(name Is Nothing, String.Empty, name) & "',"
            _SQL &= "N'" & IIf(protection Is Nothing, String.Empty, protection) & "',"
            _SQL &= "N'" & IIf(protection_limit Is Nothing, String.Empty, protection_limit) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            'If Not number_car Is Nothing Then
            DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            'Else
            '    DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            'End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbAIC() As String = {"name", "protection", "protection_limit", "note"}
                Dim TbAIC() As Object = {name, protection, protection_limit, note}
                For n As Integer = 0 To TbAIC.Length - 1
                    If Not TbAIC(n) Is Nothing Then
                        GbFn.KeepLog(StrTbAIC(n), TbAIC(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function DeleteAIC(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [act_insurance_company] WHERE aic_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateProtectionAic(ByVal aic_id As String, ByVal data As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "UPDATE [dbo].[act_insurance_company] SET [protection] = N'" & data & "' WHERE aic_id = '" & aic_id & "'"
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                GbFn.KeepLog("protection", data, "Editing", IdTable, aic_id)
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
            aic_id = 0

        End Function

#End Region

#Region "Main Insurance Company (Tew)"
        Function MainInsCom() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table Main Insurance Company
        Public Function GetMICData() As String
            Return GbFn.GetData("
                                Select mic.[mic_id]
                                    ,mic.[insurance_company]
                                    ,mic.[type]
                                    ,mic.[extension_insurance]
                                    ,mic.[note]
                                    ,mic.[create_date]
                                    ,mic.[create_by_user_id]
                                    ,mic.[update_date]
                                    ,mic.[update_by_user_id]
                                    ,IIF ( ISNUMERIC ( mic.[t1_1_1] )		=1		, CONVERT(varchar, CAST(mic.[t1_1_1]	AS money ), 1), mic.[t1_1_1] )		as [t1_1_1]
                                    ,IIF ( ISNUMERIC ( mic.[t1_1_2] )		=1		, CONVERT(varchar, CAST(mic.[t1_1_2]	AS money ), 1), mic.[t1_1_2] )		as [t1_1_2]
                                    ,IIF ( ISNUMERIC ( mic.[t1_2_1] )		=1		, CONVERT(varchar, CAST(mic.[t1_2_1]	AS money ), 1), mic.[t1_2_1] )		as [t1_2_1]
                                    ,IIF ( ISNUMERIC ( mic.[t1_2_2] )		=1		, CONVERT(varchar, CAST(mic.[t1_2_2]	AS money ), 1), mic.[t1_2_2] )		as [t1_2_2]
                                    ,IIF ( ISNUMERIC ( mic.[t2_1_1] )		=1		, CONVERT(varchar, CAST(mic.[t2_1_1]	AS money ), 1), mic.[t2_1_1] )		as [t2_1_1]
                                    ,IIF ( ISNUMERIC ( mic.[t2_1_2] )		=1		, CONVERT(varchar, CAST(mic.[t2_1_2]	AS money ), 1), mic.[t2_1_2] )		as [t2_1_2]
                                    ,IIF ( ISNUMERIC ( mic.[t2_2_1] )		=1		, CONVERT(varchar, CAST(mic.[t2_2_1]	AS money ), 1), mic.[t2_2_1] )		as [t2_2_1]
                                    ,IIF ( ISNUMERIC ( mic.[t3_1_1_a])	=1		, CONVERT(varchar, CAST(mic.[t3_1_1_a]	AS money ), 1), mic.[t3_1_1_a] )	as [t3_1_1_a]
                                    ,IIF ( ISNUMERIC ( mic.[t3_1_1_b] )	=1		, CONVERT(varchar, CAST(mic.[t3_1_1_b]	AS money ), 1), mic.[t3_1_1_b] )	as [t3_1_1_b]
                                    ,IIF ( ISNUMERIC ( mic.[t3_1_2_a] )	=1		, CONVERT(varchar, CAST(mic.[t3_1_2_a]	AS money ), 1), mic.[t3_1_2_a] )	as [t3_1_2_a]
                                    ,IIF ( ISNUMERIC ( mic.[t3_1_2_b] )	=1		, CONVERT(varchar, CAST(mic.[t3_1_2_b]	AS money ), 1), mic.[t3_1_2_b] )	as [t3_1_2_b]
                                    ,IIF ( ISNUMERIC ( mic.[t3_2_1] )		=1		, CONVERT(varchar, CAST(mic.[t3_2_1]	AS money ), 1), mic.[t3_2_1] )		as [t3_2_1]
                                    ,IIF ( ISNUMERIC ( mic.[t3_3_1] )		=1		, CONVERT(varchar, CAST(mic.[t3_3_1]	AS money ), 1), mic.[t3_3_1] )		as [t3_3_1]
                                    ,N'ประวัติ' as history
	                                FROM [dbo].[main_insurance_company] mic 
                            ")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameMIC() As String
            Return GbFn.fnRename(Request, "main_insurance_company")
        End Function
        Public Function UpdateMIC(ByVal insurance_company As String _
                                  , ByVal type As String _
                                  , ByVal extension_insurance As String, ByVal note As String, ByVal key As String, ByVal t1_1_1 As String, ByVal t1_1_2 As String, ByVal t1_2_1 As String _
                                  , ByVal t1_2_2 As String, ByVal t2_1_1 As String, ByVal t2_1_2 As String, ByVal t2_2_1 As String, ByVal t3_1_1_a As String, ByVal t3_1_1_b As String _
                                  , ByVal t3_1_2_a As String, ByVal t3_1_2_b As String, ByVal t3_2_1 As String, ByVal t3_3_1 As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [main_insurance_company] SET "
            Dim StrTbMIC() As String = {"insurance_company", "type", "extension_insurance", "note", "t1_1_1", "t1_1_2", "t1_2_1", "t1_2_2", "t2_1_1", "t2_1_2", "t2_2_1", "t3_1_1_a", "t3_1_1_b", "t3_1_2_a", "t3_1_2_b", "t3_2_1", "t3_3_1"}
            Dim TbMIC() As Object = {insurance_company, type, extension_insurance, note, t1_1_1, t1_1_2, t1_2_1, t1_2_2, t2_1_1, t2_1_2, t2_2_1, t3_1_1_a, t3_1_1_b, t3_1_2_a, t3_1_2_b, t3_2_1, t3_3_1}
            For n As Integer = 0 To TbMIC.Length - 1
                If Not TbMIC(n) Is Nothing Then
                    _SQL &= StrTbMIC(n) & " = N'" & TbMIC(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE mic_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbMIC.Length - 1
                    If Not TbMIC(n) Is Nothing Then
                        GbFn.KeepLog(StrTbMIC(n), TbMIC(n), "Editing", IdTable, key)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertMIC(ByVal insurance_company As String _
                                  , ByVal type As String _
                                  , ByVal extension_insurance As String, ByVal note As String, ByVal key As String, ByVal t1_1_1 As String, ByVal t1_1_2 As String, ByVal t1_2_1 As String _
                                  , ByVal t1_2_2 As String, ByVal t2_1_1 As String, ByVal t2_1_2 As String, ByVal t2_2_1 As String, ByVal t3_1_1_a As String, ByVal t3_1_1_b As String _
                                  , ByVal t3_1_2_a As String, ByVal t3_1_2_b As String, ByVal t3_2_1 As String, ByVal t3_3_1 As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO [main_insurance_company] ([insurance_company],[type]" &
                ",[extension_insurance],[note],[create_date],[create_by_user_id],[t1_1_1],[t1_1_2],[t1_2_1],[t1_2_2],[t2_1_1],[t2_1_2],[t2_2_1]" &
                ",[t3_1_1_a],[t3_1_1_b],[t3_1_2_a],[t3_1_2_b],[t3_2_1],[t3_3_1]) OUTPUT Inserted.mic_id"
            _SQL &= " VALUES (N'" & IIf(insurance_company Is Nothing, String.Empty, insurance_company) & "',"
            _SQL &= "N'" & IIf(type Is Nothing, String.Empty, type) & "',"
            _SQL &= "N'" & IIf(extension_insurance Is Nothing, String.Empty, extension_insurance) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ","
            _SQL &= "N'" & IIf(t1_1_1 Is Nothing, String.Empty, t1_1_1) & "',"
            _SQL &= "N'" & IIf(t1_1_2 Is Nothing, String.Empty, t1_1_2) & "',"
            _SQL &= "N'" & IIf(t1_2_1 Is Nothing, String.Empty, t1_2_1) & "',"
            _SQL &= "N'" & IIf(t1_2_2 Is Nothing, String.Empty, t1_2_2) & "',"
            _SQL &= "N'" & IIf(t2_1_1 Is Nothing, String.Empty, t2_1_1) & "',"
            _SQL &= "N'" & IIf(t2_1_2 Is Nothing, String.Empty, t2_1_2) & "',"
            _SQL &= "N'" & IIf(t2_2_1 Is Nothing, String.Empty, t2_2_1) & "',"
            _SQL &= "N'" & IIf(t3_1_1_a Is Nothing, String.Empty, t3_1_1_a) & "',"
            _SQL &= "N'" & IIf(t3_1_1_b Is Nothing, String.Empty, t3_1_1_b) & "',"
            _SQL &= "N'" & IIf(t3_1_2_a Is Nothing, String.Empty, t3_1_2_a) & "',"
            _SQL &= "N'" & IIf(t3_1_2_b Is Nothing, String.Empty, t3_1_2_b) & "',"
            _SQL &= "N'" & IIf(t3_2_1 Is Nothing, String.Empty, t3_2_1) & "',"
            _SQL &= "N'" & IIf(t3_3_1 Is Nothing, String.Empty, t3_3_1) & "'"
            _SQL &= ")"

            'If Not number_car Is Nothing Then
            DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            'Else
            '    DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            'End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbMIC() As String = {"insurance_company", "type", "extension_insurance", "note", "t1_1_1", "t1_1_2", "t1_2_1", "t1_2_2", "t2_1_1", "t2_1_2", "t2_2_1", "t3_1_1_a", "t3_1_1_b", "t3_1_2_a", "t3_1_2_b", "t3_2_1", "t3_3_1"}
                Dim TbMIC() As Object = {insurance_company, type, extension_insurance, note, t1_1_1, t1_1_2, t1_2_1, t1_2_2, t2_1_1, t2_1_2, t2_2_1, t3_1_1_a, t3_1_1_b, t3_1_2_a, t3_1_2_b, t3_2_1, t3_3_1}
                For n As Integer = 0 To TbMIC.Length - 1
                    If Not TbMIC(n) Is Nothing Then
                        GbFn.KeepLog(StrTbMIC(n), TbMIC(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If


            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function DeleteMIC(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [main_insurance_company] WHERE mic_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


#End Region

#Region "Product Insurance Company (Tew)"
        Function ProInsCom() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table Product Insurance Company
        Public Function GetPICData() As String
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history,N'View' as protection_view,N'View' as not_protection_view FROM [dbo].[product_insurance_company] pic ")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenamePIC() As String
            Return GbFn.fnRename(Request, "product_insurance_company")
        End Function
        Public Function UpdatePIC(ByVal insurance_company As String, ByVal assured As String, ByVal protection_scope As String,
                                  ByVal note As String, ByVal t1 As String, ByVal t2 As String, ByVal t3 As String, ByVal t4 As String _
                                      , ByVal key As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [product_insurance_company] SET "
            Dim StrTbPIC() As String = {"insurance_company", "assured", "protection_scope", "note", "t1", "t2", "t3", "t4"}
            Dim TbPIC() As Object = {insurance_company, assured, protection_scope, note, t1, t2, t3, t4}
            For n As Integer = 0 To TbPIC.Length - 1
                If Not TbPIC(n) Is Nothing Then
                    _SQL &= StrTbPIC(n) & "=N'" & TbPIC(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE pic_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbPIC.Length - 1
                    If Not TbPIC(n) Is Nothing Then
                        GbFn.KeepLog(StrTbPIC(n), TbPIC(n), "Editing", IdTable, key)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertPIC(ByVal insurance_company As String, ByVal assured As String, ByVal protection_scope As String,
                                  ByVal note As String, ByVal t1 As String, ByVal t2 As String, ByVal t3 As String, ByVal t4 As String _
                                      , ByVal key As String, ByVal IdTable As String, ByVal DataHtmlEditorProtection As String, ByVal dataHtmlEditorNotProtection As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO [product_insurance_company] ([insurance_company],[assured],[protection_scope],[note],[t1],[t2],[t3],[t4],[protection],[not_protection],[create_date],[create_by_user_id]) OUTPUT Inserted.pic_id"
            _SQL &= " VALUES (N'" & IIf(insurance_company Is Nothing, String.Empty, insurance_company) & "',"
            _SQL &= "N'" & IIf(assured Is Nothing, String.Empty, assured) & "',"
            _SQL &= "N'" & IIf(protection_scope Is Nothing, String.Empty, protection_scope) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "N'" & IIf(t1 Is Nothing, String.Empty, t1) & "',"
            _SQL &= "N'" & IIf(t2 Is Nothing, String.Empty, t2) & "',"
            _SQL &= "N'" & IIf(t3 Is Nothing, String.Empty, t3) & "',"
            _SQL &= "N'" & IIf(t4 Is Nothing, String.Empty, t4) & "',"
            _SQL &= "N'" & IIf(DataHtmlEditorProtection Is Nothing, String.Empty, DataHtmlEditorProtection) & "',"
            _SQL &= "N'" & IIf(dataHtmlEditorNotProtection Is Nothing, String.Empty, dataHtmlEditorNotProtection) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"

            DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbPIC() As String = {"insurance_company", "assured", "protection_scope", "note", "t1", "t2", "t3", "t4", "protection", "not_protection"}
                Dim TbPIC() As Object = {insurance_company, assured, protection_scope, note, t1, t2, t3, t4, DataHtmlEditorProtection, dataHtmlEditorNotProtection}
                For n As Integer = 0 To TbPIC.Length - 1
                    If Not TbPIC(n) Is Nothing Then
                        GbFn.KeepLog(StrTbPIC(n), TbPIC(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function DeletePIC(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [product_insurance_company] WHERE pic_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateProtectionPic(ByVal pic_id As String, ByVal data As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "UPDATE [dbo].[product_insurance_company] SET [protection] = N'" & data & "' WHERE pic_id = '" & pic_id & "'"
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                GbFn.KeepLog("protection", data, "Editing", IdTable, pic_id)
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))

        End Function

        Public Function UpdateNotProtectionPic(ByVal pic_id As String, ByVal data As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "UPDATE [dbo].[product_insurance_company] SET [not_protection] = N'" & data & "' WHERE pic_id = '" & pic_id & "'"
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                GbFn.KeepLog("protection", data, "Editing", IdTable, pic_id)
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))

        End Function

#End Region

#Region "Environment Insurance Company"
        Function EnvInsCom() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table Product Insurance Company
        Public Function GetEICData() As String
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history FROM [dbo].[env_insurance_company] eic")
        End Function

        'Rename Folder or Files(eic,pdf)
        Public Function fnRenameEIC() As String
            Return GbFn.fnRename(Request, "env_insurance_company")
        End Function
        Public Function UpdateEIC(ByVal insurance_company As String, ByVal assured As String, ByVal protection_scope As String,
                                  ByVal note As String, ByVal t2_1_1 As String, ByVal t2_1_2 As String, ByVal t2_2 As String, ByVal t2_3 As String, ByVal t_conclude As String _
                                       , ByVal key As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [env_insurance_company] SET "
            Dim StrTbEIC() As String = {"insurance_company", "assured", "protection_scope", "note", "t2_1_1", "t2_1_2", "t2_2", "t2_3", "t_conclude"}
            Dim TbEIC() As Object = {insurance_company, assured, protection_scope, note, t2_1_1, t2_1_2, t2_2, t2_3, t_conclude}
            For n As Integer = 0 To TbEIC.Length - 1
                If Not TbEIC(n) Is Nothing Then
                    _SQL &= StrTbEIC(n) & "=N'" & TbEIC(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE eic_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbEIC.Length - 1
                    If Not TbEIC(n) Is Nothing Then
                        GbFn.KeepLog(StrTbEIC(n), TbEIC(n), "Editing", IdTable, key)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertEIC(ByVal insurance_company As String, ByVal assured As String, ByVal protection_scope As String,
                                  ByVal note As String, ByVal t2_1_1 As String, ByVal t2_1_2 As String, ByVal t2_2 As String, ByVal t2_3 As String, ByVal t_conclude As String _
                                       , ByVal key As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO [env_insurance_company] ([insurance_company],[assured],[protection_scope],[note],[t2_1_1],[t2_1_2],[t2_2],[t2_3],[t_conclude],[create_date],[create_by_user_id]) OUTPUT Inserted.eic_id"
            _SQL &= " VALUES ( N'" & IIf(insurance_company Is Nothing, String.Empty, insurance_company) & "',"
            _SQL &= "N'" & IIf(assured Is Nothing, String.Empty, assured) & "',"
            _SQL &= "N'" & IIf(protection_scope Is Nothing, String.Empty, protection_scope) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "N'" & IIf(t2_1_1 Is Nothing, String.Empty, t2_1_1) & "',"
            _SQL &= "N'" & IIf(t2_1_2 Is Nothing, String.Empty, t2_1_2) & "',"
            _SQL &= "N'" & IIf(t2_2 Is Nothing, String.Empty, t2_2) & "',"
            _SQL &= "N'" & IIf(t2_3 Is Nothing, String.Empty, t2_3) & "',"
            _SQL &= "N'" & IIf(t_conclude Is Nothing, String.Empty, t_conclude) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"

            DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))

            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbEIC() As String = {"insurance_company", "assured", "protection_scope", "note", "t2_1_1", "t2_1_2", "t2_2", "t2_3", "t_conclude"}
                Dim TbEIC() As Object = {insurance_company, assured, protection_scope, note, t2_1_1, t2_1_2, t2_2, t2_3, t_conclude}
                For n As Integer = 0 To TbEIC.Length - 1
                    If Not TbEIC(n) Is Nothing Then
                        GbFn.KeepLog(StrTbEIC(n), TbEIC(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If


            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function DeleteEIC(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [env_insurance_company] WHERE eic_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#Region "Gps Company (Tew)"
        Function GpsCompany() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table gps_company
        Public Function GetGpsCompanyData() As String
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history FROM [dbo].[gps_company] gc")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameGpsCompany() As String
            Return GbFn.fnRename(Request, "Gps_company")
        End Function
        Public Function UpdateGpsCompany(ByVal company As String, ByVal address As String, ByVal email As String _
                                         , ByVal contact_number As String, ByVal coordinator As String, ByVal delivery_conditions As String, ByVal term_of_payment As String _
                                         , ByVal note As String _
                                         , ByVal key As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [gps_company] SET "
            Dim StrTbGpsCompany() As String = {"company", "address", "email", "contact_number", "coordinator", "delivery_conditions", "term_of_payment", "note"}
            Dim TbGpsCompany() As Object = {company, address, email, contact_number, coordinator, delivery_conditions, term_of_payment, note}
            For n As Integer = 0 To TbGpsCompany.Length - 1
                If Not TbGpsCompany(n) Is Nothing Then
                    _SQL &= StrTbGpsCompany(n) & "=N'" & TbGpsCompany(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE gc_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbGpsCompany.Length - 1
                    If Not TbGpsCompany(n) Is Nothing Then
                        GbFn.KeepLog(StrTbGpsCompany(n), TbGpsCompany(n), "Editing", IdTable, key)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertGpsCompany(ByVal company As String, ByVal address As String, ByVal email As String _
                                         , ByVal contact_number As String, ByVal coordinator As String, ByVal delivery_conditions As String, ByVal term_of_payment As String _
                                         , ByVal note As String _
                                         , ByVal key As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO [gps_company] ([company],[address],[email],[contact_number],[coordinator],[delivery_conditions],[term_of_payment],[note],[create_date],[create_by_user_id]) OUTPUT Inserted.gc_id"
            _SQL &= " VALUES (N'" & IIf(company Is Nothing, String.Empty, company) & "',"
            _SQL &= "N'" & IIf(address Is Nothing, String.Empty, address) & "',"
            _SQL &= "N'" & IIf(email Is Nothing, String.Empty, email) & "',"
            _SQL &= "N'" & IIf(contact_number Is Nothing, String.Empty, contact_number) & "',"
            _SQL &= "N'" & IIf(coordinator Is Nothing, String.Empty, coordinator) & "',"
            _SQL &= "N'" & IIf(delivery_conditions Is Nothing, String.Empty, delivery_conditions) & "',"
            _SQL &= "N'" & IIf(term_of_payment Is Nothing, String.Empty, term_of_payment) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbGpsCompany() As String = {"company", "address", "email", "contact_number", "coordinator", "delivery_conditions", "term_of_payment", "note"}
                Dim TbGpsCompany() As Object = {company, address, email, contact_number, coordinator, delivery_conditions, term_of_payment, note}
                For n As Integer = 0 To TbGpsCompany.Length - 1
                    If Not TbGpsCompany(n) Is Nothing Then
                        GbFn.KeepLog(StrTbGpsCompany(n), TbGpsCompany(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If


            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function DeleteGpsCompany(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [gps_company] WHERE gc_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#Region "Driving License"
        Function driving_license() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table driving_license
        Public Function GetDLData() As String
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history FROM [dbo].[driving_license] dl")
        End Function

        Public Function InsertDL(ByVal driver_id As String, ByVal id_no As String, ByVal type As String _
                                         , ByVal start_date As String, ByVal expire_date As String, ByVal status As String, ByVal IdTable As String) As String

            If Not start_date Is Nothing Then
                start_date = Convert.ToDateTime(start_date).ToString("MM/dd/yyyy")
            End If

            If Not expire_date Is Nothing Then
                expire_date = Convert.ToDateTime(expire_date).ToString("MM/dd/yyyy")
            End If

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO [driving_license] ([driver_id],[id_no],[type],[start_date],[expire_date],[status]) OUTPUT Inserted.dl_id"
            _SQL &= " VALUES (N'" & IIf(driver_id Is Nothing, String.Empty, driver_id) & "',"
            _SQL &= "N'" & IIf(id_no Is Nothing, String.Empty, id_no) & "',"
            _SQL &= "N'" & IIf(type Is Nothing, String.Empty, type) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(expire_date Is Nothing, String.Empty, expire_date) & "',"
            _SQL &= "N'" & IIf(status Is Nothing, String.Empty, status) & "')"
            DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbGpsCompany() As String = {"driver_id", "id_no", "type", "start_date", "expire_date", "status"}
                Dim TbGpsCompany() As Object = {driver_id, id_no, type, start_date, expire_date, status}
                For n As Integer = 0 To TbGpsCompany.Length - 1
                    If Not TbGpsCompany(n) Is Nothing Then
                        GbFn.KeepLog(StrTbGpsCompany(n), TbGpsCompany(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If


            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateDL(ByVal driver_id As String, ByVal id_no As String, ByVal type As String _
                                , ByVal start_date As String, ByVal expire_date As String, ByVal status As String, ByVal IdTable As String, ByVal key As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [driving_license] SET "
            Dim StrTbGpsCompany() As String = {"driver_id", "id_no", "type", "start_date", "expire_date", "status"}
            Dim TbGpsCompany() As Object = {driver_id, id_no, type, start_date, expire_date, status}
            For n As Integer = 0 To TbGpsCompany.Length - 1
                If Not TbGpsCompany(n) Is Nothing Then
                    _SQL &= StrTbGpsCompany(n) & "=N'" & TbGpsCompany(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE dl_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbGpsCompany.Length - 1
                    If Not TbGpsCompany(n) Is Nothing Then
                        GbFn.KeepLog(StrTbGpsCompany(n), TbGpsCompany(n), "Editing", IdTable, key)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function DeleteDL(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [driving_license] WHERE dl_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertFileDL() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim fk_id As String = String.Empty
                Dim newFile As String = String.Empty

                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        End If
                    Next
                    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                    If Request.Files.Count <> 0 Then

                        Dim pathServer As String = Server.MapPath("~/Files/LF/" & fk_id)
                        If (Not System.IO.Directory.Exists(pathServer)) Then
                            System.IO.Directory.CreateDirectory(pathServer)
                        End If
                        Dim fileName As String = String.Empty
                        For i As Integer = 0 To Request.Files.Count - 1
                            Dim file = Request.Files(i)
                            fileName = file.FileName
                            file.SaveAs(pathServer & "/" & fileName)
                            Dim _SQL As String = "UPDATE driving_license SET path = N'../Files/LF/" & fk_id & "/" & file.FileName & "' WHERE dl_id = " & fk_id
                            objDB.ExecuteSQL(_SQL, cn)
                        Next

                        DtJson.Rows.Add("../Files/LF/" & fk_id & "/" & fileName)
                    Else
                        DtJson.Rows.Add("0")
                    End If

                    objDB.DisconnectDB(cn)

                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


#End Region

#Region "Passport"
        Function passport() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table driving_license
        Public Function GetPASData() As String
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history FROM [dbo].[passport]")
        End Function

        Public Function InsertPAS(ByVal driver_id As String, ByVal id_no As String _
                                         , ByVal start_date As String, ByVal expire_date As String, ByVal status As String, ByVal IdTable As String) As String

            If Not start_date Is Nothing Then
                start_date = Convert.ToDateTime(start_date).ToString("MM/dd/yyyy")
            End If

            If Not expire_date Is Nothing Then
                expire_date = Convert.ToDateTime(expire_date).ToString("MM/dd/yyyy")
            End If

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO [passport] ([driver_id],[id_no],[start_date],[expire_date],[status]) OUTPUT Inserted.pas_id"
            _SQL &= " VALUES (N'" & IIf(driver_id Is Nothing, String.Empty, driver_id) & "',"
            _SQL &= "N'" & IIf(id_no Is Nothing, String.Empty, id_no) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(expire_date Is Nothing, String.Empty, expire_date) & "',"
            _SQL &= "N'" & IIf(status Is Nothing, String.Empty, status) & "')"
            DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbGpsCompany() As String = {"driver_id", "id_no", "start_date", "expire_date", "status"}
                Dim TbGpsCompany() As Object = {driver_id, id_no, start_date, expire_date, status}
                For n As Integer = 0 To TbGpsCompany.Length - 1
                    If Not TbGpsCompany(n) Is Nothing Then
                        GbFn.KeepLog(StrTbGpsCompany(n), TbGpsCompany(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If


            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdatePAS(ByVal driver_id As String, ByVal id_no As String _
                                , ByVal start_date As String, ByVal expire_date As String, ByVal status As String, ByVal IdTable As String, ByVal key As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [passport] SET "
            Dim StrTbGpsCompany() As String = {"driver_id", "id_no", "start_date", "expire_date", "status"}
            Dim TbGpsCompany() As Object = {driver_id, id_no, start_date, expire_date, status}
            For n As Integer = 0 To TbGpsCompany.Length - 1
                If Not TbGpsCompany(n) Is Nothing Then
                    _SQL &= StrTbGpsCompany(n) & "=N'" & TbGpsCompany(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE pas_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbGpsCompany.Length - 1
                    If Not TbGpsCompany(n) Is Nothing Then
                        GbFn.KeepLog(StrTbGpsCompany(n), TbGpsCompany(n), "Editing", IdTable, key)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function DeletePAS(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [passport] WHERE pas_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertFilePAS() As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Try
                Dim fk_id As String = String.Empty
                Dim newFile As String = String.Empty

                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        End If
                    Next
                    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                    If Request.Files.Count <> 0 Then

                        Dim pathServer As String = Server.MapPath("~/Files/LF/" & fk_id)
                        If (Not System.IO.Directory.Exists(pathServer)) Then
                            System.IO.Directory.CreateDirectory(pathServer)
                        End If
                        Dim fileName As String = String.Empty
                        For i As Integer = 0 To Request.Files.Count - 1
                            Dim file = Request.Files(i)
                            fileName = file.FileName
                            file.SaveAs(pathServer & "/" & fileName)
                            Dim _SQL As String = "UPDATE passport SET path = N'../Files/LF/" & fk_id & "/" & file.FileName & "' WHERE pas_id = " & fk_id
                            objDB.ExecuteSQL(_SQL, cn)
                        Next

                        DtJson.Rows.Add("../Files/LF/" & fk_id & "/" & fileName)
                    Else
                        DtJson.Rows.Add("0")
                    End If

                    objDB.DisconnectDB(cn)

                Else
                    DtJson.Rows.Add("0")
                End If
            Catch ex As Exception
                DtJson.Rows.Add("0")
            End Try
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#Region "Function Tew"

        Public Function getHistory(ByVal table As String, ByVal idOfTable As String) As String
            If table = "13" Then
                table = "'13','14'"
            ElseIf table = "20" Then
                table = "'20','21'"
            End If
            Return GbFn.GetData("select Row_Number() Over ( Order By la._date desc ) As row,la._event,cc.display as column_display,CASE WHEN _format is NULL or _format like '%#,##0.##%' THEN [_data] ELSE  FORMAT(convert(datetime, [_data]),'MM/dd/yyyy') END as _data  ,ac.firstname,la._date 
                                    from log_all la inner join config_column cc on cc.column_id = la.column_id inner join account ac on ac.user_id = la.by_user 
                                    where la.id_of_table = '" & idOfTable & "' and la._table in (" & table & ") order by la._date desc
                                ")
        End Function

        'Config Column Data
        Public Sub SetDataOfConfigColumnData()
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim dt As DataTable = New DataTable
            Dim _SQL As String = "INSERT INTO [config_column_data]
                                    SELECT cc.column_id,'" & Session("UserId") & "',cc.visible
                                    FROM [dbo].[config_column] as cc  left join (select * FROM [dbo].[config_column_data] where user_id = " & Session("UserId").ToString & ") as ccd on cc.column_id = ccd.cc_id  
                                    where  ccd.user_id is null"
            If (objDB.ExecuteSQL(_SQL, cn)) Then

            Else

            End If

        End Sub

        'Get data Filses
        Public Function GetFilesTew(ByVal Id As Integer, ByVal IdTable As Integer) As String
            Return GbFn.GetFiles(Id, IdTable)
        End Function

        'Get data config column
        Public Function GetColumnChooser(ByVal gbTableId As Integer) As String
            Return GbFn.GetColumnChooser(gbTableId)
        End Function
        'Insert File (pic,pdf)
        Public Function InsertFile() As String
            Return GbFn.InsertFile(Request)
        End Function

        'Delete File (pic,pdf)
        Public Function DeleteFile(ByVal keyId As String, ByVal FolderName As String) As String
            Return GbFn.DeleteFile(keyId, FolderName)
        End Function

        Public Function GetLicenseCarTew(ByVal number_car As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT license_id,license_car FROM license WHERE number_car = '" & number_car & "'"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function SetColumnHide(ByVal dataColumnVisible As String, ByVal dataColumnHide As String) As String
            Dim ColumnHideForWhere As String = String.Empty
            Dim ColumnVisibleForWhere As String = String.Empty

            Dim ColumnVisible As String() = dataColumnVisible.Split(New Char() {"*"})
            Dim ColumnHide As String() = dataColumnHide.Split(New Char() {"*"})

            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)

            ColumnVisibleForWhere = GenSqlForUpdate_config_column_data(ColumnVisible)
            ColumnHideForWhere = GenSqlForUpdate_config_column_data(ColumnHide)
            Dim _SQL As String = "UPDATE config_column_data SET [visible] = '1' where cc_id in (" & ColumnVisibleForWhere & ") and user_id = '" + Session("UserId").ToString + "' "
            If objDB.ExecuteSQL(_SQL, cn) Then
                If (ColumnHideForWhere <> "") Then
                    _SQL = "UPDATE config_column_data SET [visible] = '0' where cc_id in (" & ColumnHideForWhere & ") and user_id = '" + Session("UserId").ToString + "' "
                    If objDB.ExecuteSQL(_SQL, cn) Then
                        Return 1
                    Else
                        Return 0
                    End If
                Else
                    Return 1
                End If

            Else
                Return 0
            End If
        End Function

        Public Function GenSqlForUpdate_config_column_data(ByVal ColumnValue As String())
            Dim SqlForUpdate As String = String.Empty
            For CountArr As Integer = 1 To ColumnValue.Length - 1
                If CountArr = ColumnValue.Length - 1 Then
                    SqlForUpdate = SqlForUpdate & "'" & ColumnValue(CountArr) & "'"
                Else
                    SqlForUpdate = SqlForUpdate & "'" & ColumnValue(CountArr) & "',"
                End If
            Next
            Return SqlForUpdate
        End Function

        Public Function group_update_(ByVal TableName As String, ByVal Status As String, ByVal NameColumnStatus As String, ByVal id As String, ByVal NameColumnId As String)
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _Sql As String = "select group_update from " & TableName & " where " & NameColumnId & " = '" & id & "'"
            Dim Dt As DataTable = objDB.SelectSQL(_Sql, cn)

            If (Not IsDBNull(Dt.Rows(0).Item("group_update"))) Then
                _Sql = "UPDATE " & TableName & " SET flag_status = 0, update_status = GETDATE()," & NameColumnStatus & " = N'" & Status & "' where group_update = '" & Dt.Rows(0).Item("group_update") & "'"
                If objDB.ExecuteSQL(_Sql, cn) Then
                    Return 1
                Else
                    Return 0
                End If
            Else
                Return 1
            End If

        End Function

        Public Function GetDriverNameJoinTable(ByVal TableName As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "  select distinct d.driver_id, d.driver_name from driver as d join " & TableName & " on d.driver_id = " & TableName & ".driver_id"
            Dim Dt As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In Dt.Rows Select Dt.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetDriverFull() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "  select distinct d.driver_id, d.driver_name from driver as d where d.driver_name is not null and d.driver_name <> ''"
            Dim Dt As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In Dt.Rows Select Dt.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region

#Region "Dashboard"

        Function dashboard() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function GetFleetCategoryTabien()
            Return GbFn.GetData("select data_fleet.fleet,COUNT(data_fleet.fleet) as qty from (
                                    select case 
                                    when li.fleet is null then 'NULL'
                                    else li.fleet
                                    end fleet
                                       from license li ) data_fleet
                                       group by data_fleet.fleet")
        End Function

        Public Function GetCarCategoryTabien()
            Return GbFn.GetData("select _data.internal_call,COUNT(_data.internal_call) as qty from (
                                    select case 
                                    when li.internal_call is null then 'NULL'
                                    else li.internal_call
                                    end internal_call
                                       from license li) _data
                                       group by _data.internal_call")
        End Function

        Public Function FilterFleetToCar(ByVal filter)


            Return GbFn.GetData("select _data.internal_call,COUNT(_data.internal_call) as qty from (
                                    select case 
                                    when li.internal_call is null then 'NULL'
                                    else li.internal_call
                                    end internal_call
                                       from license li
                                       where " & GenSqlForFleet(filter) & "
                                       ) _data
                                       group by _data.internal_call")
        End Function

        Public Function GenSqlForFleet(filter)
            Dim StrSplit As String() = filter.Split(",")
            Dim StrIn As String = ""
            Dim IsNull As String = ""
            For i As Integer = 0 To StrSplit.Length - 1
                If (StrSplit(i) = "NULL") Then
                    IsNull = "li.fleet is null"
                Else
                    If (StrIn = "") Then
                        StrIn = "li.fleet in (N'" & StrSplit(i) & "'"
                    Else
                        StrIn = StrIn & ",N'" & StrSplit(i) & "'"
                    End If
                End If
            Next
            If (StrIn <> "") Then
                StrIn = StrIn & ") "
            End If

            If (StrIn <> "") And (IsNull <> "") Then
                IsNull = " or " & IsNull
            End If

            Return "(" & StrIn & " " & IsNull & ")"
        End Function

        Public Function getDetailCarCategory(filterFleet, filterCar)
            Dim StrAnd As String = ""
            If (filterCar = "NULL") Then
                StrAnd = " and internal_call is null"
            ElseIf (filterCar = "Car Category") Then
                StrAnd = ""
            Else
                StrAnd = " and internal_call = N'" & filterCar & "'"
            End If

            Return GbFn.GetData("
                                    select li.number_car,li.license_car,li.internal_call
                                       from license li
                                       where " & GenSqlForFleet(filterFleet) & " " & StrAnd & "
                                       order by LEN(li.number_car),li.number_car")
        End Function

        Public Function GetStatusTabien()
            Return GbFn.GetData("
                                select _data2.status2 as status,count(_data2.status2) as qty from (
                                select _data.*, case 
                                    when _data.status is null then 'NULL'
                                    else _data.status
                                    end status2

                                from (SELECT  li.license_id, li.number_car, li.license_car
	                                , tax.tax_id as id_of_table, 'tax' as _table, tax.tax_status as status
	                                FROM [TT1995].[dbo].[license] li
	                                inner join tax on li.license_id = tax.license_id
	                                where  (tax.tax_status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'ตรวจ GPS', N'เสร็จสมบูรณ์') or tax.tax_status IS NULL)
                                union
                                SELECT  li.license_id, li.number_car, li.license_car
	                                , mi.mi_id as id_of_table, 'main_insurance' as _table, mi.status as status
	                                FROM [TT1995].[dbo].[license] li
	                                inner join main_insurance mi on li.license_id = mi.license_id
	                                where  (mi.status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'ตรวจ GPS', N'เสร็จสมบูรณ์') or mi.status IS NULL)
                                union
                                SELECT  li.license_id, li.number_car, li.license_car
	                                , ai.ai_id as id_of_table, 'main_insurance' as _table, ai.status as status
	                                FROM [TT1995].[dbo].[license] li
	                                inner join act_insurance ai on li.license_id = ai.license_id
	                                where  (ai.status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'ตรวจ GPS', N'เสร็จสมบูรณ์') or ai.status IS NULL)
                                union
                                SELECT  li.license_id, li.number_car, li.license_car
	                                , ai.ai_id as id_of_table, 'main_insurance' as _table, ai.status as status
	                                FROM [TT1995].[dbo].[license] li
	                                inner join act_insurance ai on li.license_id = ai.license_id
	                                where  (ai.status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'ตรวจ GPS', N'เสร็จสมบูรณ์') or ai.status IS NULL)
                                union
                                SELECT  li.license_id, li.number_car, li.license_car
	                                , dpi.dpi_id as id_of_table, 'domestic_product_insurance' as _table, dpi.status as status
	                                FROM [TT1995].[dbo].[license] li
	                                inner join domestic_product_insurance dpi on li.license_id = dpi.license_id
	                                where  (dpi.status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'ตรวจ GPS', N'เสร็จสมบูรณ์') or dpi.status IS NULL)
                                union
                                SELECT  li.license_id, li.number_car, li.license_car
	                                , ei.ei_id as id_of_table, 'environment_insurance' as _table, ei.status as status
	                                FROM [TT1995].[dbo].[license] li
	                                inner join environment_insurance ei on li.license_id = ei.license_id
	                                where  (ei.status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'ตรวจ GPS', N'เสร็จสมบูรณ์') or ei.status IS NULL)
                                union
                                SELECT  li.license_id, li.number_car, li.license_car
	                                , lv8.lv8_id as id_of_table, 'license_v8' as _table, lv8.lv8_status as status
	                                FROM [TT1995].[dbo].[license] li
	                                inner join license_v8 lv8 on li.license_id = lv8.license_id
	                                where  (lv8.lv8_status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'ตรวจ GPS', N'เสร็จสมบูรณ์') or lv8.lv8_status IS NULL)
                                union
                                SELECT '' as license_id, '' as number_car, '' as license_car,lc.lc_id as id_of_table, 'license_cambodia' as _table, lc.lc_status as status
	                                FROM  license_cambodia lc 
	                                where  (lc.lc_status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'ตรวจ GPS', N'เสร็จสมบูรณ์') or lc.lc_status IS NULL)
                                union
                                SELECT '' as license_id, '' as number_car, '' as license_car,lmr.lmr_id as id_of_table, 'license_mekong_river' as _table, lmr.lmr_status as status
	                                FROM license_mekong_river lmr 
	                                where  (lmr.lmr_status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'ตรวจ GPS', N'เสร็จสมบูรณ์') or lmr.lmr_status IS NULL)
                                union
                                SELECT '' as license_id, '' as number_car, '' as license_car,bi.business_id as id_of_table, 'business_in' as _table, bi.business_status as status
	                                FROM  business_in bi 
	                                where  (bi.business_status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'ตรวจ GPS', N'เสร็จสมบูรณ์') or bi.business_status IS NULL)
                                union
                                SELECT '' as license_id, '' as number_car, '' as license_car,bo.business_id as id_of_table, 'business_in' as _table, bo.business_status as status
	                                FROM  business_out bo 
	                                where  (bo.business_status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'ตรวจ GPS', N'เสร็จสมบูรณ์') or bo.business_status IS NULL) ) _data ) _data2
	                                group by _data2.status2
                                    ")
        End Function

        Public Function GenSqlForStatus(filter, TableSelectIn)
            Dim StrSplit As String() = filter.Split(",")
            Dim StrIn As String = ""
            Dim IsNull As String = ""
            For i As Integer = 0 To StrSplit.Length - 1
                If (StrSplit(i) = "NULL") Then
                    IsNull = TableSelectIn & " is null"
                Else
                    If (StrIn = "") Then
                        StrIn = TableSelectIn & " in (N'" & StrSplit(i) & "'"
                    Else
                        StrIn = StrIn & ",N'" & StrSplit(i) & "'"
                    End If
                End If
            Next
            If (StrIn <> "") Then
                StrIn = StrIn & ") "
            End If

            If (StrIn <> "") And (IsNull <> "") Then
                IsNull = " or " & IsNull
            End If

            Return "(" & StrIn & " " & IsNull & ")"
        End Function

        Public Function GetStackedBarTabien(filter)
            Return GbFn.GetData("
                                   	select  _data.month_expired,
		sum(_data.tax_qty) as tax_qty,  
		sum(_data.ai_qty) as ai_qty,
		sum(_data.mi_qty) as mi_qty ,
		FORMAT(sum(_data.price), 'C', 'th-TH') as price
	from(
		SELECT  DATENAME(month, tax.tax_expire) as month_expired, COUNT(MONTH(tax.tax_expire)) as tax_qty, 0 as ai_qty, 0 as mi_qty,sum(tax.tax_rate) as price
		  FROM [TT1995].[dbo].[tax] tax 
		  where (" & GenSqlForStatus(filter, "tax.tax_status") & ") 
		  group by tax.tax_status, DATENAME(month, tax.tax_expire)
		Union
		SELECT  DATENAME(month, ai.end_date) as month_expired, 0 as tax_qty, COUNT(MONTH(ai.end_date)) as ai_qty, 0 as mi_qty,sum(ai.price) as price
		  FROM [TT1995].[dbo].[act_insurance] ai
		  where (" & GenSqlForStatus(filter, "ai.status") & ") 
		  group by ai.status, DATENAME(month, ai.end_date)
		Union
		SELECT  DATENAME(month, mi.end_date) as month_expired, 0 as tax_qty, 0 as ai_qty, COUNT(MONTH(mi.end_date)) as mi_qty,sum(mi.current_cowrie) as price
		  FROM [TT1995].[dbo].[main_insurance] mi
		  where (" & GenSqlForStatus(filter, "mi.status") & ") 
		  group by mi.status, DATENAME(month, mi.end_date)
		
	) _data 
	group by _data.month_expired
	order by case month_expired   when 'January' then 1
												   when 'February' then 2
												   when 'March' then 3
												   when 'April' then 4
												   when 'May' then 5
												   when 'June' then 6
												   when 'July' then 7
												   when 'August' then 8
												   when 'September' then 9
												   when 'October' then 10
												   when 'November' then 11
												   when 'December' then 12
								 end
                                       ")

        End Function

        Public Function GetStackedBarOther(filter)
            Return GbFn.GetData("
                                    select  _data.month_expired,
		                                sum(_data.dpi_qty) as dpi_qty,  
		                                sum(_data.ei_qty) as ei_qty,
		                                sum(_data.lv8_qty) as lv8_qty ,
		                                sum(_data.lc_qty) as lc_qty ,
		                                sum(_data.lmr_qty) as lmr_qty ,
		                                sum(_data.bi_qty) as bi_qty ,
		                                sum(_data.bo_qty) as bo_qty ,
		                                FORMAT(sum(_data.price), 'C', 'th-TH') as price
	                                from(
		                                SELECT  DATENAME(month, dpi.end_date) as month_expired, COUNT(MONTH(dpi.end_date)) as dpi_qty, 0 as ei_qty, 0 as lv8_qty, 0 as lc_qty, 0 as lmr_qty, 0 as bi_qty, 0 as bo_qty
			                                ,sum(dpi.current_cowrie) as price
		                                  FROM [TT1995].[dbo].[domestic_product_insurance] dpi 
		                                  where (" & GenSqlForStatus(filter, "dpi.status") & ") 
		                                  group by dpi.status, DATENAME(month, dpi.end_date)
		                                Union
		                                SELECT  DATENAME(month, ei.end_date) as month_expired,0 as dpi_qty, COUNT(MONTH(ei.end_date)) as ei_qty, 0 as lv8_qty, 0 as lc_qty, 0 as lmr_qty, 0 as bi_qty, 0 as bo_qty
			                                ,sum(ei.current_cowrie) as price
		                                  FROM [TT1995].[dbo].[environment_insurance] ei
		                                  where (" & GenSqlForStatus(filter, "ei.status") & ") 
		                                  group by ei.status, DATENAME(month, ei.end_date)
		                                Union
		                                SELECT  DATENAME(month, lv8.lv8_expire) as month_expired,0 as dpi_qty, 0 as ei_qty, COUNT(MONTH(lv8.lv8_expire)) as lv8_qty, 0 as lc_qty, 0 as lmr_qty, 0 as bi_qty, 0 as bo_qty
			                                ,'0' as price
		                                  FROM [TT1995].[dbo].[license_v8] lv8 
		                                  where  (" & GenSqlForStatus(filter, "lv8.lv8_status") & ") 
		                                  group by lv8.lv8_status, DATENAME(month, lv8.lv8_expire)
		                                Union
		                                SELECT  DATENAME(month, lc.lc_expire) as month_expired,0 as dpi_qty, 0 as ei_qty, 0 as lv8_qty, COUNT(lc.lc_expire) as lc_qty, 0 as lmr_qty, 0 as bi_qty, 0 as bo_qty
			                                ,'0' as price
		                                  FROM [TT1995].[dbo].[license_cambodia] lc
		                                  where  (" & GenSqlForStatus(filter, "lc.lc_status") & ") 
		                                  group by lc.lc_status, DATENAME(month, lc.lc_expire)
		                                Union
		                                SELECT  DATENAME(month, lmr.lmr_expire) as month_expired,0 as dpi_qty, 0 as ei_qty, 0 as lv8_qty, 0 as lc_qty, COUNT(lmr.lmr_expire) as lmr_qty, 0 as bi_qty, 0 as bo_qty
			                                ,'0' as price
		                                  FROM [TT1995].[dbo].[license_mekong_river] lmr
		                                  where  (" & GenSqlForStatus(filter, "lmr.lmr_status") & ") 
		                                  group by lmr.lmr_status, DATENAME(month, lmr.lmr_expire)
		                                Union
		                                SELECT  DATENAME(month, bi.business_expire) as month_expired,0 as dpi_qty, 0 as ei_qty, 0 as lv8_qty, 0 as lc_qty, 0 as lmr_qty, COUNT(bi.business_expire) as bi_qty, 0 as bo_qty
			                                ,'0' as price
		                                  FROM [TT1995].[dbo].[business_in] bi
		                                  where  (" & GenSqlForStatus(filter, "bi.business_status") & ") 
		                                  group by bi.business_status, DATENAME(month, bi.business_expire)
		                                Union
		                                SELECT  DATENAME(month, bo.business_expire) as month_expired,0 as dpi_qty, 0 as ei_qty, 0 as lv8_qty, 0 as lc_qty, 0 as lmr_qty, 0 as bi_qty, COUNT(bo.business_expire) as bo_qty
			                                ,'0' as price
		                                  FROM [TT1995].[dbo].[business_out] bo
		                                  where  (" & GenSqlForStatus(filter, "bo.business_status") & ") 
		                                  group by bo.business_status, DATENAME(month, bo.business_expire)
		
	                                ) _data 
	                                group by _data.month_expired
	                                order by case month_expired   when 'January' then 1
												                                   when 'February' then 2
												                                   when 'March' then 3
												                                   when 'April' then 4
												                                   when 'May' then 5
												                                   when 'June' then 6
												                                   when 'July' then 7
												                                   when 'August' then 8
												                                   when 'September' then 9
												                                   when 'October' then 10
												                                   when 'November' then 11
												                                   when 'December' then 12
								                                 end
                    ")
        End Function

        Public Function GetStackedBarEnterFactory(filter)
            Return GbFn.GetData("
                                    	select  _data.month_expired, 
		sum(_data.lf_qty) as lf_qty,  
		sum(_data.lcf_qty) as lcf_qty,
		FORMAT(sum(_data.price), 'C', 'th-TH') as price
	from(
		SELECT DATENAME(month, lf.expire_date) as month_expired, COUNT(MONTH(lf.expire_date)) as lf_qty, 0 as lcf_qty, 0 as price
		  FROM [TT1995].[dbo].[license_factory] lf 
		  where (" & GenSqlForStatus(filter, "lf.license_factory_status") & ") 
		  group by lf.license_factory_status, DATENAME(month, lf.expire_date)
		Union
		SELECT  DATENAME(month, lcf.expire_date) as month_expired, COUNT(MONTH(lcf.expire_date)) as lf_qty, 0 as lcf_qty, 0 as price
		  FROM [TT1995].[dbo].[license_car_factory] lcf
		  where (" & GenSqlForStatus(filter, "lcf.status") & ") 
		  group by lcf.status, DATENAME(month, lcf.expire_date)
	) _data 
	group by _data.month_expired
	order by case month_expired   when 'January' then 1
												   when 'February' then 2
												   when 'March' then 3
												   when 'April' then 4
												   when 'May' then 5
												   when 'June' then 6
												   when 'July' then 7
												   when 'August' then 8
												   when 'September' then 9
												   when 'October' then 10
												   when 'November' then 11
												   when 'December' then 12
								 end
                                ")
        End Function

        Public Function GetFleetCategoryDriver()
            Return GbFn.GetData("select data_fleet.fleet,COUNT(data_fleet.fleet) as qty from (
                                    select case 
                                    when driver.fleet is null then 'NULL'
                                    else driver.fleet
                                    end fleet
                                       from driver ) data_fleet
                                       group by data_fleet.fleet")
        End Function

        Public Function GetDlCategoryDriver()
            Return GbFn.GetData("
                    select _data.table_name,COUNT(_data.table_name) as qty from (
SELECT dl.id_no as id_no,driver.driver_name,driver.driver_name2,N'ใบอนุญาตขับขี่' as table_name
  FROM [TT1995].[dbo].driving_license dl
  inner join driver on driver.driver_id = dl.driver_id
union
SELECT dldot.id_no as id_no,driver.driver_name,driver.driver_name2,N'ใบอนุญาตขับขี่ขนส่งวัตถุอันตราย' as table_name
  FROM [TT1995].[dbo].driving_license_dangerous_objects_transportation dldot
  inner join driver on driver.driver_id = dldot.driver_id
union
SELECT dlngt.id_no as id_no,driver.driver_name,driver.driver_name2,N'ใบอนุญาตขับขี่ขนส่งก๊าสธรรมชาติ' as table_name
  FROM [TT1995].[dbo].driving_license_natural_gas_transportation dlngt
  inner join driver on driver.driver_id = dlngt.driver_id
union
SELECT dlot.id_no as id_no,driver.driver_name,driver.driver_name2,N'ใบอนุญาตขับขี่ขนส่งน้ำมัน' as table_name
  FROM [TT1995].[dbo].driving_license_oil_transportation dlot
  inner join driver on driver.driver_id = dlot.driver_id
union
SELECT passport.id_no as id_no,driver.driver_name,driver.driver_name2,N'พาสสปอร์ต' as table_name
  FROM [TT1995].[dbo].passport
  inner join driver on driver.driver_id = passport.driver_id
  ) _data
                                       group by _data.table_name


                ")
        End Function

        Public Function GetStatusCategoryDriver()
            Return GbFn.GetData("
                select _data2.status2 as status,count(_data2.status2) as qty from (
                                select _data.*, case 
                                    when _data.status is null then 'NULL'
                                    else _data.status
                                    end status2

                                from (
SELECT  dl.dl_id as id_of_table, 'driving_license' as _table, dl.status as status
	    FROM [TT1995].[dbo].driving_license dl
	    where  (dl.status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'เสร็จสมบูรณ์') or dl.status IS NULL)
union
SELECT  dldot.dldot_id as id_of_table, 'driving_license_dangerous_objects_transportation' as _table, dldot.status as status
	    FROM [TT1995].[dbo].driving_license_dangerous_objects_transportation dldot
	    where  (dldot.status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'เสร็จสมบูรณ์') or dldot.status IS NULL)
union
SELECT  dlngt.dlngt_id as id_of_table, 'driving_license_natural_gas_transportation' as _table, dlngt.status as status
	    FROM [TT1995].[dbo].driving_license_natural_gas_transportation dlngt
	    where  (dlngt.status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'เสร็จสมบูรณ์') or dlngt.status IS NULL)
union
SELECT  dlot.dlot_id as id_of_table, 'driving_license_oil_transportation' as _table, dlot.status as status
	    FROM [TT1995].[dbo].driving_license_oil_transportation dlot
	    where  (dlot.status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'เสร็จสมบูรณ์') or dlot.status IS NULL)
union
SELECT  passport.pas_id as id_of_table, 'passport' as _table, passport.status as status
	    FROM [TT1995].[dbo].passport
	    where  (passport.status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'เสร็จสมบูรณ์') or passport.status IS NULL)
union
SELECT  lf.license_factory_id as id_of_table, 'license_factory' as _table, lf.license_factory_status as status
	    FROM [TT1995].[dbo].license_factory lf
	    where  (lf.license_factory_status in (N'ยังไม่ได้ดำเนินการ', N'ขาดต่อ', N'จัดเตรียมเอกสาร', N'ยื่นเอกสาร', N'เสร็จสมบูรณ์') or lf.license_factory_status IS NULL)

		) _data ) _data2
	                                group by _data2.status2
            ")
        End Function

        Public Function GetStackedBarDlDriver()
            Return GbFn.GetData("
                select  _data.month_expired, 
		sum(_data.dl_qty) as dl_qty,  
		sum(_data.dldot_qty) as dldot_qty,  
		sum(_data.dlngt_qty) as dlngt_qty,  
		sum(_data.dlot_qty) as dlot_qty,  
		sum(_data.p_qty) as p_qty,  
		sum(_data.lf_qty) as lf_qty,
		FORMAT(sum(_data.price), 'C', 'th-TH') as price
	from(
SELECT  DATENAME(month, dl.expire_date) as month_expired, COUNT(MONTH(dl.expire_date)) as dl_qty,
 0 as dldot_qty, 0 as dlngt_qty, 0 as dlot_qty, 0 as p_qty, 0 as lf_qty,0 as price
		  FROM [TT1995].[dbo].driving_license dl 
		  where (dl.status in (N'ยังไม่ได้ดำเนินการ',N'จัดเตรียมเอกสาร',N'ยื่นเอกสาร',N'เสร็จสมบูรณ์',N'ขาดต่อ')) 
		  group by dl.status, DATENAME(month, dl.expire_date)
union
SELECT  DATENAME(month, dldot.expire_date) as month_expired, 0 as dl_qty,
 COUNT(MONTH(dldot.expire_date)) as dldot_qty, 0 as dlngt_qty, 0 as dlot_qty, 0 as p_qty, 0 as lf_qty,0 as price
		  FROM [TT1995].[dbo].driving_license_dangerous_objects_transportation dldot 
		  where (dldot.status in (N'ยังไม่ได้ดำเนินการ',N'จัดเตรียมเอกสาร',N'ยื่นเอกสาร',N'เสร็จสมบูรณ์',N'ขาดต่อ')) 
		  group by dldot.status, DATENAME(month, dldot.expire_date)
union
SELECT  DATENAME(month, dlngt.expire_date) as month_expired, 0 as dl_qty,
 0 as dldot_qty, COUNT(MONTH(dlngt.expire_date)) as dlngt_qty, 0 as dlot_qty, 0 as p_qty, 0 as lf_qty,0 as price
		  FROM [TT1995].[dbo].driving_license_natural_gas_transportation dlngt
		  where (dlngt.status in (N'ยังไม่ได้ดำเนินการ',N'จัดเตรียมเอกสาร',N'ยื่นเอกสาร',N'เสร็จสมบูรณ์',N'ขาดต่อ')) 
		  group by dlngt.status, DATENAME(month, dlngt.expire_date)
union
SELECT  DATENAME(month, dldot.expire_date) as month_expired, 0 as dl_qty,
 0 as dldot_qty, 0 as dlngt_qty, COUNT(MONTH(dldot.expire_date)) as dlot_qty, 0 as p_qty, 0 as lf_qty,0 as price
		  FROM [TT1995].[dbo].driving_license_oil_transportation dldot
		  where (dldot.status in (N'ยังไม่ได้ดำเนินการ',N'จัดเตรียมเอกสาร',N'ยื่นเอกสาร',N'เสร็จสมบูรณ์',N'ขาดต่อ')) 
		  group by dldot.status, DATENAME(month, dldot.expire_date)
union
SELECT  DATENAME(month, p.expire_date) as month_expired, 0 as dl_qty,
 0 as dldot_qty, 0 as dlngt_qty, 0 as dlot_qty, COUNT(MONTH(p.expire_date)) as p_qty, 0 as lf_qty,0 as price
		  FROM [TT1995].[dbo].passport p
		  where (p.status in (N'ยังไม่ได้ดำเนินการ',N'จัดเตรียมเอกสาร',N'ยื่นเอกสาร',N'เสร็จสมบูรณ์',N'ขาดต่อ')) 
		  group by p.status, DATENAME(month, p.expire_date)
union
SELECT  DATENAME(month, lf.expire_date) as month_expired,0 as dl_qty,
 0 as dldot_qty, 0 as dlngt_qty, 0 as dlot_qty, 0 as p_qty,  COUNT(MONTH(lf.expire_date)) as lf_qty, 0 as price
		  FROM [TT1995].[dbo].license_factory lf 
		  where (lf.license_factory_status in (N'ยังไม่ได้ดำเนินการ',N'จัดเตรียมเอกสาร',N'ยื่นเอกสาร',N'เสร็จสมบูรณ์',N'ขาดต่อ')) 
		  group by lf.license_factory_status, DATENAME(month, lf.expire_date)
			) _data 
	group by _data.month_expired
	order by case month_expired   when 'January' then 1
									when 'February' then 2
									when 'March' then 3
									when 'April' then 4
									when 'May' then 5
									when 'June' then 6
									when 'July' then 7
									when 'August' then 8
									when 'September' then 9
									when 'October' then 10
									when 'November' then 11
									when 'December' then 12
					end
            ")

        End Function

#End Region

#End Region

#Region "Develop By Poom"
        'Global Function Poom (Copy Tew)
        Dim GbFnPoom As GlobalFunction = New GlobalFunction
#Region "Expressway"


        Function Expressway() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View("../Home/Expressway")
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table Expressway
        Public Function GetExpresswayData() As String
            Return GbFnPoom.GetData("SELECT *,N'ประวัติ' as history FROM [dbo].[expressway] epw , [dbo].[license] li where epw.license_id = li.license_id order by LEN(li.number_car),li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameExpressway() As String
            Return GbFnPoom.fnRename(Request, "Expressway")
        End Function

        Public Function UpdateExpressway(ByVal number_car As String, ByVal start_date As String, ByVal expire_date As String _
                                      , ByVal processing_fee As String, ByVal special_money As String, ByVal epw_license As String _
                                      , ByVal note As String, ByVal key As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [expressway] SET "
            Dim StrTbExpressway() As String = {"number_car", "start_date", "expire_date", "processing_fee", "special_money", "epw_license", "note"}
            Dim TbExpressway() As Object = {number_car, start_date, expire_date, processing_fee, special_money, epw_license, note}
            For n As Integer = 0 To TbExpressway.Length - 1
                If Not TbExpressway(n) Is Nothing Then
                    _SQL &= StrTbExpressway(n) & "=N'" & TbExpressway(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE epw_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbExpressway.Length - 1
                    If Not TbExpressway(n) Is Nothing Then
                        GbFn.KeepLog(StrTbExpressway(n), TbExpressway(n), "Editing", IdTable, key)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertExpressway(ByVal number_car As String, ByVal start_date As String, ByVal expire_date As String _
                                      , ByVal processing_fee As String, ByVal special_money As String, ByVal epw_license As String _
                                      , ByVal note As String, ByVal key As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [expressway] ([license_id],[number_car],[start_date],[expire_date],[processing_fee],[special_money],[epw_license],[note],[create_date],[create_by_user_id]) OUTPUT Inserted.epw_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(number_car Is Nothing, 0, number_car) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(expire_date Is Nothing, String.Empty, expire_date) & "',"
            _SQL &= "N'" & IIf(processing_fee Is Nothing, 0, processing_fee) & "',"
            _SQL &= "N'" & IIf(special_money Is Nothing, 0, special_money) & "',"
            _SQL &= "N'" & IIf(epw_license Is Nothing, 0, epw_license) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            If Not number_car Is Nothing Then
                'If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                'DtJson.Rows.Add("1")
                'Else
                '    DtJson.Rows.Add("0")
                'End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbExpressway() As String = {"number_car", "start_date", "expire_date", "processing_fee", "special_money", "epw_license", "note"}
                Dim TbExpressway() As Object = {number_car, start_date, expire_date, processing_fee, special_money, epw_license, note}
                For n As Integer = 0 To TbExpressway.Length - 1
                    If Not TbExpressway(n) Is Nothing Then
                        GbFn.KeepLog(StrTbExpressway(n), TbExpressway(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


        Public Function DeleteExpressway(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [expressway] WHERE epw_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region 'Expressway

#Region "Gps_car"
        Function Gps_car() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View("../Home/Gps_car")
            Else
                Return View("../Account/Login")
            End If
        End Function

        'Get data of table Gps_car
        Public Function GetGps_carData() As String
            Return GbFnPoom.GetData("SELECT *,N'ประวัติ' as history,N'View' as Installation_list_view  FROM [dbo].[gps_car] gps_car , [dbo].[license] li where gps_car.license_id = li.license_id order by LEN(li.number_car),li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameGps_car() As String
            Return GbFnPoom.fnRename(Request, "Gps_car")
        End Function

        Public Function UpdateGps_car(ByVal number_car As String, ByVal company As String, ByVal sim_no As String, ByVal start_date As String, ByVal end_date As String, ByVal reading_data As String _
                                      , ByVal usage As String, ByVal price As String, ByVal number_book As String, ByVal type As String, ByVal model As String, ByVal number_serial As String, ByVal Installation_list As String _
                                      , ByVal note As String, ByVal key As String, ByVal IdTable As String) As String
            If Not start_date Is Nothing Then
                start_date = Convert.ToDateTime(start_date).ToString("MM/dd/yyyy")
            End If

            If Not end_date Is Nothing Then
                end_date = Convert.ToDateTime(end_date).ToString("MM/dd/yyyy")
            End If

            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [gps_car] SET "
            Dim StrTbGps_car() As String = {"company", "sim_no", "start_date", "end_date", "reading_data", "usage", "price", "number_book", "type", "model", "number_serial", "Installation_list", "note"}
            Dim TbGps_car() As Object = {company, sim_no, start_date, end_date, reading_data, usage, price, number_book, type, model, number_serial, Installation_list, note}
            For n As Integer = 0 To TbGps_car.Length - 1
                If Not TbGps_car(n) Is Nothing Then
                    _SQL &= StrTbGps_car(n) & "=N'" & TbGps_car(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE [gps_car_id] = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbGps_car.Length - 1
                    If Not TbGps_car(n) Is Nothing Then
                        GbFn.KeepLog(StrTbGps_car(n), TbGps_car(n), "Editing", IdTable, key)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertGps_car(ByVal number_car As String, ByVal company As String, ByVal sim_no As String, ByVal start_date As String, ByVal end_date As String, ByVal reading_data As String _
                                      , ByVal usage As String, ByVal price As String, ByVal number_book As String, ByVal type As String, ByVal model As String, ByVal number_serial As String, ByVal Installation_list As String _
                                      , ByVal note As String, ByVal key As String, ByVal IdTable As String) As String
            If Not start_date Is Nothing Then
                start_date = Convert.ToDateTime(start_date).ToString("MM/dd/yyyy")
            End If

            If Not end_date Is Nothing Then
                end_date = Convert.ToDateTime(end_date).ToString("MM/dd/yyyy")
            End If

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [gps_car] ([license_id],[company],[sim_no],[start_date],[end_date],[reading_data],[usage],[price],[number_book],[type],[model],[number_serial],[Installation_list],[note],[create_date],[create_by_user_id])  OUTPUT Inserted.gps_car_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(company Is Nothing, 0, company) & "',"
            _SQL &= "N'" & IIf(sim_no Is Nothing, 0, sim_no) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(end_date Is Nothing, String.Empty, end_date) & "',"
            _SQL &= "N'" & IIf(reading_data Is Nothing, 0, reading_data) & "',"
            _SQL &= "N'" & IIf(usage Is Nothing, 0, usage) & "',"
            _SQL &= "N'" & IIf(price Is Nothing, 0, price) & "',"
            _SQL &= "N'" & IIf(number_book Is Nothing, 0, number_book) & "',"
            _SQL &= "N'" & IIf(type Is Nothing, 0, type) & "',"
            _SQL &= "N'" & IIf(model Is Nothing, 0, model) & "',"
            _SQL &= "N'" & IIf(number_serial Is Nothing, 0, number_serial) & "',"
            _SQL &= "N'" & IIf(Installation_list Is Nothing, 0, Installation_list) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            If Not number_car Is Nothing Then
                'If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                'DtJson.Rows.Add("1")
                'Else
                '    DtJson.Rows.Add("0")
                'End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbGps_car() As String = {"company", "sim_no", "start_date", "end_date", "reading_data", "usage", "price", "number_book", "type", "model", "number_serial", "Installation_list", "note"}
                Dim TbGps_car() As Object = {company, sim_no, start_date, end_date, reading_data, usage, price, number_book, type, model, number_serial, Installation_list, note}
                For n As Integer = 0 To TbGps_car.Length - 1
                    If Not TbGps_car(n) Is Nothing Then
                        GbFn.KeepLog(StrTbGps_car(n), TbGps_car(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


        Public Function DeleteGps_car(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [gps_car] WHERE [gps_car_id] = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function updateInstallation_list(ByVal gps_car_id As String, ByVal data As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "UPDATE [dbo].[gps_car] SET [Installation_list] = N'" & data & "' WHERE gps_car_id = '" & gps_car_id & "'"
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                GbFn.KeepLog("Installation_list", data, "Editing", IdTable, gps_car_id)
            Else
                DtJson.Rows.Add("0")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region 'Gps_car


#Region "Installment"
        Function Installment() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View("../Home/Installment")
            Else
                Return View("../Account/Login")
            End If
        End Function
        'Get data of table Installment
        Public Function GetInstallmentData() As String
            Return GbFnPoom.GetData("SELECT *,N'ประวัติ' as history FROM [dbo].[installment] itm , [dbo].[license] li where itm.license_id = li.license_id order by LEN(li.number_car),li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameInstallment() As String
            Return GbFnPoom.fnRename(Request, "Installment")
        End Function

        Public Function UpdateInstallment(ByVal number_car As String, ByVal itm_name As String, ByVal start_date As String _
                                      , ByVal no_of_itm As String, ByVal payment_of_itm As String, ByVal last_date As String _
                                      , ByVal payed_of_itm As String, ByVal last_payment As String, ByVal postponement_itm As String _
                                      , ByVal note As String, ByVal key As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [installment] SET "
            Dim StrTbInstallment() As String = {"number_car", "itm_name", "start_date", "no_of_itm", "payment_of_itm", "last_date", "payed_of_itm", "last_payment", "postponement_itm", "note"}
            Dim TbInstallment() As Object = {number_car, itm_name, start_date, no_of_itm, payment_of_itm, last_date, payed_of_itm, last_payment, postponement_itm, note}
            For n As Integer = 0 To TbInstallment.Length - 1
                If Not TbInstallment(n) Is Nothing Then
                    _SQL &= StrTbInstallment(n) & "=N'" & TbInstallment(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE [itm_id] = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
                For n As Integer = 0 To TbInstallment.Length - 1
                    If Not TbInstallment(n) Is Nothing Then
                        GbFn.KeepLog(StrTbInstallment(n), TbInstallment(n), "Editing", IdTable, key)
                    End If
                Next
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertInstallment(ByVal number_car As String, ByVal itm_name As String, ByVal start_date As String _
                                      , ByVal no_of_itm As String, ByVal payment_of_itm As String, ByVal last_date As String _
                                      , ByVal payed_of_itm As String, ByVal last_payment As String, ByVal postponement_itm As String _
                                      , ByVal note As String, ByVal key As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [installment] ([license_id],[number_car],[itm_name],[start_date],[no_of_itm],[payment_of_itm],[last_date],[payed_of_itm],[last_payment],[postponement_itm],[note],[create_date],[create_by_user_id])  OUTPUT Inserted.itm_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(number_car Is Nothing, 0, number_car) & "',"
            _SQL &= "N'" & IIf(itm_name Is Nothing, String.Empty, itm_name) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(no_of_itm Is Nothing, 0, no_of_itm) & "',"
            _SQL &= "N'" & IIf(payment_of_itm Is Nothing, 0, payment_of_itm) & "',"
            _SQL &= "N'" & IIf(last_date Is Nothing, String.Empty, last_date) & "',"
            _SQL &= "N'" & IIf(payed_of_itm Is Nothing, 0, payed_of_itm) & "',"
            _SQL &= "N'" & IIf(last_payment Is Nothing, 0, last_payment) & "',"
            _SQL &= "N'" & IIf(postponement_itm Is Nothing, String.Empty, postponement_itm) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            If Not number_car Is Nothing Then
                'If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                'DtJson.Rows.Add("1")
                'Else
                '    DtJson.Rows.Add("0")
                'End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            If DtJson.Rows(0).Item("Status").ToString <> "0" Then
                Dim StrTbinstallment() As String = {"number_car", "itm_name", "start_date", "no_of_itm", "payment_of_itm", "last_date", "payed_of_itm", "last_payment", "postponement_itm", "note"}
                Dim Tbinstallment() As Object = {number_car, itm_name, start_date, no_of_itm, payment_of_itm, last_date, payed_of_itm, last_payment, postponement_itm, note}
                For n As Integer = 0 To Tbinstallment.Length - 1
                    If Not Tbinstallment(n) Is Nothing Then
                        GbFn.KeepLog(StrTbinstallment(n), Tbinstallment(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                    End If
                Next
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))

        End Function


        Public Function DeleteInstallment(ByVal keyId As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [installment] WHERE [itm_id] = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


#End Region 'Installment

#Region "Trackingwork"
        Function Trackingwork() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View("../Home/Trackingwork")
            Else
                Return View("../Account/Login")
            End If
        End Function
        'Get data of table Trackingwork
        Public Function GetTrackingworkData() As String
            'Return GbFnPoom.GetData("SELECT *,N'ประวัติ' as history   FROM [dbo].[trackingwork] tkw , [dbo].[license] li where tkw.license_id = li.license_id order by li.number_car")
            Dim SQL As String = "select l.license_id as number_car,l.license_id as license_car, t.tax_expire as date_expire , N'ภาษี' as tablename,
                    CASE t.tax_status 
                      WHEN N'ยังไม่ได้ดำเนินการ' THEN N'ไม่มี' 
                      WHEN N'ขาดต่อ' THEN N'ไม่มี' 
                      ELSE (select top 1 ac.firstname from log_all la inner join account ac on la.by_user = ac.user_id  where column_id = 46 and _data = t.tax_status  order by la_id desc)
                    END as operator ,
                    t.tax_expire as start_schedule,dateadd(dd,90,t.tax_expire) as deadline,t.note as note ,isnull(t.tax_status, '') as data_status 
                     from license as l 
	                    join tax as t on l.license_id = t.license_id 
		                    where (t.tax_status <> null or t.tax_status <> '')

                    union select l.license_id as number_car,l.license_id as license_car, bi.business_expire as date_expire , N'ใบประกอบการภายในประเทศ' as tablename,
                    CASE bi.business_status 
                      WHEN N'ยังไม่ได้ดำเนินการ' THEN N'ไม่มี' 
                      WHEN N'ขาดต่อ' THEN N'ไม่มี' 
                      ELSE (select top 1 ac.firstname from log_all la inner join account ac on la.by_user = ac.user_id  where column_id = 46 and _data = bi.business_status  order by la_id desc)
                    END as operator ,
                    bi.business_expire as start_schedule,dateadd(dd,90,bi.business_expire) as deadline,bi.note as note ,isnull(bi.business_status, '') as data_status 
                     from license as l 
	                    join business_in_permission as bip on l.license_id = bip.license_id 
	                    join business_in as bi on bip.business_id = bi.business_id 
		                    where (bi.business_status <> null or bi.business_status <> '')

                    union select l.license_id as number_car,l.license_id as license_car, bo.business_expire as date_expire , N'ใบประกอบการภายนอกประเทศ' as tablename,
                    CASE bo.business_status 
                      WHEN N'ยังไม่ได้ดำเนินการ' THEN N'ไม่มี' 
                      WHEN N'ขาดต่อ' THEN N'ไม่มี' 
                      ELSE (select top 1 ac.firstname from log_all la inner join account ac on la.by_user = ac.user_id  where column_id = 46 and _data = bo.business_status  order by la_id desc)
                    END as operator ,
                    bo.business_expire as start_schedule,dateadd(dd,90,bo.business_expire) as deadline,bo.note as note ,isnull(bo.business_status, '') as data_status 
                     from license as l 
	                    join business_out_permission as bop on l.license_id = bop.license_id 
	                    join business_out as bo on bop.business_id = bo.business_id 
		                    where (bo.business_status <> null or bo.business_status <> '')

                    union select l.license_id as number_car,l.license_id as license_car, lc.lc_expire as date_expire , N'ใบอนุญาตกัมพูชา' as tablename,
                    CASE lc.lc_status 
                      WHEN N'ยังไม่ได้ดำเนินการ' THEN N'ไม่มี' 
                      WHEN N'ขาดต่อ' THEN N'ไม่มี' 
                      ELSE (select top 1 ac.firstname from log_all la inner join account ac on la.by_user = ac.user_id  where column_id = 46 and _data = lc.lc_status  order by la_id desc)
                    END as operator ,
                    lc.lc_expire as start_schedule,dateadd(dd,90,lc.lc_expire) as deadline,lc.note as note ,isnull(lc.lc_status, '') as data_status 
                     from license as l 
	                    join license_cambodia_permission as lcp on l.license_id = lcp.license_id_head 
		                    join license_cambodia as lc on lcp.lc_id = lc.lc_id 
			                    where (lc.lc_status <> null or lc.lc_status <> '')

                    union select l.license_id as number_car,l.license_id as license_car, lmr.lmr_expire as date_expire , N'ใบอนุญาตลุ่มแม่น้ำโขง' as tablename,
                    CASE lmr.lmr_status 
                      WHEN N'ยังไม่ได้ดำเนินการ' THEN N'ไม่มี' 
                      WHEN N'ขาดต่อ' THEN N'ไม่มี' 
                      ELSE (select top 1 ac.firstname from log_all la inner join account ac on la.by_user = ac.user_id  where column_id = 46 and _data = lmr.lmr_status  order by la_id desc)
                    END as operator ,
                    lmr.lmr_expire as start_schedule,dateadd(dd,90,lmr.lmr_expire) as deadline,lmr.note as note ,isnull(lmr.lmr_status, '') as data_status 
                     from license as l 
	                    join license_mekong_river_permission as lmrp on l.license_id = lmrp.license_id_head 
	                    join license_mekong_river as lmr on lmrp.lmr_id = lmr.lmr_id 
		                    where (lmr.lmr_status <> null or lmr.lmr_status <> '')

                    union select l.license_id as number_car,l.license_id as license_car,  lv8.lv8_expire as date_expire , N'ใบอนุญาต(วอ.8)' as tablename,
                    CASE lv8.lv8_status 
                      WHEN N'ยังไม่ได้ดำเนินการ' THEN N'ไม่มี' 
                      WHEN N'ขาดต่อ' THEN N'ไม่มี' 
                      ELSE (select top 1 ac.firstname from log_all la inner join account ac on la.by_user = ac.user_id  where column_id = 46 and _data = lv8.lv8_status  order by la_id desc)
                    END as operator ,
                     lv8.lv8_expire as start_schedule,dateadd(dd,90, lv8.lv8_expire) as deadline,lv8.note as note ,isnull(lv8.lv8_status, '') as data_status 
                     from license as l 
	                    join license_v8 as lv8 on l.license_id = lv8.license_id 
		                    where(lv8.lv8_status <> null or lv8.lv8_status <> '')

                    union select l.license_id as number_car,l.license_id as license_car,  ai.end_date as date_expire , N'ประกันพรบ.' as tablename,
                    CASE ai.status 
                      WHEN N'ยังไม่ได้ดำเนินการ' THEN N'ไม่มี' 
                      WHEN N'ขาดต่อ' THEN N'ไม่มี' 
                      ELSE (select top 1 ac.firstname from log_all la inner join account ac on la.by_user = ac.user_id  where column_id = 46 and _data = ai.status order by la_id desc)
                    END as operator ,
                     ai.end_date as start_schedule,dateadd(dd,90, ai.end_date) as deadline,ai.note as note ,isnull(ai.status, '') as data_status 
                     from license as l 
	                    join act_insurance as ai on l.license_id = ai.license_id 
		                    where (ai.status <> null or ai.status <> '')

                    union select l.license_id as number_car,l.license_id as license_car,  mi.end_date as date_expire , N'ประกันภัยรถยนต์.' as tablename,
                    CASE mi.status 
                      WHEN N'ยังไม่ได้ดำเนินการ' THEN N'ไม่มี' 
                      WHEN N'ขาดต่อ' THEN N'ไม่มี' 
                      ELSE (select top 1 ac.firstname from log_all la inner join account ac on la.by_user = ac.user_id  where column_id = 46 and _data = mi.status order by la_id desc)
                    END as operator ,
                     mi.end_date as start_schedule,dateadd(dd,90, mi.end_date) as deadline,mi.note as note ,isnull(mi.status, '') as data_status 
	                    from license as l 
		                    join main_insurance as mi on l.license_id = mi.license_id 
			                    where (mi.status <> null or mi.status <> '')

                    union select l.license_id as number_car,l.license_id as license_car,  dpi.end_date as date_expire , N'ประกันภัยรถยนต์.' as tablename,
                    CASE dpi.status 
                      WHEN N'ยังไม่ได้ดำเนินการ' THEN N'ไม่มี' 
                      WHEN N'ขาดต่อ' THEN N'ไม่มี' 
                      ELSE (select top 1 ac.firstname from log_all la inner join account ac on la.by_user = ac.user_id  where column_id = 46 and _data = dpi.status order by la_id desc)
                    END as operator ,
                     dpi.end_date as start_schedule,dateadd(dd,90, dpi.end_date) as deadline,dpi.note as note ,isnull(dpi.status, '') as data_status 
	                     from license as l 
		                    join domestic_product_insurance as dpi on l.license_id = dpi.license_id 
			                    where (dpi.status <> null or dpi.status <> '')

                    union select l.license_id as number_car,l.license_id as license_car,  ei.end_date as date_expire , N'ประกันภัยรถยนต์.' as tablename,
                    CASE ei.status 
                      WHEN N'ยังไม่ได้ดำเนินการ' THEN N'ไม่มี' 
                      WHEN N'ขาดต่อ' THEN N'ไม่มี' 
                      ELSE (select top 1 ac.firstname from log_all la inner join account ac on la.by_user = ac.user_id  where column_id = 46 and _data = ei.status order by la_id desc)
                    END as operator ,
                     ei.end_date as start_schedule,dateadd(dd,90, ei.end_date) as deadline,ei.note as note ,isnull(ei.status, '') as data_status 
	                    from license as l 
		                    join environment_insurance as ei on l.license_id = ei.license_id 
			                    where (ei.status <> null or ei.status <> '')

                    order by date_expire
                    "
            Return GbFnPoom.GetData(SQL)
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameTrackingwork() As String
            Return GbFnPoom.fnRename(Request, "Trackingwork")
        End Function

        Public Function UpdateTrackingwork(ByVal number_car As String, ByVal expiredate As String, ByVal trackinglistid As String _
                                      , ByVal detail As String, ByVal operator_e As String, ByVal agencycontact As String _
                                      , ByVal startschedule As String, ByVal endschedule As String, ByVal cost_estimate As String _
                                      , ByVal statusurgencyid As String, ByVal note As String, ByVal key As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [trackingwork] SET "
            Dim StrTbTrackingwork() As String = {"number_car", "itm_name", "expiredate", "no_of_itm", "trackinglistid", "detail", "operator_e", "agencycontact", "startschedule", "cost_estimate", "statusurgencyid", "note"}
            Dim TbInsTrackingwork() As Object = {number_car, expiredate, trackinglistid, detail, operator_e, agencycontact, startschedule, endschedule, cost_estimate, statusurgencyid, note}
            For n As Integer = 0 To TbInsTrackingwork.Length - 1
                If Not TbInsTrackingwork(n) Is Nothing Then
                    _SQL &= StrTbTrackingwork(n) & "=N'" & TbInsTrackingwork(n) & "',"
                    GbFn.KeepLog(StrTbTrackingwork(n), TbInsTrackingwork(n), "Editing", IdTable, key)
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE [tw_id] = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertTrackingwork(ByVal number_car As String, ByVal expiredate As String, ByVal trackinglistid As String _
                                      , ByVal detail As String, ByVal operator_e As String, ByVal agencycontact As String _
                                      , ByVal startschedule As String, ByVal endschedule As String, ByVal cost_estimate As String _
                                      , ByVal statusurgencyid As String, ByVal note As String, ByVal key As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [trackingwork] ([license_id],[number_car],[expiredate],[trackinglistid],[detail],[operator_e],[agencycontact],[startschedule],[endschedule],[cost_estimate],[statusurgencyid],[note],[create_date],[create_by_user_id])  OUTPUT Inserted.tw_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(number_car Is Nothing, 0, number_car) & "',"
            _SQL &= "N'" & IIf(expiredate Is Nothing, String.Empty, expiredate) & "',"
            _SQL &= "N'" & IIf(trackinglistid Is Nothing, String.Empty, trackinglistid) & "',"
            _SQL &= "N'" & IIf(detail Is Nothing, String.Empty, detail) & "',"
            _SQL &= "N'" & IIf(operator_e Is Nothing, String.Empty, operator_e) & "',"
            _SQL &= "N'" & IIf(agencycontact Is Nothing, String.Empty, agencycontact) & "',"
            _SQL &= "N'" & IIf(startschedule Is Nothing, String.Empty, startschedule) & "',"
            _SQL &= "N'" & IIf(endschedule Is Nothing, String.Empty, endschedule) & "',"
            _SQL &= "N'" & IIf(cost_estimate Is Nothing, 0, cost_estimate) & "',"
            _SQL &= "N'" & IIf(statusurgencyid Is Nothing, String.Empty, statusurgencyid) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            If Not number_car Is Nothing Then
                'If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                'DtJson.Rows.Add("1")
                'Else
                '    DtJson.Rows.Add("0")
                'End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            Dim StrTbTrackingwork() As String = {"number_car", "itm_name", "expiredate", "no_of_itm", "trackinglistid", "detail", "operator_e", "agencycontact", "startschedule", "cost_estimate", "statusurgencyid", "note"}
            Dim TbInsTrackingwork() As Object = {number_car, expiredate, trackinglistid, detail, operator_e, agencycontact, startschedule, endschedule, cost_estimate, statusurgencyid, note}
            For n As Integer = 0 To TbInsTrackingwork.Length - 1
                If Not TbInsTrackingwork(n) Is Nothing Then
                    _SQL &= StrTbTrackingwork(n) & "=N'" & TbInsTrackingwork(n) & "',"
                    GbFn.KeepLog(StrTbTrackingwork(n), TbInsTrackingwork(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                End If
            Next
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


        Public Function DeleteTrackingwork(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [trackingwork] WHERE [tw_id] = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region 'Trackingwork

#Region "Accident"
        Function Accident() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View("../Home/Accident")
            Else
                Return View("../Account/Login")
            End If
        End Function
        'Get data of table Accident
        Public Function GetAccidentData() As String
            Return GbFnPoom.GetData("SELECT *,N'ประวัติ' as history   FROM [dbo].[accident] acd , [dbo].[license] li where acd.license_id = li.license_id order by li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameAccident() As String
            Return GbFnPoom.fnRename(Request, "Accident")
        End Function

        Public Function UpdateAccident(ByVal number_car As String, ByVal acd_date As String, ByVal damages As String _
                                      , ByVal detail As String, ByVal who_pay As String, ByVal note As String, ByVal key As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [accident] SET "
            Dim StrTbAccident() As String = {"number_car", "acd_date", "damages", "detail", "who_pay", "note"}
            Dim TbInsAccident() As Object = {number_car, acd_date, damages, detail, who_pay, note}
            For n As Integer = 0 To TbInsAccident.Length - 1
                If Not TbInsAccident(n) Is Nothing Then
                    _SQL &= StrTbAccident(n) & "=N'" & TbInsAccident(n) & "',"
                    GbFn.KeepLog(StrTbAccident(n), TbInsAccident(n), "Editing", IdTable, key)
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE [acd_id] = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertAccident(ByVal number_car As String, ByVal acd_date As String, ByVal damages As String _
                                      , ByVal detail As String, ByVal who_pay As String, ByVal note As String, ByVal key As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [accident] ([license_id],[number_car],[acd_date],[damages],[detail],[who_pay],[note],[create_date],[create_by_user_id])   OUTPUT Inserted.acd_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(number_car Is Nothing, 0, number_car) & "',"
            _SQL &= "N'" & IIf(acd_date Is Nothing, String.Empty, acd_date) & "',"
            _SQL &= "N'" & IIf(damages Is Nothing, 0, damages) & "',"
            _SQL &= "N'" & IIf(detail Is Nothing, String.Empty, detail) & "',"
            _SQL &= "N'" & IIf(who_pay Is Nothing, String.Empty, who_pay) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            If Not number_car Is Nothing Then
                'If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                'DtJson.Rows.Add("1")
                'Else
                '    DtJson.Rows.Add("0")
                'End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            Dim StrTbAccident() As String = {"number_car", "acd_date", "damages", "detail", "who_pay", "note"}
            Dim TbInsAccident() As Object = {number_car, acd_date, damages, detail, who_pay, note}
            For n As Integer = 0 To TbInsAccident.Length - 1
                If Not TbInsAccident(n) Is Nothing Then
                    _SQL &= StrTbAccident(n) & "=N'" & TbInsAccident(n) & "',"
                    GbFn.KeepLog(StrTbAccident(n), TbInsAccident(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                End If
            Next

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function


        Public Function DeleteAccident(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "DELETE [accident] WHERE [acd_id] = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#End Region 'Accident

#Region "Function Poom (coppy tew.)"
        'Get data Filses
        Public Function GetFilesPoom(ByVal Id As Integer, ByVal IdTable As Integer) As String
            Return GbFnPoom.GetFiles(Id, IdTable)
        End Function

        'Get data config column
        Public Function GetColumnChooserPoom(ByVal gbTableId As Integer) As String
            Return GbFnPoom.GetColumnChooser(gbTableId)
        End Function
        'Insert File (pic,pdf)
        Public Function InsertFilePoom() As String
            Return GbFnPoom.InsertFile(Request)
        End Function

        'Delete File (pic,pdf)
        Public Function DeleteFilePoom(ByVal keyId As String, ByVal FolderName As String) As String
            Return GbFnPoom.DeleteFile(keyId, FolderName)
        End Function

        Public Function GetLicenseCarPoom(ByVal number_car As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT license_car FROM license WHERE number_car = '" & number_car & "'"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
#End Region
#End Region

    End Class
End Namespace