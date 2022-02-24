using System.Collections.Generic;

namespace HSServer.Web
{
    public interface IWebHttpResponseForm : IWebCollectionReadOnly<string>
    {
        IReadOnlyCollection<IWebFormFile> Files { get; }
    }
}
