using MultiversX.Avatar.Loader.Editor.WalletConnectMx;
using UnityEngine.UIElements;

namespace MultiversX.Avatar.Loader.Editor.Windows.VisualElements
{
    public class UserDetails : VisualElement
    {
        private readonly Label _userAddressLabel = VisualElementHelper.Hide(new Label());

        public UserDetails()
        {
            style.flexShrink = 0;
            style.display = DisplayStyle.Flex;
            style.flexDirection = FlexDirection.Column;
            style.justifyContent = Justify.FlexStart;

            WalletConnectFacade.Instance.OnStateChange += UpdateUserDetails;

            Add(new LoadAvatarButton() { style = { marginBottom = 16, }, });
            Add(_userAddressLabel);
        }

        private void UpdateUserDetails(WalletConnectState state)
        {
            switch (state)
            {
                case WalletConnectState.Authenticated:
                    EnableUserDetails();
                    break;
                case WalletConnectState.LoginUriReady:
                case WalletConnectState.Disconnected:
                case WalletConnectState.AwaitingLogin:
                    DisableUserDetails();
                    break;
                default:
                    DisableUserDetails();
                    break;
            }
        }

        private void DisableUserDetails()
        {
            VisualElementHelper.Hide(_userAddressLabel);
            MarkDirtyRepaint();
        }

        private void EnableUserDetails()
        {
            VisualElementHelper.Show(_userAddressLabel);
            _userAddressLabel.text = LoaderTexts.DisplayUserAddress(
                WalletConnectFacade.Instance.GetShortAddress(WalletConnectFacade.Instance.Address)
            );
            MarkDirtyRepaint();
        }
    }
}
