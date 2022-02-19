using UnityEngine;
using System.IO;
using UnhollowerRuntimeLib;
using System.Reflection;

namespace ProgramLauncher
{
    internal class BundleManager
    {
        private static AssetBundle _bundle;
        public static Sprite Plaunch, Plus, Minus, Launch, LaunchRed;

        private static Sprite LoadSprite(string sprite)
        {
            Sprite sprite2 = _bundle.LoadAsset_Internal(sprite, Il2CppType.Of<Sprite>()).Cast<Sprite>();
            sprite2.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return sprite2;
        }

        public static void InIt()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ProgramLauncher.plaunch"))
            {
                using (var memoryStream = new MemoryStream((int)stream.Length))
                {
                    stream.CopyTo(memoryStream);
                    _bundle = AssetBundle.LoadFromMemory_Internal(memoryStream.ToArray(), 0);
                    _bundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                    Plaunch = LoadSprite("plaunch.png");
                    Plus = LoadSprite("plus.png");
                    Minus = LoadSprite("Minus.png");
                    Launch = LoadSprite("launch.png");
                    LaunchRed = LoadSprite("launchred.png");


                }
            }
        }
    }
}
