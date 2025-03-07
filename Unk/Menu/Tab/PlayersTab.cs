using Photon.Pun;
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
        private string message = "I'm dumb";
        private string color = "-1";

        public override void Draw()
        {
            GUILayout.BeginVertical(GUILayout.Width(UnkMenu.Instance.contentWidth * 0.3f - UnkMenu.Instance.spaceFromLeft));
            PlayersList();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(UnkMenu.Instance.contentWidth * 0.7f - UnkMenu.Instance.spaceFromLeft));
            scrollPos2 = GUILayout.BeginScrollView(scrollPos2);
            GeneralActions();
            PlayerActions();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void GeneralActions()
        {
            if (!PhotonNetwork.InRoom) return;

            UI.Header("General Actions");

            UI.Button("Kill All", () =>
            {
                GameObjectManager.players.Where(p => p != null).ToList().ForEach(p => p.photonView.RPC("PlayerDeathRPC", RpcTarget.All, 0));
            });

            UI.Button("Kill others", () =>
            {
                GameObjectManager.players.Where(p => p != null && !p.IsLocalPlayer()).ToList().ForEach(p => p.photonView.RPC("PlayerDeathRPC", RpcTarget.All, 0));
            });

            UI.Button("Crash All", () =>
            {
                GameObjectManager.players.Where(p => p != null).ToList().ForEach(p => p.photonView.RPC("OutroStartRPC", p.PhotonPlayer()));
            });

            UI.Button("Crash others", () =>
            {
                GameObjectManager.players.Where(p => p != null && !p.IsLocalPlayer()).ToList().ForEach(p => p.photonView.RPC("OutroStartRPC", p.PhotonPlayer()));
            });
        }

        private void PlayerActions()
        {
            if (selectedPlayer == null) return;

            UI.Header("Selected Player Actions");

            GUILayout.TextArea($"SteamID: {selectedPlayer.GetSteamID()}");
            UI.Button("Go To Profile", () => Process.Start($"https://steamcommunity.com/profiles/{selectedPlayer.GetSteamID()}"));
            UI.Label("Unk User", selectedPlayer.Handle().IsUnkUser().ToString());
            UI.Button("Heal", () => selectedPlayer.playerHealth.Reflect().GetValue<PhotonView>("photonView").RPC("UpdateHealthRPC", RpcTarget.All, selectedPlayer.playerHealth.Reflect().GetValue<int>("maxHealth"), selectedPlayer.playerHealth.Reflect().GetValue<int>("maxHealth"), false));
            UI.Button("Crown", () => selectedPlayer.photonView.RPC("CrownPlayerRPC", RpcTarget.All, selectedPlayer.GetSteamID()));
            UI.Button("Crash", () => selectedPlayer.photonView.RPC("OutroStartRPC", selectedPlayer.PhotonPlayer()));
            UI.Button("Disable", () => selectedPlayer.photonView.RPC("SetDisabledRPC", selectedPlayer.PhotonPlayer()));
            UI.Button("Kill", () => selectedPlayer.photonView.RPC("PlayerDeathRPC", RpcTarget.All, 0));
            UI.Button("Revive", () => selectedPlayer.RevivePlayer());
            UI.Button("Force Jump", () => selectedPlayer.photonView.RPC("JumpRPC", RpcTarget.All, true));
            UI.Button("Force Land", () => selectedPlayer.photonView.RPC("LandRPC", RpcTarget.All));
            UI.TextboxAction("Chat Message", ref message, 100,
                new UIButton("Send", () => selectedPlayer.photonView.RPC("ChatMessageSendRPC", RpcTarget.All, message, false) 
            ));
            UI.TextboxAction("Change Color", ref color, 2,
                new UIButton("Set", () => selectedPlayer.photonView.RPC("SetColorRPC", RpcTarget.All, int.Parse(color))
            ));
            UI.Button("Lure monsters to player", () =>
            {
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
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            foreach (PlayerAvatar player in GameObjectManager.players)
            {
                if (selectedPlayer == null) selectedPlayer = player;
                if (player.Handle().IsUnkUser()) GUI.contentColor = Settings.c_primary.GetColor();
                if (selectedPlayer.GetInstanceID() == player.GetInstanceID()) GUI.contentColor = Settings.c_espPlayers.GetColor();
                if (GUILayout.Button(player.GetName(), GUI.skin.label)) selectedPlayer = player;

                GUI.contentColor = Settings.c_menuText.GetColor();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
