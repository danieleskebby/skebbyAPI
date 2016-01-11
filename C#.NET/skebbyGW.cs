// Skebby C# Gateway Class
// Skebby namespace
namespace SkebbyGW
{
    // Import useful libraries	
    using System;
    using System.IO;
    using System.Web;
    using System.Net;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;

    // The actual class
    public class skebbyAPI
    {
        // declare static variable for doPostRequest method
        protected static string Url { get; set; }
        protected static string Username { get; set; }
        protected static string Password { get; set; }

        public skebbyAPI(string User, string Pass)
        {
            Username = User;
            Password = Pass;
            Url = "http://gateway.skebby.it/api/send/smseasy/advanced/http.php";
        }
        
        // core method, sends POST request to Skebby server
        protected static string doPostRequest(Dictionary<string,dynamic> data)
        {
            WebRequest request = WebRequest.Create(Url);
            request.Credentials = CredentialCache.DefaultCredentials;
            ((HttpWebRequest)request).UserAgent = "Skebby C# Gateway";
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            
            data.Add("username", Username);
            data.Add("password", Password);

            string parameters = "";
            foreach (KeyValuePair<string,dynamic> item in data)
            {
                if (Convert.ToString(item.Key) == "recipients")
                {
                    if (item.Value is IList)
                    {
                        for(int i = 0; i < item.Value.Count; i++)
                        {
                            foreach(KeyValuePair<string,string> rec in item.Value[i])
                            {
                                Console.WriteLine("{0}: {1}",rec.Key,rec.Value);
                                parameters += "&recipients[" + i + "][" + WebUtility.UrlEncode(Convert.ToString(rec.Key)) + "]=" + WebUtility.UrlEncode(Convert.ToString(rec.Value));
                            }
                        }
                    }
                    else
                    {
                        foreach (string rec in item.Value)
                        {
                            parameters += "&recipients[]=" + WebUtility.UrlEncode(Convert.ToString(rec));
                        }
                    }
                }
                else
                {
                    parameters += "&" + WebUtility.UrlEncode(Convert.ToString(item.Key)) + "=" + WebUtility.UrlEncode(Convert.ToString(item.Value));
                }
            }
            parameters = parameters.Substring(1);

            byte[] byteArray = Encoding.UTF8.GetBytes (parameters);
            request.ContentLength = byteArray.Length;
			
            Stream dataStream = request.GetRequestStream();
            dataStream.Write (byteArray, 0, byteArray.Length);
			
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
			
            StreamReader reader = new StreamReader(dataStream);
			
            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            response.Close();
            dataStream.Close();

            return responseFromServer;
        }

        // sendSMS method
        public string sendSMS(Dictionary<string,dynamic> input)
        {
            if (!input.ContainsKey("method"))
            {
                input.Add("method", "send_sms_classic");
            }

            return doPostRequest(input);
        }

        // getCredit method
        public string getCredit(string charset = "")
        {
            string var_method = "get_credit";
            Dictionary<string, dynamic> input = new Dictionary<string, dynamic>();

            input.Add("method", var_method);
            if (charset != "") { input.Add("charset", charset); }

            return doPostRequest(input);
        }

        // addAlias method
        public string addAlias(Dictionary<string, dynamic> input)
        {
            string var_method = "add_alias";
            input.Add("method", var_method);

            return doPostRequest(input);
        }

        // arrayResult method
        public Dictionary<string, string> arrayResult(string result)
        {
            string[] tempResult, temp;
            Dictionary<string, string> array = new Dictionary<string, string>();
            tempResult = result.Split('&');
            for (int i = 0; i < tempResult.Length; i++)
            {
                temp = tempResult[i].Split('=');
                array.Add(temp[0], temp[1]);
            }

            return array;
        }

        // printResult method
        public void printResult(string result)
        {
            Dictionary<string, string> data = arrayResult(result);
            foreach (KeyValuePair<string, string> item in data)
            {
                Console.WriteLine("{0}: {1}", WebUtility.UrlDecode(item.Key), WebUtility.UrlDecode(item.Value));
            }
        }
    }
}