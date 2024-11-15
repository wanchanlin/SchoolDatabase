namespace SchoolDatabase.Models
{
    public class Student
    {
      
            // Unique identifier for the teacher
            public int studentid { get; set; }

            // First name of the teacher (nullable)
            public string? studentfname { get; set; }

              // Last name of the teacher (nullable)
            public string? studentlname { get; set; }

             public string? studentnumber { get; set; }
             // Employee number for the teacher (nullable)
            public DateTime enroldate { get; set; }

          
         
              
       

    }
}
