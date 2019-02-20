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
                Return View("../Home/License")
            Else
                Return View("../Account/Login")
            End If
        End Function

#Region "Develop by Thung"

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
            'Dim _SQL As String = "SELECT cc1.column_id, cc1.name_column AS dataField, cc1.display AS caption, cc1.data_type AS dataType, cc1.alignment, cc1.width, ISNULL(cc2.visible,0) AS visible FROM [config_column] AS cc1 LEFT JOIN [chooser_column] AS cc2 ON cc1.column_id = cc2.column_id WHERE cc2.user_id = " & Session("UserId")
            'Dim _SQL As String = "SELECT column_id, name_column AS dataField, display AS caption, data_type AS dataType, alignment, width, ISNULL(visible,0) AS visible, fixed, format, colSpan FROM [config_column] WHERE name_column <> 'license_id' ORDER BY sort ASC"
            Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(cc.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id WHERE cc.name_column <> 'license_id' AND table_id = " & table_id & " ORDER BY cc.sort ASC"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetLicense() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT [license_id],[number_car],[license_car],[province],[type_fuel],[type_car],[style_car],[brand_car],[model_car],[color_car],[number_body],[number_engine],[number_engine_point_1],[number_engine_point_2],[brand_engine],[pump],[horse_power],[shaft],[wheel],[tire],[license_date],[weight_car],[weight_lade],[weight_total],[ownership],[transport_operator],[transport_type],FORMAT([create_date], 'yyyy-MM-dd'),[create_by_user_id],[update_date],[update_by_user_id], [seq], [nationality], [id_card], [address], [license_expiration], [possessory_right], [license_no], [license_status] FROM [license] ORDER BY number_car"
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
                                      , ByVal license_expiration As String, ByVal possessory_right As String, ByVal license_no As String, ByVal license_status As String) As String

            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [license] SET "
            Dim StrTbLicense() As String = {"number_car", "license_car", "province", "type_fuel", "type_car", "style_car", "brand_car", "model_car", "color_car", "number_body", "number_engine", "number_engine_point_1", "number_engine_point_2", "brand_engine", "pump", "horse_power", "shaft", "wheel", "tire", "license_date", "weight_car", "weight_lade", "weight_total", "ownership", "transport_operator", "transport_type", "seq", "nationality", "id_card", "[address]", "license_expiration", "possessory_right", "license_no", "license_status"}
            Dim TbLicense() As Object = {number_car, license_car, province, type_fuel, type_car, style_car, brand_car, model_car, color_car, number_body, number_engine, number_engine_point_1, number_engine_point_2, brand_engine, pump, horse_power, shaft, wheel, tire, license_date, weight_car, weight_lade, weight_total, ownership, transport_operator, transport_type, seq, nationality, id_card, address, license_expiration, possessory_right, license_no, license_status}
            For n As Integer = 0 To TbLicense.Length - 1
                If Not TbLicense(n) Is Nothing Then
                    _SQL &= StrTbLicense(n) & "=N'" & TbLicense(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE license_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
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
                                      , ByVal license_expiration As String, ByVal possessory_right As String, ByVal license_no As String, ByVal license_status As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO [license] ([number_car],[license_car],[province],[type_fuel],[type_car],[style_car],[brand_car],[model_car],[color_car],[number_body],[number_engine],[number_engine_point_1],[number_engine_point_2],[brand_engine],[pump],[horse_power],[shaft],[wheel],[tire],[license_date],[weight_car],[weight_lade],[weight_total],[ownership],[transport_operator],[transport_type], [seq], [nationality], [id_card], [address], [license_expiration], [possessory_right], [license_no], [license_status],[create_by_user_id]) OUTPUT Inserted.license_id"
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
            _SQL &= Session("UserId") & ")"
            If Not number_car Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                'If objDB.ExecuteSQL(_SQL, cn) Then
                '    DtJson.Rows.Add("1")
                'Else
                '    DtJson.Rows.Add("0")
                'End If
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
                If Request.Form.AllKeys.Length <> 0 Then
                    For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                        If Request.Form.AllKeys(i) = "fk_id" Then
                            fk_id = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "parentDirId" Then
                            parentDirId = Request.Form(i)
                        ElseIf Request.Form.AllKeys(i) = "newFolder" Then
                            newFolder = Request.Form(i)
                        End If
                    Next
                    Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                    If Request.Files.Count = 0 Then
                        Dim _Path As String = fnGetPath(parentDirId)
                        Dim pathServer As String = Server.MapPath("~/Files/License/Root/" & fk_id & "/" & IIf(_Path = String.Empty, String.Empty, _Path & "/") & newFolder)
                        If (Not System.IO.Directory.Exists(pathServer)) Then
                            System.IO.Directory.CreateDirectory(pathServer)
                        End If
                        Dim _SQL As String = "INSERT INTO [files_all] ([fk_id],[table_id],[name_file],[type_file],[path_file],[parentDirId],[icon],[create_by_user_id]) VALUES (" & fk_id & ",1,N'" & newFolder & "','folder',N'','" & parentDirId & "','../Img/folder.png'," & Session("UserId") & ")"
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
                            Dim _SQL As String = "INSERT INTO [files_all] ([fk_id], [table_id], [name_file], [type_file], [path_file], [parentDirId], [icon], [create_by_user_id]) VALUES (" & fk_id & ",1, N'" & file.FileName & "','" & name_icon & "',N'.." & PathFile & "','" & parentDirId & "','../Img/" & name_icon & ".png'," & Session("UserId") & ")"
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
            Dim _SQL As String = "SELECT t.tax_id,t.tax_expire,t.tax_startdate,t.tax_rate,t.tax_status,l.number_car, l.license_car, l.license_id, t.special_expenses_1, t.special_expenses_2, t.contract_wages FROM tax as t join license as l on t.license_id = l.license_id"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateTax(ByVal tax_id As String, ByVal tax_expire As String, ByVal tax_rate As String, ByVal tax_startdate As String, ByVal tax_status As String, ByVal special_expenses_1 As String, ByVal special_expenses_2 As String, ByVal contract_wages As String) As String

            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [tax] SET "
            Dim StrTbTax() As String = {"tax_expire", "tax_rate", "tax_startdate", "tax_status", "special_expenses_1", "special_expenses_2", "contract_wages"}
            Dim TbTax() As Object = {tax_expire, tax_rate, tax_startdate, tax_status, special_expenses_1, special_expenses_2, contract_wages}
            For n As Integer = 0 To TbTax.Length - 1
                If Not TbTax(n) Is Nothing Then
                    _SQL &= StrTbTax(n) & "=N'" & TbTax(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE tax_id = " & tax_id
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertTax(ByVal license_id As String, ByVal tax_expire As DateTime, ByVal tax_rate As String, ByVal tax_startdate As DateTime, ByVal tax_status As String, ByVal special_expenses_1 As String, ByVal special_expenses_2 As String, ByVal contract_wages As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO tax (tax_expire, tax_startdate, tax_rate, tax_status, license_id, create_by_user_id, create_date, special_expenses_1, special_expenses_2, contract_wages) OUTPUT Inserted.tax_id VALUES "
            _SQL &= "('" & tax_expire & "', '" & tax_startdate & "', '" & tax_rate & "', N'" & tax_status & "', '" & license_id & "', '" & Session("UserId") & "', GETDATE(), " & IIf(special_expenses_1 Is Nothing, 0, special_expenses_1) & ", " & IIf(special_expenses_2 Is Nothing, 0, special_expenses_2) & ", " & IIf(special_expenses_2 Is Nothing, 0, contract_wages) & ")"
            If Not license_id Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                'If objDB.ExecuteSQL(_SQL, cn) Then
                '    DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
                'Else
                '    DtJson.Rows.Add("0")
                'End If
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
                DtJson.Rows.Add("0")
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
            Dim _SQL As String = "SELECT o.or_list, o.or_id,o.registrar,o.record_date,l.number_car, l.license_car, l.license_id FROM officer_records as o join license as l on o.license_id = l.license_id"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateOR(ByVal or_id As String, ByVal registrar As String, ByVal record_date As String, ByVal or_list As String) As String

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
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertOR(ByVal license_id As String, ByVal record_date As DateTime, ByVal registrar As String, ByVal or_list As String) As String

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
            Dim _SQL As String = "SELECT d.driver_id, d.driver_name, d.start_work_date, d.license_id_head, d.license_id_tail, (select license_car from license where license_id = d.license_id_head) as license_car_head, (select license_car from license where license_id = d.license_id_tail) as license_car_tail FROM driver as d"
            Dim DtDriver As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtDriver.Rows Select DtDriver.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertDriver(ByVal driver_name As String, ByVal start_work_date As DateTime, ByVal license_id_head As Integer, ByVal license_id_tail As Integer) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO driver (driver_name, start_work_date, license_id_head, license_id_tail, create_by_user_id) OUTPUT Inserted.driver_id VALUES ('" & driver_name & "', '" & start_work_date & "', " & license_id_head & ", " & license_id_tail & ", '" & Session("UserId") & "')"
            If license_id_head <> Nothing And license_id_tail <> Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateDriver(ByVal driver_id As String, ByVal driver_name As String, ByVal start_work_date As String, ByVal license_id_head As String, ByVal license_id_tail As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [driver] SET "
            Dim StrTbDriver() As String = {"driver_name", "start_work_date", "license_id_head", "license_id_tail"}
            Dim TbDriver() As Object = {driver_name, start_work_date, license_id_head, license_id_tail}
            For n As Integer = 0 To TbDriver.Length - 1
                If Not TbDriver(n) Is Nothing Then
                    _SQL &= StrTbDriver(n) & "=N'" & TbDriver(n) & "',"
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

#Region "LicenseCambodia"
        Function LicenseCambodia() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
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
            Dim _SQL As String = "SELECT business_path, business_id, business_number, business_expire, business_start, business_name, business_address, business_type, country_code, benefit, business_status FROM business_in"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetNumberCarBusiness() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT l.number_car, l.license_id, l.license_car, l.brand_car, l.number_body, l.number_engine, l.style_car, t.tax_expire, [bit].bit_name FROM license as l left join tax as t on l.license_id = t.license_id left join business_in_permission as bip on l.license_id = bip.license_id left join business_in_type as [bit] on bip.bit_id = [bit].bit_id"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetBusinessInPermission() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT bip.bip_id, bip.business_id, [bit].bit_id as bit_name, l.license_id, l.number_car, l.license_car, l.brand_car, l.number_body, l.number_engine, t.tax_expire, l.style_car FROM business_in_permission as bip join business_in_type as [bit] on bip.bit_id = [bit].bit_id join license as l on bip.license_id = l.license_id join tax as t on l.license_id = t.license_id"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateBusinessIn(ByVal business_id As String, ByVal business_number As String, ByVal business_expire As String, ByVal business_start As String, ByVal business_name As String, ByVal business_address As String, ByVal business_type As String, ByVal business_path As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [business_in] SET "
            Dim StrTbBusiness() As String = {"business_number", "business_expire", "business_start", "business_name", "business_address", "business_type", "business_path"}
            Dim TbBusiness() As Object = {business_number, business_expire, business_start, business_name, business_address, business_type, business_path}
            For n As Integer = 0 To TbBusiness.Length - 1
                If Not TbBusiness(n) Is Nothing Then
                    _SQL &= StrTbBusiness(n) & "=N'" & TbBusiness(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE business_id = " & business_id
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
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

        Public Function InsertBusinessIn(ByVal business_number As String, ByVal business_expire As DateTime, ByVal business_start As DateTime, ByVal business_name As String, ByVal business_address As String, ByVal business_type As String, ByVal country_code As String, ByVal benefit As String, ByVal business_status As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO business_in (business_number, business_expire, business_start, business_name, business_address, business_type, country_code, benefit, business_status, create_by_user_id) OUTPUT Inserted.business_id VALUES "
            _SQL &= "(N'" & business_number & "', '" & business_expire & "', '" & business_start & "', N'" & business_name & "', N'" & business_address & "', N'" & business_type & "', N'" & country_code & "', N'" & benefit & "', N'" & business_status & "', '" & Session("UserId") & "')"
            If Not business_number Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertBusinessInPermission(ByVal bit_name As String, ByVal business_id As String, ByVal number_car As String) As String

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

        Public Function DeleteBusinessInPermission(ByVal keyId As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = String.Empty
            _SQL = "DELETE business_in_permission WHERE bip_id = " & keyId
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
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

#Region "Utility"

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
            Dim _SQL As String = "SELECT DISTINCT(number_car) as number_car, license_id, license_car FROM license"
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
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history FROM [dbo].[act_insurance_company] aic , [dbo].[license] li where aic.license_id = li.license_id order by li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameAIC() As String
            Return GbFn.fnRename(Request, "act_insurance_company")
        End Function
        Public Function UpdateAIC(ByVal number_car As String, ByVal name As String, ByVal start_date As String, ByVal end_date As String, ByVal first_damages As String _
                                  , ByVal current_cowrie As String, ByVal previous_cowrie As String, ByVal price As String, ByVal protection As String, ByVal protection_limit As String _
                                      , ByVal note As String, ByVal key As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [act_insurance_company] SET "
            Dim StrTbAIC() As String = {"name", "start_date", "end_date", "first_damages", "current_cowrie", "previous_cowrie", "price", "protection", "protection_limit", "note"}
            Dim TbAIC() As Object = {name, start_date, end_date, first_damages, current_cowrie, previous_cowrie, price, protection, protection_limit, note}
            For n As Integer = 0 To TbAIC.Length - 1
                If Not TbAIC(n) Is Nothing Then
                    _SQL &= StrTbAIC(n) & "=N'" & TbAIC(n) & "',"
                    GbFn.KeepLog(StrTbAIC(n), TbAIC(n), "Editing", IdTable, key)
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE aic_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertAIC(ByVal number_car As String, ByVal name As String, ByVal start_date As String, ByVal end_date As String, ByVal first_damages As String _
                                  , ByVal current_cowrie As String, ByVal previous_cowrie As String, ByVal price As String, ByVal protection As String, ByVal protection_limit As String _
                                      , ByVal note As String, ByVal key As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [act_insurance_company] ([license_id],[name],[start_date],[end_date],[first_damages],[current_cowrie],[previous_cowrie],[price],[protection],[protection_limit],[note],[create_date],[create_by_user_id]) OUTPUT Inserted.aic_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(name Is Nothing, String.Empty, name) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(end_date Is Nothing, String.Empty, end_date) & "',"
            _SQL &= "N'" & IIf(first_damages Is Nothing, String.Empty, first_damages) & "',"
            _SQL &= "N'" & IIf(current_cowrie Is Nothing, String.Empty, current_cowrie) & "',"
            _SQL &= "N'" & IIf(previous_cowrie Is Nothing, String.Empty, previous_cowrie) & "',"
            _SQL &= "N'" & IIf(price Is Nothing, String.Empty, price) & "',"
            _SQL &= "N'" & IIf(protection Is Nothing, String.Empty, protection) & "',"
            _SQL &= "N'" & IIf(protection_limit Is Nothing, String.Empty, protection_limit) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            If Not number_car Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            Dim StrTbAIC() As String = {"name", "start_date", "end_date", "first_damages", "current_cowrie", "previous_cowrie", "price", "protection", "protection_limit", "note"}
            Dim TbAIC() As Object = {name, start_date, end_date, first_damages, current_cowrie, previous_cowrie, price, protection, protection_limit, note}
            For n As Integer = 0 To TbAIC.Length - 1
                If Not TbAIC(n) Is Nothing Then
                    _SQL &= StrTbAIC(n) & "=N'" & TbAIC(n) & "',"
                    GbFn.KeepLog(StrTbAIC(n), TbAIC(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                End If
            Next
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
                                SELECT li.*,mic.[mic_id]
                                    ,mic.[license_id]
                                    ,mic.[insurance_company]
                                    ,mic.[first_damages]
                                    ,mic.[insurance_number]
                                    ,mic.[assured]
                                    ,mic.[start_date]
                                    ,mic.[end_date]
                                    ,mic.[type]
                                    ,mic.[price]
                                    ,mic.[protection]
                                    ,mic.[protection_limit]
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
	                                FROM [dbo].[main_insurance_company] mic , [dbo].[license] li where mic.license_id = li.license_id order by li.number_car
                            ")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameMIC() As String
            Return GbFn.fnRename(Request, "main_insurance_company")
        End Function
        Public Function UpdateMIC(ByVal number_car As String, ByVal insurance_company As String, ByVal first_damages As String, ByVal insurance_number As String, ByVal assured As String _
                                  , ByVal start_date As String, ByVal end_date As String, ByVal type As String, ByVal price As String, ByVal protection As String, ByVal protection_limit As String _
                                  , ByVal extension_insurance As String, ByVal note As String, ByVal key As String, ByVal t1_1_1 As String, ByVal t1_1_2 As String, ByVal t1_2_1 As String _
                                  , ByVal t1_2_2 As String, ByVal t2_1_1 As String, ByVal t2_1_2 As String, ByVal t2_2_1 As String, ByVal t3_1_1_a As String, ByVal t3_1_1_b As String _
                                  , ByVal t3_1_2_a As String, ByVal t3_1_2_b As String, ByVal t3_2_1 As String, ByVal t3_3_1 As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [main_insurance_company] SET "
            Dim StrTbMIC() As String = {"insurance_company", "first_damages", "insurance_number", "assured", "start_date", "end_date", "type", "price", "protection", "protection_limit", "extension_insurance", "note", "t1_1_1", "t1_1_2", "t1_2_1", "t1_2_2", "t2_1_1", "t2_1_2", "t2_2_1", "t3_1_1_a", "t3_1_1_b", "t3_1_2_a", "t3_1_2_b", "t3_2_1", "t3_3_1"}
            Dim TbMIC() As Object = {insurance_company, first_damages, insurance_number, assured, start_date, end_date, type, price, protection, protection_limit, extension_insurance, note, t1_1_1, t1_1_2, t1_2_1, t1_2_2, t2_1_1, t2_1_2, t2_2_1, t3_1_1_a, t3_1_1_b, t3_1_2_a, t3_1_2_b, t3_2_1, t3_3_1}
            For n As Integer = 0 To TbMIC.Length - 1
                If Not TbMIC(n) Is Nothing Then
                    _SQL &= StrTbMIC(n) & " = N'" & TbMIC(n) & "',"
                    GbFn.KeepLog(StrTbMIC(n), TbMIC(n), "Editing", IdTable, key)
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE mic_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertMIC(ByVal number_car As String, ByVal insurance_company As String, ByVal first_damages As String, ByVal insurance_number As String, ByVal assured As String _
                                  , ByVal start_date As String, ByVal end_date As String, ByVal type As String, ByVal price As String, ByVal protection As String, ByVal protection_limit As String _
                                  , ByVal extension_insurance As String, ByVal note As String, ByVal key As String, ByVal t1_1_1 As String, ByVal t1_1_2 As String, ByVal t1_2_1 As String _
                                  , ByVal t1_2_2 As String, ByVal t2_1_1 As String, ByVal t2_1_2 As String, ByVal t2_2_1 As String, ByVal t3_1_1_a As String, ByVal t3_1_1_b As String _
                                  , ByVal t3_1_2_a As String, ByVal t3_1_2_b As String, ByVal t3_2_1 As String, ByVal t3_3_1 As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [main_insurance_company] ([license_id],[insurance_company],[first_damages],[insurance_number],[assured],[start_date],[end_date],[type]" &
                ",[price],[protection],[protection_limit],[extension_insurance],[note],[create_date],[create_by_user_id],[t1_1_1],[t1_1_2],[t1_2_1],[t1_2_2],[t2_1_1],[t2_1_2],[t2_2_1]" &
                ",[t3_1_1_a],[t3_1_1_b],[t3_1_2_a],[t3_1_2_b],[t3_2_1],[t3_3_1]) OUTPUT Inserted.mic_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(insurance_company Is Nothing, String.Empty, insurance_company) & "',"
            _SQL &= "N'" & IIf(first_damages Is Nothing, String.Empty, first_damages) & "',"
            _SQL &= "N'" & IIf(insurance_number Is Nothing, String.Empty, insurance_number) & "',"
            _SQL &= "N'" & IIf(assured Is Nothing, String.Empty, assured) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(end_date Is Nothing, String.Empty, end_date) & "',"
            _SQL &= "N'" & IIf(type Is Nothing, String.Empty, type) & "',"
            _SQL &= "N'" & IIf(price Is Nothing, String.Empty, price) & "',"
            _SQL &= "N'" & IIf(protection Is Nothing, String.Empty, protection) & "',"
            _SQL &= "N'" & IIf(protection_limit Is Nothing, String.Empty, protection_limit) & "',"
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

            If Not number_car Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

            Dim StrTbMIC() As String = {"insurance_company", "first_damages", "insurance_number", "assured", "start_date", "end_date", "type", "price", "protection", "protection_limit", "extension_insurance", "note", "t1_1_1", "t1_1_2", "t1_2_1", "t1_2_2", "t2_1_1", "t2_1_2", "t2_2_1", "t3_1_1_a", "t3_1_1_b", "t3_1_2_a", "t3_1_2_b", "t3_2_1", "t3_3_1"}
            Dim TbMIC() As Object = {insurance_company, first_damages, insurance_number, assured, start_date, end_date, type, price, protection, protection_limit, extension_insurance, note, t1_1_1, t1_1_2, t1_2_1, t1_2_2, t2_1_1, t2_1_2, t2_2_1, t3_1_1_a, t3_1_1_b, t3_1_2_a, t3_1_2_b, t3_2_1, t3_3_1}
            For n As Integer = 0 To TbMIC.Length - 1
                If Not TbMIC(n) Is Nothing Then
                    GbFn.KeepLog(StrTbMIC(n), TbMIC(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                End If
            Next

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
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history FROM [dbo].[product_insurance_company] pic , [dbo].[license] li where pic.license_id = li.license_id order by li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenamePIC() As String
            Return GbFn.fnRename(Request, "product_insurance_company")
        End Function
        Public Function UpdatePIC(ByVal number_car As String, ByVal first_damages As String, ByVal current_cowrie As String, ByVal previous_cowrie As String,
                                  ByVal start_date As String, ByVal end_date As String, ByVal main_protection_product As String, ByVal price As String, ByVal protection_limit As String _
                                      , ByVal extension_insurance As String, ByVal note As String, ByVal key As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [product_insurance_company] SET "
            Dim StrTbPIC() As String = {"first_damages", "current_cowrie", "previous_cowrie", "start_date", "end_date", "main_protection_product", "price", "protection_limit", "extension_insurance", "note"}
            Dim TbPIC() As Object = {first_damages, current_cowrie, previous_cowrie, start_date, end_date, main_protection_product, price, protection_limit, extension_insurance, note}
            For n As Integer = 0 To TbPIC.Length - 1
                If Not TbPIC(n) Is Nothing Then
                    _SQL &= StrTbPIC(n) & "=N'" & TbPIC(n) & "',"
                    GbFn.KeepLog(StrTbPIC(n), TbPIC(n), "Editing", IdTable, key)
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE pic_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertPIC(ByVal number_car As String, ByVal first_damages As String, ByVal current_cowrie As String, ByVal previous_cowrie As String,
                                  ByVal start_date As String, ByVal end_date As String, ByVal main_protection_product As String, ByVal price As String, ByVal protection_limit As String _
                                      , ByVal extension_insurance As String, ByVal note As String, ByVal key As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable

            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [product_insurance_company] ([license_id],[first_damages],[current_cowrie],[previous_cowrie],[start_date],[end_date],[main_protection_product],[price],[protection_limit],[extension_insurance],[note],[create_date],[create_by_user_id]) OUTPUT Inserted.pic_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(first_damages Is Nothing, String.Empty, first_damages) & "',"
            _SQL &= "N'" & IIf(current_cowrie Is Nothing, String.Empty, current_cowrie) & "',"
            _SQL &= "N'" & IIf(previous_cowrie Is Nothing, String.Empty, previous_cowrie) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(end_date Is Nothing, String.Empty, end_date) & "',"
            _SQL &= "N'" & IIf(main_protection_product Is Nothing, String.Empty, main_protection_product) & "',"
            _SQL &= "N'" & IIf(price Is Nothing, String.Empty, price) & "',"
            _SQL &= "N'" & IIf(protection_limit Is Nothing, String.Empty, protection_limit) & "',"
            _SQL &= "N'" & IIf(extension_insurance Is Nothing, String.Empty, extension_insurance) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            If Not number_car Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

            Dim StrTbPIC() As String = {"first_damages", "current_cowrie", "previous_cowrie", "start_date", "end_date", "main_protection_product", "price", "protection_limit", "extension_insurance", "note"}
            Dim TbPIC() As Object = {first_damages, current_cowrie, previous_cowrie, start_date, end_date, main_protection_product, price, protection_limit, extension_insurance, note}
            For n As Integer = 0 To TbPIC.Length - 1
                If Not TbPIC(n) Is Nothing Then
                    GbFn.KeepLog(StrTbPIC(n), TbPIC(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                End If
            Next

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
            Return GbFn.GetData("SELECT *,N'ประวัติ' as history FROM [dbo].[gps_company] gc , [dbo].[license] li where gc.license_id = li.license_id order by li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameGpsCompany() As String
            Return GbFn.fnRename(Request, "Gps_company")
        End Function
        Public Function UpdateGpsCompany(ByVal number_car As String, ByVal number_sim As String, ByVal number_book As String, ByVal company As String _
                                         , ByVal type As String, ByVal model As String, ByVal number_serial As String, ByVal expire_date As String _
                                         , ByVal reading_data As String, ByVal usage As String, ByVal price As String, ByVal note As String _
                                         , ByVal key As String, ByVal IdTable As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [gps_company] SET "
            Dim StrTbGpsCompany() As String = {"number_sim", "number_book", "company", "type", "model", "number_serial", "expire_date", "reading_data", "usage", "price", "note"}
            Dim TbGpsCompany() As Object = {number_sim, number_book, company, type, model, number_serial, expire_date, reading_data, usage, price, note}
            For n As Integer = 0 To TbGpsCompany.Length - 1
                If Not TbGpsCompany(n) Is Nothing Then
                    _SQL &= StrTbGpsCompany(n) & "=N'" & TbGpsCompany(n) & "',"
                    GbFn.KeepLog(StrTbGpsCompany(n), TbGpsCompany(n), "Editing", IdTable, key)
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE gc_id = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertGpsCompany(ByVal number_car As String, ByVal number_sim As String, ByVal number_book As String, ByVal company As String _
                                         , ByVal type As String, ByVal model As String, ByVal number_serial As String, ByVal expire_date As String _
                                         , ByVal reading_data As String, ByVal usage As String, ByVal price As String, ByVal note As String _
                                         , ByVal key As String, ByVal IdTable As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [gps_company] ([license_id],[number_sim],[number_book],[company],[type],[model],[number_serial],[expire_date],[reading_data],[usage],[price],[note],[create_date],[create_by_user_id]) OUTPUT Inserted.gc_id"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(number_sim Is Nothing, String.Empty, number_sim) & "',"
            _SQL &= "N'" & IIf(number_book Is Nothing, String.Empty, number_book) & "',"
            _SQL &= "N'" & IIf(company Is Nothing, String.Empty, company) & "',"
            _SQL &= "N'" & IIf(type Is Nothing, String.Empty, type) & "',"
            _SQL &= "N'" & IIf(model Is Nothing, String.Empty, model) & "',"
            _SQL &= "N'" & IIf(number_serial Is Nothing, String.Empty, number_serial) & "',"
            _SQL &= "N'" & IIf(expire_date Is Nothing, String.Empty, expire_date) & "',"
            _SQL &= "N'" & IIf(reading_data Is Nothing, String.Empty, reading_data) & "',"
            _SQL &= "N'" & IIf(usage Is Nothing, String.Empty, usage) & "',"
            _SQL &= "N'" & IIf(price Is Nothing, String.Empty, price) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            If Not number_car Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If
            Dim StrTbGpsCompany() As String = {"number_sim", "number_book", "company", "type", "model", "number_serial", "expire_date", "reading_data", "usage", "price", "note"}
            Dim TbGpsCompany() As Object = {number_sim, number_book, company, type, model, number_serial, expire_date, reading_data, usage, price, note}
            For n As Integer = 0 To TbGpsCompany.Length - 1
                If Not TbGpsCompany(n) Is Nothing Then
                    _SQL &= StrTbGpsCompany(n) & "=N'" & TbGpsCompany(n) & "',"
                    GbFn.KeepLog(StrTbGpsCompany(n), TbGpsCompany(n), "Add", IdTable, DtJson.Rows(0).Item("Status").ToString)
                End If
            Next

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

#Region "Function Tew"

        Public Function getHistory(ByVal table As String, ByVal idOfTable As String) As String
            Return GbFn.GetData("select Row_Number() Over ( Order By la._date desc ) As row,la._event,cc.display as column_display,la._data,ac.firstname,la._date 
                                    from log_all la inner join config_column cc on cc.column_id = la.column_id inner join account ac on ac.user_id = la.by_user 
                                    where la.id_of_table = '" & idOfTable & "' and la._table = " & table & " order by la._date desc
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
                _SQL = "UPDATE config_column_data SET [visible] = '0' where cc_id in (" & ColumnHideForWhere & ") and user_id = '" + Session("UserId").ToString + "' "
                If objDB.ExecuteSQL(_SQL, cn) Then
                    Return 1
                Else
                    Return 0
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
            Return GbFnPoom.GetData("SELECT * FROM [dbo].[expressway] epw , [dbo].[license] li where epw.license_id = li.license_id order by li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameExpressway() As String
            Return GbFnPoom.fnRename(Request, "Expressway")
        End Function

        Public Function UpdateExpressway(ByVal number_car As String, ByVal start_date As String, ByVal expire_date As String _
                                      , ByVal processing_fee As String, ByVal special_money As String, ByVal epw_license As String _
                                      , ByVal note As String, ByVal key As String) As String
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
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertExpressway(ByVal number_car As String, ByVal start_date As String, ByVal expire_date As String _
                                      , ByVal processing_fee As String, ByVal special_money As String, ByVal epw_license As String _
                                      , ByVal note As String, ByVal key As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [expressway] ([license_id],[number_car],[start_date],[expire_date],[processing_fee],[special_money],[epw_license],[note],[create_date],[create_by_user_id])"
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
                If objDB.ExecuteSQL(_SQL, cn) Then
                    DtJson.Rows.Add("1")
                Else
                    DtJson.Rows.Add("0")
                End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
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
            Return GbFnPoom.GetData("SELECT * FROM [dbo].[gps_car] gps_car , [dbo].[license] li where gps_car.license_id = li.license_id order by li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameGps_car() As String
            Return GbFnPoom.fnRename(Request, "Gps_car")
        End Function

        Public Function UpdateGps_car(ByVal number_car As String, ByVal sim_no As String, ByVal gps_no As String _
                                      , ByVal start_date As String, ByVal expire_date As String, ByVal gps_price As String _
                                      , ByVal note As String, ByVal key As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [gps_car] SET "
            Dim StrTbGps_car() As String = {"number_car", "sim_no", "gps_no", "start_date", "expire_date", "gps_price", "note"}
            Dim TbGps_car() As Object = {number_car, sim_no, gps_no, start_date, expire_date, gps_price, note}
            For n As Integer = 0 To TbGps_car.Length - 1
                If Not TbGps_car(n) Is Nothing Then
                    _SQL &= StrTbGps_car(n) & "=N'" & TbGps_car(n) & "',"
                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE [gps_car_id] = " & key
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertGps_car(ByVal number_car As String, ByVal sim_no As String, ByVal gps_no As String _
                                      , ByVal start_date As String, ByVal expire_date As String, ByVal gps_price As String _
                                      , ByVal note As String, ByVal key As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [gps_car] ([license_id],[number_car],[sim_no],[gps_no],[start_date],[expire_date],[gps_price],[note],[create_date],[create_by_user_id])"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(number_car Is Nothing, 0, number_car) & "',"
            _SQL &= "N'" & IIf(sim_no Is Nothing, 0, sim_no) & "',"
            _SQL &= "N'" & IIf(gps_no Is Nothing, 0, gps_no) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(expire_date Is Nothing, String.Empty, expire_date) & "',"
            _SQL &= "N'" & IIf(gps_price Is Nothing, 0, gps_price) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            If Not number_car Is Nothing Then
                If objDB.ExecuteSQL(_SQL, cn) Then
                    DtJson.Rows.Add("1")
                Else
                    DtJson.Rows.Add("0")
                End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
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
            Return GbFnPoom.GetData("SELECT * FROM [dbo].[installment] installment , [dbo].[license] li where installment.license_id = li.license_id order by li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameInstallment() As String
            Return GbFnPoom.fnRename(Request, "Installment")
        End Function

        Public Function UpdateInstallment(ByVal number_car As String, ByVal itm_name As String, ByVal start_date As String _
                                      , ByVal no_of_itm As String, ByVal payment_of_itm As String, ByVal last_date As String _
                                      , ByVal payed_of_itm As String, ByVal last_payment As String, ByVal postponement_itm As String _
                                      , ByVal note As String, ByVal key As String) As String
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
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertInstallment(ByVal number_car As String, ByVal itm_name As String, ByVal start_date As String _
                                      , ByVal no_of_itm As String, ByVal payment_of_itm As String, ByVal last_date As String _
                                      , ByVal payed_of_itm As String, ByVal last_payment As String, ByVal postponement_itm As String _
                                      , ByVal note As String, ByVal key As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [installment] ([license_id],[number_car],[itm_name],[start_date],[no_of_itm],[payment_of_itm],[last_date],[payed_of_itm],[last_payment],[postponement_itm],[note],[create_date],[create_by_user_id])"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(number_car Is Nothing, 0, number_car) & "',"
            _SQL &= "N'" & IIf(itm_name Is Nothing, String.Empty, itm_name) & "',"
            _SQL &= "N'" & IIf(start_date Is Nothing, String.Empty, start_date) & "',"
            _SQL &= "N'" & IIf(no_of_itm Is Nothing, 0, no_of_itm) & "',"
            _SQL &= "N'" & IIf(payment_of_itm Is Nothing, 0, payment_of_itm) & "',"
            _SQL &= "N'" & IIf(last_date Is Nothing, 0, last_date) & "',"
            _SQL &= "N'" & IIf(payed_of_itm Is Nothing, 0, payed_of_itm) & "',"
            _SQL &= "N'" & IIf(last_payment Is Nothing, 0, last_payment) & "',"
            _SQL &= "N'" & IIf(postponement_itm Is Nothing, 0, postponement_itm) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            If Not number_car Is Nothing Then
                If objDB.ExecuteSQL(_SQL, cn) Then
                    DtJson.Rows.Add("1")
                Else
                    DtJson.Rows.Add("0")
                End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
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
            Return GbFnPoom.GetData("SELECT * FROM [dbo].[trackingwork] trackingwork , [dbo].[license] li where trackingwork.license_id = li.license_id order by li.number_car")
        End Function

        'Rename Folder or Files(pic,pdf)
        Public Function fnRenameTrackingwork() As String
            Return GbFnPoom.fnRename(Request, "Trackingwork")
        End Function

        Public Function UpdateTrackingwork(ByVal number_car As String, ByVal expiredate As String, ByVal trackinglistid As String _
                                      , ByVal detail As String, ByVal operator_e As String, ByVal agencycontact As String _
                                      , ByVal startschedule As String, ByVal endschedule As String, ByVal cost_estimate As String _
                                      , ByVal statusurgencyid As String, ByVal note As String, ByVal key As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [trackingwork] SET "
            Dim StrTbTrackingwork() As String = {"number_car", "itm_name", "expiredate", "no_of_itm", "trackinglistid", "detail", "operator_e", "agencycontact", "startschedule", "cost_estimate", "statusurgencyid", "note"}
            Dim TbInsTrackingwork() As Object = {number_car, expiredate, trackinglistid, detail, operator_e, agencycontact, startschedule, endschedule, cost_estimate, statusurgencyid, note}
            For n As Integer = 0 To TbInsTrackingwork.Length - 1
                If Not TbInsTrackingwork(n) Is Nothing Then
                    _SQL &= StrTbTrackingwork(n) & "=N'" & TbInsTrackingwork(n) & "',"
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
                                      , ByVal statusurgencyid As String, ByVal note As String, ByVal key As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim license_id As Integer = objDB.SelectSQL("SELECT * FROM [dbo].[license] where number_car = '" & number_car & "'", cn).Rows(0).Item("license_id")
            Dim _SQL As String = "INSERT INTO [trackingwork] ([license_id],[number_car],[expiredate],[trackinglistid],[detail],[operator_e],[agencycontact],[startschedule],[endschedule],[cost_estimate],[statusurgencyid],[note],[create_date],[create_by_user_id])"
            _SQL &= " VALUES (" & IIf(license_id.ToString Is Nothing, 0, license_id.ToString) & ","
            _SQL &= "N'" & IIf(number_car Is Nothing, 0, number_car) & "',"
            _SQL &= "N'" & IIf(expiredate Is Nothing, 0, expiredate) & "',"
            _SQL &= "N'" & IIf(trackinglistid Is Nothing, String.Empty, trackinglistid) & "',"
            _SQL &= "N'" & IIf(detail Is Nothing, String.Empty, detail) & "',"
            _SQL &= "N'" & IIf(operator_e Is Nothing, String.Empty, operator_e) & "',"
            _SQL &= "N'" & IIf(agencycontact Is Nothing, String.Empty, agencycontact) & "',"
            _SQL &= "N'" & IIf(startschedule Is Nothing, 0, startschedule) & "',"
            _SQL &= "N'" & IIf(endschedule Is Nothing, 0, endschedule) & "',"
            _SQL &= "N'" & IIf(cost_estimate Is Nothing, String.Empty, cost_estimate) & "',"
            _SQL &= "N'" & IIf(statusurgencyid Is Nothing, String.Empty, statusurgencyid) & "',"
            _SQL &= "N'" & IIf(note Is Nothing, String.Empty, note) & "',"
            _SQL &= "getdate(),"
            _SQL &= Session("UserId") & ")"
            If Not number_car Is Nothing Then
                If objDB.ExecuteSQL(_SQL, cn) Then
                    DtJson.Rows.Add("1")
                Else
                    DtJson.Rows.Add("0")
                End If
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

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