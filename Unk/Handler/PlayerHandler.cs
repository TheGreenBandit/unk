using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unk.Cheats.Components;
using Unk.Manager;
using Unk.Util;

namespace Unk.Handler
{
    public class PlayerHandler
    {
        private static List<string> rpcBlockedClients = new List<string>();
        public static Dictionary<string, Queue<RPCData>> rpcHistory = new Dictionary<string, Queue<RPCData>>();
        public static List<PlayerAvatar> GetAlivePlayers() => GameObjectManager.players.Where(p => p != null && !p.IsDead()).ToList();

        private PlayerAvatar player;
        public Player photonPlayer => player.photonView.Owner;
        public string steamId => player.Reflect().GetValue<string>("steamID");

        public PlayerHandler(PlayerAvatar player)
        {
            this.player = player;
        }

        public static void ClearRPCHistory() => rpcHistory.Clear();

        public Enemy GetClosestEnemy() => GameObjectManager.enemies.OrderBy(x => Vector3.Distance(x.transform.position, player.transform.position)).FirstOrDefault();
        //todo
        public PlayerAvatar GetClosestPlayer() => GetAlivePlayers().Where(x => x.GetInstanceID() != player.GetInstanceID()).OrderBy(x => Vector3.Distance(x.transform.position, player.transform.position)).FirstOrDefault();

        public void RPC(string name, RpcTarget target, params object[] args) => player.photonView.RPC(name, target, args);

        public bool IsRPCBlocked() => photonPlayer is not null && rpcBlockedClients.Contains(steamId) && !IsDev();

        public bool IsDev() => (player.GetSteamID() == "76561199159991462") || (player.GetSteamID() == "76561198093261109") || (player.GetSteamID() == "76561198846294221");

        public bool IsUnkUser() => player != null && GameObjectManager.UnkPlayers.Contains(player);

        public void BlockRPC()
        {
            if (IsRPCBlocked() || photonPlayer is null && IsDev()) return;
            rpcBlockedClients.Add(steamId);
        }

        public void UnblockRPC()
        {
            if (!IsRPCBlocked() || photonPlayer is null) return;
            rpcBlockedClients.Remove(steamId);
        }

        public void ToggleRPCBlock()
        {
            if (photonPlayer is null && IsDev()) return;
            if (IsRPCBlocked()) rpcBlockedClients.Remove(steamId);
            else rpcBlockedClients.Add(steamId);
        }

        public Queue<RPCData> GetRPCHistory()
        {
            if (!rpcHistory.ContainsKey(steamId))
                rpcHistory.Add(steamId, new Queue<RPCData>());
            return rpcHistory[steamId];
        }
        public List<RPCData> GetRPCHistory(string rpc) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc));

        public List<RPCData> GetRPCHistory(string rpc, int seconds) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds));
        public List<RPCData> GetRPCHistory(string rpc, int seconds, bool suspected) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.suspected == suspected);
        public RPCData GetRPCMatch(string rpc, int seconds, object data) => GetRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.data.Equals(data));
        public RPCData GetRPCMatch(string rpc, int seconds, object data, bool suspected) => GetRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.data.Equals(data) && r.suspected == suspected);
        public RPCData GetRPCMatch(string rpc, int seconds, Func<object, bool> predicate) => GetRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data));
        public RPCData GetRPCMatch(string rpc, int seconds, Func<object, bool> predicate, bool suspected) => GetRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data) && r.suspected == suspected);
        public bool HasSentRPC(string rpc, int seconds) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds)).Count > 0;
        public bool HasSentRPC(string rpc, int seconds, bool suspected) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.suspected == suspected).Count > 0;
        public bool HasSentRPC(string rpc, int seconds, object data) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.data.Equals(data)).Count > 0;
        public bool HasSentRPC(string rpc, int seconds, Func<object, bool> predicate) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data)).Count > 0;
        public bool HasSentRPC(string rpc, int seconds, Func<object, bool> predicate, bool suspected) => GetRPCHistory().ToList().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data) && r.suspected == suspected).Count > 0;
        public List<RPCData> GetAllRPCHistory() => rpcHistory.Values.SelectMany(x => x).ToList();
        public List<RPCData> GetAllRPCHistory(int seconds) => GetAllRPCHistory().FindAll(r => r.IsRecent(seconds));
        public List<RPCData> GetAllRPCHistory(string rpc, int seconds) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds));
        public List<RPCData> GetAllRPCHistory(string rpc, int seconds, bool suspected) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.suspected == suspected);
        public RPCData GetAnyRPCMatch(string rpc, int seconds, object data) => GetAllRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.data.Equals(data));
        public RPCData GetAnyRPCMatch(string rpc, int seconds, object data, bool suspected) => GetAllRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.data.Equals(data) && r.suspected == suspected);
        public RPCData GetAnyRPCMatch(string rpc, int seconds, Func<object, bool> predicate) => GetAllRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data));
        public RPCData GetAnyRPCMatch(string rpc, int seconds, Func<object, bool> predicate, bool suspected) => GetAllRPCHistory().FirstOrDefault(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data) && r.suspected == suspected);
        public bool HasAnySentRPC(string rpc, int seconds) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds)).Count > 0;
        public bool HasAnySentRPC(string rpc, int seconds, bool suspected) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.suspected == suspected).Count > 0;
        public bool HasAnySentRPC(string rpc, int seconds, object data) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && r.data.Equals(data)).Count > 0;
        public bool HasAnySentRPC(string rpc, int seconds, Func<object, bool> predicate) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data)).Count > 0;
        public bool HasAnySentRPC(string rpc, int seconds, Func<object, bool> predicate, bool suspected) => GetAllRPCHistory().FindAll(r => r.rpc.StartsWith(rpc) && r.IsRecent(seconds) && predicate(r.data) && r.suspected == suspected).Count > 0;

        public bool OnReceivedRPC(string rpc, Hashtable rpcHash)
        {
            if (player is null || photonPlayer is null || (player.GetSteamID() == PlayerAvatar.instance.GetSteamID())) return true;

            RPCData rpcData = new RPCData(photonPlayer, rpc, rpcHash);

            object[] parameters = null;
            if (rpcHash.ContainsKey(Patches.keyByteFour))
                parameters = (object[])rpcHash[Patches.keyByteFour];

            if (!Patches.IgnoredRPCDebugs.Contains(rpc) && parameters != null) Debug.LogWarning($"RPC Params '{string.Join(", ", parameters.Select(p => p?.ToString() ?? "null"))}'");

            if (rpc.Equals("OutroStartRPC") && !HasSentRPC("ModulesReadyRPC", 10))
            {
                Debug.LogError($"{photonPlayer.NickName} is probably trying to crash you!");
                rpcData.SetSuspected();
                return false;
            }

            if (rpc.Equals("SetDisabledRPC") && GameDirector.instance.Reflect().GetValue<bool>("gameStateStartImpulse"))
            {
                Debug.LogError($"{photonPlayer.NickName} is probably trying to disable you!");
                rpcData.SetSuspected();
                return false;
            }

            GetRPCHistory().Enqueue(rpcData);
            CleanupRPCHistory();
            return true;
        }

        private void CleanupRPCHistory()
        {
            var queue = GetRPCHistory();
            while (queue.Count > 0 && queue.Peek().IsExpired()) queue.Dequeue();
        }
    }

    

    public static class PlayerExtensions
    {
        public static PlayerHandler Handle(this PlayerAvatar player) => new PlayerHandler(player);
        public static Player PhotonPlayer(this PlayerAvatar player) => player.photonView.Owner;
        public static string GetSteamID(this PlayerAvatar player) => player.Reflect().GetValue<string>("steamID");
        public static bool IsValid(this PlayerAvatar player) => player is not null || player.IsDead();
    }

    public static class PhotonPlayerExtensions
    {
        public static string GetSteamID(this Player photonPlayer)
        {
            return GamePlayer(photonPlayer).GetSteamID();
        }
        public static PlayerAvatar GamePlayer(this Player photonPlayer)
        {
            if (GameObjectManager.players == null) return null;
            return GameObjectManager.players.Find(x => x != null && x.PhotonPlayer() != null && x.PhotonPlayer().ActorNumber == photonPlayer.ActorNumber);
        }
    }
}
