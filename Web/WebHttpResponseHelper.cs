using HS.Utils;
using HSServer.Web.Module;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HSServer.Web
{
    public static class WebHttpResponseHelper
    {
        /// <summary>
        /// 특정 주소로 리다이렉트 시킵니다 (헤더사용)
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="URL">이동시킬 주소 입니다</param>
        /// <returns>응답코드 302(Found) 를 반환합니다 (반드시 응답코드를 반환하여야 합니다)</returns>
        public static ModuleResultCode Redirect(this IWebHttpResponse Response, string URL)
        {
            Response.Headers.Add("Location", URL);
            return (ModuleResultCode)302;
        }

        #region Body Stream
        #region Non-Async
        /// <summary>
        /// 응답 요청에 문자열을 씁니다.
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Message">메세지 입니다</param>
        public static void Write(this IWebHttpResponse Response, string Message) { Write(Response, Message, Encoding.UTF8); }
        /// <summary>
        /// 응답 요청에 문자열을 씁니다.
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Message">메세지 입니다</param>
        /// <param name="Encoding">인코딩 입니다. (인코딩을 잘못 지정하면 문자열이 깨질수도 있습니다)</param>
        public static void Write(this IWebHttpResponse Response, string Message, Encoding Encoding) { WriteData(Response, Encoding.GetBytes(Message)); }
        /// <summary>
        /// 응답 요청에 데이터를 씁니다
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Buffer">쓸 데이터 입니다</param>
        public static void WriteData(this IWebHttpResponse Response, byte[] Data) { if(Data != null) Response.Body.Write(Data, 0, Data.Length); }
        /// <summary>
        /// 응답 요청에 스트림을 씁니다
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Stream">원본 스트림입니다</param>
        /// <param name="Buffer">버퍼 크기 입니다 (기본값은 1024 입니다(</param>
        /// <param name="Close">쓰고난다음 원본 스트림을 닫을지의 여부입니다.</param>
        public static void WriteStream(this IWebHttpResponse Response, Stream Stream, int Buffer = 1024, bool Close = true) { Stream.CopyStream(Response.Body, Buffer, false); if (Close) Stream.Close(); }
        #endregion

        #region Async
        /// <summary>
        /// 응답 요청에 비동기로 문자열을 씁니다.
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Message">메세지 입니다</param>
        public static async Task WriteAsync(this IWebHttpResponse Response, string Message) { await WriteAsync(Response, Message, Encoding.UTF8); }
        /// <summary>
        /// 응답 요청에 문자열을 씁니다.
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Message">메세지 입니다</param>
        /// <param name="Encoding">인코딩 입니다. (인코딩을 잘못 지정하면 문자열이 깨질수도 있습니다)</param>
        public static async Task WriteAsync(this IWebHttpResponse Response, string Message, Encoding Encoding) { await WriteDataAsync(Response, Encoding.GetBytes(Message)); }
        /// <summary>
        /// 응답 요청에 데이터를 씁니다
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Buffer">쓸 데이터 입니다</param>
        public static async Task WriteDataAsync(this IWebHttpResponse Response, byte[] Data) { if (Data != null) await Response.Body.WriteAsync(Data, 0, Data.Length); }
        /// <summary>
        /// 응답 요청에 스트림을 씁니다
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Stream">원본 스트림입니다</param>
        /// <param name="Buffer">버퍼 크기 입니다 (기본값은 1024 입니다(</param>
        /// <param name="Close">쓰고난다음 원본 스트림을 닫을지의 여부입니다.</param>
        public static async Task WriteStreamAsync(this IWebHttpResponse Response, Stream Stream, int Buffer = 1024, bool Close = true) { await Stream.CopyStreamAsync(Response.Body, Buffer, Close); }
        #endregion
        #endregion
    }
}
