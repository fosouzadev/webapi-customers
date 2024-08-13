Feature: Edit a customer

Scenario: Edit a customer
	Given I choose an existing customer id
	And   I choose random valid values ​​for editing
	When  I send the edit request
	Then  The http response should be 204
	And   The customer must be edited in the database