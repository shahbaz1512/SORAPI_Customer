using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace SORAPI.Classes
{
    public class EncryptDecrypt
    {
        private static readonly string key = "YourEncryptionKey123";
        private static readonly string iv = "YourIVKey12345678";
        public static string EncryptString(string plainText)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(key);
                    aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                            return Convert.ToBase64String(msEncrypt.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing.");
                return null;
            }
        }

        public static string DecryptString(string cipherText)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(key);
                    aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex )
            {

                Log.Error(ex, "An error occurred during processing.");
                return null;
            }
        }

        public static string Encrypt(string text, RSA publicKey)
        {
            try
            {
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(text);
                byte[] encryptedData = publicKey.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during processing Encrypt.");
                return null;
            }
        }

        public static string Decrypt(string encryptedText, RSA privateKey)
        {
            try
            {
                byte[] dataToDecrypt = Convert.FromBase64String(encryptedText);
                byte[] decryptedData = privateKey.Decrypt(dataToDecrypt, RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(decryptedData);
            }
            catch (Exception ex )
            {
                Log.Error(ex, "An error occurred during processing Decrypt.");
                return null; ;
            }
        }
    }
}
