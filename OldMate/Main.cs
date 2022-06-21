using System;
using System.Collections;
using MelonLoader;
using VRC;

namespace OldMate
{
    public static class BuildShit
    {
        public const string Name = "OldMate";
        public const string Author = "Penny";
        public const string Version = "2.0.1";
        public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
        public const string Description = "A mod that allows you to assign custom nicknames to players";
    }

    public class Main : MelonMod
    {

        internal static readonly MelonLogger.Instance Log = new(BuildShit.Name, ConsoleColor.DarkYellow);

        public override void OnApplicationStart()
        {
            UI.CacheIcons();
            NicknameManager.LoadNicknames();
            MelonCoroutines.Start(Initialize());
            VRChatAPI.Patch(HarmonyInstance);
            Log.Msg("OldMate loaded!");
        }

        private static int _scenesLoaded = 0;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (_scenesLoaded <= 2)
            {
                _scenesLoaded++;
                if (_scenesLoaded == 2)
                {
                    MelonCoroutines.Start(UI.UIInit());
                }
            }
        }

        private IEnumerator Initialize()
        {
            while (ReferenceEquals(NetworkManager.field_Internal_Static_NetworkManager_0, null))
                yield return null;

            Log.Msg("Initializing OldMate.");
            NetworkManagerHooks.Initialize();
            NetworkManagerHooks.OnJoin += OnPlayerJoined;
        }

        public void OnPlayerJoined(Player player)
        {
            if (player != null)
            {
                if (NicknameManager.Contains(player.field_Private_APIUser_0.id))
                {
                    VRChatAPI.UpdatePlayerNameplate(player);
                }
            }
        }
    }
}