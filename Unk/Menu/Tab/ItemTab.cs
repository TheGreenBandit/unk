﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk.Menu.Tab
{
    internal class ItemTab : MenuTab
    {
        public ItemTab() : base("Item") { }
        private Vector2 scrollPos = Vector2.zero;
        private string searchText = "";
        private int amount = 1;

        private bool equipOnSpawn = false;

        public override void Draw()
        {
            UI.VerticalSpace(() =>
            {
                UI.Label("This is Host/Singleplayer only currently.\n As Nonhost you can spawn objects however they cannot be interacted with.");

                UI.HorizontalSpace("Options", () =>
                {
                    UI.Textbox("Search", ref searchText);
                    GUILayout.FlexibleSpace();
                    UI.Checkbox("Equip on Spawn", ref equipOnSpawn);
                    GUILayout.FlexibleSpace();
                    UI.Textbox<int>("Amount:", ref amount, false);
                });

                UI.ScrollView(ref scrollPos, () =>
                {
                    List<Item> items = StatsManager.instance.itemDictionary.OrderBy(x => x.Key).Select(x => x.Value).ToList();

                    int gridWidth = 4;
                    int btnWidth = (int)(UnkMenu.Instance.contentWidth - (UnkMenu.Instance.spaceFromLeft * 2)) / gridWidth;
                    UI.ButtonGrid<Item>(items, item => item.itemName, searchText, item => ItemUtil.SpawnItem(item.name, ItemUtil.GetSpawnPos(), amount, equipOnSpawn), gridWidth, btnWidth);
                });
            });
        }

        
    }
}
