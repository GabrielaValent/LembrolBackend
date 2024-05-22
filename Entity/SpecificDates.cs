using Microsoft.EntityFrameworkCore;

namespace backend_lembrol.Entity
{
    [PrimaryKey(nameof(TagId), nameof(SpecificDate))]
    public class SpecificDates
    {
        public string TagId {get;set;}
        public DateTime SpecificDate {get;set;}
        public int Active {get;set;}
    }
}