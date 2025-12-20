using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace EmployeeTimeEntry.Models
{
    public class EmployeeTimeEntryModel
    {
        public int EntryID { get; set; }

        public string EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Date { get; set; }

        public DateTime InTime { get; set; }

        public DateTime OutTime { get; set; }

        public string SelectedNameId { get; set; }

        public string SelectedFilter { get; set; }

        [DisplayName("Select Your Name")]
        public List<SelectListItem> NamesList { get; set; }

        [DisplayName("Filter By:")]
        public List<SelectListItem> FilterList { get; set; }
    }
}
