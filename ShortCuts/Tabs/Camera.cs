using System;
using UnityEngine;

namespace ShortCuts.Tabs;

public static class Camera
{
    public static void AddListener()
    {
        UI.CameraTabButton.onClick.AddListener(new Action(() =>
        {
            if (Time.realtimeSinceStartup - Main.Doubleclicktime < 0.2f)
            {
                Actions.DoubleClickHandler(Main.CameraAction.Value);
            }

        }));
    }
}