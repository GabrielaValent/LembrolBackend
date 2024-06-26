using Microsoft.EntityFrameworkCore;

namespace backend_lembrol.Entity
{
    [PrimaryKey(nameof(TagId))]
    public class Tag
    {
        public string TagId {get;set;}
        public string Name {get;set;}
        public string Color { get; set; } = "#FFFFFFF";
        public int Active {get;set;}
        public double? Lng {get;set;}
        public double? Lat {get;set;}
    }
}