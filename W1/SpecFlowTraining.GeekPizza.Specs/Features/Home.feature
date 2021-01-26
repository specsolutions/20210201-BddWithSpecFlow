@web
Feature: Home

Scenario: Welcome message is displayed on home page
	When I check the home page
	Then a message should be displayed: "Welcome to Geek Pizza!"
