namespace backend_lembrol.Dto
{
public class CompleteTagDto
{
    public string TagId { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public int Active { get; set; }
    public List<DaysOfWeekDto> DaysOfWeek { get; set; }
    public List<SpecificDatesDto> SpecificDates { get; set; }
}

public class DaysOfWeekDto
{
    public string TagId { get; set; }
    public int Day { get; set; }
    public int Active { get; set; }
}

public class SpecificDatesDto
{
    public string TagId { get; set; }
    public DateTime Date { get; set; }
    public int Active { get; set; }
}
}