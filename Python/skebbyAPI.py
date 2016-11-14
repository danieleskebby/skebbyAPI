#!/usr/bin/python

# Skebby Python Gateway Class
# Importing useful libraries
import urllib
import urllib2

# define constants
NET_ERROR = "Network error, unable to send the message"
SENDER_ERROR = "You can specify only one type of sender, numeric or alphanumeric"
url = 'http://api.skebby.it/api/send/smseasy/advanced/http.php'

# The actual class
class skebbyAPI:

    # init
    def __init__(self, usr, pwd):
        self.usr = usr
        self.pwd = pwd
        self.charset = 'UTF-8'

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
    def do_post_request(self,input):
        input['username'] = self.usr
        input['password'] = self.pwd
        rec = ''
        if 'recipients' in input.keys():
            rec = '&' + self._urlEncode(input['recipients'])
            print rec
            #input.pop('recipients',None)

        data = urllib.urlencode(input) + rec
        headers = { 'User-Agent' : 'Skebby Python Gateway' }

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
        return resultString

    # send SMS function
    def sendSMS(self,input):
        if 'method' not in input.keys():
            input['method'] = 'send_sms_classic'
        if ( 'sender_number' in input.keys() and 'sender_string' in input.keys() ) and (input['sender_number'] != '' and input['sender_string'] != '') :
            result = {}
            result['status'] = 'failed'
            result['message'] = SENDER_ERROR
            return result

        if 'charset' in input.keys() and input['charset'] != 'ISO-8859-1' :
            input['charset'] = self.charset
        elif 'charset' in input.keys() and input['charset'] == 'ISO-8859-1':
            self.charset = 'ISO-8859-1'
        
        result = self.do_post_request(input)
        return result

    # get Credit function
    def getCredit(self,input = {}):
        input['method'] = 'get_credit'
        if 'charset' in input.keys() and input['charset'] != 'ISO-8859-1' :
            input['charset'] = self.charset
        elif 'charset' in input.keys() and input['charset'] == 'ISO-8859-1':
            self.charset = 'ISO-8859-1'
        
        result = self.do_post_request(input)
        return result

    # Add Alias function
    def addAlias(self,input):
        input['method'] = 'add_alias'
        if 'charset' in input.keys() and input['charset'] != 'ISO-8859-1' :
            input['charset'] = self.charset
        elif 'charset' in input.keys() and input['charset'] == 'ISO-8859-1':
            self.charset = 'ISO-8859-1'
        
        result = self.do_post_request(input)
        return result
        
    def arrayResult(self,resultString):
        results = resultString.split('&')
        result = {}
        for r in results:
            temp = r.split('=')
            result[temp[0]] = urllib.unquote(temp[1]).replace('+',' ')

        return result
    
    def printResult(self,result):
        r = self.arrayResult(result)
        for key,value in r.items():
            print key + ': ' + value