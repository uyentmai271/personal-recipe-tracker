using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalRecipeTrackerLibrary.Data
{
    public static class GlobalConfig
    {

        public static string ConnectionString => ConfigurationManager.ConnectionStrings["PersonalRecipeTrackerConnection"]?.ConnectionString ?? string.Empty;
        public static string Database => "PersonalRecipeTracker_DB";
        public static string ServerName => ConfigurationManager.AppSettings["ServerName"] ?? string.Empty;
        public static string ApplicationName => ConfigurationManager.AppSettings["ApplicationName"] ?? string.Empty;
        public static string Version => ConfigurationManager.AppSettings["Version"] ?? string.Empty;
    }
}
