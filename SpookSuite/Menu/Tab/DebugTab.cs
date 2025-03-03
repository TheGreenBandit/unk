using ExitGames.Client.Photon;
using Photon.Pun;
using Unk.Menu.Core;
using UnityEngine;

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


            GUILayout.BeginHorizontal();
            GUILayout.Label("Become Master Client");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
                //PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);

                //PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer);

            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Add $1000");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
            }
            GUILayout.EndHorizontal();

            //GUILayout.EndScrollView();
        }
    }
}
