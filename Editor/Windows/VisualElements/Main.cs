using UnityEngine.UIElements;

namespace MultiversX.Avatar.Loader.Editor.Windows.VisualElements
{
    public class Main : VisualElement
    {
        public Main()
        {
            style.flexGrow = 1;
            style.justifyContent = Justify.Center;
            LoginPrompt loginPrompt = new LoginPrompt();
            QrCode qrCode = new QrCode();
            UserDetails userDetails = new UserDetails();

            Add(loginPrompt);
            Add(qrCode);
            Add(userDetails);
        }
    }
}
