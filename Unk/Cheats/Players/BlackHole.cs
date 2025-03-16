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
        Vector3 pos;

        public override void OnEnable()
        {

            //spawn blackhole object
        }

        public override void Update()
        {
            foreach (PlayerAvatar p in GameObjectManager.players.Where(x => !x.IsDead()))
            {
                Vector3 force = new Vector3(p.transform.position.x - pos.x, p.transform.position.y - pos.y, p.transform.position.z - pos.z);

                p.photonView.RPC("ForceImpulseRPC", RpcTarget.All, force);
            }
        }

        public override void OnGui()
        {
            Vector3 c = Camera.current.WorldToScreenPoint(pos, Camera.MonoOrStereoscopicEye.Mono);

            UI.DrawCircle(new Rect(c.x, c.y, 20, 20), 20 / GetDistanceToPos(PlayerAvatar.instance.transform.position));
        }
    }
}
