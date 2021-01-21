using HS.Utils;
using HSServer.Web.Module;
using System.Threading.Tasks;

namespace HSServer.Web.MiddleWare
{
    public class MiddleWareData : ModuleData
    {
        public string Path { get; set; }

        public LanguageManager STR_LANG { get; set; }
        public WebHttpContextRaw ContextRaw { get; }

        public WebHttpContext Context { get; set; }
        public object ExtraData { get; set; }

        internal MiddleWareData(string Path, LanguageManager STR_LANG, WebHttpContextRaw ContextRaw)
        {
            this.Path = Path;
            this.STR_LANG = STR_LANG;
            this.ContextRaw = ContextRaw;

            Context = new WebHttpContext(ContextRaw);
        }


        /// <summary>
        /// IsClose 가 true 면 웹 모듈로 라우팅되지 않고 다음 미들웨어도 실행 안함
        /// </summary>
        public bool IsClose { get; private set; }
        /// <summary>
        /// 웹 모듈로 라우팅 중지 (비동기)
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="StatusCode"></param>
        /// <param name="Abort">True 면 즉시 라우팅 중지 False 면 다른 미들웨어 까지만 실행후 웹 모듈로 라우팅 중지</param>
        public async Task CloseAsync(float StatusCode = 500, string Message = null, bool Abort = false) 
        {
            IsClose = true;
            this.Message = Message;
            this.StatusCode = StatusCode;
            IsAbort = Abort;

            ContextRaw.Response.StatusCode = StatusCode;
            if (Message != null) await Context.Response.WriteAsync(Message);
        }
        /// <summary>
        /// 웹 모듈로 라우팅 중지
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="StatusCode"></param>
        /// <param name="Abort">True 면 즉시 라우팅 중지 False 면 다른 미들웨어 까지만 실행후 웹 모듈로 라우팅 중지</param>
        public void Close(float StatusCode = 500, string Message = null, bool Abort = false) { CloseAsync(StatusCode, Message, Abort).Wait(); }

        internal float StatusCode { get; private set; }
        internal string Message { get; private set; }
        internal bool IsAbort { get; private set; }
    }
}
