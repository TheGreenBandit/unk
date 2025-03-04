using Steamworks.Ugc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public override void OnGui()
        {
            if (!Cheat.Instance<ESP>().Enabled) return;
            if (displayPlayers) DisplayPlayers();
            if (displayItems) DisplayItems();
            if (displayEnemies) DisplayEnemies();
        }

        public static void ToggleAll()
        {
            displayPlayers = !displayPlayers;
            displayEnemies = !displayEnemies;
            displayItems = !displayItems;
        }

        private void DisplayObjects<T>(IEnumerable<T> objects, Func<T, string> labelSelector, Func<T, RGBAColor> colorSelector) where T : Component
        {
            if (objects == null) return;
            foreach (var obj in objects?.Where(o => o != null && o.gameObject.activeSelf))
            {
                if (obj.transform == null) continue;
                float distance = GetDistanceToPos(obj.transform.position);
                if (distance == 0f || !WorldToScreen(obj.transform.position, out var screen)) continue;
                VisualUtil.DrawDistanceString(screen, labelSelector(obj), colorSelector(obj), distance);
            }
        }

        private void DisplayPlayers()
        {
            DisplayObjects(
                GameObjectManager.players?.Where(p => p != null && !p.IsLocalPlayer()),
                player => $"{player.GetName()}",
                player => Settings.c_espPlayers
            );
        }

        private void DisplayItems()
        {
            DisplayObjects(
                GameObjectManager.items?.Where(i => i != null),
                item => $"{item.name} ( {item.dollarValueCurrent} ){(item.GetComponent<Trap>() ? " ( Trap )" : "")}".Format(),
                item => Settings.c_espItems
            );
        }


        private void DisplayEnemies()
        {
            DisplayObjects(
                GameObjectManager.enemies?.Where(e => e != null),
                enemy => $"{enemy.GetName()}",
                enemy => Settings.c_espEnemies
            );
        }
    }

    public static class ESPExtensions
    {
        public static string Format(this string @string) => @string.Replace("(Clone)", "").Trim();
        public static bool IsLocalPlayer(this PlayerController player) => player != null && player != PlayerController.instance;
    }
}
