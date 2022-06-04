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
        public const string Version = "1.0.1";
        public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
        public const string Description = "A mod that dumps a selected user's API tags to the ML console";
    }
    
    public class Main : MelonMod
    {
        internal static readonly MelonLogger.Instance log = new MelonLogger.Instance(BuildShit.Name, ConsoleColor.Cyan);
        private static int _scenesLoaded = 0;
        public override void OnApplicationStart()
        {
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
    }
}