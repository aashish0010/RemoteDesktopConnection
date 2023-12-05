using Connector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using RDP;
using System.Data;
using System.Text;

namespace RemoteDesktopApplication.Controllers
{
	public static class StaticHandler
	{
		public static void DeleteAllRdpFiles(string directoryPath)
		{
			try
			{
				// Get all files with the ".rdp" extension in the specified directory
				string[] rdpFiles = Directory.GetFiles(directoryPath, "*.rdp");

				// Delete each file
				foreach (var filePath in rdpFiles)
				{
					File.Delete(filePath);
				}

				Console.WriteLine($"Deleted {rdpFiles.Length} .rdp files in {directoryPath}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error deleting .rdp files: {ex.Message}");
			}
		}
	}
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _hostingEnvironment;


		public HomeController(ApplicationDbContext dbContext, IWebHostEnvironment hostingEnvironment)
		{
			_context = dbContext;
			_hostingEnvironment = hostingEnvironment;

		}

		public IActionResult Index()
		{
			List<Servermanager> servermanagerRequests = new List<Servermanager>();
			servermanagerRequests = _context.servermanager.OrderByDescending(x => x.ServerName).ToList();
			return View(servermanagerRequests);
		}
		[HttpPost]
		public JsonResult AddServer(ServermanagerRequest request)
		{
			request.ServerUsername = request.ServerUsername.Replace("adserver\\", "");
			if (request.IsAdServer == true)
				request.ServerUsername = "adserver\\" + request.ServerUsername;


			if (!string.IsNullOrWhiteSpace(request.id))
			{
				Guid guid = Guid.Parse(request.id);
				Servermanager servermanager = _context.servermanager.Find(guid);
				servermanager.ServerHost = request.ServerHost;
				servermanager.ServerIpAddress = request.ServerIpAddress;
				servermanager.ServerPassword = request.ServerPassword;
				servermanager.ServerName = request.ServerName;
				servermanager.ServerDescription = request.ServerDescription;
				servermanager.ServerType = request.ServerType;
				servermanager.ServerUsername = request.ServerUsername;
				_context.SaveChanges();
			}
			else
			{
				_context.servermanager.Add(new Servermanager()
				{
					ServerDescription = request.ServerDescription,
					ServerName = request.ServerName,
					ServerIpAddress = request.ServerIpAddress.Replace("\n", ""),
					ServerType = request.ServerType,
					ServerPassword = request.ServerPassword.Replace("\n", ""),
					ServerUsername = request.ServerUsername.Replace("\n", ""),
					ServerHost = request.ServerHost
				});
				_context.SaveChanges();
			}

			return Json("");
		}
		[HttpGet]
		public IActionResult ConnectRdc(string id)
		{
			Guid guid = new Guid(id);
			var server = _context.servermanager.Where(x => x.Id == guid).FirstOrDefault();
			ConnectHandler connect = new ConnectHandler();

			string rootfile = Path.Combine(_hostingEnvironment.ContentRootPath, "FileManager");
			StaticHandler.DeleteAllRdpFiles(rootfile);
			StaticHandler.DeleteAllRdpFiles("C:\\Users\\pudas\\Downloads");
			var fileName =  connect.ConnectionManger(new LogInfoV2()
			{
				Name = server.ServerName + server.ServerHost,
				Ipaddress = server.ServerIpAddress,
				Username = server.ServerUsername,
				Password = server.ServerPassword,
			});

			string filePath = Path.Combine(rootfile, fileName);
			if (System.IO.File.Exists(filePath))
			{
				byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
				string fileContent = Encoding.UTF8.GetString(fileBytes);

				Response.Headers["Content-Disposition"] = $"attachment; filename={fileName}";
				Response.ContentType = "application/x-rdp";
				return File(fileBytes, "application/octet-stream", fileName);
			}
			return NotFound();

		}
		[HttpPost]
		public JsonResult DeleteRdc(string id)
		{
			Guid guid = new Guid(id);
			var server = _context.servermanager.Where(x => x.Id == guid).FirstOrDefault();
			_context.Remove(server);
			_context.SaveChanges();
			return Json("");
		}

		[HttpPost]
		public JsonResult RDCDetail(string id)
		{
			Guid guid = new Guid(id);
			Servermanager details = _context.servermanager.Where(x => x.Id == guid).FirstOrDefault();
			//details.ServerUsername = details.ServerUsername.Replace("adserver\\", "");
			return Json(details);
		}

		[HttpGet]
		public ActionResult SiteManager()
		{
			List<SiteManager> siteManagers = new List<SiteManager>();
			var sites = _context.siteManagers.OrderByDescending(x => x.SiteName).ToList();
			return View(sites);
		}

		[HttpPost]
		public JsonResult AddSite(SiteManageRequest request)
		{
			if (!string.IsNullOrEmpty(request.Id))
			{
				Guid guid = new Guid(request.Id);
				var data = _context.siteManagers.Find(guid);
				data.SiteLink = request.SiteLink;
				data.AccessCode = request.AccessCode;
				data.SiteRoot = request.SiteRoot;
				data.SiteName = request.SiteName;
				data.SiteType = request.SiteType;
				data.UserName = request.UserName;
				data.Password = request.Password;
				_context.SaveChanges();
			}
			else
			{
				_context.siteManagers.Add(new SiteManager()
				{
					SiteName = request.SiteName,
					SiteLink = request.SiteLink,
					SiteRoot = request.SiteRoot,
					SiteType = request.SiteType,
					UserName = request.UserName,
					Password = request.Password,
					AccessCode = request.AccessCode
				});
				_context.SaveChanges();
			}

			return Json("");
		}

		public JsonResult Connect(string id)
		{
			Guid guid = new Guid(id);
			var data = _context.siteManagers.Where(x => x.Id == guid).FirstOrDefault();
			IWebDriver driver = new ChromeDriver();
			driver.Manage().Window.Maximize();
			string loginUrl = data.SiteLink;
			driver.Navigate().GoToUrl(loginUrl);

			string username = data.UserName;
			string password = data.Password;
			string accesstoken = data.AccessCode;
			IWebElement usernameField = driver.FindElement(By.Name("UserName"));
			IWebElement passwordField = driver.FindElement(By.Name("Password"));
			IWebElement accesstokenField = driver.FindElement(By.Name("AccessCode"));
			IWebElement loginButton = driver.FindElement(By.ClassName("cus_btn"));
			WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
			usernameField.SendKeys(username);
			passwordField.SendKeys(password);
			accesstokenField.SendKeys(accesstoken);
			((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", loginButton);
			return Json("");

		}

		[HttpPost]
		public JsonResult DeleteSite(string id)
		{
			Guid guid = new Guid(id);
			var server = _context.siteManagers.Where(x => x.Id == guid).FirstOrDefault();
			_context.Remove(server);
			_context.SaveChanges();
			return Json("");
		}

		[HttpPost]
		public JsonResult SiteDetail(string id)
		{
			Guid guid = new Guid(id);
			SiteManager server = _context.siteManagers.Where(x => x.Id == guid).FirstOrDefault();
			return Json(server);
		}

		[HttpPost]
		public JsonResult ClearSesssion(string id)
		{
			try
			{
				Guid guid = new Guid(id);
				var data = _context.siteManagers.Where(x => x.Id == guid).FirstOrDefault();
				IWebDriver driver = new ChromeDriver();
				driver.Manage().Window.Maximize();
				string loginUrl = data.SiteLink;
				driver.Navigate().GoToUrl(loginUrl + "/usercontrol");

				string username = data.UserName;
				string password = data.Password;
				string accesstoken = data.AccessCode;
				IWebElement usernameField = driver.FindElement(By.Name("UserName"));
				IWebElement passwordField = driver.FindElement(By.Name("Password"));
				IWebElement accesstokenField = driver.FindElement(By.Name("AccessCode"));
				IWebElement loginButton = driver.FindElement(By.ClassName("btn"));



				WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				usernameField.SendKeys(username);
				passwordField.SendKeys(password);
				accesstokenField.SendKeys(accesstoken);
				((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", loginButton);
				WebDriverWait wait1 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

				IWebElement clearsession = driver.FindElement(By.ClassName("btn-default"));
				((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", clearsession);
				WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				IWebElement clearsessionv2 = driver.FindElement(By.ClassName("btn-default"));
				((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", clearsessionv2);

				IWebElement backtologin = driver.FindElement(By.TagName("a"));
				((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", backtologin);


				WebDriverWait wait5 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				IWebElement usernameField1 = driver.FindElement(By.Name("UserName"));
				IWebElement passwordField1 = driver.FindElement(By.Name("Password"));
				IWebElement accesstokenField1 = driver.FindElement(By.Name("AccessCode"));

				IWebElement loginButton1 = driver.FindElement(By.ClassName("cus_btn"));
				WebDriverWait wait6 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				usernameField1.SendKeys(username);
				passwordField1.SendKeys(password);
				accesstokenField1.SendKeys(accesstoken);
				((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", loginButton1);
			}
			catch (Exception)
			{
				Connect(id);
			}
			return Json("");
		}
		[HttpGet]
		public ActionResult JsonXmlBeautify()
		{
			return View();
		}


		

	}
}