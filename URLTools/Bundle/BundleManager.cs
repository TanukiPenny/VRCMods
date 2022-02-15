using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace URLTools
{
    internal class BundleManager
    {
        private static AssetBundle Bundle;
        public static Sprite globe, copy;
        private static Sprite loadSprite(string sprite)
        {
            Sprite sprite2 = Bundle.LoadAsset_Internal(sprite, Il2CppType.Of<Sprite>()).Cast<Sprite>();
            sprite2.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return sprite2;
        }
        public static void InIt()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("URLTools.Bundle.urltools"))
            {
                using (var memoryStream = new MemoryStream((int)stream.Length))
                {
                    stream.CopyTo(memoryStream);
                    Bundle = AssetBundle.LoadFromMemory_Internal(memoryStream.ToArray(), 0);
                    Bundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;

                    copy = loadSprite("copy.png");
                    globe = loadSprite("globe.png");
                }
            }
        }
    }
}
