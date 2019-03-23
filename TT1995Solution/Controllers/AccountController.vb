Imports System.Data.SqlClient
Imports System.Web.Mvc
Imports System.Web.Script.Serialization

Namespace Controllers
    Public Class AccountController
        Inherits Controller

        ' GET: Account
        Function Index() As ActionResult
            Return View()
        End Function

        Function Login() As ActionResult
            'Dim Pass As String = EncryptSHA256Managed("user01")
            If Session("StatusLogin") = "1" Then
                Return View("../Home/Index")
            Else
                Return View()
            End If
        End Function

        Function Logout() As ActionResult
            Session.Abandon()
            Return Redirect("../Account/Login")
        End Function

        Public Function EncryptSHA256Managed(ByVal StrInput As String) As String
            Dim uEncode As New UnicodeEncoding()
            Dim bytClearString() As Byte = uEncode.GetBytes(StrInput)
            Dim sha As New _
            System.Security.Cryptography.SHA256Managed()
            Dim hash() As Byte = sha.ComputeHash(bytClearString)
            Return Convert.ToBase64String(hash)
        End Function

        Public Function CheckLogin(ByVal Username As String, ByVal Password As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT * FROM [account] WHERE username = '" & Username & "' AND password = '" & EncryptSHA256Managed(Password) & "'"
            Dim DtAccount As DataTable = objDB.SelectSQL(_SQL, cn)
            If DtAccount.Rows.Count > 0 Then
                Session("StatusLogin") = "1"
                Session("UserId") = DtAccount.Rows(0)("user_id")
                Session("GroupId") = DtAccount.Rows(0)("group_id")
                Session("FirstName") = DtAccount.Rows(0)("firstname")
                _SQL = "select ct.table_id as application_id, permission_status from config_table as ct left join permission as p on ct.table_id = p.application_id and p.group_id = " & DtAccount.Rows(0)("group_id")
                Dim DtPermission As DataTable = objDB.SelectSQL(_SQL, cn)
                For Each _Item In DtPermission.Rows
                    Session(_Item("application_id").ToString) = IIf(Not IsDBNull(_Item("permission_status")), _Item("permission_status"), 0)
                Next
            Else
                Session("StatusLogin") = "0"
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtAccount.Rows Select DtAccount.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

#Region "Config"

        Function Permission() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function GetUsername() As String
            Dim dtUser As DataTable = New DataTable
            Using cn = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                Dim _SQL As String = "SELECT user_id, username FROM [account] WHERE ORDER BY Username ASC"
                dtUser = objDB.SelectSQL(_SQL, cn)
            End Using
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtUser.Rows Select dtUser.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetApplication() As String
            Dim dtApp As DataTable = New DataTable
            Using cn = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                Dim _SQL As String = "SELECT table_id as application_id, display as application_name FROM config_table"
                dtApp = objDB.SelectSQL(_SQL, cn)
            End Using
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtApp.Rows Select dtApp.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetGroup() As String
            Dim dtApp As DataTable = New DataTable
            Using cn = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                Dim _SQL As String = "SELECT group_id, name as group_name, remark FROM [group]"
                dtApp = objDB.SelectSQL(_SQL, cn)
            End Using
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtApp.Rows Select dtApp.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function GetPermission() As String
            Dim dtApp As DataTable = New DataTable
            Using cn = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                Dim _SQL As String = "SELECT * FROM permission"
                dtApp = objDB.SelectSQL(_SQL, cn)
            End Using
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtApp.Rows Select dtApp.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function InsertPermission(ByVal group_id As String, ByVal application_id As String, ByVal permission_status As String) As String
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO permission (group_id, application_id, permission_status, create_by_user_id) OUTPUT Inserted.permission_id VALUES "
            _SQL &= "('" & group_id & "', '" & application_id & "', '" & permission_status & "', '" & Session("UserId") & "')"
            If Not group_id Is Nothing And Not application_id Is Nothing And Not permission_status Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeletePermission(ByVal keyId As String) As String
            Dim dtStatus As DataTable = New DataTable
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            cn.Open()
            Dim _SQL As String = "delete [permission] where permission_id = " & keyId
            objDB.ExecuteSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            dtStatus.Columns.Add("Status")
            dtStatus.Rows.Add("1")
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtStatus.Rows Select dtStatus.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        '###################################################################################################

        Function Account() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function
        Public Function GetAccount() As String
            Dim dtApp As DataTable = New DataTable
            Using cn = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
                Dim _SQL As String = "SELECT user_id, username, N'●●●●●●●●●●●●●●' as password, tel, address, firstname, lastname, group_id FROM account"
                dtApp = objDB.SelectSQL(_SQL, cn)
            End Using
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtApp.Rows Select dtApp.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function
        Public Function InsertAccount(ByVal username As String, ByVal password As String, ByVal tel As String, ByVal address As String, ByVal firstname As String, ByVal lastname As String, ByVal group_id As String) As String
            tel = IIf(tel Is Nothing, String.Empty, tel)
            address = IIf(address Is Nothing, String.Empty, address)
            firstname = IIf(firstname Is Nothing, String.Empty, firstname)
            lastname = IIf(lastname Is Nothing, String.Empty, lastname)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO account (username, password, tel, address, firstname, lastname, group_id, create_by_user_id) OUTPUT Inserted.user_id VALUES "
            _SQL &= "('" & username & "', '" & EncryptSHA256Managed(password) & "', '" & tel & "', '" & address & "', '" & firstname & "', '" & lastname & "', '" & group_id & "', '" & Session("UserId") & "')"
            If Not username Is Nothing And Not password Is Nothing And Not group_id Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function UpdateAccount(ByVal user_id As String, ByVal username As String, ByVal password As String, ByVal tel As String, ByVal address As String, ByVal firstname As String, ByVal lastname As String, ByVal group_id As String) As String
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim _SQL As String = "UPDATE [account] SET "
            Dim StrTbLc() As String = {"username", "password", "tel", "address", "firstname", "lastname", "group_id"}
            Dim TbLc() As Object = {username, password, tel, address, firstname, lastname, group_id}
            For n As Integer = 0 To TbLc.Length - 1
                If Not TbLc(n) Is Nothing Then
                    If StrTbLc(n) = "password" Then
                        _SQL &= StrTbLc(n) & "=N'" & EncryptSHA256Managed(TbLc(n)) & "',"
                    Else
                        _SQL &= StrTbLc(n) & "=N'" & TbLc(n) & "',"
                    End If


                End If
            Next
            _SQL &= "update_date = GETDATE(), update_by_user_id = " & Session("UserId") & " WHERE user_id = " & user_id
            If objDB.ExecuteSQL(_SQL, cn) Then
                DtJson.Rows.Add("1")
            Else
                DtJson.Rows.Add("0")
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteAccount(ByVal keyId As String) As String
            Dim dtStatus As DataTable = New DataTable
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            cn.Open()
            Dim _SQL As String = "delete [account] where user_id = " & keyId
            objDB.ExecuteSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            dtStatus.Columns.Add("Status")
            dtStatus.Rows.Add("1")
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtStatus.Rows Select dtStatus.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Function Group() As ActionResult
            If Session("StatusLogin") = "1" Then
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function InsertGroup(ByVal group_name As String, ByVal remark As String) As String
            If Not remark Is Nothing Then
                remark = String.Empty
            End If
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "INSERT INTO [group] (name, remark, create_by_user_id) OUTPUT Inserted.group_id VALUES "
            _SQL &= "('" & group_name & "', '" & remark & "', '" & Session("UserId") & "')"
            If Not group_name Is Nothing Then
                DtJson.Rows.Add(objDB.ExecuteSQLReturnId(_SQL, cn))
            Else
                DtJson.Rows.Add("กรุณากรอกข้อมูลให้ถูกต้อง")
            End If

            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtJson.Rows Select DtJson.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Public Function DeleteGroup(ByVal keyId As String) As String
            Dim dtStatus As DataTable = New DataTable
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            cn.Open()
            Dim _SQL As String = "delete [group] where group_id = " & keyId
            objDB.ExecuteSQL(_SQL, cn)
            objDB.DisconnectDB(cn)
            dtStatus.Columns.Add("Status")
            dtStatus.Rows.Add("1")
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtStatus.Rows Select dtStatus.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

        Function Settings() As ActionResult
            Dim HC As HomeController = New HomeController
            If Session("StatusLogin") = "1" Then
                'HC.SetDataOfConfigColumnData()
                Return View()
            Else
                Return View("../Account/Login")
            End If
        End Function

        Public Function ChangePassword(ByVal Password As String, ByVal Old_Password As String)
            Dim DtJson As DataTable = New DataTable
            DtJson.Columns.Add("Status")
            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
            Dim _SQL As String = "SELECT * FROM [account] WHERE user_id = '" & Session("UserId") & "' AND password = '" & EncryptSHA256Managed(Old_Password) & "'"
            Dim DtAccount As DataTable = objDB.SelectSQL(_SQL, cn)
            If DtAccount.Rows.Count > 0 Then
                Dim _SQL_U As String = "UPDATE [account] SET password = '" & EncryptSHA256Managed(Password) & "' where user_id =" & Session("UserId")
                If objDB.ExecuteSQL(_SQL_U, cn) Then
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
#End Region

        '#Region "Account"

        '        Public Function InsertPermission(ByVal AppId As String, ByVal AccessId As String, ByVal UserId As String) As String
        '            Dim dtStatus As DataTable = New DataTable
        '            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        '            cn.Open()
        '            Dim _SQL As String = "insert into [Auth].[dbo].[Permission] (AppId,AccessId,UserId) values (" & AppId & ", " & AccessId & ", " & UserId & ")"
        '            objDB.ExecuteSQL(_SQL, cn)
        '            objDB.DisconnectDB(cn)
        '            dtStatus.Columns.Add("Status")
        '            dtStatus.Rows.Add("OK")
        '            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtStatus.Rows Select dtStatus.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        '        End Function

        '        Public Function DeletePermission(ByVal PerId As String) As String
        '            Dim dtStatus As DataTable = New DataTable
        '            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        '            cn.Open()
        '            Dim _SQL As String = "delete [Auth].[dbo].[Permission] where PermissionId = " & PerId
        '            objDB.ExecuteSQL(_SQL, cn)
        '            objDB.DisconnectDB(cn)
        '            dtStatus.Columns.Add("Status")
        '            dtStatus.Rows.Add("OK")
        '            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtStatus.Rows Select dtStatus.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        '        End Function

        '        'Public Sub GetUsers()
        '        '    Dim searcher As DirectorySearcher = New DirectorySearcher("(&(objectCategory=person)(objectClass=user))")
        '        '    searcher.SearchScope = System.DirectoryServices.SearchScope.Subtree
        '        '    searcher.PageSize = 1000
        '        '    Dim results As SearchResultCollection = searcher.FindAll()
        '        '    Dim b = ""
        '        '    For Each result As SearchResult In results
        '        '        Dim a = result.Properties("samaccountname")
        '        '        Dim p = result.Properties("memberof")
        '        '        b &= a(0) & ","
        '        '        If a(0) = "Thanakrit.J" Then
        '        '            Dim f = 0
        '        '        End If
        '        '    Next
        '        '    Dim x() = b.Split(",")
        '        '    Dim z = 0
        '        'End Sub

        '        Function Account() As ActionResult
        '            If Session("StatusLogin") = "OK" Then
        '                Return View()
        '            Else
        '                Return View("../Account/Login")
        '            End If
        '        End Function

        '        Function Setting() As ActionResult
        '            If Session("StatusLogin") = "OK" Then
        '                Return View()
        '            Else
        '                Return View("../Account/Login")
        '            End If
        '        End Function

        '        Public Function UpDateAccount() As String
        '            Dim name As String = String.Empty
        '            Dim username As String = String.Empty
        '            Dim password As String = String.Empty
        '            Dim department As String = String.Empty
        '            Dim sections As String = String.Empty
        '            Dim email As String = String.Empty
        '            Dim userid As Integer = 0

        '            For i As Integer = 0 To Request.Form.AllKeys.Length - 1
        '                If Request.Form.AllKeys(i) = "name" Then
        '                    name = Request.Form(i)
        '                ElseIf Request.Form.AllKeys(i) = "sections" Then
        '                    sections = Request.Form(i)
        '                ElseIf Request.Form.AllKeys(i) = "username" Then
        '                    username = Request.Form(i)
        '                ElseIf Request.Form.AllKeys(i) = "password" Then
        '                    password = Request.Form(i)
        '                ElseIf Request.Form.AllKeys(i) = "department" Then
        '                    department = Request.Form(i)
        '                ElseIf Request.Form.AllKeys(i) = "email" Then
        '                    email = Request.Form(i)
        '                ElseIf Request.Form.AllKeys(i) = "userid" Then
        '                    userid = Request.Form(i)
        '                End If
        '            Next
        '            Using cn = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        '                cn.Open()
        '                Dim _SQL As String = "update [Auth].[dbo].[Account] set [Name] = '" & name & "',[Username] = '" & username & "',[Password] = '" & password & "',[Department] = '" & department & "',[Sections] = '" & sections & "',[Email] = '" & email & "' where UserId = " & userid
        '                objDB.ExecuteSQL(_SQL, cn)
        '                objDB.DisconnectDB(cn)
        '            End Using
        '            Dim dtStatus As DataTable = New DataTable
        '            dtStatus.Columns.Add("Status")
        '            dtStatus.Rows.Add("OK")
        '            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtStatus.Rows Select dtStatus.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        '        End Function

        '        Public Function GetApp(ByVal UserId As String) As String
        '            Dim dtApp As DataTable = New DataTable
        '            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        '            cn.Open()
        '            Dim _SQL As String = " select * from [Auth].[dbo].[Application] where appId not in (select AppId from [Auth].[dbo].[Permission] where UserId = '" & UserId & "')"
        '            dtApp = objDB.SelectSQL(_SQL, cn)
        '            objDB.DisconnectDB(cn)
        '            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtApp.Rows Select dtApp.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        '        End Function

        '        Public Function GetAccount() As String
        '            Dim dtAcc As DataTable = New DataTable
        '            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        '            cn.Open()
        '            'Dim _SQL As String = "SELECT u.UserId, u.Firstname, u.Lastname, u.Username, u.Department, u.email, u.IsActive, g.nameGroup, u.createBy, Format(u.createDate, 'yyyy-MM-dd HH:mm:ss') as createDate FROM [management].[dbo].[UserProfile] AS u join [management].[dbo].[Group] AS g on u.GroupId = g.GroupId"
        '            'Dim _SQL As String = "SELECT u.UserId, u.Firstname, u.Lastname, u.Username, u.Department, u.email, u.IsActive, g.nameGroup FROM [management].[dbo].[UserProfile] AS u join [management].[dbo].[Group] AS g on u.GroupId = g.GroupId"
        '            Dim _SQL As String = "select [UserId],[Name],[Username],[Department],[Sections],[Email] from [Auth].[dbo].[Account] where Admin <> 1"

        '            dtAcc = objDB.SelectSQL(_SQL, cn)
        '            objDB.DisconnectDB(cn)
        '            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtAcc.Rows Select dtAcc.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        '        End Function

        '        Public Function InsertAccount() As String
        '            Try
        '                Dim name As String = String.Empty
        '                Dim sections As String = String.Empty
        '                Dim username As String = String.Empty
        '                Dim password As String = String.Empty
        '                Dim department As String = String.Empty
        '                Dim email As String = String.Empty
        '                Dim AppPer As String = String.Empty
        '                Dim AccPer As String = String.Empty

        '                For i As Integer = 0 To Request.Form.AllKeys.Length - 1
        '                    If Request.Form.AllKeys(i) = "name" Then
        '                        name = Request.Form(i)
        '                    ElseIf Request.Form.AllKeys(i) = "sections" Then
        '                        sections = Request.Form(i)
        '                    ElseIf Request.Form.AllKeys(i) = "username" Then
        '                        username = Request.Form(i)
        '                    ElseIf Request.Form.AllKeys(i) = "password" Then
        '                        password = Request.Form(i)
        '                    ElseIf Request.Form.AllKeys(i) = "department" Then
        '                        department = Request.Form(i)
        '                    ElseIf Request.Form.AllKeys(i) = "email" Then
        '                        email = Request.Form(i)
        '                    ElseIf Request.Form.AllKeys(i) = "appper" Then
        '                        AppPer = Request.Form(i)
        '                    ElseIf Request.Form.AllKeys(i) = "accper" Then
        '                        AccPer = Request.Form(i)
        '                    End If
        '                Next
        '                Dim arrAppPer1() As String = AppPer.Split(",")
        '                Using cn = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        '                    cn.Open()
        '                    Dim _SQL As String = "insert into [Auth].[dbo].[Account] ([Name],[Username],[Password],[Department],[Sections],[Email],[Admin]) OUTPUT Inserted.UserId values ('" & name & "', '" & username & "', '" & password & "', '" & department & "', '" & sections & "', '" & email & "',0)"
        '                    Dim dtUserId As DataTable = objDB.SelectSQL(_SQL, cn)

        '                    If dtUserId.Rows.Count > 0 Then
        '                        Dim arrAppPer() As String = AppPer.Split(",")
        '                        Dim arrAccPer() As String = AccPer.Split(",")
        '                        For i As Integer = 0 To arrAccPer.Length - 2
        '                            _SQL = "INSERT INTO [Auth].[dbo].[Permission] ([AppId],[AccessId],[UserId]) VALUES ((select AppId from [Auth].[dbo].[Application] where name = '" & arrAppPer(i) & "'), (select AccessId from [Auth].[dbo].[Access] where name = '" & arrAccPer(i) & "'), " & dtUserId.Rows(0)("UserId") & ")"
        '                            objDB.ExecuteSQL(_SQL, cn)
        '                        Next
        '                    End If

        '                    objDB.DisconnectDB(cn)
        '                End Using
        '                Dim dtStatus As DataTable = New DataTable
        '                dtStatus.Columns.Add("Status")
        '                dtStatus.Rows.Add("OK")

        '            Catch ex As Exception
        '                Using cn = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        '                    cn.Open()
        '                    Dim _SQL As String = "INSERT INTO [management].[dbo].[log] (logdetail) VALUES('" & ex.Message & "')"
        '                    objDB.ExecuteSQL(_SQL, cn)
        '                    objDB.DisconnectDB(cn)
        '                End Using
        '            End Try
        '            Dim dt As DataTable = New DataTable
        '            dt.Columns.Add("Status")
        '            dt.Rows.Add("OK")
        '            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dt.Rows Select dt.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        '        End Function

        '        Public Function GetAccountWithUserId(ByVal UserId As Integer) As String
        '            Dim dtGroup As DataTable = New DataTable
        '            Dim cn As SqlConnection = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        '            cn.Open()
        '            'Dim _SQL As String = "SELECT * FROM [management].[dbo].[UserProfile] WHERE UserId = " & UserId
        '            Dim _SQL As String = "SELECT * FROM [Auth].[dbo].[Account] WHERE UserId = " & UserId
        '            dtGroup = objDB.SelectSQL(_SQL, cn)
        '            objDB.DisconnectDB(cn)
        '            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtGroup.Rows Select dtGroup.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        '        End Function

        '        Public Function DelAccount() As String
        '            Dim UserId As Integer = 0
        '            For i As Integer = 0 To Request.Form.AllKeys.Length - 1
        '                If Request.Form.AllKeys(i) = "UserId" Then
        '                    UserId = Request.Form(i)
        '                End If
        '            Next
        '            Using cn = objDB.ConnectDB(My.Settings.NameServer, My.Settings.Username, My.Settings.Password, My.Settings.DataBase)
        '                cn.Open()
        '                Dim _SQL As String = "DELETE [Auth].[dbo].[Account] WHERE UserId = " & UserId
        '                'Dim _SQL As String = "DELETE [management].[dbo].[UserProfile] WHERE UserId = " & UserId
        '                objDB.ExecuteSQL(_SQL, cn)
        '                objDB.DisconnectDB(cn)
        '            End Using
        '            Dim dtStatus As DataTable = New DataTable
        '            dtStatus.Columns.Add("Status")
        '            dtStatus.Rows.Add("OK")
        '            Return New JavaScriptSerializer().Serialize(From dr As DataRow In dtStatus.Rows Select dtStatus.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        '        End Function

        '#End Region



    End Class
End Namespace