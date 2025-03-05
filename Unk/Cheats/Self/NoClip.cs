using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Windows;
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
            //idk what wrong atp
            if (movement is null) movement = PlayerController.instance.gameObject.AddComponent<KBInput>();
            Quaternion rotation = PlayerController.instance.playerAvatarScript.Reflect().GetValue<Quaternion>("localCameraRotation");
            movement.Configure(GameDirector.instance.MainCameraParent.forward, GameDirector.instance.MainCameraParent.right, GameDirector.instance.MainCameraParent.up); //normal, right, up check me
            movement.movementSpeed = Value;
            PlayerController.instance.GetComponentsInChildren<Collider>().ToList().ForEach(c => c.enabled = false);
            PlayerController.instance.rb.useGravity = false; //todo get no gravity working
            PlayerController.instance.rb.AddForce(movement.movement, ForceMode.Impulse);
        }

        public override void OnDisable()
        {
            Destroy(movement);
            movement = null;
            PlayerController.instance.GetComponentsInChildren<Collider>().ToList().ForEach(c => c.enabled = true);
            PlayerController.instance.CollisionController.enabled = true;
            PlayerController.instance.rb.useGravity = true;
        }
    }
}
