﻿using HS.Utils;
using HSServer.Web.Module;

namespace HSServer.Web.MiddleWare
{
    public static class MiddleWareFactory
    {
        public class ModuleDataAlloc : ModuleData
        {
            public ModuleDataAlloc(LanguageManager STR_LANG, WebHttpContext Context, dynamic ExtraData)
            { this.STR_LANG = STR_LANG; this.Context = Context; this.ExtraData = ExtraData; }

            public ModuleDataAlloc(ModuleData Data) { STR_LANG = Data.STR_LANG; Context = Data.Context; ExtraData = Data.ExtraData; }
            public ModuleDataAlloc(MiddleWareData Data) { Path = Data.Path; STR_LANG = Data.STR_LANG; Context = Data.Context; ExtraData = Data.ExtraData; }

            /// <summary>
            /// 웹 접속 경로
            /// </summary>
            public string Path { get; }

            /// <summary>
            /// 언어
            /// </summary>
            public LanguageManager STR_LANG { get; set; }

            /// <summary>
            /// 웹 Http 컨텍스트
            /// </summary>
            public WebHttpContext Context { get; set; }

            public dynamic ExtraData { get; set; }
        }
    }
}
