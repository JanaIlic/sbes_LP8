using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Contracts;
using Security;
using System.ServiceModel;
using System.Security;
using System.Security.Principal;
using System.ServiceModel.Description;
using System.IdentityModel;
using System.IdentityModel.Policy;

namespace Service
{
    public class Program
    {

        static void Main(string[] args)
        {

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;


            ServiceHost sh = new ServiceHost(typeof(Service));       
            sh.AddServiceEndpoint(typeof(IService), binding, "net.tcp://localhost:9000/Service");

            sh.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            sh.Description.Behaviors.Add(new ServiceDebugBehavior()
            {IncludeExceptionDetailInFaults = true });

            sh.Authorization.ServiceAuthorizationManager = new CustomServiceAuthorizationManager();
            sh.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAutoriztionPolicy());
            sh.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();


            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;
            newAudit.SuppressAuditFailure = true;

            sh.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            sh.Description.Behaviors.Add(newAudit); 

            sh.Open();

            Console.WriteLine("{0} je pokrenuo servis.", WindowsIdentity.GetCurrent().Name);

            Console.ReadLine();
            sh.Close();
        }
    }
}
