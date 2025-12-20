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


        public TimeEntryController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult TimeEntry(EmployeeTimeEntryModel employeeTimeEntryModel)
        {
            string csvFolderPathEmployees = Path.Combine(_hostingEnvironment.ContentRootPath, "EmployeeData", "Employees.csv");
            string csvFolderPathTimeEntries = Path.Combine(_hostingEnvironment.ContentRootPath, "EmployeeData", "TimeEntries.csv");

            if (employeeTimeEntryModel.EmployeeID != null)
            {
                // Append to the file.
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false
                };

                //config.hea
                bool append = true;
                using var streamWriterEmployees = new StreamWriter(csvFolderPathTimeEntries, true);

                List<TimeEntries> timeEntries = new List<TimeEntries>();
                TimeEntries tm = new TimeEntries();
                tm.EntryID = "1200";
                tm.EmployeeID = employeeTimeEntryModel.EmployeeID;
                tm.Date = employeeTimeEntryModel.Date.ToShortDateString();
                tm.InTime = employeeTimeEntryModel.InTime;
                tm.OutTime = employeeTimeEntryModel.OutTime;
                timeEntries.Add(tm);

                foreach (var timeEntry in timeEntries)
                {
                    Debug.WriteLine(timeEntry);
                }

                using (var csvWriteEmployee = new CsvWriter(streamWriterEmployees, config))
                {
                    csvWriteEmployee.WriteRecords(timeEntries);
                };
            }


            // try
            // {
            //if (!File.Exists(csvFolderPathTimeEntries))
            // {

            // }
            //  }

            //  catch
            //  {

            //  }


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
            viewModel.NamesList = new List<SelectListItem>();

            foreach (var employee in employeesList)
            {
                viewModel.NamesList.Add(new SelectListItem { Text = employee.FirstName + " " + employee.LastName, Value = employee.EmployeeID });
            }


            List<EmployeeTimeEntryModel> employeeTimeEntryList = new List<EmployeeTimeEntryModel>();  // contains columns from Empployees.csv and TimeEntries.csv

            foreach (var timedEntry in recordTimeEntries)  // match EmployeeID with employees first and last name and add columns to display in employeeTimeEntryList
            {
                string firstName = employeesList.Where(e => e.EmployeeID == timedEntry.EmployeeID).First().FirstName;
                string lastName = employeesList.Where(e => e.EmployeeID == timedEntry.EmployeeID).First().LastName;

                Debug.WriteLine(timedEntry.Date);
                EmployeeTimeEntryModel employeeTimeEntry = new EmployeeTimeEntryModel();
                employeeTimeEntry.FirstName = firstName;
                employeeTimeEntry.LastName = lastName;
                employeeTimeEntry.Date = DateTime.Parse( timedEntry.Date);
                employeeTimeEntry.InTime = timedEntry.InTime;
                employeeTimeEntry.OutTime = timedEntry.OutTime;


                employeeTimeEntryList.Add(employeeTimeEntry);
            }

            var sortedTest = employeeTimeEntryList.OrderBy(e => e.InTime).ToList();

            //ViewData["employeeList"] = sortedTest;
            ViewData["employeeList"] = employeeTimeEntryList;


            return View(viewModel);
        }
    }
}
