using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Utilities.IO.Pem;

namespace CR.HostConsole
{
    public class RsaGenerate
    {
        public static void Generate()
        {
            int keyLength = 2048;
            using RSACryptoServiceProvider rsa = new(keyLength);

            byte[] privateKey = rsa.ExportRSAPrivateKey();

            //ghi dưới dạng PKCS#1 header "-----BEGIN RSA PRIVATE KEY-----"
            var privateKeyPemPkcs = rsa.ExportRSAPrivateKeyPem();

            //ghi dưới dạng pem thường
            //var stringWriter = new StringWriter();
            //var pemWriter = new PemWriter(stringWriter);
            //pemWriter.WriteObject(new PemObject("PRIVATE KEY", privateKey));
            //pemWriter.Writer.Flush();
            //var privateKeyPem = stringWriter.ToString();

            File.WriteAllText("private_key.pem", privateKeyPemPkcs);

            byte[] publicKey = rsa.ExportRSAPublicKey();

            //ghi dưới dạng PKCS#1 header "-----BEGIN RSA PUBLIC KEY-----"
            string publicKeyPemPkcs = rsa.ExportRSAPublicKeyPem();

            //ghi dưới dạng pem thường
            //stringWriter = new StringWriter();
            //pemWriter = new PemWriter(stringWriter);
            //pemWriter.WriteObject(new PemObject("PUBLIC KEY", publicKey));
            //pemWriter.Writer.Flush();
            //var publicKeyPem = stringWriter.ToString();

            //ghi dưới dạng PKCS#8 header "-----BEGIN PUBLIC KEY-----"
            string publicKeyPem = rsa.ExportSubjectPublicKeyInfoPem();
            File.WriteAllText("public_key.pem", publicKeyPemPkcs);

            TryRead();
        }

        public static void TryRead()
        {
            // Đọc khóa bí mật từ file
            string privateKeyPem = File.ReadAllText("private_key.pem");
            var privateKeyObject = new PemReader(new StringReader(privateKeyPem)).ReadPemObject();
            byte[] privateKey = privateKeyObject.Content;

            // Đọc khóa công khai từ file
            string publicKeyPem = File.ReadAllText("public_key.pem");
            var publicKeyObject = new PemReader(new StringReader(publicKeyPem)).ReadPemObject();
            byte[] publicKey = publicKeyObject.Content;

            RSACryptoServiceProvider rsa = new();
            rsa.ImportRSAPublicKey(publicKey, out int _);
            //rsa.ImportSubjectPublicKeyInfo(publicKey, out int _);
            rsa.ImportRSAPrivateKey(privateKey, out int _);

            var key = rsa.ExportParameters(true); //param kèm private key

            string data = "abc";
            string signature = Convert.ToBase64String(
                rsa.SignData(Encoding.UTF8.GetBytes(data), SHA256.Create())
            );

            //kiểm tra chữ ký
            RSACryptoServiceProvider rsa2 = new();
            //rsa2.ImportSubjectPublicKeyInfo(new PemReader(new StringReader(publicKeyPem)).ReadPemObject().Content, out _);
            rsa2.ImportRSAPublicKey(publicKey, out _);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] signatureBytes = Convert.FromBase64String(signature);
            var test = rsa2.VerifyData(dataBytes, SHA256.Create(), signatureBytes);
        }
    }
}
