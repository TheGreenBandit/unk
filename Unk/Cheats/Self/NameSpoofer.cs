using Photon.Pun;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;
using Unk.Cheats.Core;

namespace Unk.Cheats
{
    internal class NameSpoofer : ToggleCheat, IVariableCheat<string>
    {
        public static string Value = SteamClient.Name;

        public override void Update()
        {
            if (!Enabled) return;
            PhotonNetwork.LocalPlayer.NickName = Value;
            PlayerController.instance.PlayerSetName(Value, SemiFunc.PlayerGetSteamID(PlayerAvatar.instance));
        }

        public override void OnDisable()
        {
            PhotonNetwork.LocalPlayer.NickName = SteamClient.Name;
            PlayerController.instance.PlayerSetName(SteamClient.Name, SemiFunc.PlayerGetSteamID(PlayerAvatar.instance));
        }
    }
}
