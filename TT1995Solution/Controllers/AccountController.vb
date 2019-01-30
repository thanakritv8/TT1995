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
            Return View("../Account/Login")
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
            Else
                Session("StatusLogin") = "0"
            End If
            objDB.DisconnectDB(cn)
            Return New JavaScriptSerializer().Serialize(From dr As DataRow In DtAccount.Rows Select DtAccount.Columns.Cast(Of DataColumn)().ToDictionary(Function(col) col.ColumnName, Function(col) dr(col)))
        End Function

    End Class
End Namespace