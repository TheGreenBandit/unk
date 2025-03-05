using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk.Menu.Tab
{
    internal class StartTab : MenuTab
    {
        Vector2 scrollPos = Vector2.zero;
        public StartTab() : base("Start") { }

        public override void Draw()
        {
            UI.VerticalSpace(() =>
            {
                UI.Header(Settings.c_primary.AsString("Welcome to Unk!"), 30);
                GUILayout.Space(20);
                UI.Label("Developed by TGB & Dustin, receiving constant updates to better the menu!");
                GUILayout.Space(20);
                //changelog
            });
        }
    }
}
