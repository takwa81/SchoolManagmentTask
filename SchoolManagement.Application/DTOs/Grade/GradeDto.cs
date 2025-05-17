using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs.Grade
{
    public class GradeDto
    {
        public int AssignmentId { get; set; }
        public string AssignmentTitle { get; set; }
        public float Score { get; set; }
        public string? Feedback { get; set; }

    }

}
