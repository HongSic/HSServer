using HS.Utils;

namespace HSServer.Web.Controller
{
    public interface IController
    {
        void Init(ref LanguageManager Language);
    }
}
