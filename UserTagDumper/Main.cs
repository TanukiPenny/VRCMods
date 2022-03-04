using System;
using System.Linq;
using System.Net;
using System.Reflection;
using MelonLoader;

namespace UserTagDumper
{
    public static class BuildShit
    {
        public const string Name = "UserTagDumper";
        public const string Author = "Penny";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
        public const string Description = "A mod that dumps a selected user's API tags to the ML console";
    }
    
    public class Main : MelonMod
    {
        internal static readonly MelonLogger.Instance log = new MelonLogger.Instance(BuildShit.Name, ConsoleColor.Cyan);
        private static int _scenesLoaded = 0;
        public override void OnApplicationStart()
        {
            LoadReModCore(out _);
            log.Msg("UserTagDumper Loaded");
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (_scenesLoaded <= 2)
            {
                _scenesLoaded++;
                if (_scenesLoaded == 2)
                {
                    MelonCoroutines.Start(UI.OnQuickMenu());
                }
            }
        }
        private void LoadReModCore(out Assembly loadedAssembly)
        {
            byte[] bytes = null;
            var wc = new WebClient();
        

            try
            {
                loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(ass => ass.FullName.Contains("ReMod.Core")) ?? 
                                 Assembly.Load(new WebClient().DownloadData("https://github.com/RequiDev/ReMod.Core/releases/latest/download/ReMod.Core.dll"));
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

    }
}