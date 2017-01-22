using System.Collections.Generic;
using System.Data;
using System.Linq;

using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.DataProviders.Sql;
using Sitecore.Data.SqlServer;
using Sitecore.Diagnostics;

namespace Sitecore.Cleanup
{
    public class TaskBase
    {
        protected string Threshold { get; set; }
        protected string Table { get; set; }
        protected string CleanupSqlQueryStringFormat { get; set; }

        private int ThresholdValue
        {
            get
            {
                int setThreshold;
                return int.TryParse(Threshold, out setThreshold) ? setThreshold : Constants.DefaultThreshold;
            }
        }

        public void RunTask()
        {
            Log.Info(string.Format("[Cleanup Monitor] Check started for {0} table", Table), typeof(TaskBase));

            var databasesWithHighCount = DatabasesWithHighCount();
            RemoveOldRowsAboveThreshold(databasesWithHighCount);

            Log.Info(string.Format("[Cleanup Monitor] Check completed for {0} table", Table), typeof(TaskBase));
        }

        public List<Database> DatabasesWithHighCount()
        {
            var highCountDatabases = new List<Database>();

            foreach (var database in Factory.GetDatabases().Where(c => !string.IsNullOrEmpty(c.ConnectionStringName)))
            {
                var recordCount = GetRecordCount(database);
                
                if (recordCount > ThresholdValue)
                {
                    highCountDatabases.Add(database);
                    Log.Info(string.Format("[Cleanup Monitor] {0} database {1} table above threshold with {2} rows", database.Name, Table, recordCount), typeof(TaskBase));
                }
                else
                {
                    Log.Info(string.Format("[Cleanup Monitor] {0} database {1} table number of rows: {2}", database.Name, Table, recordCount), typeof(TaskBase));
                }
            }

            return highCountDatabases;
        }

        public void RemoveOldRowsAboveThreshold(IEnumerable<Database> databasesWithHighCount)
        {
            foreach (var database in databasesWithHighCount)
            {
                var connectionString = Settings.GetConnectionString(database.ConnectionStringName);
                var dataApi = new SqlServerDataApi(connectionString);

                var sqlStatement = new SqlStatement
                {
                    Select = string.Format(CleanupSqlQueryStringFormat, ThresholdValue, Table),
                    OrderBy = string.Empty
                };

                dataApi.ExecuteNoResult(sqlStatement.Select, sqlStatement.GetParameters());

                var recordCount = GetRecordCount(database);

                Log.Info(string.Format("[Cleanup Monitor] Old rows removed from {0} database {1} table and row count now {2}", database.Name, Table, recordCount), typeof(TaskBase));
            }
        }

        public long GetRecordCount(Database database)
        {
            var connectionString = Settings.GetConnectionString(database.ConnectionStringName);
            var dataApi = new SqlServerDataApi(connectionString);
            return Enumerable.FirstOrDefault(dataApi.CreateObjectReader(string.Format(Constants.CountSqlQuery, Table), new object[0], (r => GetLong(r, 0))));
        }
        protected static long GetLong(IDataReader reader, int columnIndex)
        {
            Assert.ArgumentNotNull((object)reader, "reader");
            var obj = reader.GetValue(columnIndex);
            var numArray = obj as byte[];
            if (numArray != null)
            {
                return numArray.Aggregate(0L, (current, b) => (current << 8) + b);
            }
            return MainUtil.GetLong(obj, 0L);
        }
    }
}
