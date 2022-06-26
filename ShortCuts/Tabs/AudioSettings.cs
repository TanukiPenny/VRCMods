using System;
using UnityEngine;

namespace ShortCuts.Tabs;

public static class AudioSettings
{
    public static void AddListener()
    {
        UI.AudioSettingsTabButton.onClick.AddListener(new Action(() =>
        {
            if (Time.realtimeSinceStartup - Main.Doubleclicktime < 0.2f)
            {
                Actions.DoubleClickHandler(Main.AudioSettingsAction.Value);
            }

        }));
    }
}