namespace EmployeeTimeEntry.Models
{
    public class TimeEntries
    {
        public int EntryID { get; set; }

        public string EmployeeID { get; set; }

        public string Date { get; set; }

        public string InTime { get; set; }

        public string OutTime { get; set; }
    }
}
