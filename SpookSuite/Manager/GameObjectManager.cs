using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unk.Manager
{
    internal class GameObjectManager
    {
        private static GameObjectManager instance;
        public static GameObjectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObjectManager();
                }
                return instance;
            }
        }
        private float collectInterval = 1f;

        public static List<Enemy> enemies = new List<Enemy>();
        public static List<PlayerController> players = new List<PlayerController>();
        public static List<Trap> traps = new List<Trap>();
        public static List<Item> items = new List<Item>(); //changeme

        public IEnumerator CollectObjects()
        {
            while (true)
            {
                CollectObjects(enemies);
                CollectObjects(players);
                CollectObjects(traps);
                //CollectObjects(items); //todo

                yield return new WaitForSeconds(collectInterval);
            }
        }

        private void CollectObjects<T>(List<T> list, Func<T, bool> filter = null) where T : MonoBehaviour
        {
            list.Clear();
            list.AddRange(filter == null ? Object.FindObjectsOfType<T>() : Object.FindObjectsOfType<T>().Where(filter));
            //Debug.Log($"Collected {list.Count} objects of type {typeof(T).Name}");
        }
    }
}
