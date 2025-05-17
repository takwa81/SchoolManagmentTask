using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public float Score { get; set; }

        public string? Feedback { get; set; }
        public Assignment Assignment { get; set; }
        public User Student { get; set; }
    }
}
