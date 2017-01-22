namespace Sitecore.Cleanup
{
    public class History : TaskBase
    {
        public void Run()
        {
            Table = Constants.HistoryTable;
            CleanupSqlQueryStringFormat = Constants.HistoryTableSqlCleanupQuery;
            RunTask();
        }
    }
}
