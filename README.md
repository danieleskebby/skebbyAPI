# skebbyAPI Version 0.1 beta

The goal of this project is to give the developers a tool that can be integrated inside the application in no time, it can be extremely easy to use and can be readable.

## Functions available
The functions available by the API are:

- `sendSMS`: sends a SMS message to a given array of recipients. Return a querystring with the result of the request.
- `getCredit`: get the the available currency on your Skebby account, and gives you the SMSs available, divided by type.
- `addAlias`: sends an alphanumeric sender (aka Alias) on approvation by AGCOM.

The API is based on calling the Skebby Gateway Server via HTTP POST Request. Developing a class that'll include SOAP and REST is on the to-do list.

## API Installation
- download the class file of the programming language of your choice
- put it in the same directory of your application (or import it on your project made by an IDE)
- declare the class and
- use its functions

Here's a brief example in PHP:

```php
<?php
// include API file
require_once 'skebbyAPI.php';

// declare skebbyAPI class
$sms = new skebbyAPI('username','password');

// use sendSMS function to send a SMS
$send = $sms->sendSMS( array('391234567890'), 'test classe skebbyAPI via PHP' );

// display results
print_r( $send );
?>
```

## TO-DO list
- Make the logic of inserting the variables into the functions more uniform trough the languages
- Include SOAP & REST sending method

## Skebby documentation
To read about Skebby Gateway references and variables, you can see them on [Skebby website](http://www.skebby.it/business/index/send-docs)