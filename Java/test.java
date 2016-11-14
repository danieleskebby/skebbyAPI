import java.util.*;
import java.net.*;

public class test {
	public static void main(String[] args) {
		skebbyAPI sms = new skebbyAPI("test_biz_web","Entrasito23!");
		Map<String,Object> data = new LinkedHashMap<>();
//      String[] recipients = {"393887535893"};
        Map<String, String> singleReci = new LinkedHashMap<>();
        singleReci.put("recipient","3887535893");
        singleReci.put("name","Daniele");
        
        Map<String, String> singleReci2 = new LinkedHashMap<>();
        singleReci2.put("recipient","3887535893");
        singleReci2.put("name","Daniele");
        
        List<Object> recipients = new ArrayList<Object>();
        recipients.add( singleReci );
        recipients.add( singleReci2 );
        
		data.put("recipients", recipients); 
		data.put("text","It's easy to send a message :)");
		try {
//			String[] result = sms.getCredit(data);
			String result = sms.sendSMS(data);
			
			sms.printResult(result);
		} catch(Exception e) {
			System.out.println(e);
		}
	}
}