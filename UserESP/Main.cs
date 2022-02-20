using System;
using MelonLoader;

namespace UserESP
{
    public static class BuildShit
    {
        public const string Name = "UserESP";
        public const string Author = "Penny";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
        public const string Description = "A standalone mod to turn on a ESP for selected user";
    }

    public class Main : MelonMod
    {
        internal static readonly MelonLogger.Instance log = new MelonLogger.Instance(BuildShit.Name, ConsoleColor.Yellow);

        public override void OnApplicationStart()
        {
            log.Msg("Ur gay");
        }
    }
}