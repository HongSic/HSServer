using System;

namespace HSServer.Web.Middleware
{
    /// <summary>
    /// 미들웨어 정보
    /// </summary>
    public class MiddlewareAttribute : Attribute, IEquatable<MiddlewareAttribute>
    {
        private static int RandomSeed = 20200923;
        private readonly Random Random = new Random(RandomSeed);

        /// <summary>
        /// 미들웨어 고유 ID (자동할당 됨)
        /// </summary>
        public uint ID { get; private set; }
        /// <summary>
        /// 미들웨어 이름
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 미들웨어 초기화시 자동 등록 여부
        /// </summary>
        public bool AutoRegister { get; private set; }
        /// <summary>
        /// 미들웨어 등록(초기화) 우선순위
        /// </summary>
        public MiddlewarePriority Priority { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name">미들웨어 이름</param>
        /// <param name="Priority">미들웨어 등록(초기화) 우선순위</param>
        /// <param name="AutoRegister">미들웨어 초기화시 자동 등록 여부</param>
        public MiddlewareAttribute(string Name, MiddlewarePriority Priority = MiddlewarePriority.Normal, bool AutoRegister = true) { this.Name = Name; this.Priority = Priority; this.AutoRegister = AutoRegister; ID = (uint)Random.Next(int.MinValue, int.MaxValue); }

        public virtual bool Equals(MiddlewareAttribute other) { return other.ID == ID; }
    }
}
