using System;
using System.Collections.Generic;
using UnityEngine;
using Unk.Cheats.Core;
using Unk.Manager;
using Unk.Util;

namespace Unk.Cheats
{
    internal class ESP : ToggleCheat
    {
        public static bool displayPlayers = false;
        public static bool displayEnemies = false;
        public static bool displayItems = false;
        public static bool displayDivingBell = false;
        public static bool displayTraps = false;

        public override void OnGui()
        {
            if (!Cheat.Instance<ESP>().Enabled) return;
            if (displayPlayers)
                DisplayPlayers();
            if (displayItems)
                DisplayItems();
            if (displayEnemies)
                DisplayEnemies();
            if (displayTraps)
                DisplayTraps();
        }

        public static void ToggleAll()
        {
            displayPlayers = !displayPlayers;
            displayEnemies = !displayEnemies;
            displayItems = !displayItems;
            displayTraps = !displayTraps;
        }
        private void DisplayObjects<T>(IEnumerable<T> objects, Func<T, string> labelSelector, Func<T, RGBAColor> colorSelector) where T : Component
        {
            try
            {
                foreach (T obj in objects)
                {
                    if (obj != null && obj.gameObject.activeSelf)
                    {
                        float distance = GetDistanceToPos(obj.transform.position);

                        if (!WorldToScreen(obj.transform.position, out Vector3 screen)) continue;

                        VisualUtil.DrawDistanceString(screen, labelSelector(obj), colorSelector(obj), distance);
                    }
                }
            }
            catch (Exception e) { }
        }

        private void DisplayPlayers()
        {
            foreach (PlayerController p in GameObjectManager.players)
            {
                if (p == PlayerController.instance) continue; //check if local

                float distance = GetDistanceToPos(p.transform.up);

                if (!WorldToScreen(p.transform.up, out Vector3 screen)) continue;

                VisualUtil.DrawDistanceString(screen, p.GetName(), Settings.c_espPlayers, distance);
            }
        }

        private void DisplayItems()
        {
            //DisplayObjects(GameObjectManager.items, item => item.item.GetName(), item => Settings.c_espItems);
        }

        private void DisplayTraps()
        {
            DisplayObjects(GameObjectManager.traps, trap => trap.name.Replace("(Clone)", ""), trap => Settings.c_espTraps);
        }

        private void DisplayEnemies()
        {
            foreach (Enemy e in GameObjectManager.enemies)
            {
                if (e is null) continue;
                float distance = GetDistanceToPos(e.CenterTransform.position);
                if (!WorldToScreen(e.CenterTransform.position, out Vector3 screen)) continue;
                VisualUtil.DrawDistanceString(screen, e.Reflect().GetValue<EnemyParent>("EnemyParent").enemyName, Settings.c_espEnemies, distance);
            }
        }
    }
}
