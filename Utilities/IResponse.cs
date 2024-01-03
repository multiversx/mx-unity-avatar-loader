using UnityEngine.Networking;

namespace MultiversX.Avatar.Core.Utilities
{
    public interface IResponse
    {
        bool IsSuccess { get; set; }
        string Error { get; set; }
        long ResponseCode { get; set; }
        void Parse(UnityWebRequest request);
    }
}
