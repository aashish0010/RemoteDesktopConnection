using Microsoft.AspNetCore.Mvc;
using RemoteDesktopApplication.Models;
using System.Diagnostics;

namespace RemoteDesktopApplication.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext dbContext)
		{
			_context = dbContext;
		}

		public IActionResult Index()
		{
			List<Servermanager> servermanagerRequests = new List<Servermanager>();
			servermanagerRequests = _context.servermanager.ToList();

			return View(servermanagerRequests);
		}
		[HttpPost]
		public JsonResult AddServer(ServermanagerRequest request)
		{

			_context.servermanager.Add(new Servermanager()
			{
				ServerDescription = request.ServerDescription,
				ServerName = request.ServerName,
				ServerIpAddress = request.ServerIpAddress,
				ServerType = request.ServerType,
				ServerPassword = request.ServerPassword,
				ServerUsername = request.ServerUsername
			});
			_context.SaveChanges();
			return Json("");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}