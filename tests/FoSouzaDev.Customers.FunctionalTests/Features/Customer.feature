Feature: Test the possible actions on a customer registration

Scenario: Register a customer
	Given I choose valid random data for a new client
	When  I send a registration request
	Then  The request response must be successful with status code 201
	And   The response contais the inserted id
	And   The customer must exist in the database