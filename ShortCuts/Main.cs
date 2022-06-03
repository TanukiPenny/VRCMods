using System;
using System.Net;
using System.Reflection;
using MelonLoader;

namespace ShortCuts;

public static class BuildShit
{
    public const string Name = "ShortCuts";
    public const string Author = "Penny";
    public const string Version = "1.0.0";
    public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
    public const string Description = "A standalone mod to add double click short cuts on all your QM tabs!";
}
public class Main : MelonMod
{
    internal static readonly MelonLogger.Instance Log = new(BuildShit.Name, ConsoleColor.Green);

    internal static MelonPreferences_Category Category = MelonPreferences.CreateCategory(BuildShit.Name, BuildShit.Name);
        
    public static MelonPreferences_Entry<Actions.Action> LaunchPadAction;
    public static MelonPreferences_Entry<Actions.Action> NotificationsAction;
    public static MelonPreferences_Entry<Actions.Action> HereAction;
    public static MelonPreferences_Entry<Actions.Action> CameraAction;
    public static MelonPreferences_Entry<Actions.Action> AudioSettingsAction;
    public static MelonPreferences_Entry<Actions.Action> SettingsAction;

    private static int _scenesLoaded = 0;

    public override void OnApplicationStart()
    {
        LoadReModCore(out _);
        LaunchPadAction = Category.CreateEntry("LaunchPad", Actions.Action.None, "LaunchPad");
        NotificationsAction = Category.CreateEntry("Notifications", Actions.Action.None, "Notifications");
        HereAction = Category.CreateEntry("Here", Actions.Action.None, "Here");
        CameraAction = Category.CreateEntry("Camera", Actions.Action.None, "Camera");
        AudioSettingsAction = Category.CreateEntry("AudioSettings", Actions.Action.None, "Audio Settings");
        SettingsAction = Category.CreateEntry("Settings", Actions.Action.None, "Settings");
        Log.Msg("ShortCuts loaded successfully!");
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (_scenesLoaded <= 2)
        {
            _scenesLoaded++;
            if (_scenesLoaded == 2)
            {
                MelonCoroutines.Start(UI.UIInit());
            }
        }
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