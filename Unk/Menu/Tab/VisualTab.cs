using UnityEngine;
using Unk.Cheats.Core;
using Unk.Cheats;
using Unk.Util;
using Unk.Menu.Core;
using System.Collections.Generic;

namespace Unk.Menu.Tab
{
    internal class VisualTab : MenuTab
    {
        public VisualTab() : base("Visual") { }
        private Vector2 scrollPos = Vector2.zero; 
        private Vector2 scrollPos2 = Vector2.zero;

        public override void Draw()
        {
            UI.VerticalSpace(ref scrollPos, VisualContent,
                GUILayout.Width(UnkMenu.Instance.contentWidth * 0.5f - UnkMenu.Instance.spaceFromLeft));
            UI.VerticalSpace(ref scrollPos, ESPContent,
                GUILayout.Width(UnkMenu.Instance.contentWidth * 0.5f - UnkMenu.Instance.spaceFromLeft));
        }

        private void VisualContent()
        {
            UI.ScrollView(ref scrollPos, () =>
            {
                UI.Header("Stuff", true);
                UI.Checkbox("Fullbright", Cheat.Instance<Fullbright>());
                if (!PlayerAvatar.instance.Reflect().GetValue<bool>("spectating"))
                    UI.Button("Begin Spectate (wip)", () => PlayerAvatar.instance.SetSpectate());
                else
                    UI.Button("End Spectate", () => { PlayerAvatar.instance.Reflect().SetValue("spectating", false); });
            });
        }

        private void ESPContent()
        {
            UI.ScrollView(ref scrollPos2, () =>
            {
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
            });
        }
    }
}