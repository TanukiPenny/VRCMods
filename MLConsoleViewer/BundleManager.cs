using System.IO;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace MLConsoleViewer.Bundle
{
    internal class BundleManager
    {
        private static AssetBundle Bundle;
        public static Sprite console;
        public static GameObject prefab;
        
        private static Sprite loadSprite(string sprite)
        {
            Sprite sprite2 = Bundle.LoadAsset_Internal(sprite, Il2CppType.Of<Sprite>()).Cast<Sprite>();
            sprite2.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return sprite2;
        }
        public static void InIt()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MLConsoleViewer.console"))
            {
                using (var memoryStream = new MemoryStream((int)stream.Length))
                {
                    stream.CopyTo(memoryStream);
                    Bundle = AssetBundle.LoadFromMemory_Internal(memoryStream.ToArray(), 0);
                    Bundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                    console = loadSprite("console.png");
                    prefab = Bundle.LoadAsset_Internal("console", GameObject);
                }
            }
        }
    }
}