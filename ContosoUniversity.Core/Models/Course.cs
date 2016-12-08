using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Core.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int CourseId { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; private set; }
        public virtual ICollection<Instructor> Instructors { get; private set; }

        public Course()
        {
            Enrollments = new Collection<Enrollment>();
            Instructors = new Collection<Instructor>();
        }
    }
}