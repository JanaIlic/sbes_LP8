using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Security;
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

            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9000/Service"));

            Console.WriteLine("{0} je pokrenuo klijenta.", WindowsIdentity.GetCurrent().Name);


            using (Proxy proxy = new Proxy(binding, address))
            {

                /*adminove metode, rade, testirane, kasnije srediti UI i pozive..
                  proxy.AddNewRole("new role");
                  proxy.RemoveSomeRole("new role");
                  proxy.RemoveSomePermissions("Editor", new string[] {"Edit"});
                  proxy.AddNewPermissions("Editor", new string[] {"Edit"}); */


                /* datoteke
                  Console.WriteLine("Unesi ime fajla: ");
                  string filename = Console.ReadLine();
                  Console.WriteLine("Unesi tekst:");
                  string txt = Console.ReadLine();
                  proxy.CreateFile(filename, txt);

                 proxy.ReadFile(filename); */



                //  proxy.CreateFolder("readerov folder","novi folder");
              //  proxy.ShowFolderContent("novi folder");
            

            }

            Console.ReadLine();


        }
    }
}
