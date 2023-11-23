using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace xyzboutique.dataaccess.Filters;
public static class DbHealthCheckProvider
{
  public static HealthCheckResult Check(string connectionString)
  {
    // Code to check if DB is running
    try
    {
      using (var connection = new SqlConnection(connectionString))
      {
        connection.Open();
        connection.Close();
      }
      return HealthCheckResult.Healthy("Database is running");
    }
    catch
    {
      return HealthCheckResult.Unhealthy("Could not connect to database!, SqlConnection: " + string.Format(connectionString));
    }
  }
}