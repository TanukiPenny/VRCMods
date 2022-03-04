using System;
using System.Collections;
using System.Linq;
using MelonLoader;
using ReMod.Core.UI.QuickMenu;
using ReMod.Core.UI.Wings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.UI.Core;
using Object = UnityEngine.Object;

namespace MLConsoleViewer;

public static class UI
{
    private static ReCategoryPage _consoleTab;
    private static GameObject _mlMenu, _consolePrefab;
    public static TextMeshProUGUI Text;
    private static ScrollRect _scrollRect;
    private static ReMirroredWingMenu mlcvWingMenu;

    public static IEnumerator OnQuickMenu()
    {
        while (UIManager.prop_UIManager_0 == null) yield return null;
        while (Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
        BuildTab();
    }

    private static void BuildTab()
    {
        _consoleTab = new ReCategoryPage("MLConsoleViewer", true);
        /*ConsoleTabMenu = ConsoleTab.AddCategory("MLConsoleViewer", false);
        ConsoleTabMenu.AddToggle("Disable auto scroll", "Disables auto scroll on new log", b => 
        {
            Main.autoElastic.Value = b;
        },Main.autoElastic.Value);*/
        ReTabButton.Create("MLConsoleViewer", "Open MLConsoleViewer", "MLConsoleViewer", BundleManager.ConsoleImg);
        _mlMenu = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_MLConsoleViewer/");
        _consolePrefab = Object.Instantiate(BundleManager.ConsolePrefab, _mlMenu.transform);
        _consolePrefab.transform.localPosition = new Vector3(0, -42, 0);
        Tools.SetLayerRecursively(_consolePrefab, LayerMask.NameToLayer("InternalUI"));
        _scrollRect = _consolePrefab.GetComponentInChildren<ScrollRect>(true);
        Text = _consolePrefab.transform.Find("Scroll View/Viewport/Content/")
                         .GetComponentInChildren<TextMeshProUGUI>(true);
        _consoleTab.OnOpen += ResetOffsets;
        mlcvWingMenu = ReMirroredWingMenu.Create("MLCV", "Open's up the MLConsoleViewer wing menu", BundleManager.ConsoleImg);
        mlcvWingMenu.AddButton("Clear Logs", "Clears all the logs in MLCV", () =>
        {
            Text.text = "";
        }, BundleManager.CleanIcon, false);
        mlcvWingMenu.AddToggle("Auto Scroll", "Toggles Auto Scroll", b =>
        {
            Main.AutoElastic.Value = b;
            MelonPreferences.Save();
        },  Main.AutoElastic.Value);
        mlcvWingMenu.AddToggle("Time Stamps", "Toggles Time Stamps", b =>
        {
            Main.TimeStamp.Value = b;
            MelonPreferences.Save();
        }, Main.TimeStamp.Value);

        foreach (var i in ConsoleManager.Cached)
            AppendText(i);
        MelonPreferences.Save();
    }

    #region Text Appending
    private static int _lineNum;
    public static void AppendText(string txt)
    {
        if (_lineNum >= Main.MaxLines.Value)
            Text.text = GetReducedStr(Text.text, Main.MaxLines.Value);
        else
            _lineNum++;
        Text.text += txt;
    }
    private static string GetReducedStr(string content, int nthIndex)
    {
        var index = 0;
        nthIndex = content.Count(occ => occ == '\n') - nthIndex + 1;
        
        if (nthIndex < 0)
            return content;
        
        for (; nthIndex != 0; nthIndex--)
            index = content.IndexOf('\n', index) + 1;
        
        return content.Substring(index);
    }
    #endregion
    
    #region ResetOffsets
    private static bool _fired;
    public static void ResetOffsets()
    {
        if (!_fired) MelonCoroutines.Start(ResetOffsetsCoroutine());
    }
    private static IEnumerator ResetOffsetsCoroutine()
    {
        _scrollRect.movementType = ScrollRect.MovementType.Elastic;
        
        yield return new WaitForSeconds(.5f);
        
        _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        _fired = false;
    }
    #endregion
}