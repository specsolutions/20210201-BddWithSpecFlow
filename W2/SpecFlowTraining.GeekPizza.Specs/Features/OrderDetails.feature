@web
Feature: Order Details

@login
Scenario: Pizza is ordered for today by default
	Given I have items in my order
	When I check the my order page
	Then my order should indicate that the delivery date is today

@login
Scenario: Pizza is ordered for tomorrow
	Given I have items in my order
	When I specify tomorrow at 14:00 as delivery time
	Then my order should indicate that the delivery date is tomorrow
	And the delivery time should be 2pm

@login
Scenario: Practice: Pizza is ordered for a different date
	Given I have items in my order
	When I specify 5 days later at 1pm as delivery time
	Then my order should indicate that the delivery date is 5 days later

@login
Scenario Outline: Pizza is ordered for different dates and times
	Given I have items in my order
	When I specify <date> at <time> as delivery time
	Then my order should indicate that the delivery date is <date>

Examples: 
	| case                | date         | time     |
	| usual case          | today        | earliest |
	| later today         | today        | 5pm      |
	| tomorrow lunch      | tomorrow     | noon     |
	| meeting in two days | 2 days later | 13:30    |
