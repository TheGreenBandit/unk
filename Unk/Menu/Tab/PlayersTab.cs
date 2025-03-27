using Photon.Pun;
using System;
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
        private int color = 0;

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

            UI.VerticalSpace(() =>
            {
                UI.SubHeader("Blackhole");

                UI.Checkbox("Toggle Blackhole", Cheat.Instance<BlackHole>());
                UI.Checkbox("Pull Me Too", ref BlackHole.self);
                UI.Button("Set Position To Me", () => { BlackHole.pos = PlayerAvatar.instance.playerTransform.position; });
                UI.Slider("Strength", BlackHole.strength.ToString(), ref BlackHole.strength, 1, 20);
            });

            UI.Button("Kill All", () => {
                GameObjectManager.players.Where(p => p != null).ToList().ForEach(p => p.photonView.RPC("PlayerDeathRPC", RpcTarget.All, 0));
            });

            UI.Button("Kill others", () => {
                GameObjectManager.players.Where(p => p != null && !p.IsLocalPlayer()).ToList().ForEach(p => p.photonView.RPC("PlayerDeathRPC", RpcTarget.All, 0));
            });

            UI.Button("Kick All", () =>
            {
                PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer.IsMasterClient ? GameObjectManager.players.Find(x => x.GetSteamID() != PlayerAvatar.instance.GetSteamID()).PhotonPlayer() : PlayerAvatar.instance.PhotonPlayer());
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

            UI.SubHeader("Info");

            UI.Label("SteamId:", selectedPlayer.GetSteamID().ToString());
            UI.Label("Status:", selectedPlayer.IsDead() ? "Dead" : "Alive");
            UI.Label("Health:", selectedPlayer.GetHealth().ToString());
            UI.Label("Position: ", selectedPlayer.playerTransform.position.ToString());
            UI.Label("Is Master Client:", selectedPlayer.IsLocalPlayer() ? SemiFunc.IsMasterClientOrSingleplayer().ToString() : selectedPlayer.PhotonPlayer().IsMasterClient.ToString());
            UI.Label("Is Unk User:", selectedPlayer.Handle().IsUnkUser().ToString());

            UI.SubHeader("Nice");
            UI.Button("Heal", () => selectedPlayer.playerHealth.Reflect().GetValue<PhotonView>("photonView").RPC("UpdateHealthRPC", RpcTarget.All, selectedPlayer.playerHealth.Reflect().GetValue<int>("maxHealth"), selectedPlayer.playerHealth.Reflect().GetValue<int>("maxHealth"), false));
            UI.Button("Revive", () => selectedPlayer.RevivePlayer());
            UI.Button("Crown", () => selectedPlayer.photonView.RPC("CrownPlayerRPC", RpcTarget.All, selectedPlayer.GetSteamID())); //idk what crown does so good?
            UI.HorizontalSpace(null, () =>
            {
                UI.DrawColoredBox(" ", AssetManager.instance.playerColors[color], GUILayout.Width(20f), GUILayout.Height(20f));
                UI.NumSelect("Color", ref color, 0, AssetManager.instance.playerColors.Count - 1);
                UI.Button("Set", () => selectedPlayer.photonView.RPC("SetColorRPC", RpcTarget.All, color), null);
            });
            
            UI.SubHeader("Toxic");

            UI.Button("Set Blackhole Pos On Player", () => { BlackHole.pos = selectedPlayer.playerTransform.position; });
            UI.Button("Hurt Absolute", () => selectedPlayer.playerHealth.Reflect().GetValue<PhotonView>("photonView").RPC("UpdateHealthRPC", RpcTarget.All, 1, selectedPlayer.playerHealth.Reflect().GetValue<int>("maxHealth"), false));
            UI.Button($"Turn Godmode " + (PlayerGodmode.IsGodmode(selectedPlayer) ? "Off" : "On"), () => PlayerGodmode.ToggleGodmode(selectedPlayer));
            UI.Button("Crash (toxic af)", () => selectedPlayer.photonView.RPC("OutroStartRPC", selectedPlayer.PhotonPlayer()));
            UI.Button("Send to Void", () => { selectedPlayer.Revive(); selectedPlayer.Revive(); });
            UI.Button("Disable", () => selectedPlayer.photonView.RPC("SetDisabledRPC", selectedPlayer.PhotonPlayer()));
            UI.Button("Kill", () => selectedPlayer.photonView.RPC("PlayerDeathRPC", RpcTarget.All, 0));
            UI.Button("Force Jump", () => selectedPlayer.photonView.RPC("JumpRPC", RpcTarget.All, false));
            UI.Button("Force Tumble", () => selectedPlayer.tumble.Reflect().GetValue<PhotonView>("photonView").RPC("TumbleRequestRPC", RpcTarget.All, true, true));
            UI.TextboxAction("Chat Message", ref message, 100,
                new UIButton("Send", () => selectedPlayer.photonView.RPC("ChatMessageSendRPC", RpcTarget.All, message, false) 
            ));

            UI.Button("Lure monsters", () => {
                GameObjectManager.enemies.Where(e => e != null && !e.IsDead()).ToList().ForEach(e => e.SetChaseTarget(selectedPlayer));
            });
            //todo confirm this even works
            UI.CheatToggleSlider(Cheat.Instance<OverrideAnimSpeed>(), "Anim Speed Multiplier", OverrideAnimSpeed.Value.ToString(), ref OverrideAnimSpeed.Value, -10, 10);
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
