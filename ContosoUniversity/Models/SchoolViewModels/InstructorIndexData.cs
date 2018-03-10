using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
 * Create a view model for the Instructor Index view

The Instructors page shows data from three different tables. Therefore, you'll create a view model that includes 
three properties, each holding the data for one of the tables.

    In the SchoolViewModels folder, create InstructorIndexData.cs and replace the existing code with the following code:
 * **/
namespace ContosoUniversity.Models.SchoolViewModels
{
    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}