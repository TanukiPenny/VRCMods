using System.Diagnostics;
using ReMod.Core.VRChat;
using UnityEngine;

namespace URLTools;

public class InstanceFunc
{
    public static void CopyInstanceURL()
    {
        string worldid = RoomManager.field_Internal_Static_ApiWorld_0.id;
        string instanceid = RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceId;

        if (!instanceid.Contains("private") && (!instanceid.Contains("friends")))
        {
            GUIUtility.systemCopyBuffer = $"https://vrchat.com/home/launch?worldId={worldid}&instanceId={instanceid}";
            Main.log.Msg($"Instance URL copied");
        }
        else
        {
            QuickMenuEx.Instance.ShowAlertDialog("Oops", "This mods will not copy instance URL if it is private!");
            Main.log.Warning("This mod will not copy instance URL if it is private!");
        }
    }
    public static void OpenInstancePage()
    {
        string worldid = RoomManager.field_Internal_Static_ApiWorld_0.id;
        string instanceid = RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceId;

        if (!instanceid.Contains("private") && (!instanceid.Contains("friends")))
        {
            Process.Start($"https://vrchat.com/home/launch?worldId={worldid}&instanceId={instanceid}");
            Main.log.Msg($"Instance page opened");
        }
        else
        {
            QuickMenuEx.Instance.ShowAlertDialog("Oops", "This mod will not copy instance URL if it is private!");
            Main.log.Warning("This mod will not open instance page if private!");
        }
    }
}