#!/usr/bin/python

from skebbyAPI import skebbyAPI

sms = skebbyAPI('username','password')

parameters = {
    'text': 'test classe skebbyAPI via Python',
    'recipients': ['391234567890']
}

send_sms = sms.sendSMS( parameters )

sms.printResult( send_sms )