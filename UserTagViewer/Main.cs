using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;

namespace UserTagViewer
{
    public static class BuildShit
    {
        public const string Name = "UserTagViewer";
        public const string Author = "Penny";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
        public const string Description = "Use this mod to view users API tags in quick menu.";
    }
    public class Main : MelonMod
    {
        internal static readonly MelonLogger.Instance log = new MelonLogger.Instance(BuildShit.Name, ConsoleColor.Cyan);
        private static int scenesLoaded = 0;
        public override void OnApplicationStart()
        {
            LoadRemodCore(out _);
            //BundleManager.InIt();
            log.Msg("UserTagViewer Loaded");
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (scenesLoaded <= 2)
            {
                scenesLoaded++;
                if (scenesLoaded == 2)
                {
                    MelonCoroutines.Start(UIBuild.OnQuickMenu());
                }
            }
        }
        private void LoadRemodCore(out Assembly loadedAssembly)
        {
            byte[] bytes = null;
            var wc = new WebClient();


            try
            {
                bytes = wc.DownloadData($"https://assets.ellyvr.dev/vrc/mods/ReMod.Core.dll");
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
