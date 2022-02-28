using System;
using System.Net;
using System.Reflection;
using MelonLoader;
using BuildShit = MLConsoleViewer.BuildShit;
using Main = MLConsoleViewer.Main;

#region Info & Namespace
[assembly: AssemblyDescription(BuildShit.Description)]
[assembly: AssemblyCopyright($"Created by {BuildShit.Author}, Copyright © 2022")]
[assembly: MelonInfo(typeof(Main), BuildShit.Name, BuildShit.Version, BuildShit.Author, BuildShit.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonPriority(Int32.MinValue)]
[assembly: MelonColor(ConsoleColor.DarkMagenta)]
[assembly: MelonOptionalDependencies("ReMod.Core")]

namespace MLConsoleViewer;

public static class BuildShit
{
    public const string Name = "MLConsoleViewer";
    public const string Author = "Penny & Davi";
    public const string Version = "1.0.0";
    public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
    public const string Description = "A standalone mod that adds a tab to your quick menu that has a simple copy of your console!";
}
#endregion

public class Main : MelonMod
{
    internal static readonly MelonLogger.Instance log = new(BuildShit.Name, ConsoleColor.Cyan);
    private static MelonPreferences_Category _mlConsoleViewer;
    private static MelonPreferences_Entry<int> fontSize;
    public static MelonPreferences_Entry<int> maxLines;
    public static MelonPreferences_Entry<bool> timeStamp;
    public static MelonPreferences_Entry<bool> autoElastic;
    public override void OnApplicationStart()
    {
        LoadReModCore(out _);
        BundleManager.Init();
        ConsoleManager.AttachTrackers();
        _mlConsoleViewer = MelonPreferences.CreateCategory("MLConsoleViewer", "MLConsoleViewer");
        fontSize = _mlConsoleViewer.CreateEntry("fontSize", 20, "Font Size",
            "Font size of the text in your console tab");
        maxLines = _mlConsoleViewer.CreateEntry("maxLines", 150, "Max Displayed Lines",
            "Defines the limit in which your console starts discarding old lines");
        timeStamp = _mlConsoleViewer.CreateEntry("timeStamp", true, "Time Stamp",
            "Sets whether logs show time stamps or not");
        autoElastic = _mlConsoleViewer.CreateEntry("autoElastic", true, "Elastic on new log",
            "Sets whether logs set scrollrect to be elastic");
        log.Msg("MLConsoleViewer Loaded");
    }
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (buildIndex == 1)
            MelonCoroutines.Start(UI.OnQuickMenu());
    }

    public override void OnPreferencesSaved()
    {
        if (UI.text == null)
            return;
        UI.text.fontSize = fontSize.Value;
    }

    private void LoadReModCore(out Assembly loadedAssembly)
    {
        byte[] bytes;
        var wc = new WebClient();
            
        try
        {
            bytes = wc.DownloadData($"https://github.com/RequiDev/ReMod.Core/releases/latest/download/ReMod.Core.dll");
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