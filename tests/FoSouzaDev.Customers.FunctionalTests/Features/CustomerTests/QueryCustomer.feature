Feature: Consult a customer

Scenario: Successfully consulting a customer
	Given I choose a customer id: valid
	When  I send a query request
	Then  The http response should be 200
	And   The response contains the requested customer data

Scenario: Trying to consult a non-existent customer
	Given I choose a customer id: 111111111111111111111111
	When  I send a query request
	Then  The http response should be 404
	And   The response contains the following value for the ErrorMessage field: Not found.
	And   The response contains the following value for the Data field: 111111111111111111111111