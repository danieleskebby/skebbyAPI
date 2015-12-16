<?php
// Skebby PHP Gateway Class
class skebbyAPI {
	
	protected $user;
	protected $pass;
	protected $http_url = "https://gateway.skebby.it/api/send/smseasy/advanced/http.php";
	
	const CURL_ERROR 	= 'cURL module is not installed. This module is necessary for the correct handling of HTTP requests by SkebbyAPI';
	const NET_ERROR 	= 'Network error: cURL can\'t connect to Skebby URL';
	const EMPTY_ERROR 	= 'One+or+more+of+the+required+variables+is+missing%3A+';
	const SENDER_ERROR 	= 'You+can+specify+only+one+type+of+sender,+numeric+or+alphanumeric';
	
	public function __construct( $username, $password ) {
		$this->user = $username;
		$this->pass = $password;
	}
	
	/**
	 * Core function where the server connects to Skebby Gateway HTTP URL
	 * and sends the data via cURL.
	 *
	 * @uses skebbyAPI::cleanPhoneNumbers( $recipients )
	 * @access protected
	 * @return string
	 */
	protected function doRequest($data) {
		try {
			$data_string = 'username='.urlencode( $this->user ).'&password='.urlencode( $this->pass );
			
			foreach( $data as $key => $val ){
				$recipients_string = '';
				if($key == 'recipients') {
					if( is_array($val[0]) && array_key_exists( 'recipient', $val[0] ) ) {
						for($i = 0; $i < count($val); $i++) {
							foreach( $val[$i] as $k => $v ) {
								$recipients_string .= '&recipients['.$i.']['.urlencode($k).']='.( ($k == 'recipient') ? $this->cleanPhoneNumbers($v) : urlencode($v) );
							}
						}
					} else {
						$recipients_string = '&recipients[]='.implode('&recipients[]=',$this->cleanPhoneNumbers($val));
					}
					$data_string .= $recipients_string;
				} else {
					$data_string .= '&'.urlencode($key).'='.urlencode($val);	
				}
			}
			
			if(!function_exists('curl_init')) { throw new Exception(self::CURL_ERROR); }
			$ch = curl_init();
			
			curl_setopt($ch,CURLOPT_CONNECTTIMEOUT,10);
			curl_setopt($ch,CURLOPT_USERAGENT,'Skebby PHP Gateway - HTTP POST method');
			curl_setopt($ch,CURLOPT_URL,$this->http_url);
			curl_setopt($ch,CURLOPT_TIMEOUT,60);
			curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
			curl_setopt($ch,CURLOPT_POST,true);
			curl_setopt($ch,CURLOPT_POSTFIELDS,$data_string);
			
			$response = curl_exec($ch);
			curl_close($ch);
			if(!$response){ throw new Exception(self::NET_ERROR); }
			return $response;
		} catch (Exception $e) {
			return $e;
		}
	}
	
	/**
	 * Main function to send the SMS. Returns a querystring or an XML, depending 
	 * on what sending method you choose before.
	 * 
	 * @uses skebbyAPI::doRequest( $data )
	 * @access public
	 * @return mixed
	 */
	public function sendSMS( $data ) {
		$error = '';
		$required = array( 
			'text', 
			'recipients'
		);
		foreach( $required as $r ) {
			if( !isset($data[ $r ]) ) {
				$error .= $r . '%2C+';
			}	
		}
		if( $error != '' ) { return (string) 'status=failed&message='.self::EMPTY_ERROR.(substr($error,0,-4)); }
		
		if(!isset($data['method'])) { $data['method'] = 'send_sms_classic'; }
		if(!is_array($data['recipients'])) { $data['recipients'] = array($data['recipients']); }
	    if($data['method'] != 'send_sms_basic') {
		    if(isset($data['sender_number']) && isset($data['sender_string'])) {
		        return 'status=failed&code=10&message='.self::SENDER_ERROR;
		    }
	    }
	    
	    return $this->doRequest($data);
	}

	/**
	 * Function to check the current balance. Returns a querystring or an XML, depending 
	 * on what sending method you choose before.
	 *
	 * @uses skebbyAPI::doRequest( $data )
	 * @access public
	 * @return string
	 */
	public function getCredit( $charset = '' ) {
		$data = array( 'method' => 'get_credit' );
		if($charset == 'UTF-8') { $data['charset'] = 'UTF-8'; }
		
	    return $this->doRequest($data);
	}
	
	/**
	 * Function to add an Alias, an alphanumeric sender_string, to be validated. Returns a querystring or an XML, depending 
	 * on what sending method you choose before.
	 *
	 * @uses skebbyAPI::doRequest( $data )
	 * @access public
	 * @return string
	 */
	public function addAlias( $data ) {
		$error = '';
		$required = array(
			'alias',
			'business_name',
			'nation',
			'vat_number',
			'taxpayer_number',
			'street',
			'city',
			'postcode',
			'contact'
		);
		foreach( $required as $r ) {
			if( !isset($data[ $r ]) ) {
				$error .= $r . '%2C+';
			}	
		}
		if( $error != '' ) { return 'status=failed&code=10&message='.self::EMPTY_ERROR.(substr($error,0,-4)); }
		
		$data['method'] = 'add_alias';
	    return $this->doRequest($data);
	}
	
	/**
	 * Function to convert the POST Request result to an associative array instead of a querystring
	 * 
	 * @access public
	 * @return array
	 */
	public function querystringToArray($data) {
		parse_str($data,$result);
		return $result;
	}
	
	/**
	 * Function to clean the phone numbers from '00' and '+' international prefixes
	 * 
	 * @access protected
	 * @return array
	 */
	protected function cleanPhoneNumbers($num) {
		for( $i = 0; $i < count($num); $i++ ) {
			$num[$i] = ltrim($num[$i], '0+');
			$num[$i] = preg_replace('/[^0-9]+/', '', $num[$i]);
		}
		return $num;
	}
}
?>