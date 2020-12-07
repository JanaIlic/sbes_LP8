using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Security;
using Contracts;
using System.ServiceModel;
using System.Security;
using System.ServiceModel.Security;

namespace Client
{
    public class Proxy : ChannelFactory<IService>, IService, IDisposable
    {
        IService factory;
        public Proxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            Credentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            factory = this.CreateChannel();                    
        }

        public void CreateFile(string filename)
        {
            try
            {
                try
                {
                    factory.CreateFile(filename);
                }
                catch (SecurityAccessDeniedException e)
                {
                    Console.WriteLine("Error, security access denied exception: {0}", e.Message);
                }
                catch (FaultException e)
                {
                    Console.WriteLine("Error, fault exception: {0}", e.Message);
                }
            }
            catch (SecurityException e)
            {
                Console.WriteLine("Error while tryin ImAlive, security exception: {0}", e.Message);
            }  


        }


        public void Dispose()
        {
            if (factory != null)
                factory = null;

            this.Close();
        }

        public void AddNewPermissions(string rolename, params string[] permissios)
        {
            try
            {
                factory.AddNewPermissions(rolename, permissios);
                Console.WriteLine("Adding new permissios allowed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to add new permissions : {0}", e.Message);
            }
        }

        public void RemoveSomePermissions(string rolename, params string[] permissions)
        {
            try
            {
                factory.RemoveSomePermissions(rolename, permissions);
                Console.WriteLine("Removing permissions allowed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to remove permissions : {0}", e.Message);
            }
        }

        public void AddNewRole(string rolename)
        {
            try
            {
                factory.AddNewRole(rolename);
                Console.WriteLine("Adding new role allowed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to add a new role : {0}", e.Message);
            }
        }

        public void RemoveSomeRole(string rolename)
        {
            try
            {
                factory.RemoveSomeRole(rolename);
                Console.WriteLine("Removing roles allowed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to remove a role : {0}", e.Message);
            }
        }
    }
}
