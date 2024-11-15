namespace SchoolDatabase.Models
{
    public class Course
    {
        // Unique identifier for the course
        public int courseid { get; set; }

        // Code associated with the course (e.g., 'CS101')
        public string? coursecode { get; set; }

        // Identifier for the teacher assigned to the course
        public long teacherid { get; set; }

        // Date when the course starts
        public DateTime startdate { get; set; }

        // Date when the course finishes
        public DateTime finishdate { get; set; }

        // Name of the course (e.g., 'Introduction to Programming')
        public string? coursename { get; set; }
    }
}
