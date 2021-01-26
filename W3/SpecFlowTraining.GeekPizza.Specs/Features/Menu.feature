@web
Feature: Menu

Scenario: All pizzas from the menu are displayed on the menu page
	Given there are 5 pizzas on the menu
	When I check the menu page
	Then there should be 5 pizzas displayed

Scenario: The menu is displayed in alphabetical order on the menu page
	Given the following pizzas are on the menu
		| name             | ingredients                                          | calories |
		| Margherita       | tomato slices, oregano, mozzarella                   | 1920     |
		| Fitness          | sweetcorn, broccoli, feta cheese, mozzarella         | 1340     |
		| BBQ              | BBQ sauce, bacon, chicken breast strips, onions      | 1580     |
		| Mexican          | taco sauce, bacon, beans, sweetcorn, mozzarella      | 2340     |
		| Quattro formaggi | blue cheese, parmesan, smoked mozzarella, mozzarella | 2150     |
	When I check the menu page
	Then the following pizzas should be displayed in this order
		| name             |
		| BBQ              |
		| Fitness          | 
		| Margherita       |
		| Mexican          |
		| Quattro formaggi |
