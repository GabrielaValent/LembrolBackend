using Microsoft.EntityFrameworkCore;

namespace backend_lembrol.Entity
{
[PrimaryKey(nameof(TagId), nameof(DayOfWeek))]
    public class DaysOfWeek
    {

        public string TagId {get;set;}
        public int DayOfWeek {get;set;}
        public int Active {get;set;}
 
    }
}