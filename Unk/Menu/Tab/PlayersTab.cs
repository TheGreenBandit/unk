using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unk.Cheats.Core;
using Unk.Manager;
using Unk.Util;
using Unk.Menu.Core;
using Unk.Handler;

namespace Unk.Menu.Tab
{
    internal class PlayersTab : MenuTab
    {
        public PlayersTab() : base("Players") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        public static PlayerAvatar selectedPlayer = null;

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

            UI.Header("ALL Players");
            //options

            if (PlayerAvatar.instance.Handle().IsDev()) //all player options we wanna do but dont want our users doing
            {
                UI.Header("Dev Only Non Unk Player Options");
                //UI.Checkbox("Freeze Others", Cheat.Instance<FreezeAll>());
            }
        }

        private void PlayerActions()
        {
            if (selectedPlayer is null) return;

            if (selectedPlayer.Handle().IsUnkUser() && PlayerAvatar.instance.Handle().IsDev())
            {
                UI.Header("Unk Specialty");
                //add things that we could do to our users for fun, maybe disabling something in their menu?
                UI.Button("WASSUP", () => { });
            }

            UI.Header("Selected Player Actions");

            GUILayout.TextArea("SteamID: " + (selectedPlayer.Handle().IsDev() ? 0 : selectedPlayer.GetSteamID()));
            UI.Button("Go To Profile", () => System.Diagnostics.Process.Start("https://steamcommunity.com/profiles/" + selectedPlayer.GetSteamID()));
            UI.Label("Unk User", selectedPlayer.Handle().IsUnkUser().ToString());

            if (!PlayerAvatar.instance.Handle().IsDev() && selectedPlayer.Handle().IsDev())
            {
                UI.Label("User IS Dev So You Cant Do Anything :) Make sure to say hi!");
                return;
            }

            UI.Button("Clone", () => selectedPlayer.photonView.RPC("SpawnRPC", RpcTarget.All, selectedPlayer.transform.position, selectedPlayer.transform.rotation));
            UI.Button("Crash", () => selectedPlayer.photonView.RPC("OutroStartRPC", selectedPlayer.PhotonPlayer()));
            UI.Button("Disable", () => selectedPlayer.photonView.RPC("SetDisabledRPC", selectedPlayer.PhotonPlayer()));
            UI.Button("Kill", () => selectedPlayer.photonView.RPC("PlayerDeathRPC", RpcTarget.All, 0));

            if (!selectedPlayer.IsLocalPlayer())
                UI.Button("Block RPCs", () => selectedPlayer.Handle().ToggleRPCBlock(), selectedPlayer.Handle().IsRPCBlocked() ? "UnBlock" : "Block");
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
                if (!player.IsValid()) continue;
                if (selectedPlayer is null) selectedPlayer = player;
                if (player.Handle().IsUnkUser()) GUI.contentColor = Settings.c_primary.GetColor();
                if (selectedPlayer.GetInstanceID() == player.GetInstanceID()) GUI.contentColor = Settings.c_espPlayers.GetColor();
                if (GUILayout.Button(player.PhotonPlayer().NickName, GUI.skin.label)) selectedPlayer = player;

                GUI.contentColor = Settings.c_menuText.GetColor();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
