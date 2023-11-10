using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AutoStart
{
	public partial class Service1 : ServiceBase
	{
		public Service1()
		{
			StartASPNetCoreApplication();
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			StartASPNetCoreApplication();
		}

		protected override void OnStop()
		{
			StopASPNetCoreApplication();
		}
		private static bool IsPortInUse(int port)
		{
			IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
			IPEndPoint[] endPoints = ipProperties.GetActiveTcpListeners();
			return endPoints.Any(endpoint => endpoint.Port == port);
		}

		private void StartASPNetCoreApplication()
		{
			string appPath = "C:\\Users\\INFI\\Desktop\\RemoteConnectionService\\RemoteDesktopApplication.dll";


			int port = 5000;
			bool portInUse;

			do
			{
				portInUse = IsPortInUse(port);

				if (portInUse)
				{
					port++;
				}
			} while (portInUse);

			if (System.IO.File.Exists(appPath))
			{
				ProcessStartInfo startInfo = new ProcessStartInfo
				{
					FileName = "dotnet",
					Arguments = $"\"{appPath}\" --urls {port}",
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

					process.Start();
					process.BeginOutputReadLine();
					process.BeginErrorReadLine();
					process.WaitForExit();

					if (process.ExitCode != 0)
					{
						Console.WriteLine("ASP.NET Core application failed to start.");
					}
					else
					{
						Console.WriteLine("ASP.NET Core application started successfully.");
					}
				}
			}
			else
			{
				Console.WriteLine("The specified ASP.NET Core application does not exist.");
			}
		}

		private void StopASPNetCoreApplication()
		{

		}
	}
}
