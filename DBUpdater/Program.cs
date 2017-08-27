using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            Helper helper = new Helper();

            //List<string> collName = new List<string>();
            //collName.Add("extra2");
            //collName.Add("extra3");

            //collName.Add("extra4");

            //Console.WriteLine(QueryProvider.GetSQLFor_FindDependentView("hitbgt", collName));

            //helper.GetDependentView("AT19", "hitworkpackage", collName);

            //helper.RunUpdateScript();
            //for (int i = 0; i < 3; i++)
            //{
                //DBHandler handler = new DBHandler(Config.GetProjectDBConnectionString("test"));
                //DataTable dt = handler.GetDataTableTest("select * from student");

                //Console.WriteLine(dt.Rows.Count);
            //}

            helper.RunUpdateScript(); 
            
            Console.WriteLine("\nDone");
            Console.ReadKey();
        }
    }
}
