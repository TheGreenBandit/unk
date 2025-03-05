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
            UI.Checkbox("Unlimited Energy", Cheat.Instance<UnlimitedStamina>());
            UI.Checkbox("Godmode", Cheat.Instance<Godmode>());
            UI.Checkbox("No Tumble", Cheat.Instance<NoTumble>());
            UI.CheatToggleSlider(Cheat.Instance<SuperSpeed>(), "Super Speed", SuperSpeed.Value.ToString("#"), ref SuperSpeed.Value, 10f, 100f);
            UI.Checkbox("Override Flashlight Color", Cheat.Instance<FlashlightColors>());
            UI.TextboxAction("Color", ref FlashlightColors.s_color, 8,
                new UIButton("Set", () => UI.SetColor(ref FlashlightColors.c_color, FlashlightColors.s_color))
            );

            UI.Button("Revive all", () =>
            {
                GameObjectManager.players.Where(p => p != null).ToList().ForEach(p => p.Revive());
            });

            UI.Button("Revive all", () =>
            {
                GameObjectManager.players.FirstOrDefault(p => p != null && p.IsLocalPlayer()).Revive();
            });

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

            UI.Button("Unload test", () =>
            {
                Loader.Unload();
            });

            //GUILayout.EndScrollView();
        }
    }
}
