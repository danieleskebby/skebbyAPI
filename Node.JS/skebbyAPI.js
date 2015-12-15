// Skebby Node.js Gateway Class
var qs = require('querystring');
var https = require('https');

var method = skebbyAPI.prototype;

function skebbyAPI(user,pass) {
	this._user = user;
	this._pass = pass;
	this._path = '/api/send/smseasy/advanced/http.php';
	this._host = 'gateway.skebby.it';
}

// the core function
method.do_post_request = function(input,callback,callback_error) {
	
	var res_done = false,
	data = qs.stringify(input),
	client = https.request({
        port:	 443,
        path:	 this._path,
        host:	 this._host,
		method : "POST", 
        headers: { 
            "Content-Type": 	"application/x-www-form-urlencoded",
            "Content-Length": 	data.length,
            "Content-Encoding": "utf8",
			"User-Agent":		"Skebby Node.js Gateway"
        }
    }, function(res){
        var res_data = "";
        res.on("data", function(data) { res_data+=data; });
        res.on("end",function(){
            if (!res_done){
				var res_parsed = qs.parse(res_data);
				if(res_parsed.status == "success") {
					callback({data:res_parsed});
					return;
				} else {
					callback_error({data: res_parsed});
					return;
				}
            	res_done = true;
            }
        });
    }); 
 
    client.end(data);
    client.on('error', function(e) {
        if (!res_done){
            res_done = true;
            callback_error(e);
			return;
        }
    });
};

// send SMS function
method.sendSMS = function(input,callback,callback_error) {
	var data = {
		username:		 this._user,
		password:		 this._pass,
		"recipients[]":	 input.recipients || [],
		text:			 input.text,
		method:			 input.method || "send_sms_classic"
	};
	
	if(input.sender_number) { data.sender_number = input.sender_number; }
	if(input.sender_string) { data.sender_string = input.sender_string; }
	if(input.delivery_start) { data.delivery_start = input.delivery_start; }
	if(input.encoding_scheme) { data.encoding_scheme = input.encoding_scheme; }
	if(input.validity_period) { data.validity_period = input.validity_period; }
	if(input.user_reference) { data.user_reference = input.user_reference; }
	
	this.do_post_request(
		data,
		function(response) {
			callback(res);
			return;
		},
		function(error) {
			callback_error(err);
			return;
		}
	);
};

// get Credit function
method.getCredit = function(charset,callback,callback_error) {
	var data = {
		method:		 "get_credit",
		username:	 this._user,
		password:	 this._pass,
		charset:	 charset || "UTF-8"
	};
	
	this.do_post_request(
		data,
		function(response) {
			callback(res);
			return;
		},
		function(error) {
			callback_error(err);
			return;
		}
	);
};

// add Alias function
method.addAlias = function(input,callback,callback_error) {
	var data = {
		method:			 "add_alias",
		username:		 this._user,
		password:		 this._pass,
		charset:		 charset || "UTF-8",
		alias:			 input.alias,
		business_name:	 input.business_name,
		nation:			 input.nation,
		vat_number:		 input.vat_number,
		taxpayer_number: input.taxpayer_number,
		street:			 input.street,
		city:			 input.city,
		postcode:		 input.postcode,
		contact:		 input.contact
	};
	
	this.do_post_request(
		data,
		function(response) {
			callback(res);
			return;
		},
		function(error) {
			callback_error(err);
			return;
		}
	);
}

module.exports = skebbyAPI;