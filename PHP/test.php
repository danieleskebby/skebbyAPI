<?php
require_once 'skebbyAPI.php';

$sms = new skebbyAPI('username','password');

$parameters = array(
	'text' => "it's easy to send a message :)",
	'recipients' => array('391234567890')
);

$send = $sms->sendSMS( $parameters );
//$send = $sms->getCredit();
//$send = $sms->addAlias( $parameters );

print_r( $sms->querystringToArray( $send ) );
echo "\n";
?>