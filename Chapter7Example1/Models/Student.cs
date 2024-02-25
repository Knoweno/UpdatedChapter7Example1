using System.Collections.Generic;

namespace Chapter7Example1.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
