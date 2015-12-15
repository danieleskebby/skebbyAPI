<?php
require_once 'skebbyAPI.php';

$sms = new skebbyAPI('username','password');

$recipients = array('391234567890');

$send = $sms->sendSMS($recipients, 'test classe skebbyAPI via PHP');

print_r($send);
echo "\n";
?>