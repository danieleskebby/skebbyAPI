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
	using System.Collections.Generic;
	
	// The actual class
	public class skebbyAPI
	{	
		// declare static variable for do_post_request method
		protected static string Url {get;set;}
		protected static string Username {get;set;}
		protected static string Password {get;set;}
		
		public skebbyAPI(string User, string Pass)
		{
			Username = User;
			Password = Pass;
			Url = "http://gateway.skebby.it/api/send/smseasy/advanced/http.php";
		}
		
		// core method, sends POST request to Skebby server
		protected static Dictionary<string, string> do_post_request( Dictionary<string, string> data ){
			
			Dictionary<string, string> result = new Dictionary<string, string>();
			string[] tempResult, temp;
			
			WebRequest request = WebRequest.Create(Url);
			request.Credentials = CredentialCache.DefaultCredentials;
			((HttpWebRequest)request).UserAgent = "Skebby C# Gateway";
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			
			string parameters = "";
			foreach(KeyValuePair<string, string> item in data)
			{
				parameters += "&" + item.Key + "=" + WebUtility.UrlEncode( item.Value );
			}
			parameters = parameters.Substring(1);
			
			byte[] byteArray = Encoding.UTF8.GetBytes (parameters);
			request.ContentLength = byteArray.Length;
			
			Stream dataStream = request.GetRequestStream();
			dataStream.Write (byteArray, 0, byteArray.Length);
			
			WebResponse response = request.GetResponse();
			//Console.WriteLine (((HttpWebResponse)response).StatusDescription);
			dataStream = response.GetResponseStream();
			
			StreamReader reader = new StreamReader(dataStream);
			
			string responseFromServer = reader.ReadToEnd();
			
			tempResult = responseFromServer.Split('&');
			for (int i = 0; i < tempResult.Length; i++)
            {
                temp = tempResult[i].Split('=');
                result.Add(temp[0], temp[1]);
            }
			
			reader.Close();
            response.Close();	
			dataStream.Close();
			
			return result;
		}
		// sendSMS method
		public Dictionary<string, string> sendSMS(
			string[] recipients,
			string text,
			string var_method = "",
			string sender_number = "",
			string sender_string = "",
			string charset = "",
			string delivery_start = "",
			string encoding_scheme = "",
			string validity_period = "",
			string user_reference = ""
		){
			int i = 0;
			Dictionary<string, string> data = new Dictionary<string, string>();
			
			data.Add("method",(var_method != "" ) ? var_method : "send_sms_classic");
			data.Add("username",Username);
			data.Add("password",Password);
			data.Add("text",text);
			
			foreach (var number in recipients)
			{
				data.Add("recipients["+i+"]",number);
				i++;
			}
			
			if (sender_number != "") 	{ data.Add("sender_number",sender_number); }
			if (sender_string != "") 	{ data.Add("sender_string",sender_string); }
			if (charset != "") 			{ data.Add("charset",charset); }
			if (delivery_start != "") 	{ data.Add("delivery_start",delivery_start); }
			if (encoding_scheme != "")	{ data.Add("encoding_scheme",encoding_scheme); }
			if (validity_period != "")	{ data.Add("validity_period",validity_period); }
			if (user_reference != "") 	{ data.Add("user_reference",user_reference); }
			
			return do_post_request(data);
		}
		// getCredit method
		public Dictionary<string, string> getCredit(
			string charset = ""
		)
		{
			string var_method = "get_credit";
			Dictionary<string, string> data = new Dictionary<string, string>();
			
			data.Add("method",var_method);
			data.Add("username",Username);
			data.Add("password",Password);
			if (charset != "") { data.Add("charset",charset); }
			
			return do_post_request(data);
		}
		// addAlias method
		public Dictionary<string, string> addAlias(
			string alias_name,
			string business_name,
			string nation,
			string vat_number,
			string taxpayer_number,
			string street,
			string city,
			string postcode,
			string contact,
			string charset = ""
		)
		{
			string var_method = "add_alias";
			Dictionary<string, string> data = new Dictionary<string, string>();
			
			data.Add("method",var_method);
			data.Add("username",Username);
			data.Add("password",Password);
			data.Add("alias",alias_name);
			data.Add("business_name",business_name);
			data.Add("nation",nation);
			data.Add("vat_number",vat_number);
			data.Add("taxpayer_number",taxpayer_number);
			data.Add("street",street);
			data.Add("city",city);
			data.Add("postcode",postcode);
			data.Add("contact",contact);
			if (charset != "") { data.Add("charset",charset); }
			
			return do_post_request(data);
		}
		// printResult method
		public Dictionary<string, string> printResult( Dictionary<string, string> data )
		{
			foreach (KeyValuePair<string, string> item in result1)
			{
				string line = string.Format("{0}: {1}",item.Key,item.Value);
				Console.WriteLine(line);	
			}	
		}
	}
}