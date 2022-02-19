using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core;
using System.Collections;
using VRC.UI.Element;


namespace QMScale
{
    public static class BuildShit
    {
        public const string Name = "QMScale";
        public const string Author = "Penny";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/PennyBunny/VRCMods/";
        public const string Description = "Lets user set scale of their quick menu";
    }

    public class Main : MelonMod
    {
        public static MelonLogger.Instance Logger = new MelonLogger.Instance("QMScale");
        public static MelonPreferences_Category _QMScale;
        public static MelonPreferences_Entry<float> Scale;
        public static Transform QmContainer;
        public static RectTransform QmContainerRect;
        public override void OnApplicationStart()
        {
            _QMScale = MelonPreferences.CreateCategory("QMScale", "QMScale");
            Scale = _QMScale.CreateEntry("scale", 1f, "Scale", "Your quick menus's overall scale", false, false);
            MelonCoroutines.Start(WaitForQm());
            Logger.Msg("QMScale Loaded correct!");
        }

        public override void OnPreferencesSaved()
        {
            Resize();
        }
        public static IEnumerator WaitForQm()
        {
            while (VRCUiManager.field_Private_Static_VRCUiManager_0 == null) yield return null;
            while (UIManager.field_Private_Static_UIManager_0 == null) yield return null;
            while (GameObject.Find("UserInterface").GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true) == null) yield return null;
            QmContainer = GameObject.Find("UserInterface").GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true)
                .transform;
            //QmContainerRect = GameObject.Find("UserInterface").GetComponentInChildren<RectTransform>(true);
            Resize();
        }

        public static void Resize()
        {
            if(QmContainer == null) 
                return;
            QmContainer.localScale = new Vector3(Scale.Value, Scale.Value, 1);
            //QmContainerRect.anchoredPosition = new Vector2(QmContainerRect.anchoredPosition.x * Scale.Value,
                //QmContainerRect.anchoredPosition.y * Scale.Value);
            Logger.Msg("QM scale changed");
        }
    }
}