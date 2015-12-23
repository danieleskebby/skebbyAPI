// Skebby Java Gateway Class
// TO DO:
// - integrate SOAP requests
// - integrate REST requests

import java.util.*;
import java.io.*;
import java.net.*;

/**
 * skebbyAPI class 
 * it connects to the Skebby URL and uses the API to send a SMS, check the remaining credit and add an Alias (an alphanumeric sender)
 * 
 * @author Daniele Biggiogero
 * @version 1.0
 * @see URL
 * @see Map
 * @see URLEncoder
 */
public class skebbyAPI {
	// Main parameters declaration
	protected String username;
	protected String password;
	protected String charset;
	protected String url;
	
	/**
	 * skebbyAPI constructor
	 * 
	 * @param user the username necessary for authentication
	 * @param pass the password necessary for authentication
	 */
	public skebbyAPI(
		String user,
		String pass
	) {
		this(user,pass,"UTF-8");
	}
	public skebbyAPI(
		String user,
		String pass,
		String cset
	) {
		username = user;
		password = pass;
		charset = cset;
		url = "http://gateway.skebby.it/api/send/smseasy/advanced/http.php";
	}
	
	/**
	 * Core method.
	 * connects to skebby URL and sends data via HTTP POST request
	 * 
	 * @param data	Map<String,String> object that contains the data
	 *				that will the sent through the request.
	 * @return 		the response from the POST request, formatted as a querystring.
	 * @throws IOException 
	 * @throws MalformedURLException if the URL is not formatted correctly
	 * @throws ProtocolException if a protocol outside <i>POST, PUT, DELETE, GET</i> is inserted
	 * @throws UnsupportedEncodingException when a charset not supported inside the URLEncoder class is used
	 */
	protected String doPostRequest( Map<String,String> data ) throws IOException, MalformedURLException, ProtocolException, UnsupportedEncodingException {
		URL connUrl = new URL(url);
		StringBuilder postData = new StringBuilder();
		for (Map.Entry<String,String> param : data.entrySet()) {
			if (postData.length() != 0) postData.append('&');
			postData.append(URLEncoder.encode(param.getKey(), data.get("charset")));
			postData.append('=');
			postData.append(URLEncoder.encode(String.valueOf(param.getValue()), data.get("charset")));
		}
		byte[] postDataBytes = postData.toString().getBytes(data.get("charset"));

		HttpURLConnection conn = (HttpURLConnection)connUrl.openConnection();
		conn.setRequestMethod("POST");
		conn.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
		conn.setRequestProperty("User-Agent", "Skebby Java Gateway");
		conn.setRequestProperty("Content-Length", String.valueOf(postDataBytes.length));
		conn.setDoOutput(true);
		conn.getOutputStream().write(postDataBytes);

		Reader in = new BufferedReader(new InputStreamReader(conn.getInputStream(), data.get("charset")));
		
		String result = new String();
		for ( int c = in.read(); c != -1; c = in.read() ) {
			result += (char)c;
		}
		return result;
	}
	
	/**
	 * sends one SMS to one or more recipients.
	 * <p>If the HTTP POST request is successful, the given querystring will return 2 cases: 
	 * <p>- Dispatch successful: status=success&remaining_sms=00
	 * <p>- Dispatch failed: status=failed&message=error+description&code=0
	 * <p>data must be included inside a Map object (HashMap, TreeMap, LinkedHashMap) to be iterated correctly
	 * 
	 * @param input the data that will be included in the doPostRequest method
	 * @return 		the result of the doPostRequest method, usually formatted as a querystring.
	 * @throws IOException 
	 * @throws IllegalArgumentException if a charset different than UTF-8 or ISO-8859-1 is used
	 */
	public String sendSMS( Map<String,String> input ) throws IOException {
		if (input.containsKey("charset") && (!input.get("charset").equals("UTF-8") && !input.get("charset").equals("ISO-8859-1"))) {
            throw new IllegalArgumentException("Charset not supported.");
        }
        Map<String,String> data = new LinkedHashMap<>();
		data.put("username",username);
		data.put("password",password);
		if(null != input.get("text")) { data.put("text",input.get("text")); }
		if(input.containsKey("recipients")) {
			String[] recipients = input.get("recipients").split(",");
			int i = 0;
			for(String r: recipients) {	
				data.put("recipients[" + i + "]",r);
				i++;
			}
		}
		data.put("method", input.containsKey("method") ? input.get("method") : "send_sms_classic");
		if(null != input.get("sender_number")) { data.put("sender_number",input.get("sender_number")); }
		if(null != input.get("sender_string")) { data.put("sender_string",input.get("sender_string")); }
		data.put("charset", input.containsKey("charset") ? input.get("charset") : "UTF-8");
		if(null != input.get("delivery_start")) { data.put("delivery_start",input.get("delivery_start")); }
		if(null != input.get("encoding_scheme")) { data.put("encoding_scheme",input.get("encoding_scheme")); }
		if(null != input.get("validity_period")) { data.put("validity_period",input.get("validity_period")); }
		if(null != input.get("user_reference")) { data.put("user_reference",input.get("user_reference")); }
		
		return doPostRequest(data);
	}
	
	/**
	 * check the remaining credit. 
	 * <p>If the HTTP POST request is successful, the given querystring will return:
	 * <p>status=success&credit_left=00.000&classic_sms=00&basic_sms=00&classic_plus_sms=00
	 * <p>data must be included inside a Map object (HashMap, TreeMap, LinkedHashMap) to be parsed correctly
	 * 
	 * @param input the data that will be included in the doPostRequest method
	 * @return 		the result of the doPostRequest method, usually formatted as a querystring
	 * @throws IOException 
	 * @throws IllegalArgumentException if a charset different than UTF-8 or ISO-8859-1 is used
	 */
	public String getCredit() {
		
	}
	public String getCredit( Map<String,String> input ) throws IOException {
		if (input.containsKey("charset") && (!input.get("charset").equals("UTF-8") && !input.get("charset").equals("ISO-8859-1"))) {
            throw new IllegalArgumentException("Charset not supported.");
        }
        Map<String,String> data = new LinkedHashMap<>();
		data.put("method", "get_credit");
		data.put("username", username);
		data.put("password", password);
		data.put("charset", input.containsKey("charset") ? input.get("charset") : "UTF-8");
		
		return doPostRequest(data);
	}
	
	/**
	 * adds an Alphanumeric sender (also known as Alias).
	 * <p>If the HTTP POST request is successful, the given querystring will return 2 cases: 
	 * <p>- Dispatch successful: status=success&alias=alias+name
	 * <p>- Dispatch failed: status=failed&message=error+description&code=0
	 * 
	 * @param input the data that will be included in the doPostRequest method
	 * @return 		the result of the doPostRequest method, usually formatted as a querystring
	 * @throws IOException 
	 * @throws IllegalArgumentException if a charset different than UTF-8 or ISO-8859-1 is used
	 */
	public String addAlias( Map<String,String> input ) throws IOException {
		if (input.containsKey("charset") && (!input.get("charset").equals("UTF-8") && !input.get("charset").equals("ISO-8859-1"))) {
            throw new IllegalArgumentException("Charset not supported.");
        }
        Map<String,String> data = new LinkedHashMap<>();
		data.put("method", "add_alias");
		data.put("username", username);
		data.put("password", password);
		if(null != input.get("alias")) { data.put("alias",input.get("alias")); }
		if(null != input.get("business_name")) { data.put("business_name",input.get("business_name")); }
		if(null != input.get("nation")) { data.put("nation",input.get("nation")); }
		if(null != input.get("vat_number")) { data.put("vat_number",input.get("vat_number")); }
		if(null != input.get("taxpayer_number")) { data.put("taxpayer_number",input.get("taxpayer_number")); }
		if(null != input.get("street")) { data.put("street",input.get("street")); }
		if(null != input.get("city")) { data.put("city",input.get("city")); }
		if(null != input.get("postcode")) { data.put("postcode",input.get("postcode")); }
		if(null != input.get("contact")) { data.put("contact",input.get("contact")); }
		data.put("charset", input.containsKey("charset") ? input.get("charset") : "UTF-8");
		
		return doPostRequest(data);
	}
	
	/**
	 * Prints the result in a fancy way
	 * 
	 * @param result a querystring from the doPostRequest method
	 * @return 		 prints the result in a fancier way than a querystring
	 * @throws UnsupportedEncodingException when you insert an unsupported encoding on the decode method
	 */
	 public void printResult( String result ) throws UnsupportedEncodingException {
		 String[] data = result.split("&");
		 for(String s: data) {
			String[] temp = s.split("=");
			System.out.println(temp[0] + ": " + URLDecoder.decode(temp[1],"UTF-8"));
		}
	 }
}