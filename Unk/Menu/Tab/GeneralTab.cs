using UnityEngine;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk.Menu.Tab
{
    internal class GeneralTab : MenuTab
    {
        Vector2 scrollPos = Vector2.zero;
        public GeneralTab() : base("General") { }

        public override void Draw()
        {
            UI.VerticalSpace(ref scrollPos, () =>
            {
                UI.Header(Settings.c_primary.AsString("Welcome to Unk!"), 30);
                GUILayout.Space(20);
                UI.Label("Developed by TGB & Dustin, receiving constant updates to better the menu!");
                UI.Label("Version: " + Unk.VERSION);
                GUILayout.Space(20);
                //changelog
            });
        }
    }
}
