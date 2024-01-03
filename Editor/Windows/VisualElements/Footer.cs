using UnityEngine.UIElements;

namespace MultiversX.Avatar.Loader.Editor.Windows.VisualElements
{
    public class Footer : VisualElement
    {
        public Footer()
        {
            style.flexShrink = 0;
            Add(new ConnectButton());
            Add(new DisconnectButton());
        }
    }
}
