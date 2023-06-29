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

namespace RazorWeb.Areas.Admin.Pages.User
{
    public class EditUserRoleClaimModel : PageModel
    {
        private readonly MyBlogContext _context;
        private readonly UserManager<AppUser> _userManager;
        public EditUserRoleClaimModel(MyBlogContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [TempData]
        public string StatusMessage { get; set; }
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
        public AppUser user { set; get; }
        public IdentityUserClaim<string> userclaim { set; get; }
        public void OnGet() => NotFound("Không được truy cập");
        public async Task<IActionResult> OnGetAddClaimAsync(string userid)
        {
            user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound("Không tìm thấy user");
            return Page();
        }
        public async Task<IActionResult> OnPostAddClaimAsync(string userid)
        {
            user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound("Không tìm thấy user");
            if (!ModelState.IsValid) return Page();
            var claims = _context.UserClaims.Where(c => c.UserId == user.Id);
            if(claims.Any(c => c.ClaimType == Input.ClaimType && c.ClaimValue == Input.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Đặc tính này đã có rồi");
                return Page();
            }
            await _userManager.AddClaimAsync(user, new Claim(Input.ClaimType, Input.ClaimValue));
            StatusMessage = "Đã thêm đặc tính cho user";
            return RedirectToPage("./AddRole", new { id = user.Id });
        }
        public async Task<IActionResult> OnGetEditClaimAsync(int? claimid)
        {
            
            if (claimid == null) return NotFound("Không tìm thấy claimid");
            userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
            user = await _userManager.FindByIdAsync(userclaim.UserId);
            if (user == null) return NotFound("Không tìm thấy user");
            Input = new InputModel()
            {
                ClaimType = userclaim.ClaimType,
                ClaimValue = userclaim.ClaimValue,
            };
            return Page();
        }
        public async Task<IActionResult> OnPostEditClaimAsync(int? claimid)
        {

            if (claimid == null) return NotFound("Không tìm thấy claimid");
            userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
            user = await _userManager.FindByIdAsync(userclaim.UserId);
            if (user == null) return NotFound("Không tìm thấy user");
            if(!ModelState.IsValid) return Page();
            if(_context.UserClaims.Any(c => c.UserId == user.Id
                                    && c.ClaimType == Input.ClaimType
                                    && c.ClaimValue == Input.ClaimValue
                                    && c.Id != userclaim.Id))
            {
                ModelState.AddModelError(string.Empty, "Đặc tính này đã có rồi");
                return Page();
            }    
            userclaim.ClaimType = Input.ClaimType;
            userclaim.ClaimValue = Input.ClaimValue;
            await _context.SaveChangesAsync();
            StatusMessage = "Cập nhật thành công claim";
            return RedirectToPage("./AddRole", new { id = user.Id });
        }
        public async Task<IActionResult> OnPostDeleteClaimAsync(int? claimid)
        {
            if (claimid == null) return NotFound("Không tìm thấy claimid");
            userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
            user = await _userManager.FindByIdAsync(userclaim.UserId);
            if (user == null) return NotFound("Không tìm thấy user");
            _context.UserClaims.Remove(userclaim);
            await _context.SaveChangesAsync();
            StatusMessage = "Xóa thành công Claim riêng của user!";
            return RedirectToPage("./AddRole", new { id = user.Id });
        }
    }
}
