using Photon.Pun;
using System.Collections.Generic;
using Unk.Cheats.Core;
using Unk.Util;

namespace Unk.Cheats
{
    class PlayerGodmode : ToggleCheat
    {
        public static List<PlayerAvatar> players = new List<PlayerAvatar>();

        public override void Update()
        {
            if (players == null) return;
            foreach (PlayerAvatar player in players)
                player.playerHealth.Reflect().GetValue<PhotonView>("photonView").RPC("UpdateHealthRPC", RpcTarget.All, 9999999, 9999999, false);
        }

        public static void ToggleGodmode(PlayerAvatar player)
        {
            if (IsGodmode(player))
            {
                player.playerHealth.Reflect().GetValue<PhotonView>("photonView").RPC("UpdateHealthRPC", RpcTarget.All, 100, 100, false);
                players.Remove(player);
            }
            else
                players.Add(player);
        }

        public static bool IsGodmode(PlayerAvatar player)
        {
            return players.Contains(player);
        }
    }
}
