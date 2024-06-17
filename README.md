Tax Calculator Web API

Overview
This project is a Tax Calculator Web API application developed using .NET 6. The implementation follows SOLID principles and Clean Code architecture.Technologies and frameworks used in this project include Dapper, SQLite, InMemory, Serilog, XUnit for testing, and Docker for containerization.

Technologies Used
•	.NET 6: The framework used to build the web API.
•	Dapper: ORM for data access.
•	SQLite: The database used to store tax configuration parameters.
•	InMemory: For caching and in-memory operations.
•	Serilog: Logging library for structured logging.
•	XUnit: Testing framework for unit tests.
•	Docker: Containerization for running the application in isolated environments.

Project Structure
•	TaxCalculator.API: Contains the main API project and database initialization logic.
•	TaxCalculator.Domain: Contains domain entities, interfaces, enums and valueobjects.
•	TaxCalculator.Infrastructure: Contains the implementation of services, helpers and data access using Dapper.
•	TaxCalculator.Application: Contains business logic and service layer.
•	TaxCalculator.Tests: Contains unit tests covering all calculations and functionalities.

Database
•	SQLite Database: The database is stored in the “TaxCalculator.API” called as “exadel” project. It includes a “TaxConfig” table that holds all parameters for tax calculations.
•	TaxConfig Table: Parameters stored in this table can be modified without changing the code, providing flexibility.

Initial Database Setup
On project startup, the application creates the SQLite database as  “exadel” and initializes the “TaxConfig” table with default parameter values located in the “TaxCalculator.API/” folder.

Adding New Tax Types
The application is designed with the Open-Closed Principle in mind, allowing easy extension. To add a new tax type:
1.	Create a new class implementing the “ITaxCalculator” interface in the “TaxCalculator.Infrastructure/Services” folder.
2.	Implement the required methods.
3.	The new tax type will be automatically integrated into the tax calculation process.
This design makes the project easy to extend without modifying existing code.

Unit Testing
The project includes comprehensive unit tests to cover all possible scenarios for tax calculations. XUnit is used as the testing framework to ensure code quality and correctness.

Logging
Serilog is used for structured logging, making it easier to trace and debug the application.

Docker Support
The project includes Docker support for running the application.

Getting Started

Prerequisites
•	.NET 6 SDK
•	Docker (for containerization)
•	SQLite

Running the Application
1.	Clone the Repository:
    git clone https://github.com/memo330102/TaxCalculator.git
2.	Navigate to the Project Directory:
    cd TaxCalculator.API
3.	Build and Run the Application:
    dotnet build and dotnet run
  	
 Conclusion
This Tax Calculator Web API is built using .NET 6 and leverages Dapper, SQLite, Serilog, and XUnit for efficient tax calculations and robust testing. 
The design adheres to SOLID principles and Clean Code architecture, ensuring maintainability and scalability.
The project supports easy extension for new tax types and includes comprehensive unit tests. Docker support simplifies deployment across different environments.




