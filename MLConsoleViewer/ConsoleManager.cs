using System;
using System.Linq;
using MelonLoader;
using UnityEngine;

namespace MLConsoleViewer;

public class ConsoleManager
{
    public static void AttachTrackers()
    {
        MelonLogger.MsgCallbackHandler += OnMsg;
    }

    public static void AddMsg(ConsoleColor melonColor, ConsoleColor txtColor, string callingMod, string logText)
    {   
        UI.text.text += $"[<color=green>{DateTime.Now.AddMilliseconds(-1.0).ToString("HH:mm:ss.fff")}</color>]" +
                        $" [<color={melonColor.ToString()}>{callingMod}</color>]" +
                        $" <color={txtColor.ToString()}>{logText}</color>\n";
    }
    public static void OnMsg(ConsoleColor melonColor, ConsoleColor txtColor, string callingMod, string logText)
    {
        AddMsg(melonColor, txtColor, callingMod, logText);
    }
}
