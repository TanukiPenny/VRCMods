using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MelonLoader;
using Newtonsoft.Json;
using UnityEngine;

namespace ProgramLauncher
{
    public static class BuildShit
    {
        public const string Name = "ProgramLauncher";
        public const string Author = "1 Cent Cutie, Lily";
        public const string Version = "0.9.0";
        public const string DownloadLink = "";
        public const string Description = "A standalone mod to launch external programs from inside VRChat with a click of a button!";
    }

    public class Programs {
        public List<Sets> ListOfPrograms { get; set; }
    }

    public class Sets {
        public string Name { get; set; }
        public string FilePath { get; set; }
    }

    public static class SetPrograms {
        public static Programs _prog { get; set; }// = Load();

        public static readonly string path = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}UserData{Path.DirectorySeparatorChar}Programs.json";

        public static void CheckIfFileExists() {
            if (!File.Exists(path)) {
                File.WriteAllText(path, JsonConvert.SerializeObject(new Programs(), Formatting.Indented));
                AddItem("Notepad", "C:\\WINDOWS\\system32\\notepad.exe");
            }

            Load();
        }

        public static void Load() {
            _prog = JsonConvert.DeserializeObject<Programs>(File.ReadAllText(path));
        }

        private static void Save() => File.WriteAllText(path, JsonConvert.SerializeObject(_prog, Formatting.Indented));

        private static string GetPath(string name) {
            var _Path = _prog.ListOfPrograms.FirstOrDefault(n => n.Name == name);
            if (_Path == null) return null;
            return _Path.FilePath;
        }

        public static void AddItem(string _Name, string Path) {
            _prog.ListOfPrograms.Add(new Sets {
                Name = _Name,
                FilePath = Path
            });
            Save();
        }

        public static void RemoveItem(string name) {
            _prog.ListOfPrograms.Remove(_prog.ListOfPrograms.FirstOrDefault(n => n.Name == name));
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
            try { SetPrograms.CheckIfFileExists(); } catch (Exception e) { log.Error(e); }
        }
    }
}
