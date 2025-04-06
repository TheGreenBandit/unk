using Photon.Pun;
using UnityEngine;

namespace Unk.Util
{
    class GameUtil
    {
        public static void Teleport(PlayerAvatar player, Vector3 position)
        {
            player.tumble.Reflect().GetValue<PhotonView>("photonView").RPC("TumbleRequestRPC", RpcTarget.All, true, true);
            player.GetObject().Reflect().GetValue<PhotonView>("photonView").
            RPC("SetPositionRPC", RpcTarget.All, position, Quaternion.identity);
        }
        public static void Teleport(ValuableObject item, Vector3 position)
        {
            item.GetObject().Reflect().GetValue<PhotonView>("photonView").
            RPC("SetPositionRPC", RpcTarget.All, position, Quaternion.identity);
        }
    }
}