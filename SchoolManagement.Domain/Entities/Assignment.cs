using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }

        public Course Course { get; set; }
        public ICollection<Grade> Grades { get; set; }
    }
}
