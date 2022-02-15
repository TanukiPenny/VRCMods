using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ReMod;
using ReMod.Core.UI.QuickMenu;
using VRC.UI.Core;
using System.Collections;
using MelonLoader;

namespace URLTools
{
    public class UIBuild
    {
        public static ReMenuCategory userSelectCategory, hereCategory;
        public static GameObject TheUserSelectMenu;
        public static GameObject TheHereMenu;
        public static IEnumerator OnQuickMenu()
        {
            while (UIManager.prop_UIManager_0 == null) yield return null;
            while (UnityEngine.Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
            UserSelMenu();
            HereMenu();
            Main.log.Msg("Menu Built");
        }
        
        private static void UserSelMenu()
        {
            TheUserSelectMenu = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup");
            userSelectCategory = new ReMenuCategory("URLTools", TheUserSelectMenu.transform);

            userSelectCategory.AddButton("Copy User URL", "Copy User URL", () => UserFunc.CopyUserURL(), BundleManager.copy);
            userSelectCategory.AddButton("Open User Page", "Open User Page", () => UserFunc.OpenUserPage(), BundleManager.globe);
        }
        private static void HereMenu()
        {
            TheHereMenu = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup/");
            hereCategory = new ReMenuCategory("URLTools", TheHereMenu.transform);

            hereCategory.AddButton("Copy World URL", "Copy World URL", () => WorldFunc.CopyWorldURL(), BundleManager.copy);
            hereCategory.AddButton("Open World Page", "Open World Page", () => WorldFunc.OpenWorldPage(), BundleManager.globe);
            hereCategory.Header.RectTransform.SetSiblingIndex(3);
            hereCategory.RectTransform.SetSiblingIndex(4);

        }
    }
}
