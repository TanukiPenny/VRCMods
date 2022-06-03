using System;
using System.Collections;
using MelonLoader;
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
        GetTabIcons();
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

    private static Sprite LaunchPadTabImage, NotificationsTabImage, HereTabImage, CameraTabImage, AudioSettingsTabImage, SettingsTabImage;
    private static void GetTabIcons()
    {
        
        LaunchPadTabImage = LaunchPadTabButton.GetComponentInChildren<Image>().sprite;
        NotificationsTabImage = LaunchPadTabButton.GetComponentInChildren<Image>().sprite;
        HereTabImage = LaunchPadTabButton.GetComponentInChildren<Image>().sprite;
        CameraTabImage = LaunchPadTabButton.GetComponentInChildren<Image>().sprite;
        AudioSettingsTabImage = LaunchPadTabButton.GetComponentInChildren<Image>().sprite;
        SettingsTabImage = LaunchPadTabButton.GetComponentInChildren<Image>().sprite;
    }

    private static ReCategoryPage ShortCutsTab;
    private static ReMenuCategory ShortCutsConfig;
    private static ReRadioTogglePage LaunchPadRadio, NotificationsRadio, HereRadio, CameraRadio, AudioSettingsRadio, SettingsRadio;
    private static void CreateReModTabMenu()
    {
        ShortCutsTab = new ReCategoryPage("ShortCuts", true);
        ReTabButton.Create("ShortCuts", "Open ShortCuts", "ShortCuts", null);
        ShortCutsConfig = ShortCutsTab.AddCategory("ShortCuts Config");
        LaunchPadRadio = new ReRadioTogglePage("Launch Pad Config");
        foreach (int a in Enum.GetValues(typeof(Actions.Action)))
        {
            LaunchPadRadio.AddItem(((Actions.Action)a).ToString(), null, () =>
            {
                Main.LaunchPadAction.Value = (Actions.Action)a;
            });
        }
        HereRadio = new ReRadioTogglePage("Here Config");
        foreach (int a in Enum.GetValues(typeof(Actions.Action)))
        {
            HereRadio.AddItem(((Actions.Action)a).ToString(), null, () =>
            {
                Main.HereAction.Value = (Actions.Action)a;
            });
        }
        CameraRadio = new ReRadioTogglePage("Camera Config");
        foreach (int a in Enum.GetValues(typeof(Actions.Action)))
        {
            CameraRadio.AddItem(((Actions.Action)a).ToString(), null, () =>
            {
                Main.CameraAction.Value = (Actions.Action)a;
            });
        }
        AudioSettingsRadio = new ReRadioTogglePage("Audio Settings Config");
        foreach (int a in Enum.GetValues(typeof(Actions.Action)))
        {
            AudioSettingsRadio.AddItem(((Actions.Action)a).ToString(), null, () =>
            {
                Main.AudioSettingsAction.Value = (Actions.Action)a;
            });
        }
        SettingsRadio = new ReRadioTogglePage("Settings Config");
        foreach (int a in Enum.GetValues(typeof(Actions.Action)))
        {
            SettingsRadio.AddItem(((Actions.Action)a).ToString(), null, () =>
            {
                Main.SettingsAction.Value = (Actions.Action)a;
            });
        }
        NotificationsRadio = new ReRadioTogglePage("Notifications Config");
        foreach (int a in Enum.GetValues(typeof(Actions.Action)))
        {
            NotificationsRadio.AddItem(((Actions.Action)a).ToString(), null, () =>
            {
                Main.NotificationsAction.Value = (Actions.Action)a;
            });
        }
        ShortCutsConfig.AddButton("Launch Pad Tab", "Open Launch Pad Tab config menu", LaunchPadRadio.Open, LaunchPadTabImage);
        ShortCutsConfig.AddButton("Notifications Tab", "Open Notifications Tab config menu", NotificationsRadio.Open, NotificationsTabImage);
        ShortCutsConfig.AddButton("Here Tab", "Open Here Tab config menu", HereRadio.Open, HereTabImage);
        ShortCutsConfig.AddSpacer();
        ShortCutsConfig.AddButton("Camera Tab", "Open Camera Tab config menu", CameraRadio.Open, CameraTabImage);
        ShortCutsConfig.AddButton("Audio Settings Tab", "Open Audio Settings Tab config menu", AudioSettingsRadio.Open, AudioSettingsTabImage);
        ShortCutsConfig.AddButton("Settings Tab", "Open Settings Tab config menu", SettingsRadio.Open, SettingsTabImage);
        ShortCutsConfig.AddSpacer();
    }

    public static void CloseQM()
    {
        QuickMenuExtensions.CloseQuickMenu(UIManagerImpl.prop_UIManagerImpl_0);
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
}