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
                      while(true)
                      {
                          Console.WriteLine("Upišite naziv funkcije ili broj u zagradi pored. Pritisnite 'x' za kraj unosa: ");
                          Console.WriteLine("Funkcije kojima Admin upravlja rolama i permisijama korisnika:");
                          Console.WriteLine("AddRole (1), RemoveRole (2), AddPermissions (3), RemovePermissions (4)");
                          Console.WriteLine("Funkcije za upravljanje folderima i fajlovima:");
                          Console.WriteLine("ShowFolderContent (5), ReadFile (6), CreateFolder (7), CreateFile (8), Delete (9), Rename (10), MoveTo (11)");

                          string input = Console.ReadLine();

                          if (input.Equals("x"))
                              break;
                          else
                          {
                              switch (input)
                              {
                                    case "1":
                                    case "AddRole" :
                                          Console.WriteLine("Unesite naziv nove role:");
                                          proxy.AddNewRole(Console.ReadLine());
                                      break;

                                    case "2":
                                    case "RemoveRole":
                                          Console.WriteLine("Unesite naziv role koju želite da uklonite:");
                                          proxy.RemoveSomeRole(Console.ReadLine());
                                      break;

                                    case "3":
                                    case "AddPermissions":
                                          Console.WriteLine("Unesite naziv role kojoj želite da dodelite permisije");
                                          string role = Console.ReadLine();
                                          Console.WriteLine("Unesite permisije. Ako unosite vise, odvojte ih zarezima.");
                                          string[] permissions = Console.ReadLine().Split(',');   

                                          proxy.AddNewPermissions(role, permissions);
                                      break;

                                    case "4":
                                    case "RemovePermissions":
                                          Console.WriteLine("Unesite naziv role čije permisije želite da uklonite.");
                                          role = Console.ReadLine();
                                          Console.WriteLine("Unesite permisije. Ako unosite vise, odvojte ih zarezima.");
                                          permissions = Console.ReadLine().Split(',');

                                          proxy.RemoveSomePermissions(role, permissions);
                                       break;

                                    case "5":
                                    case "ShowFolderContent":
                                          Console.WriteLine("Unesite naziv foldera čiji sadržaj želite da prikažete:");
                                          proxy.ShowFolderContent(Console.ReadLine());
                                       break;

                                    case "6":
                                    case "ReadFile":
                                          Console.WriteLine("Unesite naziv fajla čiji sadržaj želite da pročitate:");
                                          proxy.ReadFile(Console.ReadLine());
                                       break;

                                    case "7":
                                    case "CreateFolder":
                                          Console.WriteLine("Unesite naziv novog foldera:");
                                          string newfolder = Console.ReadLine();
                                          Console.WriteLine("Unesite naziv foldera u kom želite da napravite novi:");
                                          string parent = Console.ReadLine();

                                          proxy.CreateFolder(newfolder, parent);
                                       break;

                                    case "8":
                                    case "CreateFile":
                                          Console.WriteLine("Unesite naziv novog fajla:");
                                          string  file = Console.ReadLine();
                                          Console.WriteLine("Unesite naziv foldera u kom želite da napravite fajl:");
                                          parent = Console.ReadLine();
                                          Console.WriteLine("Unesite sadržaj fajla:");

                                          proxy.CreateFile(file, Console.ReadLine(), parent);
                                       break;

                                    case "9":
                                    case "Delete":
                                          Console.WriteLine("Unesite naziv fajla ili foldera koji želite da obrišete:");
                                          proxy.Delete(Console.ReadLine());
                                       break;

                                    case "10":
                                    case "Rename":
                                          Console.WriteLine("Unesite naziv fajla ili foldera koji  želite da preimenujete:");
                                          file = Console.ReadLine();
                                          Console.WriteLine("Unesite novi naziv:");

                                          proxy.Rename(file, Console.ReadLine());
                                       break;

                                    case "11":
                                    case "MoveTo":
                                          Console.WriteLine("Unesite naziv fajla ili foldera koji želite da pomerite:");
                                          file = Console.ReadLine();
                                          Console.WriteLine("Unesite naziv foldera gde:");

                                          proxy.MoveTo(file, Console.ReadLine());
                                      break;

                                    default:
                                           Console.WriteLine("Greška! Unesite ponovo.");
                                      break;
                              }
                          }

                      } 

              



               

            }

            Console.ReadLine();


        }
    }
}
