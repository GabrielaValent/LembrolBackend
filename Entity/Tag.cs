using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace backend_lembrol.Entity
{
    [PrimaryKey(nameof(TagId))]
    public class Tag
    {
        [Required]
        public string TagId {get;set;}  = string.Empty;
        public string Name {get;set;} = string.Empty;
        public string Color { get; set; } = "#FFFFFFF";
        public int Active {get;set;}
    }
}