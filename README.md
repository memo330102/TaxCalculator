Tax Calculator Web API

Overview

This project is a Tax Calculator Web API application developed using .NET 6. The implementation follows SOLID principles and Clean Code architecture.Technologies and frameworks used in this project include Dapper, SQLite, InMemory, Serilog, XUnit for testing, and Docker for containerization.

Technologies Used

1. 	.NET 6: The framework used to build the web API.
2.	Dapper: ORM for data access.
3.	SQLite: The database used to store tax configuration parameters.
4.	InMemory: For caching and in-memory operations.
5.	Serilog: Logging library for structured logging.
6.	XUnit: Testing framework for unit tests.
7.	Docker: Containerization for running the application in isolated environments.

Project Structure

1.	TaxCalculator.API: Contains the main API project and database initialization logic.
2.	TaxCalculator.Domain: Contains domain entities, interfaces, enums and valueobjects.
3.	TaxCalculator.Infrastructure: Contains the implementation of services, helpers and data access using Dapper.
4.	TaxCalculator.Application: Contains business logic and service layer.
5.	TaxCalculator.Caching: Contains the caching logic for in-memory operations, allowing easy integration with other caching mechanisms like Redis.
6.	TaxCalculator.Tests: Contains unit tests covering all calculations and functionalities.

Database

1.	SQLite Database: The database is stored in the “TaxCalculator.API” called as “exadel” project. It includes a “TaxConfig” table that holds all parameters for tax calculations.
2.	TaxConfig Table: Parameters stored in this table can be modified without changing the code, providing flexibility.

Initial Database Setup

On project startup, the application creates the SQLite database as  “exadel” located in the “TaxCalculator.API/” folder and initializes the “TaxConfig” table with default parameter values by using IHostedService.

Adding New Tax Types

The application is designed with the Open-Closed Principle in mind, allowing easy extension. To add a new tax type:
1.	Create a new class implementing the “ITaxCalculator” interface in the “TaxCalculator.Infrastructure/Services” folder.
2.	Implement the required methods.
3.	The new tax type will be automatically integrated into the tax calculation process.
This design makes the project easy to extend without modifying existing code.

Caching

Caching is implemented in the TaxCalculator.Caching project. The default implementation uses in-memory caching, but it can be extended to use other caching mechanisms like Redis.

Global Exception Handling

Global exception handling is implemented using middleware. This ensures that all exceptions are handled consistently across the application. The middleware catches exceptions like ArgumentException, InvalidOperationException, and Exception, returning appropriate HTTP status codes and error messages.

Unit Testing

The project includes comprehensive unit tests to cover all possible scenarios for tax calculations. XUnit is used as the testing framework to ensure code quality and correctness.

Logging

Serilog is used for structured logging, making it easier to trace and debug the application.

Docker Support

The project includes Docker support for running the application.

Getting Started

Prerequisites

1.	.NET 6 SDK
2.	Docker (for containerization)
3.	SQLite

Running the Application

1.	Clone the Repository:
    git clone https://github.com/memo330102/TaxCalculator.git
2.	Navigate to the Project Directory:
    cd TaxCalculator.API
3.	Build and Run the Application:
    dotnet build and dotnet run

Running With Docker

1.	 docker-compose up -d 
2.	 docker-compose down --rmi local

 Access Swagger
 
Open your browser and navigate to http://localhost:<Port>/swagger/index.html.
  	
 Conclusion
 
This Tax Calculator Web API is built using .NET 6 and leverages Dapper, SQLite, Serilog, and XUnit for efficient tax calculations and robust testing. 
The design adheres to SOLID principles and Clean Code architecture, ensuring maintainability and scalability.
The project supports easy extension for new tax types and includes comprehensive unit tests. Docker support simplifies deployment across different environments.




