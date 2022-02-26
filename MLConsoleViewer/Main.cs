using System;
using System.Net;
using System.Reflection;
using MelonLoader;


namespace MLConsoleViewer
{
    public static class BuildShit
    {
        public const string Name = "MLConsoleViewer";
        public const string Author = "Penny, Davi";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
        public const string Description = "";
    }
    public class Main : MelonMod
    {
        internal static readonly MelonLogger.Instance log = new MelonLogger.Instance(BuildShit.Name, ConsoleColor.Cyan);
        private static int scenesLoaded = 0;
        public override void OnApplicationStart()
        {
            LoadRemodCore(out _);
            BundleManager.Init();
            log.Msg("MLConsoleViewer Loaded");
            log.Msg("Echo....... *there was no response* :(");
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (scenesLoaded <= 2)
            {
                scenesLoaded++;
                if (scenesLoaded == 2)
                {
                    MelonCoroutines.Start(UI.OnQuickMenu());
                }
            }
        }
        private void LoadRemodCore(out Assembly loadedAssembly)
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
    }
}