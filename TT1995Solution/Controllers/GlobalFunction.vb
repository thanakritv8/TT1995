Imports System.Data.SqlClient
Imports System.Web.Script.Serialization
Imports System.IO
Public Class GlobalFunction
    Function GetData(ByVal SQL As String) As String
        Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        Dim _SQL As String = SQL
        Dim Dt As DataTable = objDB.SelectSQL(_SQL, cn)
        objDB.DisconnectDB(cn)
        Return New JavaScriptSerializer().Serialize(From dr As DataRow In Dt.Rows Select Dt.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
    End Function

    Public Function GetColumnChooser(ByVal gbTableId As Integer) As String
        Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        Dim _SQL As String = "SELECT distinct(cc.sort), cc.column_id, cc.name_column AS dataField, cc.display AS caption, cc.data_type AS dataType, cc.alignment, cc.width, ISNULL(ccd.visible,0) AS visible, cc.fixed, cc.format, cc.colSpan, isnull(lu.column_id, 0) as status_lookup ,cc.group_field,cc.placeholder,cc.location
                              FROM config_column AS cc LEFT JOIN lookup AS lu ON cc.column_id = lu.column_id inner join config_column_data AS ccd on ccd.cc_id = cc.column_id
                              WHERE table_id = " & gbTableId & " and user_id = " & HttpContext.Current.Session("UserId") & "
                              ORDER BY cc.sort ASC"
        Dim Dt As DataTable = objDB.SelectSQL(_SQL, cn)
        objDB.DisconnectDB(cn)
        Return New JavaScriptSerializer().Serialize(From dr As DataRow In Dt.Rows Select Dt.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
    End Function

    Public Function GetFiles(ByVal fk_id As Integer, ByVal IdTable As Integer) As String
        Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        Dim _SQL As String = "SELECT * FROM [files_all] WHERE table_id = " & IdTable & " and fk_id = " & fk_id
        Dim DtFiles As DataTable = objDB.SelectSQL(_SQL, cn)
        objDB.DisconnectDB(cn)
        Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtFiles.Rows Select DtFiles.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
    End Function

    Public Function InsertFile(ByVal Request As HttpRequestBase)
        Dim DtJson As DataTable = New DataTable
        DtJson.Columns.Add("Status")
        Try
            Dim fk_id As String = String.Empty
            Dim parentDirId As String = String.Empty
            Dim newFolder As String = String.Empty
            Dim tableId As String = String.Empty
            Dim FolderName As String = String.Empty
            If Request.Form.AllKeys.Length <> 0 Then
                For i As Integer = 0 To Request.Form.AllKeys.Length - 1
                    If Request.Form.AllKeys(i) = "fk_id" Then
                        fk_id = Request.Form(i)
                    ElseIf Request.Form.AllKeys(i) = "parentDirId" Then
                        parentDirId = Request.Form(i)
                    ElseIf Request.Form.AllKeys(i) = "newFolder" Then
                        newFolder = Request.Form(i)
                    ElseIf Request.Form.AllKeys(i) = "tableId" Then
                        tableId = Request.Form(i)
                    ElseIf Request.Form.AllKeys(i) = "tableName" Then
                        FolderName = Request.Form(i)
                    End If
                Next
                Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                If Request.Files.Count = 0 Then
                    Dim _Path As String = fnGetPath(parentDirId)
                    Dim pathServer As String = HttpContext.Current.Server.MapPath("~/Files/" & FolderName & "/Root/" & fk_id & "/" & IIf(_Path = String.Empty, String.Empty, _Path & "/") & newFolder)
                    If (Not System.IO.Directory.Exists(pathServer)) Then
                        System.IO.Directory.CreateDirectory(pathServer)
                    End If
                    Dim _SQL As String = "INSERT INTO [files_all] ([fk_id],[table_id],[name_file],[type_file],[path_file],[parentDirId],[icon],[create_by_user_id]) VALUES (" & fk_id & "," & tableId & ",N'" & newFolder & "','folder',N'','" & parentDirId & "','../Img/folder.png'," & HttpContext.Current.Session("UserId") & ")"
                    objDB.ExecuteSQL(_SQL, cn)
                Else
                    'Create File
                    For i As Integer = 0 To Request.Files.Count - 1
                        Dim file = Request.Files(i)
                        Dim _Path As String = fnGetPath(parentDirId)
                        Dim PathFile As String = "/Files/" & FolderName & "/Root/" & fk_id & "/" & IIf(_Path = String.Empty, String.Empty, _Path & "/") & file.FileName
                        Dim type_file As String = Path.GetExtension(PathFile)
                        Dim pathServer As String = HttpContext.Current.Server.MapPath("~" & PathFile)
                        Dim name_icon As String = String.Empty

                        Dim pathCheckId = HttpContext.Current.Server.MapPath("~/Files/" & FolderName & "/Root/" & fk_id)
                        If (Not System.IO.Directory.Exists(pathCheckId)) Then
                            System.IO.Directory.CreateDirectory(pathCheckId)
                        End If

                        If type_file = ".pdf" Then
                            name_icon = "pdf"
                        Else
                            name_icon = "pic"
                        End If
                        file.SaveAs(pathServer)
                        Dim _SQL As String = "INSERT INTO [files_all] ([fk_id], [table_id], [name_file], [type_file], [path_file], [parentDirId], [icon], [create_by_user_id]) VALUES (" & fk_id & "," & tableId & ", N'" & file.FileName & "','" & name_icon & "',N'.." & PathFile & "','" & parentDirId & "','../Img/" & name_icon & ".png'," & HttpContext.Current.Session("UserId") & ")"
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

    Public Function DeleteFile(ByVal keyId As String, ByVal FolderName As String) As String
        Dim DtJson As DataTable = New DataTable
        DtJson.Columns.Add("Status")
        Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        Dim _SQL As String = String.Empty
        _SQL = "SELECT * FROM files_all WHERE file_id = " & keyId
        Dim dtFKId As DataTable = objDB.SelectSQL(_SQL, cn)
        If dtFKId.Rows.Count > 0 Then
            Dim Path As String = fnGetPath(keyId)
            Path = "/Files/" & FolderName & "/Root/" & dtFKId.Rows(0)("fk_id") & "/" & Path
            Dim PathServer As String = HttpContext.Current.Server.MapPath("~" & Path)
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

    Public Function fnRename(ByVal Request As HttpRequestBase, ByVal FolderName As String) As String
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
            Dim PathServer As String = HttpContext.Current.Server.MapPath("~/Files/" & FolderName & "/Root/" & fk_id & "/" & _Path)
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
            Dim PathOld As String = ("../Files/" & FolderName & "/Root/" & fk_id & "/" & _Path)
            Dim PathNew As String = ("../Files/" & FolderName & "/Root/" & fk_id & "/")
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
                PathNew = ("../Files/" & FolderName & "/Root/" & fk_id & "/" & NewNameFull)
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

    Public Function KeepLog(ByVal ColumnName As String, ByVal Data As String, ByVal _event As String, ByVal IdTable As String, ByVal IdOfTable As String) As Boolean
        Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        If (IsDate(Data)) Then
            Data = Convert.ToDateTime(Data).ToString("MM/dd/yyyy")
        End If
        Dim _SQL As String = "INSERT INTO [dbo].[log_all] ([column_id],[_data],[_format],[_event],[_table],[id_of_table],[by_user],[_date])
							  select column_id,N'" & IIf(Data.IndexOf("'") >= 0, Data.Insert(Data.IndexOf("'") + 1, "'"), Data) & "',format,'" & _event & "'," & IdTable & "," & IdOfTable & "," & HttpContext.Current.Session("UserId") & ",getdate() from config_column where name_column = '" & ColumnName & "' and table_id = " & IdTable & ""
        Return objDB.ExecuteSQL(_SQL, cn)
    End Function
End Class