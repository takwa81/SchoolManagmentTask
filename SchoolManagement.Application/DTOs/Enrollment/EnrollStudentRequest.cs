using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs.Enrollment
{
    public class EnrollStudentRequest
    {
        [Required(ErrorMessage = "Student ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Student ID must be a positive integer.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Course ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Course ID must be a positive integer.")]
        public int CourseId { get; set; }
    }
}
