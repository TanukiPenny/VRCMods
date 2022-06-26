using System;
using UnityEngine;

namespace ShortCuts.Tabs;

public static class Here
{
    public static void AddListener()
    {
        UI.HereTabButton.onClick.AddListener(new Action(() =>
        {
            if (Time.realtimeSinceStartup - Main.Doubleclicktime < 0.2f)
            {
                Actions.DoubleClickHandler(Main.HereAction.Value);
            }

        }));
    }
}