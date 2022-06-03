using System;
using System.Linq;
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
    public const string Version = "1.1.5";
    public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
    public const string Description = "A standalone mod that adds a tab to your quick menu that has a simple copy of your console!";
}
#endregion

public class Main : MelonMod
{
    public static readonly MelonLogger.Instance Log = new(BuildShit.Name, ConsoleColor.Cyan);
    private static MelonPreferences_Category _mlConsoleViewer;
    private static MelonPreferences_Entry<int> _fontSize;
    public static MelonPreferences_Entry<int> MaxLines;
    public static MelonPreferences_Entry<int> MaxChars;
    public static MelonPreferences_Entry<bool> TimeStamp;
    public static MelonPreferences_Entry<bool> AutoElastic;
    public override void OnApplicationStart()
    {
        LoadReModCore(out _);
        BundleManager.Init();
        ConsoleManager.AttachTrackers();
        _mlConsoleViewer = MelonPreferences.CreateCategory("MLConsoleViewer", "MLConsoleViewer");
        _fontSize = _mlConsoleViewer.CreateEntry("fontSize", 20, "Font Size",
            "Font size of the text in your console tab");
        MaxLines = _mlConsoleViewer.CreateEntry("maxLines", 150, "Max Displayed Lines",
            "Defines the limit in which your console starts discarding old lines");
        MaxChars = _mlConsoleViewer.CreateEntry("maxChars", 1000, "Max Characters per Log",
            "Defines the limit of characters per log, printing only part of it if length is greater (will break if way too high because TextMesh limits)");
        TimeStamp = _mlConsoleViewer.CreateEntry("timeStamp", true, "Time Stamp",
            "Sets whether logs show time stamps or not");
        AutoElastic = _mlConsoleViewer.CreateEntry("autoElastic", true, "Elastic on new log",
            "Sets whether logs automatically scrolls down to the bottom");
        Log.Msg("MLConsoleViewer Loaded");
    }
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (buildIndex == 1)
            MelonCoroutines.Start(UI.OnQuickMenu());
    }

    public override void OnPreferencesSaved()
    {
        if (UI.Text == null)
            return;
        UI.Text.fontSize = _fontSize.Value;
    }

    private void LoadReModCore(out Assembly loadedAssembly)
    {
        try
        {
            loadedAssembly = Assembly.Load(new WebClient().DownloadData("https://github.com/RequiDev/ReMod.Core/releases/latest/download/ReMod.Core.dll"));
        }
        catch (Exception e)
        {
            MelonLogger.Error($"Unable to Load ReModCore Dependency: {e}");
            loadedAssembly = null;
        }
    }
}