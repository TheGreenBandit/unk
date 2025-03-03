using Unk.Cheats.Core;
using Unk.Util;

namespace Unk.Cheats
{
    internal class Godmode : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled) return;

            PlayerAvatar.instance.playerHealth.Reflect().SetValue("godMode", true);
        }

        public override void OnDisable()
        {
            PlayerAvatar.instance.playerHealth.Reflect().SetValue("godMode", false);
        }
    }
}
