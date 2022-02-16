using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using URLTools;
using System.IO;

namespace URLTools
{
    public static class BuildShit
    {
        public const string Name = "URLTools";
        public const string Author = "Penny";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
        public const string Description = "Use this mod to copy or open user, world and instance web pages";
    }
    public class Main : MelonMod
    {
        internal static readonly MelonLogger.Instance log = new MelonLogger.Instance(BuildShit.Name, ConsoleColor.Cyan);
        private static int scenesLoaded = 0;
        public override void OnApplicationStart()
        {
            try
            {
                var CoreDL = new HttpClient();
                CoreDL.DefaultRequestHeaders.Add("User-Agent", BuildShit.Name);
                var bytes = CoreDL.GetByteArrayAsync("https://github.com/PennyBunny/VRCMods/raw/main/Dependencies/ReMod.Core.dll").GetAwaiter().GetResult();
                File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "UserLibs", "ReMod.Core.dll"), bytes);
                CoreDL.Dispose();
            }
            catch (Exception ex) { log.Error(ex); }
            BundleManager.InIt();
            log.Msg("URLTools Loaded");
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
    }
}
