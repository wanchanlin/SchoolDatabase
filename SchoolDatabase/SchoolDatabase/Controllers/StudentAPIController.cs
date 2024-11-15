using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolDatabase.Models;
using System;
using MySql.Data.MySqlClient;

namespace SchoolDatabase.Controllers
{
    // API controller to manage student data in the school database
    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        // Dependency injection of the database context
        private readonly SchoolDbContext _context;

        public StudentAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route(template: "ListStudents")]
        public List<Student> ListStudents()
        {
            // Initialize an empty list to hold student data
            List<Student> Students = new List<Student>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                // SQL query to select all students
                MySqlCommand Command = Connection.CreateCommand();
                string query = "select * from students";


                //SQL QUERY
                Command.CommandText = query;
                Command.Prepare();

                // Execute the query and read each student record
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Map each database column to the Student object's properties
                    while (ResultSet.Read())
                    {
                        int Id = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrollmentDate = Convert.ToDateTime(ResultSet["enroldate"]);
                   

                        // Create and populate a Student object with the data
                        Student CurrentStudent = new Student()
                        {
                            studentid = Id,
                            studentfname = FirstName,
                            studentlname = LastName,
                            studentnumber = StudentNumber,
                            enroldate = EnrollmentDate
                          
                        };
                        // Add the student to the list
                        Students.Add(CurrentStudent);
                    }
                }
            }
            // Return the full list of students
            return Students;
        }

        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            // Initialize an empty Student object to hold the result
            Student SelectedStudent = new Student();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT * FROM students where studentid = @id";
                Command.Parameters.AddWithValue("@id", id);

                // Open the database connection to retrieve the student by ID
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int Id = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrollmentDate = Convert.ToDateTime(ResultSet["enrollmentdate"]);
               

                        // Assign data to the SelectedStudent object
                        SelectedStudent.studentid = Id;
                        SelectedStudent.studentfname = FirstName;
                        SelectedStudent.studentlname = LastName;
                        SelectedStudent.studentnumber = StudentNumber;
                        SelectedStudent.enroldate = EnrollmentDate;
                    
                    }
                }
            }
            // Return the found student or an empty object if not found
            return SelectedStudent;
        }
    }
}

