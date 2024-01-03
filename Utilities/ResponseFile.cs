using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace MultiversX.Avatar.Core.Utilities
{
    public class ResponseFile : IResponse
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public long ResponseCode { get; set; }

        public byte[] Data { get; private set; }

        // ReSharper disable once InconsistentNaming
        private ulong length;

        public void Parse(UnityWebRequest request)
        {
            length = request.downloadedBytes;
        }

        public async Task ReadFile(string path)
        {
            var byteLength = (long)length;
            var info = new FileInfo(path);

            while (info.Length != byteLength)
            {
                info.Refresh();
                await Task.Yield();
            }

            // Reading file since can't access raw bytes from downloadHandler
            Data = File.ReadAllBytes(path);
        }
    }
}
