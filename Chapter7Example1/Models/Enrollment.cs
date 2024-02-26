using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Chapter7Example1.Models
{
    public enum LetterGrade
    {
        A,B,C,D,F,I,W,P
    }
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int StudentId{ get; set; }
        public int CourseId { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
        [DisplayFormat(NullDisplayText ="No grade")]
       public LetterGrade? LetterGrade { get; set; }


    }
}
