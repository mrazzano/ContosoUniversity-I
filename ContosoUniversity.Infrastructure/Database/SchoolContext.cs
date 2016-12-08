using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Infrastructure.Database
{
    public class SchoolContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException("modelBuilder");

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Instructors)
                .WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("CourseID")
                .MapRightKey("InstructorID")
                .ToTable("CourseInstructor"));

            // Configure the primary key for the OfficeAssignment 
            modelBuilder.Entity<OfficeAssignment>()
                .HasKey(t => t.InstructorId);

            // Map one-to-zero or one relationship 
            modelBuilder.Entity<OfficeAssignment>()
                .HasRequired(t => t.Instructor)
                .WithOptional(t => t.OfficeAssignment)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Department>().MapToStoredProcedures();
        }
    }
}