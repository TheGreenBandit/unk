using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unk.Cheats.Core;
using Unk.Cheats;
using Unk.Menu.Core;
using Unk.Util;

namespace Unk.Menu.Tab
{
    internal class SelfTab : MenuTab
    {
        public SelfTab() : base("Self") { }
        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            UI.VerticalSpace(() =>
            {
                UI.Checkbox("Unlimited Energy", Cheat.Instance<UnlimitedStamina>());
                UI.Checkbox("Godmode", Cheat.Instance<Godmode>());
                UI.Checkbox("No Tumble", Cheat.Instance<NoTumble>());
                UI.Checkbox("Infinite Jump", Cheat.Instance<InfiniteJump>());
                UI.Checkbox("Invisibility", Cheat.Instance<Invisibility>());
                UI.Textbox("Spoofed Name", ref NameSpoofer.Value, true, 100);
                UI.Checkbox("Use Spoofed Name", Cheat.Instance<NameSpoofer>());
                UI.CheatToggleSlider(Cheat.Instance<NoClip>(), "No Clip", NoClip.Value.ToString("#"), ref NoClip.Value, 1f, 20f);
                UI.CheatToggleSlider(Cheat.Instance<SuperSpeed>(), "Super Speed", SuperSpeed.Value.ToString("#"), ref SuperSpeed.Value, 1f, 100f);
            });
        }
    }
}
