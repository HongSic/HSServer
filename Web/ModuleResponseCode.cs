namespace HSServer.Web
{
    /// <summary>
    /// 모듈 결과 코드 (HTTP 응답 코드)
    /// (만약 모듈에서 클라이언트로 이미 응답을 보낸상태라면 응답을 보내지 않음)
    /// </summary>
    public enum ModuleResponseCode
    {
        /// <summary>
        /// [Special] Router Bypass
        /// </summary>
        Bypass = -1,
        /// <summary>
        /// [Special] OK. but warning
        /// </summary>
        Warning = 1,
        OK = 200,
        /// <summary>
        /// Found
        /// </summary>
        Redirect = 302,
        BadRequest = 400,
        /// <summary>
        /// Not Login or Authentication error
        /// </summary>
        NotAuth = 401,
        Forbidden = 403,
        NotFound = 404,
        /// <summary>
        /// Method Not Allow
        /// </summary>
        NotAllowed = 405,
        /// <summary>
        /// Access to the target resource is no longer available
        /// </summary>
        Gone = 410,
        /// <summary>
        /// Server Error
        /// </summary>
        Error = 500,
        NotImplemented = 501,
        Timeout = 504,
    }
}