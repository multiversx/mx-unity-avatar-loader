namespace MultiversX.Avatar.Loader.Editor.WalletConnectMx
{
    public class NativeAuthClientConfig
    {
        public string Origin { get; set; }
        public string ApiUrl { get; set; }
        public int BlockHashShard { get; set; }

        public int ExpirySeconds { get; set; } = 86400;
    }
}
