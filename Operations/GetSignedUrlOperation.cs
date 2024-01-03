using System;
using System.Threading;
using System.Threading.Tasks;
using MultiversX.Avatar.Core.Utilities;
using UnityEngine;
using UnityEngine.Networking;

namespace MultiversX.Avatar.Core.Operations
{
    public class GetSignedUrlOperation : IOperation<AvatarLoaderContext>
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
            string signedUrl;

            UnityWebRequest www = UnityWebRequest.Get(AvatarLoaderContext.SignedUrlApi);
            www.SetRequestHeader("Authorization", context.NativeAuthToken);
            www.SetRequestHeader("Format", "GLB");

            UnityWebRequestAsyncOperation asyncOperation = www.SendWebRequest();

            while (asyncOperation.isDone == false) { }

            if (www.result == UnityWebRequest.Result.Success)
            {
                SignedUrlResponse response = JsonUtility.FromJson<SignedUrlResponse>(
                    www.downloadHandler.text
                );

                signedUrl = response.signed_url;
            }
            else
            {
                throw new Exception($"Request failed: {www.error}");
            }

            context.SignedUrl = signedUrl;

            return context;
        }

        public IOperation<AvatarLoaderContext>.Status CurrentStatus { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
