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
        BadRequest = 400,
        NotAuth = 401,
        Forbidden = 403,
        NotFound = 404,
        /// <summary>
        /// Method Not Allow
        /// </summary>
        NotAllowed = 405,
        /// <summary>
        /// Server Error
        /// </summary>
        Error = 500,
        NotImplemented = 501,
        Timeout = 504,
    }
}