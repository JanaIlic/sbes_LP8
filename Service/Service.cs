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
        public void CreateFile(string filename)
        {
            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity winID = id as WindowsIdentity;

            try
            {
                using (winID.Impersonate())
                {
                    Console.WriteLine("Impersonifikacija klijenta {0}", WindowsIdentity.GetCurrent().Name);

                    if (Thread.CurrentPrincipal.IsInRole("Change"))
                    {
                           StreamWriter f = File.CreateText("probni fajl");
                           Console.WriteLine("Napravio sam probni fajl.");
                    }
                    else
                        Console.WriteLine("Nemam dozvolu za CreateFile");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Impersonate error: {0}", e.Message);
            }

        } 





    }
}
