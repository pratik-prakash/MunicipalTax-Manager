### Case Scenario: 
A small application which manages taxes applied in different municipalities, where the taxes are scheduled in time, providing the user an ability to get taxes applied in certain municipality at the given day.

### Logic Implemented:
Return the tax applied for the first/shortest period of time available for the given municipality and the given date falling in range of available scheduled time period, after required data validations.

### Project Structure/Organization:

**Solution Project:** *MunicipalTax Manager*

**I. MunicipalTax.Management** – Contains Business/Service and Data Access Layer.

    1. MunicipalTax.Common: Contains service interfaces, global data models and utility class for data manipulation/parsing.
    
    2. MunicipalTax.DAL: Contains interfaces, implementation for database CRUD operations, database context – schema & migrations.
        - TaxManagerDB.db: Empty Sqlite Database with entity schema.
        - MunicipalityTaxDetailsSample.csv: Sample data file for municipality tax details import.
    
    3. MunicipalTax.Entity: Contains model for database entity/tables and related data type helpers.
    
    4. MunicipalTax.Services: Contains CRUD service logic, file data import implementation, with data and logic validation.

**II. MunicipalTax.Web** – Contains the WebApis.

    1. MunicipalTax.WebApi: Contains endpoint controllers for service layer.
	    - TaxManagerDB.db: Sqlite Database with sample data for municipality taxes data storage.

**III. MunicipalTax.Consumer** – Contains the consumer console client application to use the producer service.

    - App.config: Contains the producer service base endpoint configuration settings.
    
**IV. MunicipalTax.Tests** – Contains the unit tests.

    1. MunicipalTax.UnitTest: Contains the unit test implementation for service layer, with mocking used for data access layer.

### Service Endpoints:
  **1. GET Requests:**
  
      a. GET:
        - URI parameters: ?municipalityName=Copenhagen&dateString=2016.01.01
        
      b. GET:/GetMTax
        - URI parameters: ?municipalityName=Copenhagen&date=2016-01-01T00:00:00
   
   **2. POST Requests:**
   
      a. POST:/AddMTax
        - Payload: {"MunicipalityName" : "Copenhagen", "TaxType" : 3, "StartDate" :  "2016-01-01T00:00:00", "EndDate" : "2016-12-31T00:00:00", "Tax" : 0.2}
        
      b. POST:/ImportMTaxDataFromFile
        - URI parameters: ?filePath=C:\temp\MunicipalityTaxDetails.csv
        
   **3. PUT Requests:**
   
      a. PUT:
         - Payload: {"MunicipalityName" : "Copenhagen", "TaxType" : 3, "StartDate" :  "2016-01-01T00:00:00", "EndDate" : "2016-12-31T00:00:00", "Tax" : 0.2}
