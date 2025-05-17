using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs.Grade
{
    public class SubmitGradeRequest
    {
        [Required(ErrorMessage = "Assignment ID is required.")]
        public int AssignmentId { get; set; }

        [Required(ErrorMessage = "Student ID is required.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Score is required.")]
        [Range(0, 100, ErrorMessage = "Score must be between 0 and 100.")]
        public float Score { get; set; }

        [StringLength(500, ErrorMessage = "Feedback cannot exceed 500 characters.")]
        public string? Feedback { get; set; }
    }

}
