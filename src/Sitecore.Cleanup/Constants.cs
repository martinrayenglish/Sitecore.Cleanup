namespace Sitecore.Cleanup
{
    public class Constants
    {
        public static int DefaultThreshold
        {
            get { return 900; }
        }

        public static string EventQueueTable
        {
            get { return "EventQueue"; }
        }

        public static string HistoryTable
        {
            get { return "History"; }
        }

        public static string PublishQueueTable
        {
            get { return "PublishQueue"; }
        }

        public static string EventQueueTableSqlCleanupQuery
        {
            get { return "DELETE FROM {1} WHERE id NOT IN(SELECT Top({0}) id FROM {1} ORDER BY Created DESC)"; }
        }

        public static string PublishQueueTableSqlCleanupQuery
        {
            get { return "DELETE FROM {1} WHERE id NOT IN(SELECT Top({0}) id FROM {1} ORDER BY Date DESC)"; }
        }

        public static string HistoryTableSqlCleanupQuery
        {
            get { return "DELETE FROM {1} WHERE id NOT IN(SELECT Top({0}) id FROM {1} ORDER BY Created DESC)"; }
        }

        public static string CountSqlQuery
        {
            get { return "SELECT COUNT(*) FROM {{0}}{0}{{1}}"; }
        }
    }
}
