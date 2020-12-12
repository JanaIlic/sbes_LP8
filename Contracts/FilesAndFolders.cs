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

        public static List<string> GetFolderPaths(string foldername)
        {
            List<string> retPaths = new List<string>();

            foreach (string p in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
                if (p.Contains(foldername))
                    retPaths.Add(p);

            return retPaths;
        }

        public static List<string> GetFilePaths(string filename)
        {
            List<string> retPaths = new List<string>();

            foreach (string p in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
                if (p.Contains(filename))
                    retPaths.Add(p);

            return retPaths;
        }




        public static void ShowFolderContent(string foldername)
        {
            Console.WriteLine("{0} folder sadrži podfoldere: ", foldername);
            foreach (string folder in Directory.EnumerateDirectories(GetFolderPaths(foldername).First()))
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
                if (fp.Split('\\').ToList().Last().Equals(fileorfolder))
                {
                    ret = "file";
                    break;
                }
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
                    File.Delete(fp);
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
            foreach (string p in GetFilePaths(filename))
            {
                string newP = p.Replace(filename, newname);
                File.WriteAllText(newP + ".txt", File.ReadAllText(p));
                File.Delete(p);
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
            string newP = GetFolderPaths(foldername).First();
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
            foreach (string p in GetFolderPaths(moving))
                Directory.CreateDirectory(MovingPath(p, foldername, moving));

                   
            string[] oldFiles = Directory.GetFiles(GetFolderPaths(moving).First(), "*", SearchOption.AllDirectories);
            foreach (string p in oldFiles)
            {
                string content = File.ReadAllText(p);
                File.WriteAllText(MovingPath(p, foldername, moving), content);
            }

              Directory.Delete(GetFolderPaths(moving).First(), true);

        }

        public static void MoveFile(string moving, string foldername)
        {
            string old = GetFilePaths(moving).First();
            File.WriteAllText(GetFolderPaths(foldername).First() + '\\' + moving, File.ReadAllText(old));
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
