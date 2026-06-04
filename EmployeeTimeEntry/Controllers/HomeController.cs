using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using EmployeeTimeEntry.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTimeEntry.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string myName = "JoeF";


            SqlConnection con = new SqlConnection("Data Source=JOE-DESKTOP;Initial Catalog=FirstDB;Integrated Security=True;");


            SqlCommand cmd = new SqlCommand("insert into TestTab Values ('mynameeeee', 'fool street 222');", con);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            

            HashSet<string> uniqueHobbies = new HashSet<string>();
            uniqueHobbies.Add("football");
            uniqueHobbies.Add("football");
            uniqueHobbies.Add("golf");
            uniqueHobbies.Add("Tennis");

            if (uniqueHobbies.Contains("football"))
            {
                Console.WriteLine("There are " + uniqueHobbies.Count + " hobbies, and somebody likes football");  // the count will be 3, since duplicates aren't allowed
            }


            Task task = new Task(() =>
            {
                Console.WriteLine("Running task in separate thread");
                int result = 1 + 2;
                Console.WriteLine(result);
            });

            task.Start();

            Console.WriteLine("Main thread is done");

            Console.ReadLine();


            return RedirectToAction("Index", "TimeEntry");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
