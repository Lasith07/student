using System;

namespace DemoAPI.Helpers.Utils.GlobalAttributes
{
    public static class GlobalAttributes
    {
        public static MySqlConfiguration MySqlConfiguration = new MySqlConfiguration();
        internal static object mySqlConfiguration;
    }

    public class MySqlConfiguration
    {
        public string? connectionString { get; set; }
    }
}
