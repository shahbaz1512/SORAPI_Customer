using Serilog;
using SORAPI.Classes;
using SORAPI.Interface;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace SORAPI.DataValidator
{
    public class VaildateRequest : Datavalidator
    {
        Response _response = new Response();
        string certPath = "E:\\Documents\\MaximusNew.pfx";
        string certPassword = "P@ss1234";

        public async Task<bool> DataValidators(enumTransactionType _TransType, Request request)
        {
            bool status = false;
            try
            {
                switch (_TransType)
                {
                    case enumTransactionType.CustomerOnboard:
                        if (string.IsNullOrEmpty(request.State))
                            return false;
                        else
                            return true;
                        break;
                }
                return status;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during datavalidator processing.");
                return false;
            }
        }

        public async Task<string> SSLDecrypt(string encryptedMessage)
        {
            try
            {
                X509Certificate2 cert = new X509Certificate2(certPath, certPassword);
                // Get the public key
                using (RSA privateKey = cert.GetRSAPrivateKey())
                {
                    // Decrypt the message using the private key
                    string decryptedMessage = EncryptDecrypt.Decrypt(encryptedMessage, privateKey);
                    Console.WriteLine("Decrypted Message: \n" + decryptedMessage);
                }
                return encryptedMessage;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during SSLDecrypt.");
                return encryptedMessage;
            }
        }

        public async Task<string> SSLEncrypt(string PlainMessage)
        {
            try
            {
                X509Certificate2 cert = new X509Certificate2(certPath, certPassword);
                // Get the public key
                using (RSA publicKey = cert.GetRSAPublicKey())
                {
                    // Encrypt the message using the public key
                    string encryptedMessage = EncryptDecrypt.Encrypt(PlainMessage, publicKey);
                    Console.WriteLine("Encrypted Message: \n" + encryptedMessage);
                }
                return PlainMessage;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during SSLencrypt.");
                return PlainMessage;
            }
        }

    }
}
