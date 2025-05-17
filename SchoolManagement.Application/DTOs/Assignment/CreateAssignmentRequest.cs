using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs.Assignment
{
    public class CreateAssignmentRequest
    {
        public string Title { get; set; } = string.Empty;
        public int CourseId { get; set; }
    }

}
