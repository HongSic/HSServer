using HSServer.Web.Socket;

namespace HSServer.Web
{
    public abstract class WebHttpContextRaw
    {
        public WebHttpContextRaw() { }
        public WebHttpContextRaw(IWebHttpRequest Request, IWebHttpResponse Response, WebHttpSessionRaw Session, IWebHttpInfo Info) 
        { this.Request = Request; this.Response = Response; this.Session = Session; this.Info = Info; }
        public WebHttpContextRaw(IWebHttpRequest Request, IWebHttpResponse Response, WebHttpSessionRaw Session, IWebHttpInfo Info, IWebSocketContext Socket)
        { this.Request = Request; this.Response = Response; this.Session = Session; this.Info = Info; this.Socket = Socket; }
        /// <summary>
        /// 요청 객체
        /// </summary>
        public IWebHttpRequest Request { get; protected set; }
        /// <summary>
        /// 응답 객체
        /// </summary>
        public IWebHttpResponse Response { get; protected set; }
        /// <summary>
        /// 세션 객체
        /// </summary>
        public WebHttpSessionRaw Session { get; protected set; }
        /// <summary>
        /// 연결 정보
        /// </summary>
        public IWebHttpInfo Info { get; protected set; }
        /// <summary>
        /// 웹 소켓 객체
        /// </summary>
        public IWebSocketContext Socket { get; protected set; }
    }
}
