using System;
using UnityEditor;
using UnityEngine;

namespace MultiversX.Avatar.Core.Operations.Managers
{
    public class AvatarLoaderManager
    {
        private readonly OperationsExecutor<AvatarLoaderContext> _operationsExecutor;
        public event Action OnChange;

        public AvatarLoaderManager()
        {
            var operations = new IOperation<AvatarLoaderContext>[]
            {
                new GetSignedUrlOperation(),
                new GetAvatarDataOperation(),
                new ConvertAvatarDataOperation()
            };

            _operationsExecutor = new OperationsExecutor<AvatarLoaderContext>(operations)
            {
                Timeout = 30 * 1000
            };
        }

        public async void LoadAvatar(string nativeAuthToken)
        {
            AvatarLoaderContext context = new AvatarLoaderContext
            {
                NativeAuthToken = nativeAuthToken
            };

            try
            {
                await _operationsExecutor.Execute(context);
                AvatarLoaderContext.IsFailed = false;
                AvatarLoaderContext.IsLoaded = true;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load avatar: " + e.Message);
                AvatarLoaderContext.IsLoaded = false;
                AvatarLoaderContext.IsFailed = true;
            }

            AssetDatabase.Refresh();
            OnChange?.Invoke();
        }
    }
}
