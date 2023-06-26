using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorWeb.Models;

namespace RazorWeb.Areas.Admin.Pages.User
{
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AddRoleModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }
        public AppUser user { set; get; }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        [Display(Name = "Các role đã gán cho user")]
        public string[] RoleName { get; set; }

        public SelectList allRoles { get; set; }

        

        
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không thấy user có id : {id}");
            }
            RoleName =  (await _userManager.GetRolesAsync(user)).ToArray<string>();
            ViewData["Title"] = "Truyền Role";
            List<string> roleName = await _roleManager.Roles.Select(r=>r.Name).ToListAsync();
            allRoles = new SelectList(roleName);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Không thấy user có id : {id}");
            }
            
            var OldRoleNames = (await _userManager.GetRolesAsync(user)).ToArray();
            var deleteRoles = OldRoleNames.Where(r => !RoleName.Contains(r));
            var addRoles = RoleName.Where(r => !OldRoleNames.Contains(r));

            List<string> roleName = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            allRoles = new SelectList(roleName);

            var resultDelete = await _userManager.RemoveFromRolesAsync(user, deleteRoles);
            if (!resultDelete.Succeeded)
            {
                resultDelete.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                return Page();
            }

            var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);
            if (!resultAdd.Succeeded)
            {
                resultAdd.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                return Page();
            }
            StatusMessage = $"Thiết lập role thành công cho user:{user.UserName}";
            return RedirectToPage("./Index");
        }
    }
}
