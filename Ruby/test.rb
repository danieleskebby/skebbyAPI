# load the class
require_relative 'skebbyAPI'

sms = SkebbyAPI.new('username','password')
recipients = ['391234567890']
result = sms.sendSMS(recipients,'test classe SkebbyAPI via Ruby')
sms.printResult(result)