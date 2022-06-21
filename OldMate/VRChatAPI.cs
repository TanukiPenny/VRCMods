//using Harmony;
using HarmonyLib;
using MelonLoader;
using System;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRC.DataModel.Core;
using VRC.UI.Elements.Menus;

namespace OldMate
{
    public static class VRChatAPI
    {

        public delegate Player GetPlayerFromIdMethod(string id);
        public static GetPlayerFromIdMethod GetPlayerFromIdMethodDelegate;

        private static MethodInfo VRCUiManagerInstanceMethodInfo;
        public static bool testing { get; set; }
        public static VRCUiManager VRCUiManagerInstance
        {
            get
            {
                if (VRCUiManagerInstanceMethodInfo == null)
                {
                    VRCUiManagerInstanceMethodInfo = typeof(VRCUiManager).GetMethods().First(x => (x.ReturnType == typeof(VRCUiManager)));
                }
                return (VRCUiManager)VRCUiManagerInstanceMethodInfo.Invoke(null, Array.Empty<object>());
            }
        }

        public static GetPlayerFromIdMethod GetPlayerFromId
        {
            get
            {
                if (GetPlayerFromIdMethodDelegate == null)
                {
                    var targetMethod = typeof(PlayerManager).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(it => it.GetParameters().Length == 1 && it.GetParameters()[0].ParameterType.ToString() == "System.String").Last();
                    GetPlayerFromIdMethodDelegate = (GetPlayerFromIdMethod)Delegate.CreateDelegate(typeof(GetPlayerFromIdMethod), targetMethod);
                }
                return GetPlayerFromIdMethodDelegate;
            }
        }

        public static void Patch(HarmonyLib.Harmony harmonyInstance)
        {
            harmonyInstance.Patch(typeof(TMP_Text).GetProperty("text").GetSetMethod(), new HarmonyMethod(typeof(VRChatAPI).GetMethod("OnTmpTextSet", BindingFlags.Static | BindingFlags.NonPublic)));
            harmonyInstance.Patch(typeof(Text).GetProperty("text").GetSetMethod(), new HarmonyMethod(typeof(VRChatAPI).GetMethod("OnUnityTextGet", BindingFlags.Static | BindingFlags.NonPublic)));
        }

        private static void OnTmpTextSet(ref string __0)
        {
            if (__0 != null)
            {
                foreach (var nickname in NicknameManager.nicknames)
                {
                    if (__0 == nickname.OriginalName)
                    {
                        //MelonLogger.Msg("Modified Name: " + nickname.ModifiedName);
                        __0 = __0.Replace(nickname.OriginalName, nickname.ModifiedName);
                    }
                    //if (__0 == nickname.OriginalName.Substring(0, 3) && testing == false)
                    //{
                    //    __0 = __0.Replace(nickname.OriginalName.Substring(0, 3), nickname.ModifiedName.Substring(0, 3));
                    //}
                    //if (__0 == nickname.ModifiedName.Substring(0, 3) && testing == true)
                    //{
                    //    __0 = __0.Replace(nickname.ModifiedName.Substring(0, 3), nickname.OriginalName.Substring(0, 3));
                    //}
                }
            }
        }

        private static void OnUnityTextGet(ref string __0)
        {
            if (__0 != null)
            {
                foreach (var nickname in NicknameManager.nicknames)
                {
                    if (__0 == nickname.OriginalName)
                    {
                        __0 = __0.Replace(nickname.OriginalName, nickname.ModifiedName);
                    }
                    //if (__0 == nickname.OriginalName.Substring(0, 3) && testing == false)
                    //{
                    //    __0 = __0.Replace(nickname.OriginalName.Substring(0, 3), nickname.ModifiedName.Substring(0, 3));
                    //}
                    //if (__0 == nickname.ModifiedName.Substring(0, 3) && testing == true)
                    //{
                    //    __0 = __0.Replace(nickname.ModifiedName.Substring(0, 3), nickname.OriginalName.Substring(0, 3));
                    //}
                }
            }
        }

        public static void UpdatePlayerNameplate(Player player, bool OriginalName = false)
        {
            if (NicknameManager.Contains(player.prop_APIUser_0.id))
            {
                player.prop_VRCPlayer_0.field_Public_PlayerNameplate_0.field_Public_TextMeshProUGUI_0.text = !OriginalName ? NicknameManager.GetModifiedName(player.field_Private_APIUser_0.id) : player.field_Private_APIUser_0.displayName;
                player.prop_VRCPlayer_1.field_Public_PlayerNameplate_0.field_Public_TextMeshProUGUI_0.text = !OriginalName ? NicknameManager.GetModifiedName(player.field_Private_APIUser_0.id) : player.field_Private_APIUser_0.displayName;
                Main.Log.Msg($"Updating {player.name}'s nameplate to {(!OriginalName ? NicknameManager.GetModifiedName(player.field_Private_APIUser_0.id) : player.field_Private_APIUser_0.displayName)}");
            }
        }

        public static void UpdateInitials(string id, bool OriginalName = false, string prevName = null)
        {
            //MelonLogger.Msg("Going into UpdateInitials | " + id);
            if (NicknameManager.Contains(id))
            {
                GameObject leftWing = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer/Profile/ScrollRect/Viewport/VerticalLayoutGroup/ProfileImage/UserIcon/Icon/UserDefaultIcon/");
                GameObject rightWing = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/Wing_Right/Container/InnerContainer/Profile/ScrollRect/Viewport/VerticalLayoutGroup/ProfileImage/UserIcon/Icon/UserDefaultIcon/");
                GameObject userInfo = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo/User Panel/UserIcon/");
                if (leftWing != null)
                {
                    foreach (TMP_Text t in leftWing.GetComponentsInChildren<TMP_Text>())
                    {
                        if (t.text.Equals(!OriginalName ? NicknameManager.GetModifiedName(id).Substring(0, 3) : NicknameManager.GetOriginalName(id).Substring(0, 3)))
                        {
                            //MelonLogger.Msg("Updated leftWing Initials");
                            t.text = !OriginalName ? NicknameManager.GetModifiedName(id).Substring(0, 3) : NicknameManager.GetOriginalName(id).Substring(0, 3);
                        }
                        else if (prevName != null && t.text.Equals(prevName.Substring(0, 3)))
                        {
                            t.text = NicknameManager.GetModifiedName(id).Substring(0, 3);
                        }
                    }
                }
                if (rightWing != null)
                {
                    foreach (TMP_Text t in rightWing.GetComponentsInChildren<TMP_Text>())
                    {
                        if (t.text.Equals(!OriginalName ? NicknameManager.GetModifiedName(id).Substring(0, 3) : NicknameManager.GetOriginalName(id).Substring(0, 3)))
                        {
                            //MelonLogger.Msg("Updated rightWing Initials");
                            t.text = !OriginalName ? NicknameManager.GetModifiedName(id).Substring(0, 3) : NicknameManager.GetOriginalName(id).Substring(0, 3);
                        }
                        else if (prevName != null && t.text.Equals(prevName.Substring(0, 3)))
                        {
                            t.text = NicknameManager.GetModifiedName(id).Substring(0, 3);
                        }
                    }
                }
                if (userInfo == null)
                {
                    Main.Log.Msg("UserInfo is null!");
                }
                if (userInfo != null)
                {
                    //MelonLogger.Msg("userInfo is not null!");
                    foreach (TMP_Text t in userInfo.GetComponentsInChildren<TMP_Text>())
                    {
                        //MelonLogger.Msg(t.text + " | " + NicknameManager.GetModifiedName(id).Substring(0, 3) + " | " + NicknameManager.GetOriginalName(id).Substring(0, 3));
                        if (t.text.Equals(OriginalName ? NicknameManager.GetModifiedName(id).Substring(0, 3) : NicknameManager.GetOriginalName(id).Substring(0, 3)))
                        {
                            //MelonLogger.Msg("Updated userInfo Initials");
                            //MelonLogger.Msg("text = " + (!OriginalName ? NicknameManager.GetModifiedName(id).Substring(0, 3) : NicknameManager.GetOriginalName(id).Substring(0, 3)));
                            t.text = !OriginalName ? NicknameManager.GetModifiedName(id).Substring(0, 3) : NicknameManager.GetOriginalName(id).Substring(0, 3);
                        }
                        else if (prevName != null && t.text.Equals(prevName.Substring(0, 3)))
                        {
                            t.text = NicknameManager.GetModifiedName(id).Substring(0, 3);
                        }
                    }
                }
            }
        }

        public static void UpdateQuickMenuText(bool OriginalName = false, string prevNickname = null, string displayName = null, string modifiedName = null, string userID = null)
        {
            GameObject test = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo");

            foreach (Text t in test.GetComponentsInChildren<Text>())
            {
                // Removing Nickname
                if (modifiedName != null)
                {
                    if (displayName != null && t.text.Equals(modifiedName))
                    {
                        t.text = t.text.Replace(modifiedName, displayName);
                    }
                }

                NicknameManager.nicknames.ForEach(nickname =>
                {
                    // Updating Nickname
                    if (t.text.Equals(prevNickname) && prevNickname != null && OriginalName == false && userID == nickname.UserId)
                    {
                        t.text = t.text.Replace(!OriginalName ? prevNickname : nickname.ModifiedName, !OriginalName ? nickname.ModifiedName : prevNickname);
                    }
                    // Setting a Nickname
                    else if (t.text.Equals(!OriginalName ? nickname.OriginalName : nickname.ModifiedName))
                    {
                        t.text = t.text.Replace(!OriginalName ? nickname.OriginalName : nickname.ModifiedName, !OriginalName ? nickname.ModifiedName : nickname.OriginalName);
                    }
                });
            }
        }

        public static void UpdateQMName(bool OriginalName = false, string prevNickname = null, string displayName = null, string modifiedName = null, string userID = null)
        {
            GameObject test = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup/UserProfile_Compact/PanelBG/Info");

            foreach (TMP_Text t in test.GetComponentsInChildren<TMP_Text>())
            {
                // Removing Nickname
                if (modifiedName != null)
                {
                    if (displayName != null && t.text.Equals(modifiedName))
                    {
                        t.text = t.text.Replace(modifiedName, displayName);
                    }
                }

                NicknameManager.nicknames.ForEach(nickname =>
                {
                    // Updating Nickname
                    if (t.text.Equals(prevNickname) && prevNickname != null && OriginalName == false && userID == nickname.UserId)
                    {
                        t.text = t.text.Replace(!OriginalName ? prevNickname : nickname.ModifiedName, !OriginalName ? nickname.ModifiedName : prevNickname);
                    }
                    // Setting a Nickname
                    else if (t.text.Equals(!OriginalName ? nickname.OriginalName : nickname.ModifiedName))
                    {
                        t.text = t.text.Replace(!OriginalName ? nickname.OriginalName : nickname.ModifiedName, !OriginalName ? nickname.ModifiedName : nickname.OriginalName);
                    }
                });
            }
        }

        public static void UpdateMenuContentText(bool OriginalName = false)
        {
            foreach (Text t in VRCUiManagerInstance.field_Public_GameObject_0.GetComponentsInChildren<Text>())
            {
                NicknameManager.nicknames.ForEach(nickname =>
                {
                    if (t.text.Equals(!OriginalName ? nickname.OriginalName : nickname.ModifiedName))
                    {
                        t.text = t.text.Replace(!OriginalName ? nickname.OriginalName : nickname.ModifiedName, !OriginalName ? nickname.ModifiedName : nickname.OriginalName);
                    }
                });
            }
        }

        public static SelectedUserMenuQM _selectedUserMenuQM;

        public static APIUser GetSelectedAPIUser()
        {
            DataModel<APIUser> user = _selectedUserMenuQM.field_Private_IUser_0.Cast<DataModel<APIUser>>();

            return user.field_Protected_TYPE_0;
        }
    }
}