using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HSServer.Web
{
    public interface IWebHttpInfo
    {
        IPAddress RemoteIpAddress { get; }
        IPAddress LocalIpAddress { get; }

        int RemotePort { get; }
        int LocalPort { get; }

        X509Certificate2 ClientCertificate { get; }
        Task<X509Certificate2> GetClientCertificateAsync();
    }
}
