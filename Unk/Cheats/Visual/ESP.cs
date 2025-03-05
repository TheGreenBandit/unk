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
        public static bool displayCart = false;
        public static bool displayExtractions = false;
        public static bool displayDeathHeads = false;

        public override void OnGui()
        {
            if (!Cheat.Instance<ESP>().Enabled) return;
            if (displayPlayers) DisplayPlayers();
            if (displayItems) DisplayItems();
            if (displayEnemies) DisplayEnemies();
            if (displayCart) DisplayCart();
            if (displayExtractions) DisplayExtractions();
            if (displayDeathHeads) DisplayDeathHeads();
        }

        public static void ToggleAll()
        {
            displayPlayers = !displayPlayers;
            displayEnemies = !displayEnemies;
            displayItems = !displayItems;
            displayCart = !displayCart;
            displayExtractions = !displayExtractions;
            displayDeathHeads = !displayDeathHeads;
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
                GameObjectManager.players?.Where(p => p != null && !p.IsLocalPlayer() && !p.IsDead()),
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
                GameObjectManager.enemies?.Where(e => e != null && !e.IsDead()),
                enemy => $"{enemy.GetName()}",
                enemy => Settings.c_espEnemies
            );
        }

        private void DisplayDeathHeads()
        {
            DisplayObjects(
                GameObjectManager.deathHeads?.Where(d => d != null && d.playerAvatar != null && d.playerAvatar.IsDead()),
                deathHead => $"{deathHead.playerAvatar.GetName()}'s Death Head",
                deathHead => Settings.c_espEnemies
            );
        }

        private void DisplayExtractions()
        {
            DisplayObjects(
                GameObjectManager.extractions?.Where(e => e != null),
                extraction => "Extraction",
                extraction => Settings.c_espEnemies
            );
        }

        private void DisplayCart()
        {
            DisplayObjects(
                new[] { GameObjectManager.cart },
                cart => $"Cart",
                cart => Settings.c_espCart
            );
        }
    }
}
