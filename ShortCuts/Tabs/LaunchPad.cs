using System;
using UnityEngine;

namespace ShortCuts.Tabs;

public static class LaunchPad
{
    public static void AddListener()
    {
        UI.LaunchPadTabButton.onClick.AddListener(new Action(() =>
        {
            if (Time.realtimeSinceStartup - Main.Doubleclicktime < 0.2f)
            {
                Actions.DoubleClickHandler(Main.LaunchPadAction.Value);
            }

        }));
    }
}