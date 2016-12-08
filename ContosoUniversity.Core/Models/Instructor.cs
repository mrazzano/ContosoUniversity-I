using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Core.Models
{
    public class Instructor : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }


        [Display(Name = "Office Location")]
        public virtual OfficeAssignment OfficeAssignment { get; set; }
        public virtual ICollection<Course> Courses { get; private set; }

        public Instructor()
        {
            Courses = new Collection<Course>();
        }
    }
}