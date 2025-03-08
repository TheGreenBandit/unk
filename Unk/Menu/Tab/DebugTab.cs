using ExitGames.Client.Photon;
using Photon.Pun;
using Unk.Menu.Core;
using UnityEngine;
using Unk.Util;
using Unk.Cheats.Core;
using Unk.Cheats;
using System.Linq;
using System.Collections.Generic;
using System;
using Unk.Manager;
using Photon.Realtime;
using Steamworks;
using Steamworks.Data;
using Object = UnityEngine.Object;
using static UnityEngine.EventSystems.EventTrigger;

namespace Unk.Menu.Tab
{
    internal class DebugTab : MenuTab
    {
        public DebugTab() : base("Debug") { }
        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            GUILayout.BeginVertical();
            MenuContent();
            GUILayout.EndVertical();         
        }

        private void MenuContent()
        {
            //scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Master Client: ");
            GUILayout.FlexibleSpace();
            GUILayout.Label(PhotonNetwork.IsMasterClient ? "Yes" : "No");
            GUILayout.EndHorizontal();
            UI.Button("Log RPCS", () => PhotonNetwork.PhotonServerSettings.RpcList.ToList().ForEach(r => Debug.Log(r)));

            UI.Button("Host Lobby", SteamManager.instance.HostLobby);
            UI.Button("Set Lobby Public", () => SetPublic(true));
            UI.Button("Set Lobby Private", () => SetPublic(false));
            UI.Button("Set Lobby Joinable", () => SetJoinable(true));
            UI.Button("Set Lobby NonJoinable", () => SetJoinable(false));
            
            //	public Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>(); //get item names from here
            if (StatsManager.instance.itemDictionary != null)
            {
                for (int i = 0; i < StatsManager.instance.itemDictionary.Count; i++)
                    UI.Button(StatsManager.instance.itemDictionary.ElementAt(i).Key, () => { PunManager.instance.AddingItem(StatsManager.instance.itemDictionary.ElementAt(i).Key, StatsManager.instance.GetIndexThatHoldsThisItemFromItemDictionary(StatsManager.instance.itemDictionary.ElementAt(i).Key), -1, null); });
            }
            //UI.Button("AddingItemRPC test", () => { PunManager.instance.AddingItem("name", StatsManager.instance.GetIndexThatHoldsThisItemFromItemDictionary("anem"), -1, null); });

            UI.Button("Revive all", () =>
            {
                GameObjectManager.players.Where(p => p != null && p.IsDead()).ToList().ForEach(p => p.Revive());
            });

            UI.Button("Revive localPlayer", () =>
            {
                PlayerAvatar.instance.GetLocalPlayer().Revive();
            });

            // use for target enemy.SetChaseTarget

            UI.Button("Raycast", () =>
            {
                string debugMessage = "";
                foreach (RaycastHit hit in SemiFunc.MainCamera().transform.SphereCastForward())
                {
                    Collider collider = hit.collider;
                    debugMessage += $"Hit: {collider.name} => {collider.gameObject.name} => Layer {LayerMask.LayerToName(collider.gameObject.layer)} {collider.gameObject.layer}\n";
                }
                Debug.Log(debugMessage);
            });

            UI.Button("Unload test", Loader.Unload);

            //GUILayout.EndScrollView();
        }
        internal static void SetPublic(bool value)
        {
            Lobby l = SteamManager.instance.Reflect().GetValue<Lobby>("currentLobby");
            if (value) l.SetPublic();
            else l.SetPrivate();
        }

        internal static void SetJoinable(bool value)
        {
            Lobby l = SteamManager.instance.Reflect().GetValue<Lobby>("currentLobby");
            l.SetJoinable(value);
        }
    }

}
