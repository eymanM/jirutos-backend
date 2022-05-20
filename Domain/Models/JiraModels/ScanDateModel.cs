namespace Domain.Models.JiraModels;

public class ScanDateModel
{
    public DateTime DateFromDT { get; set; }
    public DateTime DateToDT { get; set; }

    private string _dateFrom;

    public string DateFrom

    {
        get { return _dateFrom; }
        set
        {
            _dateFrom = value;
            DateFromDT = DateTime.Parse(value);
        }
    }

    private string _dateTo;

    public string DateTo

    {
        get { return _dateTo; }
        set
        {
            _dateTo = value;
            DateToDT = DateTime.Parse(value);
        }
    }
}