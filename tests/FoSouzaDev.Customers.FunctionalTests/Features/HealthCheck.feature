Feature: Health check

Scenario: Healthy
	Given All dependencies are ok
	When  I send the request
	Then  The http response should be 200
	And   The response data must be Healthy

Scenario: Unhealthy
	Given Database unavailable
	When  I send the request
	Then  The http response should be 503
	And   The response data must be Unhealthy