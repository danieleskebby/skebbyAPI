# VB .NET skebby API SDK

This is the VB .NET version of Skebby API SDK.

## Usage

Here's the examples that uses the functions inside the Class

### Inclusion and class initialization
In your project, import the file skebbyAPI.cs or put it in the same directory of your main project file

```VisualBasic
' Import the required libraries
Imports System
' Import the skebbAPI SDK using its namespace
Imports SkebbyGW

Module Module1

    Sub Main()
    
        ' Initialize the Class by entering your Skebby credentials
    	Dim sms = New skebbyGW.skebbyAPI("username", "password")
	
    End Sub

End Module
```

### Send a SMS message
```VisualBasic
' set the recipients
Dim recipients(0) As String = "391234567890"

' Call the sendSMS function
Dim send As Dictionary(Of String, String) = sms.sendSMS("test classe skebbyAPI via VB.NET", recipients)

' print the result
sms.printResult(send)
```

### Check available credit
```VisualBasic
' Call the getCredit function
Dim credit As Dictionary(Of String, String) = sms.getCredit();

' print the result
sms.printResult(credit)
```

### Register an Alphanumeric Sender (Alias)
```VisualBasic
' Call the addAlias function
Dim alias_string As Dictionary(Of String, String) = sms.addAlias( "Skebby", "Skebby", "IT", "111222333444", "111222333444", "Via Melzo 12", "Milano", "20100", "contact@email.com" )

' print the result
sms.printResult(alias_string)
```