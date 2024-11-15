using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolDatabase.Models;
using System;
using MySql.Data.MySqlClient;


namespace SchoolDatabase.Controllers
{   
    // API controller to manage teacher data in the school database
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        // Dependency injection of the database context
        private readonly SchoolDbContext _context;
        
        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(template: "ListTeachers")]
        public List<Teacher> ListTeachers(string SearchKey=null)
        {
            // Initialize an empty list to hold teacher data
            List<Teacher> Teachers = new List<Teacher>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                // SQL query to select all teachers
                MySqlCommand Command = Connection.CreateCommand();
                string query = "select * from teachers";

                if (SearchKey != null)
                {
                    query += " where hiredate like @key ";
                    Command.Parameters.AddWithValue("@key", $"%{SearchKey}%");
                }

                //SQL QUERY
                Command.CommandText = query;
                Command.Prepare();

                // Execute the query and read each teacher record
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Map each database column to the Teacher object's properties
                    while (ResultSet.Read())
                    {
                        // Map each database column to the Teacher object's properties
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);


                        // Create and populate a Teacher object with the data
                        Teacher CurrentTeacher = new Teacher()
                        {
                            teacherid = Id,
                            teacherfname = FirstName,
                            teacherlname = LastName,
                            employeenumber = EmployeeNumber,
                            hiredate = HireDate,
                            salary = Salary
                        };
                        // Add the teacher to the list
                        Teachers.Add(CurrentTeacher);

                    }
                }
            }
            // Return the full list of teachers
            return Teachers;
        }
     
         /// <summary>
         /// 
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            // Initialize an empty Teacher object to hold the result
            Teacher SelectedTeacher = new Teacher();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                // Initialize an empty Teacher object to hold the result
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT * FROM teachers where teacherid = " + id;
                Command.Parameters.AddWithValue("@id", id);

                // Open the database connection to retrieve the teacher by ID
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    while (ResultSet.Read())
                    {
                        // Map database columns to the SelectedTeacher object's properties
                        int Id = Convert.ToInt32(ResultSet["teacherid"]);
                        string FirstName = ResultSet["teacherfname"].ToString();
                        string LastName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);


                        // Assign data to the SelectedTeacher object
                        SelectedTeacher.teacherid = Id;
                        SelectedTeacher.teacherfname = FirstName;
                        SelectedTeacher.teacherlname = LastName;
                        SelectedTeacher.employeenumber = EmployeeNumber;
                        SelectedTeacher.hiredate = HireDate;
                        SelectedTeacher.salary = Salary;
                    }
                }
            } // Return the found teacher or an empty object if not found
            return SelectedTeacher;
        }
    }
}