using HarmonyLib;
using Photon.Pun;
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
        public static List<ValuableObject> items = new List<ValuableObject>();
        public static List<ItemAttributes> items2 = new List<ItemAttributes>();
        public static List<ExtractionPoint> extractions = new List<ExtractionPoint>();
        public static List<PlayerDeathHead> deathHeads = new List<PlayerDeathHead>();
        public static List<PhysGrabCart> carts = new List<PhysGrabCart>();
        public static TruckScreenText truck;

        [HarmonyPatch(typeof(ValuableObject), "Awake"), HarmonyPrefix]
        public static void Awake(ValuableObject __instance) => AddToObjectQueue(() => items.Add(__instance));

        [HarmonyPatch(typeof(PlayerAvatar), "Awake"), HarmonyPrefix]
        public static void Awake(PlayerAvatar __instance) => AddToObjectQueue(() => players.Add(__instance));

        [HarmonyPatch(typeof(Enemy), "Awake"), HarmonyPrefix]
        public static void Awake(Enemy __instance) => AddToObjectQueue(() => enemies.Add(__instance));

        [HarmonyPatch(typeof(ExtractionPoint), "Start"), HarmonyPrefix]
        public static void Start(ExtractionPoint __instance) => AddToObjectQueue(() => extractions.Add(__instance));

        [HarmonyPatch(typeof(PlayerDeathHead), "Start"), HarmonyPrefix]
        public static void Start(PlayerDeathHead __instance) => AddToObjectQueue(() => deathHeads.Add(__instance));

        [HarmonyPatch(typeof(PhysGrabCart), "Start"), HarmonyPrefix]
        public static void Start(PhysGrabCart __instance) => AddToObjectQueue(() => carts.Add(__instance));

        [HarmonyPatch(typeof(ItemAttributes), "Awake"), HarmonyPrefix]
        public static void Awake(ItemAttributes __instance) => AddToObjectQueue(() => items2.Add(__instance));

        [HarmonyPatch(typeof(TruckScreenText), "Start"), HarmonyPrefix]
        public static void Start(TruckScreenText __instance) => AddToObjectQueue(() => truck = __instance);

        //[HarmonyPatch(typeof(PlayerAvatar), "ChatMessageSendRPC"), HarmonyPrefix]
        //public static void ChatMessageSendRPC(PlayerAvatar __instance, string _message)
        //{
        //    if (_message == "" && !UnkPlayers.Contains(__instance))
        //    {
        //        UnkPlayers.Add(__instance);
        //        if (!__instance.GetLocalPlayer()) Unk.Instance.AlertUsingUnkMenu();
        //    }
        //}

        [HarmonyPatch(typeof(PhotonNetwork), "RemoveInstantiatedGO"), HarmonyPrefix]
        public static void RemoveInstantiatedGO(GameObject go, bool localOnly)
        {
            if (go?.GetComponent<ValuableObject>() is { } valuableObject) items.Remove(valuableObject);
            if (go?.GetComponent<PlayerAvatar>() is { } playerAvatar) players.Remove(playerAvatar);
            if (go?.GetComponent<Enemy>() is { } enemy) enemies.Remove(enemy);
            if (go?.GetComponent<ExtractionPoint>() is { } extractionPoint) extractions.Remove(extractionPoint);
            if (go?.GetComponent<PlayerDeathHead>() is { } playerDeathHead) deathHeads.Remove(playerDeathHead);
            if (go?.GetComponent<PhysGrabCart>() is { } physGrabCart) carts.Remove(physGrabCart);
            if (go?.GetComponent<ItemAttributes>() is { } itemAttributes) items2.Remove(itemAttributes);
        }

        public static void CollectObjects()
        {
            CollectObjects(enemies);
            CollectObjects(players);
            CollectObjects(items);
            CollectObjects(items2);
            CollectObjects(extractions);
            CollectObjects(deathHeads);
            CollectObjects(carts);
            truck = Object.FindAnyObjectByType<TruckScreenText>();
        }

        public static void ClearObjects()
        {
            enemies?.Clear();
            players?.Clear();
            items?.Clear();
            items2?.Clear();
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
