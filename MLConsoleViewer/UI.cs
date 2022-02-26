using System.Collections;
using ReMod.Core;
using ReMod.Core.UI.QuickMenu;
using UnityEngine;
using VRC.UI.Core;

namespace MLConsoleViewer
{
    public class UI
    {
        //public static ReMenuCategory Console;
        private static ReCategoryPage ConsoleTab;
        private static GameObject MLMenu;
        
        public static IEnumerator OnQuickMenu()
        {
            while (UIManager.prop_UIManager_0 == null) yield return null;
            while (UnityEngine.Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
            BuildTab();
            Main.log.Msg("Menu Built");
        }

        public static void BuildTab()
        {
            ConsoleTab = new ReCategoryPage("MLConsoleViewer", true);
            ReTabButton.Create("MLConsoleViewer", "Open MLConsoleViewer", "MLConsoleViewer", BundleManager.console);
            MLMenu = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_MLConsoleViewer/");
            GameObject.Instantiate(BundleManager.prefab, MLMenu.transform);
        }
    }
}