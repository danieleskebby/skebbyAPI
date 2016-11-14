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
		url = "http://api.skebby.it/api/send/smseasy/advanced/http.php";
	}
	
	/**
	 * Core method.
	 * connects to skebby URL and sends data via HTTP POST request
	 * 
	 * @param data	Map<String,Object> object that contains the data
	 *				that will the sent through the request.
	 * @return 		the response from the POST request, formatted as a querystring.
	 * @throws IOException 
	 * @throws MalformedURLException if the URL is not formatted correctly
	 * @throws ProtocolException if a protocol outside <i>POST, PUT, DELETE, GET</i> is inserted
	 * @throws UnsupportedEncodingException when a charset not supported inside the URLEncoder class is used
	 */
	protected String doPostRequest( Map<String,Object> data ) throws IOException, MalformedURLException, ProtocolException, UnsupportedEncodingException {
		
        data.put("username",username);
		data.put("password",password);
        if ( !data.containsKey("charset") ) {
            data.put("charset","UTF-8");
        }
        
		if(data.containsKey("recipients")) {
			
            System.out.println(data.get("recipients").getClass().getName());
            
            if(data.get("recipients").getClass() == ArrayList.class) {
                List<Object> recipients = (ArrayList<Object>)data.get("recipients");
                System.out.println("It's an arrayList!");
                int i = 0;
                for(Object rec: recipients) {
                    System.out.println(rec);
                    i++;
                }
                
                //data.put("recipients[" + i + "]", recipients.get(i));
            } else {
                System.out.println("It's not an arrayList :(");
                String[] recipients = (String[])data.get("recipients");
            }
			/*for(int i = 0; i < recipients; i++) {
				System.out.println(i);
                //data.put("recipients[" + i + "]", recipients.get(i));
			}
            if(data.containsKey("recipients[0]") || data.containsKey("recipients[0][recipient]")) { 
                data.remove("recipients");
            }*/
		}
        
        URL connUrl = new URL(url);
		StringBuilder postData = new StringBuilder();
		for (Map.Entry<String,Object> param : data.entrySet()) {
			if (postData.length() != 0) postData.append('&');
			postData.append(URLEncoder.encode(param.getKey(), data.get("charset").toString()));
			postData.append('=');
			postData.append(URLEncoder.encode(String.valueOf(param.getValue()), data.get("charset").toString()));
		}
		byte[] postDataBytes = postData.toString().getBytes(data.get("charset").toString());

		HttpURLConnection conn = (HttpURLConnection)connUrl.openConnection();
		conn.setRequestMethod("POST");
		conn.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
		conn.setRequestProperty("User-Agent", "Skebby Java Gateway");
		conn.setRequestProperty("Content-Length", String.valueOf(postDataBytes.length));
		conn.setDoOutput(true);
		conn.getOutputStream().write(postDataBytes);

		Reader in = new BufferedReader(new InputStreamReader(conn.getInputStream(), data.get("charset").toString()));
		
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
	public String sendSMS( Map<String,Object> input ) throws IOException {
		if (input.containsKey("charset") && (!input.get("charset").equals("UTF-8") && !input.get("charset").equals("ISO-8859-1"))) {
            throw new IllegalArgumentException("Charset not supported.");
        }
        if(null == input.get("method") ) {
            input.put("method","send_sms_classic");
        }
		
		return doPostRequest( input );
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
	public String getCredit() throws IOException {
        Map<String,Object> input = new LinkedHashMap<>();
        input.put("charset","UTF-8");
		return getCredit( input );
	}
	public String getCredit( Map<String,Object> input ) throws IOException {
		if (input.containsKey("charset") && (!input.get("charset").equals("UTF-8") && !input.get("charset").equals("ISO-8859-1"))) {
            throw new IllegalArgumentException("Charset not supported.");
        }
		input.put("method", "get_credit");
		
		return doPostRequest( input );
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
	public String addAlias( Map<String,Object> input ) throws IOException {
		if (input.containsKey("charset") && (!input.get("charset").equals("UTF-8") && !input.get("charset").equals("ISO-8859-1"))) {
            throw new IllegalArgumentException("Charset not supported.");
        }
        input.put("method", "add_alias");
		
		return doPostRequest( input );
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