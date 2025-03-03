using System;
using System.Collections.Generic;
using System.Text;
using Unk.Cheats.Core;
using Unk.Util;

namespace Unk.Cheats.Self
{
    internal class Godmode : ToggleCheat
    {
        public override void Update()
        {
            PlayerAvatar.instance.playerHealth.Reflect().SetValue("godMode", true);
        }

        public override void OnDisable()
        {
            PlayerAvatar.instance.playerHealth.Reflect().SetValue("godMode", false);
        }
    }
}
