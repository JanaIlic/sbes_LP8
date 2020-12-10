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
using System.IO;

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



        public void CreateFile(string filename, string text)
        {
            try
            {
                try
                {
                    string path = @"~\..\..\..\..\Service\bin\Debug\";
                    factory.CreateFile(path + "encryptedFile_" + filename + ".txt", text);
                    if (File.Exists(path + "key.txt"))
                    {
                          string key = Key.LoadKey(path + "key.txt");
                          AES.Decrypt(path + "encryptedFile_" + filename + ".txt", path + filename + ".txt", key);
                    }

                }
                catch (SecurityAccessDeniedException e)
                {
                    Console.WriteLine("Error 1, security access denied exception: {0}", e.Message);
                }
                catch (FaultException e)
                {
                    Console.WriteLine("Error 2, fault exception: {0}", e.Message);
                }
            }
            catch (SecurityException e)
            {
                Console.WriteLine("Error 3 while tryin to CreateFile, security exception: {0}", e.Message);
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
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to add a new role {0} : {1}", rolename, e.Message);
            }
        }

        public void RemoveSomeRole(string rolename)
        {
            try
            {
                factory.RemoveSomeRole(rolename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to remove a role {0} : {1}", rolename, e.Message);
            }
        }

        public void ReadFile(string filename)
        {
            try
            {
                factory.ReadFile(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to read file {0} : {1}", filename, e.Message);
            }


        }

        public void ShowFolderContent(string foldername)
        {
            try
            {
                factory.ShowFolderContent(foldername);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to show content of {0} folder: {1}", foldername, e.Message);
            }
        }

        public void CreateFolder(string foldername, string parent)
        {
            try
            {
                factory.CreateFolder(foldername, parent);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to create folder {0} : {1}", foldername, e.Message);
            }
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
