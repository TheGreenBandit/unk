using Unk.Cheats.Core;
using Unk.Util;

namespace Unk.Cheats
{
    internal class InfiniteJump : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled) return;
            PlayerController.instance.Reflect().SetValue("JumpGroundedBuffer", 1f);
        }

        public override void OnDisable() //prevent getting stuck in air
        {
            PlayerController.instance.Reflect().SetValue("JumpGroundedBuffer", 0f);
        }
    }
}
