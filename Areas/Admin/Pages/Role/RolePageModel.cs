using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorWeb.Areas.Admin.Pages.Role
{
    public class RolePageModel:PageModel
    {
        protected readonly MyBlogContext _context;
        protected readonly RoleManager<IdentityRole> _roleManager;
        [TempData]
        public string StatusMessage { get; set; }
        public RolePageModel(RoleManager<IdentityRole> roleManager,MyBlogContext myBlogContext)
        {
            this._roleManager = roleManager;
            _context = myBlogContext;
        }
    }
}
