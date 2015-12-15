#!/usr/bin/python

from skebbyAPI import skebbyAPI
#import pprint

sms = skebbyAPI('username','password')

recipients = ['391234567890']

send_sms = sms.sendSMS(recipients,'test classe skebbyAPI via Python')

print send_sms