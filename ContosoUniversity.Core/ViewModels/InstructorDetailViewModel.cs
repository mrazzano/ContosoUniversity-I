using System.Collections.Generic;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.ViewModels
{
    public class InstructorDetailViewModel
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}

