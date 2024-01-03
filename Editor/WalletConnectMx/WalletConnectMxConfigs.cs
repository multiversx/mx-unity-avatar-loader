using System.IO;
using WalletConnectSharp.Core;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;
using WalletConnectSharp.Storage;

namespace MultiversX.Avatar.Loader.Editor.WalletConnectMx
{
    public static class WalletConnectMxConfigs
    {
        public static string DAppName = "MultiversX Unity Avatar Teleporter";
        public static string DAppDescription =
            "Unity Plugin used to import your Mx Avatar into your project";
        public static string DAppUrl = MxOrigin.GetOrigin(AvatarLoaderPreferences.SelectedNetwork);
        public static string[] DAppIcons = { "https://multiversx.com/favicon.ico" };
        public static Metadata Metadata = new Metadata()
        {
            Description = DAppDescription,
            Url = DAppUrl,
            Icons = DAppIcons,
            Name = DAppName
        };
        public static string ProjectId = "c7d3aa2b21836c991357e8a56c252962";
        public static FileSystemStorage Storage = new FileSystemStorage(
            Path.Combine(".wc", "wc_storage.json")
        );
        public static SignClientOptions SignClientOptions = new SignClientOptions
        {
            Metadata = Metadata,
            ProjectId = ProjectId,
            Storage = Storage
        };
        public static string WalletConnectMultiversxNamespace = "multiversx";
        public static string ChainID = MxChainIDs.GetChainID(
            AvatarLoaderPreferences.SelectedNetwork
        );
        public static RequiredNamespaces RequiredNamespaces = new RequiredNamespaces
        {
            {
                WalletConnectMultiversxNamespace,
                new ProposedNamespace
                {
                    Methods = new[]
                    {
                        "multiversx_signTransaction",
                        "multiversx_signTransactions",
                        "multiversx_signMessage",
                        "multiversx_signLoginToken",
                        "multiversx_cancelAction"
                    },
                    Chains = new[] { $"{WalletConnectMultiversxNamespace}:{ChainID}" },
                    Events = new string[] { }
                }
            }
        };
        public static ConnectOptions ConnectOptions = new ConnectOptions()
        {
            RequiredNamespaces = RequiredNamespaces
        };
        public static string ApiUrl = MxApiUrls.GetApiUrl(AvatarLoaderPreferences.SelectedNetwork);
        public static NativeAuthClientConfig NativeAuthClientConfig = new NativeAuthClientConfig
        {
            Origin = DAppUrl,
            ApiUrl = ApiUrl,
            BlockHashShard = 1
        };

        public static void UpdateConfigsOnNetworkChange()
        {
            DAppUrl = MxOrigin.GetOrigin(AvatarLoaderPreferences.SelectedNetwork);
            Metadata.Url = DAppUrl;
            SignClientOptions.Metadata = Metadata;
            ChainID = MxChainIDs.GetChainID(AvatarLoaderPreferences.SelectedNetwork);
            RequiredNamespaces = new RequiredNamespaces
            {
                {
                    WalletConnectMultiversxNamespace,
                    new ProposedNamespace
                    {
                        Methods = new[]
                        {
                            "multiversx_signTransaction",
                            "multiversx_signTransactions",
                            "multiversx_signMessage",
                            "multiversx_signLoginToken",
                            "multiversx_cancelAction"
                        },
                        Chains = new[] { $"{WalletConnectMultiversxNamespace}:{ChainID}" },
                        Events = new string[] { }
                    }
                }
            };
            ConnectOptions = new ConnectOptions() { RequiredNamespaces = RequiredNamespaces };
            ApiUrl = MxApiUrls.GetApiUrl(AvatarLoaderPreferences.SelectedNetwork);
            NativeAuthClientConfig = new NativeAuthClientConfig
            {
                Origin = DAppUrl,
                ApiUrl = ApiUrl,
                BlockHashShard = 1
            };
        }
    }
}
