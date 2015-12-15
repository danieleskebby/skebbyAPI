using System;
using System.Collections.Generic;
using SkebbyGW;

public class Program 
{ 
	public static void Main(string[] args)
	{ 
		var sms = new skebbyAPI("username","password");
		string[] recipients = new string[] {"391234567890"};
		
		Dictionary<string, string> result = sms.sendSMS(recipients,"test classe SkebbyAPI via C#");
		sms.printResult(result);
	}
}