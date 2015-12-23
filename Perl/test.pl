#!/usr/bin/perl

use skebbyAPI;

$sms = skebbyAPI->new("username","password");

$recipients = ('391234567890');
$recipients = "&recipients[]=".join("&recipients[]=", $recipients);
    
#%credit = $sms->getCredit();
%send = $sms->sendSMS($recipients,'test classe skebbyAPI via Perl');
$sms->printResult(%send);