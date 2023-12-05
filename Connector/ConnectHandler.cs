using RDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connector
{
	public class ConnectHandler
	{
		public string ConnectionManger(LogInfoV2 log)
		{
			return RdpHandler.Rrocess(log);
		}
	}
}
