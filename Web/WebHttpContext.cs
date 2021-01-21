namespace HSServer.Web
{
    public class WebHttpContext
    {
        protected WebHttpContext() { }
        public WebHttpContext(IWebHttpRequest Request, IWebHttpResponse Response, WebHttpSession Session, IWebHttpInfo Info) 
        { this.Request = Request; this.Response = Response; this.Session = Session; this.Info = Info; }
        public WebHttpContext(WebHttpContextRaw Raw) 
        { this.Request = Raw.Request; this.Response = Raw.Response; this.Session = new WebHttpSession(Raw.Session); this.Info = Raw.Info; }

        public IWebHttpRequest Request { get; }
        public IWebHttpResponse Response { get; }
        public WebHttpSession Session { get; }
        public IWebHttpInfo Info { get; }
        
        /*
        IWebHttpRequest Request { get; }
        IWebHttpResponse Response { get; }
        WebHttpSession Session { get; }
        IWebHttpInfo Info { get; }
        */
    }
}
