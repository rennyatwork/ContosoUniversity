using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        /*
         A department may or may not have an administrator, and an administrator is always an instructor. 
         Therefore the InstructorID property is included as the foreign key to the Instructor entity, 
         and a question mark is added after the int type designation to mark the property as nullable.
             */
        public int? InstructorID { get; set; }


        /*
         The Timestamp attribute specifies that this column will be included in the 
         Where clause of Update and Delete commands sent to the database. 
         The attribute is called Timestamp because previous versions of SQL Server used a SQL timestamp data 
         type before the SQL rowversion replaced it. The .NET type for rowversion is a byte array.
If you prefer to use the fluent API, you can use the IsConcurrencyToken method (in Data/SchoolContext.cs) 
to specify the tracking property, as shown in the following example:
modelBuilder.Entity<Department>()
    .Property(p => p.RowVersion).IsConcurrencyToken();
          */
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Instructor Administrator { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}