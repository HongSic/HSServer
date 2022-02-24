using System.IO;

namespace HSServer.Web
{
    public interface IWebFormFile
    {
        string ContentDisposition { get; }
        string ContentType { get; }
        string FileName { get; }
        string Name { get; }
        long Length { get; }

        Stream GetStream();
    }
}
