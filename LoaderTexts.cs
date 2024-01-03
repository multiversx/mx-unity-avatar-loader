namespace MultiversX.Avatar.Loader
{
    public class LoaderTexts
    {
        public static readonly string Connect = "Connect";
        public static readonly string Disconnect = "Disconnect";
        public static readonly string RequestSent =
            "A request has been sent to refresh your token. Please check your phone.";
        public static readonly string AvatarLoader = "Avatar Loader";
        public static readonly string LoadAvatar = "Load Avatar";
        public static readonly string LoadingQr = "Loading QR code...";
        public static readonly string LoginWithQr =
            "Log in to MultiversX by using the xPortal app to scan the QR code.";

        public static string DisplayUserAddress(string address)
        {
            return $"User address: {address}";
        }
    }
}
