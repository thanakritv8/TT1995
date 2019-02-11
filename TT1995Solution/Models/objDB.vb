Imports System.Data.SqlClient

Public Class objDB
    ''' <summary>
    ''' Ex. Connect(IPServer,Username,Password)
    ''' </summary>
    ''' <param name="strIp"></param>
    ''' <param name="strUser"></param>
    ''' <param name="strPass"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ConnectDB(ByVal strIp As String, ByVal strUser As String, ByVal strPass As String, ByVal DataBase As String) As SqlConnection
        Dim conDb_form As String = "Server={0};Initial Catalog={3};UID={1};PASSWORD={2};Max Pool Size=4000;Connect Timeout=600;Trusted_Connection=False;"
        Dim conDb As String = String.Format(conDb_form, strIp, strUser, strPass, DataBase)
        Return New SqlConnection(conDb)
    End Function

    ''' <summary>
    ''' When close program please disconnect 
    ''' </summary>
    ''' <param name="conn"></param>
    ''' <remarks></remarks>
    Public Shared Sub DisconnectDB(ByVal conn As SqlConnection)
        conn.Close()
    End Sub

    ''' <summary>
    ''' Select return datatable
    ''' </summary>
    ''' <param name="_SQL"></param>
    ''' <param name="conn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SelectSQL(ByVal _SQL As String, ByVal conn As SqlConnection) As DataTable
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim cmd As New SqlCommand(_SQL, conn)
        Dim da As New SqlDataAdapter(cmd)
        Try
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Return New DataTable
        End Try
    End Function

    ''' <summary>
    ''' Ex. Insert, Update, Delete, After etc.
    ''' </summary>
    ''' <param name="_SQL"></param>
    ''' <param name="conn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ExecuteSQL(ByVal _SQL As String, ByVal conn As SqlConnection) As Object
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim cmd As New SqlCommand(_SQL, conn)
        Try
            Dim Status As Boolean = cmd.ExecuteNonQuery()
            Return Status
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function ExecuteSQLReturnId(ByVal _SQL As String, ByVal conn As SqlConnection) As Integer
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim cmd As New SqlCommand(_SQL, conn)
        Try
            Dim Status As Integer = cmd.ExecuteScalar()
            Return Status
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class
