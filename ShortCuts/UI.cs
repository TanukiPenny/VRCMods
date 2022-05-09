using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Il2CppSystem.Reflection;
using MelonLoader;
using UnhollowerRuntimeLib.XrefScans;
using VRC.Core;
using VRC.UI.Core;
using VRC.UI.Elements;
using Pointer = System.IntPtr;

namespace ShortCuts;

public static class UI
{
    public static Button LaunchPad, Notifications, Here, Camera, AudioSettings, Settings;
    public static Camera MainCamera;
    public static Slider MasterAudioSlider;
    private static VRC.UI.Elements.QuickMenu _quickMenuInstance;
    public static Button.ButtonClickedEvent SettingsExpand;


    public static IEnumerator UIInit()
    {
        while (GameObject.Find("UserInterface").GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true) == null)
            yield return null;
        MainCamera = GameObject
            .Find(
                "_Application/TrackingVolume/TrackingSteam(Clone)/SteamCamera/[CameraRig]/Neck/Camera (head)/Camera (eye)")
            .GetComponent<Camera>();
        MasterAudioSlider = FindObject("VolumeSlider_Master").GetComponentInChildren<Slider>();
        SettingsExpand = Instance.field_Public_Transform_0
            .Find("Window/QMParent/Menu_Settings/QMHeader_H1/RightItemContainer/Button_QM_Expand")
            .GetComponent<Button>().onClick;
        GetTabButtons();
        AddListeners();
    }

    private static void GetTabButtons()
    {
        LaunchPad = FindObject("Page_Dashboard").GetComponent<Button>();
        Notifications = FindObject("Page_Notifications").GetComponent<Button>();
        Here = FindObject("Page_Here").GetComponent<Button>();
        Camera = FindObject("Page_Camera").GetComponent<Button>();
        AudioSettings = FindObject("Page_AudioSettings").GetComponent<Button>();
        Settings = FindObject("Page_Settings").GetComponent<Button>();
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

    //This method is from ReMod.Core, thanks Requi!!!
    public static VRC.UI.Elements.QuickMenu Instance
    {
        get
        {
            if (_quickMenuInstance == null)
            {
                _quickMenuInstance = GameObject.Find("UserInterface")
                    .GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true);
            }

            return _quickMenuInstance;
        }
    }
}