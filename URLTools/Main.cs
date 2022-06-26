using System;
using MelonLoader;

namespace URLTools;

public static class BuildShit
{
    public const string Name = "URLTools";
    public const string Author = "Penny";
    public const string Version = "1.0.6";
    public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
    public const string Description = "Use this mod to copy or open user, world and instance web pages";
}
public class Main : MelonMod
{
    internal static readonly MelonLogger.Instance log = new MelonLogger.Instance(BuildShit.Name, ConsoleColor.DarkYellow);
    private static int scenesLoaded = 0;
    public override void OnApplicationStart()
    {
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