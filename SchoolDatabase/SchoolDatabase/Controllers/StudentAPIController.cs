using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolDatabase.Models;
using System;
using MySql.Data.MySqlClient;

namespace SchoolDatabase.Controllers
{
    /// <summary>
    /// API controller to manage student data in the school database.
    /// Provides endpoints for retrieving, adding, and deleting student records.
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
        /// <summary>
        /// Retrieves details of a specific student by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the student to retrieve.</param>
        /// <returns>A <see cref="Student"/> object representing the student, or null if not found.</returns>
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

        /// <summary>
        /// Adds a new student record to the database.
        /// </summary>
        /// <param name="StudentData">A <see cref="Student"/> object containing the details of the new student.</param>
        /// <example>
        /// POST: api/Student/AddStudent
        /// Body:
        /// {
        ///     "studentfname": "John",
        ///     "studentlname": "Doe",
        ///     "studentnumber": "S12345",
        ///     "enroldate": "2024-01-01"
        /// }
        /// </example>
        /// <returns>The ID of the newly added student, or 0 if the operation fails.</returns>
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

        /// <summary>
        /// Deletes a student record from the database by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the student to delete.</param>
        /// <example>
        /// DELETE: api/Student/DeleteConfirm/{id}
        /// </example>
        /// <returns>The number of rows affected by the delete operation.</returns>
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
        [HttpPut(template: "UpdatedStudent/{StudentId}")]
        public Student UpdatedStudent(int StudentId, [FromBody] Student StudentData)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "update students set studentfname=@studentfname, studentlname=@studentlname, studentnumber=@studentnumber, enroldate=@enroldate where studentid=@id";
                Command.Parameters.AddWithValue("@studentfname", StudentData.studentfname);
                Command.Parameters.AddWithValue("@studentlname", StudentData.studentlname);
                Command.Parameters.AddWithValue("@studentnumber", StudentData.studentnumber);
                Command.Parameters.AddWithValue("@enroldate", StudentData.enroldate);
                Command.Parameters.AddWithValue("@id", StudentId);

                Command.ExecuteNonQuery();
            }

            return FindStudent(StudentId);
        }
    }
}
