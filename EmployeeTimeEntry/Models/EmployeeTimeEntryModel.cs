using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeTimeEntry.Models
{
    public class EmployeeTimeEntryModel
    {
        public string EntryID { get; set; }

        public string EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Date { get; set; }

        public DateTime InTime { get; set; }

        public DateTime OutTime { get; set; }

       // public string SelectedNameId { get; set; }

       // public List<SelectListItem> NamesList { get; set; }
    }
}
