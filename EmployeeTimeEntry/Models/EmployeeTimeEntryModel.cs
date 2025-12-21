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

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please enter a proper date.")]
        public DateTime Date { get; set; }

        [DisplayName("In Time:")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Please enter an In Time.")]
        public DateTime InTime { get; set; }

        [DisplayName("Out Time:")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Please enter an Out Time.")]
        public DateTime OutTime { get; set; }

        public int totalHours { get; set; }

        public string? SelectedNameId { get; set; }

        public string? SelectedFilter { get; set; }


        public string? NameSelect { get; set; }
        public List<SelectListItem>? NamesList { get; set; }

        [DisplayName("Filter By:")]
        public List<SelectListItem>? FilterList { get; set; }
    }
}
