using Photon.Pun;
using Steamworks;
using Unk.Cheats.Core;
using Unk.Handler;

namespace Unk.Cheats
{
    internal class NameSpoofer : ToggleCheat, IVariableCheat<string>
    {
        public static string Value = SteamClient.Name;

        public override void Update()
        {
            if (!Enabled) return;
            if (PhotonNetwork.LocalPlayer.NickName != Value)
            {
                PhotonNetwork.LocalPlayer.NickName = Value;
                PlayerAvatar.instance.photonView.RPC("AddToStatsManagerRPC", RpcTarget.All, Value, PlayerAvatar.instance.GetSteamID());
            }          
        }

        public override void OnDisable()
        {
            PhotonNetwork.LocalPlayer.NickName = SteamClient.Name;
            PlayerAvatar.instance.photonView.RPC("AddToStatsManagerRPC", RpcTarget.All, SteamClient.Name, PlayerAvatar.instance.GetSteamID());
        }
    }
}
