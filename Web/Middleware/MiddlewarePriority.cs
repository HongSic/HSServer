namespace HSServer.Web.Middleware
{
    /// <summary>
    /// 미들웨어 등록(초기화) 우선순위
    /// </summary>
    public enum MiddlewarePriority
    {
        /// <summary>
        /// 우선순위 최고 높음
        /// </summary>
        Crital = -1,
        /// <summary>
        /// 우선순위 높음
        /// </summary>
        High = 0,
        /// <summary>
        /// 우선순위 약간 높음
        /// </summary>
        BelowHigh = 1,
        /// <summary>
        /// 우선순위 중간 (기본값)
        /// </summary>
        Normal = 2,
        /// <summary>
        /// 우선순위 약간 낮음
        /// </summary>
        BelowLow = 3,
        /// <summary>
        /// 우선순위 낮음
        /// </summary>
        Low = 4
    }
}
