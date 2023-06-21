using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RazorWeb.Models
{
    /*[Table("")]*/
    public class Article
    {
        [Key]
        public int Id { set; get; }
        [StringLength(255)]
        [Required]
        [Column(TypeName = "nvarchar")]
        public string Title { set; get; }
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime Created { set; get; }
        [Column(TypeName = "ntext")]

        public string Content { set; get; }
    }
}
