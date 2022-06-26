using System;
using UnityEngine;

namespace ShortCuts.Tabs;

public static class LaunchPad
{
    public static void AddListener()
    {
        UI.LaunchPadTabButton.onClick.AddListener(new Action(() =>
        {
            if (Time.realtimeSinceStartup - Main.doubleclicktime < 0.02f)
            {
                Actions.DoubleClickHandler(Main.LaunchPadAction.Value);
            }

        }));
    }
}