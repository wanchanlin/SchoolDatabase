namespace SchoolDatabase.Models
{
    // Represents a teacher entity in the school database

    public class Teacher
    {
        // Unique identifier for the teacher
        public int teacherid { get; set; }

        // First name of the teacher (nullable)
        public string? teacherfname { get; set; }

        // Last name of the teacher (nullable)
        public string? teacherlname { get; set; }

        // Employee number for the teacher (nullable)
        public string? employeenumber { get; set; }

        // Date the teacher was hired
        public DateTime hiredate { get; set; }

        // Teacher's salary (decimal type for financial precision)
        public decimal salary { get; set; }
    }
}
