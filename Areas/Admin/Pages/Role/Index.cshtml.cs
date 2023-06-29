using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorWeb.Models;

namespace RazorWeb.Areas.Admin.Pages.Role
{
    [Authorize]

    public class IndexModel : RolePageModel
    {
        public IndexModel(RoleManager<IdentityRole> roleManager, MyBlogContext myBlogContext) : base(roleManager, myBlogContext)
        {
        }
        public class RoleModel : IdentityRole
        {
            public string[] Claims { get;  set; }
        }
        public List<RoleModel> roles { get; set; }
        public async Task OnGet()
        {
            
            var r = await _roleManager.Roles.ToListAsync();
            roles = new List<RoleModel>();
            foreach(var _r in r)
            {
                var claims = await _roleManager.GetClaimsAsync(_r);
                var calimsString = claims.Select(c => c.Type + "=" + c.Value);
                var rm = new RoleModel()
                {
                    Name = _r.Name,
                    Id = _r.Id,
                    Claims = calimsString.ToArray()
                };
                roles.Add(rm);
            }
        }
        public void OnPost() => RedirectToPage();
    }
}
