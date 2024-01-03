using System;
using System.Threading;
using System.Threading.Tasks;
using GLTFast;
using MultiversX.Avatar.Core.Operations.Managers;
using MultiversX.Avatar.Core.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MultiversX.Avatar.Core.Operations
{
    public class ConvertAvatarDataOperation : IOperation<AvatarLoaderContext>
    {
        public int Timeout { get; set; }
        public Action<float> ProgressChanged { get; set; }
        public IOperation<AvatarLoaderContext>.Status CurrentStatus { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

#pragma warning disable CS1998
        public async Task<AvatarLoaderContext> Execute(
            AvatarLoaderContext context,
            CancellationToken cancellationToken
        )
#pragma warning restore CS1998
        {
            GameObject avatar;
            IDeferAgent agent = new UninterruptedDeferAgent();

            GltfImport gltf = new GltfImport(deferAgent: agent);
            bool success = await gltf.LoadGltfBinary(
                context.AvatarData,
                cancellationToken: cancellationToken
            );

            if (success)
            {
                avatar = new GameObject();
                avatar.SetActive(false);
                GltFastGameObjectInstantiator customInstantiator =
                    new GltFastGameObjectInstantiator(gltf, avatar.transform);
                await gltf.InstantiateMainSceneAsync(customInstantiator, cancellationToken);
            }
            else
            {
                throw new Exception("Failed to load glb");
            }

            context.Avatar = avatar;

#if UNITY_EDITOR
            Object.DestroyImmediate(avatar);
            AssetDatabase.Refresh();
            DirectoryUtility.CreateDirectory(AvatarLoaderContext.AvatarDirectory);
            GameObject avatarAsset = AssetDatabase.LoadAssetAtPath<GameObject>(
                AvatarLoaderContext.AvatarGlb
            );
            context.GlbData = Object.Instantiate(avatarAsset);
#endif
            GameObject oldAvatar = GameObject.Find("Avatar");
            if (oldAvatar)
            {
                Object.DestroyImmediate(oldAvatar);
            }

            ((Object)context.GlbData).name = "Avatar";

            GameObject newAvatar = (GameObject)context.GlbData;

            PrefabUtility.SaveAsPrefabAssetAndConnect(
                newAvatar,
                AvatarLoaderContext.AvatarPrefab,
                InteractionMode.AutomatedAction,
                out bool prefabSuccess
            );

            if (prefabSuccess == false)
            {
                throw new Exception("Failed to save prefab");
            }

            PrefabUtility.ApplyObjectOverride(
                newAvatar,
                AvatarLoaderContext.AvatarPrefab,
                InteractionMode.AutomatedAction
            );
            AssetDatabase.Refresh();

            return context;
        }
    }
}
