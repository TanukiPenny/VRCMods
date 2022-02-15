using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace URLTools
{
    public class WorldFunc
    {
        public static void CopyWorldURL()
        {
            string worldid = RoomManager.field_Internal_Static_ApiWorld_0.id;

            GUIUtility.systemCopyBuffer = $"https://vrchat.com/home/world/{worldid}";
            Main.log.Msg($"World URL copied");
        }
        public static void OpenWorldPage()
        {
            string worldid = RoomManager.field_Internal_Static_ApiWorld_0.id;

            Process.Start($"https://vrchat.com/home/world/{worldid}");
            Main.log.Msg($"World page opened");
        }
    }
}
