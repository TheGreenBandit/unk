using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unk.Cheats.Core;
using Unk.Util;

namespace Unk.Cheats
{
    internal class FlashlightColors : ToggleCheat
    {
        public static string s_color = "";
        public static RGBAColor c_color = new RGBAColor(0, 0, 0, 0);
        private static Color o_color = new Color(0, 0, 0, 0);

        public override void OnEnable()
        {
            if (PlayerAvatar.instance.flashlightController is null)
            {
                Toggle();
                return;
            }
                
            o_color = PlayerAvatar.instance.flashlightController.spotlight.color;
        }
        public override void Update()
        {
            if (!Enabled || PlayerAvatar.instance.flashlightController is null)
                return;

            PlayerAvatar.instance.flashlightController.spotlight.color = new Color(c_color.r, c_color.g, c_color.b, 255);
        }
        public override void OnDisable()
        {
            PlayerAvatar.instance.flashlightController.spotlight.color = o_color;
        }
    }
}
