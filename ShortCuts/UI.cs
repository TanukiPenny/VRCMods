using HarmonyLib;
using MelonLoader;
using ReMod.Core.Managers;
using ReMod.Core.UI.QuickMenu;
using ReMod.Core.VRChat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
using VRC.UI.Elements;
using static ShortCuts.Main;
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
        PatchPointerDown();
        CreateReModTabMenu();
        Log.Msg("Set up successful");
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

    private static Dictionary<IntPtr, MelonPreferences_Entry<Actions.Action>> ActionDict;

    private static void PatchPointerDown()
    {
        ActionDict = new Dictionary<IntPtr, MelonPreferences_Entry<Actions.Action>>()
        {
            { AudioSettingsTabButton.gameObject.Pointer, AudioSettingsAction },
            { CameraTabButton.gameObject.Pointer, CameraAction },
            { HereTabButton.gameObject.Pointer, HereAction },
            { LaunchPadTabButton.gameObject.Pointer, HereAction },
            { NotificationsTabButton.gameObject.Pointer, NotificationsAction },
            { SettingsTabButton.gameObject.Pointer, SettingsAction }

        };

        var orig = AccessTools.Method(typeof(Button), "OnPointerDown");
        var patch = AccessTools.Method(typeof(UI), "OnPointerDownPatch");

        MyHarmony.Patch(orig, null, new HarmonyMethod(patch));
    }

    public static GameObject lastClicked;
    public static float clicked = 0;
    public static float clickTime = -50;
    public static float clickDelay = 0.5f;


    public static void OnPointerDownPatch(PointerEventData eventData)
    {
        if (lastClicked != null && lastClicked.Pointer != eventData.selectedObject.Pointer)
        {
            clicked = 0;
            clickTime = 0;
        }

        lastClicked = eventData.selectedObject;
        if (!ActionDict.TryGetValue(lastClicked.Pointer, out var action) ||
        action.Value == Actions.Action.None)
            return;
        // Because Unity is dumb, the first "double click" actually has to be a "triple click", fuck it.
        clicked++;
        if (((clicked == 1 && clickTime != 0) || clicked > 1) && Time.time - clickTime < clickDelay)
        {
            clicked = 0;
            clickTime = 0;
            Actions.DoubleClickHandler(action.Value);
        }
        else if (clicked <= 1)
            clickTime = Time.time;
        else if (clicked > 2 || Time.time - clickTime > 1)
        {
            clicked = 0;
            clickTime = 0;
        }
    }

    private static ReCategoryPage ShortCutsTab;
    private static ReMenuCategory ShortCutsConfig;
    private static ReRadioTogglePage LaunchPadRadio, NotificationsRadio, HereRadio, CameraRadio, AudioSettingsRadio, SettingsRadio;
    public static ReTabButton ShortsTabButton;

    private static void CreateReModTabMenu()
    {
        ShortCutsTab = new ReCategoryPage("ShortCuts", true);
        ShortsTabButton = ReTabButton.Create("ShortCuts", "Open ShortCuts", "ShortCuts", ResourceManager.GetSprite("ShortCuts.shortcuts"));
        ShortsTabButton.GameObject.SetActive(Showtab.Value);
        ShortCutsConfig = ShortCutsTab.AddCategory("ShortCuts Config", false);

        LaunchPadRadio = MakePage(LaunchPadAction);
        HereRadio = MakePage(HereAction);
        CameraRadio = MakePage(CameraAction);
        AudioSettingsRadio = MakePage(AudioSettingsAction);
        SettingsRadio = MakePage(SettingsAction);
        NotificationsRadio = MakePage(NotificationsAction);

        ShortCutsConfig.AddButton("Launch Pad Tab", "Open Launch Pad Tab config menu", () => LaunchPadRadio.Open((int)LaunchPadAction.Value), ResourceManager.GetSprite("ShortCuts.launchpad"));
        ShortCutsConfig.AddButton("Notifications Tab", "Open Notifications Tab config menu", () => NotificationsRadio.Open((int)NotificationsAction.Value), ResourceManager.GetSprite("ShortCuts.notifications"));
        ShortCutsConfig.AddButton("Here Tab", "Open Here Tab config menu", () => HereRadio.Open((int)HereAction.Value), ResourceManager.GetSprite("ShortCuts.here"));
        ShortCutsConfig.AddSpacer();

        ShortCutsConfig.AddButton("Camera Tab", "Open Camera Tab config menu", () => CameraRadio.Open((int)CameraAction.Value), ResourceManager.GetSprite("ShortCuts.camera"));
        ShortCutsConfig.AddButton("Audio Settings Tab", "Open Audio Settings Tab config menu", () => AudioSettingsRadio.Open((int)AudioSettingsAction.Value), ResourceManager.GetSprite("ShortCuts.audio"));
        ShortCutsConfig.AddButton("Settings Tab", "Open Settings Tab config menu", () => SettingsRadio.Open((int)SettingsAction.Value), ResourceManager.GetSprite("ShortCuts.settings"));
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
        radiopage.OnSelect += o => preferencesEntry.Value = (Actions.Action)o;
        foreach (int a in Enum.GetValues(typeof(Actions.Action)))
        {
            var name = ((Actions.Action)a).ToString().Replace("_", " ");
            radiopage.AddItem(name, (int)a);
        }

        radiopage.OnClose += () =>
        { 
            MelonPreferences.Save();
        };
        return radiopage;
    }

    private static void CameraTabGoBrrrr()
    {
        var aaa = tabButtons.FindChild("Page_Camera").gameObject.GetComponents<MonoBehaviour>();
        Object.DestroyImmediate(aaa[6]);
    }
}