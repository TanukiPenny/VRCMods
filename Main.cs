using System;
using System.Net;
using System.Reflection;
using MelonLoader;

namespace MLConsoleViewer;

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
    // ReSharper disable once InconsistentNaming
    internal static readonly MelonLogger.Instance log = new(BuildShit.Name, ConsoleColor.Cyan);
    private static int scenesLoaded;
    private static MelonPreferences_Category _mlConsoleViewer;
    private static MelonPreferences_Entry<int> _fontSize;
    public override void OnApplicationStart()
    {
        LoadReModCore(out _);
        BundleManager.Init();
        ConsoleManager.AttachTrackers();
        _mlConsoleViewer = MelonPreferences.CreateCategory("MLConsoleViewer", "MLConsoleViewer");
        _fontSize = _mlConsoleViewer.CreateEntry("fontSize", 20, "Font Size",
            "Font size of the text in your console tab");
        log.Msg("MLConsoleViewer Loaded");
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

    public override void OnPreferencesSaved()
    {
        if (UI.text == null)
            return;
        UI.text.fontSize = _fontSize.Value;
    }

    private void LoadReModCore(out Assembly loadedAssembly)
    {
        byte[] bytes;
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
        catch (BadImageFormatException)
        {
            loadedAssembly = null;
        }
        loadedAssembly = null;
    }
}