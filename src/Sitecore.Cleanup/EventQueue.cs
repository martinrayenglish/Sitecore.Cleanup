using System.Collections.Generic;
using System.Data;
using System.Linq;

using Sitecore.Diagnostics;

namespace Sitecore.Cleanup
{
    public class EventQueue : TaskBase
    {
        public void Run()
        {
            Table = Constants.EventQueueTable;
            CleanupSqlQueryStringFormat = Constants.EventQueueTableSqlCleanupQuery;
            RunTask();
        }
    }
}
