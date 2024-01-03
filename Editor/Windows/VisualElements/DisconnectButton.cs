using MultiversX.Avatar.Loader.Editor.WalletConnectMx;
using UnityEngine.UIElements;

namespace MultiversX.Avatar.Loader.Editor.Windows.VisualElements
{
    public class DisconnectButton : VisualElement
    {
        private readonly Button _button = VisualElementHelper.Hide(
            new Button { text = LoaderTexts.Disconnect, }
        );

        public DisconnectButton()
        {
            WalletConnectFacade.Instance.OnStateChange += UpdateDisconnectButtonState;

            _button.clicked += Disconnect;
            Add(_button);
        }

        private void UpdateDisconnectButtonState(WalletConnectState state)
        {
            switch (state)
            {
                case WalletConnectState.LoginUriReady:
                case WalletConnectState.Disconnected:
                    DisableDisconnectButton();
                    break;
                case WalletConnectState.AwaitingLogin:
                case WalletConnectState.Authenticated:
                    EnableDisconnectButton();
                    break;
                default:
                    DisableDisconnectButton();
                    break;
            }
        }

        private void DisableDisconnectButton()
        {
            VisualElementHelper.Hide(_button);
            MarkDirtyRepaint();
        }

        private void EnableDisconnectButton()
        {
            VisualElementHelper.Show(_button);
            MarkDirtyRepaint();
        }

        private static async void Disconnect()
        {
            await WalletConnectFacade.Instance.Disconnect();
        }
    }
}
