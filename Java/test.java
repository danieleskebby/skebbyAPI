import java.util.*;
import java.net.*;

public class test {
	public static void main(String[] args) {
		skebbyAPI sms = new skebbyAPI("username","password");
		Map<String,String> data = new LinkedHashMap<>();
		data.put("recipients","391234567890"); 
		data.put("text","test classe skebbyAPI via Java");
		try {
//			String[] result = sms.getCredit(data).split("&");
			String[] result = sms.sendSMS(data).split("&");
			
			for(String s: result) {
				String[] temp = s.split("=");
				System.out.println(temp[0] + ": " + URLDecoder.decode(temp[1],"UTF-8"));
			}	
		} catch(Exception e) {
			System.out.println(e);
		}
	}
}