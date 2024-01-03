using MultiversX.Avatar.Loader.Editor.WalletConnectMx;
using UnityEngine.UIElements;

namespace MultiversX.Avatar.Loader.Editor.Windows.VisualElements
{
    public class ConnectButton : VisualElement
    {
        private readonly Button _button = VisualElementHelper.Show(
            new Button { text = LoaderTexts.Connect, }
        );

        public ConnectButton()
        {
            WalletConnectFacade.Instance.OnStateChange += UpdateConnectButtonState;

            _button.clicked += ClickEvent;
            Add(_button);
        }

        private void UpdateConnectButtonState(WalletConnectState state)
        {
            switch (state)
            {
                case WalletConnectState.AwaitingLogin:
                case WalletConnectState.Authenticated:
                    DisableConnectButton();
                    break;
                case WalletConnectState.LoginUriReady:
                case WalletConnectState.Disconnected:
                    EnableConnectButton();
                    break;
                default:
                    DisableConnectButton();
                    break;
            }
        }

        private void EnableConnectButton()
        {
            VisualElementHelper.Show(_button);
            MarkDirtyRepaint();
        }

        private void DisableConnectButton()
        {
            VisualElementHelper.Hide(_button);
            MarkDirtyRepaint();
        }

        private static async void ClickEvent()
        {
            await WalletConnectFacade.Instance.SetSession();
        }
    }
}
