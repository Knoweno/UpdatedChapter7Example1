using Chapter7Example1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Chapter7Example1.Controllers
{
    [Authorize(Roles ="Instructor")]
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext db;
        public InstructorController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ActionResult AddCourse()
        {
            Course course = new Course();
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            course.InstructorId = db.Instructors.SingleOrDefault(i => i.UserId == currentUserId).InstructorId;
            return View(course); 

        }
        [HttpPost]
        public async Task<IActionResult> AddCourse(Course course)
        {
            db.Add(course);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Instructor");
        }


        [HttpPost]
        public async Task<IActionResult> AddProfile(Instructor instructor)

        {
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (db.Instructors.Any(i => i.UserId == currentUserId))

            {
                var instructorToUpdate = db.Instructors.FirstOrDefault(i => i.UserId == currentUserId);

                instructorToUpdate.InstructorName =instructor.InstructorName;
                instructorToUpdate.OfficeLocation= instructor.OfficeLocation;
                db.Update(instructorToUpdate);
            }
            else
            {
                db.Add(instructor);
            }
            
            await db.SaveChangesAsync();
            return View("Index");
        }

        public IActionResult AddProfile()
        {
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Instructor instructor = new Instructor();
            if(db.Instructors.Any(i=>i.UserId ==currentUserId))
            {
                instructor =db.Instructors.FirstOrDefault(i=>i.UserId ==currentUserId);
            }
            else
            {
                instructor.UserId = currentUserId;
            }
            return View(instructor);
        }
        
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CourseByInstructor()
        {
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var instructorId = db.Instructors.SingleOrDefault(i => i.UserId == currentUserId).InstructorId;
            var course = await db.Courses.Where(i => i.InstructorId == instructorId).ToListAsync();
            return View(course);
        }
        public async Task<IActionResult> PostGrade(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var allStudents = await db.Enrollments.Include(c => c.Course).Where(c => c.CourseId == id).ToListAsync();

            if(allStudents==null || allStudents.Count==0)
            {
                return NotFound();
            }
            return View(allStudents);
        }
        [HttpPost]
        public ActionResult PostGrade(List<Enrollment> enrollments)
        {
            foreach (var enrollment in enrollments)
            {
                var er = db.Enrollments.Find(enrollment.EnrollmentId);
                er.LetterGrade = enrollment.LetterGrade;
            }

            db.SaveChanges();
            return RedirectToAction("CourseByInstructor");
        }


    }
}
