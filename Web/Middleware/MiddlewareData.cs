using HS.Utils;
using HS.Utils.Web.Http;
using HSServer.Extension;
using HSServer.Web.Router;
using System.Net;
using System.Threading.Tasks;

namespace HSServer.Web.Middleware
{
    public class MiddlewareData : RouterData
    {
        public string Path { get; set; }

        public LanguageManager STR_LANG { get; set; }
        public WebHttpContextRaw ContextRaw { get; }

        public WebHttpContext Context { get; set; }
        public object ExtraData { get; set; }

        /// <summary>
        /// IsClose 가 true 면 웹 모듈로 라우팅되지 않고 다음 미들웨어도 실행 안함
        /// </summary>
        public bool IsClose { get; private set; }

        internal MiddlewareData(string Path, LanguageManager STR_LANG, WebHttpContextRaw ContextRaw)
        {
            this.Path = Path;
            this.STR_LANG = STR_LANG;
            this.ContextRaw = ContextRaw;

            Context = new WebHttpContext(ContextRaw);
        }

        /// <summary>
        /// 웹 모듈로 라우팅 중지 (비동기)
        /// </summary>
        /// <param name="StatusCode">HTTP 상태 코드</param>
        /// <param name="Data">데이터 (JSON 으로 출력됩니다)</param>
        /// <param name="Abort">True 면 즉시 라우팅 중지 False 면 다른 미들웨어 까지만 실행후 웹 모듈로 라우팅 중지</param>
        public async Task CloseAsync(float StatusCode = 500, object Data = null, bool Abort = false) { await WriteAsync(StatusCode, JSONUtils.ToSerializeJSON1(Data), Abort, "application/json; utf-8"); }
        /// <summary>
        /// 웹 모듈로 라우팅 중지 (비동기)
        /// </summary>
        /// <param name="StatusCode">HTTP 상태 코드</param>
        /// <param name="Data">데이터 (JSON 으로 출력됩니다)</param>
        /// <param name="Abort">True 면 즉시 라우팅 중지 False 면 다른 미들웨어 까지만 실행후 웹 모듈로 라우팅 중지</param>
        public async Task CloseAsync(ModuleResponseCode StatusCode = ModuleResponseCode.Error, object Data = null, bool Abort = false) { await CloseAsync((float)StatusCode, Data, Abort); }
        /// <summary>
        /// 웹 모듈로 라우팅 중지 (비동기)
        /// </summary>
        /// <param name="StatusCode">HTTP 상태 코드</param>
        /// <param name="Message">출력 메세지</param>
        /// <param name="Abort">True 면 즉시 라우팅 중지 False 면 다른 미들웨어 까지만 실행후 웹 모듈로 라우팅 중지</param>
        public async Task CloseAsync(ModuleResponseCode StatusCode = ModuleResponseCode.Error, string Message = null, bool Abort = false) { await CloseAsync((float)StatusCode, Message, Abort); }
        /// <summary>
        /// 웹 모듈로 라우팅 중지 (비동기)
        /// </summary>
        /// <param name="StatusCode">HTTP 상태 코드</param>
        /// <param name="Message">출력 메세지</param>
        /// <param name="Abort">True 면 즉시 라우팅 중지 False 면 다른 미들웨어 까지만 실행후 웹 모듈로 라우팅 중지</param>
        public async Task CloseAsync(float StatusCode = 500, string Message = null, bool Abort = false) { await WriteAsync(StatusCode, Message, Abort, "text/html; utf-8"); }
        private async Task WriteAsync(float StatusCode, string Message, bool Abort, string ContentType)
        {
            IsClose = true;
            this.Message = Message;
            this.StatusCode = StatusCode;
            IsAbort = Abort;

            if (!ContextRaw.Response.IsHeaderSent) ContextRaw.Response.StatusCode = StatusCode;
            if (Message != null && ContextRaw.Response.IsWritable)
            {
                Context.Response.SetHeader(HttpHeaderKind.ContentType, ContentType);
                await ContextRaw.Response.WriteAsync(Message, (HttpStatusCode)StatusCode);
            }
        }

        /// <summary>
        /// 웹 모듈로 라우팅 중지
        /// </summary>
        /// <param name="StatusCode">HTTP 상태 코드</param>
        /// <param name="Data">데이터 (JSON 으로 출력됩니다)</param>
        /// <param name="Abort">True 면 즉시 라우팅 중지 False 면 다른 미들웨어 까지만 실행후 웹 모듈로 라우팅 중지</param>
        public void Close(float StatusCode = 500, object Data = null, bool Abort = false) { CloseAsync(StatusCode, Data, Abort).Wait(); }
        /// <summary>
        /// 웹 모듈로 라우팅 중지
        /// </summary>
        /// <param name="StatusCode">HTTP 상태 코드</param>
        /// <param name="Message">출력 메세지</param>
        /// <param name="Abort">True 면 즉시 라우팅 중지 False 면 다른 미들웨어 까지만 실행후 웹 모듈로 라우팅 중지</param>
        public void Close(float StatusCode = 500, string Message = null, bool Abort = false) { CloseAsync(StatusCode, Message, Abort).Wait(); }
        /// <summary>
        /// 웹 모듈로 라우팅 중지
        /// </summary>
        /// <param name="StatusCode">HTTP 상태 코드</param>
        /// <param name="Message">출력 메세지</param>
        /// <param name="Abort">True 면 즉시 라우팅 중지 False 면 다른 미들웨어 까지만 실행후 웹 모듈로 라우팅 중지</param>
        public void Close(ModuleResponseCode StatusCode = ModuleResponseCode.Error, string Message = null, bool Abort = false) { CloseAsync(StatusCode, Message, Abort).Wait(); }
        /// <summary>
        /// 웹 모듈로 라우팅 중지
        /// </summary>
        /// <param name="StatusCode">HTTP 상태 코드</param>
        /// <param name="Data">데이터 (JSON 으로 출력됩니다)</param>
        /// <param name="Abort">True 면 즉시 라우팅 중지 False 면 다른 미들웨어 까지만 실행후 웹 모듈로 라우팅 중지</param>
        public void Close(ModuleResponseCode StatusCode = ModuleResponseCode.Error, object Data = null, bool Abort = false) { CloseAsync(StatusCode, Data, Abort).Wait(); }

        internal float StatusCode { get; private set; }
        internal string Message { get; private set; }
        internal bool IsAbort { get; private set; }
    }
}
