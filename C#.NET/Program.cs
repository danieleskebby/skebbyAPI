using System;
using System.Collections.Generic;
using SkebbyGW;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sms = new skebbyAPI("username","password");
            string[] recipient = { "393887535893" };
            
            Dictionary<string,dynamic> input = new Dictionary<string,dynamic>();
            input.Add("text","It's easy to send a message :)");
            input.Add("recipients", recipient );
            
            string result = sms.sendSMS( input );
            sms.printResult(result);
        }
    }
}
