@web
Feature: Authentication

Scenario: Client logs in with valid credentials
	Given there is a user registered with user 'Ford' and password '1234'
	And I am on the login page
	When I log in with user 'Ford' and password '1234'
	Then the home page should be opened
