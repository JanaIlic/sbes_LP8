using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.DirectoryServices;
using System.Windows.Forms;

namespace Contracts
{
    public class FilesAndFolders
    {

       public static string path = @"~\..\..\..\..\Service\bin\Debug\";

        public  static void CreateFolder(string foldername, string parent)
        {

            if (parent.Equals(String.Empty))
            {
                if (Directory.Exists(path + foldername))
                    Console.WriteLine("Već postoji folder {0}.", foldername);
                else
                    Directory.CreateDirectory(path + foldername);
            }
            else
            {
                if (!Directory.Exists(path + parent))
                    Console.WriteLine("Ne postoji folder {0}!", parent);
                else
                {
                    if (Directory.Exists(path + parent + @"\" + foldername))
                        Console.WriteLine("Već postoji folder {0} u folderu {1}.", foldername, parent);
                    else
                      Directory.CreateDirectory(path + parent + @"\" + foldername);
                }

               
            }




        }



        public static string GetThePath(string foldername)
        {
                List<string> subfolders = new List<string>();
                List<string> subsubfolders = new List<string>();

                string retPath = string.Empty;
                string middle = String.Empty;

            if (Directory.Exists(path + foldername))
                    retPath = path + foldername;
            else
            {
                foreach (string subfolder in Directory.EnumerateDirectories(path))
                    {
                    if (Directory.Exists(subfolder + @"\" + foldername))
                    {
                            retPath = subfolder + @"\" + foldername;
                            break;
                    }
                    else
                    {
                         foreach (string subsubfolder in Directory.EnumerateDirectories(subfolder))
                         {
                                if (Directory.Exists(subsubfolder + @"\" + foldername))
                                {
                                    retPath = subsubfolder + @"\" + foldername;
                                }

                         }
                    }
                }

            }

            return retPath;
        }
        

        public static void ShowFolderContent(string foldername)
        {
              Console.WriteLine("{0} folder sadrži podfoldere: ", foldername);
              foreach (string folder in Directory.EnumerateDirectories(GetThePath(foldername)))
              {
                        List<string> words = folder.Split('\\').ToList();
                        Console.WriteLine(words.Last());
              }

              Console.WriteLine("...i fajlove:");
              foreach (string file in Directory.EnumerateFiles(GetThePath(foldername)))
              {
                     List<string> words = file.Split('\\').ToList();
                     Console.WriteLine(words.Last());
              }
                                
        }




    }
}
