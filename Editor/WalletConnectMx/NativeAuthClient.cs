using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MultiversX.Avatar.Loader.Editor.WalletConnectMx
{
    public class NativeAuthClient
    {
        public NativeAuthClientConfig Config { get; set; }

        public NativeAuthClient(NativeAuthClientConfig config)
        {
            Config = config;
        }

        private static string EncodeValue(string value) =>
            Escape(Convert.ToBase64String(Encoding.UTF8.GetBytes(value)));

        private static string Escape(string value) =>
            value.Replace('+', '-').Replace('/', '_').Replace("=", "");

        private async Task<Block> GetCurrentBlock()
        {
            string requestUri = Config.ApiUrl + "/blocks?size=1&fields=hash,timestamp";
            if (Config.BlockHashShard != -1)
                requestUri += $"&shard={(object)Config.BlockHashShard}";
            HttpResponseMessage response = await new HttpClient().GetAsync(requestUri);
            Block[] blockArray =
                JsonConvert.DeserializeObject<Block[]>(await response.Content.ReadAsStringAsync())
                ?? throw new InvalidOperationException();
            if (!response.IsSuccessStatusCode || blockArray == null)
                throw new Exception("Could not get the Block from API");

            Block currentBlock = blockArray[0];
            return currentBlock;
        }

        public static string GetAccessToken(string address, string token, string signature) =>
            EncodeValue(address) + "." + EncodeValue(token) + "." + signature;

        public async Task<string> GenerateToken()
        {
            string origin = EncodeValue(Config.Origin);
            Block currentBlock = await GetCurrentBlock();
            string str = EncodeValue(
                JsonConvert.SerializeObject(new ExtraInfo(currentBlock.Timestamp))
            );
            string token =
                $"{(object)origin}.{(object)currentBlock.Hash}.{(object)Config.ExpirySeconds}.{(object)str}";
            return token;
        }
    }
}
