using System;
using System.Collections;
using System.Linq;
using MelonLoader;
using ReMod.Core.UI.QuickMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.UI.Core;
using Object = UnityEngine.Object;

namespace MLConsoleViewer;

public static class UI
{
    private static ReCategoryPage ConsoleTab;
    public static GameObject MLMenu, consolePrefab;
    public static TextMeshProUGUI text;
    private static ScrollRect scrollRect;

    public static IEnumerator OnQuickMenu()
    {
        while (UIManager.prop_UIManager_0 == null) yield return null;
        while (Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
        BuildTab();
    }

    private static void BuildTab()
    {
        ConsoleTab = new ReCategoryPage("MLConsoleViewer", true);
        /*ConsoleTabMenu = ConsoleTab.AddCategory("MLConsoleViewer", false);
        ConsoleTabMenu.AddToggle("Disable auto scroll", "Disables auto scroll on new log", b => 
        {
            Main.autoElastic.Value = b;
        },Main.autoElastic.Value);*/
        ReTabButton.Create("MLConsoleViewer", "Open MLConsoleViewer", "MLConsoleViewer", BundleManager.ConsoleImg);
        MLMenu = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_MLConsoleViewer/");
        consolePrefab = Object.Instantiate(BundleManager.ConsolePrefab, MLMenu.transform);
        consolePrefab.transform.localPosition = new Vector3(0, -42, 0);
        Tools.SetLayerRecursively(consolePrefab, LayerMask.NameToLayer("InternalUI"));
        scrollRect = consolePrefab.GetComponentInChildren<ScrollRect>(true);
        text = consolePrefab.transform.Find("Scroll View/Viewport/Content/")
                         .GetComponentInChildren<TextMeshProUGUI>(true);
        ConsoleTab.OnOpen += ResetOffsets;
        scrollRect.elasticity = 0;

        foreach (var i in ConsoleManager.Cached)
            AppendText(i);
    }

    #region Text Appending
    private static int lineNum;
    public static void AppendText(string txt)
    {
        if (lineNum >= Main.maxLines.Value)
            text.text = GetReducedStr(text.text, Main.maxLines.Value);
        else
            lineNum++;
        text.text += txt;
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
    private static bool fired;
    public static void ResetOffsets()
    {
        if (!fired) MelonCoroutines.Start(resetOffsetsCoroutine());
    }
    private static IEnumerator resetOffsetsCoroutine()
    {
        scrollRect.movementType = ScrollRect.MovementType.Elastic;
        
        yield return new WaitForSeconds(.5f);
        
        scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        fired = false;
    }
    #endregion
}