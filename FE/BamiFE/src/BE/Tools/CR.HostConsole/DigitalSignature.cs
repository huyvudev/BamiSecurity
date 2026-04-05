using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;

namespace CR.HostConsole
{
    public static class DigitalSignature
    {
        public static void PfxGenerate()
        {
            try
            {
                string pfxFilePath = "private-key.pfx"; // Tên tệp PFX đầu ra
                string pfxPassword = "your_password"; // Mật khẩu cho tệp PFX
                int keySize = 2048; // Độ dài của khóa (bit)

                // Tạo cặp khóa riêng tư và khóa công khai
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize))
                {
                    // Tạo một yêu cầu chứng thư X.509 (Certificate Request)
                    CertificateRequest request = new CertificateRequest(
                        "cn=MyCertificate",
                        rsa,
                        HashAlgorithmName.SHA256,
                        RSASignaturePadding.Pkcs1
                    );

                    // Tạo một chứng thư tự ký (self-signed certificate)
                    X509Certificate2 certificate = request.CreateSelfSigned(
                        DateTimeOffset.Now,
                        DateTimeOffset.Now.AddYears(1)
                    );

                    // Lưu cặp khóa riêng tư và chứng thư vào tệp PFX
                    File.WriteAllBytes(
                        pfxFilePath,
                        certificate.Export(X509ContentType.Pkcs12, pfxPassword)
                    );
                }

                Console.WriteLine("Tạo tệp PFX thành công.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi: " + e.Message);
            }
        }

        public static void TestSign()
        {
            //var pk12 = new iText.Bouncycastle.Cert.k.Pkcs12Store();
            //string? alias = null;
            //foreach (var a in pk12.Aliases)
            //{
            //    alias = ((string)a);
            //    if (pk12.IsKeyEntry(alias))
            //        break;
            //}

            //ICipherParameters pk = pk12.GetKey(alias).Key;
            //X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            //iText.Commons.Bouncycastle.Cert.IX509Certificate[] chain = new iText.Bouncycastle.X509.X509CertificateBC[ce.Length];
            //for (int k = 0; k < ce.Length; ++k)
            //{
            //    chain[k] = ce[k].Certificate;
            //}

            //Sign("", "", chain, pk, DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS, "Test 1", "Ghent");
        }

        [Obsolete]
        public static void Sign(
            String src,
            String dest,
            iText.Commons.Bouncycastle.Cert.IX509Certificate[] chain,
            iText.Commons.Bouncycastle.Crypto.IPrivateKey pk,
            String digestAlgorithm,
            PdfSigner.CryptoStandard subfilter,
            String reason,
            String location
        )
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(
                reader,
                new FileStream(dest, FileMode.Create),
                new StampingProperties()
            );

            // Create the signature appearance
            Rectangle rect = new Rectangle(36, 648, 200, 100);
            PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
            appearance
                .SetReason(reason)
                .SetLocation(location)
                // Specify if the appearance before field is signed will be used
                // as a background for the signed field. The "false" value is the default value.
                .SetReuseAppearance(false)
                .SetPageRect(rect)
                .SetPageNumber(1);
            signer.SetFieldName("sig");

            IExternalSignature pks = new PrivateKeySignature(pk, digestAlgorithm);

            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, chain, null, null, null, 0, subfilter);
        }
    }
}
