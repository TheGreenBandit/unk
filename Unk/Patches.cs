using HarmonyLib;
using Photon.Pun;
using Steamworks.ServerList;
using UnityEngine;
using Unk.Cheats;
using Unk.Cheats.Core;
using Unk.Util;

namespace Unk
{
    [HarmonyPatch]
    internal class Patches
    {
        //[HarmonyPatch(typeof(MenuCursor), "Show"), HarmonyPrefix]
        //public static bool Show(MenuCursor __instance)
        //{//bugs out our shit
        //    return false;
        //}

        [HarmonyPatch(typeof(PlayerAvatar), "OnPhotonSerializeView"), HarmonyPrefix]
        public static bool OnPhotonSerializeView(PlayerAvatar __instance, PhotonStream stream, PhotonMessageInfo info)
        {
            if (__instance != PlayerController.instance.playerAvatar) return true; 
            if (stream.IsWriting)
            {
                stream.SendNext(__instance.Reflect().GetValue("isCrouching"));
                stream.SendNext(__instance.Reflect().GetValue("isSprinting"));
                stream.SendNext(__instance.Reflect().GetValue("isCrawling"));
                stream.SendNext(__instance.Reflect().GetValue("isSliding"));
                stream.SendNext(__instance.Reflect().GetValue("isMoving"));
                stream.SendNext(__instance.Reflect().GetValue("isGrounded"));
                stream.SendNext(__instance.Reflect().GetValue("Interact"));
                stream.SendNext(__instance.Reflect().GetValue<Vector3>("InputDirection"));
                stream.SendNext(PlayerController.instance.VelocityRelative);
                stream.SendNext(__instance.Reflect().GetValue<Vector3>("rbVelocityRaw"));
                stream.SendNext(Cheat.Instance<Invisibility>().Enabled ? new Vector3(10000, 100000, 10000): PlayerController.instance.transform.position); //this also makes it so they cant hear u i think
                stream.SendNext(PlayerController.instance.transform.rotation);
                stream.SendNext(__instance.Reflect().GetValue<Quaternion>("localCameraPosition"));
                stream.SendNext(__instance.Reflect().GetValue<Quaternion>("localCameraRotation"));
                stream.SendNext(PlayerController.instance.CollisionGrounded.physRiding);
                stream.SendNext(PlayerController.instance.CollisionGrounded.physRidingID);
                stream.SendNext(PlayerController.instance.CollisionGrounded.physRidingPosition);
                stream.SendNext(__instance.flashlightLightAim.clientAimPoint);
                stream.SendNext(__instance.Reflect().GetValue<int>("playerPing"));
                return false;
            }
            return true;
        }
    }
}
