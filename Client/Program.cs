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
                string fileName = "unetoImeFajla";
                string unetText = "Originalan tekst, ovo bi trebalo da bude u fajlu, nakon dekriptovanja.";

                //  proxy.CreateFile(fileName, unetText);

                //  proxy.ReadFile("probniZaCitanje.txt");

                proxy.CreateFile("jedanNovi", "samo da proverim ko od korisnika može da ga napravi..");

               /*adminove metode, rade, testirane, kasnije srediti UI i pozive..
                 proxy.AddNewRole("new role");
                 proxy.RemoveSomeRole("new role");
                 proxy.RemoveSomePermissions("Editor", new string[] {"Edit"});
                 proxy.AddNewPermissions("Editor", new string[] {"Edit"}); */

            }

            Console.ReadLine();


        }
    }
}
