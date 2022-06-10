using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using MelonLoader;
using ReMod.Core.Managers;
using ReMod.Core.UI.QuickMenu;
using ReMod.Core.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI;
using Pointer = System.IntPtr;

namespace ShortCuts;

public static class UI
{
    
    public static IEnumerator UIInit()
    {
        while (GameObject.Find("UserInterface").GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true) == null)
            yield return null;
        GetMainCamera();
        GetVolumeSlider();
        GetTabButtons();
        AddListeners();
        CreateReModTabMenu();
        Main.Log.Msg("Set up successful");
    }

    public static Button LaunchPadTabButton, NotificationsTabButton, HereTabButton, CameraTabButton, AudioSettingsTabButton, SettingsTabButton;
    private static void GetTabButtons()
    {
        LaunchPadTabButton = FindObject("Page_Dashboard").GetComponent<Button>();
        NotificationsTabButton = FindObject("Page_Notifications").GetComponent<Button>();
        HereTabButton = FindObject("Page_Here").GetComponent<Button>();
        CameraTabButton = FindObject("Page_Camera").GetComponent<Button>();
        AudioSettingsTabButton = FindObject("Page_AudioSettings").GetComponent<Button>();
        SettingsTabButton = FindObject("Page_Settings").GetComponent<Button>();
    }

    public static Slider MasterAudioSlider;
    private static void GetVolumeSlider()
    {
        MasterAudioSlider = FindObject("VolumeSlider_Master").GetComponentInChildren<Slider>();
    }
    
    public static Camera MainCamera;
    private static void GetMainCamera()
    {
        MainCamera = GameObject
            .Find(
                "_Application/TrackingVolume/TrackingSteam(Clone)/SteamCamera/[CameraRig]/Neck/Camera (head)/Camera (eye)")
            .GetComponent<Camera>();
    }

    private static void AddListeners()
    {
        Tabs.LaunchPad.AddListener();
        Tabs.Notifications.AddListener();
        Tabs.Here.AddListener();
        Tabs.Camera.AddListener();
        Tabs.AudioSettings.AddListener();
        Tabs.Settings.AddListener();
    }

    private static ReCategoryPage ShortCutsTab;
    private static ReMenuCategory ShortCutsConfig;
    private static ReRadioTogglePage LaunchPadRadio, NotificationsRadio, HereRadio, CameraRadio, AudioSettingsRadio, SettingsRadio;
    private static void CreateReModTabMenu()
    {
        ShortCutsTab = new ReCategoryPage("ShortCuts", true);
        ReTabButton.Create("ShortCuts", "Open ShortCuts", "ShortCuts", ResourceManager.GetSprite("ShortCuts.shortcuts"));
        ShortCutsConfig = ShortCutsTab.AddCategory("ShortCuts Config", false);
        
        LaunchPadRadio = MakePage(Main.LaunchPadAction);
        HereRadio = MakePage(Main.HereAction);
        CameraRadio = MakePage(Main.CameraAction);
        AudioSettingsRadio = MakePage(Main.AudioSettingsAction);
        SettingsRadio = MakePage(Main.SettingsAction);
        NotificationsRadio = MakePage(Main.NotificationsAction);

        ShortCutsConfig.AddButton("Launch Pad Tab", "Open Launch Pad Tab config menu", () => LaunchPadRadio.Open((int)Main.LaunchPadAction.Value), ResourceManager.GetSprite("ShortCuts.launchpad"));
        ShortCutsConfig.AddButton("Notifications Tab", "Open Notifications Tab config menu",() => NotificationsRadio.Open((int)Main.NotificationsAction.Value),  ResourceManager.GetSprite("ShortCuts.notifications"));
        ShortCutsConfig.AddButton("Here Tab", "Open Here Tab config menu", () => HereRadio.Open((int)Main.HereAction.Value), ResourceManager.GetSprite("ShortCuts.here"));
        ShortCutsConfig.AddSpacer();
        ShortCutsConfig.AddButton("Camera Tab", "Open Camera Tab config menu", () => CameraRadio.Open((int)Main.CameraAction.Value), ResourceManager.GetSprite("ShortCuts.camera"));
        ShortCutsConfig.AddButton("Audio Settings Tab", "Open Audio Settings Tab config menu", () => AudioSettingsRadio.Open((int)Main.AudioSettingsAction.Value), ResourceManager.GetSprite("ShortCuts.audio"));
        ShortCutsConfig.AddButton("Settings Tab", "Open Settings Tab config menu", () => SettingsRadio.Open((int)Main.SettingsAction.Value), ResourceManager.GetSprite("ShortCuts.settings"));
        ShortCutsConfig.AddSpacer();
    }

    //Found this on stackoverflow forms: https://stackoverflow.com/questions/44456133/find-inactive-gameobject-by-name-tag-or-layer
    private static GameObject FindObject(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
    
    public static void CacheIcons()
    {
        //https://github.com/RequiDev/ReModCE/blob/master/ReModCE/ReMod.cs
        var ourAssembly = Assembly.GetExecutingAssembly();
        var resources = ourAssembly.GetManifestResourceNames();
        foreach (var resource in resources)
        {
            if (!resource.EndsWith(".png"))
                continue;

            var stream = ourAssembly.GetManifestResourceStream(resource);

            var ms = new MemoryStream();
            stream.CopyTo(ms);
            var resourceName = Regex.Match(resource, @"([a-zA-Z\d\-_]+)\.png").Groups[1].ToString();
            ResourceManager.LoadSprite("ShortCuts", resourceName, ms.ToArray());
        }
    }

    private static ReRadioTogglePage MakePage(MelonPreferences_Entry<Actions.Action> preferencesEntry)
    {
        var radiopage = new ReRadioTogglePage(preferencesEntry.DisplayName);
        radiopage.OnSelect += o => preferencesEntry.Value = (Actions.Action) o;
        foreach (int a in Enum.GetValues(typeof(Actions.Action)))
        {
            var name = ((Actions.Action) a).ToString().Replace("_", " ");
            radiopage.AddItem(name, (int)a);
        }

        return radiopage;
    }
}