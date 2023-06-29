using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWeb.Models;

namespace RazorWeb.Areas.Admin.Pages.Role
{
    public class EditRoleClaimModel : RolePageModel
    {
        public EditRoleClaimModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }

        public class InputModel
        {
            [Display(Name = "Tên claim")]
            [Required(ErrorMessage = "Phải nhập {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự")]
            public string ClaimType { get; set; }
            [Display(Name = "Giá trị")]
            [Required(ErrorMessage = "Phải nhập {0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự")]
            public string ClaimValue { get; set; }
        }


        [BindProperty]
        public InputModel Input { set; get; }
        public IdentityRole role { set; get; }
        public IdentityRoleClaim<string> claim { set; get; }
        public async Task<IActionResult> OnGet(int? claimid)
        {
            if(claimid == null)
                return NotFound("Không tìm thấy  claim");
            claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
            if(claim == null)
                return NotFound("Không tìm thấy  claim");
            role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null) return NotFound("Không tìm thấy  role");
            Input = new InputModel()
            {
                ClaimType = claim.ClaimType,
                ClaimValue = claim.ClaimValue
            };

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? claimid)
        {
            if (claimid == null)
                return NotFound("Không tìm thấy  claim");
            claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
            if (claim == null)
                return NotFound("Không tìm thấy  claim");
            role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null) return NotFound("Không tìm thấy role");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if ((_context.RoleClaims.Any(c => c.ClaimType == Input.ClaimType && c.ClaimValue == Input.ClaimValue && c.RoleId == role.Id)))
            {
                ModelState.AddModelError(string.Empty, "Claim này đã có trong role");
                return Page();
            }

            claim.ClaimType = Input.ClaimType;
            claim.ClaimValue = Input.ClaimValue;

            await _context.SaveChangesAsync();

            StatusMessage = "Vừa thêm đặc tính (claim) mới thành công";
            return RedirectToPage("./Edit",new { roleid = role.Id });
        }
        public async Task<IActionResult> OnPostDeleteAsync(int? claimid)
        {
            if (claimid == null)
                return NotFound("Không tìm thấy  claim");
            claim = _context.RoleClaims.Where(c => c.Id == claimid).FirstOrDefault();
            if (claim == null)
                return NotFound("Không tìm thấy  claim");
            role = await _roleManager.FindByIdAsync(claim.RoleId);
            if (role == null) return NotFound("Không tìm thấy  role");
            var result = await _roleManager.RemoveClaimAsync(role, new Claim(claim.ClaimType,claim.ClaimValue));
            
            StatusMessage = "Xóa đặc tính (claim) thành công";

            return RedirectToPage("./Edit", new { roleid = role.Id });

        }
    }
}
