using System;
using UnityEngine;

namespace ShortCuts.Tabs;

public static class Notifications
{
    public static void AddListener()
    {
        UI.NotificationsTabButton.onClick.AddListener(new Action(() =>
        {
            if (Time.realtimeSinceStartup - Main.Doubleclicktime < 0.2f)
            {
                Actions.DoubleClickHandler(Main.NotificationsAction.Value);
            }

        }));
    }
}