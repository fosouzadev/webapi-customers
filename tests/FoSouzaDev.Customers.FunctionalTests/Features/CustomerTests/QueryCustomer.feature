Feature: Query a customer

Scenario: Query a customer
	Given I choose an existing customer id
	When  I send a query request
	Then  The request response must be successful with status code 200
	And   The response contains the requested customer data