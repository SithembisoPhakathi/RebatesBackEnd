using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rebates1.Models
{
    public class ApplicationStatistics
    {
        public string RebateType { get; set; }
        public int NumberOfApplications { get; set; }
        public string? Name { get; set; }
    }
}

