<?php
// Skebby PHP Gateway Class
class skebbyAPI {
	
	protected $user;
	protected $pass;
	protected $send_method;
	protected $http_url = "https://gateway.skebby.it/api/send/smseasy/advanced/http.php";
	protected $soap_url = "https://gateway.skebby.it/api/send/smseasy/advanced/soapv3.php?wsdl";
	protected $rest_url = "https://gateway.skebby.it/api/send/smseasy/advanced/rest.php";
	
	const METHOD_ERROR = "Method+error:+method+is+not+supported";
	const NET_ERROR = "Network+error:+unable+to+send+the+message";
	const SENDER_ERROR = "You+can+specify+only+one+type+of+sender,+numeric+or+alphanumeric";
	
	public function __construct( $username, $password, $method = NULL ) {
		$this->user = $username;
		$this->pass = $password;
		$this->send_method = ($method != NULL) ? strtoupper($method) : 'HTTP';
	}
	
	/**
	 * Core function where the server connects to Skebby Gateway HTTP URL
	 * and sends the data via CURL.
	 *
	 * @access protected
	 * @return mixed
	 */
	protected function doRequest($data) {
		try {
			$data_string = '';
			
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
			$data_string = substr( $data_string, 1 );
			
			if(!function_exists('curl_init')) { throw new Exception('cUrl module is not installed.'); }
			$ch = curl_init();
			
			echo $this->soap_url."#".$data['method']."\n";
			
			switch($this->send_method) {
				case 'SOAP':
					$headers = array(
						"Content-type: text/xml;charset=\"utf-8\"",
						//"Accept-Encoding: gzip, deflate",
						"Cache-Control: no-cache",
						"SOAPAction:".$this->soap_url."#".$data['method'],
						"Host: gateway.skebby.it",
						"Connection: Keep-Alive",
						"Content-length: ".strlen($data_string)
                    );
					curl_setopt($ch,CURLOPT_HTTPHEADER,$headers);
					curl_setopt($ch,CURLOPT_USERAGENT,'Skebby PHP Gateway - HTTP SOAP method');
					curl_setopt($ch,CURLOPT_URL,$this->soap_url);
					break;
				case 'REST':
					curl_setopt($ch,CURLOPT_CONNECTTIMEOUT,10);
					curl_setopt($ch,CURLOPT_USERAGENT,'Skebby PHP Gateway - HTTP REST method');
					curl_setopt($ch,CURLOPT_URL,$this->rest_url);
					break;
				case 'HTTP':
					curl_setopt($ch,CURLOPT_CONNECTTIMEOUT,10);
					curl_setopt($ch,CURLOPT_USERAGENT,'Skebby PHP Gateway - HTTP POST method');
					curl_setopt($ch,CURLOPT_URL,$this->http_url);
					break;
				default:
					return 'status=failed&message='.METHOD_ERROR;
			}
			
			curl_setopt($ch,CURLOPT_TIMEOUT,60);
			curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
			curl_setopt($ch,CURLOPT_POST,true);
			curl_setopt($ch,CURLOPT_POSTFIELDS,$data_string);
			
			$response = curl_exec($ch);
			curl_close($ch);
			if(!$response){
				return 'status=failed&message='.NET_ERROR;
			}
			return $response;	
		} catch (Exception $e) {
			return $e;
		}
	}
	
	/**
	 * Function to clean the phone numbers from '00' and '+' international prefixes
	 * 
	 * @access protected
	 * @return array
	 */
	protected function cleanPhoneNumbers($recipients) {
		for( $i = 0; $i < count($recipients); $i++ ) {
			$recipients[$i] = ltrim($recipients[$i], '0+');
			$recipients[$i] = preg_replace('/[^0-9]+/', '', $recipients[$i]);
		}
		return $recipients;
	}
	
	/**
	 * Main function to send the SMS. Returns a querystring or an XML, depending 
	 * on what sending method you choose before.
	 * 
	 * @uses skebbyAPI::doRequest( $data )
	 * @uses skebbyAPI::cleanPhoneNumbers( $recipients )
	 * @access public
	 * @return mixed
	 */
	public function sendSMS(
		$recipients,
		$text,
		$method='send_sms_classic',
		$sender_number='',
		$sender_string='',
		$charset='',
		$delivery_start='',
		$encoding_scheme='',
		$validity_period='',
		$user_reference=''
	) {
		if (!is_array($recipients)){
			$recipients = array($recipients); 
		}
		$parameters = array(
			'method' => $method,
			'username' => $this->user,
			'password' => $this->pass,
			'recipients' => $recipients,
			'text' => $text
		);
        
		if($delivery_start != '') { $parameters['delivery_start'] = $delivery_start; }
		if($charset == 'UTF-8') { $parameters['charset'] = 'UTF-8'; }
	    
	    if($method != 'send_sms_basic') {
		    if($sender_number != '' && $sender_string != '') {
		        parse_str('status=failed&message='.SENDER_ERROR,$result);
		        return $result;
		    }
			if($sender_number != '') { $parameters['sender_number'] = $sender_number; }
			if($sender_string != '') { $parameters['sender_string'] = $sender_string; }
			if($validity_period != '') { $parameters['validity_period'] = $validity_period; }
			if($user_reference != '') { $parameters['user_reference'] = $user_reference; }
			if($encoding_scheme == 'UCS2') { $parameters['validity_period'] = 'UCS2'; }
	    }
	    
	    return $this->doRequest($parameters);
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
		$parameters = array(
			'method' => 'get_credit',
			'username' => $this->user,
			'password' => $this->pass
		);
		if($charset == 'UTF-8') { $parameters['charset'] = 'UTF-8'; }
		
	    return $this->doRequest($parameters);
	}
	
	/**
	 * Function to add an Alias, an alphanumeric sender_string, to be validated. Returns a querystring or an XML, depending 
	 * on what sending method you choose before.
	 *
	 * @uses skebbyAPI::doRequest( $data )
	 * @access public
	 * @return string
	 */
	public function addAlias(
		$alias,
		$business_name,
		$nation,
		$vat_number,
		$taxpayer_number,
		$street,
		$city,
		$postcode,
		$contact,
		$charset=''
	) {
		$parameters = array(
			'method' => 'add_alias',
			'username' => $this->user,
			'password' => $this->pass,
			'alias' => $alias,
			'business_name' => $business_name,
			'nation' => $nation,
			'vat_number' => $vat_number,
			'taxpayer_number' => $taxpayer_number,
			'street' => $street,
			'city' => $city,
			'postcode' => $postcode,
			'contact' => $contact
		);
		if($charset == 'UTF-8') { $parameters['charset'] = 'UTF-8'; }
	    
	    return $this->doRequest($parameters);
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
}
?>