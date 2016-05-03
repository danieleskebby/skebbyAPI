# VB .NET skebby API SDK

This is the VB .NET version of Skebby API SDK.

## Usage

Here's the examples that uses the functions inside the Class

### Inclusion and class initialization
In your project, import the file skebbyAPI.cs or put it in the same directory of your main project file

```vbnet
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
```vbnet
' set the variables
Dim data As Dictionary(Of String, Object)
Dim recipients(0) As String = "391234567890"

data.Add("text","It's easy to send a message")
data.Add("recipients", recipients)

' Call the sendSMS function
Dim send As Dictionary(Of String, String) = sms.sendSMS(data)

' print the result
sms.printResult(send)
```

### Check available credit
```vbnet
' Call the getCredit function
Dim credit As Dictionary(Of String, String) = sms.getCredit();

' print the result
sms.printResult(credit)
```

### Register an Alphanumeric Sender (Alias)
```vbnet
' Set the variables
Dim data As Dictionary(Of String, String)

data.Add("alias","Skebby")
data.Add("business_name","Skebby")
data.Add("nation","IT")
data.Add("vat_number","111222333444")
data.Add("taxpayer_number","111222333444") 
data.Add("street","Via Melzo 12")
data.Add("city","Milan")
data.Add("postcode","20100")
data.Add("contact","contact@email.com")

' Call the addAlias function
Dim alias_string As Dictionary(Of String, String) = sms.addAlias(data)

' print the result
sms.printResult(alias_string)
```