using MultiversX.Avatar.Loader.Editor.WalletConnectMx;
using MultiversX.Avatar.Core.Operations.Managers;
using UnityEngine.UIElements;

namespace MultiversX.Avatar.Loader.Editor.Windows.VisualElements
{
    public class LoadAvatarButton : VisualElement
    {
        private readonly Button _button = VisualElementHelper.Hide(
            new Button { text = LoaderTexts.LoadAvatar, }
        );

        public LoadAvatarButton()
        {
            WalletConnectFacade.Instance.OnStateChange += UpdateLoadAvatarButtonState;

            _button.clicked += LoadAvatar;
            Add(_button);
        }

        private void UpdateLoadAvatarButtonState(WalletConnectState state)
        {
            switch (state)
            {
                case WalletConnectState.LoginUriReady:
                case WalletConnectState.Disconnected:
                case WalletConnectState.AwaitingLogin:
                    DisableLoadAvatarButton();
                    break;
                case WalletConnectState.Authenticated:
                    EnableLoadAvatarButton();
                    break;
                default:
                    DisableLoadAvatarButton();
                    break;
            }
        }

        private void DisableLoadAvatarButton()
        {
            VisualElementHelper.Hide(_button);
            MarkDirtyRepaint();
        }

        private void EnableLoadAvatarButton()
        {
            VisualElementHelper.Show(_button);
            MarkDirtyRepaint();
        }

        private static void LoadAvatar()
        {
            AvatarLoaderManager manager = new AvatarLoaderManager();
            manager.LoadAvatar(WalletConnectFacade.Instance.AccessToken);
        }
    }
}
