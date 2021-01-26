@web
Feature: My Order

# This is an alternative to the @login tag
#Background: 
#	Given I am logged in

@login
Scenario: Practice: Apply defaults
	Given my order contains the following items
		| name       | 
		| Margherita | 
		| BBQ        | 
	When I check the my order page
	Then my order should contain the ordered items

@login
Scenario: Client adds the first pizza to the order
	Given my order is empty
	When I add a large "Margherita" pizza to my order
	Then my order should contain the following items
		| name       |
		| Margherita |

@login
Scenario: Client adds a pizza to an existing order
	Given my order contains the following items
		| name       |
		| Margherita |
		| BBQ        |
	When I add a large "Margherita" pizza to my order
	Then my order should contain the following items
		| name       |
		| Margherita |
		| Margherita |
		| BBQ        | 
