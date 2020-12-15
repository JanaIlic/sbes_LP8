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

        public static void CreateFolder(string foldername, string parent)
        {
                if (!Directory.Exists(GetFolderPaths(parent).FirstOrDefault()))
                    Console.WriteLine("Ne postoji folder {0}!", parent);
                else
                {
                     if (Directory.Exists(GetFolderPaths(parent).FirstOrDefault() + "\\" + foldername))
                         Console.WriteLine("Već postoji folder {0} u folderu {1}.", foldername, parent);
                     else
                          Directory.CreateDirectory(GetFolderPaths(parent).FirstOrDefault() + "\\" + foldername + "\\" );
                }           
        }

        public static List<string> GetFolderPaths(string foldername)
        {
                List<string> retPaths = new List<string>();

                foreach (string p in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
                {
                    foreach(string folder in p.Split('\\'))
                     if (folder.Equals(foldername))
                          retPaths.Add(p);
                }

            return retPaths;
        }

        public static List<string> GetFilePaths(string filename)
        {
                List<string> retPaths = new List<string>();

                foreach (string p in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
                {
                      string last = p.Split('\\').ToList().Last();
                     if (last.Equals(filename) || last.Equals(filename + ".txt"))
                              retPaths.Add(p);
                }

             return retPaths;
        }


        public static string GetShortestPath(List<string> paths)
        {
            string retPath = paths[0];

                foreach (string p in paths)
                    if (p.Length < retPath.Length)
                         retPath = p;

                 return retPath;
        }

        public static void ShowFolderContent(string foldername)
        {
                Console.WriteLine("{0} folder sadrži podfoldere: ", foldername);
                foreach (string folder in Directory.EnumerateDirectories(GetFolderPaths(foldername).FirstOrDefault()))
                {
                    List<string> words = folder.Split('\\').ToList();
                     Console.WriteLine(words.Last());

                        if (Directory.EnumerateFiles(folder).Count() > 0)
                        {
                            Console.WriteLine(" sa fajlovima: ");

                            foreach (string file in Directory.EnumerateFiles(folder))
                            {
                                 List<string> parts = file.Split('\\').ToList();
                                 Console.WriteLine("\t {0}", parts.Last());
                            }
                        }
                }

                if (Directory.EnumerateFiles(GetFolderPaths(foldername).First()).Count() > 0)
                {
                        Console.WriteLine("...i fajlove:");
                        foreach (string file in Directory.EnumerateFiles(GetFolderPaths(foldername).First()))
                        {
                            List<string> parts = file.Split('\\').ToList();
                            Console.Write("\t {0}", parts.Last());
                        }
                }

        }

        public static string FileOrFolder(string fileorfolder)
        {
                string ret = String.Empty;
                foreach (string fp in GetFilePaths(fileorfolder))
                {
                     if (fp.Split('\\').ToList().Last().Equals(fileorfolder + ".txt"))
                     {
                            ret = "file";
                            break;
                     }
                     Console.WriteLine(fp);
                }

                if (ret.Equals(String.Empty))
                {
                     foreach (string fp in GetFolderPaths(fileorfolder))
                     {
                         if (Directory.Exists(fp))
                         {
                              ret = "folder";
                              break;
                         }
                     }
                }

            return ret;
        }

        public static void DeleteFile(string filename)
        {
                foreach (string fp in GetFilePaths(filename))
                {
                    if (!File.Exists(fp))
                        Console.WriteLine("Ne postoji fajl {0}.", filename);
                    else
                    {
                        File.Delete(fp);

                        if (File.Exists(path + "encryptedFile_" + filename + ".txt"))
                        {
                            File.Delete(path + "encryptedFile_" + filename + ".txt");
                       
                        }                            
                    }
                }
        }

        public static void DeleteFolder(string foldername)
        {
                foreach (string fp in GetFolderPaths(foldername))
                {
                     if (!Directory.Exists(fp))
                         Console.WriteLine("Ne postoji folder {1}.", foldername);
                     else
                         Directory.Delete(fp, true);
                }
        }


        public static void Delete(string fileorfolder)
        {
                if (FileOrFolder(fileorfolder).Equals("file"))
                      DeleteFile(fileorfolder);                  
                else if (FileOrFolder(fileorfolder).Equals("folder"))
                     DeleteFolder(fileorfolder);
                else
                     Console.WriteLine("Ne postoji ni fajl ni folder pod imenom {0}.", fileorfolder);
        }


        public static string ChangedPath(string fullpath, string old, string newname)
        {
                string ret = String.Empty;

                string[] folders = fullpath.Split('\\');
                for (int i = 0; i < folders.Length; i++)
                {
                    if (folders[i].Equals(old))
                         folders[i] = newname;


                    if (i == 0)
                          ret += folders[i];
                    else
                      ret += "\\" + folders[i];
                }
            return ret;
        }

        public static void RenameFolder(string foldername, string newname)
        {
                 string[] oldFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

                foreach (string of in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
                {
                     if (of.Contains(foldername))
                          Directory.CreateDirectory(ChangedPath(of, foldername, newname));
                }

                foreach (string of in oldFiles)
                {
                    if (of.Contains(foldername))
                    {
                          string content = File.ReadAllText(of);
                          File.WriteAllText(ChangedPath(of, foldername, newname), content);
                    }
                }

             Directory.Delete(GetFolderPaths(foldername).First(), true);
        }


        public static void RenameFile(string filename, string newname)
        {
                string p = GetShortestPath(GetFilePaths(filename));
                string newP = p.Replace(filename, newname);
                File.WriteAllText(newP, File.ReadAllText(p));
                File.Delete(p);

                string enc = GetFilePaths("encryptedFile_" + filename).First();
                if (File.Exists(enc))
                {
                     string newEnc = enc.Replace(filename, newname);
                     File.WriteAllText(newEnc, File.ReadAllText(enc));
                     File.Delete(enc);
                }
        }


        public static void Rename(string fileorfolder, string newname)
        {
                if (FileOrFolder(fileorfolder).Equals("file"))
                     RenameFile(fileorfolder, newname);
                else if (FileOrFolder(fileorfolder).Equals("folder"))
                    RenameFolder(fileorfolder, newname);
                else
                    Console.WriteLine("Ne postoji ni fajl ni folder pod imenom {0}.", fileorfolder);
        }


        public static string MovingPath(string old, string foldername, string moving)
        {
                string newP = GetShortestPath(GetFolderPaths(foldername)); 
                string[] folders = old.Split('\\');
                int gotIt = 0;

                for (int i = 0; i < folders.Length; i++)
                if (folders[i].Equals(moving))
                    gotIt = i;

                for (int i = gotIt; i < folders.Length; i++)
                    newP += "\\" + folders[i];

            return newP;
        }

        public static void MoveFolder(string moving, string foldername)
        {
                 string[] oldFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                 string old =  GetShortestPath(GetFolderPaths(moving));

                foreach (string of in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
                {
                     if (of.Contains(moving))
                         Directory.CreateDirectory(MovingPath(of, foldername, moving));
                }

                foreach (string of in oldFiles)
                {
                    if (of.Contains(moving))
                    {
                        string content = File.ReadAllText(of);
                        File.WriteAllText(MovingPath(of, foldername, moving), content);
                    }
                }

             Directory.Delete(old , true);
        }

        public static void MoveFile(string moving, string foldername)
        {
                string old = GetShortestPath(GetFilePaths(moving));
                File.WriteAllText(GetShortestPath(GetFolderPaths(foldername)) + '\\' + moving + ".txt", File.ReadAllText(old));
                File.Delete(old);
        }

        public static void MoveTo(string fileorfolder, string foldername)
        {
            if (FileOrFolder(fileorfolder).Equals("file"))
                MoveFile(fileorfolder, foldername);
            else if (FileOrFolder(fileorfolder).Equals("folder"))
                MoveFolder(fileorfolder, foldername);
            else
                Console.WriteLine("Ne postoji ni fajl ni folder pod imenom {0}.", fileorfolder);
        }



    }
}
