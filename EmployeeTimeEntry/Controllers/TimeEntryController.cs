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

        string csvFolderPathEmployees;     // folder path for Employees.csv
        string csvFolderPathTimeEntries;   // folder path for TimeEntries.csv     

        EmployeeTimeEntryModel viewModel = new EmployeeTimeEntryModel();

        List<EmployeeTimeEntryModel> employeeTimeEntryList = new List<EmployeeTimeEntryModel>();  // contains columns from Employees.csv and TimeEntries.csv

        public TimeEntryController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            csvFolderPathEmployees = Path.Combine(_hostingEnvironment.ContentRootPath, "App_Data", "Employees.csv");
            csvFolderPathTimeEntries = Path.Combine(_hostingEnvironment.ContentRootPath, "App_Data", "TimeEntries.csv");
        }

        public IActionResult Index()
        {
            ViewData["employeeList"] = readCsvFiles("default");
            viewModel.EntryID = employeeTimeEntryList.Count;

            return View(viewModel);
        }

        public ActionResult SubmitTimeEntry(EmployeeTimeEntryModel employeeTimeEntryModel)
        {
            bool newEntrySuccess = false;

            if (ModelState.IsValid)
            {
                bool entryExists = checkTimeEntry(csvFolderPathTimeEntries, employeeTimeEntryModel);

                if (!entryExists) // check if time entry exists for that employee and date, if it exists, don't add them
                {
                    try  // use StreamWriter and CsvWriter to add employee to TimeEntries.csv
                    {
                        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            HasHeaderRecord = false
                        };

                        using var streamWriterEmployees = new StreamWriter(csvFolderPathTimeEntries, true);

                        List<TimeEntries> timeEntries = new List<TimeEntries>();
                        TimeEntries tm = new TimeEntries();

                        tm.EntryID = employeeTimeEntryModel.EntryID + THOUSAND_ONE;
                        tm.EmployeeID = employeeTimeEntryModel.EmployeeID;
                        tm.Date = employeeTimeEntryModel.Date.ToShortDateString();
                        tm.InTime = employeeTimeEntryModel.InTime.ToString("HH:mm");
                        tm.OutTime = employeeTimeEntryModel.OutTime.ToString("HH:mm");
                        timeEntries.Add(tm);
                        TimeSpan timeDifference = DateTime.Parse(tm.OutTime) - DateTime.Parse(tm.InTime);

                        if (timeDifference.TotalMinutes <= 0)
                        {
                            ViewBag.timeDifferenceOff = "Please Input an 'In Time' that takes place before 'Out Time' (" + DateTime.Parse(tm.OutTime).ToString("hh:mm tt") + ") ";
                        }

                        else
                        {
                            using (var csvWriteEmployee = new CsvWriter(streamWriterEmployees, config))
                            {
                                csvWriteEmployee.WriteRecords(timeEntries);
                                newEntrySuccess = true;
                            }
                            ;
                        }
                    }

                    catch (Exception ex)
                    {
                        // Debug.WriteLine($"An unexpected error occurred while writing to file path: " + csvFolderPathTimeEntries + "  : {ex.Message}");
                        Debug.WriteLine($"An unexpected error occurred while writing to file: {ex.Message}");
                    }
                }
            }

            ViewData["employeeList"] = readCsvFiles("");

            if (newEntrySuccess)  // if a new employee was added successfully, reset Time Entry Form, keep the sort
            {
                ViewBag.entrySuccessNotification = "Time Entry Added";
                ModelState.Clear();
            }

            viewModel.EntryID = employeeTimeEntryList.Count;
            return View("Index", viewModel);
        }

        public List<EmployeeTimeEntryModel> readCsvFiles(string sortString)  // function to call for reading the csv files and displaying time entries on screen
        {
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

            viewModel.SortingList = new List<SelectListItem>();
            viewModel.NamesList = new List<SelectListItem>();

            viewModel.SortingList.Add(new SelectListItem { Value = "Employee Full Name", Text = "Employee Full Name" });
            viewModel.SortingList.Add(new SelectListItem { Value = "Date", Text = "Date" });

            foreach (var employee in employeesList)
            {
                viewModel.NamesList.Add(new SelectListItem { Text = employee.FirstName + " " + employee.LastName, Value = employee.EmployeeID });
            }

            foreach (var timedEntry in recordTimeEntries)  // match EmployeeID with employees first and last name and add columns to display in employeeTimeEntryList
            {
                string firstName = employeesList.Where(e => e.EmployeeID == timedEntry.EmployeeID).First().FirstName;
                string lastName = employeesList.Where(e => e.EmployeeID == timedEntry.EmployeeID).First().LastName;

                EmployeeTimeEntryModel employeeTimeEntry = new EmployeeTimeEntryModel();
                employeeTimeEntry.EntryID = timedEntry.EntryID;
                employeeTimeEntry.FirstName = firstName;
                employeeTimeEntry.LastName = lastName;
                employeeTimeEntry.Date = DateTime.Parse(timedEntry.Date);
                employeeTimeEntry.InTime = DateTime.Parse(timedEntry.InTime);
                employeeTimeEntry.OutTime = DateTime.Parse(timedEntry.OutTime);
                TimeSpan timeDifference = employeeTimeEntry.OutTime - employeeTimeEntry.InTime;
                employeeTimeEntry.TotalHours = Convert.ToInt16(timeDifference.TotalHours);

                employeeTimeEntryList.Add(employeeTimeEntry);
            }

            var sortedNames = employeeTimeEntryList.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();
            var sortedDates = employeeTimeEntryList.OrderBy(e => e.Date).ToList();

            if (sortString == "default")
            {
                return employeeTimeEntryList;
            }

            if (!string.IsNullOrEmpty(TempData["currentSort"] as string)) { sortString = TempData["currentSort"] as string; }
            else {  TempData["currentSort"] = sortString; }
            TempData.Keep("currentSort");

            if (sortString == "Employee Full Name")
            {
                return sortedNames;
            }

            else if (sortString == "Date")
            {
                return sortedDates;
            }

            else
            {
                return employeeTimeEntryList;
            }
        }

        public ActionResult SortList(EmployeeTimeEntryModel employeeTimeEntryModel)
        {
            string sortString = employeeTimeEntryModel.SelectedSorting;
            TempData["currentSort"] = sortString;
            ViewData["employeeList"] = readCsvFiles(sortString);
            viewModel.EntryID = employeeTimeEntryList.Count;

            ModelState.Remove("EmployeeID");
            TempData.Keep("currentSort");
            return View("Index", viewModel);
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
                //Debug.WriteLine(employee.EmployeeID + "   " + employeeTimeEntryModel.EmployeeID + "  " + DateTime.Parse(employee.Date) + "   " + selectedDate);
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
