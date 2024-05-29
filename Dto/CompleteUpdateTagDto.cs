namespace backend_lembrol.Dto
{
public class CompleteUpdateTagDto
{
    public string Name { get; set; }
    public string Color { get; set; } = "#FFFFFF";
    public int Active { get; set; }
    public List<DaysOfWeekDto> DaysOfWeek { get; set; }
    public List<SpecificDatesDto> SpecificDates { get; set; }
}

public class DaysOfWeekDto
{
    public int Day { get; set; }
    public int Active { get; set; }
}

public class SpecificDatesDto
{
    public DateTime Date { get; set; }
    public int Active { get; set; }
}
}