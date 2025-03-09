using UnityEngine;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk.Menu.Tab
{
    internal class SettingsTab : MenuTab
    {
        public SettingsTab() : base("Settings") { }
        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            UI.VerticalSpace(ref scrollPos, () =>
            {
                UI.Header("Settings", true);
                UI.Toggle("Debug Mode", ref Settings.b_DebugMode, "Enable", "Disable", UnkMenu.Instance.ToggleDebugTab);
            });
        }
    }
}
