using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorWeb.Helpers
{
    public class PagingModel
    {
        public int currentpage { set; get; }
        public int countpages { set; get; }
        public Func<int?,string> generateUrl { set; get; }
    }
}
