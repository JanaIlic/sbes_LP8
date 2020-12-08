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
    public class AES
    {

        public static void  Encrypt(byte[] bytesToEncrypt, string toReturn, string key)
        {
            byte[] encrypted = null;

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(key),
                Mode = CipherMode.CBC, Padding =PaddingMode.Zeros
            };

            aes.GenerateIV();
            ICryptoTransform encrypt = aes.CreateEncryptor();

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    cs.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                    cs.FlushFinalBlock();
                    encrypted = aes.IV.Concat(ms.ToArray()).ToArray();
                }
            }

            BinaryWriter writer = new BinaryWriter(File.OpenWrite(toReturn));
            writer.Write(encrypted);
            writer.Flush();
            writer.Close();

        }



        public static void Decrypt(string toDecrypt, string toReturn, string key)
        {
            byte[] bytesToDecrypt = File.ReadAllBytes(toDecrypt);
            byte[] decrypted = null;

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                Key = ASCIIEncoding.ASCII.GetBytes(key),
                Mode = CipherMode.CBC, Padding = PaddingMode.Zeros
            };

            aes.IV = bytesToDecrypt.Take(aes.BlockSize /8).ToArray();
            ICryptoTransform decrypt = aes.CreateDecryptor();

            using (MemoryStream ms = new MemoryStream(bytesToDecrypt.Skip(aes.BlockSize / 8).ToArray()))
            {
                using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
                {
                    decrypted = new byte[bytesToDecrypt.Length - aes.BlockSize/8];
                    cs.Read(decrypted, 0, decrypted.Length);
                }
            }

            BinaryWriter writer = new BinaryWriter(File.OpenWrite(toReturn));
            writer.Write(decrypted);
            writer.Flush();
            writer.Close();

        }




    }
}
