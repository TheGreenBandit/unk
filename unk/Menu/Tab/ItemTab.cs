﻿using UnityEngine;
using unk.Menu.Core;
using Zorro.Core;
using Random = UnityEngine.Random;
using Photon.Pun;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using unk.Util;

namespace unk.Menu.Tab
{
    internal class ItemTab : MenuTab
    {
        public ItemTab() : base("Item") { }

        private Vector2 scrollPos = Vector2.zero;
        private string searchText = "";
        private bool equipOnSpawn = false;
        public override void Draw()
        {
            GUILayout.BeginVertical();
            
            MenuContent();
            GUILayout.EndVertical();

        }

        private void MenuContent()
        {
            GUILayout.BeginHorizontal();
            
            UI.Textbox("Search", ref searchText);
            GUILayout.FlexibleSpace();
            UI.Checkbox("Equip on Spawn", ref equipOnSpawn);

            GUILayout.EndHorizontal();

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            List<Item> items = ItemDatabase.Instance.Objects.ToList().OrderBy(x => String.IsNullOrEmpty(x.displayName) ? x.name : x.displayName).ToList();

            int gridWidth = 4;
            int btnWidth = (int) (unkMenu.Instance.contentWidth - (unkMenu.Instance.spaceFromLeft*2)) / gridWidth;

            UI.ButtonGrid<Item>(items, item => String.IsNullOrEmpty(item.displayName) ? item.name : item.displayName, searchText, item => GameUtil.SpawnItem(item.id, equipOnSpawn), gridWidth, btnWidth);
            
            GUILayout.EndScrollView();
        }

        
    }
}