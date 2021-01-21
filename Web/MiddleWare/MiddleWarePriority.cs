namespace HSServer.Web.MiddleWare
{
    /// <summary>
    /// 미들웨어 등록(초기화) 우선순위
    /// </summary>
    public enum MiddleWarePriority
    {
        /// <summary>
        /// 우선순위 높음
        /// </summary>
        High = 0,
        /// <summary>
        /// 우선순위 중간 (기본값)
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 우선순위 낮음
        /// </summary>
        Low = 2
    }
}
