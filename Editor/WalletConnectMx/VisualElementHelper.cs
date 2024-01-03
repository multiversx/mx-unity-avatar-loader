using UnityEngine.UIElements;

namespace MultiversX.Avatar.Loader.Editor.WalletConnectMx
{
    public static class VisualElementHelper
    {
        public static T Hide<T>(T element)
            where T : VisualElement
        {
            element.visible = false;
            element.style.display = DisplayStyle.None;
            return element;
        }

        public static T Show<T>(T element)
            where T : VisualElement
        {
            element.visible = true;
            element.style.display = DisplayStyle.Flex;
            return element;
        }
    }
}
