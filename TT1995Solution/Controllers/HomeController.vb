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
                Return View("../Home/License")
            Else
                Return View("../Account/Login")
            End If
        End Function



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

        Public Function GetLicense() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT [license_id],[number_car],[license_car],[province],[type_fuel],[type_car],[style_car],[brand_car],[model_car],[color_car],[number_body],[number_engine],[number_engine_point_1],[number_engine_point_2],[brand_engine],[pump],[horse_power],[shaft],[wheel],[tire],[license_date],[weight_car],[weight_lade],[weight_total],[ownership],[transport_operator],[transport_type],FORMAT([create_date], 'yyyy-MM-dd'),[create_by_user_id],[update_date],[update_by_user_id] FROM [license] ORDER BY number_car"
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
                                      , ByVal weight_total As String, ByVal ownership As String, ByVal transport_operator As String, ByVal transport_type As String, ByVal key As String) As String

            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [license] SET "
            Dim StrTbLicense() As String = {"number_car", "license_car", "province", "type_fuel", "type_car", "style_car", "brand_car", "model_car", "color_car", "number_body", "number_engine", "number_engine_point_1", "number_engine_point_2", "brand_engine", "pump", "horse_power", "shaft", "wheel", "tire", "license_date", "weight_car", "weight_lade", "weight_total", "ownership", "transport_operator", "transport_type"}
            Dim TbLicense() As Object = {number_car, license_car, province, type_fuel, type_car, style_car, brand_car, model_car, color_car, number_body, number_engine, number_engine_point_1, number_engine_point_2, brand_engine, pump, horse_power, shaft, wheel, tire, license_date, weight_car, weight_lade, weight_total, ownership, transport_operator, transport_type}
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
                                      , ByVal weight_total As String, ByVal ownership As String, ByVal transport_operator As String, ByVal transport_type As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO [license] ([number_car],[license_car],[province],[type_fuel],[type_car],[style_car],[brand_car],[model_car],[color_car],[number_body],[number_engine],[number_engine_point_1],[number_engine_point_2],[brand_engine],[pump],[horse_power],[shaft],[wheel],[tire],[license_date],[weight_car],[weight_lade],[weight_total],[ownership],[transport_operator],[transport_type],[create_by_user_id])"
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
                Dim PathNew As String = String.Empty
                For i As Integer = 0 To ArrPath.Length - 2
                    If i = ArrPath.Length - 2 Then
                        PathNew &= ArrPath(i) & "/" & NewNameFull
                    Else
                        PathNew &= ArrPath(i) & "/"
                    End If
                Next

                Dim _SQL As String = "Update files_all Set name_file = N'" & NewNameFull & "', expanded = 1 WHERE file_id = '" & file_id & "'"
                If objDB.ExecuteSQL(_SQL, cn) Then
                    _SQL = "Update files_all SET path_file = REPLACE(path_file,N'" & _Path & "', N'" & PathNew & "') WHERE path_file like N'%" & _Path & "%'"
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

        Public Function GetTax() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT t.tax_id,t.tax_expire,t.tax_startdate,t.tax_rate,t.tax_status,l.number_car, l.license_car, l.license_id FROM tax as t join license as l on t.license_id = l.license_id"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateTax(ByVal number_car As String, ByVal license_car As String, ByVal province As String _
                                      , ByVal type_fuel As String, ByVal type_car As String, ByVal style_car As String _
                                      , ByVal brand_car As String, ByVal model_car As String, ByVal color_car As String _
                                      , ByVal number_body As String, ByVal number_engine As String, ByVal number_engine_point_1 As String _
                                      , ByVal number_engine_point_2 As String, ByVal brand_engine As String, ByVal pump As String _
                                      , ByVal horse_power As String, ByVal shaft As String, ByVal wheel As String, ByVal tire As String _
                                      , ByVal license_date As String, ByVal weight_car As String, ByVal weight_lade As String _
                                      , ByVal weight_total As String, ByVal ownership As String, ByVal transport_operator As String, ByVal transport_type As String, ByVal key As String) As String

            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [license] SET "
            Dim StrTbLicense() As String = {"number_car", "license_car", "province", "type_fuel", "type_car", "style_car", "brand_car", "model_car", "color_car", "number_body", "number_engine", "number_engine_point_1", "number_engine_point_2", "brand_engine", "pump", "horse_power", "shaft", "wheel", "tire", "license_date", "weight_car", "weight_lade", "weight_total", "ownership", "transport_operator", "transport_type"}
            Dim TbLicense() As Object = {number_car, license_car, province, type_fuel, type_car, style_car, brand_car, model_car, color_car, number_body, number_engine, number_engine_point_1, number_engine_point_2, brand_engine, pump, horse_power, shaft, wheel, tire, license_date, weight_car, weight_lade, weight_total, ownership, transport_operator, transport_type}
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

        Public Function InsertTax(ByVal license_car As String, ByVal number_car As String, ByVal tax_expire As DateTime, ByVal tax_rate As String, ByVal tax_startdate As DateTime, ByVal tax_status As String) As String

            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO tax ()"

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

        Public Function InsertFileTax() As String
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
                        Dim pathServer As String = Server.MapPath("~/Files/Tax/Root/" & fk_id & "/" & IIf(_Path = String.Empty, String.Empty, _Path & "/") & newFolder)
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
                Dim PathNew As String = String.Empty
                For i As Integer = 0 To ArrPath.Length - 2
                    If i = ArrPath.Length - 2 Then
                        PathNew &= ArrPath(i) & "/" & NewNameFull
                    Else
                        PathNew &= ArrPath(i) & "/"
                    End If
                Next

                Dim _SQL As String = "Update files_all Set name_file = N'" & NewNameFull & "', expanded = 1 WHERE file_id = '" & file_id & "'"
                If objDB.ExecuteSQL(_SQL, cn) Then
                    _SQL = "Update files_all SET path_file = REPLACE(path_file,N'" & _Path & "', N'" & PathNew & "') WHERE path_file like N'%" & _Path & "%'"
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

#End Region

#Region "Officer Records"
        Function OfficerRecords() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View("../Home/OfficerRecords")
            Else
                Return View("../Account/Login")
            End If
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

#Region "Utility"
        Public Function GetColumnChooser(ByVal table_id As Integer) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            'Dim _SQL As String = "SELECT cc1.column_id, cc1.name_column AS dataField, cc1.display AS caption, cc1.data_type AS dataType, cc1.alignment, cc1.width, ISNULL(cc2.visible,0) AS visible FROM [config_column] AS cc1 LEFT JOIN [chooser_column] AS cc2 ON cc1.column_id = cc2.column_id WHERE cc2.user_id = " & Session("UserId")
            'Dim _SQL As String = "SELECT column_id, name_column AS dataField, display AS caption, data_type AS dataType, alignment, width, ISNULL(visible,0) AS visible, fixed, format, colSpan FROM [config_column] WHERE name_column <> 'license_id' ORDER BY sort ASC"
            Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(cc.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id WHERE cc.name_column <> 'license_id' AND table_id = " & table_id & " ORDER BY cc.sort ASC"
            Dim DtLicense As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtLicense.Rows Select DtLicense.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
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

        Public Function GetFiles() As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT * FROM [files_all] WHERE table_id = 1"
            Dim DtFiles As DataTable = objDB.SelectSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtFiles.Rows Select DtFiles.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
#End Region

    End Class
End Namespace