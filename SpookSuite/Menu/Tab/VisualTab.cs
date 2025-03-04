using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unk.Cheats.Core;
using Unk.Cheats;
using Unk.Util;
using Unk.Menu.Core;

namespace Unk.Menu.Tab
{
    internal class VisualTab : MenuTab
    {
        public VisualTab() : base("Visual") { }
        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            GUILayout.BeginVertical(GUILayout.Width(UnkMenu.Instance.contentWidth * 0.5f - UnkMenu.Instance.spaceFromLeft));
            VisualContent();
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(UnkMenu.Instance.contentWidth * 0.5f - UnkMenu.Instance.spaceFromLeft));
            ESPContent();
            GUILayout.EndVertical();
        }

        private void VisualContent()
        {
            //fov etc
        }

        private void ESPContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            UI.Checkbox("Enable ESP", Cheat.Instance<ESP>());
            UI.Button("Toggle All ESP", () => ESP.ToggleAll());
            UI.Checkbox("Display Players", ref ESP.displayPlayers);
            UI.Checkbox("Display Enemies", ref ESP.displayEnemies);
            UI.Checkbox("Display Items", ref ESP.displayItems);
            UI.Checkbox("Display Traps", ref ESP.displayTraps);

            GUILayout.EndScrollView();
        }
    }
}
