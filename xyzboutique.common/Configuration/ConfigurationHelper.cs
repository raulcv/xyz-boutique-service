using Microsoft.Extensions.Configuration;
using System;

namespace xyzboutique.common.Configuration;
public class ConfigurationHelper
{
  public static IConfigurationRoot GetConfiguration(string path, string environmentName = null)
  {
    var builder = new ConfigurationBuilder()
        .SetBasePath(path)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

    if (!String.IsNullOrWhiteSpace(environmentName))
    {
      builder = builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false);
    }

    return builder.Build();
  }
}