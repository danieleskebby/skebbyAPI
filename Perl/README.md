# Perl skebby API SDK

This is the Perl version of Skebby API SDK.

## Usage

Here's the examples that uses the functions inside the Class

### Inclusion and class initialization
```Perl
# Include the file that contains the SDK
use skebbyAPI;

# Call the skebbyAPI class and set the credentials 
$sms = skebbyAPI->new("username","password");
```

### Send a SMS message
```Perl
# Set the recipients
$recipients = ('391234567890');
$recipients = "&recipients[]=".join("&recipients[]=", $recipients);

# Call the sendSMS function
%send = $sms->sendSMS($recipients,'test classe skebbyAPI via Perl');

# Print the result
sms->printResult(%send);
```

### Check available credit
```Perl
# Call the getCredit function
%credit = sms->getCredit();

# Print the result
sms->printResult(%credit);
```

### Register an Alphanumeric Sender (Alias)
```Perl
# Call the addAlias function
%alias = sms->addAlias( 'Skebby', 'Skebby', 'IT', '111222333444', '111222333444', 'Via Melzo 12', 'Milan', '20100', 'contact@email.com' );

# Print the result
sms->printResult(%alias);
```