using System;
using UnityEngine;

namespace ShortCuts.Tabs;

public static class AudioSettings
{
    public static void AddListener()
    {
        UI.AudioSettingsTabButton.onClick.AddListener(new Action(() =>
        {
            if (Time.realtimeSinceStartup - Main.doubleclicktime < 0.02f)
            {
                Actions.DoubleClickHandler(Main.AudioSettingsAction.Value);
            }

        }));
    }
}