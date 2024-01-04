using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MultiversX.Avatar.Core;
using UnityEditor;
using WalletConnectSharp.Common.Model.Errors;
using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;

namespace MultiversX.Avatar.Loader.Editor.WalletConnectMx
{
    public class WalletConnectFacade
    {
        public EditorWindow Window;
        private static WalletConnectFacade _instance;
        public static WalletConnectFacade Instance => _instance ??= new WalletConnectFacade();
        private WalletConnectSignClient _signClient;
        private readonly Task<WalletConnectSignClient> _initTask;
        private Task<string> _loginUriTask;
        private SessionStruct _session;

        [CanBeNull]
        public ConnectedData ConnectedData;
        public string Address;
        public string AccessToken;

        public event Action<WalletConnectState> OnStateChange;

        private WalletConnectFacade()
        {
            AvatarLoaderPreferences.OnPreferencesChange += OnPreferencesChange;
            _initTask = WalletConnectSignClient.Init(WalletConnectMxConfigs.SignClientOptions);
        }

        private async void OnPreferencesChange()
        {
            await Disconnect();
            _signClient = null;
            _session = default;
            ConnectedData = null;
            Address = null;
            AccessToken = null;
            WalletConnectMxConfigs.UpdateConfigsOnNetworkChange();
            AvatarLoaderContext.UpdateAvatarLoaderContextOnNetworkChange();
        }

        private void OnSessionDelete()
        {
            EditorApplication.update += DispatchOnStateChangeDisconnected;
        }

        private void DispatchOnStateChangeDisconnected()
        {
            OnStateChange?.Invoke(WalletConnectState.Disconnected);
            EditorApplication.update -= DispatchOnStateChangeDisconnected;
        }

        private void SubscribeToEvents()
        {
            if (_signClient == null)
            {
                return;
            }

            _signClient.SessionDeleted += (_, _) => OnSessionDelete();
            _signClient.SessionExpired += (_, _) => OnSessionDelete();
            _signClient.PairingExpired += (_, _) => OnSessionDelete();
            _signClient.PairingDeleted += (_, _) => OnSessionDelete();
        }

        private async Task GetSignClient()
        {
            if (_signClient != null)
            {
                return;
            }

            await _initTask;
            _signClient = _initTask.Result;
            SubscribeToEvents();
        }

        public async Task SetSession()
        {
            await GetSignClient();
            SessionStruct[] sessions = _signClient.Find(WalletConnectMxConfigs.RequiredNamespaces);

            if (sessions.Length > 0)
            {
                _session = sessions.First(x => x.Acknowledged ?? false);
            }
            else
            {
                ConnectedData = await _signClient.Connect(WalletConnectMxConfigs.ConnectOptions);
                OnStateChange?.Invoke(WalletConnectState.LoginUriReady);
                try
                {
                    _session = await ConnectedData!.Approval;
                }
                catch (WalletConnectException)
                {
                    OnStateChange?.Invoke(WalletConnectState.Disconnected);
                }
            }

            if (!_session.Equals(default(SessionStruct)))
            {
                OnStateChange?.Invoke(WalletConnectState.AwaitingLogin);
                await Login();
            }
        }

        public async Task Disconnect()
        {
            await _signClient.Disconnect(_session.Topic, null);

            _session = default;

            ConnectedData = null;
            OnStateChange?.Invoke(WalletConnectState.Disconnected);
        }

        public async Task Login()
        {
            Namespace selectedNamespace = _session.Namespaces[
                WalletConnectMxConfigs.WalletConnectMultiversxNamespace
            ];
            Address = selectedNamespace.Accounts[0].Split(':')[2];

            NativeAuthClient nativeAuthClient = new NativeAuthClient(
                WalletConnectMxConfigs.NativeAuthClientConfig
            );
            string authToken = await nativeAuthClient.GenerateToken();

            LoginRequest request = new LoginRequest(authToken, Address);
            LoginResponse response = await _signClient.Request<LoginRequest, LoginResponse>(
                _session.Topic,
                request,
                $"{WalletConnectMxConfigs.WalletConnectMultiversxNamespace}:{WalletConnectMxConfigs.ChainID}"
            );

            if (response == null)
            {
                throw new Exception("Login failed");
            }

            AccessToken = NativeAuthClient.GetAccessToken(Address, authToken, response.Signature);
            OnStateChange?.Invoke(WalletConnectState.Authenticated);
        }

        public string GetShortAddress([CanBeNull] string address = null)
        {
            address ??= Address;

            if (address.Length < 23)
                return address;

            return address[..10] + "..." + address[^9..];
        }
    }
}
