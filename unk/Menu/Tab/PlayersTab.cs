﻿using CurvedUI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using unk.Handler;
using unk.Manager;
using unk.Menu.Core;
using unk.Util;
using System;
using UnityEngine;

namespace unk.Menu.Tab
{
    internal class PlayersTab : MenuTab
    {
        public PlayersTab() : base("Players") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        public Player selectedPlayer = null;

        public int num;

        public override void Draw()
        {
            GUILayout.BeginVertical(GUILayout.Width(unkMenu.Instance.contentWidth * 0.3f - unkMenu.Instance.spaceFromLeft));
            PlayersList();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(unkMenu.Instance.contentWidth * 0.7f - unkMenu.Instance.spaceFromLeft));
            scrollPos2 = GUILayout.BeginScrollView(scrollPos2);
            GeneralActions();
            PlayerActions();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void GeneralActions()
        {
            GUILayout.Label("ALL Players");
            if (GUILayout.Button("Kick All (NonHost)"))
            {
                //if (PhotonNetwork.LocalPlayer.IsMasterClient) // need fixing
                //{
                //    for (int i = 0; i < GameObjectManager.players.Count; i++)
                //    {
                //        //if (!GameObjectManager.players[i].IsLocal)
                //            PhotonNetwork.CloseConnection(GameObjectManager.players[i].refs.view.Owner);
                //    }                  
                //}
                //else
                    Cheats.KickAll.Execute();
            }
        }
        private void PlayerActions()
        {

            UI.Header("Selected Player Actions");
            UI.Button("Teleport", () => selectedPlayer.transform.position = Player.localPlayer.HeadPosition(), "Teleport");
            UI.Button("Bring", () => selectedPlayer.transform.position = Player.localPlayer.HeadPosition(), "Bring");
            UI.Button("Nearby Monsters Attack", () => selectedPlayer.GetClosestMonster().SetTargetPlayer(selectedPlayer), "Nearby Monsters Attack");
            UI.Button("All Monsters Attack", () => GameObjectManager.monsters.ForEach(m => m.SetTargetPlayer(selectedPlayer)), "All Monsters Attack");
            UI.Button("Spawn Bomb", () => GameUtil.SpawnItem(58, selectedPlayer.data.groundPos), "Bomb");
            UI.Button("Kill", () => selectedPlayer.Reflect().Invoke("CallDie"), "Kill");
            UI.Button("Revive", () => selectedPlayer.CallRevive(), "Kill");
        }

        private void PlayersList()
        {
            float width = unkMenu.Instance.contentWidth * 0.3f - unkMenu.Instance.spaceFromLeft * 2;
            float height = unkMenu.Instance.contentHeight - 20;

            Rect rect = new Rect(0, 0, width, height);
            GUI.Box(rect, "Player List");

            GUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(height));

            GUILayout.Space(25);
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            foreach (Player player in GameObjectManager.players)
            {
                //if (player.disconnectedMidGame || !player.IsSpawned) continue;
                if (player.ai) continue;

                if (selectedPlayer is null) selectedPlayer = player;

                if (selectedPlayer.GetInstanceID() == player.GetInstanceID()) GUI.contentColor = Settings.c_espPlayers.GetColor();

                if (GUILayout.Button(player.refs.view.Owner.NickName, GUI.skin.label)) selectedPlayer = player;

                GUI.contentColor = Settings.c_menuText.GetColor();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
