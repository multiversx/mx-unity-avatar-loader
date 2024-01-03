using UnityEngine;
using System.IO;
using MultiversX.Avatar.Loader.Editor.WalletConnectMx;

namespace MultiversX.Avatar.Core
{
    public class AvatarLoaderContext : Context
    {
        public static string SignedUrlApi = AvatarLoaderSignedUrlApi.GetSignedUrlApi(
            AvatarLoaderPreferences.SelectedNetwork
        );
        public string NativeAuthToken { get; set; }
        public string SignedUrl { get; set; }
        public byte[] AvatarData { get; set; }
        public static bool IsLoaded { get; set; }
        public GameObject Avatar { get; set; }
        public object GlbData { get; set; }
        public static bool IsFailed { get; set; }
        public static readonly string AvatarDirectory = "Assets/MultiversX";
        public static readonly string AvatarGlb = Path.Combine(AvatarDirectory, "Avatar.glb");
        public static string AvatarPrefab = Path.Combine(AvatarDirectory, "Avatar.prefab");

        public static void UpdateAvatarLoaderContextOnNetworkChange()
        {
            SignedUrlApi = AvatarLoaderSignedUrlApi.GetSignedUrlApi(
                AvatarLoaderPreferences.SelectedNetwork
            );
        }
    }
}
