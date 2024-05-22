namespace backend_lembrol.Dto
{
    public class TagDto
    {
        public string TagId {get;set;}
        public string Name {get;set;}
        public List<int> DaysOfWeek {get;set;}
        public List<DateTime> SpecificDates {get;set;}
        public string Color {get;set;}
    }
}