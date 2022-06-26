using System;
using System.Collections;
using MelonLoader;
using UnityEngine;

namespace ShortCuts;

public static class BuildShit
{
    public const string Name = "ShortCuts";
    public const string Author = "Penny";
    public const string Version = "1.1.0";
    public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
    public const string Description = "A standalone mod to add double click short cuts on all your QM tabs!";
}
public class Main : MelonMod
{
    internal static readonly MelonLogger.Instance Log = new(BuildShit.Name, ConsoleColor.DarkYellow);

    internal static MelonPreferences_Category Category = MelonPreferences.CreateCategory(BuildShit.Name, BuildShit.Name);

    public static MelonPreferences_Entry<bool> Showtab;
        
    public static MelonPreferences_Entry<Actions.Action> LaunchPadAction;
    public static MelonPreferences_Entry<Actions.Action> NotificationsAction;
    public static MelonPreferences_Entry<Actions.Action> HereAction;
    public static MelonPreferences_Entry<Actions.Action> CameraAction;
    public static MelonPreferences_Entry<Actions.Action> AudioSettingsAction;
    public static MelonPreferences_Entry<Actions.Action> SettingsAction;

    public static VRCInput UiSelectRight, UiSelectLeft;
    
    
    public override void OnApplicationStart()
    {
        LaunchPadAction = Category.CreateEntry("LaunchPad", Actions.Action.None, "LaunchPad");
        NotificationsAction = Category.CreateEntry("Notifications", Actions.Action.None, "Notifications");
        HereAction = Category.CreateEntry("Here", Actions.Action.None, "Here");
        CameraAction = Category.CreateEntry("Camera", Actions.Action.None, "Camera");
        AudioSettingsAction = Category.CreateEntry("AudioSettings", Actions.Action.None, "Audio Settings");
        SettingsAction = Category.CreateEntry("Settings", Actions.Action.None, "Settings");
        Showtab = Category.CreateEntry("Show Tab", true, "Show QM Tab");
        UI.CacheIcons();
        Log.Msg("ShortCuts loaded successfully!");
    }

    private static int _scenesLoaded = 0;
    
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

    public override void OnPreferencesSaved()
    {
        if (UI.ShortsTabButton != null)
        {
            UI.ShortsTabButton.GameObject.SetActive(Main.Showtab.Value);
        }
    }

    private static bool clicked;
    public static float doubleclicktime;
    public static bool clicking;

    public override void OnUpdate()
    {
        if (UiSelectRight == null || UiSelectLeft == null)
        {
            return;
        }

        if (UiSelectRight.field_Private_Boolean_0 || UiSelectLeft.field_Private_Boolean_0)
        {
            clicking = true;
        }
        else
        {
            clicking = false;
        }

        switch (clicking)
        {
            case true:
                clicked = true;
                break;
            case false when clicked:
                clicked = false;
                if (IsDoubleClick())
                {
                    doubleclicktime = Time.realtimeSinceStartup;
                }
                
                break;
        }
    }
    
    private static float _lastTimeClicked = 0;
    private const float Threshold = 0.5f;
    private const bool MultipleInRow = false;
    
    private static bool IsDoubleClick()
    {
        if (_lastTimeClicked == 0)
        {
            _lastTimeClicked = Time.time;
            return false;
        }
        if (Time.time - _lastTimeClicked <= Threshold)
        {
            _lastTimeClicked = MultipleInRow ? Time.time : Threshold * 2f;
            return true;
        }
        _lastTimeClicked = Time.time;
        return false;
    }
}