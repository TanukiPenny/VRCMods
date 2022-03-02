using System.Collections;
using ReMod.Core.UI.QuickMenu;
using UnityEngine;
using VRC.UI.Core;

namespace UserTagDumper
{
    public class UI
    {
        public static ReMenuCategory userCategory;
        public static GameObject UserMenu;
        
        public static IEnumerator OnQuickMenu()
        {
            while (UIManager.prop_UIManager_0 == null) yield return null;
            while (Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
            UserSelMenu();
            Main.log.Msg("Menu Built");
        }
        
        private static void UserSelMenu()
        {
            UserMenu = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup");
            userCategory = new ReMenuCategory("UserTagDumper", UserMenu.transform);

            userCategory.AddButton("Dump Tags", "Dumps User's API Tags to ML console", () => Functions.LogTags());
        }
    }
}