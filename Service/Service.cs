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
            if (Thread.CurrentPrincipal.IsInRole("Administrate"))
            {
                Console.WriteLine("Uspešno izvršeno AddPermissions.");
                RolesConfig.AddPermissions(rolename, permissions);
            }
            else
                Console.WriteLine("Nemam dozvolu za AddPermissions.");

        }

        public void RemoveSomePermissions(string rolename, params string[] permissions)
        {
            if (Thread.CurrentPrincipal.IsInRole("Administrate"))
            {
                Console.WriteLine("Uspešno izvršeno RemovePermissions.");
                RolesConfig.RemovePermissions(rolename, permissions);
            }
            else
                Console.WriteLine("Nemam dozvolu za RemovePermissios.");
        }

        public void AddNewRole(string rolename)
        {
            if (Thread.CurrentPrincipal.IsInRole("Administrate"))
            {
                Console.WriteLine("Uspešno izvršeno AddRole.");
                RolesConfig.AddRole(rolename);
            }
            else
                Console.WriteLine("Nemam dozvolu za AddRole.");
        }

        public void RemoveSomeRole(string rolename)
        {
            if (Thread.CurrentPrincipal.IsInRole("Administrate"))
            {
                Console.WriteLine("Uspešno izvršeno RemoveRole.");
                RolesConfig.RemoveRole(rolename);
            }
            else
                Console.WriteLine("Nemam dozvolu za RemoveRole.");

        }


        [OperationBehavior(Impersonation =ImpersonationOption.Required)]
        public void CreateFile(string filename, string text)
        {
            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity winID = id as WindowsIdentity;


            if (!Thread.CurrentPrincipal.IsInRole("Change"))
                Console.WriteLine("Nemam dozvolu za CreateFile");
            else
            {
                try
                {
                    using (winID.Impersonate())
                    {
                        Console.WriteLine("Impersonifikacija klijenta {0}", WindowsIdentity.GetCurrent().Name);

                        string key = Key.GenerateKey();
                        Key.StoreKey(key, "key.txt");
                        AES.Encrypt(ASCIIEncoding.ASCII.GetBytes(text), filename, key);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Impersonate error: {0}", e.Message);
                }
            }

        }

        public void ReadFile(string filename)
        {

            AES.Decrypt("encryptedFile_"+ filename + ".txt", filename + "_decryptedToRead.txt", Key.LoadKey("key.txt"));

            Console.WriteLine("Content of file {0} : ", filename);
            Console.WriteLine(ASCIIEncoding.ASCII.GetString(File.ReadAllBytes(filename + "_decryptedToRead.txt")));
            File.Delete(filename + "_decryptedToRead.txt");
        }


        [OperationBehavior(Impersonation = ImpersonationOption.Required)]
        public void CreateFolder(string foldername, string parent)
        {

            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity winID = id as WindowsIdentity;


            if (!Thread.CurrentPrincipal.IsInRole("Change"))
                Console.WriteLine("Nemam dozvolu za CreateFolder");
            else
            {
                try
                {
                    using (winID.Impersonate())
                    {
                        Console.WriteLine("Impersonifikacija klijenta {0}", WindowsIdentity.GetCurrent().Name);
                        FilesAndFolders.CreateFolder(foldername, parent);
   
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Impersonate error: {0}", e.Message);
                }
            }
        }

        public void ShowFolderContent(string foldername)
        {
            FilesAndFolders.ShowFolderContent(foldername);
        }

        public void Delete(string fileOrFolder)
        {
            throw new NotImplementedException();
        }

        public void Rename(string fileOrFOlder)
        {
            throw new NotImplementedException();
        }

        public void MoveTo(string filename, string foldername)
        {
            throw new NotImplementedException();
        }
    }
}
