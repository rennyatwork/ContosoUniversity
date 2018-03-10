using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; }


        /*
         Foreign key and navigation properties
The foreign key properties and navigation properties reflect the following relationships:
An enrollment record is for a single course, so there's a CourseID foreign key property and a Course navigation property:
             */
        public int CourseID { get; set; }


        public int StudentID { get; set; }
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}