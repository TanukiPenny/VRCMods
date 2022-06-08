using ReMod.Core.VRChat;
using UnityEngine.XR;
using VRC.SDKBase;
using VRC.UI;
using VRC.UserCamera;
// ReSharper disable InconsistentNaming

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
                VRCUiManagerEx.Instance.ShowUi();
                VRCUiManagerEx.Instance.ShowScreen(QuickMenu.MainMenuScreenIndex.WorldsMenu);
                UI.CloseQM();
                return;
            case Action.Open_Avatars:
                VRCUiManagerEx.Instance.ShowUi();
                VRCUiManagerEx.Instance.ShowScreen(QuickMenu.MainMenuScreenIndex.AvatarMenu);
                UI.CloseQM();
                return;
            case Action.Open_Social:
                VRCUiManagerEx.Instance.ShowUi();
                VRCUiManagerEx.Instance.ShowScreen(QuickMenu.MainMenuScreenIndex.SocialMenu);
                UI.CloseQM();
                return;
            case Action.Open_Settings:
                VRCUiManagerEx.Instance.ShowUi();
                VRCUiManagerEx.Instance.ShowScreen(QuickMenu.MainMenuScreenIndex.SettingsMenu);
                UI.CloseQM();
                return;
            case Action.Open_Safety:
                VRCUiManagerEx.Instance.ShowUi();
                VRCUiManagerEx.Instance.ShowScreen(QuickMenu.MainMenuScreenIndex.SafetyMenu);
                UI.CloseQM();
                return;
            case Action.Open_Gallery:
                VRCUiManagerEx.Instance.ShowUi();
                VRCUiManagerEx.Instance.ShowScreen(QuickMenu.MainMenuScreenIndex.GalleryMenu);
                UI.CloseQM();
                return;
            case Action.Sound_Off:
                UI.MasterAudioSlider.value = 0f;
                Main.Log.Msg("Audio muted!");
                return;
            case Action.Deafen:
                UI.MasterAudioSlider.value = 0f;
                //TODO Fix deafen function
                Main.Log.Msg("You deafened yourself!");
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
        Deafen
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
        }
        else
        {
            UserCameraController.field_Internal_Static_UserCameraController_0.prop_UserCameraMode_0 = UserCameraMode.Off;
        }
    }
}