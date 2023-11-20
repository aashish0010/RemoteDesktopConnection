using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Timers;
using System.Threading.Tasks;
using System.Security.Policy;

namespace AutoStart
{
	public partial class Service1 : ServiceBase
	{
		private readonly System.Timers.Timer timer1 = new System.Timers.Timer();
		private readonly string appPath = "C:\\Users\\INFI\\Desktop\\RemoteConnectionService\\RemoteDesktopApplication.dll";

		public Service1()
		{
			InitializeComponent();
		}
		private void LogInfo(string message)
		{
			EventLog.WriteEntry("YourServiceName", message, EventLogEntryType.Information);
		}

		private void LogError(string message)
		{
			EventLog.WriteEntry("YourServiceName", message, EventLogEntryType.Error);
		}

		protected override void OnStart(string[] args)
		{
			try
			{
				LogInfo("Service starting...");
				Task.Run(() => StartASPNetCoreApplication());
				LogInfo("Service started.");
			}
			catch (Exception ex)
			{
				LogError($"Error during service startup: {ex.Message}");
			}
		}

		private async void StartASPNetCoreApplication()
		{
			int port = FindAvailablePort(5000);

			if (System.IO.File.Exists(appPath))
			{
				ProcessStartInfo startInfo = new ProcessStartInfo
				{
					FileName = "dotnet",
					Arguments = $"\"{appPath}\" --urls http://localhost:{port}",
					CreateNoWindow = true,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					WorkingDirectory = System.IO.Path.GetDirectoryName(appPath)
				};

				using (Process process = new Process { StartInfo = startInfo })
				{
					process.OutputDataReceived += (sender, e) =>
					{
						if (!string.IsNullOrEmpty(e.Data))
						{
							Console.WriteLine(e.Data);
						}
					};

					process.ErrorDataReceived += (sender, e) =>
					{
						if (!string.IsNullOrEmpty(e.Data))
						{
							Console.WriteLine("Error: " + e.Data);
						}
					};

					StorePinno($"http://localhost:{port}");
					process.Start();
					process.BeginOutputReadLine();
					process.BeginErrorReadLine();
					await Task.Run(() => process.WaitForExit());

					if (process.ExitCode != 0)
					{
						Console.WriteLine("ASP.NET Core application failed to start.");
					}
					else
					{
						Console.WriteLine($"ASP.NET Core application started successfully on port {port}.");
					}
				}
			}
			else
			{
				Console.WriteLine("The specified ASP.NET Core application does not exist.");
			}
		}



		private void StorePinno(string url)
		{
			string sql = $@"insert into PortHandler(Port) values ('{url}')";
			DbHelper helper = new DbHelper();
			helper.Execute(sql);
		}

		private int FindAvailablePort(int startPort)
		{
			int port = startPort;
			while (IsPortInUse(port))
			{
				port++;
			}
			return port;
		}

		private static bool IsPortInUse(int port)
		{
			IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
			IPEndPoint[] endPoints = ipProperties.GetActiveTcpListeners();
			return endPoints.Any(endpoint => endpoint.Port == port);
		}

		private void StopASPNetCoreApplication()
		{
			// Your stop logic here
		}
	}
}
