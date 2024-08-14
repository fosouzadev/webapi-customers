Feature: Edit a customer

Scenario: Editing a customer successfully
	Given I choose a customer id: valid
	And   I choose random valid values ​​for editing
	When  I send the edit request
	Then  The http response should be 204
	And   The customer must be edited in the database

Scenario Outline: Trying to edit a customer with invalid data
	Given I choose a customer id: <id>
	And   I choose the data to edit a customer with an invalid <invalidData>
	When  I send the edit request
	Then  The http response should be <httpStatusCode>
	And   The response contains the following value for the ErrorMessage field: <errorMessage>
	And   The response contains the following value for the Data field: <data>

	Examples:
		| id                       | invalidData   | httpStatusCode | errorMessage       | data                     |
		| valid                    | name          | 400            | Invalid name.      | null                     |
		| valid                    | last name     | 400            | Invalid last name. | null                     |
		| 111111111111111111111111 | none          | 404            | Not found.         | 111111111111111111111111 |