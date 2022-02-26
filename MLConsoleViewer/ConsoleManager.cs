using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace MLConsoleViewer;

public static class ConsoleManager
{
    public static void AttachTrackers()
    {
        MelonLogger.MsgCallbackHandler += OnMsg;
    }
    private static void OnMsg(ConsoleColor melonColor, ConsoleColor txtColor, string callingMod, string logText)
    {
        AddMsg(melonColor, txtColor, callingMod, logText);
    }
    private static void AddMsg(ConsoleColor melonColor, ConsoleColor txtColor, string callingMod, string logText)
    {   
        UI.text.text += $"[<color=green>{DateTime.Now.AddMilliseconds(-1.0).ToString("HH:mm:ss.fff")}</color>]" +
                        $" [<color={HexStrings[melonColor]}>{callingMod}</color>]" +
                        $" <color={HexStrings[txtColor]}>{logText}</color>\n";
    }
    private static readonly Hashtable HexStrings = new()
    {
        {ConsoleColor.Black, "#000000"},
        {ConsoleColor.DarkBlue, "#00008b"},
        {ConsoleColor.DarkGreen, "#008b00"},
        {ConsoleColor.DarkCyan, "#008b8b"},
        {ConsoleColor.DarkRed, "#8b0000"},
        {ConsoleColor.DarkMagenta, "#8b008b"},
        {ConsoleColor.DarkYellow, "#8b8b00"},
        {ConsoleColor.Gray, "#c0c0c0"},
        {ConsoleColor.DarkGray, "#8b8b8b"},
        {ConsoleColor.Blue, "#0000ff"},
        {ConsoleColor.Green, "#00ff00"},
        {ConsoleColor.Cyan, "#00ffff"},
        {ConsoleColor.Red, "#ff0000"},
        {ConsoleColor.Magenta, "#ff00ff"},
        {ConsoleColor.Yellow, "#ffff00"},
        {ConsoleColor.White, "#ffffff"}
    };
}
