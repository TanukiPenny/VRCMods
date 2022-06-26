using System;
using UnityEngine;

namespace ShortCuts.Tabs;

public static class Notifications
{
    public static void AddListener()
    {
        UI.NotificationsTabButton.onClick.AddListener(new Action(() =>
        {
            if (Time.realtimeSinceStartup - Main.doubleclicktime < 0.02f)
            {
                Actions.DoubleClickHandler(Main.NotificationsAction.Value);
            }

        }));
    }
}