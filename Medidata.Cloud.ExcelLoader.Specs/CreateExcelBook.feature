Feature: CreateExcelBook
	In order to create Excel workbook
	As a ExcelLoader user
	I want to create workbook

Scenario: Create workbook based on sheet model
	Given I have a loader associated with a sheet model class
	When I add an instance of the sheet model
	And I save the workbook
	Then there should be a worksheet with the name defined on the sheet model
	And there should be a row that matches the sheet model
