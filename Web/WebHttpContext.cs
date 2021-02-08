namespace HSServer.Web
{
    public class WebHttpContext
    {
        protected WebHttpContext() { }
        public WebHttpContext(IWebHttpRequest Request, IWebHttpResponse Response, WebHttpSession Session, IWebHttpInfo Info) 
        { this.Request = Request; this.Response = Response; this.Session = Session; this.Info = Info; }
        public WebHttpContext(WebHttpContextRaw Raw) 
        {
            this.Request = Raw.Request; 
            this.Response = Raw.Response; 
            this.Info = Raw.Info; 
            try { if(Raw.Session != null) this.Session = new WebHttpSession(Raw.Session); } catch { } }

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
