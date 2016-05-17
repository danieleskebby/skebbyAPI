# C# .NET skebby API SDK

This is the C# .NET version of Skebby API SDK.

## Usage

Here's the examples that uses the functions inside the Class

### Inclusion and class initialization
In your project, import the file skebbyAPI.cs or put it in the same directory of your main project file

```C#
// Import the required libraries
using System;
using System.Collections.Generic;
// Import the skebbAPI SDK using its namespace
using SkebbyGW;

public class Program 
{ 
	public static void Main(string[] args)
	{
		// Initialize the Class by entering your Skebby credentials
		var sms = new skebbyAPI("username","password");
	}
}
```

### Send a SMS message
```C#
// Set the variables
Dictionary<string,dynamic> data = new Dictionary<string,dynamic>();

data.Add("text","It's easy to send a message :)");
data.Add("recipients", {"391234567890"});

// Call the sendSMS method
string send = sms.sendSMS(input);

// Print the result
sms.printResult(send);
```

### Check available credit
```C#
// Call the getCredit function
string credit = sms.getCredit();

// Print the result
sms.printResult(credit);
```

### Register an Alphanumeric Sender (Alias)
```C#
// Set the variables
Dictionary<string,dynamic> data = new Dictionary<string,dynamic>();

data.Add("alias","Skebby")
data.Add("business_name","Skebby")
data.Add("nation","IT")
data.Add("vat_number","111222333444")
data.Add("taxpayer_number","111222333444") 
data.Add("street","Via Melzo 12")
data.Add("city","Milan")
data.Add("postcode","20100")
data.Add("contact","contact@email.com")

// Call the addAlias function
string alias_string = sms.addAlias(data);

// Print the result
sms.printResult(alias_string);
```