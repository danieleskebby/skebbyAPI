# PHP skebby API SDK

This is the PHP version of Skebby API SDK.

## Usage

Here's the examples that uses the functions inside the Class

### Inclusion and class initialization
```Php
// Include the file that contains the SDK
require_once 'skebbyAPI.php';

// Call the skebbyAPI class and set the credentials 
$sms = new skebbyAPI('username','password');
```

### Send a SMS message
```Php
// Set the parameters inside an array
$parameters = array(
	'text' => "it's easy to send a message :)",
	'recipients' => array('391234567890')
);

// Call the sendSMS function
$send = $sms->sendSMS( $parameters );

// Print the result
echo $send;
```

### Check available credit
```Php
// Call the getCredit function
$credit = $sms->getCredit();

// Print the result
echo $credit;
```

### Register an Alphanumeric Sender (Alias)
```Php
//Set the parameters inside an array
$parameters = array(
  'alias' => 'Skebby',
  'business_name' => 'Skebby'
  'nation' => 'IT'
  'vat_number' => '111222333444'
  'taxpayer_number' => '111222333444'
  'street' => 'Via Melzo 12'
  'city' => 'Milan'
  'postcode' => '20100'
  'contact' => 'contact@email.com'
);

// Call the addAlias function
$alias = $sms->addAlias( $parameters );

// Print the result
echo $alias;
```