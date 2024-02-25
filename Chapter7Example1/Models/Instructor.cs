using System.Collections.Generic;

namespace Chapter7Example1.Models
{
    public class Instructor
    {
        public int InstructorId { get; set; }   
        public string InstructorName { get; set; }
        public string OfficeLocation { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
