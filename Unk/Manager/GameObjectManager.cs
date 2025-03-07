using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unk.Util;
using Object = UnityEngine.Object;

namespace Unk.Manager
{
    [HarmonyPatch]
    public static class GameObjectManager
    {
        private static Queue<Action> ObjectQueue = new Queue<Action>();
        private static bool CoroutineStarted = false;

        public static List<PlayerAvatar> UnkPlayers = new List<PlayerAvatar>();
        public static List<Enemy> enemies = new List<Enemy>();
        public static List<PlayerAvatar> players = new List<PlayerAvatar>();
        public static List<Trap> traps = new List<Trap>();
        public static List<ValuableObject> items = new List<ValuableObject>();
        public static List<ExtractionPoint> extractions = new List<ExtractionPoint>();
        public static List<PlayerDeathHead> deathHeads = new List<PlayerDeathHead>();
        public static List<PhysGrabCart> carts = new List<PhysGrabCart>();
        public static TruckScreenText truck;

        [HarmonyPatch(typeof(ValuableObject), "Awake"), HarmonyPostfix]
        public static void Awake(ValuableObject __instance) => AddToObjectQueue(() => items.Add(__instance));

        [HarmonyPatch(typeof(PlayerAvatar), "Awake"), HarmonyPostfix]
        public static void Awake(PlayerAvatar __instance) => AddToObjectQueue(() => players.Add(__instance));

        [HarmonyPatch(typeof(Enemy), "Awake"), HarmonyPostfix]
        public static void Awake(Enemy __instance) => AddToObjectQueue(() => enemies.Add(__instance));

        [HarmonyPatch(typeof(Trap), "Start"), HarmonyPostfix]
        public static void Start(Trap __instance) => AddToObjectQueue(() => traps.Add(__instance));

        [HarmonyPatch(typeof(ExtractionPoint), "Start"), HarmonyPostfix]
        public static void Start(ExtractionPoint __instance) => AddToObjectQueue(() => extractions.Add(__instance));

        [HarmonyPatch(typeof(PlayerDeathHead), "Start"), HarmonyPostfix]
        public static void Start(PlayerDeathHead __instance) => AddToObjectQueue(() => deathHeads.Add(__instance));

        [HarmonyPatch(typeof(PhysGrabCart), "Start"), HarmonyPostfix]
        public static void Start(PhysGrabCart __instance) => AddToObjectQueue(() => carts.Add(__instance));

        [HarmonyPatch(typeof(TruckScreenText), "Start"), HarmonyPostfix]
        public static void Start(TruckScreenText __instance) => AddToObjectQueue(() => truck = __instance);

        [HarmonyPatch(typeof(PlayerAvatar), "ChatMessageSendRPC"), HarmonyPostfix]
        public static void ChatMessageSendRPC(PlayerAvatar __instance, string _message)
        {
            if (_message == "" && !UnkPlayers.Contains(__instance))
            {
                UnkPlayers.Add(__instance);
                if (!__instance.GetLocalPlayer()) Unk.Instance.AlertUsingUnkMenu();
            }
        }

        public static void CollectObjects()
        {
            CollectObjects(enemies);
            CollectObjects(players);
            CollectObjects(traps);
            CollectObjects(items);
            CollectObjects(extractions);
            CollectObjects(deathHeads);
            CollectObjects(carts);
            truck = Object.FindAnyObjectByType<TruckScreenText>();
        }

        public static void ClearObjects()
        {
            enemies?.Clear();
            players?.Clear();
            traps?.Clear();
            items?.Clear();
            extractions?.Clear();
            deathHeads?.Clear();
            carts?.Clear();
            truck = null;
        }

        private static void CollectObjects<T>(List<T> list, Func<T, bool> filter = null) where T : MonoBehaviour
        {
            list.Clear();
            list.AddRange(filter == null ? Object.FindObjectsOfType<T>() : Object.FindObjectsOfType<T>().Where(filter));
        }

        public static void AddToObjectQueue(Action action)
        {
            ObjectQueue.Enqueue(action);
            if (!CoroutineStarted) Unk.Instance.StartCoroutine(RunObjectQueue());
        }

        private static IEnumerator RunObjectQueue()
        {
            CoroutineStarted = true;
            while (ObjectQueue.Count > 0)
            {
                yield return new WaitForSeconds(0.1f * 0.1f);
                ObjectQueue.Dequeue()?.Invoke();
            }
            CoroutineStarted = false;
        }
    }
}
