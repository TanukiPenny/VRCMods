using System;
using System.Collections;
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
    //private static ReMenuCategory ConsoleTabMenu;
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
        foreach (var i in ConsoleManager.Cached)
            text.text += i;
        
        // Might add directly to the prefab
        //consolePrefab.transform.localScale = new Vector3(1.8f, 1.8f, 1);
        scrollRect.elasticity = 0;
    }

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