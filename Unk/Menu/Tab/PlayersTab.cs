﻿using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Unk.Cheats;
using Unk.Cheats.Core;
using Unk.Handler;
using Unk.Manager;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk.Menu.Tab
{
    internal class PlayersTab : MenuTab
    {
        public PlayersTab() : base("Players") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        public static PlayerAvatar selectedPlayer = null;
        private string message = "Unk.";
        private string color = "-1";

        public override void Draw()
        {
            UI.VerticalSpace(ref scrollPos, () =>
            {
                PlayersList();
            }, GUILayout.Width(UnkMenu.Instance.contentWidth * 0.3f - UnkMenu.Instance.spaceFromLeft));
            UI.VerticalSpace(ref scrollPos2, () =>
            {
                GeneralActions();
                PlayerActions();
            }, GUILayout.Width(UnkMenu.Instance.contentWidth * 0.7f - UnkMenu.Instance.spaceFromLeft));
        }

        private void GeneralActions()
        {
            UI.Header("General Actions");

            UI.Button("Kill All", () => {
                GameObjectManager.players.Where(p => p != null).ToList().ForEach(p => p.photonView.RPC("PlayerDeathRPC", RpcTarget.All, 0));
            });

            UI.Button("Kill others", () => {
                GameObjectManager.players.Where(p => p != null && !p.IsLocalPlayer()).ToList().ForEach(p => p.photonView.RPC("PlayerDeathRPC", RpcTarget.All, 0));
            });

            if (PlayerAvatar.instance.Handle().IsDev())
            {
                UI.Header("General Dev Options");

                UI.Button("Crash All", () => {
                    GameObjectManager.players.Where(p => p != null).ToList().ForEach(p => p.photonView.RPC("OutroStartRPC", p.PhotonPlayer()));
                });

                UI.Button("Crash others", () => {
                    GameObjectManager.players.Where(p => p != null && !p.IsLocalPlayer()).ToList().ForEach(p => p.photonView.RPC("OutroStartRPC", p.PhotonPlayer()));
                });

                UI.Button("Kick All", () =>
                {
                    PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer.IsMasterClient ? GameObjectManager.players.Find(x => x.GetSteamID() != PlayerAvatar.instance.GetSteamID()).PhotonPlayer() : PlayerAvatar.instance.PhotonPlayer());
                });
            }
        }

        private void PlayerActions()
        {
            if (selectedPlayer == null) return;

            if (!selectedPlayer.IsLocalPlayer() && selectedPlayer.Handle().IsDev())
            {
                UI.Label("User is a developer so you can't do anything.\n Make sure to say hi!");
                return;
            }

            if (PlayerAvatar.instance.Handle().IsDev())
            {
                UI.Header("Dev Only Options!");
            }

            UI.Header("Selected Player Actions");

            UI.Label("SteamId:", selectedPlayer.GetSteamID().ToString());
            UI.Label("Status:", selectedPlayer.IsDead() ? "Dead" : "Alive");
            UI.Label("Health:", selectedPlayer.GetHealth().ToString());
            UI.Label("Is Master Client:", selectedPlayer.IsLocalPlayer() ? SemiFunc.IsMasterClientOrSingleplayer().ToString() : selectedPlayer.PhotonPlayer().IsMasterClient.ToString());

            UI.Label("Unk User", selectedPlayer.Handle().IsUnkUser().ToString());
            UI.Button("test", () => selectedPlayer.photonView.RPC("OutroDoneRPC", selectedPlayer.PhotonPlayer()));

            UI.Button("Heal", () => selectedPlayer.playerHealth.Reflect().GetValue<PhotonView>("photonView").RPC("UpdateHealthRPC", RpcTarget.All, selectedPlayer.playerHealth.Reflect().GetValue<int>("maxHealth"), selectedPlayer.playerHealth.Reflect().GetValue<int>("maxHealth"), false));
            UI.Button("Crown", () => selectedPlayer.photonView.RPC("CrownPlayerRPC", RpcTarget.All, selectedPlayer.GetSteamID()));
            UI.Button("Disable", () => selectedPlayer.photonView.RPC("SetDisabledRPC", selectedPlayer.PhotonPlayer()));
            UI.Button("Kill", () => selectedPlayer.photonView.RPC("PlayerDeathRPC", RpcTarget.All, 0));
            UI.Button("Revive", () => selectedPlayer.RevivePlayer());
            UI.Button("Force Jump", () => selectedPlayer.photonView.RPC("JumpRPC", RpcTarget.All, true));
            UI.Button("Force Land", () => selectedPlayer.photonView.RPC("LandRPC", RpcTarget.All));
            UI.TextboxAction("Chat Message", ref message, 100,
                new UIButton("Send", () => selectedPlayer.photonView.RPC("ChatMessageSendRPC", RpcTarget.All, message, false) 
            ));
            
            UI.HorizontalSpace(null, () =>
            {
                //if (AssetManager.instance.playerColors.Count > 0)
                //{
                //    Color o = GUI.backgroundColor;
                //    int i = int.Parse(color);
                //    GUI.backgroundColor = AssetManager.instance.playerColors.ElementAt(i);
                //    GUILayout.Box("Preview");
                //    GUI.backgroundColor = o;
                //}

                UI.TextboxAction("Change Color", ref color, 2,
                    new UIButton("Set", () => selectedPlayer.photonView.RPC("SetColorRPC", RpcTarget.All, int.Parse(color))
                ));
            });
            UI.Button("Lure monsters to player", () => {
                GameObjectManager.enemies.Where(e => e != null && !e.IsDead()).ToList().ForEach(e => e.SetChaseTarget(selectedPlayer));
            });
            UI.CheatToggleSlider(Cheat.Instance<OverrideAnimSpeed>(), "Anim Speed Multiplier", OverrideAnimSpeed.Value.ToString(), ref OverrideAnimSpeed.Value, 0, 10);
            if (!selectedPlayer.IsLocalPlayer()) UI.Button("Block RPCs", () => selectedPlayer.Handle().ToggleRPCBlock(), selectedPlayer.Handle().IsRPCBlocked() ? "UnBlock" : "Block");
        }

        private void PlayersList()
        {
            float width = UnkMenu.Instance.contentWidth * 0.3f - UnkMenu.Instance.spaceFromLeft * 2;
            float height = UnkMenu.Instance.contentHeight - 20;
            Rect rect = new Rect(0, 0, width, height);
            GUI.Box(rect, "Player List");
            GUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(height));
            GUILayout.Space(25);
            foreach (PlayerAvatar player in GameObjectManager.players.Where(p => p != null))
            {
                if (selectedPlayer == null) selectedPlayer = player;
                if (player.Handle().IsUnkUser()) GUI.contentColor = Settings.c_primary.GetColor();
                if (selectedPlayer.GetInstanceID() == player.GetInstanceID()) GUI.contentColor = Settings.c_espPlayers.GetColor();
                if (GUILayout.Button(player.GetName(), GUI.skin.label)) selectedPlayer = player;
                GUI.contentColor = Settings.c_menuText.GetColor();
            }
            GUILayout.EndVertical();
        }
    }
}
