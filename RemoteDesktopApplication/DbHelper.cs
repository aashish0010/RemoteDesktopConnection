using Dapper;
using Microsoft.Data.SqlClient;

namespace RemoteDesktopApplication
{
	public class DbHelper
	{
		private readonly IConfiguration _configuration;
		public DbHelper(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Execute(string sql)
		{
			SqlConnection connection = new SqlConnection(_configuration.GetConnectionString(""));
			connection.Open();
			connection.Execute(sql);
		}
	}
}
