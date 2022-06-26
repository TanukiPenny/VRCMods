using System;
using UnityEngine;

namespace ShortCuts.Tabs;

public static class Settings
{
    public static void AddListener()
    {
        UI.SettingsTabButton.onClick.AddListener(new Action(() =>
        {
            if (Time.realtimeSinceStartup - Main.Doubleclicktime < 0.2f)
            {
                Actions.DoubleClickHandler(Main.SettingsAction.Value);
            }

        }));
    }
}