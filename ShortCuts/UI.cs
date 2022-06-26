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
using UnityEngine.XR;
using VRC.UI.Elements;
using Object = UnityEngine.Object;

namespace ShortCuts;

public static class UI
{

    public static UIPage CameraUIPage;
    
    public static IEnumerator UIInit()
    {
        while (GameObject.Find("UserInterface").GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true) == null)
            yield return null;
        CameraUIPage = QuickMenuEx.Instance.transform.Find("Container/Window/QMParent/Menu_Camera").GetComponent<UIPage>();
        GetMainCamera();
        GetVolumeSlider();
        GetTabButtons();
        CameraTabGoBrrrr();
        AddListeners();
        GetVRCInput();
        CreateReModTabMenu();
        Main.Log.Msg("Set up successful");
    }

    public static Button LaunchPadTabButton, NotificationsTabButton, HereTabButton, CameraTabButton, AudioSettingsTabButton, SettingsTabButton;
    public static Transform tabButtons;
    private static void GetTabButtons()
    {
        tabButtons = QuickMenuEx.Instance.transform.Find("Container/Window/Page_Buttons_QM/HorizontalLayoutGroup");
        LaunchPadTabButton = tabButtons.FindChild("Page_Dashboard").GetComponent<Button>();
        NotificationsTabButton = tabButtons.FindChild("Page_Notifications").GetComponent<Button>();
        HereTabButton = tabButtons.FindChild("Page_Here").GetComponent<Button>();
        CameraTabButton = tabButtons.FindChild("Page_Camera").GetComponent<Button>();
        AudioSettingsTabButton = tabButtons.FindChild("Page_AudioSettings").GetComponent<Button>();
        SettingsTabButton = tabButtons.FindChild("Page_Settings").GetComponent<Button>();
    }

    public static Slider MasterAudioSlider;
    private static void GetVolumeSlider()
    {
        MasterAudioSlider = QuickMenuEx.Instance.transform.Find("Container/Window/QMParent/Menu_AudioSettings/Content/Audio/VolumeSlider_Master").GetComponentInChildren<Slider>();
    }
    
    public static Camera MainCamera;
    private static void GetMainCamera()
    {
        if (XRDevice.isPresent) return;
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
    public static ReTabButton ShortsTabButton;
    
    private static void CreateReModTabMenu()
    {
        ShortCutsTab = new ReCategoryPage("ShortCuts", true);
        ShortsTabButton = ReTabButton.Create("ShortCuts", "Open ShortCuts", "ShortCuts", ResourceManager.GetSprite("ShortCuts.shortcuts"));
        ShortsTabButton.GameObject.SetActive(Main.Showtab.Value);
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

    private static void CameraTabGoBrrrr()
    {
        var aaa = tabButtons.FindChild("Page_Camera").gameObject.GetComponents<MonoBehaviour>();
        Object.DestroyImmediate(aaa[6]);
    }

    private static void GetVRCInput()
    {
        var VRCInputs = VRCInputManager.field_Private_Static_Dictionary_2_String_VRCInput_0;
        foreach (var a in VRCInputs)
        {
            switch (a.value.prop_String_0)
            {
                case "UiSelectLeft":
                    Main.UiSelectLeft = a.value;
                    break;
                case "UiSelectRight":
                    Main.UiSelectRight = a.value;
                    break;
            }
        }
    }
}