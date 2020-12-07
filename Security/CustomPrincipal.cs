using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Contracts;
using System.Security;
using System.Security.Principal;


namespace Security
{
    public class CustomPrincipal : IPrincipal
    {
        WindowsIdentity identity = null;

        public CustomPrincipal(WindowsIdentity winID)
        {
            identity = winID;
        }

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string permission)
        {
            bool ret = false;
            foreach (IdentityReference group in this.identity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));
                string groupName = Parser.Parse(name.ToString());
                string[] perms;


                if (RolesConfig.GetPermissios(groupName, out perms))
                {
                    foreach (string p in perms)
                    {
                        if (p.Equals(permission))
                            ret = true;                      
                    }
                }

            }

            return ret;
        }

 


    }
}
