using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Resources;
using System.Reflection;

namespace Security
{
    public enum AuditEventTypes
    {
        AuthenticationSuccess = 0,
        AuthorizationSuccess = 1,
        AuthorizationFailed = 2
    }

    public class AuditEvents
    {
        public static ResourceManager manager = null;
        public static object resourceLock = new object();

        private static ResourceManager Manager
        {
            get
            {
                lock (resourceLock)
                {
                    if (manager == null)
                     manager = new ResourceManager(typeof(AuditEventFile).FullName, Assembly.GetExecutingAssembly()); 

                    return manager;
                }
            }
        }


        public static string AuthenticationSuccess
        {
            get { return Manager.GetString(AuditEventTypes.AuthenticationSuccess.ToString()); }
        }

        public static string AuthorizationSuccess
        {
            get { return Manager.GetString(AuditEventTypes.AuthorizationSuccess.ToString()); }
        }

        public static string AuthorizationFailed
        {
            get { return Manager.GetString(AuditEventTypes.AuthorizationFailed.ToString()); }
        }    

    }
}
