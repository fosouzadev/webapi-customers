Feature: Delete a customer

Scenario: Successfully deleting a customer
	Given I choose a customer id: valid
	When  I send the deletion request
	Then  The http response should be 204
	And   The customer must not exist in the database

Scenario: Trying to delete a non-existent customer
	Given I choose a customer id: 111111111111111111111111
	When  I send the deletion request
	Then  The http response should be 404
	And   The response contains the following value for the ErrorMessage field: Not found.
	And   The response contains the following value for the Data field: 111111111111111111111111