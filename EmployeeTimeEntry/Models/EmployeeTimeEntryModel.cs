using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeTimeEntry.Models
{
    public class EmployeeTimeEntryModel
    {
        public int EntryID { get; set; }

        [DisplayName("Select Employee Name")]
        [Required(ErrorMessage = "Please Select Employee Name.")]
        public string EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DisplayName("Select Date of Time Entry")]
        [Required(ErrorMessage = "Please provide a date.")]
        public DateTime Date { get; set; }

        [DisplayName("Input Time Started")]
        [Required(ErrorMessage = "Please provide Time Started.")]
        public DateTime InTime { get; set; }


        // [Required]
        // [DataType(DataType.Time)]
        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [DisplayName("Input Time Ended")]
        [Required(ErrorMessage = "Please provide Time Ended.")]
        public DateTime OutTime { get; set; }

        public string SelectedNameId { get; set; }

        public string SelectedFilter { get; set; }


        public string? NameSelect { get; set; }
        public List<SelectListItem> NamesList { get; set; }

        [DisplayName("Filter By:")]
        public List<SelectListItem> FilterList { get; set; }
    }
}
