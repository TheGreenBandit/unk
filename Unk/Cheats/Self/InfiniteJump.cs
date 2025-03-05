using System;
using System.Collections.Generic;
using System.Text;
using Unk.Cheats.Core;
using Unk.Util;

namespace Unk.Cheats
{
    internal class InfiniteJump : ToggleCheat
    {
        public override void Update()
        {
            PlayerController.instance.Reflect().SetValue("JumpCooldown", 0);
        }
    }
}
