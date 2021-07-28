using System.IO;
using System.Threading.Tasks;

namespace HSServer.Web
{
    public interface IWebHttpResponse
    {
        /*
        Task WriteAsync(string Message);
        void Write(string Message);
        Task WriteStreamAsync(Stream Stream, bool Close = true);
        void WriteStream(Stream Stream, bool Close = true);
        */
        string ContentType { get; set; }
        long? ContentLength { get; set; }

        IWebHttpHeaders Headers { get; }
        IWebHttpResponseCookie Cookies { get; }

        Stream Body { get; set; }
        bool IsHeaderSent { get; }
        bool IsWritable { get; }

        float StatusCode { get; set; }
    }
}
