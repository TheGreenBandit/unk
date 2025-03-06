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
    }
}
