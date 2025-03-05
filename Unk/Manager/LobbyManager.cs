using Steamworks.Data;
using Steamworks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using Unk.Util;

namespace Unk.Manager
{
    internal class LobbyManager
    {
        public static List<Lobby> lobbyList;
        public static List<Lobby> lobbyListWithData;
        public static async Task JoinLobby(Lobby lobby)
        {
            if (await lobby.Join() == RoomEnter.Success)
            {
                lobbyListWithData.Add(lobby);
                lobby.Leave();
            }
            else lobby.Leave();
        }

        public static async void RefreshLobbies()
        {
            lobbyList = null;
            Debug.Log("Requested server list");
            LobbyQuery lobbyQuery = SteamMatchmaking.LobbyList;
            lobbyQuery = SteamMatchmaking.LobbyList.FilterDistanceWorldwide();
            Lobby[] array = await lobbyQuery.RequestAsync();
            lobbyList = array.ToList();
        }

        public static async Task TryToJoinInLobby(ulong lobby)
        {
            SteamId lobbyId2 = lobby;
            Debug.Log("[TryToJoinInLobby]SteamId lobby: " + lobbyId2.ToString());
            await SteamMatchmaking.JoinLobbyAsync(lobbyId2);
            MenuManager.instance.PageCloseAll();
            MenuManager.instance.PageOpen(MenuPageIndex.Main, false);
            if (RunManager.instance.levelCurrent != RunManager.instance.levelMainMenu)
            {
                foreach (PlayerAvatar player in GameDirector.instance.PlayerList)
                    player.OutroStartRPC();

                RunManager.instance.Reflect().SetValue("lobbyJoin", true);
                RunManager.instance.ChangeLevel(true, false, RunManager.ChangeLevelType.LobbyMenu);
            }
            RunManager.instance.Reflect().SetValue("joinLobby", true);
        }
    }
}
