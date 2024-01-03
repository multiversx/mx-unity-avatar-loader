using Newtonsoft.Json;
using WalletConnectSharp.Common.Utils;
using WalletConnectSharp.Network.Models;

namespace MultiversX.Avatar.Loader.Editor.WalletConnectMx
{
    [RpcMethod("multiversx_signLoginToken")]
    [RpcRequestOptions(Clock.ONE_DAY, 1108)]
    [RpcResponseOptions(Clock.ONE_DAY, 1109)]
    public class LoginRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public LoginRequest(string token, string address)
        {
            Token = token;
            Address = address;
        }
    }
}
