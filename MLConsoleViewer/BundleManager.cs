using System.IO;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace MLConsoleViewer;

internal static class BundleManager
{
    public static Sprite ConsoleImg;
    public static GameObject ConsolePrefab;

    private static void LoadAsset<T>(this AssetBundle bundle, out T cache, int posInBundle) where T : Object =>
    (cache = bundle.LoadAsset_Internal(bundle.GetAllAssetNames()[posInBundle], Il2CppType.Of<T>()).Cast<T>()).hideFlags |= HideFlags.DontUnloadUnusedAsset;
    
    public static void Init()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MLConsoleViewer.console");
        using var memoryStream = new MemoryStream((int)stream!.Length);
        stream.CopyTo(memoryStream);
        var bundle = AssetBundle.LoadFromMemory_Internal(memoryStream.ToArray(), 0);
        bundle.LoadAsset(out ConsoleImg, 0);
        bundle.LoadAsset(out ConsolePrefab, 1);
    }
}