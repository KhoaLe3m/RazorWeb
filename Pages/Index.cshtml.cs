using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RazorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorWeb.Pages
{
    
    public class IndexModel : PageModel
    {
        
        private readonly ILogger<IndexModel> _logger;
        private readonly MyBlogContext _myBlogContext;
        public IndexModel(ILogger<IndexModel> logger,MyBlogContext _myContext)
        {
            _myBlogContext = _myContext;
            _logger = logger;
        }

        public void OnGet()
        {
            var posts = (from p in _myBlogContext.articles
                        orderby p.Created descending
                        select p).ToList();
            ViewData["posts"] = posts;
        }
    }
}
