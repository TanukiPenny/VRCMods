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
        UI.text.text += $"[{callingMod}] {logText}\n";
    }
    public static void OnMsg(ConsoleColor melonColor, ConsoleColor txtColor, string callingMod, string logText)
    {
        AddMsg(melonColor, txtColor, callingMod, logText);
    }
}
