using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Contracts;
using System.ServiceModel;
using System.Security;
using System.Security.Principal;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;          

            Console.WriteLine("{0} je pokrenuo klijenta.", WindowsIdentity.GetCurrent().Name);

          // EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9000/Service"), EndpointIdentity.CreateUpnIdentity("wcfservice"));
           EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9000/Service"));

            using (Proxy proxy = new Proxy(binding, address))
            {
                proxy.CreateFile("probni fajl");
            }

            Console.ReadLine();


        }
    }
}
