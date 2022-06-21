using System.Collections;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using ReMod.Core.Managers;
using ReMod.Core.UI;
using ReMod.Core.UI.QuickMenu;
using ReMod.Core.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRC.DataModel.Core;
using VRC.UI;
using VRC.UI.Core;
using VRC.UI.Elements.Menus;

namespace OldMate;

public class UI
{
    
    public static ReMenuCategory userSelectCategory;
    public static GameObject TheUserSelectMenu;
    
    public static IEnumerator UIInit()
    {
        while (UIManager.prop_UIManager_0 == null) yield return null;
        while (Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
        VRChatAPI._selectedUserMenuQM = QuickMenuEx.Instance.transform.FindChild("Container/Window/QMParent/Menu_SelectedUser_Local").GetComponent<SelectedUserMenuQM>();
        UserSelMenu();
        UserInfoScreen();
        Main.Log.Msg("Menu Built");
    }
    
    private static void UserSelMenu()
    {
        TheUserSelectMenu = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup");
        userSelectCategory = new ReMenuCategory("OldMate", TheUserSelectMenu.transform);

        userSelectCategory.AddButton("Add/Update Nickname", "Add/Update Nickname", () =>
        {
            VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowInputPopupWithCancel("Enter a Nickname", "",
                InputField.InputType.Standard, false, "Set Nickname", (nickname, ignore, ignore2) =>
                {
                    
                    var user = VRChatAPI.GetSelectedAPIUser();
                    if (user == null) return;
                    var nick = new NicknameManager.Nickname();
                    nick.UserId = user.id;
                    nick.OriginalName = user.displayName;
                    nick.ModifiedName = nickname;
                    NicknameManager.UpdateNickname(nick);
                    Main.Log.Msg($"Set {nick.OriginalName}'s nickname to {nick.ModifiedName} ({nick.UserId})");
                }, () => VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup());
        }, ResourceManager.GetSprite("OldMate.add"));
        userSelectCategory.AddButton("Remove Nickname", "Remove Nickname", () =>
        { 
            var user = VRChatAPI.GetSelectedAPIUser();
            if (user == null) return;
            NicknameManager.RemoveNickname(user.id);
            Main.Log.Msg($"Removed {user.displayName}'s nickname ({user.id})");
        }, ResourceManager.GetSprite("OldMate.remove"));
    }

    public static void UserInfoScreen()
    {
        
        var userInfoTransform = VRCUiManagerEx.Instance.MenuContent().transform.Find("Screens/UserInfo");
        var pageUserInfo = userInfoTransform.GetComponent<PageUserInfo>();
        var addnick = new ReUiButton("Add/Update Nick",  Vector2.zero, new Vector2(0.68f, 1.2f), () =>
            {
                VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowInputPopupWithCancel("Enter a Nickname", "",
                    InputField.InputType.Standard, false, "Set Nickname", (nickname, ignore, ignore2) =>
                    {
                        var user = pageUserInfo.field_Private_IUser_0.Cast<DataModel<APIUser>>().field_Protected_TYPE_0;
                        if (user == null) return;
                        var nick = new NicknameManager.Nickname();
                        nick.UserId = user.id;
                        nick.OriginalName = user.displayName;
                        nick.ModifiedName = nickname;
                        NicknameManager.UpdateNickname(nick);
                        Main.Log.Msg($"Set {nick.OriginalName}'s nickname to {nick.ModifiedName} ({nick.UserId})");
                    }, () => VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup());
            },
            VRCUiManagerEx.Instance.MenuContent().transform
                .Find("Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/"));
        var removenick = new ReUiButton("Remove Nick", Vector2.zero, new Vector2(0.68f, 1.2f), () =>
        {
            var user = pageUserInfo.field_Private_IUser_0.Cast<DataModel<APIUser>>().field_Protected_TYPE_0;
            if (user == null) return;
            NicknameManager.RemoveNickname(user.id);
            Main.Log.Msg($"Removed {user.displayName}'s nickname ({user.id})");
        }, VRCUiManagerEx.Instance.MenuContent().transform
            .Find("Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/"));
    }
    
    public static void CacheIcons()
    {
        //https://github.com/RequiDev/ReModCE/blob/master/ReModCE/ReMod.cs
        var ourAssembly = Assembly.GetExecutingAssembly();
        var resources = ourAssembly.GetManifestResourceNames();
        foreach (var resource in resources)
        {
            if (!resource.EndsWith(".png"))
                continue;

            var stream = ourAssembly.GetManifestResourceStream(resource);

            var ms = new MemoryStream();
            stream.CopyTo(ms);
            var resourceName = Regex.Match(resource, @"([a-zA-Z\d\-_]+)\.png").Groups[1].ToString();
            ResourceManager.LoadSprite("OldMate", resourceName, ms.ToArray());
        }
    }
}