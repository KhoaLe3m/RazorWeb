using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RazorWeb.Models
{
    public class AppUser: IdentityUser
    {
        [Column(TypeName = "nvarchar")]
        [StringLength(400)]
        public string HomeAddress { set; get; }
    }
}
