# Python skebby API SDK

This is the Python version of Skebby API SDK.

## Usage

Here's the examples that uses the functions inside the Class

### Inclusion and class initialization
```Python
# Include the file that contains the SDK
from skebbyAPI import skebbyAPI

# Call the skebbyAPI class and set the credentials 
sms = skebbyAPI('username','password')
```

### Send a SMS message
```Python
# Set the data inside an array
parameters = {
    'text': ''
    'recipients': ['391234567890']
}

# Call the sendSMS function
send_sms = sms.sendSMS( parameters )

# Print the result
sms.printResult( send_sms )
```

### Check available credit
```Python
# Call the getCredit function
credit = sms.getCredit()

# Print the result
sms.printResult( credit )
```

### Register an Alphanumeric Sender (Alias)
```Python
# Set the parameters inside an array
parameters = {
  'alias': 'Skebby',
  'business_name': 'Skebby'
  'nation': 'IT'
  'vat_number': '111222333444'
  'taxpayer_number': '111222333444'
  'street': 'Via Melzo 12'
  'city': 'Milan'
  'postcode': '20100'
  'contact': 'contact@email.com'
}

# Call the addAlias function
alias = sms.addAlias( parameters )

# Print the result
sms.printResult( alias )
```