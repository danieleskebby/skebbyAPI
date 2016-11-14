#!/usr/bin/ruby

# Skebby Ruby Gateway Class
# Importing useful libraries
require 'net/http'
require 'uri'
require 'cgi'

# The actual class
class SkebbyAPI
	def initialize(username,password)
		@url = 'http://api.skebby.it/api/send/smseasy/advanced/http.php'
		@parameters = {
			'username' => username,
			'password' => password,
		}
	end
	
	# Core function where the class connects to Skebby Gateway URL and sends the data.
	def do_post_request(data)
		response = Net::HTTP.post_form(URI(@url),data)
        if response.message == "OK"
			result = {}
			response.body.split('&').each do |res|
				if res != ''
					temp = res.split('=')
					if temp.size > 1
						result[temp[0]] = temp[1]
					end
				end
			end
        else
            result = "Error in the HTTP request"
        end
        return result
	end
	
	# Send SMS function
	def sendSMS(recipients,text,method='send_sms_classic',options = {})
		unless recipients.kind_of?(Array)
        	recipients = [recipients]
		end
		
		if (recipients[0].kind_of?(Hash) and recipients[0].has_key?('recipient'))
			for i in 0..(recipients.length-1)
				recipients[i].each do |key,value|
					@parameters["recipients[#{i}][#{key}]"] = value
				end
			end
		else
			@parameters["recipients[]"] = recipients
		end
		
      	@parameters['text'] = text
		@parameters['method'] = method

		unless options[:sender_number].nil?
			@parameters['sender_number'] = options[:sender_number]
		end
	
		unless options[:sender_string].nil?
			@parameters['sender_string'] = options[:sender_string]
		end
	
		unless options[:charset].nil?
			@parameters['charset'] = options[:charset]
		end
	
		unless options[:delivery_start].nil?
			@parameters['delivery_start'] = options[:delivery_start]
		end
	
		unless options[:encoding_scheme].nil?
			@parameters['encoding_scheme'] = options[:encoding_scheme]
		end
	
		unless options[:validity_period].nil?
			@parameters['validity_period'] = options[:validity_period]
		end
	
		unless options[:user_reference].nil?
			@parameters['user_reference'] = options[:user_reference]
		end
		
		result = self.do_post_request(@parameters)
		return result
	end
	
	# Get Credit function
	def getCredit()
		@parameters['method'] = 'get_credit'
		result = self.do_post_request(@parameters)
		return result
	end
	
	# Add Alias function
	def addAlias(alias_name,business_name,nation,vat_number,taxpayer_number,street,city,postcode,contact,charset=nil)
		@parameters['method'] = 'add_alias'
		@parameters['alias'] = alias_name
		@parameters['business_name'] = business_name
		@parameters['nation'] = nation
		@parameters['vat_number'] = vat_number
		@parameters['taxpayer_number'] = taxpayer_number
		@parameters['street'] = street
		@parameters['city'] = city
		@parameters['postcode'] = postcode
		@parameters['contact'] = contact
	
		unless charset.nil?
			@parameters['charset'] = charset
		end
		
		result = self.do_post_request(@parameters)
		return result
	end
	
	# Print result in a fancy way
	def printResult(result)
		if result.has_key?('status') and result['status'] == 'success'
			puts "Success, response contains:"
			result.each do |key,value|
				puts "\t#{key} => #{CGI::unescape(value)}"
			end
			true
		else      
			puts "Error, trace is:"
			result.each do |key,value|
				puts "\t#{key} => #{CGI::unescape(value)}"
			end
			false
		end
	end
end