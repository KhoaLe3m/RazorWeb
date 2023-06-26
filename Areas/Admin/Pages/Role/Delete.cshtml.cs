using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWeb.Models;

namespace RazorWeb.Areas.Admin.Pages.Role
{
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }

        public IdentityRole role { set; get; }

        public async Task<IActionResult> OnGetAsync(string roleid)
        {
            if (roleid == null)
                return NotFound("Không tìm thấy roleid");
            role = await _roleManager.FindByIdAsync(roleid);
            if (role != null)
            {
                return Page();
            }
            return NotFound("Không tìm thấy roleid");
        }
        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if (roleid == null)
                return NotFound("Không tìm thấy roleid");
            role = await _roleManager.FindByIdAsync(roleid);
            if (role == null) return NotFound("Không tìm thấy roleid");
            if (!ModelState.IsValid) return Page();
           
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn đổi xóa role: {role.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
            }
            return Page();

        }
    }
}
