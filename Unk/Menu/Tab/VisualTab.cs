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
        private Vector2 scrollPos2 = Vector2.zero;

        public override void Draw()
        {
            GUILayout.BeginVertical(GUILayout.Width(UnkMenu.Instance.contentWidth * 0.5f - UnkMenu.Instance.spaceFromLeft));
            ESPContent();
            GUILayout.EndVertical();
        }

        private void ESPContent()
        {
            scrollPos2 = GUILayout.BeginScrollView(scrollPos2);

            UI.Header("ESP", true);
            UI.Checkbox("Enable ESP", Cheat.Instance<ESP>());
            UI.Button("Toggle All ESP", ESP.ToggleAll);
            UI.Checkbox("Display Players", ref ESP.displayPlayers);
            UI.Checkbox("Display Enemies", ref ESP.displayEnemies);
            UI.Checkbox("Display Items", ref ESP.displayItems);
            UI.Checkbox("Display Cart", ref ESP.displayCarts);
            UI.Checkbox("Display Extractions", ref ESP.displayExtractions);
            UI.Checkbox("Display Death Heads", ref ESP.displayDeathHeads);
            UI.Checkbox("Display Truck", ref ESP.displayTruck);

            GUILayout.EndScrollView();
        }
    }
}