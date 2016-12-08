using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Core.ViewModels
{
    public class EnrollmentDateViewModel
    {
        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }

        public int StudentCount { get; set; }
    }
}