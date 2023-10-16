using Microsoft.AspNetCore.Mvc;
using RDP;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Data;
using Microsoft.Extensions.Hosting;

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
			servermanagerRequests = _context.servermanager.OrderByDescending(x => x.ServerName).ToList();
            return View(servermanagerRequests);
        }
		[HttpPost]
		public JsonResult AddServer(ServermanagerRequest request)
		{
			//if(request.IsAdServer ==true)
			//	request.ServerUsername = "adserver\\" + request.ServerUsername;

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
			return Json("");
		}
		[HttpPost]
		public JsonResult ConnectRdc(string id)
		{
			Guid guid = new Guid(id);
			var server= _context.servermanager.Where(x => x.Id == guid).FirstOrDefault();
			Programv2.Connect(new LogInfo()
			{
				Name = server.ServerName + server.ServerHost,
				Ipaddress = server.ServerIpAddress,
				Username = server.ServerUsername,
				Password = server.ServerPassword,
			});

			return Json("");
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

		[HttpGet]
		public ActionResult SiteManager()
		{
			List<SiteManager> siteManagers = new List<SiteManager>();
			var  sites= _context.siteManagers.OrderByDescending(x => x.SiteName).ToList();
			return View(sites);
		}

		[HttpPost]
		public JsonResult AddSite(SiteManager request)
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

			string username =data.UserName;
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
			catch(Exception)
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