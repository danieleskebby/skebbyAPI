# Ruby skebby API SDK

This is the Ruby version of Skebby API SDK.

## Usage

Here's the examples that uses the functions inside the Class

### Inclusion and class initialization
```Ruby
# Include the file that contains the SDK
require_relative 'skebbyAPI'

# Call the skebbyAPI class and set the credentials 
sms = SkebbyAPI.new('username','password')
```

### Send a SMS message
```Ruby
# Set the recipients
recipients = ['391234567890']

# Call the sendSMS function
send = sms.sendSMS( recipients, 'It's easy to send an SMS :)' )

# Print the result
sms.printResult( send )
```

### Check available credit
```Ruby
# Call the getCredit function
credit = sms.getCredit()

# Print the result
sms.printResult( credit )
```

### Register an Alphanumeric Sender (Alias)
```Ruby
# Call the addAlias function
alias_string = sms.addAlias( 'Skebby', 'Skebby', 'IT', '111222333444', '111222333444', 'Via Melzo 12', 'Milan', '20100', 'contact@email.com' )

# Print the result
sms.printResult( alias_string )
```