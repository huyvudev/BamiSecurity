using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.OpenSsl;

namespace CR.HostConsole
{
    public static class CertificateProtectedKey
    {
        public static void GenerateCertificate(string privatePath)
        {
            string privateKeyPem = File.ReadAllText(privatePath);
            // Đọc khóa bí mật từ file
            var privateKeyObject = new PemReader(new StringReader(privateKeyPem)).ReadPemObject();
            if (privateKeyObject != null)
            {
                byte[] privateKeyByte = privateKeyObject.Content;
                RSACryptoServiceProvider rsa = new(2048);
                rsa.ImportRSAPrivateKey(privateKeyByte, out int _);
                CertificateRequest certificateRequest =
                    new(
                        new X500DistinguishedName("CN=YourCertificate"),
                        rsa,
                        HashAlgorithmName.SHA256,
                        RSASignaturePadding.Pkcs1
                    );
                X509Certificate2 certificate = certificateRequest.CreateSelfSigned(
                    DateTimeOffset.Now,
                    DateTimeOffset.Now.AddSeconds(2)
                );
                string directory = Directory.GetCurrentDirectory() + "/dataProtection/";

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllBytes(
                    directory + "protect_key_certificate.pfx",
                    certificate.Export(X509ContentType.Pkcs12)
                );
            }
        }

        public static void ConvertX509ToPkcs12(
            string crtFilePath,
            string pfxFilePath,
            string pfxPassword = ""
        )
        {
            try
            {
                // Load the CRT file
                X509Certificate2 certificate = new X509Certificate2(crtFilePath);

                // Export to PFX format with a password
                byte[] pfxData = certificate.Export(X509ContentType.Pkcs12, pfxPassword);

                // Save the PFX data to a file
                File.WriteAllBytes(pfxFilePath, pfxData);

                Console.WriteLine("Conversion successful. PFX file saved at: " + pfxFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
