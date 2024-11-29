using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolDatabase.Models;
using System;
using MySql.Data.MySqlClient;

namespace SchoolDatabase.Controllers
{
    /// <summary>
    /// API controller to manage student data in the school database
    /// </summary>
    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public StudentAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        // List all students
        [HttpGet]
        [Route(template: "ListStudents")]
        public List<Student> ListStudents()
        {
            List<Student> Students = new List<Student>();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                string query = "SELECT * FROM students";
                Command.CommandText = query;
                Command.Prepare();

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        Students.Add(new Student
                        {
                            studentid = Convert.ToInt32(ResultSet["studentid"]),
                            studentfname = ResultSet["studentfname"].ToString(),
                            studentlname = ResultSet["studentlname"].ToString(),
                            studentnumber = ResultSet["studentnumber"].ToString(),
                            enroldate = Convert.ToDateTime(ResultSet["enroldate"])
                        });
                    }
                }
            }
            return Students;
        }

        // Find a specific student by ID
        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            Student SelectedStudent = new Student();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "SELECT * FROM students WHERE studentid = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    if (ResultSet.Read())
                    {
                        SelectedStudent.studentid = Convert.ToInt32(ResultSet["studentid"]);
                        SelectedStudent.studentfname = ResultSet["studentfname"].ToString();
                        SelectedStudent.studentlname = ResultSet["studentlname"].ToString();
                        SelectedStudent.studentnumber = ResultSet["studentnumber"].ToString();
                        SelectedStudent.enroldate = Convert.ToDateTime(ResultSet["enroldate"]);
                    }
                }
            }
            return SelectedStudent;
        }

        // Add a new student
        [HttpPost(template: "AddStudent")]
        public int AddStudent([FromBody] Student StudentData)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "INSERT INTO students (studentfname, studentlname, studentnumber, enroldate) VALUES (@fname, @lname, @number, @enrolDate)";
                Command.Parameters.AddWithValue("@fname", StudentData.studentfname);
                Command.Parameters.AddWithValue("@lname", StudentData.studentlname);
                Command.Parameters.AddWithValue("@number", StudentData.studentnumber);
                Command.Parameters.AddWithValue("@enrolDate", StudentData.enroldate);

                Command.ExecuteNonQuery();
                return Convert.ToInt32(Command.LastInsertedId);
            }
        }

        // Delete a student
        [HttpDelete(template: "DeleteConfirm/{id}")]
        public int DeleteConfirm(int id)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "DELETE FROM students WHERE studentid = @id";
                Command.Parameters.AddWithValue("@id", id);
                return Command.ExecuteNonQuery();
            }
        }
    }
}
