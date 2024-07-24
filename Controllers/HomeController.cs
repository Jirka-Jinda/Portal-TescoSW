using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;

namespace Rozcestink_Redefined.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
            return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult Portal() 
		{ 
			return Redirect("https://devportal.tescosw.loc/");
		}

		public IActionResult SiteLinks()
		{
			return View(SiteLinksModel.GetSiteLinks());
		}
	}
}