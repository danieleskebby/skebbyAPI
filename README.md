# skebbyAPI SDK Version 0.1 beta

The goal of this project is to give developers a tool that can be integrated inside the application in no time, can be easy to use and can be readable.

## Functions available
The functions available by the API are:

- `sendSMS`: sends a SMS message to a given array of recipients. Return a querystring with the result of the request.
- `getCredit`: get the the available currency on your Skebby account, and gives you the SMSs available, divided by type.
- `addAlias`: sends an alphanumeric sender (aka Alias) on approvation by AGCOM.

The API is based on calling the Skebby Gateway Server via HTTP POST Request. Developing a class that'll include SOAP and REST is on the to-do list.

## API Installation
In order to use the skebbyAPI SDK, you must:
- download the class file of the programming language of your choice
- put it in the same directory of your application (or import it on your project made by an IDE)
- declare the class
- use its functions

## Usage
Here's some examples that uses the functions inside the Class (examples are written in PHP, in the folders there are examples written in the respective languages.

### Inclusion and class initialization
```php
// Include the file that contains the SDK
require_once 'skebbyAPI.php';

// Call the skebbyAPI class and set the credentials 
$sms = new skebbyAPI('username','password');
```

#### Variables accepted
- _string_ username **required**: The username used when you log on Skebby
- _string_ password **required**: The password used when you log on Skebby

### Send a SMS message
```php
//Set the parameters inside an array
$parameters = array(
	'text' => "it's easy to send a message :)",
	'recipients' => array('391234567890')
);

// Call the sendSMS function
$send = $sms->sendSMS( $parameters );

// Print the result
echo $send;
```

This example will send a text to a given array of recipients via Classic SMS, with the default values preconfigured on Skebby account.
The function will return an url-encoded string containing the result of the dispatch.

#### Variables accepted
You can add these variables to configure the dispatch:

- _string_ `text` **required**: a text string that will be sent via SMS
- _array_ `recipients` **required**: the cellphone numbers that will receive the message
- _string_ `method`: the SMS type that will be sent. You can choose between three types of message:
  - `send_sms_basic`: **Basic SMS** best effort quality, delivery non guaranteed and with possible delay, fixed sender id, delivery report not available.
  - `send_sms_classic`: **Classic SMS** high quality, guaranteed and immediate delivery time, personalize sender id. 
  - `send_sms_classic_report`: **Classic+ SMS** as Classic with delivery report for single message (DLR).
  
Default value: _**send_sms_classic**_
- _string_ `charset`: select the charset of the text. Charsets available are:
  - `UTF-8`
  - `ISO-8859-1`
  
Default value: _**ISO-8859-1**_
- _string_ `delivery_start`: To schedule SMS dispatch. Format used is [RFC 2822](http://www.php.net/manual/en/class.datetime.php#datetime.constants.rfc2822)
- _string_ `sender_number`:(only for Classic & Classic+ SMS) a numeric string that will appear as the sender of the message
- _string_ `sender_string`: (only for Classic & Classic+ SMS) an alphanumeric string that will appear as the sender of the message
- _string_ `encoding_scheme`: (only for Classic & Classic+ SMS) To use accented or special characters commonly used in the following languages: Arabic, French, Chinese, Korean, Japanese, Slavic and Spanish. Values accepted:
  - `normal`
  - `UCS2`
  
Text of the message, maximum lenght 670 characters or 10 SMS concatenated in one message.
Default value: _**normal**_
- _string_ `validity_period`: (only for Classic & Classic+ SMS) You can specify for how many minutes (or hours) the operator must retry to send the SMS in case of phone turned off or unreachable. 
Expressed in minutes, integer number (minimum value 5 minutes).
Default value: _**2 days ( 2880 minutes = 60 ** 48 ) )**_
- _string_ `user_reference`: (only for Classic+ SMS) Reference string customizable that will be returned with the delivery report.
Supported characters: `[a-zA-Z0-9-_+:;]`

#### Return values
The function will return a string url-encoded that contains the result's variables of the dispatch.

In case of successful dispatch:
- `status`: _string_ success
- `remaining_sms`: _int_ the remaining SMSs of the type previously sent

In case of unsuccessful dispatch:
- `status`: _string_ failed
- `code`: _int_ a number relative to the Error encountered. [See more on Skebby website](http://www.skebby.com/sms-api/sms-gateway/developers-docs/#errorCodesSection)
- `message`: _string_ a message that explain the error

### Check available credit
```php
// Call the getCredit function
$credit = $sms->getCredit();

// Print the result
echo $credit;
```

The function will return an url-encoded string containing the result of the request.

#### Variables accepted
You can add these variables to configure the dispatch:
- _string_ `charset`: select the charset of the text. Charsets available are:
  - `UTF-8`
  - `ISO-8859-1`
  
Default value: _**ISO-8859-1**_

#### Return values
The function will return a string url-encoded that contains the result's variables of the dispatch.

In case of successful dispatch:
- `status`: _string_ success
- `credit_left`: _float_ The currency available on your Skebby account
- `classic_sms`: _int_ The credit available converted in remaining Classic SMSs
- `basic_sms`: _int_ The credit available converted in remaining Basic SMSs
- `classic_plus_sms`: _int_ The credit available converted in remaining Classic+ SMSs

In case of unsuccessful dispatch:
- `status`: _string_ failed
- `code`: _int_ a number relative to the Error encountered.
- `message`: _string_ a message that explain the error

### Register an Alphanumeric Sender (Alias)
```php
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
$alias = $sms->addAlias();

// Print the result
echo $alias;
```

The function will return an url-encoded string containing the result of the request.

#### Variables accepted
You can add these variables to configure the dispatch:
- _string_ `alias` **required**: an alphanumeric string that will be sent to registration. **WARNING: the string must be no longer than 11 characters**
- _string_ `business_name` **required**: The name of your company
- _string_ `nation` **required**: Country (2 characters, uppercase, ISO 3166-1 alpha-2)
- _string_ `vat_number` **required**: VAT identification number
- _string_ `taxpayer_number` **required**: Taxpayer Identification number (same as vat if not available) 
- _string_ `street` **required**: The address where the company is located
- _string_ `city` **required**: The city where your company is located
- _string_ `postcode` **required**: Postal code / ZIP code 
- _string_ `contact` **required**: Contact information (email, phone number)
- _string_ `charset`: select the charset of the text. Charsets available are:
  - `UTF-8`
  - `ISO-8859-1`
Default value: _ISO-8859-1_

#### Return values
The function will return a string url-encoded that contains the result's variables of the dispatch.

In case of successful dispatch:
- `status`: _string_ success
- `alias`: _string_ the alphanumeric sender you choose

In case of unsuccessful dispatch:
- `status`: _string_ failed
- `code`: _int_ a number relative to the Error encountered.
- `message`: _string_ a message that explain the error

## TO-DO list
- [ ] Make the logic of inserting the variables into the functions more uniform trough the languages
- [ ] Include SOAP & REST sending method

## Skebby documentation
For further documentation, go on [Skebby website](http://www.skebby.it/business/index/send-docs)