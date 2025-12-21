using CsvHelper;
using CsvHelper.Configuration;
using EmployeeTimeEntry.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EmployeeTimeEntry.Controllers
{
    public class TimeEntryController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private int THOUSAND_ONE = 1001;

        public TimeEntryController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult TimeEntry(EmployeeTimeEntryModel employeeTimeEntryModel)
        {
            string csvFolderPathEmployees = Path.Combine(_hostingEnvironment.ContentRootPath, "EmployeeData", "Employees.csv");
            string csvFolderPathTimeEntries = Path.Combine(_hostingEnvironment.ContentRootPath, "EmployeeData", "TimeEntries.csv");

            if (ModelState.IsValid)
            {

            }

            // if new employee entry, then add it to TimeEntries.csv
            if (employeeTimeEntryModel.EmployeeID != null)  // EVENTUALLY CHANGE TO MODEL.ISVALID
            {
                bool entryExists = checkTimeEntry(csvFolderPathTimeEntries, employeeTimeEntryModel); // check if time entry exists for that employee and date, if so, don't add them

                if (!entryExists)
                {
                    try
                    {
                        // if (!File.Exists(csvFolderPathTimeEntries))
                        // Append to the file.
                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            HasHeaderRecord = false
                        };

                        using var streamWriterEmployees = new StreamWriter(csvFolderPathTimeEntries, true);

                        List<TimeEntries> timeEntries = new List<TimeEntries>();
                        TimeEntries tm = new TimeEntries();
                        tm.EntryID = employeeTimeEntryModel.EntryID + THOUSAND_ONE; //Convert.ToInt32(employeeTimeEntryModel.EntryID);
                        tm.EmployeeID = employeeTimeEntryModel.EmployeeID;
                        tm.Date = employeeTimeEntryModel.Date.ToShortDateString(); // "yyyy-mm-dd";
                        tm.InTime = employeeTimeEntryModel.InTime.ToString("HH:mm");
                        tm.OutTime = employeeTimeEntryModel.OutTime.ToString("HH:mm");
                        timeEntries.Add(tm);
                        TimeSpan timeDifference = DateTime.Parse(tm.OutTime) - DateTime.Parse(tm.InTime);
                        // employeeTimeEntry.totalHours = Convert.ToInt16(timeDifference.TotalHours);
                        // if ()
                        Debug.WriteLine(timeDifference.TotalHours);
                        Debug.WriteLine(timeDifference.TotalMinutes);
                        Debug.WriteLine(timeDifference.TotalSeconds);

                        //   employeeTimeEntry.InTime = DateTime.Parse(timedEntry.InTime);
                        // employeeTimeEntry.OutTime = DateTime.Parse(timedEntry.OutTime);
                        // TimeSpan timeDifference = employeeTimeEntry.OutTime - employeeTimeEntry.InTime;
                        //employeeTimeEntry.totalHours = Convert.ToInt16(timeDifference.TotalHours);

                        if (timeDifference.TotalMinutes <= 0)
                        {
                            ViewBag.timeDifferenceOff = "Please Input an 'In Time' that takes place before 'Out Time' (" + DateTime.Parse(tm.OutTime).ToString("hh:mm tt") + ") ";
                        }

                        else
                        {
                            using (var csvWriteEmployee = new CsvWriter(streamWriterEmployees, config))
                            {
                                csvWriteEmployee.WriteRecords(timeEntries);
                            }
                            ;
                        }

                            foreach (var timeEntry in timeEntries)
                            {
                                Debug.WriteLine(timeEntry);
                            }

                        
                    }

                    catch (Exception ex)
                    {
                        // Debug.WriteLine($"An unexpected error occurred while writing to file path: " + csvFolderPathTimeEntries + "  : {ex.Message}");
                        Debug.WriteLine($"An unexpected error occurred while writing to file: {ex.Message}");
                    }
                }
            }

            // StreamReader start
            using var streamReaderEmployees = new StreamReader(csvFolderPathEmployees);
            using var streamReaderTimeEntries = new StreamReader(csvFolderPathTimeEntries);

            using var csvReaderEmployees = new CsvReader(streamReaderEmployees, CultureInfo.InvariantCulture);
            using var csvReaderTimeEntries = new CsvReader(streamReaderTimeEntries, CultureInfo.InvariantCulture);

            var recordEmployees = csvReaderEmployees.GetRecords<Employees>();
            var recordTimeEntries = csvReaderTimeEntries.GetRecords<TimeEntries>();

            List<Employees> employeesList = new List<Employees>();  // contains Employees.csv

            foreach (var employee in recordEmployees)  // add each employee id, first name, and last name to an object, then add to employeesList
            {
                Employees employees = new Employees();
                employees.EmployeeID = employee.EmployeeID;
                employees.FirstName = employee.FirstName;
                employees.LastName = employee.LastName;

                employeesList.Add(employees);
            }

            var viewModel = new EmployeeTimeEntryModel();
            viewModel.FilterList = new List<SelectListItem>();
            viewModel.NamesList = new List<SelectListItem>();

            viewModel.FilterList.Add(new SelectListItem { Value = "Name", Text = "Name" });
            viewModel.FilterList.Add(new SelectListItem { Value = "Date", Text = "Date" });

            foreach (var employee in employeesList)
            {
                viewModel.NamesList.Add(new SelectListItem { Text = employee.FirstName + " " + employee.LastName, Value = employee.EmployeeID });
            }

            List<EmployeeTimeEntryModel> employeeTimeEntryList = new List<EmployeeTimeEntryModel>();  // contains columns from Employees.csv and TimeEntries.csv

            foreach (var timedEntry in recordTimeEntries)  // match EmployeeID with employees first and last name and add columns to display in employeeTimeEntryList
            {
                string firstName = employeesList.Where(e => e.EmployeeID == timedEntry.EmployeeID).First().FirstName;
                string lastName = employeesList.Where(e => e.EmployeeID == timedEntry.EmployeeID).First().LastName;

               // Debug.WriteLine(timedEntry.Date);
                EmployeeTimeEntryModel employeeTimeEntry = new EmployeeTimeEntryModel();
                employeeTimeEntry.EntryID = timedEntry.EntryID;
                employeeTimeEntry.FirstName = firstName;
                employeeTimeEntry.LastName = lastName;
                employeeTimeEntry.Date = DateTime.Parse(timedEntry.Date);
                employeeTimeEntry.InTime = DateTime.Parse(timedEntry.InTime);
                employeeTimeEntry.OutTime = DateTime.Parse(timedEntry.OutTime);
                TimeSpan timeDifference = employeeTimeEntry.OutTime - employeeTimeEntry.InTime;
                employeeTimeEntry.totalHours = Convert.ToInt16(timeDifference.TotalHours);
                
                   
               // employeeTimeEntry.totalHours = 

                employeeTimeEntryList.Add(employeeTimeEntry);
            }

            var sortedNames = employeeTimeEntryList.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();
            var sortedDates = employeeTimeEntryList.OrderBy(e => e.Date).ToList();

            if (employeeTimeEntryModel.SelectedFilter == "Name")
            {
                ViewData["employeeList"] = sortedNames;
            }

            else if (employeeTimeEntryModel.SelectedFilter == "Date")
            {
                ViewData["employeeList"] = sortedDates;
            }

            else
            {
                ViewData["employeeList"] = employeeTimeEntryList;
            }

            viewModel.EntryID = employeeTimeEntryList.Count;
            return View(viewModel);
        }

        public bool checkTimeEntry(string filePath, EmployeeTimeEntryModel employeeTimeEntryModel)
        {
            using var streamReaderTimeEntries = new StreamReader(filePath);

            using var csvReaderTimeEntries = new CsvReader(streamReaderTimeEntries, CultureInfo.InvariantCulture);

            var recordTimeEntries = csvReaderTimeEntries.GetRecords<TimeEntries>();

            List<TimeEntries> timeEntryCheckList = new List<TimeEntries>();  // contains TimeEntries.csv
            DateTime selectedDate = DateTime.Parse(employeeTimeEntryModel.Date.ToShortDateString());

            foreach (var employee in recordTimeEntries)  // add each employee id, first name, and last name to an object, then add to employeesList
            {
                Debug.WriteLine(employee.EmployeeID + "   " + employeeTimeEntryModel.EmployeeID  +  "  " + DateTime.Parse(employee.Date) + "   " + selectedDate);
                if (employee.EmployeeID == employeeTimeEntryModel.EmployeeID && DateTime.Parse(employee.Date) == selectedDate)
                {
                    ViewBag.TimeExistsMesssage = "The selected employee already has a Time entry for " + 
                        selectedDate.ToShortDateString();
                    return true;
                }
            }

            return false;
        }

        public ActionResult Modalview()
        {
            return PartialView();
        }
    }
}
