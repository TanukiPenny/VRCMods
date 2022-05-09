using UnityEngine.XR;
using VRC.SDKBase;
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
            case Action.Meme:
                Main.Log.Msg(
                    "The FitnessGram Pacer Test is a multistage aerobic capacity test that progressively gets more difficult as it continues. The 20 meter pacer test will begin in 30 seconds. Line up at the start. The running speed starts slowly, but gets faster each minute after you hear this signal. [beep] A single lap should be completed each time you hear this sound. [ding] Remember to run in a straight line, and run as long as possible. The second time you fail to complete a lap before the sound, your test is over. The test will begin on the word start. On your mark, get ready, start.");
                return;
            case Action.Rejoin_Instance:
                var currentInstance = RoomManager.field_Internal_Static_ApiWorldInstance_0;
                if (currentInstance is null)
                    return;
                Main.Log.Msg("Rejoining instance!");
                Networking.GoToRoom(currentInstance.id);
                return;
            case Action.Camera:
                CameraToggle();
                return;
            case Action.Open_Worlds:
                //TODO Add Open Worlds screen function
                return;
            case Action.Open_Avatars:
                //TODO Add Open Avatars screen function
                return;
            case Action.Open_Social:
                //TODO Add Open Social screen function
                return;
            case Action.Open_Settings:
                UI.SettingsExpand.Invoke();
                //TODO Fix Open Settings screen function
                return;
            case Action.Open_Safety:
                //TODO Add Open Safety screen function
                return;
            case Action.Sound_Off:
                UI.MasterAudioSlider.value = 0f;
                Main.Log.Msg("Audio muted!");
                return;
            case Action.Deafen:
                UI.MasterAudioSlider.value = 0f;
                DefaultTalkController.Method_Public_Static_Void_Boolean_0(true);
                Main.Log.Msg("You deafened yourself!");
                return;
            case Action.Log_Friends:
                foreach (var a in VRC.UI.FriendsListManager.prop_FriendsListManager_0.prop_List_1_IUser_0)
                {
                    Main.Log.Msg("----------------------------------------------");
                    Main.Log.Msg(a.prop_String_0);
                    Main.Log.Msg(a.prop_String_1);
                    Main.Log.Msg(a.prop_String_2);
                    Main.Log.Msg(a.prop_String_3);
                    Main.Log.Msg(a.prop_String_4);
                    Main.Log.Msg(a.prop_String_5);
                    Main.Log.Msg("----------------------------------------------");
                }
                return;
        }
    }

    public enum Action
    {
        None,
        Rejoin_Instance,
        Meme,
        Camera,
        Open_Worlds,
        Open_Avatars,
        Open_Social,
        Open_Settings,
        Open_Safety,
        Sound_Off,
        Deafen,
        Log_Friends
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