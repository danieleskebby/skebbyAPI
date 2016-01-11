using System;
using System.Collections;
using System.Collections.Generic;
using SkebbyGW;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sms = new skebbyAPI("username","password");
            
            Dictionary<string,dynamic> input = new Dictionary<string,dynamic>();
            input.Add("text","It's easy to send a message :)");
            input.Add("recipients", {"391234567890"});
            
            string result = sms.sendSMS( input );
            sms.printResult(result);
        }
    }
}
