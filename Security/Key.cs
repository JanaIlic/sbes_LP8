using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace Security
{
    public class Key
    {
        public static string GenerateKey()
        {
            SymmetricAlgorithm algorithm = AesCryptoServiceProvider.Create();

            if (algorithm == null)
                return String.Empty;
            else
                return ASCIIEncoding.ASCII.GetString(algorithm.Key);
        }


        public static void StoreKey(string key, string keyFile)
        {
            FileStream fs = new FileStream(keyFile, FileMode.Create, FileAccess.Write);
            byte[] buffer = Encoding.ASCII.GetBytes(key);

            try
            {
                fs.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in StoreKey: {0}", e.Message);
            }
            finally
            {
                fs.Close();
            }

        }


        public static string LoadKey(string keyFile)
        {
            FileStream fs = new FileStream(keyFile, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[(int)fs.Length];

            try
            {
                fs.Read(buffer, 0, (int)fs.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in LoadKey: {0}", e.Message);
            }
            finally
            {
                fs.Close();
            }

            return ASCIIEncoding.ASCII.GetString(buffer);
        }




    }
}
