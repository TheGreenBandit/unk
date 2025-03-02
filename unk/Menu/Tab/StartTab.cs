﻿using unk.Menu.Core;
using UnityEngine;

namespace unk.Menu.Tab
{
    internal class StartTab : MenuTab
    {
        public StartTab() : base("Start") { }
        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            GUILayout.BeginVertical();

            scrollPos = GUILayout.BeginScrollView(scrollPos);
            MenuContent();
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        private void MenuContent()
        {
            float width = unkMenu.Instance.contentWidth - unkMenu.Instance.spaceFromLeft * 2;
            float height = unkMenu.Instance.contentHeight - 60;

            Rect rect = new Rect(0, 0, width, height);
            GUI.Box(rect, "Credits");

            GUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(height));

            GUILayout.Space(25);

            GUILayout.TextArea("IcyRelic (Github, UnknownCheats), TheGreenBandit (Github, GsV2 UnknownCheats)");

            GUILayout.EndVertical();
        } 
    } 
}