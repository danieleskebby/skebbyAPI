Imports System
Imports SkebbyGW

Module Module1

    Sub Main()
        Dim sms = New skebbyGW.skebbyAPI("username", "password")
        Dim data As Dictionary(Of String, Object)
        Dim recipients(0) As String

        recipients(0) = "391234567890"
        
        data.Add("text","It's easy to send a message")
        data.Add("recipients",recipients)
        
        'Dim credit As Dictionary(Of String, String) = sms.getCredit()
        'Dim alias As Dictionary(Of String, String) = sms.addAlias( data )
        Dim result As Dictionary(Of String, String) = sms.sendSMS( data )
        
        sms.printResult(result)
        
        Console.ReadLine()
    End Sub

End Module
