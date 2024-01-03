using MultiversX.Avatar.Loader.Editor.WalletConnectMx;
using UnityEngine;
using UnityEngine.UIElements;

namespace MultiversX.Avatar.Loader.Editor.Windows.VisualElements
{
    public class LoginPrompt : VisualElement
    {
        private readonly Label _loginPromptLabel = VisualElementHelper.Hide(
            new Label(LoaderTexts.RequestSent)
            {
                style =
                {
                    whiteSpace = WhiteSpace.Normal,
                    unityTextAlign = TextAnchor.MiddleCenter,
                },
            }
        );

        public LoginPrompt()
        {
            WalletConnectFacade.Instance.OnStateChange += UpdateLoginPromptState;

            Add(_loginPromptLabel);
        }

        private void UpdateLoginPromptState(WalletConnectState state)
        {
            switch (state)
            {
                case WalletConnectState.AwaitingLogin:
                    EnableLoginPrompt();
                    break;
                case WalletConnectState.Authenticated:
                case WalletConnectState.LoginUriReady:
                case WalletConnectState.Disconnected:
                    DisableLoginPrompt();
                    break;
                default:
                    DisableLoginPrompt();
                    break;
            }
        }

        private void DisableLoginPrompt()
        {
            VisualElementHelper.Hide(_loginPromptLabel);
            MarkDirtyRepaint();
        }

        private void EnableLoginPrompt()
        {
            VisualElementHelper.Show(_loginPromptLabel);
            MarkDirtyRepaint();
        }
    }
}
