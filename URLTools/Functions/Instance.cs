using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace URLTools
{
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
                Main.log.Warning("Will not copy instance URL if private!");
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
                Main.log.Warning("Will not open instance page if private!");
            }
        }
    }
}
