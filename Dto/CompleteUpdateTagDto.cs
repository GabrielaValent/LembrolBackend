namespace backend_lembrol.Dto
{
public class CompleteUpdateTagDto
{
    public string Name { get; set; }
    public string Color { get; set; } = "#FFFFFF";
    public int Active { get; set; }
    public double Lat {  get; set; }
    public double Lng { get; set; }
    public List<DaysOfWeekUpdateDto> DaysOfWeek { get; set; }
    public List<SpecificDatesUpdateDto> SpecificDates { get; set; }
}

public class DaysOfWeekUpdateDto
{
    public int Day { get; set; }
    public int Active { get; set; }
}

public class SpecificDatesUpdateDto
{
    public DateTime Date { get; set; }
    public int Active { get; set; }
}
}