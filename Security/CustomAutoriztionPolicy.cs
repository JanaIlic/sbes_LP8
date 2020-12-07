using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security;
using System.Security.Principal;
using System.IdentityModel.Policy;
using System.IdentityModel.Claims;

namespace Security
{
    public class CustomAutoriztionPolicy : IAuthorizationPolicy
    {

        public CustomAutoriztionPolicy()
        {
            Id = Guid.NewGuid().ToString();
        }


        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public string Id { get; }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            if (!evaluationContext.Properties.TryGetValue("Identities", out object list))
             return false; 

            IList<IIdentity> identities = list as IList<IIdentity>;
            if (list == null || identities.Count <= 0)
             return false; 

            evaluationContext.Properties["Principal"] =
                new CustomPrincipal((WindowsIdentity)identities[0]);
            return true;
        }
    }
}
