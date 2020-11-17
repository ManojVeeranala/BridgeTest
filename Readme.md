# Teacher Tablet Battery Project

## How to Run

### Service

* Set the Json Source Path in the appsettings.json settingname: jsonDataSourcePath
* Set the logfile folder path in nlog.config in API project
* Set TeacherTablet.API as Startup Project in Visual Studio
* Press F5
* you will see the swagger page

### Tests

* Under Test Menu in Visual Studio select TestExplorer
* Build the solution, the you will see the tests
* Run the tests using TestExplorer

## Design

* Developed Asp.Net Core API using 3.1 of .Net Core with 3 Layered Architecture
* Developed a get endpoint, which returns all the devices batteries average daily usage
* If Jsonfile is not valid we return 500 from API
* Developed a middleware for exception handling
* Added Open API package for api documentation
* If any unhandled error happends, error will be logged
* Used multi-threading for better performance

## Room for Improvement

* In case of device doesn't send the datapoints for days, we may consider inhouse storage in Tablet, so it records the readings with a specified frequency and sync the data when connected to network.
