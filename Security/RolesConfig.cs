using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Resources;



namespace Security
{
    public class RolesConfig
    {
        public static string path = @"~\..\..\..\..\Security\RoleConfigFile.resx";
        public static bool GetPermissios(string rolename, out string[] permissions)
        {
            permissions = new string[10];
            string permissionString = string.Empty;
            permissionString = (string)RoleConfigFile.ResourceManager.GetObject(rolename);

            if (permissionString != null)
            {               
                permissions = permissionString.Split(',');
                return true;
            }
            else return false;
        }


        public static void AddPermissions(string rolename, params string[] permissions)
        {
            string permissionString = string.Empty;
            permissionString = (string)RoleConfigFile.ResourceManager.GetObject(rolename);

            if (rolename != null && permissionString == null)
                permissionString = string.Empty;

            if (permissionString != null)
            {
                var reader = new ResXResourceReader(path);
                var node = reader.GetEnumerator();
                var writer = new ResXResourceWriter(path);

                bool roleIn = false;
                bool permsIn = false;

                while (node.MoveNext())
                {
                    if (!node.Key.ToString().Equals(rolename))
                    {
                        writer.AddResource(node.Key.ToString(), node.Value.ToString());
                    }
                    else
                    {
                        roleIn = true;
                        List<string> currentPermissions = (node.Value.ToString().Split(',').ToList());
                        
                        foreach (string permToAdd in permissions)
                        {
                            foreach (string current in currentPermissions)
                                if (permToAdd.Equals(current))
                                    permsIn = true;
                                
                            if(!permsIn)
                                 currentPermissions.Add(permToAdd);
                        } 

                        string value = currentPermissions[0];
                        for (int i = 1; i < currentPermissions.Count(); i++)
                        {
                            if (value.Equals(String.Empty))
                                    value += currentPermissions[i];
                            else 
                                value += "," + currentPermissions[i];
                        }

                        
                        writer.AddResource(node.Key.ToString(), value);
                    }
                }

                writer.Generate();
                writer.Close();

                if (!roleIn)
                    Console.Write("Ne postoji role {0}, pa ne može ni biti", rolename);
                else if (permsIn)
                    Console.Write("{0} već ima bar jednu od od unetih permisija, pa ne može ni biti", rolename);
                else
                    Console.Write("Uspešno");

            }
        }


        public static void RemovePermissions(string rolename, params string[] permissions)
        {
            var reader = new ResXResourceReader(path);
            var node = reader.GetEnumerator();
            var writer = new ResXResourceWriter(path);

            bool roleIn = false;
            bool permIn = false;

            while (node.MoveNext())
            {
                if (!node.Key.ToString().Equals(rolename))
                {
                    writer.AddResource(node.Key.ToString(), node.Value.ToString());
                }
                else
                {
                    roleIn = true;
                    List<string> currentPermissions = (node.Value.ToString().Split(',').ToList());

                    foreach (string permToDelete in permissions)
                    {
                        for (int i = 0; i < currentPermissions.Count(); i++)
                        {
                            if (currentPermissions[i].Equals(permToDelete))
                            {
                                currentPermissions.RemoveAt(i);
                                permIn = true;
                                break;
                            }

                        }
                    }

                    string value = currentPermissions[0];
                    for (int i = 1; i < currentPermissions.Count(); i++)
                    {
                        value += "," + currentPermissions[i];
                    }
                    writer.AddResource(node.Key.ToString(), value);
                }
            }
            writer.Generate();
            writer.Close();

            if (!roleIn)
                Console.Write("Ne postoji role {0}, pa ne može ni biti", rolename);
            else if (!permIn)
                Console.Write("{0} nema bar jednu od unetih permisija, pa ne može ni biti", rolename);
            else
                Console.Write("Uspešno");
        }


        public static void AddRole(string rolename)
        {
            var reader = new ResXResourceReader(path);
            var node = reader.GetEnumerator();
            var writer = new ResXResourceWriter(path);
            bool roleIn = false;

            while (node.MoveNext())
            {
                if (node.Key.ToString().Equals(rolename))
                    roleIn = true;

                 writer.AddResource(node.Key.ToString(), node.Value.ToString());
            }

            if (roleIn)
            {
                Console.Write("Već postoji role {0}, nije", rolename);
            }
            else
            {
                var newNode = new ResXDataNode(rolename, "");
                writer.AddResource(newNode);

                Console.Write("Uspešno dodata role {0},", rolename);
            }
                writer.Generate();
                writer.Close();
            
        }


        public static void RemoveRole(string rolename)
        {
            var reader = new ResXResourceReader(path);
            var node = reader.GetEnumerator();
            var writer = new ResXResourceWriter(path);
            bool roleIn = false ;

            while (node.MoveNext())
            {
                if (!node.Key.ToString().Equals(rolename))
                    writer.AddResource(node.Key.ToString(), node.Value.ToString());
                else
                    roleIn = true;
            }

            if (!roleIn)
                Console.Write("Ne postoji role {0}, nije", rolename);
            else
                Console.Write("Uspesno");

            writer.Generate();
            writer.Close();
        }

    }
}
