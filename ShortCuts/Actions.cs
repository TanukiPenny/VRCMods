using ReMod.Core.VRChat;
using UnityEngine.XR;
using VRC.SDKBase;
using VRC.UI;
using VRC.UserCamera;

namespace ShortCuts;

public static class Actions
{
    public static void DoubleClickHandler(Action action)
    {
        switch (action)
        {
            case Action.None:
                return;
            case Action.Rejoin_Instance:
                var currentInstance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
                if (currentInstance is null)
                    return;
                Main.Log.Msg("Rejoining instance!");
                Networking.GoToRoom(currentInstance.id);
                return;
            case Action.Take_Out_Camera:
                CameraToggle();
                return;
            case Action.Open_Worlds:
                UIManagerImpl.prop_UIManagerImpl_0.CloseQuickMenu();
                VRCUiManagerEx.Instance.ShowUi(false);
                VRCUiManagerEx.Instance.ShowScreen(QuickMenu.MainMenuScreenIndex.WorldsMenu);
                Main.Log.Msg("Opening worlds menu!");
                return;
            case Action.Open_Avatars:
                UIManagerImpl.prop_UIManagerImpl_0.CloseQuickMenu();
                VRCUiManagerEx.Instance.ShowUi(false);
                VRCUiManagerEx.Instance.ShowScreen(QuickMenu.MainMenuScreenIndex.AvatarMenu);
                Main.Log.Msg("Opening avatars menu!");
                return;
            case Action.Open_Social:
                UIManagerImpl.prop_UIManagerImpl_0.CloseQuickMenu();
                VRCUiManagerEx.Instance.ShowUi(false);
                VRCUiManagerEx.Instance.ShowScreen(QuickMenu.MainMenuScreenIndex.SocialMenu);
                Main.Log.Msg("Opening social menu!");
                return;
            case Action.Open_Settings:
                UIManagerImpl.prop_UIManagerImpl_0.CloseQuickMenu();
                VRCUiManagerEx.Instance.ShowUi(false);
                VRCUiManagerEx.Instance.ShowScreen(QuickMenu.MainMenuScreenIndex.SettingsMenu);
                Main.Log.Msg("Opening settings menu!");
                return;
            case Action.Open_Safety:
                UIManagerImpl.prop_UIManagerImpl_0.CloseQuickMenu();
                VRCUiManagerEx.Instance.ShowUi(false);
                VRCUiManagerEx.Instance.ShowScreen(QuickMenu.MainMenuScreenIndex.SafetyMenu);
                Main.Log.Msg("Opening safety  menu!");
                return;
            case Action.Open_Gallery:
                UIManagerImpl.prop_UIManagerImpl_0.CloseQuickMenu();
                VRCUiManagerEx.Instance.ShowUi(false);
                VRCUiManagerEx.Instance.ShowScreen(QuickMenu.MainMenuScreenIndex.GalleryMenu);
                Main.Log.Msg("Opening gallery menu!");
                return;
            case Action.Sound_Off:
                UI.MasterAudioSlider.value = 0f;
                Main.Log.Msg("Audio muted!");
                return;
            case Action.Deafen:
                UI.MasterAudioSlider.value = 0f;
                USpeaker.Method_Public_Static_Void_Boolean_0(true);
                Main.Log.Msg("You deafened yourself!");
                return;
            case Action.Reload_All_Avatars:
                PlayerExtensions.ReloadAllAvatars(VRCPlayer.field_Internal_Static_VRCPlayer_0);
                Main.Log.Msg("Reloading all avatars!");
                return;
            case Action.Reload_Your_Avatar:
                PlayerExtensions.ReloadAvatar(VRCPlayer.field_Internal_Static_VRCPlayer_0);
                Main.Log.Msg("Reloading your avatar!");
                return;
        }
    }

    public enum Action
    {
        None,
        Rejoin_Instance,
        Take_Out_Camera,
        Open_Worlds,
        Open_Avatars,
        Open_Social,
        Open_Settings,
        Open_Safety,
        Open_Gallery,
        Sound_Off,
        Deafen,
        Reload_All_Avatars,
        Reload_Your_Avatar
    }

    private static void CameraToggle()
    {
        if (UserCameraController.field_Internal_Static_UserCameraController_0.prop_UserCameraMode_0 == UserCameraMode.Off)
        {
            if (!XRDevice.isPresent && UI.MainCamera.fieldOfView != 60)
            {
                VRCUiManager.prop_VRCUiManager_0.field_Private_List_1_String_0.Add("Using the camera on desktop with a custom FOV is not recommended due to weird distortion!");
                VRCUiManager.prop_VRCUiManager_0.field_Private_List_1_String_0.Add("");
            }
            UserCameraController.field_Internal_Static_UserCameraController_0.prop_UserCameraMode_0 = UserCameraMode.Photo;
            UserCameraController.field_Internal_Static_UserCameraController_0.prop_UserCameraSpace_0 = UserCameraSpace.Attached;
            Main.Log.Msg("Toggled camera on!");
        }
        else
        {
            UserCameraController.field_Internal_Static_UserCameraController_0.prop_UserCameraMode_0 = UserCameraMode.Off;
            Main.Log.Msg("Toggled camera off!");
        }
    }
}