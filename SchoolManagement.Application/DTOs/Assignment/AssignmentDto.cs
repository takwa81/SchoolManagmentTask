using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs.Assignment
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
    }

}
