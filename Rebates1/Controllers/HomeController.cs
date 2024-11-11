using Microsoft.AspNetCore.Mvc;
using Rebates1.Models;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Rebates1.Controllers
{
    public class HomeController : Controller
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;
        List<ApplicationStatistics> applicationStatistics = new();
        SqlConnection conn = new();
        SqlCommand com = new();
        SqlDataReader dr;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            conn.ConnectionString = _config.GetConnectionString("defaultConnection");
        }

        public IActionResult Index()
        {
            if (applicationStatistics.Count > 0)
            {
                applicationStatistics.Clear();
            }
            try
            {
                conn.Open();
                com.Connection = conn;
                com.CommandText = "SELECT COUNT(Rebate_Type) AS NumberOfApplication, Rebate_Type FROM [Objection].[dbo].[Rebate_Infos] GROUP BY Rebate_Type";

                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    applicationStatistics.Add(new ApplicationStatistics
                    {
                        RebateType = dr["Rebate_Type"].ToString(),
                        NumberOfApplications = (int)dr["NumberOfApplication"]
                    });
                }

                conn.Close();
            }
            catch (Exception e)
            {
                throw e;
            }

            return View(applicationStatistics);
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