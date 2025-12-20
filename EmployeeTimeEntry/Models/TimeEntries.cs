namespace EmployeeTimeEntry.Models
{
    public class TimeEntries
    {
        public string EntryID { get; set; }

        public string EmployeeID { get; set; }

        public string Date { get; set; }

        public DateTime InTime { get; set; }

        public DateTime OutTime { get; set; }
    }
}
