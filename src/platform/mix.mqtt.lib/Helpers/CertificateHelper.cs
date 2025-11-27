using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Mqtt.Lib.Helpers
{
    public static class CertificateHelper
    {
        public static X509Certificate2 CreateSelfSignedCertificate(string host, string oid)
        {
            var sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName(host);

            using (var rsa = RSA.Create())
            {
                var certRequest = new CertificateRequest($"CN={host}", rsa, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

                certRequest.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));

                certRequest.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection { new(oid) }, false));

                certRequest.CertificateExtensions.Add(sanBuilder.Build());

                using (var certificate = certRequest.CreateSelfSigned(DateTimeOffset.Now.AddMinutes(-10), DateTimeOffset.Now.AddMinutes(10)))
                {
                    var pfxCertificate = new X509Certificate2(
                        certificate.Export(X509ContentType.Pfx),
                        (string)null!,
                        X509KeyStorageFlags.UserKeySet);

                    return pfxCertificate;
                }
            }
        }

        public static X509Certificate2 LoadCertificate(byte[] certificateData, string password)
        {
            if (certificateData == null)
            {
                throw new ArgumentNullException(nameof(certificateData));
            }

            return new X509Certificate2(certificateData, password, X509KeyStorageFlags.DefaultKeySet);
        }
    }
}
