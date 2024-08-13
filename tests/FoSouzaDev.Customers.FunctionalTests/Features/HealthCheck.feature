Feature: Health check

Scenario: Healthy
	Given All dependencies are ok
	When  I send the request
	Then  The request response must be successful with status code 200
	And   The response data must be Healthy