using Steamworks.Data;
using UnityEngine;
using Unk.Manager;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk.Menu.Tab
{
    internal class LobbyManagerTab : MenuTab //todo really
    {
        public LobbyManagerTab() : base("Lobby Manager") { }
        private Vector2 scrollPos = Vector2.zero;
        public static Lobby selectedLobby = new Lobby();

        public override void Draw()
        {
            UI.HorizontalSpace("", () =>
            {
                UI.VerticalSpace(ServerList, GUILayout.Width(UnkMenu.Instance.contentWidth * 0.3f - UnkMenu.Instance.spaceFromLeft));
                UI.VerticalSpace(ServerActions, GUILayout.Width(UnkMenu.Instance.contentWidth * 0.7f - UnkMenu.Instance.spaceFromLeft));
            });
        }

        private void ServerList()//todo add sorting, most players, least players, friends only, etc
        {
            if (LobbyManager.lobbyList.Count == 0)
                return;

            float width = UnkMenu.Instance.contentWidth * 0.3f - UnkMenu.Instance.spaceFromLeft * 2;
            float height = UnkMenu.Instance.contentHeight - 20;

            Rect rect = new Rect(0, 0, width, height);
            GUI.Box(rect, "Server List");

            UI.VerticalSpace(() =>
            {
                GUILayout.Space(25);
                UI.ScrollView(ref scrollPos, () =>
                {                    
                    foreach (Lobby lobby in LobbyManager.lobbyList)
                    {
                        //check if blacklisted or matches sort
                        if (GUILayout.Button($"{lobby.MemberCount}/{lobby.MaxMembers} {lobby.Owner.Name}'s Lobby", GUI.skin.label)) selectedLobby = lobby;
                    }  
                });
            }, GUILayout.Width(width), GUILayout.Height(height));
            GUILayout.EndVertical();
        }

        private void ServerActions()//maybe add lobby blacklist, viewing players info, etc
        {
            UI.Button("Refresh Lobby List", LobbyManager.RefreshLobbies); //todo get me working
            UI.Button($"Join {selectedLobby.Owner.Name}'s Lobby", async () => await LobbyManager.TryToJoinInLobby(selectedLobby.Id.Value));
        }
    }
}
