using Chapter7Example1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Chapter7Example1.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext db;
        public StudentController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddProfile()
        {
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Student student = new Student();
            if (db.Instructors.Any(i => i.UserId == currentUserId))
            {
                student = db.Students.FirstOrDefault(i => i.UserId == currentUserId);
            }
            else
            {
                student.UserId = currentUserId;
            }
            return View(student);
        }


        [HttpPost]
        public async Task<IActionResult> AddProfile(Student student)

        {
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (db.Students.Any(i => i.UserId == currentUserId))

            {
                var studentToUpdate = db.Students.FirstOrDefault(i => i.UserId == currentUserId);

                studentToUpdate.StudentName = student.StudentName;
                db.Update(studentToUpdate);
            }
            else
            {
                db.Add(student);
            }

            await db.SaveChangesAsync();
            return View("Index");
        }




    }
}
