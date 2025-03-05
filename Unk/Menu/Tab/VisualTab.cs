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
            UI.CheatToggleSlider(Cheat.Instance<FOV>(), "FOV", Cheats.FOV.Value.ToString(), ref FOV.Value, 1, 140);
        }

        private void ESPContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            UI.Checkbox("Toggle lights", Cheat.Instance<LightsOn>());
            UI.Checkbox("Enable ESP", Cheat.Instance<ESP>());
            UI.Button("Toggle All ESP", ESP.ToggleAll);
            UI.Checkbox("Display Players", ref ESP.displayPlayers);
            UI.Checkbox("Display Enemies", ref ESP.displayEnemies);
            UI.Checkbox("Display Items", ref ESP.displayItems);
            UI.Checkbox("Display Cart", ref ESP.displayCart);
            UI.Checkbox("Display Extractions", ref ESP.displayExtractions);
            UI.Checkbox("Display Death Heads", ref ESP.displayDeathHeads);

            GUILayout.EndScrollView();
        }
    }
}