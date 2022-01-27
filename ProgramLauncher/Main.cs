using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using MelonLoader;
using Newtonsoft.Json;

namespace ProgramLauncher
{
    public static class BuildShit
    {
        public const string Name = "ProgramLauncher";
        public const string Author = "1 Cent Cutie";
        public const string Version = "1.0.0";
        public const string DownloadLink = "";
        public const string Description = "A standalone mod to launch external programs from inside VRChat with a click of a button!";
    }

    public class Programs {
        public List<Sets> ListOfPrograms { get;set; }
    }

    public class Sets {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public Sets() {}
        public Sets(string _Name, string Path) {
            Name = _Name;
            FilePath = Path;
        }
    }

    public static class SetPrograms {
        public static Programs _prog { get; set; } = Load();

        private static readonly string path = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}UserData{Path.DirectorySeparatorChar}Programs.json";

        private static Programs Load() {
            if (!File.Exists(path)) {
                File.Create(path);
            }

            try { 
                var j = JsonConvert.DeserializeObject<Programs>(File.ReadAllText(path));
                if (j == null) throw new Exception();
                return j;
            }
            catch {
                return new Programs() { ListOfPrograms = new List<Sets>() };
            }
        }

        public static void Save() => File.WriteAllText(path, JsonConvert.SerializeObject(_prog, Formatting.Indented));

        public static string GetPath(string name) {
            var _Path = _prog.ListOfPrograms.FirstOrDefault(n => n.Name == name);
            if (_Path == null) return "";
            return _Path.FilePath;
        }

        public static bool AlreadyAThing(string name) => _prog.ListOfPrograms.Any(a => a.Name == name);

        public static void AddItem(string _Name, string Path) {
            _prog.ListOfPrograms.Add(new Sets {
                Name = _Name,
                FilePath = Path
            });
            Save();
        }
    }


    public class Main : MelonMod
    {
        internal static readonly MelonLogger.Instance log = new MelonLogger.Instance(BuildShit.Name, ConsoleColor.Green);
        public override void OnApplicationStart()
        {
            log.Msg("ProgramLauncher loaded successfully!");
            Menu.Init();
        }
    }
}
