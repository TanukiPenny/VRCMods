using System.IO;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace MLConsoleViewer
{
    internal class BundleManager
    {
        private static AssetBundle Bundle;
        public static Sprite console;
        public static GameObject prefab;

            
        public static void Init()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MLConsoleViewer.console");
            using var memoryStream = new MemoryStream((int)stream!.Length);
            stream.CopyTo(memoryStream);
            Bundle = AssetBundle.LoadFromMemory_Internal(memoryStream.ToArray(), 0);
            (console = Bundle.LoadAsset_Internal(Bundle.GetAllAssetNames()[0], Il2CppType.Of<Sprite>()).Cast<Sprite>()).hideFlags |= HideFlags.DontUnloadUnusedAsset;
            (prefab = Bundle.LoadAsset_Internal(Bundle.GetAllAssetNames()[1], Il2CppType.Of<GameObject>()).Cast<GameObject>()).hideFlags |= HideFlags.DontUnloadUnusedAsset;
        }
    }
}
