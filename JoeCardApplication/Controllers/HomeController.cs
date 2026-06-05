using JoeCardApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace JoeCardApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public string APIKey = string.Empty;
        // Host, marketingVersionCode, currentSite, apiSuccess, InputValidPass

        public enum EventType { InvalidDataFormat = 2007, Unknown = 2000 };

       // private readonly PageTrackingService _pageTracker;
       // private readonly MemberAccessExceptionSettings _macSettings;
       // private readonly IWebHostEnvironment _env;
       // private readonly HostingEnvironmentInfo _envInfo;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            Console.WriteLine("right here");
        }

        public IActionResult Index()
        {
            // CertificateRequest certificateRequest = new CertificateRequest();
            // certificateRequest.marketingVersionCode = landingWelcomeSite(certificateRequest.currentSite);
            // 

            return View();
        }

        public IActionResult DependencyInjectionDemo()
        {
            // CertificateRequest certificateRequest = new CertificateRequest();
            // certificateRequest.marketingVersionCode = landingWelcomeSite(certificateRequest.currentSite);
            // 

            return View();
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
