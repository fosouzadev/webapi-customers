Feature: Delete a customer

Scenario: Delete a customer
	Given I choose an existing customer id
	When  I send the deletion request
	Then  The http response should be 204
	And   The customer must not exist in the database