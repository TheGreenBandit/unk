using System;
using UnityEngine;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk.Menu.Tab
{
    internal class GeneralTab : MenuTab
    {
        Vector2 scrollPos = Vector2.zero, scrollPos2 = Vector2.zero;
        public GeneralTab() : base("General") { }

        public override void Draw()
        {
            UI.VerticalSpace(ref scrollPos, () =>
            {
                UI.Header(Settings.c_primary.AsString("Welcome to Unk!"), 30);
                GUILayout.Space(20);
                UI.Label("Developed by TGB and some contributions by Corrupt.");
                UI.Label("Version: " + Unk.VERSION);
                GUILayout.Space(20);
                try
                {
                    UI.ScrollView(ref scrollPos2, () =>
                    {
                        foreach (string line in Settings.Changelog.changes)
                        {
                            GUIStyle style = new GUIStyle(GUI.skin.label);

                            if (line.StartsWith("v")) style.fontStyle = FontStyle.Bold;
                            GUILayout.Label(line.StartsWith("v") ? "Changelog " + line : line, style);
                        }
                    });
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            });
        }
    }
}
