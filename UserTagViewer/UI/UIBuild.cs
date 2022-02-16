using UnityEngine;
using ReMod.Core.UI.QuickMenu;
using VRC.UI.Core;
using System.Collections;

namespace UserTagViewer
{
    public class UIBuild
    {
        public static ReMenuCategory userSelectCategory;
        public static GameObject TheUserSelectMenu;
        public static IEnumerator OnQuickMenu()
        {
            while (UIManager.prop_UIManager_0 == null) yield return null;
            while (Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
            UserSelMenu();
            Main.log.Msg("Menu Built");
        }
        private static void UserSelMenu()
        {
            TheUserSelectMenu = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup");
            userSelectCategory = new ReMenuCategory("UserTagViewer", TheUserSelectMenu.transform);

            userSelectCategory.AddButton("Get User Tags", "Get User Tags", () => User.GetUserTags());
        }
    }
}
