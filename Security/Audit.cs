using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace Security
{
    public class Audit : IDisposable
    {
        public static EventLog thisLog = null;

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists("Security.Audit"))
                    EventLog.CreateEventSource("Security.Audit", "Test");

                thisLog = new EventLog("Test", Environment.MachineName, "Security.Audit");
            }
            catch (Exception e)
            {
                thisLog = null;
                Console.WriteLine("Greška pri kreiranju event log handle: {0}", e.Message);
            }

        }

        public static void AuthenticationSuccess(string username)
        {
            string success = AuditEvents.AuthenticationSuccess;
           if(thisLog == null)
              throw new ArgumentException(String.Format( "Neuspešan upis događaja {0} u event log.", (int)AuditEventTypes.AuthenticationSuccess));
           else
                thisLog.WriteEntry(String.Format(success, username));
          

        }

        public static void AuthorizationSuccess(string username, string servicename)
        {
            if (thisLog == null)
                throw new ArgumentException(String.Format( "Neuspešan upis događaja {0} u event log.", (int)AuditEventTypes.AuthorizationSuccess));
            else
                thisLog.WriteEntry(String.Format(AuditEvents.AuthorizationSuccess, username, servicename));

        }

        public static void AuthorizationFailed(string username, string servicename, string reason)
        {
            if (thisLog == null)
                throw new ArgumentException(String.Format("Neuspešan upis događaja {0} u event log.", (int)AuditEventTypes.AuthorizationFailed));
            else
                thisLog.WriteEntry(String.Format(AuditEvents.AuthorizationFailed, username, servicename, reason), EventLogEntryType.Error);
        }

        public void Dispose()
        {
           if(thisLog != null)
           {
                thisLog.Dispose();
                thisLog = null;
           }
        }
    }
}
