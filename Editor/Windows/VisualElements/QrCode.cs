using System.Threading.Tasks;
using QRCoder;
using QRCoder.Unity;
using MultiversX.Avatar.Loader.Editor.WalletConnectMx;
using UnityEngine;
using UnityEngine.UIElements;

namespace MultiversX.Avatar.Loader.Editor.Windows.VisualElements
{
    public class QrCode : VisualElement
    {
        private Task _qrCodeTimeoutTask;

        private readonly Image _qrCode = VisualElementHelper.Hide(
            new Image() { style = { flexShrink = 1, } }
        );
        private readonly Label _qrCodeCta = VisualElementHelper.Hide(
            new Label(LoaderTexts.LoginWithQr)
            {
                style =
                {
                    whiteSpace = WhiteSpace.Normal,
                    unityTextAlign = TextAnchor.MiddleCenter,
                },
            }
        );
        private readonly Label _qrCodeStatus = VisualElementHelper.Hide(
            new Label(LoaderTexts.LoadingQr)
            {
                style =
                {
                    whiteSpace = WhiteSpace.Normal,
                    unityTextAlign = TextAnchor.MiddleCenter,
                },
            }
        );

        public QrCode()
        {
            WalletConnectFacade.Instance.OnStateChange += UpdateQrCodeState;

            Add(_qrCode);
            Add(_qrCodeCta);
        }

        private void UpdateQrCodeState(WalletConnectState state)
        {
            switch (state)
            {
                case WalletConnectState.AwaitingLogin:
                case WalletConnectState.Authenticated:
                case WalletConnectState.Disconnected:
                    DisableQrCode();
                    break;
                case WalletConnectState.LoginUriReady:
                    EnableQrCode();
                    break;
                default:
                    DisableQrCode();
                    break;
            }
        }

        private void DisableQrCode()
        {
            VisualElementHelper.Hide(_qrCode);
            VisualElementHelper.Hide(_qrCodeCta);
            VisualElementHelper.Hide(_qrCodeStatus);
            MarkDirtyRepaint();
        }

        private void EnableQrCode()
        {
            string uri = WalletConnectFacade.Instance.ConnectedData?.Uri;

            if (uri == null)
            {
                VisualElementHelper.Hide(_qrCode);
                VisualElementHelper.Hide(_qrCodeCta);
                VisualElementHelper.Show(_qrCodeStatus);
                MarkDirtyRepaint();
                return;
            }

            _qrCode.image = GenerateQrCodeImage(uri);
            VisualElementHelper.Show(_qrCode);
            VisualElementHelper.Show(_qrCodeCta);
            VisualElementHelper.Hide(_qrCodeStatus);
            MarkDirtyRepaint();
        }

        private Texture GenerateQrCodeImage(string uri)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
            UnityQrCode qrCode = new UnityQrCode(qrCodeData);
            Texture2D qrCodeAsTexture2D = qrCode.GetGraphic(10);

            qrCodeAsTexture2D.Apply();

            return qrCodeAsTexture2D;
        }
    }
}
