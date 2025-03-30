using Photon.Pun;
using System.Linq;
using UnityEngine;
using Unk.Cheats.Core;
using Unk.Manager;
using Unk.Util;

namespace Unk.Cheats
{
    internal class BlackHole : ToggleCheat
    {
        public static Vector3 pos = new Vector3(0, 0, 0);
        public static bool self = false;
        public static float strength = 1;

        public override void Update()
        {
            foreach (PlayerAvatar p in GameObjectManager.players.Where(x => !x.IsDead()))
            {
                if (!Enabled || !PhotonNetwork.InRoom) return;
                if ((p == PlayerAvatar.instance) && !self) return;
                Vector3 force = new Vector3(pos.x - p.playerTransform.position.x, pos.y - p.playerTransform.position.y, pos.z - p.playerTransform.position.z);
                force = (force / 20) * strength;
                p.photonView.RPC("ForceImpulseRPC", RpcTarget.All, force);
            }
        }

        public override void OnGui()
        {           
            if (!Enabled || !PhotonNetwork.InRoom) return;//todo fix drawing circle shit
            if (WorldToScreen(pos, out Vector3 c))
                UI.DrawCircle(new Rect(c.x, c.y, 20, 20), 200 / (1 * GetDistanceToPos(PlayerAvatar.instance.playerTransform.position)), Color.black);
        }
    }
}
