using System;
using HarmonyLib;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unk.Util;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unk.Manager
{
    public static class GameObjectManager
    {
        private static Queue<Action> ObjectQueue = new Queue<Action>();
        private static bool CoroutineStarted = false;

        public static List<Enemy> enemies = new List<Enemy>();
        public static List<PlayerController> players = new List<PlayerController>();
        public static List<Trap> traps = new List<Trap>();
        public static List<ValuableObject> items = new List<ValuableObject>();

        [HarmonyPatch(typeof(ValuableObject), "Awake"), HarmonyPostfix]
        public static void Awake(ValuableObject __instance) => AddToObjectQueue(() => items.Add(__instance));

        [HarmonyPatch(typeof(PlayerController), "Awake"), HarmonyPostfix]
        public static void Awake(PlayerController __instance) => AddToObjectQueue(() => players.Add(__instance));

        [HarmonyPatch(typeof(Enemy), "Awake"), HarmonyPostfix]
        public static void Awake(Enemy __instance) => AddToObjectQueue(() => enemies.Add(__instance));

        [HarmonyPatch(typeof(Trap), "Start"), HarmonyPostfix]
        public static void Start(Trap __instance) { Debug.Log("aaa"); AddToObjectQueue(() => traps.Add(__instance)); }

        public static void CollectObjects()
        {
            CollectObjects(enemies);
            CollectObjects(players);
            CollectObjects(traps);
            CollectObjects(items);
        }

        public static void ClearObjects()
        {
            enemies.Clear();
            players.Clear();
            traps.Clear();
            items.Clear();
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
