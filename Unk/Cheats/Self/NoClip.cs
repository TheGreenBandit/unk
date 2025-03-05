using UnityEngine;
using Unk.Cheats.Components;
using Unk.Cheats.Core;
using Unk.Util;

namespace Unk.Cheats
{
    internal class NoClip : ToggleCheat
    {
        public static float Value = 10f;

        private KBInput movement = null;
        public override void Update()
        {
            if (PlayerController.instance is null || !Enabled || GameDirector.instance.MainCameraParent is null) return;
            if (movement is null) movement = PlayerController.instance.gameObject.AddComponent<KBInput>();
            Quaternion rotation = PlayerController.instance.playerAvatarScript.Reflect().GetValue<Quaternion>("localCameraRotation");
            movement.Configure(GameDirector.instance.MainCameraParent.forward, GameDirector.instance.MainCameraParent.right, GameDirector.instance.MainCameraParent.up); //normal, right, up check me
            movement.movementSpeed = Value;
            
            PlayerController.instance.MoveFriction = 0; //5
            PlayerCollision.instance.enabled = false;

            PlayerController.instance.rb.AddForce(movement.movement, ForceMode.Impulse);
        }

        public override void OnDisable()
        {
            Destroy(movement);
            movement = null;
            PlayerController.instance.MoveFriction = 5;
            PlayerCollision.instance.enabled = true;
        }
    }
}
