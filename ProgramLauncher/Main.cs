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
using System.Reflection;
using System.Net;

namespace ProgramLauncher
{
    public static class BuildShit
    {
        public const string Name = "ProgramLauncher";
        public const string Author = "Penny, Lily";
        public const string Version = "2.0.0";
        public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
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
        public static Programs Prog { get; set; }// = Load();

        public static readonly string Path = $"{Environment.CurrentDirectory}{System.IO.Path.DirectorySeparatorChar}UserData{System.IO.Path.DirectorySeparatorChar}Programs.json";

        public static void CheckIfFileExists() {
            if (!File.Exists(Path)) {
                File.WriteAllText(Path, JsonConvert.SerializeObject(new Programs {
                    ListOfPrograms = new List<Sets> {
                        new Sets {
                            Name = "Notepad",
                            FilePath = "C:\\WINDOWS\\system32\\notepad.exe"
                        }
                    }
                }, Formatting.Indented));
            }
            Load();
        }

        private static void Load() => Prog = JsonConvert.DeserializeObject<Programs>(File.ReadAllText(Path));

        private static void Save() => File.WriteAllText(Path, JsonConvert.SerializeObject(Prog, Formatting.Indented));

        //private static string GetPath(string name) {
        //    var _Path = Prog.ListOfPrograms.FirstOrDefault(n => n.Name == name);
        //    if (_Path == null) return null;
        //    return _Path.FilePath;
        //}

        public static void AddItem(string name, string path) {
            Prog.ListOfPrograms.Add(new Sets {
                Name = name,
                FilePath = path
            });
            Save();
        }

        public static void RemoveItem(string name) {
            Prog.ListOfPrograms.Remove(Prog.ListOfPrograms.FirstOrDefault(n => n.Name == name));
            Save();
        }
    }
    public class Main : MelonMod
    {
        internal static readonly MelonLogger.Instance log = new MelonLogger.Instance(BuildShit.Name, ConsoleColor.Green);
        private static int _scenesLoaded = 0;
        public override void OnApplicationStart()
        {
            LoadRemodCore(out _);
            BundleManager.InIt();
            try { SetPrograms.CheckIfFileExists(); } catch (Exception e) { log.Error(e); }
            log.Msg("ProgramLauncher loaded successfully!");
        }
        private static void LoadRemodCore(out Assembly loadedAssembly)
        {
            byte[] bytes = null;
            var wc = new WebClient();


            try
            {
                bytes = wc.DownloadData($"https://github.com/RequiDev/ReModCE/releases/latest/download/ReMod.Core.dll");
                loadedAssembly = Assembly.Load(bytes);
            }
            catch (WebException e)
            {
                MelonLogger.Error($"Unable to Load Core Dep RemodCore: {e}");
            }
            catch (BadImageFormatException e)
            {
                loadedAssembly = null;
            }
            loadedAssembly = null;
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (_scenesLoaded <= 2)
            {
                _scenesLoaded++;
                if (_scenesLoaded == 2)
                {
                    MelonCoroutines.Start(Menu.OnQuickMenu());
                }
            }
        }
    }
}
