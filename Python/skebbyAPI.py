#!/usr/bin/python

# Skebby Python Gateway Class
# Importing useful libraries
import urllib
import urllib2

# define constants
NET_ERROR = "Network error, unable to send the message"
SENDER_ERROR = "You can specify only one type of sender, numeric or alphanumeric"
url = 'http://gateway.skebby.it/api/send/smseasy/advanced/http.php'
                
# The actual class
class skebbyAPI:
        
	# init
	def __init__(self, usr, pwd):
		self.usr = usr
		self.pwd = pwd
                
	# useful is_array function
        def _isArray(self,obj): 
               is_array = lambda var: isinstance(var, (list, tuple))
               return is_array(obj)
	
        # urlEncode function for recipients
	def _urlEncode(self,recipients):
		resultString = ''
                if isinstance(recipients[0],dict) and ('recipient' in recipients[0]):
                        for i in range(len(recipients)):
                                for key,value in recipients[i].items():
                                        resultString = resultString + 'recipients[' + str(i) + '][' + str(key) + ']=' + urllib.quote_plus(value) + '&'
                else:
                        if self._isArray(recipients) is False: recipients = [recipients]
                        for number in recipients:
                                resultString = resultString + 'recipients[]=' + urllib.quote_plus(number) + '&'
                return resultString[:-1]
	
        # Core function where the class connects to Skebby Gateway URL and sends the data.
	def do_post_request(self,data,headers=''):
                if headers != '' and isinstance(headers,dict):
                        headers = headers
                else:
                        headers = {
                                'User-Agent' : 'Skebby Python Gateway'
                        }
                        
                req = urllib2.Request(url, data, headers)
                try:
                        response = urllib2.urlopen(req)
                except urllib2.HTTPError as e:
                        result = {}
                        result['status'] = 'failed'
                        result['code'] = e.code
                        result['message'] = NET_ERROR
                        return result
                except urllib2.URLError as e:
                        result = {}
                        result['status'] = 'failed'
                        result['message'] = e.reason
                        return result
        
                resultString = response.read()
                results = resultString.split('&')
                result = {}
                for r in results:
                        temp = r.split('=')
                        result[temp[0]] = temp[1]
        
                return result
        
        # send SMS function
        def sendSMS(self,recipients,text,method='send_sms_classic',sender_number='',sender_string='',charset='',delivery_start='',encoding_scheme='',validity_period='',user_reference='',optional_headers=''):
                parameters = {
                        'method' : method,
                        'username' : self.usr,
                        'password' : self.pwd,
                        'text' : text
                }
        
                if sender_number != '' and sender_string != '' :
                        result = {}
                        result['status'] = 'failed'
                        result['message'] = SENDER_ERROR
                        return result
        
                if sender_number != '' : parameters['sender_number'] = sender_number
                if sender_string != '' : parameters['sender_string'] = sender_string
        
                if charset != 'ISO-8859-1' : parameters['charset'] = 'UTF-8'
        
                data = urllib.urlencode(parameters) + '&' + self._urlEncode(recipients)
                result = self.do_post_request(data,optional_headers)
                return result
        
        # get Credit function
        def getCredit(self,charset=''):
                method = 'get_credit'
                parameters = {
                        'method' : method,
                        'username' : self.usr,
                        'password' : self.pwd
                }
                if charset != 'ISO-8859-1' : parameters['charset'] = 'UTF-8'
                data = urllib.urlencode(parameters)
                result = self.do_post_request(data)
                return result
        
        # Add Alias function
        def addAlias(self,alias,business_name,nation,vat_number,taxpayer_number,street,city,postcode,contact,charset=''):
                method = 'add_alias'
                parameters = {
                        'method' : method,
                        'username' : self.usr,
                        'password' : self.pwd,
                        'alias' : alias,
                        'business_name' : business_name,
                        'nation' : nation,
                        'vat_number' : vat_number,
                        'taxpayer_number' : taxpayer_number,
                        'street' : street,
                        'city' : city,
                        'postcode' : postcode,
                        'contact' : contact
                }
                if charset != 'ISO-8859-1' : parameters['charset'] = 'UTF-8'
                data = urllib.urlencode(parameters)
                result = self.do_post_request(data)
                return result