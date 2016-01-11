# Java skebby API SDK

This is the Java version of Skebby API SDK.

## Usage

Here's the examples that uses the functions inside the Class

### Inclusion and class initialization
If you put the skebbyAPI.java on the same folder where the main file is located, you don't need to write anything to import the class.

```Java
// Import the required libraries
import java.util.*;
import java.net.*;

public class test {
	public static void main(String[] args) {
        // Call the skebbyAPI class and set the credentials 
		skebbyAPI sms = new skebbyAPI("username","password");
	}
}
```

### Send a SMS message
```Java
// set the parameters
Map<String,String> data = new LinkedHashMap<>();
data.put("recipients","391234567890"); 
data.put("text","It's easy to send a message :)");

// call the sendSMS function 
try {
    String[] result = sms.sendSMS(data).split("&");
    
    for(String s: result) {
        String[] temp = s.split("=");
        System.out.println(temp[0] + ": " + URLDecoder.decode(temp[1],"UTF-8"));
    }	
} catch(Exception e) {
    System.out.println(e);
}
```

### Check available credit
```Java
// Call the getCredit function
try {
    String[] result = sms.getCredit().split("&");
    
    for(String s: result) {
        String[] temp = s.split("=");
        System.out.println(temp[0] + ": " + URLDecoder.decode(temp[1],"UTF-8"));
    }	
} catch(Exception e) {
    System.out.println(e);
}
```

### Register an Alphanumeric Sender (Alias)
```Java
// set the parameters
Map<String,String> data = new LinkedHashMap<>();
data.put("alias","Skebby"); 
data.put("business_name","Skebby");
data.put("nation","IT"); 
data.put("vat_number","111222333444");
data.put("taxpayer_number","111222333444"); 
data.put("street","Via Melzo 12");
data.put("city","Milan");
data.put("postcode","20100");
data.put("contact","contact@email.com");

// call the addAlias function 
try {
    String[] result = sms.addAlias(data).split("&");
    
    for(String s: result) {
        String[] temp = s.split("=");
        System.out.println(temp[0] + ": " + URLDecoder.decode(temp[1],"UTF-8"));
    }	
} catch(Exception e) {
    System.out.println(e);
}
```
