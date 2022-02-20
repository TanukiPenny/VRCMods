using System;
using System.Net;
using System.Reflection;
using MelonLoader;
using VRC.Core;
using VRC.UI.Elements.Menus;
using VRC.DataModel;
using VRC.DataModel.Core;
using System.Diagnostics;
using UnityEngine;
using System.Collections;
using VRC.UI.Core;
using ReMod.Core;
using ReMod.Core.UI.QuickMenu;


namespace UserESP
{
    public static class BuildShit
    {
        public const string Name = "UserESP";
        public const string Author = "Penny";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
        public const string Description = "A standalone mod to turn on a ESP for selected user";
    }

    public class Main : MelonMod
    {
        internal static readonly MelonLogger.Instance log = new MelonLogger.Instance(BuildShit.Name, ConsoleColor.Yellow);
        private static SelectedUserMenuQM _selectedUserMenuQM;
        public static GameObject TheUserSelectMenu;
        public static ReMenuCategory userSelectCategory;
        public static bool test;
        private static int scenesLoaded = 0;

        public override void OnApplicationStart()
        {
            LoadRemodCore(out _);
            log.Msg("UserESP Loaded");
        }
        
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (scenesLoaded <= 2)
            {
                scenesLoaded++;
                if (scenesLoaded == 2)
                {
                    MelonCoroutines.Start(OnQuickMenu());
                }
            }
        }
         
        private void LoadRemodCore(out Assembly loadedAssembly)
        {
            byte[] bytes = null;
            var wc = new WebClient();
        

            try
            {
                bytes = wc.DownloadData($"https://github.com/RequiDev/ReModCE/releases/latest/download/ReMod.Core.dll");
                loadedAssembly = Assembly.Load(bytes);
            }
            catch (WebException e)
            {
                MelonLogger.Error($"Unable to Load Core Dep RemodCore: {e}");
            }
            catch (BadImageFormatException e)
            {
                loadedAssembly = null;
            }
            loadedAssembly = null;
        }
        
        public static APIUser GetSelectedAPIUser()
        {
            if (_selectedUserMenuQM == null)
                _selectedUserMenuQM = UnityEngine.Object.FindObjectOfType<SelectedUserMenuQM>();
            if (_selectedUserMenuQM != null)
            {
                DataModel<APIUser> user = _selectedUserMenuQM.field_Private_IUser_0.Cast<DataModel<APIUser>>();
                return user.field_Protected_TYPE_0;
            }
            log.Error("Unable to get SelectedUserMenuQM component!");
            return null;
        }
        
        public static IEnumerator OnQuickMenu()
        {
            while (UIManager.prop_UIManager_0 == null) yield return null;
            while (UnityEngine.Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
            UserSelMenu();
            Main.log.Msg("Menu Built");
        }
        
        private static void UserSelMenu()
        {
            TheUserSelectMenu = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup");
            userSelectCategory = new ReMenuCategory("UserESP", TheUserSelectMenu.transform);

            userSelectCategory.AddToggle("UserESP", "Activate ESP on this user", test =>
            {
                if (test)
                {
                    log.Msg("true");
                }
                else
                {
                    log.Msg("false");
                }
            });
        }
    }
}