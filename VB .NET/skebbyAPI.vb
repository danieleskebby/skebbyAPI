Imports System
Imports System.IO
Imports System.Web
Imports System.Net
Imports System.Text
Imports System.Collections.Generic

Namespace skebbyGW

    Public Class skebbyAPI
        Protected Username As String
        Protected Password As String
        Protected Url As String

        Sub New(ByVal user As String, ByVal pass As String)
            MyClass.Username = WebUtility.UrlEncode(user)
            MyClass.Password = WebUtility.UrlEncode(pass)
            MyClass.Url = "http://gateway.skebby.it/api/send/smseasy/advanced/http.php"
        End Sub

        Protected Function do_post_request(ByVal data As Dictionary(Of String, String))
            Dim request As WebRequest = WebRequest.Create(MyClass.Url)
            request.Credentials = CredentialCache.DefaultCredentials
            CType(request, HttpWebRequest).UserAgent = "Skebby VB.NET Gateway"
            request.Method = "POST"

            Dim parameters As String = ""
            For Each item As KeyValuePair(Of String, String) In data
                parameters = parameters & "&" & item.Key & "=" & WebUtility.UrlEncode(item.Value)
            Next
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
            ByVal text As String, _
            ByVal recipients As String(), _
            Optional ByVal method As String = "send_sms_classic", _
            Optional ByVal sender_number As String = "", _
            Optional ByVal sender_string As String = "", _
            Optional ByVal charset As String = "", _
            Optional ByVal delivery_start As String = "", _
            Optional ByVal encoding_scheme As String = "", _
            Optional ByVal validity_period As String = "", _
            Optional ByVal user_reference As String = "" _
        )
            Dim i As Integer = 0
            Dim data As New Dictionary(Of String, String)
            data.Add("username", MyClass.Username)
            data.Add("password", MyClass.Password)
            data.Add("text", text)

            For Each item As String In recipients
                data.Add("recipients[" & i & "]", item)
                i = i + 1
            Next

            data.Add("method", method)
            If (sender_number <> "") Then
                data.Add("sender_number", sender_number)
            End If
            If (sender_string <> "") Then
                data.Add("sender_string", sender_string)
            End If
            If (charset <> "") Then
                data.Add("charset", charset)
            End If
            If (delivery_start <> "") Then
                data.Add("delivery_start", delivery_start)
            End If
            If (encoding_scheme <> "") Then
                data.Add("encoding_scheme", encoding_scheme)
            End If
            If (validity_period <> "") Then
                data.Add("validity_period", validity_period)
            End If
            If (user_reference <> "") Then
                data.Add("user_reference", user_reference)
            End If

            Return do_post_request(data)
        End Function

        Public Function getCredit(
            Optional ByVal charset As String = ""
        )
            Dim data As New Dictionary(Of String, String)
            data.Add("method", "get_credit")
            data.Add("username", MyClass.Username)
            data.Add("password", MyClass.Password)
            If (charset <> "") Then
                data.Add("charset", charset)
            End If

            Return do_post_request(data)
        End Function

        Public Function addAlias(
            ByVal alias_name As String, _
            ByVal business_name As String, _
            ByVal nation As String, _
            ByVal vat_number As String, _
            ByVal taxpayer_number As String, _
            ByVal street As String, _
            ByVal city As String, _
            ByVal postcode As String, _
            ByVal contact As String, _
            Optional ByVal charset As String = ""
        )
            Dim data As New Dictionary(Of String, String)
            data.Add("method", "add_alias")
            data.Add("username", MyClass.Username)
            data.Add("password", MyClass.Password)
            data.Add("alias", alias_name)
            data.Add("business_name", business_name)
            data.Add("nation", nation)
            data.Add("vat_number", vat_number)
            data.Add("taxpayer_number", taxpayer_number)
            data.Add("street", street)
            data.Add("city", city)
            data.Add("postcode", postcode)
            data.Add("contact", contact)
            If (charset <> "") Then
                data.Add("charset", charset)
            End If

            Return do_post_request(data)
        End Function
    End Class
End Namespace
