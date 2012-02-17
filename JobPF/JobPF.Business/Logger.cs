using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace JobPF.Business
{
    public static class Logger
    {


        public static void WriteMessage(string message)
        {
            EventLog.WriteEntry("JobPF", message, EventLogEntryType.Information);
        }

        public static void WriteError(string message)
        {
            EventLog.WriteEntry("JobPF", message, EventLogEntryType.Error);
        }
    }
}
