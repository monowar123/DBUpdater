using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdater
{
    public static class Config
    {
        public static string Host { get; set; }
        public static string Port { get; set; }
        public static string SuperUserId { get; set; }
        public static string SuperPassword { get; set; }

        static Config()
        {
            Host = "37.34.53.43";
            Port = "5432";
            SuperUserId = "postgres";
            SuperPassword = "C5RejawReh";
        }

        public static string GetSuperConnectionString()
        {
            return string.Format("SERVER={0};Port={1};User id={2};Password={3};encoding=unicode",
                Host, Port, SuperUserId, SuperPassword);
        }

        public static string GetProjectDBConnectionString(string projectDBName)
        {
            return string.Format("SERVER={0};Port={1};Database={2};User id={3};Password={4};encoding=unicode;pooling=true;connectionlifetime=1;",
                Host, Port, projectDBName, SuperUserId, SuperPassword);
        }
        
    }
}
