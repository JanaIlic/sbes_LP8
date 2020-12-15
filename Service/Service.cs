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
using System.Threading;
using System.Security.Permissions;
using System.IO;
using System.DirectoryServices;

namespace Service
{
    public class Service : IService
    {

        public void AddNewPermissions(string rolename, params string[] permissions)
        {
            if (Thread.CurrentPrincipal.IsInRole("Configure"))
            {
                    RolesConfig.AddPermissions(rolename, permissions);
                     Console.WriteLine(" izvršeno AddPermissions.");
            }
            else
                    Console.WriteLine("Nemam dozvolu za AddPermissions.");

        }

        public void RemoveSomePermissions(string rolename, params string[] permissions)
        {
            if (Thread.CurrentPrincipal.IsInRole("Configure"))
            {
                    RolesConfig.RemovePermissions(rolename, permissions);
                    Console.WriteLine(" izvršeno RemovePermissions.");
            }
            else
                    Console.WriteLine("Nemam dozvolu za RemovePermissios.");
        }


        public void AddNewRole(string rolename)
        {
            if (Thread.CurrentPrincipal.IsInRole("Configure"))
            {
                    RolesConfig.AddRole(rolename);
                    Console.WriteLine(" izvršeno AddRole.");
            }
            else
                    Console.WriteLine("Nemam dozvolu za AddRole.");
        }


        public void RemoveSomeRole(string rolename)
        {
            if (Thread.CurrentPrincipal.IsInRole("Configure"))
            {
                     RolesConfig.RemoveRole(rolename);
                     Console.WriteLine(" izvršeno RemoveRole.");
            }
            else
                     Console.WriteLine("Nemam dozvolu za RemoveRole.");

        }


        [OperationBehavior(Impersonation =ImpersonationOption.Required)]
        public void CreateFile(string filename, string text, string parent)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            WindowsIdentity winID = principal.Identity as WindowsIdentity;

            try
            {
                 using (winID.Impersonate())
                 {
                      Console.WriteLine("Impersonifikacija klijenta {0}", WindowsIdentity.GetCurrent().Name);

                        if (!Thread.CurrentPrincipal.IsInRole("Change"))
                        {
                                string username = Parser.Parse(Thread.CurrentPrincipal.Identity.Name);

                                try
                                {
                                        Audit.AuthorizationFailed(principal.Identity.Name,
                                        OperationContext.Current.IncomingMessageHeaders.Action,
                                           "Nemam dozvolu za CreateFile.");
                                }
                                catch (ArgumentException e)
                                {
                                         Console.WriteLine(e.Message);
                                }
                                throw new FaultException(username + " je pokušao da pozove CreateFile, za šta mu treba dozvola.");
                        }
                        else
                        {
                                 string key = Key.GenerateKey();
                                 Key.StoreKey(key, "key.txt");
                                 AES.Encrypt(ASCIIEncoding.ASCII.GetBytes(text), filename, key);

                                try
                                {
                                        Audit.AuthorizationSuccess(principal.Identity.Name,
                                            OperationContext.Current.IncomingMessageHeaders.Action);
                                }
                                catch (ArgumentException e)
                                {
                                        Console.WriteLine(e.Message);
                                }
                        }
                 }
            }
            catch (Exception e)
            {
                   Console.WriteLine("Impersonate error: {0}", e.Message);
            }           
        }


        public void ReadFile(string filename)
        {
            if (FilesAndFolders.GetFilePaths(filename).Count() == 0 )
                Console.WriteLine("{0} ne postoji.", filename);
            else
            {
                string filetoread = FilesAndFolders.GetShortestPath(FilesAndFolders.GetFilePaths(filename));

                string key = Key.GenerateKey();
                Key.StoreKey(key, "key.txt");
                AES.Encrypt(ASCIIEncoding.ASCII.GetBytes(File.ReadAllText(filetoread)), filename + "_encryptedToRead", key);
            }        
        }


        [OperationBehavior(Impersonation = ImpersonationOption.Required)]
        public void CreateFolder(string foldername, string parent)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            WindowsIdentity winID = principal.Identity as WindowsIdentity;

            try
            {
                 using (winID.Impersonate())
                 {
                        Console.WriteLine("Impersonifikacija klijenta {0}", WindowsIdentity.GetCurrent().Name);

                        if (!Thread.CurrentPrincipal.IsInRole("Change"))
                        {
                                 string username = Parser.Parse(Thread.CurrentPrincipal.Identity.Name);

                                 try
                                 {
                                        Audit.AuthorizationFailed(principal.Identity.Name, 
                                            OperationContext.Current.IncomingMessageHeaders.Action,
                                            "Nemam dozvolu za CreateFolder.");
                                 }
                                 catch (ArgumentException e)
                                 {
                                        Console.WriteLine(e.Message);
                                 }
                             throw new FaultException(username + " je pokušao da pozove CreateFolder, za šta mu treba dozvola.");
                        }
                        else
                        {
                                 FilesAndFolders.CreateFolder(foldername, parent);
                                 try
                                 {
                                         Audit.AuthorizationSuccess(principal.Identity.Name,
                                         OperationContext.Current.IncomingMessageHeaders.Action);
                                 }
                                 catch (ArgumentException e)
                                 {
                                         Console.WriteLine(e.Message);
                                 }
                        }
                 }
            }
            catch (Exception e)
            {
                  Console.WriteLine("Impersonate error: {0}", e.Message);
            }           
        }


        public void ShowFolderContent(string foldername)
        {
             FilesAndFolders.ShowFolderContent(foldername);
        }


        [OperationBehavior(Impersonation = ImpersonationOption.Required)]
        public void Delete(string fileOrFolder)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            WindowsIdentity winID = principal.Identity as WindowsIdentity;

              try
              {
                    using (winID.Impersonate())
                    {
                             Console.WriteLine("Impersonifikacija klijenta {0}", WindowsIdentity.GetCurrent().Name);

                            if (!Thread.CurrentPrincipal.IsInRole("Delete"))
                            {
                                 string username = Parser.Parse(Thread.CurrentPrincipal.Identity.Name);

                                 try
                                 {
                                         Audit.AuthorizationFailed(principal.Identity.Name,
                                         OperationContext.Current.IncomingMessageHeaders.Action,
                                            "Nemam dozvolu za Delete.");
                                 }
                                 catch (ArgumentException e)
                                 {
                                        Console.WriteLine(e.Message);
                                 }
                                 throw new FaultException(username + " je pokušao da pozove Delete, za šta mu treba dozvola.");
                            }
                            else
                            {
                                 FilesAndFolders.Delete(fileOrFolder);
                                 try
                                 {
                                           Audit.AuthorizationSuccess(principal.Identity.Name,
                                            OperationContext.Current.IncomingMessageHeaders.Action);
                                 }
                                 catch (ArgumentException e)
                                 {
                                        Console.WriteLine(e.Message);
                                 }                           
                            }    
                    }
              }
              catch (Exception e)
              {
                   Console.WriteLine("Impersonate error: {0}", e.Message);
              }             
        }


        [OperationBehavior(Impersonation = ImpersonationOption.Required)]
        public void Rename(string fileorfolder, string newname)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            WindowsIdentity winID = principal.Identity as WindowsIdentity;

            try
            {
                 using (winID.Impersonate())
                 {
                        Console.WriteLine("Impersonifikacija klijenta {0}", WindowsIdentity.GetCurrent().Name);

                        if (!Thread.CurrentPrincipal.IsInRole("Change"))
                        {
                            string username = Parser.Parse(Thread.CurrentPrincipal.Identity.Name);

                                try
                                {
                                        Audit.AuthorizationFailed(principal.Identity.Name,
                                             OperationContext.Current.IncomingMessageHeaders.Action,
                                            "Nemam dozvolu za Rename.");
                                }
                                catch (ArgumentException e)
                                {
                                        Console.WriteLine(e.Message);
                                }
                                throw new FaultException(username + " je pokušao da pozove Rename, za šta mu treba dozvola.");
                        }
                        else
                        {
                                FilesAndFolders.Rename(fileorfolder, newname);
                    
                                try
                                {
                                        Audit.AuthorizationSuccess(principal.Identity.Name,
                                        OperationContext.Current.IncomingMessageHeaders.Action);
                                }
                                catch (ArgumentException e)
                                {
                                        Console.WriteLine(e.Message);
                                }
                        }
                        
                 }
            }
            catch (Exception e)
            {
                  Console.WriteLine("Impersonate error: {0}", e.Message);
            }            
        }


        [OperationBehavior(Impersonation = ImpersonationOption.Required)]
        public void MoveTo(string fileorfolder, string foldername)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            WindowsIdentity winID = principal.Identity as WindowsIdentity;

            try
            {
                 using (winID.Impersonate())
                 {
                        Console.WriteLine("Impersonifikacija klijenta {0}", WindowsIdentity.GetCurrent().Name);

                        if (!Thread.CurrentPrincipal.IsInRole("Change"))
                        {
                            string username = Parser.Parse(Thread.CurrentPrincipal.Identity.Name);

                             try
                             {
                                    Audit.AuthorizationFailed(principal.Identity.Name,
                                                 OperationContext.Current.IncomingMessageHeaders.Action,
                                                 "Nemam dozvolu za MoveTo.");
                             }
                             catch (ArgumentException e)
                             {
                                    Console.WriteLine(e.Message);
                             }
                             throw new FaultException(username + " je pokušao da pozove MoveTo, za šta mu treba dozvola.");
                        }
                        else
                        {
                             FilesAndFolders.MoveTo(fileorfolder, foldername);

                             try
                             {
                                    Audit.AuthorizationSuccess(principal.Identity.Name,
                                        OperationContext.Current.IncomingMessageHeaders.Action);
                             }
                             catch (ArgumentException e)
                             {
                                    Console.WriteLine(e.Message);
                             }
                        }                            
                 }
            }
            catch (Exception e)
            {
                  Console.WriteLine("Impersonate error: {0}", e.Message);
            }           
        }





    }
}
