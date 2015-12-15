// include the API
var skebbyAPI = require('./skebbyAPI.js');

var sms = new skebbyAPI('username','password');

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