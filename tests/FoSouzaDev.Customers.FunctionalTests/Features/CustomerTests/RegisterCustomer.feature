Feature: Register a customer

Scenario: Registering a customer successfully
	Given I choose valid random data for a new client
	When  I send a registration request
	Then  The http response should be 201
	And   The response contais the inserted id
	And   The customer must exist in the database

Scenario Outline: Trying to add a customer with invalid data
	Given I choose the data for a new customer with an invalid <invalidData>
	When  I send a registration request
	Then  The http response should be 400
	And   The response contains the following value for the ErrorMessage field: <errorMessage>

	Examples:
		| invalidData   | errorMessage       |
		| email         | Invalid email.     |
		| name          | Invalid name.      |
		| last name     | Invalid last name. |
		| date of birth | Invalid age.       |