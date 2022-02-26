using System.Collections;
using MLConsoleViewer.Bundle;
using ReMod.Core;
using ReMod.Core.UI.QuickMenu;
using VRC.UI.Core;

namespace MLConsoleViewer
{
    public class UI
    {
        //public static ReMenuCategory Console;
        private static ReCategoryPage ConsoleTab;
        
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
        }
    }
}