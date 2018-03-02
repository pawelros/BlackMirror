namespace BlackMirror.HttpClient
{
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using BlackMirror.Interfaces;

    public class X509StoreCertificateProvider : IClientCertificateProvider
    {
        private readonly StoreName storeName;
        private readonly StoreLocation storeLocation;
        private readonly X509FindType findType;
        private readonly object findValue;
        private readonly bool validOnly;

        public X509StoreCertificateProvider(
            X509FindType findType,
            object findValue,
            bool validOnly = true,
            StoreName storeName = StoreName.My,
            StoreLocation storeLocation = StoreLocation.LocalMachine)
        {
            this.findType = findType;
            this.findValue = findValue;
            this.validOnly = validOnly;
            this.storeName = storeName;
            this.storeLocation = storeLocation;
        }

        public X509Certificate ClientCertificate
        {
            get
            {
                var store = new X509Store(this.storeName, this.storeLocation);
                store.Open(OpenFlags.ReadOnly);
                try
                {
                    return store
                        .Certificates
                        .Find(this.findType, this.findValue, this.validOnly)
                        .OfType<X509Certificate>()
                        .FirstOrDefault();
                }
                finally
                {
                    store.Close();
                }
            }
        }
    }
}