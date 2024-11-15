<h1>ASP.NET Core Teacher Application</h1>
<hr>
An ASP.NET Core project demonstrating a teacher management application with a search feature using parameterized SQL queries.

Overview
This project includes:

Models/SchoolDbContext.cs: Manages the database connection (ensure the connection string is correctly set).
Controllers/TeacherAPIController.cs: Provides API access to information about teachers.
Program.cs: Configures and runs the application.
Installation Guide
Set Up MySQL Environment: Download and install MAMP, XAMPP, or another MySQL environment.
Create Database:
Access phpMyAdmin and create a new database called school.
Import SQL Files:
Import teachers.sql, courses.sql, students.sqland  studentxcourses.sql into the school database via phpMyAdmin.
Configure Database Connection:
Update connection string details in /Models/SchoolDbContext.cs (User, Pass, Port, Database, Server).
Install MySQL.Data:
In Visual Studio, go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution > Browse, search for MySQL.Data, and install it.
Run the Project:
Start the database environment, and then run the project in debug mode (F5). Test the API by calling GET api/Teacher/ListTeachers.
Common Errors & Troubleshooting
Connection Refused Error: Ensure your database is running and the connection string is accurate.
Authentication Failed Error: Verify username and password fields are correct.
MAMP Issues: If MAMP is incompatible, try XAMPP or another MySQL environment.
Exercises
Enhance your understanding by completing these tasks:

Exercise 5: Implement a teacher search feature based on names.
Exercise 6: Add a department search feature to find departments by name.
