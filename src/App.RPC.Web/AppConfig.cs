using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace App.RPC.Web
{
    public static class AppConfig
    {
        public static IConfigurationRoot Configuration { get;  set; }
        public static void RegisterConfig(this IConfigurationRoot configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            Configuration = configuration;
        }
       
        public static string GetConnectionString(string name)
        {
            return Configuration.GetConnectionString(name);
        }

        public static T GetAppSettings<T>(string name)
        {
            return Configuration.GetSection("AppSettings").GetValue<T>(name);
        }
    }
}
