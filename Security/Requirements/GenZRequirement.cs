using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorWeb.Security.Requirements
{
    public class GenZRequirement : IAuthorizationRequirement
    {
        public GenZRequirement(int _fromYead=1997,int  _toYear=2012)
        {
            FromYear = _fromYead;
            ToYear = _toYear;
        }
        
        public int FromYear { get; set; }
        public int ToYear { get; set; }
    }
}
