using MultiversX.Avatar.Core.Editor.Windows;
using MultiversX.Avatar.Loader.Editor.WalletConnectMx;
using MultiversX.Avatar.Loader.Editor.Windows.VisualElements;
using MultiversX.Avatar.Core.Operations.Managers;
using UnityEditor;
using UnityEngine;

namespace MultiversX.Avatar.Loader.Editor.Windows
{
    public class MultiversxLoaderWindow : MxEditorWindow
    {
        private static AvatarLoaderManager _avatarLoaderManager;

        public MultiversxLoaderWindow()
        {
            _avatarLoaderManager = new AvatarLoaderManager();
            _avatarLoaderManager.OnChange += Repaint;
        }

        [MenuItem("MultiversX/Load Avatar", false, 0)]
        public static void ShowWindow()
        {
            WalletConnectFacade.Instance.Window = GetWindowWithRect<MultiversxLoaderWindow>(
                new Rect(0, 0, WindowWidth, WindowHeight),
                true,
                LoaderTexts.AvatarLoader
            );
        }

        protected override void InsertMainContent()
        {
            Main.Add(new Main());
            Main.Add(new Footer());
        }
    }
}
