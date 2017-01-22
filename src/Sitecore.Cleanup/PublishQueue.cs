namespace Sitecore.Cleanup
{
    public class PublishQueue : TaskBase
    {
        public void Run()
        {
            Table = Constants.PublishQueueTable;
            CleanupSqlQueryStringFormat = Constants.PublishQueueTableSqlCleanupQuery;
            RunTask();
        }
    }
}
