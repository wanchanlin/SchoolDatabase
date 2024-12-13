# ASP.NET Core School Database

An ASP.NET Core project demonstrating a teacher management application with a search feature using parameterized SQL queries.

---

## Overview

This project includes:

- **`Models/SchoolDbContext.cs`**: Manages the database connection (ensure the connection string is correctly set).
- **`Controllers/TeacherAPIController.cs`**: Provides API access to information about teachers.
- **`Program.cs`**: Configures and runs the application.

---

## Installation Guide

1. **Set Up MySQL Environment**  
   Download and install [MAMP](https://www.mamp.info/), [XAMPP](https://www.apachefriends.org/), or another MySQL environment.

2. **Create Database**  
   - Access phpMyAdmin and create a new database called `school`.

3. **Import SQL Files**  
   - Import the following SQL files into the `school` database via phpMyAdmin:  
     - `teachers.sql`  
     - `courses.sql`  
     - `students.sql`  
     - `studentxcourses.sql`

4. **Configure Database Connection**  
   - Update the connection string details in `/Models/SchoolDbContext.cs`:
     - **User**: Database username
     - **Pass**: Database password
     - **Port**: Port number for MySQL
     - **Database**: Name of the database (e.g., `school`)
     - **Server**: MySQL server address (e.g., `localhost`)

5. **Install MySQL.Data**  
   - In Visual Studio:
     1. Navigate to `Tools > NuGet Package Manager > Manage NuGet Packages for Solution`.
     2. Under the `Browse` tab, search for `MySQL.Data`.
     3. Install the package.

6. **Run the Project**  
   - Start the database environment.
   - Run the project in debug mode (press `F5` in Visual Studio).
   - Test the API by calling the following endpoint:  
     ```plaintext
     GET api/Teacher/ListTeachers
     ```

---

## Features

### Core Functionalities

1. **Teacher Management**
   - Add, update, and delete teacher information through the API.
   - View a list of all teachers with details like name, department, and courses taught.

2. **Student Management**  
   *(Potential Integration)*  
   - CRUD operations for student data.
   - Include searching for students by name, ID, or other attributes.

3. **Course Management**
   - Maintain a catalog of courses with attributes like course code, name, and assigned teachers.
   - Search courses by name or department.

4. **Teacher Search**
   - Implement search functionality to find teachers by their name, using parameterized SQL queries to prevent SQL injection.

5. **Department Search**
   - *(From the exercises)* Add a feature to search for departments by name and display related information.

### Database Integration

1. **Parameterized SQL Queries**
   - Ensure secure database interactions by preventing SQL injection.
   - Optimize queries for fetching data on teachers, courses, and departments.

---

## Common Errors & Troubleshooting

- **Connection Refused Error**  
  Ensure your database is running and the connection string is accurate.

- **Authentication Failed Error**  
  Verify that the username and password in your connection string match your MySQL setup.

- **MAMP Issues**  
  If MAMP is incompatible, consider using [XAMPP](https://www.apachefriends.org/) or another MySQL environment.

---

## Exercises

Enhance your understanding by completing the following tasks:

1. **Exercise 5**  
   Implement a search feature to find teachers by name.

2. **Exercise 6**  
   Add a search feature to find departments by name.

---

## Acknowledgments

- Tools and dependencies: ASP.NET Core, MySQL, phpMyAdmin, Visual Studio, MySQL.Data NuGet Package.

---

For any further questions or feedback, feel free to reach out!
