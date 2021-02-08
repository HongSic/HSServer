using HS.Utils;
using HSServer.Settings.WebRouter;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace HSServer.Settings
{
    public class WebRouterSetting
    {
        private const string FILENAME = "WebRouter.json";

        public WebRouterSetting(WebRouterLoadOption LoadOption, WebRouterLoad Load) { this.LoadOption = LoadOption; this.Load = Load; }
        public WebRouterLoadOption LoadOption { get; private set; }
        public WebRouterLoad Load { get; private set; }

        public static WebRouterSetting FromJSONFile(string JSONPath = null)
        {            
            //JSONPath 가 비어있거나 null 이면
            if (string.IsNullOrWhiteSpace(JSONPath))
                JSONPath = StringUtils.GetExcutePath() + "\\Setting\\" + FILENAME;

            return File.Exists(JSONPath) ? FromJSON(File.ReadAllText(JSONPath)) : null;
        }

        public static WebRouterSetting FromResource()
        {
            using (var res = IOUtils.GetResourceStream("HSServer.Settings." + FILENAME))
            using (var str = new StreamReader(res))
                return FromJSON(str.ReadToEnd());
        }

        public static WebRouterSetting FromJSON(string JSON, string RootPath = null)
        {
            var option = new WebRouterLoadOption();

            var json = JsonDocument.Parse(JSON, new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            });

            var Load = json.RootElement.GetProperty("Load");
            var load = new WebRouterLoad(GetArray(Load, "MiddleWare", RootPath), GetArray(Load, "Module", RootPath));
            return new WebRouterSetting(option, load);
        }

        private static string[] GetArray(JsonElement ele, string Property, string RootPath = null)
        {
            JsonElement array;
            bool exist = ele.TryGetProperty(Property, out array);
            if (exist)
            {
                int len = array.GetArrayLength();
                List<string> item = new List<string>(len);
                for (int i = 0; i < len; i++)
                {
                    string path;

                    if (string.IsNullOrWhiteSpace(RootPath))
                    {
                        path = array[i].GetString();
                        //if (path[0] != '\\' && path[0] != '/' && path[1] != ':') path = StringUtils.PathMaker(RootPath, path);

                        if (!ContentsExist(path))
                        {
                            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                            path = StringUtils.PathMaker(dir, array[i].GetString());
                            if (!ContentsExist(path)) continue;
                        }
                    }
                    else path = StringUtils.PathMaker(RootPath, array[i].GetString());

                    if (Directory.Exists(path))
                    {
                        string[] files = Directory.GetFiles(path);
                        item.Capacity += files.Length;
                        for (int j = 0; j < files.Length; j++)
                            item.Add(Path.GetFullPath(files[j]));
                    }
                    else item.Add(Path.GetFullPath(path));
                }
                return item.ToArray();
            }
            else return null;
        }
        private static bool ContentsExist(string Path) { return Directory.Exists(Path) || File.Exists(Path); }
    }
}
