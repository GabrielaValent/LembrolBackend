using System.ComponentModel.DataAnnotations;

namespace backend_lembrol.Dto
{
    public class TagDto
    {
        [Required]
        public string TagId {get;set;} = string.Empty;
        public string Name {get;set;} = string.Empty;
        public List<int> DaysOfWeek { get; set; } = [];
        public List<DateTime> SpecificDates { get; set; } = [];
        public string Color { get; set; } = "#FFFFFF";
    }
}