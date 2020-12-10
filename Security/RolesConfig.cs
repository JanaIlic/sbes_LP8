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


        public static void AddPermissions(string rolename, string[] permissions)
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

                while (node.MoveNext())
                {
                    if (!node.Key.ToString().Equals(rolename))
                    {
                        writer.AddResource(node.Key.ToString(), node.Value.ToString());
                    }
                    else
                    {
                        List<string> currentPermissions = (node.Value.ToString().Split(',').ToList());
                        
                        foreach (string permToAdd in permissions)
                        {
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

            }


        }


        public static void RemovePermissions(string rolename, string[] permissions)
        {
            var reader = new ResXResourceReader(path);
            var node = reader.GetEnumerator();
            var writer = new ResXResourceWriter(path);

            while (node.MoveNext())
            {
                if (!node.Key.ToString().Equals(rolename))
                {
                    writer.AddResource(node.Key.ToString(), node.Value.ToString());
                }
                else
                {
                    List<string> currentPermissions = (node.Value.ToString().Split(',').ToList());

                    foreach (string permToDelete in permissions)
                    {
                        for (int i = 0; i < currentPermissions.Count(); i++)
                        {
                            if (currentPermissions[i].Equals(permToDelete))
                            {
                                currentPermissions.RemoveAt(i);
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

        }


        public static void AddRole(string rolename)
        {
            var reader = new ResXResourceReader(path);
            var node = reader.GetEnumerator();
            var writer = new ResXResourceWriter(path);

            while (node.MoveNext())
            {
                writer.AddResource(node.Key.ToString(), node.Value.ToString());
            }

            var newNode = new ResXDataNode(rolename, "");
            writer.AddResource(newNode);
            writer.Generate();
            writer.Close();
        }


        public static void RemoveRole(string rolename)
        {
            var reader = new ResXResourceReader(path);
            var node = reader.GetEnumerator();
            var writer = new ResXResourceWriter(path);

            while (node.MoveNext())
            {
                if (!node.Key.ToString().Equals(rolename))
                    writer.AddResource(node.Key.ToString(), node.Value.ToString());
            }

            writer.Generate();
            writer.Close();
        }

    }
}
