using Chapter7Example1.Models;
using Chapter7Example1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Chapter7Example1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {

        private ApplicationDbContext db;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public RoleController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.db = db;
        }

        public IActionResult AllRole()
        {
            var roles=roleManager.Roles.ToList();
            return View(roles);
        }
       public IActionResult AddRole()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(IdentityRole role)
        {
            var result=await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("AllRole");
            }
            return View();
        }
        public async Task<IActionResult> AddUserRole(string id)
        {
            var roleDisplay = db.Roles.Select(x => new
            {
                Id = x.Id, Value=x.Name
            })
            .ToList();
            RoleAddUserRoleViewModel vm = new RoleAddUserRoleViewModel();
            var user=await userManager.FindByIdAsync(id);
            vm.User = user;
            vm.RoleList = new SelectList(roleDisplay, "Id", "Value");
            return View(vm);
        }
       [HttpPost]
        public async Task<IActionResult> AddUserRole(RoleAddUserRoleViewModel vm)
        {
            var user = await userManager.FindByIdAsync(vm.User.Id);
            var role = await roleManager.FindByIdAsync(vm.Role);
            var result = await userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("AllUser","Account");
            }
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);

            }
            var roleDisplay=db.Roles.Select(x=> new
            {

                Id=x.Id, Value=x.Name
            }).ToList();    
            vm.User=user;
            vm.RoleList = new SelectList(roleDisplay, "Id", "Value");
            return View(vm);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
