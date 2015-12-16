# C# .NET skebby API SDK

This is the C# .NET version of skebby API SDK.

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
// set the recipients
string[] recipients = new string[] {"391234567890"};

// Call the sendSMS function
Dictionary<string, string> send = sms.sendSMS(recipients,"test classe SkebbyAPI via C#");

// print the result
sms.printResult(send);
```

### Check available credit
```C#
// Call the getCredit function
Dictionary<string, string> credit = sms.getCredit();

// print the result
sms.printResult(credit);
```

### Register an Alphanumeric Sender (Alias)
```C#
// Call the addAlias function
Dictionary<string, string> alias_string = sms.addAlias(
	'Skebby',
	'Skebby',
	'IT',
	'111222333444',
	'111222333444',
	'Via Melzo 12',
	'Milano',
	'20100',
	'contact@email.com'
);

// print the result
sms.printResult(alias_string);
```