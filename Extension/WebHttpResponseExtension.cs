﻿using HS.Utils;
using HSServer.Utils;
using HSServer.Web;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HS.Utils.Stream;
using HS.Utils.Text;

namespace HSServer.Extension
{
    public static class WebHttpResponseExtension
    {
        /// <summary>
        /// 특정 주소로 리다이렉트 시킵니다 (헤더사용)
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="URL">이동시킬 주소 입니다</param>
        /// <returns>응답코드 302(Found) 를 반환합니다 (반드시 응답코드를 반환하여야 합니다)</returns>
        public static ModuleResponseCode Redirect(this IWebHttpResponse Response, string URL)
        {
            Response.Headers.Add("Location", URL);
            return (ModuleResponseCode)302;
        }

        #region Body Stream
        #region Non-Async
        /// <summary>
        /// 응답 요청에 문자열을 씁니다.
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Message">메세지 입니다</param>
        public static void Write(this IWebHttpResponse Response, string Message, HttpStatusCode Code = HttpStatusCode.OK) { Write(Response, Message, Encoding.UTF8, Code); }
        /// <summary>
        /// 응답 요청에 문자열을 씁니다.
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Message">메세지 입니다</param>
        /// <param name="Encoding">인코딩 입니다. (인코딩을 잘못 지정하면 문자열이 깨질수도 있습니다)</param>
        public static void Write(this IWebHttpResponse Response, string Message, Encoding Encoding, HttpStatusCode Code = HttpStatusCode.OK) { WriteData(Response, Encoding.GetBytes(Message), Code); }
        /// <summary>
        /// 응답 요청에 객체를 JSON 으로 씁니다.
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Data">인스턴스 입니다</param>
        public static void Write(this IWebHttpResponse Response, object Data, HttpStatusCode Code = HttpStatusCode.OK) => Write(Response, JSONUtils.ToSerializeJSON_NS(Data), Encoding.UTF8, Code);
        /// <summary>
        /// 응답 요청에 객체를 JSON 으로 씁니다.
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Data">인스턴스 입니다</param>
        /// <param name="Encoding">인코딩 입니다. (인코딩을 잘못 지정하면 문자열이 깨질수도 있습니다)</param>
        public static void Write(this IWebHttpResponse Response, object Data, Encoding Encoding, HttpStatusCode Code = HttpStatusCode.OK) => Write(Response, JSONUtils.ToSerializeJSON_NS(Data), Encoding, Code);
        /// <summary>
        /// 응답 요청에 데이터를 씁니다
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Buffer">쓸 데이터 입니다</param>
        public static void WriteData(this IWebHttpResponse Response, byte[] Data, HttpStatusCode Code = HttpStatusCode.OK)
        {
            if (Data != null)
            {
                Response.StatusCode = (float)Code;
                Response.Body.Write(Data, 0, Data.Length);
            }
        }
        /// <summary>
        /// 응답 요청에 스트림을 씁니다
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Stream">원본 스트림입니다</param>
        /// <param name="Buffer">버퍼 크기 입니다 (기본값은 1024 입니다(</param>
        /// <param name="Close">쓰고난다음 원본 스트림을 닫을지의 여부입니다.</param>
        public static void WriteStream(this IWebHttpResponse Response, Stream Stream, HttpStatusCode Code = HttpStatusCode.OK, int Buffer = 1024, bool Close = true)
        {
            if (Stream != null)
            {
                Response.StatusCode = (float)Code;
                Stream.CopyStream(Response.Body, Buffer, false); 
                if (Close) Stream.Close();
            }
        }
        #endregion

        #region Async
        /// <summary>
        /// 응답 요청에 비동기로 문자열을 씁니다.
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Message">메세지 입니다</param>
        public static async Task WriteAsync(this IWebHttpResponse Response, string Message, HttpStatusCode Code = HttpStatusCode.OK) { await WriteAsync(Response, Message, Encoding.UTF8, Code); }
        /// <summary>
        /// 응답 요청에 객체를 JSON 으로 씁니다.
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Data">인스턴스 입니다</param>
        public static async Task WriteAsync(this IWebHttpResponse Response, object Data, HttpStatusCode Code = HttpStatusCode.OK) => await WriteAsync(Response, JSONUtils.ToSerializeJSON_NS(Data), Encoding.UTF8, Code);
        /// <summary>
        /// 응답 요청에 객체를 JSON 으로 씁니다.
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Data">인스턴스 입니다</param>
        /// <param name="Encoding">인코딩 입니다. (인코딩을 잘못 지정하면 문자열이 깨질수도 있습니다)</param>
        public static async Task WriteAsync(this IWebHttpResponse Response, object Data, Encoding Encoding, HttpStatusCode Code = HttpStatusCode.OK) => await WriteAsync(Response, JSONUtils.ToSerializeJSON_NS(Data), Encoding, Code);
        /// <summary>
        /// 응답 요청에 문자열을 씁니다.
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Message">메세지 입니다</param>
        /// <param name="Encoding">인코딩 입니다. (인코딩을 잘못 지정하면 문자열이 깨질수도 있습니다)</param>
        public static async Task WriteAsync(this IWebHttpResponse Response, string Message, Encoding Encoding, HttpStatusCode Code = HttpStatusCode.OK) { await WriteDataAsync(Response, Encoding.GetBytes(Message), Code); }
        /// <summary>
        /// 응답 요청에 데이터를 씁니다
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Buffer">쓸 데이터 입니다</param>
        public static async Task WriteDataAsync(this IWebHttpResponse Response, byte[] Data, HttpStatusCode Code = HttpStatusCode.OK) 
        {
            if (Data != null)
            {
                Response.StatusCode = (float)Code;
                await Response.Body.WriteAsync(Data, 0, Data.Length);
            }
        }
        /// <summary>
        /// 응답 요청에 스트림을 씁니다
        /// </summary>
        /// <param name="Response">Response 인스턴스 입니다</param>
        /// <param name="Stream">원본 스트림입니다</param>
        /// <param name="Buffer">버퍼 크기 입니다 (기본값은 1024 입니다(</param>
        /// <param name="Close">쓰고난다음 원본 스트림을 닫을지의 여부입니다.</param>
        public static async Task WriteStreamAsync(this IWebHttpResponse Response, Stream Stream, HttpStatusCode Code = HttpStatusCode.OK, int Buffer = 1024, bool Close = true)
        {
            if (Stream != null)
            {
                Response.StatusCode = (float)Code;
                await Stream.CopyStreamAsync(Response.Body, Buffer, false);
                if (Close) Stream.Close();
            }
        }
        #endregion
        #endregion

        public static void SetHeader(this IWebHttpResponse Response, string Key, params string[] Value)
        {
            if(!Response.IsHeaderSent)
            {
                if (Response.Headers.Exist(Key)) Response.Headers.Remove(Key);
                Response.Headers.Add(Key, Value);
            }
        }

        public static WebHttpCookieValue GetCookie(this IWebHttpResponse Response, string Name, WebHttpCookieValue Default = null) => WebUtils.GetCookie(Response.Cookies, Name, Default);
        public static IList<WebHttpCookieValue> GetCookies(this IWebHttpResponse Response, string Name, IList<WebHttpCookieValue> Default = null) => WebUtils.GetCookies(Response.Cookies, Name, Default);

    }
}
