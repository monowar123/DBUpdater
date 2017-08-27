using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUpdater
{
    public class QueryProvider
    {
        public static string GetSQLFor_GetDatabaseList()
        {
            return "SELECT datname as db_name FROM pg_database WHERE datistemplate = false order by db_name asc;";
        }

        public static string GetSQLFor_FindDependentView(string tableName, List<string> collName)       
        {
            string query = string.Format(@"SELECT distinct dependent_ns.nspname as dependent_schema
                                , dependent_view.relname as dependent_view 
                                , source_ns.nspname as source_schema
                                , source_table.relname as source_table
                                --, pg_attribute.attname as column_name
                                FROM pg_depend 
                                JOIN pg_rewrite ON pg_depend.objid = pg_rewrite.oid 
                                JOIN pg_class as dependent_view ON pg_rewrite.ev_class = dependent_view.oid 
                                JOIN pg_class as source_table ON pg_depend.refobjid = source_table.oid 
                                JOIN pg_attribute ON pg_depend.refobjid = pg_attribute.attrelid 
                                    AND pg_depend.refobjsubid = pg_attribute.attnum 
                                JOIN pg_namespace dependent_ns ON dependent_ns.oid = dependent_view.relnamespace
                                JOIN pg_namespace source_ns ON source_ns.oid = source_table.relnamespace
                                WHERE 
                                source_ns.nspname = 'public'
                                AND source_table.relname = '{0}'
                                AND pg_attribute.attnum > 0 
                                AND (pg_attribute.attname = 'CollNameXX')
                                ORDER BY 1,2;", tableName);

            string collFilter = string.Empty;
            foreach (string coll in collName)
            {
                collFilter += "pg_attribute.attname = '" + coll + "' OR ";
            }

            collFilter = collFilter.Substring(0, collFilter.Length - 4);

            query = query.Replace("pg_attribute.attname = 'CollNameXX'", collFilter);

            return query;
        }

    }
}
