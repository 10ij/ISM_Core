# App

This app needs to replace an old Access based system which did the following

 - Stored Lists of Students
 - Stored their information
 - Counted their fees
 - Allowed entry of paid fees
 - Generate Invoice PDFs and calculate total owed
 - Stored Lists of Teachers
 - Stored their information
 - Generated sign in sheets for them
 - Generated estimated pay for the reporting period
 - Generated reports for upper managemnet to get a glimpse of finances
    - Generate fee balance reports
    - Generate fee summaries by class and location
    - Generate student statements
    - Generate invoices and receipts
    - Generate daily payment reports
 - Store classes, who's in it, who's teaching it
 - Stored Expenses and Income

My app will do some of the same things differently
 - Old app would have you click a button to run a batch file at the start of the month, to calculate fees
 - New app will do this automatically via scheduled task
 - Old app allowed batch entry of student payments
 - New app might keep this for user convenience
 - Old app is single desktop single user and single tenant
 - New app is web based (eventually), Multi Tenant and Multi User
 - Old app generated attendance sheets
 - New app will eventually store records
 - Old app stored student docs as a path
 - New app stores any attatchment in a central table
 

My app will be adding the following as an extra:
 - Seperation of Students and Parents in the DB
 - Store fees as something a Student is personally Liable for
 - Integrate basic scheduling of classes
 - Storing of temprorary situations like a substitute teacher