using HS.Utils;
using HSServer.Settings.WebModule;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using HS.Utils.Stream;
using HS.Utils.Text;

namespace HSServer.Settings
{
    public class ModuleSetting
    {
        public const string FILENAME = "ModuleLoad.json";
        public const string DIR = "Settings";

        public static readonly string DefaultPath = StringUtils.GetExcuteDirectory() + $"/{DIR}/{FILENAME}";

        public ModuleSetting(ModuleLoadOption LoadOption, ModuleLoad Load) { this.LoadOption = LoadOption; this.Load = Load; }
        public ModuleLoadOption LoadOption { get; private set; }
        public ModuleLoad Load { get; private set; }

        public static ModuleSetting FromJSONFile(string JSONPath = null)
        {
            //JSONPath 가 비어있거나 null 이면
            if (string.IsNullOrWhiteSpace(JSONPath))
                JSONPath = StringUtils.GetExcuteDirectory() + $"/{DIR}/{FILENAME}";

            return File.Exists(JSONPath) ? FromJSON(File.ReadAllText(JSONPath)) : null;
        }

        public static ModuleSetting FromResource()
        {
            using (var res = StreamUtils.GetResourceStream($"HSServer.{DIR}", FILENAME))
            using (var str = new StreamReader(res))
                return FromJSON(str.ReadToEnd());
        }

        public static ModuleSetting FromJSON(string JSON, string RootPath = null)
        {
            var option = new ModuleLoadOption();

            var json = JsonDocument.Parse(JSON, new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            });

            var Controller = json.RootElement.GetProperty("Controller");
            var Router = json.RootElement.GetProperty("Router");
            var MiddleWare = json.RootElement.GetProperty("MiddleWare");

            var load = new ModuleLoad(GetArray(MiddleWare, RootPath), GetArray(Router, RootPath), GetArray(Controller, RootPath));
            return new ModuleSetting(option, load);
        }

        private static string[] GetArray(JsonElement ele, string RootPath = null)
        {
            var json_path = ele.GetProperty("Path");
            var json_recursive = ele.GetProperty("Recursive").GetBoolean();

            int len = json_path.GetArrayLength();
            List<string> item = new List<string>(len);
            Stack<string> dirs = new Stack<string>();
            for (int i = 0; i < len; i++)
            {
                dirs.Clear();
                string path;

                if (string.IsNullOrWhiteSpace(RootPath))
                {
                    path = json_path[i].GetString();
                    //if (path[0] != '\\' && path[0] != '/' && path[1] != ':') path = StringUtils.PathMaker(RootPath, path);

                    if (!ContentsExist(path))
                    {
                        string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        path = StringUtils.PathMaker(dir, json_path[i].GetString());
                        if (!ContentsExist(path)) continue;
                    }
                }
                else path = StringUtils.PathMaker(RootPath, json_path[i].GetString());

                if (Directory.Exists(path))
                {
                    dirs.Push(path);
                    do
                    {
                        string dir = dirs.Pop();

                        string[] files = Directory.GetFiles(dir, "*.dll");
                        item.Capacity += files.Length;
                        for (int j = 0; j < files.Length; j++)
                            item.Add(Path.GetFullPath(files[j]));

                        dirs.PushAll(Directory.GetDirectories(dir));
                    }
                    while (json_recursive && dirs.Count > 0);
                }
                else item.Add(Path.GetFullPath(path));
            }
            return item.ToArray();
        }
        private static bool ContentsExist(string Path) { return Directory.Exists(Path) || File.Exists(Path); }
    }
}
