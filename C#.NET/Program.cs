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
            
            Hashtable input = new Hashtable();
            input.Add("text","It's easy to send a message :)");
            input.Add("recipients", new string[] {"391234567890"});
            
            string result = sms.sendSMS( input );
            sms.printResult(result);
        }
    }
}
