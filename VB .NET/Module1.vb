Imports System
Imports SkebbyGW

Module Module1

    Sub Main()
        Dim sms = New skebbyGW.skebbyAPI("username", "password")
        Dim recipients(0) As String

        recipients(0) = "391234567890"
        
        'Dim credit As Dictionary(Of String, String) = sms.getCredit()
        Dim result As Dictionary(Of String, String) = sms.sendSMS("test classe skebbyAPI via VB.NET", recipients)
        
        For Each item As KeyValuePair(Of String, String) In result
            Console.WriteLine(item.Key & ": " & item.Value)
        Next
        Console.ReadLine()
    End Sub

End Module
