using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Person> Persons { get; set; }

        /**
         * https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/complex-data-model
         * 
         Some developers prefer to use the fluent API exclusively so that they can keep their entity classes "clean." 
            You can mix attributes and fluent API if you want, and there are a few customizations that can only be done by 
            using fluent API, but in general the recommended practice is to choose one of these two approaches and use that 
            consistently as much as possible. 
            If you do use both, note that wherever there is a conflict, Fluent API overrides attributes.
             */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");


            modelBuilder.Entity<Department>().ToTable("Department")
                .Property(p => p.RowVersion).IsConcurrencyToken(); ;
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");

            modelBuilder.Entity<Person>().ToTable("Person");

            modelBuilder.Entity<CourseAssignment>()
                .HasKey(c=> new { c.CourseID, c.InstructorID});

            /**
             Composite key

            Since the foreign keys are not nullable and together uniquely identify each row of the table, 
            there is no need for a  separate primary key. 
            The InstructorID and CourseID properties should function as a composite primary key. 
            The only way to identify composite primary keys to EF is by using the fluent API 
            (it can't be done by using attributes). 
            
             */
            modelBuilder.Entity<CourseAssignment>()
                .HasKey(c => new { c.CourseID, c.InstructorID });
        }
    }
}