using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MultiversX.Avatar.Core.Operations.Managers;
using MultiversX.Avatar.Core.Utilities;
using UnityEngine.Networking;

namespace MultiversX.Avatar.Core.Operations
{
    public class GetAvatarDataOperation : IOperation<AvatarLoaderContext>
    {
        public int Timeout { get; set; }
        public Action<float> ProgressChanged { get; set; }

#pragma warning disable CS1998
        public async Task<AvatarLoaderContext> Execute(
            AvatarLoaderContext context,
            CancellationToken cancellationToken
        )
#pragma warning restore CS1998
        {
            DirectoryUtility.CreateDirectory(AvatarLoaderContext.AvatarDirectory);

            UnityWebRequest www = UnityWebRequest.Get(context.SignedUrl);
            www.method = HttpMethod.Get.ToString();
            www.downloadHandler = new DownloadHandlerFile(AvatarLoaderContext.AvatarGlb);

            UnityWebRequestAsyncOperation asyncOperation = www.SendWebRequest();

            while (!asyncOperation.isDone) { }

            ResponseFile response = new ResponseFile { ResponseCode = www.responseCode };

            if (www.result == UnityWebRequest.Result.Success)
            {
                response.IsSuccess = true;
                response.Parse(www);
                Task readFile = response.ReadFile(AvatarLoaderContext.AvatarGlb);

                while (readFile.IsCompleted == false) { }

                context.AvatarData = response.Data;
            }
            else
            {
                throw new Exception("Request failed: " + www.error);
            }

            return context;
        }

        public IOperation<AvatarLoaderContext>.Status CurrentStatus { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
