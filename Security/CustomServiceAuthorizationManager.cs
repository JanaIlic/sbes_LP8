using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security;
using System.Security.Principal;
using System.ServiceModel;

namespace Security
{
    public class CustomServiceAuthorizationManager : ServiceAuthorizationManager
    {

        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            CustomPrincipal principal = operationContext.ServiceSecurityContext.
                AuthorizationContext.Properties["Principal"] as CustomPrincipal;

            return principal.IsInRole("See");
        }

    }
}
