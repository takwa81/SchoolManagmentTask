using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Application.DTOs.Course
{
    public class CreateCourseRequest
    {
        public string Name { get; set; } = string.Empty;
        public int TeacherId { get; set; }
    }

}
