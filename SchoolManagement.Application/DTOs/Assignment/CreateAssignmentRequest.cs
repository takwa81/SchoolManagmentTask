using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs.Assignment
{
    public class CreateAssignmentRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Course ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Course ID must be a positive integer.")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Due date must be a valid date.")]
        public DateTime DueDate { get; set; }

    }

}
