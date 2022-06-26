using System;
using UnityEngine;

namespace ShortCuts.Tabs;

public static class Here
{
    public static void AddListener()
    {
        UI.HereTabButton.onClick.AddListener(new Action(() =>
        {
            if (Time.realtimeSinceStartup - Main.doubleclicktime < 0.02f)
            {
                Actions.DoubleClickHandler(Main.HereAction.Value);
            }

        }));
    }
}