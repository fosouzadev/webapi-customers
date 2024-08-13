Feature: Edit a customer

Scenario: Edit a customer
	Given I choose an existing customer id
	And   I choose random valid values ​​for editing
	When  I send the edit request
	Then  The request response must be successful with status code 204
	And   The customer must be edited in the database