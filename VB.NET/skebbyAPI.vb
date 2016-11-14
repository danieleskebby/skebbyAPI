Imports System
Imports System.IO
Imports System.Web
Imports System.Net
Imports System.Text
Imports System.Collections
Imports System.Collections.Generic

Namespace skebbyGW

    Public Class skebbyAPI
        Protected Username As String
        Protected Password As String
        Protected Url As String

        Sub New(ByVal user As String, ByVal pass As String)
            MyClass.Username = WebUtility.UrlEncode(user)
            MyClass.Password = WebUtility.UrlEncode(pass)
            MyClass.Url = "http://api.skebby.it/api/send/smseasy/advanced/http.php"
        End Sub

        Protected Function doPostRequest(ByVal data As Dictionary(Of String, Object))
            data.Add("username", MyClass.Username)
            data.Add("password", MyClass.Password)
            If Not data.ContainsKey("charset") Then
                data.Add("charset", "UTF-8")
            End If

            Dim request As WebRequest = WebRequest.Create(MyClass.Url)
            request.Credentials = CredentialCache.DefaultCredentials
            CType(request, HttpWebRequest).UserAgent = "Skebby VB.NET Gateway"
            request.Method = "POST"

            Dim parameters As String = ""

            For Each item As KeyValuePair(Of String, Object) In data
                If item.Key = "recipients" Then
                    If TypeOf item.Value Is List(Of Dictionary(Of String, String)) Then
                        For i As Integer = 0 To (item.Value.Count - 1)
                            For Each rec As KeyValuePair(Of String, String) In item.Value(i)
                                parameters = parameters & "&recipients[" & i & "][" & WebUtility.UrlEncode(rec.Key) & "]=" & WebUtility.UrlEncode(rec.Value)
                            Next
                        Next
                    Else
                        For Each number As String In item.Value
                            parameters = parameters & "&recipients[]=" & WebUtility.UrlEncode(number)
                        Next
                    End If
                Else
                    parameters = parameters & "&" & WebUtility.UrlEncode(item.Key) & "=" & WebUtility.UrlEncode(item.Value)
                End If
            Next
            Console.WriteLine(parameters)
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(parameters)

            request.ContentType = "application/x-www-form-urlencoded"
            request.ContentLength = byteArray.Length

            Dim dataStream As Stream = request.GetRequestStream()

            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()

            Dim response As WebResponse = request.GetResponse()

            dataStream = response.GetResponseStream()

            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()

            reader.Close()
            dataStream.Close()
            response.Close()

            Dim result As Dictionary(Of String, String) = New Dictionary(Of String, String)

            If responseFromServer.Length <> 0 Then
                Dim values = responseFromServer.Split(New Char() {"&"c})
                For Each value In values
                    Dim temp = value.Split(New Char() {"="c})
                    result.Add(WebUtility.UrlDecode(temp(0)), WebUtility.UrlDecode(temp(1)))
                Next
            Else
                result.Add("result", "fail")
            End If

            Return result

        End Function

        Public Function sendSMS(
            ByVal input As Dictionary(Of String, Object)
        )
            If Not input.ContainsKey("method") Then
                input.Add("method", "send_sms_classic")
            End If
            Return doPostRequest(input)
        End Function

        Public Function getCredit(
            Optional ByVal charset As String = ""
        )
            Dim input As New Dictionary(Of String, Object)
            input.Add("method", "get_credit")
            If (charset <> "") Then
                input.Add("charset", charset)
            End If

            Return doPostRequest(input)
        End Function

        Public Function addAlias(
            ByVal input As Dictionary(Of String, Object)
        )
            input.Add("method", "add_alias")

            Return doPostRequest(input)
        End Function

        Public Sub printResult(ByVal result As Dictionary(Of String, String))
            For Each item As KeyValuePair(Of String, String) In result
                Console.WriteLine(item.Key & ": " & item.Value)
            Next
        End Sub
    End Class
End Namespace