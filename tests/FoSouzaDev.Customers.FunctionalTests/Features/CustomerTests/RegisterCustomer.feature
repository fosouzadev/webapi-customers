Feature: Register a customer

Scenario: Register a customer
	Given I choose valid random data for a new client
	When  I send a registration request
	Then  The http response should be 201
	And   The response contais the inserted id
	And   The customer must exist in the database