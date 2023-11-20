using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace AutoStart
{
	public class DbHelper
	{
		//private readonly IConfiguration _configuration;
		//public DbHelper(IConfiguration configuration)
		//{
		//	_configuration = configuration;
		//}

		public void Execute(string sql)
		{
			SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString);
			connection.Open();
			connection.Execute(sql);
		}

		public IEnumerable<dynamic> ExecuteSql(string sql)
		{
			SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationSettings.GetConfig("DBConnString").ToString());
			connection.Open();
			return connection.Query(sql).ToList();
			//return data.FirstOrDefault().ToString();
		}
	}
}