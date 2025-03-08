using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unk.Cheats;
using Unk.Cheats.Core;
using Unk.Handler;
using Unk.Manager;
using Unk.Util;

namespace Unk
{
    [HarmonyPatch]
    internal class Patches
    {
        internal static readonly object keyByteZero = (object)(byte)0;
        internal static readonly object keyByteOne = (object)(byte)1;
        internal static readonly object keyByteTwo = (object)(byte)2;
        internal static readonly object keyByteThree = (object)(byte)3;
        internal static readonly object keyByteFour = (object)(byte)4;
        internal static readonly object keyByteFive = (object)(byte)5;
        internal static readonly object keyByteSix = (object)(byte)6;
        internal static readonly object keyByteSeven = (object)(byte)7;
        internal static readonly object keyByteEight = (object)(byte)8;

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
                stream.SendNext(Cheat.Instance<Invisibility>().Enabled ? new Vector3(10000, 100000, 10000) : PlayerController.instance.transform.position); //this also makes it so they cant hear u i think
                stream.SendNext(PlayerController.instance.transform.rotation);
                stream.SendNext(Cheat.Instance<Invisibility>().Enabled ? new Vector3(10000, 100000, 10000) : __instance.Reflect().GetValue<Quaternion>("localCameraPosition"));
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

        public static List<string> IgnoredRPCDebugs = new List<string>
        {
            "IsTalkingRPC",
            "ReceiveSyncData",
        };

        [HarmonyPatch(typeof(PhotonNetwork), "ExecuteRpc"), HarmonyPrefix]
        public static bool ExecuteRPC(Hashtable rpcData, Player sender)
        {
            if (sender is null || sender?.GamePlayer() == null/* || sender.GamePlayer().Handle().IsDev()*/) return true;

            string rpc = rpcData.ContainsKey(keyByteFive) ?
                PhotonNetwork.PhotonServerSettings.RpcList[Convert.ToByte(rpcData[keyByteFive])]
                : rpcData[keyByteThree] as string;

            if (!IgnoredRPCDebugs.Contains(rpc)) Debug.LogWarning($"Processing RPC '{rpc}' From '{sender.NickName}'");

            if (!sender.IsLocal && sender.GamePlayer().Handle().IsRPCBlocked())
            {
                Debug.LogError($"RPC {rpc} was blocked from {sender.NickName}.");
                return false;
            }

            return sender.GamePlayer().Handle().OnReceivedRPC(rpc, rpcData);
        }

        [HarmonyPatch(typeof(LoadBalancingPeer), "OpRaiseEvent"), HarmonyPrefix]
        public static bool OpRaiseEvent(LoadBalancingPeer __instance, byte eventCode, object customEventContent, RaiseEventOptions raiseEventOptions, SendOptions sendOptions)
        {

            Debug.Log($"Raise Event Called!\n eventcode: {eventCode}, \nraise event options| caching: {raiseEventOptions.CachingOption}" +
                $" flags: {raiseEventOptions.Flags}\nsendoptions| channel: {sendOptions.Channel}, mode: {sendOptions.DeliveryMode}");
            if (eventCode == 204)
                return false;
            return true;
        }
        [HarmonyPatch(typeof(ReloadScene), "Start"), HarmonyPostfix]
        public static void Start()
        {
            GameObjectManager.ClearObjects();
        }
    }
}
