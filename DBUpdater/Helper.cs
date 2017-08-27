using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdater
{
    public class Helper
    {
        DBHandler dbHandler;
        private string ScriptBasePath;
        List<string> ScriptContent = new List<string>();

        Dictionary<string, string> ScriptDictionary = new Dictionary<string, string>();

        public Helper()
        {
            ScriptBasePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Scripts");
        }

        public List<string> GetDataBaseList()
        {
            List<string> dbList = new List<string>();
            dbHandler = new DBHandler(Config.GetSuperConnectionString());
            DataTable dt = dbHandler.GetDataTable(QueryProvider.GetSQLFor_GetDatabaseList());

            foreach (DataRow dr in dt.Rows)
            {
                string dbName = dr["db_name"].ToString();
                if (dbName.Length <= 5 && (dbName.StartsWith("AO") || dbName.StartsWith("ML") || dbName.StartsWith("AG") || dbName.StartsWith("GB")))
                {
                    dbList.Add(dbName);
                }
            }

            return dbList;
        }

        public void ReadAllScripts()
        {
            var allFiles = Directory.GetFiles(ScriptBasePath).OrderBy(f => f);

            foreach (string file in allFiles)
            {
                string script = File.ReadAllText(file);     

                ScriptDictionary.Add(Path.GetFileName(file), script);
            }
        }

        public void RunUpdateScript()
        {
            List<string> dbList = GetDataBaseList();

            //List<string> dbList = new List<string>();

            //dbList.Add("ACC1");
            dbList.Add("AT19");
            dbList.Add("DM01");

            ReadAllScripts();

            foreach (string dbName in dbList)
            {
                try
                {
                    dbHandler = new DBHandler(Config.GetProjectDBConnectionString(dbName));
                    LogWritter.WriteLog("----------Running scripts for " + dbName + "-----------");
                    Console.WriteLine("----------Running scripts for " + dbName + "-----------");

                    foreach (var item in ScriptDictionary)
                    {
                        try
                        {
                            string script = item.Value.Replace("AOXX", dbName.ToUpper());
                            dbHandler.ExecuteNonQuery(script);

                            LogWritter.WriteLog(item.Key + " - runs successfully.");
                            Console.WriteLine(item.Key + " - runs successfully.");
                        }
                        catch (Exception ex)
                        {
                            LogWritter.WriteLog(item.Key + " - run fail. " + ex.Message.ToString());
                            Console.WriteLine(item.Key + " - run fail. " + ex.Message.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogWritter.WriteLog("--- Failed to connect " + dbName + "------");
                    LogWritter.WriteLog(ex.Message.ToString());

                    Console.WriteLine("--- Failed to connect " + dbName + "------");
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }

        public List<string> GetDependentView(string dbName, string tableName, List<string> collName)
        {
            List<string> viewList = new List<string>();

            dbHandler = new DBHandler(Config.GetProjectDBConnectionString(dbName));
            DataTable dt = dbHandler.GetDataTable(QueryProvider.GetSQLFor_FindDependentView(tableName, collName));

            foreach (DataRow dr in dt.Rows)
            {
                viewList.Add(dr["dependent_view"].ToString());
                Console.WriteLine(dr["dependent_view"].ToString());
            }

            return viewList;
        }
    }
}
