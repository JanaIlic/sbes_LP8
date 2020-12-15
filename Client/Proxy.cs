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


        public void CreateFile(string filename, string text, string parent)
        {
            try
            {
                try
                {
                    string path = @"~\..\..\..\..\Service\bin\Debug\";
                    factory.CreateFile(path + "encryptedFile_" + filename, text, parent);
                    if (File.Exists(path + "key.txt"))
                    {
                        string key = Key.LoadKey(path + "key.txt");
                        AES.Decrypt(path + "encryptedFile_" + filename + ".txt", path + filename, key);

                        FilesAndFolders.MoveFile(filename, parent);
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

        public void ReadFile(string filename)
        {
            try
            {
                factory.ReadFile(filename);

                string path = @"~\..\..\..\..\Service\bin\Debug\";
                string filetoread = path + filename + "_encryptedToRead.txt";

                if (!File.Exists(filetoread))
                    Console.WriteLine("Ne postoji {0}", filename);
                else
                {
                    string key = path + "key.txt";
                    if (File.Exists(key))
                        AES.Decrypt(filetoread, path + filename + "_decryptedToRead", Key.LoadKey(key));

                    Console.WriteLine("Dobio sam enkriptovan fajl:");
                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(File.ReadAllBytes(filetoread)));
                    Console.WriteLine("Posle dekripcije, imam sadržaj fajla {0} : ", filename);
                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(File.ReadAllBytes(path + filename + "_decryptedToRead.txt")));

                    File.Delete(path + filename + "_decryptedToRead.txt");
                    File.Delete(filetoread);
                    File.Delete(key);
                }
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

        public void Delete(string fileorfolder)
        {
            try
            {
                factory.Delete(fileorfolder);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to delete {0} : {1} ", fileorfolder, e.Message);
            }
        }

        public void Rename(string fileorfolder, string newname)
        {
            try
            {
                factory.Rename(fileorfolder, newname);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to rename {0} : {1} ", fileorfolder, e.Message);
            }
        }

        public void MoveTo(string fileorfolder, string foldername)
        {
            try
            {
                factory.MoveTo(fileorfolder, foldername);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to move {0} to {1} \n : {3} ", fileorfolder, foldername, e.Message);
            }
        }


        public void Dispose()
        {
            if (factory != null)
                factory = null;

            this.Close();
        }


    }
}
