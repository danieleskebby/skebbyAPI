#!/usr/bin/perl
use LWP::UserAgent;
use URI::Escape;
 
use constant NET_ERROR => "Network+error,+unable+to+send+the+message";
use constant SENDER_ERROR => "You+can+specify+only+one+type+of+sender,+numeric+or+alphanumeric";
 
package skebbyAPI;
sub new {
	my $class = shift;
	my $self = {
		_user => shift,
		_pass => shift
	};
	bless $self, $class;
	return $self;
}
# URL encoding/decoding functions
sub URLEncode {
    my $theURL = $_[1];
	
    $theURL =~ s/([\W])/"%" . uc(sprintf("%2.2x",ord($1)))/eg;
    return $theURL;
}
sub URLDecode {
    my $theURL = $_[1];
    
	$theURL =~ tr/+/ /;
    $theURL =~ s/%([a-fA-F0-9]{2,2})/chr(hex($1))/eg;
    $theURL =~ s/<!-(.|\n)*->//g;
    return $theURL;
}
# Core function
sub do_post_request {
	my $self = shift;
	my $data = $_[0];
	
	$ua = new LWP::UserAgent;
    $req = new HTTP::Request "POST","http://gateway.skebby.it/api/send/smseasy/advanced/http.php";
    $req->content_type("application/x-www-form-urlencoded");

	$req->content($data);
	
    $res = $ua->request($req);
	
    if ($res->is_error) {
        %results = ();
        $results{"status"} = "failed";
        $results{"code"} = "0";
        $results{"message"} = $self->URLDecode(NET_ERROR);
        return %results;
    }
 
    %results = ();
    @result = split("&", $res->content);
    foreach (@result) {
        @temp = split("=",$_);
        $results{$temp[0]} = $temp[1];
    }
 
    return %results;
}

sub sendSMS {
	my $self = shift;
	my ($recipients,$text,$method,$sender_number,$sender_string,$charset,$delivery_start,$encoding_scheme,$validity_period,$user_reference) = @_;
	
	$method = ($method ne "" ? $method : "send_sms_classic");
    
	$data = "method=".$method."&text=".$self->URLEncode($text)."&username=".$self->URLEncode($self->{_user})."&password=".$self->URLEncode($self->{_pass}).$recipients;
    print $data."\n";
    
    if($sender_number ne "" && $sender_string ne "") {
        %results = ();
        $results{"status"} = "failed";
        $results{"code"} = "0";
        $results{"message"} = $self->URLDecode(SENDER_ERROR);
        return %results;
    }
 
    $data = $data . ($sender_number ne "" ? "&sender_number=".$self->URLEncode($sender_number) : "");
    $data = $data . ($sender_string ne "" ? "&sender_string=".$self->URLEncode($sender_string) : "");
    $data = $data . ($charset ne "" ? "&charset=".$self->URLEncode($charset) : "");
    $data = $data . ($delivery_start ne "" ? "&delivery_start=".$self->URLEncode($delivery_start) : "");
    $data = $data . ($encoding_scheme ne "" ? "&encoding_scheme=".$self->URLEncode($encoding_scheme) : "");
    $data = $data . ($validity_period ne "" ? "&validity_period=".$self->URLEncode($validity_period) : "");
    $data = $data . ($user_reference ne "" ? "&user_reference=".$self->URLEncode($user_reference) : "");
    
	%result = $self->do_post_request($data);
    return %results;
}

sub getCredit {
	my $self = shift;
	my $charset = $_[0];
	$method = "get_credit";
	
	$data = "method=".$method."&username=".$self->URLEncode($self->{_user})."&password=".$self->URLEncode($self->{_pass});
	
    $data = $data . ($charset ne "" ? "&charset=".$self->URLEncode($charset) : "");
	
	%result = $self->do_post_request($data);
    return %results;
}

sub addAlias {
	my $self = shift;
	my ($alias,$business_name,$nation,$vat_number,$taxpayer_number,$street,$city,$postcode,$contact,$charset) = @_;
	
	$method = "add_alias";
	$data = "method=".$method."&username=".$self->URLEncode($self->{_user})."&password=".$self->URLEncode($self->{_pass});
	
	$data = $data . "&alias=".$self->URLEncode($alias);
	$data = $data . "&business_name=".$self->URLEncode($business_name);
	$data = $data . "&nation=".$self->URLEncode($nation);
	$data = $data . "&vat_number=".$self->URLEncode($vat_number);
	$data = $data . "&taxpayer_number=".$self->URLEncode($taxpayer_number);
	$data = $data . "&street=".$self->URLEncode($street);
	$data = $data . "&city=".$self->URLEncode($city);
	$data = $data . "&postcode=".$self->URLEncode($postcode);
	$data = $data . "&contact=".$self->URLEncode($contact);
	
    $data = $data . ($charset ne "" ? "&charset=".$self->URLEncode($charset) : "");
	
	%result = $self->do_post_request($data);
    return %results;
}

sub printResult {
    my $self = shift;
    my (%result) = @_;
    while (($k, $v) = each(%result)) {
        print "$k: $v\n";
    }
}
1;