# Node.JS skebby API SDK

This is the Node.JS version of Skebby API SDK.

## Usage

Here's the examples that uses the functions inside the Class

### Inclusion and class initialization
```JavaScript
// Include the file that contains the SDK
var skebbyAPI = require('./skebbyAPI.js');

// Call the skebbyAPI class and set the credentials
var sms = new skebbyAPI('username','password');
```

### Send a SMS message
```JavaScript
// Call the sendSMS function
sms.sendSMS(
	{
		recipients: [391234567890],
		text: "test classe skebbyAPI via Node.js"
	},
	function(data) {
		console.log("success string");
		console.log(data);
	},
	function(error) {
		console.log("fail string");
		console.log(error);
	}
);
```

### Check available credit
```JavaScript
// Call the getCredit function
sms.getCredit(
	function(data) {
		console.log("success string");
		console.log(data);
	},
	function(error) {
		console.log("fail string");
		console.log(error);
	}
);
```

### Register an Alphanumeric Sender (Alias)
```JavaScript
// Call the addAlias function
sms.addAlias(
	{
        alias: "Skebby",
        business_name: "Skebby",
        nation: "IT",
        vat_number: "111222333444",
        taxpayer_number: "111222333444",
        street: "Via Melzo 12",
        city: "Milan",
        postcode: "20100",
        contact: "contact@email.com"
	},
	function(data) {
		console.log("success string");
		console.log(data);
	},
	function(error) {
		console.log("fail string");
		console.log(error);
	}
);
```
