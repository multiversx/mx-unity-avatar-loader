using System;
using UnityEditor;
using UnityEngine;

namespace MultiversX.Avatar.Loader.Editor.WalletConnectMx
{
    public static class MxApiUrls
    {
        // Constants for each environment
        public const string Devnet = "https://devnet-api.multiversx.com";
        public const string Testnet = "https://testnet-api.multiversx.com";
        public const string Mainnet = "https://api.multiversx.com";

        public static string GetApiUrl(NetworkOption networkOption)
        {
            switch (networkOption)
            {
                case NetworkOption.Devnet:
                    return Devnet;
                case NetworkOption.Testnet:
                    return Testnet;
                case NetworkOption.Mainnet:
                    return Mainnet;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(networkOption),
                        networkOption,
                        null
                    );
            }
        }
    }

    public static class MxChainIDs
    {
        public const string Devnet = "D";
        public const string Testnet = "T";
        public const string Mainnet = "1";

        public static string GetChainID(NetworkOption networkOption)
        {
            switch (networkOption)
            {
                case NetworkOption.Devnet:
                    return Devnet;
                case NetworkOption.Testnet:
                    return Testnet;
                case NetworkOption.Mainnet:
                    return Mainnet;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(networkOption),
                        networkOption,
                        null
                    );
            }
        }
    }

    public static class MxOrigin
    {
        // NOTE: This is currently setting all the Origins the same for all networks
        // If we need to change this in the future, we can add a switch statement
        private const string Origin = "https://multiversx.com/";

        public static string GetOrigin(NetworkOption networkOption)
        {
            return Origin;
        }
    }

    public static class AvatarLoaderSignedUrlApi
    {
        public const string DevnetSignedUrlApi =
            "https://devnet-avatars-api.xportal.com/api/avatars/avatar_3d/default/";
        public const string TestnetSignedUrlApi =
            "https://testnet-avatars-api.xportal.com/api/avatars/avatar_3d/default/";
        public const string MainnetSignedUrlApi =
            "https://avatars-api.xportal.com/api/avatars/avatar_3d/default/";

        public static string GetSignedUrlApi(NetworkOption networkOption)
        {
            switch (networkOption)
            {
                case NetworkOption.Devnet:
                    return DevnetSignedUrlApi;
                case NetworkOption.Testnet:
                    return TestnetSignedUrlApi;
                case NetworkOption.Mainnet:
                    return MainnetSignedUrlApi;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(networkOption),
                        networkOption,
                        null
                    );
            }
        }
    }

    public enum NetworkOption
    {
        Devnet,
        Testnet,
        Mainnet,
    }

    public class AvatarLoaderPreferences : SettingsProvider
    {
        private const string SelectedNetworkKey = "AvatarLoader_SelectedNetwork";
        public static NetworkOption SelectedNetwork = NetworkOption.Devnet;
        public static event Action OnPreferencesChange;

        [SettingsProvider]
        public static SettingsProvider Create()
        {
            return new AvatarLoaderPreferences();
        }

        private AvatarLoaderPreferences()
            : base("Project/Avatar Teleporter", SettingsScope.Project)
        {
            LoadPreferences();
        }

        static AvatarLoaderPreferences()
        {
            LoadPreferences();
        }

        private static void LoadPreferences()
        {
            Debug.Log("Loading Avatar Teleporter preferences");
            SelectedNetwork = (NetworkOption)
                EditorPrefs.GetInt(SelectedNetworkKey, (int)SelectedNetwork);
        }

        private static void SavePreferences()
        {
            Debug.Log("Saving Avatar Teleporter preferences");
            EditorPrefs.SetInt(SelectedNetworkKey, (int)SelectedNetwork);
            OnPreferencesChange?.Invoke();
        }

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            EditorGUI.BeginChangeCheck();

            SelectedNetwork = (NetworkOption)
                EditorGUILayout.EnumPopup("Select Network", SelectedNetwork);

            if (EditorGUI.EndChangeCheck())
            {
                SavePreferences();
            }
        }
    }
}
