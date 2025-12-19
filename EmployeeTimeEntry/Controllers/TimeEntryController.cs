using CsvHelper;
using EmployeeTimeEntry.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace EmployeeTimeEntry.Controllers
{
    public class TimeEntryController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;


        public TimeEntryController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult TimeEntry(string someValue)
        {
            string csvFolderPathEmployees = Path.Combine(_hostingEnvironment.ContentRootPath, "EmployeeData", "Employees.csv");
            string csvFolderPathTimeEntries = Path.Combine(_hostingEnvironment.ContentRootPath, "EmployeeData", "TimeEntries.csv");

            // var records = null;

            using var streamReaderEmployees = new StreamReader(csvFolderPathEmployees);
            using var streamReaderTimeEntries = new StreamReader(csvFolderPathTimeEntries);


            using var csvReaderEmployees = new CsvReader(streamReaderEmployees, CultureInfo.InvariantCulture);
            using var csvReaderTimeEntries = new CsvReader(streamReaderTimeEntries, CultureInfo.InvariantCulture);

            var recordEmployees = csvReaderEmployees.GetRecords<Employees>();
            var recordTimeEntries = csvReaderTimeEntries.GetRecords<TimeEntries>();

            var employees = new List<Employees>();

            foreach (var recordEmployee in recordEmployees)
            {
                // Debug.WriteLine(record.EmployeeID + "    " + record.FirstName + "    " + record.LastName);

                employees = new List<Employees>
                {
                    new Employees {EmployeeID = recordEmployee.EmployeeID, FirstName = recordEmployee.FirstName, LastName = recordEmployee.LastName}
                };
            }

            List<EmployeeTimeEntryModel> employeeList = new List<EmployeeTimeEntryModel>();


           // ViewBag.CityList = new ToSelectList(employees, "CityID", "CityName");



            int i = 0;
            EmployeeTimeEntryModel emp = new EmployeeTimeEntryModel();

            foreach (var timedEntry in recordTimeEntries)
            {
                // Debug.WriteLine(timedEntry.EntryID + "    " + timedEntry.EmployeeID + "    " + timedEntry.Date + "    " + timedEntry.InTime + "    " + timedEntry.OutTime);
                i++;
                // EmployeesModel ee = new EmployeesModel();
                // ee = recordEmployees;
                //emp.NamesList = new SelectList(employees, "EmployeeID", "FirstName");
                emp.FirstName = "Jim " + i;
                emp.LastName = "Johnnyson";
                emp.Date = timedEntry.Date;
                emp.InTime = timedEntry.InTime;
                emp.OutTime = timedEntry.OutTime;
               // emp.SelectedNameId = emp.EmployeeID;
               // emp.NamesList = new SelectList(employeeList, "EmployeeID", "FirstName");


                employeeList.Add(emp);

               /* for (int i = 0; i < recordEmployees.Count(); i++)
                {
                   // Debug.WriteLine(records.EmployeeID + "    " + records.FirstName + "    " + records.LastName);

                    if (timedEntry.EmployeeID == recordEmployees.)
                    {
                   //     Debug.WriteLine(employee.FirstName + "  " + employee.LastName);
                   //     goto restart
                   //     break;
                    }
                } */
            }

            foreach (EmployeeTimeEntryModel ee in employeeList)
            {
                Debug.WriteLine(ee.FirstName + "   " + ee.LastName + "  " + ee.Date + "   " + ee.InTime);
            }

            var sortedTest = employeeList.OrderBy(e => e.InTime).ToList();

            ViewData["employeeList"] = sortedTest;
            //ViewData["employeeList"] = employeeList;


            return View();
        }
    }
}
