Feature: Edit a customer

Scenario: Editing a customer successfully
	Given I choose a customer id: valid
	And   I choose the following data to edit:
		| operationType | fieldName  | value        |
		| replace       | name       | TestName     |
		| replace       | lastName   | TestLastName |
		| replace       | notes      | TestNotes    |
	When  I send the edit request
	Then  The http response should be 204
	And   The customer must be edited in the database

Scenario Outline: Trying to edit a customer with an invalid name
	Given I choose a customer id: valid
	And   I choose the following data to edit:
		| operationType   | fieldName   |
		| <operationType> | <fieldName> |
	When  I send the edit request
	Then  The http response should be <httpStatusCode>
	And   The response contains the following value for the ErrorMessage field: <errorMessage>

	Examples:
		| operationType | fieldName   | httpStatusCode | errorMessage       |
		| replace       | name        | 400            | Invalid name.      |
		| remove        | name        | 400            | Invalid name.      |
		| replace       | lastName    | 400            | Invalid last name. |
		| remove        | lastName    | 400            | Invalid last name. |

Scenario: Trying to edit a customer with an invalid id
	Given I choose a customer id: 111111111111111111111111
	And   I choose the following data to edit:
		| operationType | fieldName   | value    |
		| replace       | name        | TestName |
	When  I send the edit request
	Then  The http response should be 404
	And   The response contains the following value for the ErrorMessage field: Not found.
	And   The response contains the following value for the Data field: 111111111111111111111111