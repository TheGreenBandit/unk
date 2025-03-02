using unk.Cheats.Core;
using unk.Components;
using unk.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace unk.Cheats
{
    internal class NoClip : ToggleCheat
    {

        private KBInput movement = null;

        public override void Update()
        {
            if (Player.localPlayer is null) return;

            //SphereCollider collider = Player.localPlayer.refs.Reflect().GetValue<SphereCollider>("simpleCollider");
            PlayerRagdoll ragdoll = Player.localPlayer.refs.ragdoll;
            if (Enabled)
            {
                if (movement is null) movement = Player.localPlayer.gameObject.AddComponent<KBInput>();

                //collider.enabled = false;

                Bodypart hip = Player.localPlayer.refs.ragdoll.Reflect().Invoke<Bodypart>("GetBodypart", args: BodypartType.Hip);
                Rigidbody rig = hip.Reflect().GetValue<Rigidbody>("rig");

                rig.transform.position = movement.transform.position;
                
            }
            else
            {
                //collider.enabled = true;
                Destroy(movement);
                movement = null;
            }
        }

    }
}
