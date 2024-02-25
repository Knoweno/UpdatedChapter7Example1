using Chapter7Example1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Chapter7Example1.ViewModels
{
    public class RoleAddUserRoleViewModel
    {
        public ApplicationUser User { get; set; }
        public string Role {  get; set; }
        public SelectList RoleList { get; set; }

    }
}
